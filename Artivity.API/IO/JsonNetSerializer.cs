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
    using System.Collections.Generic;
    using System.IO;
    using Nancy.IO;
    using Newtonsoft.Json;

    public class JsonNetSerializer : ISerializer
    {
        private readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializer"/> class.
        /// </summary>
        public JsonNetSerializer()
        {
            this.serializer = JsonSerializer.CreateDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializer"/> class,
        /// with the provided <paramref name="serializer"/>.
        /// </summary>
        /// <param name="serializer">Json converters used when serializing.</param>
        public JsonNetSerializer(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        /// <summary>
        /// Whether the serializer can serialize the content type
        /// </summary>
        /// <param name="contentType">Content type to serialise</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool CanSerialize(string contentType)
        {
            return Helpers.IsJsonType(contentType);
        }

        /// <summary>
        /// Gets the list of extensions that the serializer can handle.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of extensions if any are available, otherwise an empty enumerable.</value>
        public IEnumerable<string> Extensions
        {
            get { yield return "json"; }
        }

        /// <summary>
        /// Serialize the given model with the given contentType
        /// </summary>
        /// <param name="contentType">Content type to serialize into</param>
        /// <param name="model">Model to serialize</param>
        /// <param name="outputStream">Output stream to serialize to</param>
        /// <returns>Serialised object</returns>
        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            using (var writer = new JsonTextWriter(new StreamWriter(new UnclosableStreamWrapper(outputStream))))
            {
                this.serializer.Serialize(writer, model);               
            }
        }
    }
}