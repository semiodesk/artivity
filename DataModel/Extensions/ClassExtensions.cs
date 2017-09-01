using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel.Extensions
{
    public static class ClassExtensions
    {
        public static bool IsInstanceOf(this Uri resource, IModel model, Class type)
        {
            ISparqlQuery query = new SparqlQuery(@"
                ASK FROM art: WHERE { @resource a @type . }
            ");

            query.Bind("@type", type);
            query.Bind("@resource", resource);

            // NOTE: Execute the query with inferencing enabled so that the 
            // query evaluates the class inheritance in the ontology.
            return model.ExecuteQuery(query, true).GetAnwser();
        }

        public static bool IsSuperClassOf(this Class superType, IModel model, Uri subType)
        {
            if (subType == superType.Uri)
            {
                return true;
            }
            else
            {
                ISparqlQuery query = new SparqlQuery(@"
                    ASK FROM art: FROM nfo: WHERE { @subType rdfs:subClassOf+ @superType . }
                ");

                query.Bind("@subType", subType);
                query.Bind("@superType", superType);

                // NOTE: Execute the query with inferencing enabled so that the 
                // query evaluates the class inheritance in the ontology.
                return model.ExecuteQuery(query, true).GetAnwser();
            }
        }
    }
}
