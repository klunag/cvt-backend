using BCP.CVT.Cross;
using BCP.CVT.DTO.Auditoria;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers.PublicAPI
{
    [RoutePrefix("api/resourcecvt")]
    public class ResourceCvtController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetItResource([FromUri] ItResourcePag pag)
        {
            HttpResponseMessage response = null;

            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if (string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            }
            else
            {
                var applicationValid = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(headerApp);
                if (applicationValid == null)
                {
                    var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                   EAPIResponseCode.TL0006.ToString(),
                   Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0006),
                   null);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {                  
                    if (pag == null)
                    {
                        var errorResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));

                        response = Request.CreateResponse(HttpStatusCode.BadRequest, errorResp);
                        return response;
                    }

                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResources(pag, out int totalRows);

                    var apiResp = new APIResponse<List<ItResourceMgtDTO>>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                    data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return response;
        }

        [Route("{itResourceId}")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetItResource(string itResourceId)
        {
            HttpResponseMessage response = null;
            //Validar AppId
            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if(string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            }
            else
            {
                var applicationValid = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(headerApp);
                if (applicationValid == null)
                {
                    var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                   EAPIResponseCode.TL0006.ToString(),
                   Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0006),
                   null);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {
                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResourceById(itResourceId);

                    var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                        EAPIResponseCode.TL0001.ToString(),
                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }            
            return response;
        }
        
        [Route("relations")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetItResource([FromUri] ItResourceRelationsPag pag)
        {
            HttpResponseMessage response = null;

            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if (string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            }
            else
            {
                var applicationValid = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(headerApp);
                if (applicationValid == null)
                {
                    var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                   EAPIResponseCode.TL0006.ToString(),
                   Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0006),
                   null);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {                    
                    if (pag == null)
                    {
                        var errorResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));

                        response = Request.CreateResponse(HttpStatusCode.BadRequest, errorResp);
                        return response;
                    }

                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResourceRelations(pag, out int totalRows);

                    var apiResp = new APIResponse<List<ItResourceRelationsMgtDTO>>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                    data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }
           
            return response;
        }

        private void SaveAudit(System.Web.Http.Controllers.HttpControllerContext controllerContext, string app)
        {
            try
            {
                var requestBody = string.Empty;                

                if (controllerContext.Request.Method.Method == "GET")
                {
                    requestBody = controllerContext.Request.RequestUri.Query;
                }
                else
                {
                    if (controllerContext.Request.Content.Headers.ContentType == null || controllerContext.Request.Content.Headers.ContentType.MediaType != "multipart/form-data")
                    {
                        requestBody = getRawPostData(controllerContext).Result;
                    }                   
                }

                var objRegistro = new AuditoriaAPIDTO
                {
                    APIMetodo = controllerContext.Request.Method.Method,
                    APINombre = controllerContext.Request.RequestUri.AbsolutePath,
                    APIParametros = requestBody,
                    APIUsuario = app,
                    CreadoPor = string.IsNullOrEmpty(app) ? "" : app,

                };
                ServiceManager<AuditoriaDAO>.Provider.RegistrarAuditoriaAPI(objRegistro);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private async Task<String> getRawPostData(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            return await controllerContext.Request.Content.ReadAsStringAsync();            
        }
    }
}