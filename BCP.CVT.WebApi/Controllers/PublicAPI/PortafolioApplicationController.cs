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
    [RoutePrefix("api/portafolioapplication")]
    public class PortafolioApplicationController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetApplication([FromUri] ApplicationPag pag)
        {
            HttpResponseMessage response = null;

            //Validar AppId
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

                    var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplications(pag, out int totalRows);

                    var apiResp = new APIResponse<List<ApplicationMgtDTO>>(
                                        EAPIResponseCode.TL0001.ToString(),
                                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            
            return response;
        }

        [Route("{applicationId}")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetApplication(string applicationId)
        {
            HttpResponseMessage response = null;

            //Validar AppId
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
                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationById(applicationId);

                    var apiResp = new APIResponse<ApplicationIdMgtDTO>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }
            
            return response;
        }

        [Route("{applicationId}/relations")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetApplicationRelations(string applicationId)
        {
            var pag = new ApplicationRelationsPag()
            {
                applicationId = applicationId,
                date = DateTime.Now,
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            HttpResponseMessage response = null;

            //Validar AppId
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
                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationRelationsById(pag, out int totalRows);

                    var apiResp = new APIResponse<List<ApplicationRelationsMgtDTO>>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                    data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }
            
            return response;
        }
        
        [Route("{applicationId}/obsolescence")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetApplicationObsolescence(string applicationId)
        {

            var pag = new ApplicationObsolescencePag()
            {
                applicationId = applicationId,
                date = DateTime.Now,
            };
            HttpResponseMessage response = null;

            //Validar AppId
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
                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationObsolescenceById(pag.applicationId, pag.date.Value);

                    var apiResp = new APIResponse<ApplicationObsMgtDTO>(
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