using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel.Provider
{
    public class CommentProvider
    {
        #region Members
        protected ModelProvider ModelProvider;
        #endregion

        #region Constructor
        public CommentProvider(IModelProvider modelProvider)
        {
            ModelProvider = modelProvider;
        }
        #endregion

        #region Methods

        private CommentParameter GetComment(UriRef commentUri)
        {
            IModel model = ModelProvider.GetAll();

            if (model == null)
            {
                return null;
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
	                ?type
	                ?uri
                    ?agent
                    ?agentId
	                ?message
	                ?primarySource
	                ?startTime
	                ?endTime
	                CONCAT('[', STR(GROUP_CONCAT(DISTINCT ?association_; SEPARATOR=',')),']') AS ?associations
                WHERE
                {
	                ?activity prov:generated @uri ;
		                prov:wasStartedBy ?agent ;
		                prov:startedAtTime ?startTime ;
		                prov:endedAtTime ?endTime .

                    ?agent dces:identifier ?agentId .

	                @uri a ?type_ ;
		                prov:hadPrimarySource ?primarySource;
		                nie:created ?time ;
		                art:deleted @undefined;
		                sioc:content ?message .
	
	                BIND(@uri AS ?uri)
	                BIND(STRAFTER(STR(?type_) , STR(art:)) AS ?type)
	
	                OPTIONAL
	                {
		                ?activity prov:qualifiedAssociation [ prov:agent ?a ; prov:hadRole ?r ] .
	
		                BIND(CONCAT('{{agent: \'', STR(?a),'\', role: \'', STR(?r), '\'}}') AS ?association_)
	                }
                }
                GROUP BY ?type ?uri ?agent ?agentId ?message ?primarySource ?startTime ?endTime
                ORDER BY DESC (?endTime)
            ");

            query.Bind("@uri", commentUri);
            query.Bind("@undefined", DateTime.MinValue);

            List<BindingSet> result = model.GetBindings(query).ToList();

            if (result.Count == 0)
                return null;

            CommentParameter param = new CommentParameter();
            foreach (BindingSet b in result)
            {
                string json = b["associations"].ToString();

                b["associations"] = JsonConvert.DeserializeObject(json);
            }

            var first = result[0];

            param.agent = first["agent"].ToString();
            param.endTime = (DateTime)first["endTime"];
            param.startTime = (DateTime)first["startTime"];
            param.message = first["message"] as string;
            param.primarySource = first["primarySource"].ToString();
            param.type = (CommentTypes)Enum.Parse(typeof(CommentTypes), first["type"] as string);

            //param.associations = JsonConvert.SerializeObject(first["associations"]), 

            //param.marks = JsonConvert.SerializeObject(first["marks"]), 



            return param;
        }

        #endregion
    }
}
