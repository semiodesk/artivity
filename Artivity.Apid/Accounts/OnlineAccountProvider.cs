using Artivity.DataModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public abstract class OnlineAccountProvider : IOnlineAccountProvider
    {
        #region Members

        [JsonIgnore]
        public IModel Model { get; set; }

        public string Id { get; protected set; }

        public string Url { get; protected set; }

        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public string InstallUrl { get; protected set; }

        public string Status { get; protected set; }

        #endregion

        #region Constructors

        public OnlineAccountProvider(string id)
        {
            Id = id;
        }

        #endregion

        #region Methods

        public OnlineAccount TryCreateAccount()
        {
            if(Model == null)
            {
                LogError("Cannot create account because model is not set.");
                return null;
            }

            OnlineAccount account = OnCreateAccount();

            if (account == null)
            {
                return null;
            }

            // Check if there is already an account with the given ID.
            ISparqlQuery query = new SparqlQuery(@"ask where { ?account foaf:accountName @id }");
            query.Bind("@id", account.Id);

            ISparqlQueryResult result = Model.ExecuteQuery(query);

            if (result.GetAnwser())
            {
                // We do not need to install the account twice.
                LogError("There is already an account with id {0}", account.Id);
                return null;
            }

            Person user = Model.GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                LogError("Unable to retrieve user agent.");
                return null;
            }

            if(!account.IsSynchronized)
            {
                account.Commit();
            }

            user.Accounts.Add(account);
            user.Commit();

            Logger.LogInfo("Installed account with id: {0}", account.Id);

            Status = "Account installed.";

            return account;
        }
        
        protected abstract OnlineAccount OnCreateAccount();

        protected void LogError(string message, params object[] p)
        {
            Logger.LogError(message, p);

            Status = "Error";
        }

        #endregion
    }
}
