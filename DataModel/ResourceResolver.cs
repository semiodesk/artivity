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

            switch (property.PropertyName)
            {
                case "RevisionUris":
                {
                    if(property.DeclaringType == typeof(Entity))
                    {
                        property.Ignored = true;
                    }
                    break;
                }
                case "Uri":
                {
                    if (property.DeclaringType == typeof(IModel))
                    {
                        property.Ignored = true;
                        property.Readable = false;
                        property.ShouldDeserialize = instanceOfProblematic => false;
                    }
                    break;
                }
                case "Model":
                {
                    if(property.DeclaringType == typeof(Resource))
                    {
                        property.Ignored = true;
                        property.Readable = false;
                        property.ShouldDeserialize = instanceOfProblematic => false;
                    }

                    break;
                }
            }

            return property;
        }
    }
}
