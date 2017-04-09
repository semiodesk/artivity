using Artivity.Api.Platform;
using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Platform
{
    public class OwnerProvider
    {
        #region Members
        public Uri UserUri { get; set; }
        public Association UserAssociation { get; set; }
        protected IModelProvider ModelProvider { get; set; }
        protected IPlatformProvider PlatformProvider { get; set; }
        #endregion

        #region Constructor
        public OwnerProvider(IModelProvider modelProvider, IPlatformProvider platformProvider)
        {
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;
            UserUri = new Uri(PlatformProvider.Config.Uid);
            LoadUserAssociation();
        }
        #endregion

        #region Methods
        public void LoadUserAssociation()
        {

             ISparqlQuery query = new SparqlQuery(@"DESCRIBE
                            ?association
                            WHERE
                            {
                                ?association prov:agent @user .
                            }");
             query.Bind("@user", UserUri);
            var res = ModelProvider.GetAgents().GetResources<Association>(query);
            if( res.Any())
                UserAssociation = res.First();

        }
        #endregion
    }
}
