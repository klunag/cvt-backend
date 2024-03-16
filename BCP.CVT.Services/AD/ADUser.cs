using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BCP.CVT.Services
{
    public class ADUser
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string[] propiedades = {
                "sAMAccountName"
                ,"givenName"
                ,"sn"
                ,"title"
                ,"department"
                ,"physicalDeliveryOfficeName"
                ,"department"
                ,"department"
                ,"objectGUID"
                ,"company"
                ,"userPrincipalName"
                ,"objectSid"
                ,"displayName"
                ,"mail"};


        /* METODOS */

        /// <summary>
        /// Construye la clase utilizando el User Principal Name del usuario 
        /// </summary>
        /// <param name="matricula">
        /// User Principal Name</param>

        public ADUsuarioBE ObtenerADUsuario(string matricula)
        {
            DirectorySearcher searcher;
            SearchResult usuarioSR;

            //Obtener una referencia al buscador de AD para el Global Catalog
            using (searcher = Forest.GetCurrentForest().FindGlobalCatalog().GetDirectorySearcher())
            {

                searcher.PropertiesToLoad.AddRange(propiedades);

                //Buscar al usuario en base a su User Principal Name
                searcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", matricula);

                usuarioSR = searcher.FindOne();
                var objUsuarioAD = new ADUsuarioBE
                {

                    Usuario = ObtenerPropiedad(usuarioSR, "sAMAccountName"),
                    Nombre = ObtenerPropiedad(usuarioSR, "displayName"),
                    ApellidoPaterno = ObtenerPropiedad(usuarioSR, "sn"),
                    Puesto = ObtenerPropiedad(usuarioSR, "title"),
                    Area = ObtenerPropiedad(usuarioSR, "department"),
                    UbicacionFisica = ObtenerPropiedad(usuarioSR, "physicalDeliveryOfficeName"),
                    Servicio = ObtenerPropiedad(usuarioSR, "department"),
                    Division = ObtenerPropiedad(usuarioSR, "department"),
                    Guid = ObtenerPropiedad(usuarioSR, "objectGUID"),
                    Compania = ObtenerPropiedad(usuarioSR, "company"),
                    UserPrincipalName = ObtenerPropiedad(usuarioSR, "userPrincipalName"),
                    Sid = ObtenerPropiedad(usuarioSR, "objectSid"),
                    Correo = ObtenerPropiedad(usuarioSR, "mail"),
                    Anexo = ObtenerPropiedad(usuarioSR, "telephoneNumber"),
                };

                return objUsuarioAD;
            }
        }

        public List<ADUsuarioBE> ObtenerListaADUsuario(string nombre)
        {
            DirectorySearcher searcher;
            List<ADUsuarioBE> lista = new List<ADUsuarioBE>();
            //Obtener una referencia al buscador de AD para el Global Catalog
            using (searcher = Forest.GetCurrentForest().FindGlobalCatalog().GetDirectorySearcher())
            {

                searcher.PropertiesToLoad.AddRange(propiedades);

                //Buscar al usuario en base a su User Principal Name
                searcher.Filter = string.Format("(&(objectClass=user)(|(displayName=*{0})(displayName={0}*))(|(sAMAccountName=*{0})(sAMAccountName={0}*)))", nombre);

                var usuarioSR = searcher.FindAll();
                foreach (SearchResult item in usuarioSR)
                {
                    var objUsuarioAD = new ADUsuarioBE
                    {

                        Usuario = ObtenerPropiedad(item, "sAMAccountName"),
                        Nombre = ObtenerPropiedad(item, "displayName"),
                        ApellidoPaterno = ObtenerPropiedad(item, "sn"),
                        Puesto = ObtenerPropiedad(item, "title"),
                        Area = ObtenerPropiedad(item, "department"),
                        UbicacionFisica = ObtenerPropiedad(item, "physicalDeliveryOfficeName"),
                        Servicio = ObtenerPropiedad(item, "department"),
                        Division = ObtenerPropiedad(item, "department"),
                        Guid = ObtenerPropiedad(item, "objectGUID"),
                        Compania = ObtenerPropiedad(item, "company"),
                        UserPrincipalName = ObtenerPropiedad(item, "userPrincipalName"),
                        Sid = ObtenerPropiedad(item, "objectSid"),
                        Correo = ObtenerPropiedad(item, "mail"),
                        Anexo = ObtenerPropiedad(item, "telephoneNumber"),
                        Estado = 1
                    };
                    lista.Add(objUsuarioAD);
                }
                return lista;
            }
        }

        public string ObtenerNombreUsuarioAD(string matricula)
        {
            DirectorySearcher searcher;
            SearchResult usuarioSR;

            //Obtener una referencia al buscador de AD para el Global Catalog
            using (searcher = Forest.GetCurrentForest().FindGlobalCatalog().GetDirectorySearcher())
            {

                searcher.PropertiesToLoad.AddRange(propiedades);

                //Buscar al usuario en base a su User Principal Name
                searcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", matricula);

                usuarioSR = searcher.FindOne();

                return ObtenerPropiedad(usuarioSR, "userPrincipalName");

            }

        }

        public BaseUsuarioDTO GetUserDataByMail(string usermail)
        {
            var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), usermail);
            URL_API = URL_API + "?$select=displayName,id,mail,userPrincipalName,onPremisesSamAccountName";
            log.DebugFormat("Url búsqueda: {0}", URL_API);

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
            log.DebugFormat("Token: {0}", token);

            request.AddHeader("Authorization", "Bearer " + token);

            var response = client.Execute(request);

            BaseUsuarioDTO objUsuario = null;

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

                    objUsuario = new BaseUsuarioDTO
                    {
                        Matricula = accountName,
                        Nombres = result.SelectToken("displayName").ToString(),
                        CorreoElectronico = result.SelectToken("mail").ToString()
                    };

                }
                else
                {
                    log.DebugFormat("Status Description: {0}", response.StatusDescription);
                }
            }

            return objUsuario;
        }

        public string GetAccountNameByMail(string usermail, string colaborador)
        {
            string accountName = null;

            bool validarOnPremise = Settings.Get<bool>("OnPremise.Consulta");

            if (validarOnPremise)
            {
                accountName = GetAccountNameByMail_LocalApi(usermail);
                if (accountName == null)
                {
                    accountName = GetAccountNameByMail_AzureApi(usermail);
                    AddUsuario_LocalApi(usermail, accountName, colaborador);
                }
            }
            else
                accountName = GetAccountNameByMail_AzureApi(usermail);

            return accountName;
        }

        public string GetAccountNameByMail_LocalApi(string usermail)
        {
            try
            {
                string accountName = null;


                var URL_API = Settings.Get<string>("UrlApi") + "/Usuario";
                URL_API = URL_API + "?correo=" + usermail;
                log.Debug(URL_API);

                var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                ServicePointManager.UseNagleAlgorithm = false;
                ServicePointManager.Expect100Continue = false;

                client.Timeout = -1;
                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        log.DebugFormat("Json: {0}", jsonRetorno);
                        JObject result = JObject.Parse(jsonRetorno);

                        var accountObject = result.SelectToken("Matricula");
                        if (accountObject != null && !string.IsNullOrEmpty(accountObject.ToString()))
                        {
                            accountName = accountObject.ToString();
                        }

                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        accountName = null;
                    }
                    else
                    {
                        log.DebugFormat("Status Description: {0}", response.StatusDescription);
                    }
                }

                return accountName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddUsuario_LocalApi(string correo, string matricula, string colaborador)
        {
            try
            {
                var URL_API = Settings.Get<string>("UrlApi") + "/Usuario";
                URL_API = URL_API + "?correo=" + correo + "&matricula=" + matricula + "&nombres=" + colaborador;
                log.Debug(URL_API);

                var validarSSL = bool.Parse(Settings.Get<string>("Graph.API.SSLDeshabilitar"));

                RestClient client = new RestClient(new Uri(URL_API));
                IRestRequest request = new RestRequest(Method.GET);

                ServicePointManager.UseNagleAlgorithm = false;
                ServicePointManager.Expect100Continue = false;

                client.Timeout = -1;
                var response = client.Execute(request);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        log.DebugFormat("Json: {0}", jsonRetorno);
                    }
                    else
                    {
                        log.DebugFormat("Status Description: {0}", response.StatusDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public string GetAccountNameByMail_AzureApi(string usermail)
        {
            try
            {
                string accountName = null;

                var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), usermail);
                URL_API = URL_API + "?$select=onPremisesSamAccountName,userPrincipalName";
                log.Debug(URL_API);

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
                log.DebugFormat("Token: {0}", token);
                request.AddHeader("Authorization", "Bearer " + token);
                var response = client.Execute(request);
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonRetorno = response.Content;
                        log.DebugFormat("Json: {0}", jsonRetorno);
                        JObject result = JObject.Parse(jsonRetorno);

                        var accountObject = result.SelectToken("onPremisesSamAccountName");
                        if (accountObject != null && !string.IsNullOrEmpty(accountObject.ToString()))
                        {
                            accountName = accountObject.ToString();
                        }
                        else
                        {
                            accountObject = result.SelectToken("userPrincipalName");
                            if (accountObject != null && !string.IsNullOrEmpty(accountObject.ToString()))
                            {
                                accountName = accountObject.ToString();
                            }
                            else
                                accountName = null;
                        }
                    }
                    else
                    {
                        log.DebugFormat("Status Description: {0}", response.StatusDescription);
                    }
                }


                return accountName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<GraphUserMetadata> GetUsersByName(string filter_name)
        {
            var search_text = string.Format("?$select=displayName,givenName,id,mail,onPremisesSamAccountName,userPrincipalName&$filter=startswith(displayName,'{0}')", filter_name);

            var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), search_text);
            log.DebugFormat("Url búsqueda: {0}", URL_API);

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
            log.DebugFormat("Token: {0}", token);

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
                }
                else
                {
                    log.DebugFormat("Status Description: {0}", response.StatusDescription);
                }
            }

            return lista;
        }


        public List<GraphUserMetadata> GetUsersByEmail(string correo)
        {
            var search_text = string.Format("?$select=displayName,givenName,id,mail,onPremisesSamAccountName,userPrincipalName&$filter=startswith(mail,'{0}')", correo);

            var URL_API = string.Format(Settings.Get<string>("Graph.API.Users"), search_text);
            log.DebugFormat("Url búsqueda: {0}", URL_API);

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
            log.DebugFormat("Token: {0}", token);

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
                }
                else
                {
                    log.DebugFormat("Status Description: {0}", response.StatusDescription);
                }
            }

            return lista;
        }

        public GraphGroupMetadata GetGroupByName(string group_name)
        {
            //https://graph.microsoft.com/v1.0/groups?$select=displayName,id,mail&$filter=displayName eq 'Crosland'
            var search_text = string.Format("?$select=displayName,id,mail&$filter=displayName eq '{0}'", group_name);

            var URL_API = string.Concat(Settings.Get<string>("Graph.API.Groups"), search_text);
            log.DebugFormat("Url búsqueda: {0}", URL_API);

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
            log.DebugFormat("Token: {0}", token);

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
                else
                {
                    log.DebugFormat("Status Description: {0}", response.StatusDescription);
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

                var URL_API = string.Concat(Settings.Get<string>("Graph.API.Groups"), string.Format("/{0}/members", objGroup.id), select_text);
                log.DebugFormat("Url búsqueda: {0}", URL_API);

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
                log.DebugFormat("Token: {0}", token);

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
                    }
                    else
                    {
                        log.DebugFormat("Status Description: {0}", response.StatusDescription);
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
                else
                {
                    //log.DebugFormat("No se pudo procesar la URL {0}", url);
                }
            }

            return access_token;
        }

        public ADUser()
        {

        }

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


        /// <summary>
        /// Obtiene el valor de la propiedad del objecto searchResult
        /// </summary>
        /// <param name="item">object searchResult de donde se obtendra el valor.</param>
        /// <param name="nombrePropiedad">nombre de la propiedad a buscar.</param>
        /// <returns>el valor de una propiedad de un objecto SearchResult</returns>
        private string ObtenerPropiedad(SearchResult item
                , string nombrePropiedad)
        {
            string valor = string.Empty;

            if (item == null)
                return valor;

            if (item.Properties[nombrePropiedad] != null &&
                    item.Properties[nombrePropiedad].Count > 0)
            {
                if (nombrePropiedad.ToLower().Equals("objectguid"))
                {
                    valor = new Guid((byte[])
                        (item.Properties[nombrePropiedad][0])).ToString();
                }
                else if (nombrePropiedad.ToLower().Equals("objectsid"))
                {
                    valor = ConvertirByteASidString((byte[])
                            (item.Properties[nombrePropiedad][0]));
                }
                else
                    valor = item.Properties[nombrePropiedad][0].ToString();
            }

            return valor;
        }

        /// <summary>
        /// Convierte un arreglo de bytes a un security ID (sid)
        /// </summary>
        /// <param name="sidBytes">arreglo de bytes a convertir.</param>
        /// <returns>Security ID construido a partir de un arreglo de bytes</returns>
        /// <remarks>
        /// Basado en el documeto: 
        /// <see cref="http://www.codeproject.com/KB/cs/getusersid.aspx"/></remarks>
        private string ConvertirByteASidString(Byte[] sidBytes)
        {
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");
            if (sidBytes != null && sidBytes.Length > 0)
            {
                // Agregar el SID revision.
                strSid.Append(sidBytes[0].ToString());
                // Agregar los 6 bytes correspondientes al SID authority value.
                if (sidBytes[6] != 0 || sidBytes[5] != 0)
                {
                    string strAuth = String.Format
                        ("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                        (Int16)sidBytes[1],
                        (Int16)sidBytes[2],
                        (Int16)sidBytes[3],
                        (Int16)sidBytes[4],
                        (Int16)sidBytes[5],
                        (Int16)sidBytes[6]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    Int64 iVal = (Int32)(sidBytes[1]) +
                        (Int32)(sidBytes[2] << 8) +
                        (Int32)(sidBytes[3] << 16) +
                        (Int32)(sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Obtener el conteo de "sub authority"
                int iSubCount = Convert.ToInt32(sidBytes[7]);
                int idxAuth = 0;
                for (int i = 0; i < iSubCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
                return strSid.ToString();
            }
            else
                return "";
        }

        public class ADUsuarioBE
        {
            public string Usuario { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string Puesto { get; set; }
            public string Area { get; set; }
            public string UbicacionFisica { get; set; }
            public string Servicio { get; set; }
            public string Division { get; set; }
            public string Guid { get; set; }
            public string Compania { get; set; }
            public string UserPrincipalName { get; set; }
            public string Sid { get; set; }
            public string Correo { get; set; }
            public string Anexo { get; set; }
            public int Estado { get; set; }

            public string Id => Usuario ?? string.Empty;
            public string Descripcion => Usuario ?? string.Empty;
            public string value => Usuario ?? string.Empty;

        }
    }
}