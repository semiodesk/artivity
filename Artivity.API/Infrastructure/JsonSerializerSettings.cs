using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid
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
