using Artivity.DataModel;
using Nancy;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;

namespace Artivity.Apid.Modules.Accounts
{
    public class OrcidAccountModule : ModuleBase
    {
        public OrcidAccountModule(IModelProvider provider)
            : base("/artivity/api/1.0/agents/user/accounts/orcid", provider)
        {
            Get["/"] = parameters =>
            {
                OrcidAccount result = ModelProvider.AgentsModel.GetResources<OrcidAccount>().FirstOrDefault();

                if (result != null)
                {
                    return Response.AsJson(result);
                }
                else
                {
                    return null;
                }
            };

            Get["/install"] = parameters =>
            {
                string code = Request.Query["code"];

                if (string.IsNullOrEmpty(code))
                {
                    Logger.LogError("Cannot install ORCID account without authorization code.");
                    return HttpStatusCode.BadRequest;
                }

                Uri sourceUrl = new Uri(Request.Url.ToString());
                Uri targetUrl = new Uri("https://pub.sandbox.orcid.org/oauth/token");

                var values = new System.Collections.Specialized.NameValueCollection()
                {
                   { "client_id", "APP-AF3XVP6X01AMZQH1" },
                   { "client_secret", "0c883177-cfd8-4a0f-a111-03a5ba19539d" },
                   { "grant_type", "authorization_code" },
                   { "code", code },
                   { "redirect_uri", sourceUrl.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.Unescaped) }
                };

                var client = new System.Net.WebClient();
                client.UploadValuesCompleted += OnOrcidReponseReceived;
                client.UploadValuesAsync(targetUrl, "POST", values);

                return HttpStatusCode.OK;
            };

            Get["/uninstall"] = parameters =>
            {
                Person user = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();
                
                if(user == null)
                {
                    Logger.LogError("Unable to retrieve user agent.");
                    return HttpStatusCode.InternalServerError;
                }

                // TODO: Remove magic string.
                OnlineAccount account = user.Accounts.FirstOrDefault(a => a.ServiceUrl == "http://orcid.org");

                if (account == null)
                {
                    Logger.LogInfo("Cannot uninstall ORCID account because none was found.");
                    return HttpStatusCode.BadRequest;
                }

                ModelProvider.AgentsModel.DeleteResource(account);

                user.Accounts.Remove(account);
                user.Commit();

                Logger.LogInfo("Uninstalled ORCID account: {0}/{1}", account.ServiceUrl, account.Id);

                return HttpStatusCode.OK;
            };
        }

        private void OnOrcidReponseReceived(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string json = Encoding.UTF8.GetString(e.Result);

                JObject response = JObject.Parse(json);

                string name = response["name"].ToString();
                string orcid = response["orcid"].ToString();

                Logger.LogInfo("OAuth Response: {0} {1}", name, orcid);

                // Check if there is already an account with the given ID.
                ISparqlQuery query = new SparqlQuery(@"ask where { ?account foaf:accountName @id }");
                query.Bind("@id", orcid);

                ISparqlQueryResult result = ModelProvider.AgentsModel.ExecuteQuery(query);

                if(result.GetAnwser())
                {
                    // We do not need to install the account twice.
                    Logger.LogInfo("There is already an ORCID account installed.");
                    return;
                }

                Person user = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

                if (user == null)
                {
                    Logger.LogError("Unable to retrieve user agent.");
                    return;
                }

                OrcidAccount account = ModelProvider.AgentsModel.CreateResource<OrcidAccount>();
                account.Id = orcid;
                account.Description = string.Format("{0}/{1}", account.ServiceUrl, account.Id);
                account.Commit();

                user.Accounts.Add(account);
                user.Commit();

                Logger.LogInfo("Added ORCID account: {0}", account.Description);
            }
            else
            {
                Logger.LogError(e.Error.Message);
            }
        }
    }
}
