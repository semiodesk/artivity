// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;

namespace Artivity.Api.IO
{
    public class CustomJsonResourceSerializerSettings : JsonSerializerSettings
    {
        public CustomJsonResourceSerializerSettings(IStore store)
        {
            // Allow to use the private parameterless constructor of the Resource class.
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

            //ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // A custom conveter for loading the URI and setting the model.
            Converters = new JsonConverter[] { new CustomJsonResourceConverter(store) };
        }
    }

    public class CustomJsonResourceConverter : JsonConverter
    {
        #region Members

        private IStore _store;

        #endregion

        #region Constructors

        public CustomJsonResourceConverter(IStore store)
        {
            _store = store;
        }

        #endregion

        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return typeof(Resource).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject resourceJson = JObject.Load(reader);
            JObject modelJson = resourceJson["Model"].Value<JObject>();

            if (modelJson == null)
            {
                return null;
            }

            Uri modelUri = new Uri(modelJson["Uri"].Value<string>());
            Uri resourceUri = new Uri(resourceJson["Uri"].Value<string>());

            IModel model = _store.GetModel(modelUri);

            Resource resource;
            if (!model.ContainsResource(resourceUri))
            {
                resource = (Resource)model.CreateResource(resourceUri, objectType);
            }
            else
            {
                resource = (Resource)model.GetResource(resourceUri, objectType);
            }

            // Do not let the serializer set the model or URI..
            resourceJson.Remove("Model");
            resourceJson.Remove("Uri");

            // Load all the other properties from the JSON.
            serializer.Populate(resourceJson.CreateReader(), resource);

            return resource;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
