using BCP.CVT.Cross;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.EnvioCorreos
{ 
    public class EmailApiService
    {
       public EmailApiService()
       {
           
        }

        public bool SendMail(string mailSenderId, List<string> to, List<string> cc, string subject, string bodyContent, byte[] attachmentBytes, string attachmentName)
        {
            try
            {
                string URL_API = Settings.Get<string>("Graph.API.SendMail");
                URL_API = string.Format(URL_API, mailSenderId);
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

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + Constantes.GraphAuthToken);

                var objMail = new MailDTO
                {
                    message = new MailDTO.Message
                    {
                        subject = subject,
                        body = new MailDTO.Message.Body
                        {
                            contentType = "HTML",
                            content = bodyContent
                        },
                        toRecipients = to.Select(x => new MailDTO.Message.EmailAdress
                        {
                            emailAddress = new MailDTO.Message.Adress { address = x }

                        }).ToList(),
                        ccRecipients = cc == null ? new List<MailDTO.Message.EmailAdress>() :
                                        cc.Where(x=>!string.IsNullOrEmpty(x)).Select(x => new MailDTO.Message.EmailAdress
                                        {
                                            emailAddress = new MailDTO.Message.Adress { address = x }

                                        }).ToList()
                        ,
                        attachments = attachmentBytes != null ? new List<MailDTO.Message.FileAttachment>
                    {
                         new MailDTO.Message.FileAttachment{
                              odatatype = "#microsoft.graph.fileAttachment",
                                contentBytes = attachmentBytes,
                                contentId = Guid.NewGuid().ToString(),
                                name = attachmentName
                         }
                    } : new List<MailDTO.Message.FileAttachment>()
                    },

                    saveToSentItems = true

                };

                var jsonRequest = JsonConvert.SerializeObject(objMail);
                jsonRequest = jsonRequest.Replace("odatatype", "@odata.type");

                request.AddParameter("application/json", jsonRequest, ParameterType.RequestBody);

                var response = client.Execute(request);


                if (response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Mail response: " + response.ErrorMessage);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMail - Error");
                Console.WriteLine("Error: " + ex.ToString());
                return true;
            }
        }


        public string GetUserByMail(string userMail)
        {
            Constantes.GraphAuthToken = new AzureUtilitarios().GetGraphAuthToken();

            string URL_API = Settings.Get<string>("Graph.API.Users");   
            URL_API = string.Format(URL_API, userMail);

            var validarSSL =  Settings.Get<bool>("Graph.API.SSLDeshabilitar");


            RestClient client = new RestClient(new Uri(URL_API));
            IRestRequest request = new RestRequest(Method.GET);

            if (!validarSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;
            }
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            client.Timeout = -1;
            request.AddHeader("Authorization", "Bearer " + Constantes.GraphAuthToken);

            var response = client.Execute(request);

            string id_user = null;

            if (response != null)
            {
                Console.WriteLine("GetUserByMail - StatusCode: " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    id_user = result.SelectToken("id").ToString();
                }
                else
                {
                    Console.WriteLine("GetUserByMail - ErrorMessage: " + response.Content);
                    //log.DebugFormat("No se pudo procesar la URL {0}", url);
                }
            }

            Console.WriteLine("GetUserByMail: " + id_user);

            return id_user;
        }


    }
}
