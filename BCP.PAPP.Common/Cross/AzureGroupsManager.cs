using BCP.CVT.Cross;
using BCP.PAPP.Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
 
namespace BCP.PAPP.Common.Cross
{
    public class GraphUserMetadata
    {
        public string displayName { get; set; }
        public string id { get; set; }
        public string mail { get; set; }
        public string onPremisesSamAccountName { get; set; }
        public string userPrincipalName { get; set; }

        public string matricula
        {

            get
            {
                return string.IsNullOrEmpty(this.onPremisesSamAccountName) ? this.userPrincipalName : this.onPremisesSamAccountName;
            }
        }
    }
    public class GraphGroupMetadata
    {
        public string displayName { get; set; }
        public string id { get; set; }
        public string mail { get; set; }
    }

    public class AzureGroupsManager
    {
        public GraphGroupMetadata GetGroupByName(string group_name)
        {
            //https://graph.microsoft.com/v1.0/groups?$select=displayName,id,mail&$filter=displayName eq 'Crosland'
            var search_text = string.Format("?$select=displayName,id,mail&$filter=displayName eq '{0}'", group_name);

            var URL_API = string.Concat(Settings.Get<string>("Graph.API.Groups"), search_text);            
            var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));


            RestClient client = new RestClient(new Uri(URL_API));
            IRestRequest request = new RestRequest(Method.GET);

            if (!validarSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
            }
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            client.Timeout = -1;

            var token = this.GetGraphAuthToken();            

            request.AddHeader("Authorization", "Bearer " + token);

            var response = client.Execute(request);

            GraphGroupMetadata objGrupo = null;
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    var lista = JsonConvert.DeserializeObject<List<GraphGroupMetadata>>(result.SelectToken("value").ToString());
                    objGrupo = lista.FirstOrDefault();
                }                
            }

            return objGrupo;
        }

        public List<GraphUserMetadata> GetGroupMembersByName(string group_name)
        {
            ///5ae1d20a-0f94-4b78-a8e5-f5697ca62835/members?$select=displayName,givenName,id,mail,onPremisesSamAccountName
            var objGroup = GetGroupByName(group_name);
            if (objGroup != null)
            {
                var select_text = "?$select=displayName,givenName,id,mail,onPremisesSamAccountName,userPrincipalName";

                var URL_API = string.Concat(Settings.Get<string>("Graph.API.Groups"), string.Format("/{0}/members", objGroup.id), select_text);//, "&$top = 300");                

                var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));


                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                if (!validarSSL)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
                }
                ServicePointManager.UseNagleAlgorithm = false;
                ServicePointManager.Expect100Continue = false;

                client.Timeout = -1;

                var token = this.GetGraphAuthToken();                

                request.AddHeader("Authorization", "Bearer " + token);

                var response = client.Execute(request);

                List<GraphUserMetadata> lista = null;
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        lista = JsonConvert.DeserializeObject<List<GraphUserMetadata>>(result.SelectToken("value").ToString());
                        lista = lista.Where(x => !string.IsNullOrEmpty(x.mail)).ToList();

                        string next = "";

                        if (result.Count==3)
                        {
                            next = result["@odata.nextLink"].ToString();
                        }
                        else
                        {
                            next = "";
                        }

                        while (next != "")
                        {
                            RestClient client1 = new RestClient(new Uri(next));
                            IRestRequest request1 = new RestRequest(Method.GET);
                            if (!validarSSL)
                            {
                                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
                            }
                            ServicePointManager.UseNagleAlgorithm = false;
                            ServicePointManager.Expect100Continue = false;
                            client1.Timeout = -1;
                            var token1 = this.GetGraphAuthToken();
                            request1.AddHeader("Authorization", "Bearer " + token1);
                            var response1 = client1.Execute(request1);
                            List<GraphUserMetadata> lista1 = null;
                            if (response1 != null)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var jsonRetorno1 = response1.Content;
                                    JObject result1 = JObject.Parse(jsonRetorno1);

                                    lista1 = JsonConvert.DeserializeObject<List<GraphUserMetadata>>(result1.SelectToken("value").ToString());
                                    lista1 = lista1.Where(x => !string.IsNullOrEmpty(x.mail)).ToList();
                                    lista.AddRange(lista1);

                                    if (result1.Count == 3)
                                    {
                                        next = result1["@odata.nextLink"].ToString();
                                    }
                                    else
                                    {
                                        next = "";
                                    }
                                }
                            }
                         }
                    }
                }
                return lista;
            }
            return null;
        }

        public string GetGraphAuthToken()
        {
            var azureAppTenantId = Settings.Get<string>("Azure.App.Tenant");            
            var azureAppId = Settings.Get<string>("Azure.App.Id");            
            var azureAppSecret = Constantes.AzureAppSecret;            

            string URL_API = Settings.Get<string>("Graph.API.Token");
            URL_API = string.Format(URL_API, azureAppTenantId);
            var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));

            RestClient client = new RestClient(new Uri(URL_API));
            IRestRequest request = new RestRequest(Method.POST);

            if (!validarSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
            }
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            client.Timeout = -1;

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("scope", "https://graph.microsoft.com/.default");
            request.AddParameter("client_id", azureAppId);
            request.AddParameter("client_secret", azureAppSecret);
            request.AddParameter("grant_type", "client_credentials");

            var response = client.Execute(request);

            string access_token = null;

            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    access_token = result.SelectToken("access_token").ToString();
                    Console.WriteLine("access_token: " + access_token);
                }                
            }

            return access_token;
        }

        public AzureUserDto GetUserDataByMail(string usermail)
        {
            if (!string.IsNullOrWhiteSpace(usermail))
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), usermail);
                URL_API = URL_API + "?$select=displayName,id,mail,userPrincipalName,onPremisesSamAccountName";

                var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));


                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                if (!validarSSL)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
                }
                ServicePointManager.UseNagleAlgorithm = false;
                ServicePointManager.Expect100Continue = false;

                client.Timeout = -1;

                var token = this.GetGraphAuthToken();

                request.AddHeader("Authorization", "Bearer " + token);

                var response = client.Execute(request);

                AzureUserDto objUsuario = null;

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var accountName = string.Empty;
                        var accountObject = result.SelectToken("onPremisesSamAccountName");
                        if (accountObject != null)
                        {
                            accountName = accountObject.ToString();
                        }
                        else
                        {
                            accountObject = result.SelectToken("userPrincipalName");
                            if (accountObject != null)
                            {
                                accountName = accountObject.ToString();
                            }
                            else
                                accountName = null;
                        }

                        objUsuario = new AzureUserDto
                        {
                            Matricula = accountName,
                            Nombres = result.SelectToken("displayName").ToString(),
                            CorreoElectronico = result.SelectToken("mail").ToString()
                        };

                    }
                }

                return objUsuario;
            }
            else
                return new AzureUserDto();
        }
    }
}
