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
    public class AccountOwnerProvider
    {
        #region Members

        protected IModelProvider ModelProvider { get; set; }

        protected IPlatformProvider PlatformProvider { get; set; }

        public Uri UserUri { get; set; }

        public Association UserAssociation { get; set; }

        #endregion

        #region Constructor

        public AccountOwnerProvider(IModelProvider modelProvider, IPlatformProvider platformProvider)
        {
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;

            UserUri = new Uri(PlatformProvider.Config.Uid);

            LoadUserAssociation();
        }

        #endregion

        #region Methods

        private void LoadUserAssociation()
        {
             ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE ?association WHERE
                {
                    ?association prov:agent @user .
                }");

            query.Bind("@user", UserUri);

            IEnumerable<Association> result = ModelProvider.GetAgents().GetResources<Association>(query);

            if (result.Any())
            {
                UserAssociation = result.First();
            }
        }

        #endregion
    }
}
