
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using BCP.CVT.Services.Interface;
using BCP.CVT.DTO.Auditoria;
using System.IO;
using System;
using System.Security.Claims;

namespace BCP.CVT.WebApi.Controllers
{
    public class BaseController : ApiController
    {


        //public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var requestBody = "";
        //        var codAplicacion = "";

        //        if (controllerContext.Request.Method.Method == "GET")
        //        {
        //            requestBody = controllerContext.Request.RequestUri.Query;
        //        }
        //        else
        //        {
        //            if(controllerContext.Request.Content.Headers.ContentType == null || controllerContext.Request.Content.Headers.ContentType.MediaType != "multipart/form-data")
        //            {
        //                requestBody = getRawPostData(controllerContext).Result;
        //            }
        //            //else if(controllerContext.Request.Content.Headers.ContentType.MediaType == "multipart/form-data")
        //            //{
        //            //    byte[] requestBodyBytes = getRawPostFormData(controllerContext).Result;
        //            //    var stream = new MemoryStream(requestBodyBytes);
        //            //    //HttpContentParser parser = new HttpContentParser(stream);
        //            //    requestBody = System.Text.Encoding.UTF8.GetString(requestBodyBytes);

        //            //}
                    
        //        }

        //        if (controllerContext.Request.Headers.Authorization != null)
        //        {
        //            var authToken = controllerContext.Request.Headers.Authorization.Parameter;

        //            var decodeauthToken = System.Text.Encoding.UTF8.GetString(
        //                Convert.FromBase64String(authToken));

        //            var arrUserNameandPassword = decodeauthToken.Split(':');
        //            codAplicacion = arrUserNameandPassword[0];
        //        }

        //        var userClaims = controllerContext.RequestContext.Principal.Identity as ClaimsIdentity;
        //        var username = userClaims?.FindFirst("name")?.Value;

        //        var objRegistro = new AuditoriaAPIDTO
        //        {
        //            APIMetodo = controllerContext.Request.Method.Method,
        //            APINombre = controllerContext.Request.RequestUri.AbsolutePath,
        //            APIParametros = requestBody,
        //            APIUsuario = codAplicacion,
        //            CreadoPor = string.IsNullOrEmpty(username) ? "" : username,

        //        };
        //        ServiceManager<AuditoriaDAO>.Provider.RegistrarAuditoriaAPI(objRegistro);
        //        return base.ExecuteAsync(controllerContext, cancellationToken);
        //    }
        //    catch(HttpResponseException ex) { throw ex; }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
            

        //}

        private async Task<String> getRawPostData(HttpControllerContext controllerContext)
        {
            return await controllerContext.Request.Content.ReadAsStringAsync();

            //using (var contentStream = await controllerContext.Request.Content.ReadAsStreamAsync())
            //{
            //    contentStream.Seek(0, SeekOrigin.Begin);
            //    using (var sr = new StreamReader(contentStream))
            //    {
            //        return sr.ReadToEnd();
            //    }
            //}
        }

        //private async Task<byte[]> getRawPostFormData(HttpControllerContext controllerContext)
        //{
        //    var param = System.Web.HttpContext.Current.Request.Params;
        //    return await controllerContext.Request.Content.ReadAsByteArrayAsync();
        //}
    }
}
