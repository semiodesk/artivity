/*
The MIT License

Copyright (c) 2010 Andreas Håkansson, Steven Robbins and contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace Nancy.Serialization.JsonNet
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Nancy.Extensions;
    using Nancy.ModelBinding;
    using Newtonsoft.Json;

    public class JsonNetBodyDeserializer : IBodyDeserializer
    {
        private readonly JsonSerializer serializer;

        /// <summary>
        /// Empty constructor if no converters are needed
        /// </summary>
        public JsonNetBodyDeserializer()
        {
            this.serializer = JsonSerializer.CreateDefault();
        }

        /// <summary>
        /// Constructor to use when a custom serializer are needed.
        /// </summary>
        /// <param name="serializer">Json serializer used when deserializing.</param>
        public JsonNetBodyDeserializer(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        /// <summary>
        /// Whether the deserializer can deserialize the content type
        /// </summary>
        /// <param name="contentType">Content type to deserialize</param>
        /// <param name="context">Current <see cref="BindingContext"/>.</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool CanDeserialize(string contentType, BindingContext context)
        {
            return Helpers.IsJsonType(contentType);
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        /// <param name="contentType">Content type to deserialize</param>
        /// <param name="bodyStream">Request body stream</param>
        /// <param name="context">Current context</param>
        /// <returns>Model instance</returns>
        public object Deserialize(string contentType, Stream bodyStream, BindingContext context)
        {
            var deserializedObject = 
                this.serializer.Deserialize(new StreamReader(bodyStream), context.DestinationType);

            var properties = 
                context.DestinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new BindingMemberInfo(p));

            var fields = 
                context.DestinationType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f => new BindingMemberInfo(f));

            if (properties.Concat(fields).Except(context.ValidModelBindingMembers).Any())
            {
                return CreateObjectWithBlacklistExcluded(context, deserializedObject);
            }

            return deserializedObject;
        }

        private static object ConvertCollection(object items, Type destinationType, BindingContext context)
        {
            var returnCollection = Activator.CreateInstance(destinationType);

            var collectionAddMethod =
                destinationType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);

            foreach (var item in (IEnumerable)items)
            {
                collectionAddMethod.Invoke(returnCollection, new[] { item });
            }

            return returnCollection;
        }

        private static object CreateObjectWithBlacklistExcluded(BindingContext context, object deserializedObject)
        {
            var returnObject = Activator.CreateInstance(context.DestinationType, true);

            if (context.DestinationType.IsCollection())
            {
                return ConvertCollection(deserializedObject, context.DestinationType, context);
            }

            foreach (var property in context.ValidModelBindingMembers)
            {
                CopyPropertyValue(property, deserializedObject, returnObject);
            }

            return returnObject;
        }

        private static void CopyPropertyValue(BindingMemberInfo property, object sourceObject, object destinationObject)
        {
            property.SetValue(destinationObject, property.GetValue(sourceObject));
        }
    }
}