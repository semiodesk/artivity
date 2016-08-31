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
