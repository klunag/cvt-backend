using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.Identity;
using BCP.CVT.Cross.Models;

namespace BCP.CVT.Cross
{
    public class AzureUtilitarios
    {
        public string AccessToken { get; set; }
        public List<ResourceModel> ResourceModelList { get; set; }
        public List<VirtualMachineModel> VirtualMachineList { get; set; }

        public AzureUtilitarios()
        {
            AccessToken = string.Empty;
            ResourceModelList = new List<ResourceModel>();
            VirtualMachineList = new List<VirtualMachineModel>();
        }
        
        public static string GetSecret(string secret, out bool result)
        {
            result = true;
            try
            {
                var azureCredentialOptions = new DefaultAzureCredentialOptions();                

                Console.WriteLine(Settings.Get<string>("Azure.KeyVault.Url"));
                Console.WriteLine("Secreto a obtener: " + secret);

                var secretClient = new SecretClient(new Uri(Settings.Get<string>("Azure.KeyVault.Url")), new DefaultAzureCredential(azureCredentialOptions));
                var secretRpta = secretClient.GetSecret(secret);
                return secretRpta.Value.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR GET SECRET");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);

                //Si hay un error, obtener el valor del web.config

                result = true;
                var configuracion = Settings.Get<int>("Configuracion.Id");
                if (secret == Settings.Get<string>("Azure.KeyVault.Secret.SecretAPP"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "6DR10CaDv2bfsD@moUdMz/]AmnPYXBO:";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.ServerSQL"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "tcp:msqleu2e195d01.database.windows.net,1433";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.BDSQL"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "MSQLEU2E195DINT01";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.UserSQL"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "exitoperu";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.PwdSQL"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "9KSHaku1CzHKvDAVufQJ";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.StorageName"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "staceu2e195d01";
                    else
                        return string.Empty;
                }
                else if (secret == Settings.Get<string>("Azure.KeyVault.Secret.StorageKey"))
                {
                    if (configuracion == 1) //Desa ITS
                        return "hQWZldF8glkbgpkRjBDNwKwojVZyF18UxfNCw23NJmupa7IjoOC/DDtsLb6F3WduU6sBMrME9KQ+fCIRqsaaBA==";
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;

            }
        }        

        public string GetAuthToken()
        {

            var azureAppTenantId = Settings.Get<string>("Azure.App.TenantId");
            var azureAppClientId = Settings.Get<string>("Azure.App.ClientId");
            var azureAppResource = Settings.Get<string>("Azure.App.Resource");
            var azureAppClientSecret = Constantes.AzureAppSecret;

            string URL_API = Settings.Get<string>("Graph.API.Token");
            URL_API = string.Format(URL_API, azureAppTenantId);

            RestClient client = new RestClient(new Uri(URL_API));
            IRestRequest request = new RestRequest(Method.POST);

            client.Timeout = -1;

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("resource", azureAppResource);
            request.AddParameter("client_id", azureAppClientId);
            request.AddParameter("client_secret", azureAppClientSecret);
            request.AddParameter("grant_type", "client_credentials");

            var response = client.Execute(request);

            string access_token = null;
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    //access_token = result.SelectToken("access_token").ToString();
                    var responseToken = result.SelectToken("access_token");
                    if (responseToken != null)
                    {
                        AccessToken = responseToken.ToString();
                        Console.WriteLine("Token generado: {0}", AccessToken);
                    }
                }
                else
                {
                    Console.WriteLine("Error interno al obtener el token: {0}", response.ErrorMessage);
                    //log.DebugFormat("No se pudo procesar la URL {0}", url);
                }
            }

            return access_token;
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
                }
                else
                {
                    //log.DebugFormat("No se pudo procesar la URL {0}", url);
                }
            }

            return access_token;
        }

        //public string GetGraphAuthToken()
        //{
        //    try
        //    {
        //        var credential = new DefaultAzureCredential();
        //        var token = credential.GetToken(
        //            new Azure.Core.TokenRequestContext(
        //                new[] { "https://graph.microsoft.com/.default" }));

        //        Console.WriteLine("access_token _ default credentials: " + token.Token);
        //        return token.Token;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ex: " + ex.Message);
        //        return "ERROR";
        //    }
        //}


        public List<SubscriptionModel> GetAllSubscriptions()
        {
            try
            {
                var URL_API = Settings.Get<string>("Graph.API.Subscriptions");
                URL_API = $"{URL_API}?api-version=2020-01-01";

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //AccessToken = GetAuthToken();
                GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<SubscriptionModel> lstSubs = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var lstSubscriptions = result.SelectToken("value").ToObject(typeof(List<SubscriptionModel>));
                        lstSubs = lstSubscriptions as List<SubscriptionModel>;

                        Console.WriteLine("Total de suscripciones cargadas: {0}", lstSubs.Count);
                    }
                    else
                    {
                        Console.WriteLine("Error interno en la carga de suscripciones: {0}", response.ErrorMessage);
                    }
                }

                return lstSubs;

            }
            catch (Exception e)
            {
                Console.WriteLine("Excepción al cargar las suscripciones: {0} ", e.Message);
                return null;
            }
        }

        public List<ResourceModel> GetAllResourcesBySubscription(string subscriptionId, bool isNextLink = false, string nextUrl = null)
        {
            try
            {
                var URL_API = string.Empty;
                if (isNextLink)
                    URL_API = nextUrl;
                else
                {
                    URL_API = string.Format(Settings.Get<string>("Graph.API.Resources"), subscriptionId);
                    URL_API = $"{URL_API}?api-version=2020-06-01&$expand=createdTime,changedTime,provisioningState,tags";
                }

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //var token = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<ResourceModel> lstRsrc = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var lstResources = result.SelectToken("value").ToObject(typeof(List<ResourceModel>));
                        lstRsrc = lstResources as List<ResourceModel>;
                        if (lstRsrc != null) ResourceModelList.AddRange(lstRsrc);

                        //next Link
                        var nextLinkObj = result.SelectToken("nextLink");
                        if (nextLinkObj != null) GetAllResourcesBySubscription("", true, nextLinkObj.ToString());
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var newToken = GetAuthToken();
                        GetAllResourcesBySubscription(subscriptionId);
                    }
                }

                return lstRsrc;
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public List<VirtualMachineModel> GetAllVirtualMachinesBySubscription(string subscriptionId, string nextLink = null)
        {
            try
            {
                var URL_API = string.Empty;
                if (string.IsNullOrEmpty(nextLink))
                {
                    URL_API = string.Format(Settings.Get<string>("Graph.API.VirtualMachine"), subscriptionId);
                    URL_API = $"{URL_API}?api-version=2020-06-01";
                }
                else
                    URL_API = nextLink;

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //var token = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<VirtualMachineModel> lstVirtualMachine = new List<VirtualMachineModel>();

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var lstResources = result.SelectToken("value").ToObject(typeof(List<VirtualMachineModel>));
                        lstVirtualMachine = lstResources as List<VirtualMachineModel>;

                        if (lstVirtualMachine != null) VirtualMachineList.AddRange(lstVirtualMachine);

                        //next Link
                        var nextLinkObj = result.SelectToken("nextLink");
                        if (nextLinkObj != null) GetAllVirtualMachinesBySubscription("", nextLinkObj.ToString());
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var newToken = GetAuthToken();
                        GetAllVirtualMachinesBySubscription(subscriptionId);
                    }
                }

                return lstVirtualMachine;
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public List<AKSModel> GetAKSBySubscription(string subscriptionId)
        {
            try
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.AKS"), subscriptionId);
                URL_API = $"{URL_API}?api-version=2020-09-01";

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //AccessToken = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<AKSModel> lstAKS = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var _lstAKS = result.SelectToken("value").ToObject(typeof(List<AKSModel>));
                        lstAKS = _lstAKS as List<AKSModel>;
                    }
                    else
                    {
                        //log.DebugFormat("No se pudo procesar la URL {0}", url);
                    }
                }

                return lstAKS;

            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public List<WebSiteModel> GetWebSitesBySubscription(string subscriptionId)
        {
            try
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.WebSites"), subscriptionId);
                URL_API = $"{URL_API}?api-version=2019-08-01";

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //AccessToken = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<WebSiteModel> lstWebSite = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var _lstWebSite = result.SelectToken("value").ToObject(typeof(List<WebSiteModel>));
                        lstWebSite = _lstWebSite as List<WebSiteModel>;
                    }
                    else
                    {
                        //log.DebugFormat("No se pudo procesar la URL {0}", url);
                    }
                }

                return lstWebSite;

            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public List<CognitiveServiceModel> GetCognitiveServicesBySubscription(string subscriptionId)
        {
            try
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.CognitiveServices"), subscriptionId);
                URL_API = $"{URL_API}?api-version=2017-04-18";

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //AccessToken = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                List<CognitiveServiceModel> lstCognitiveServices = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var data = result.SelectToken("value").ToObject(typeof(List<CognitiveServiceModel>));
                        lstCognitiveServices = data as List<CognitiveServiceModel>;
                    }
                    else
                    {
                        //log.DebugFormat("No se pudo procesar la URL {0}", url);
                    }
                }

                return lstCognitiveServices;
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public VirtualMachineInfoModel GetVMInfoByFilters(string subscriptionId, string resourceGroup, string virtualMachine)
        {
            try
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.VMInfo"), subscriptionId, resourceGroup, virtualMachine);
                URL_API = $"{URL_API}?api-version=2020-06-01";

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                client.Timeout = -1;
                //AccessToken = GetAuthToken();

                request.AddHeader("Authorization", "Bearer " + AccessToken);

                VirtualMachineInfoModel itemVM = null;

                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

                        var data = result.ToObject(typeof(VirtualMachineInfoModel));
                        itemVM = data as VirtualMachineInfoModel;
                    }
                    else
                    {
                        //log.DebugFormat("No se pudo procesar la URL {0}", url);
                    }
                }

                return itemVM;
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }
        }

        public string GetAccountNameByMail(string usermail)
        {
            try
            {
                var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), usermail);
                URL_API = URL_API + "?$select=onPremisesSamAccountName,userPrincipalName";

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

                string accountName = null;

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        JObject result = JObject.Parse(jsonRetorno);

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

                    }
                    else
                    {
                        //log.DebugFormat("No se pudo procesar la URL {0}", url);
                    }
                }

                return accountName;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
