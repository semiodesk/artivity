using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    public class ResourceResolver : DefaultContractResolver
    {
        public Type[] IngoreTypes { get; set; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Artivity.DataModel.Entity) && property.PropertyName == "RevisionUris")
            {
                property.Ignored = true;
            }

            if (property.DeclaringType == typeof(Resource) && property.PropertyName == "Model")
            {
                property.Ignored = true;
                property.Readable = false;
                property.ShouldDeserialize = instanceOfProblematic => false;
            }

            if (property.DeclaringType == typeof(IModel) && property.PropertyName == "Uri")
            {
                property.Ignored = true;
                property.Readable = false;
                property.ShouldDeserialize = instanceOfProblematic => false;
            }

            return property;
        }
    }
}
