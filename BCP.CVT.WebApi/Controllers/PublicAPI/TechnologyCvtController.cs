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
    [RoutePrefix("api/technologycvt")]
    public class TechnologyCvtController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("types")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetTypes()
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetTypeTechnologies();

                    var apiResp = new APIResponse<List<TypeTechnologyDTO>>(
                        EAPIResponseCode.TL0001.ToString(),
                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }            
            return response;
        }

        [Route("reasons")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetReasons()
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetReasons();

                    var apiResp = new APIResponse<List<ReasonDTO>>(
                        EAPIResponseCode.TL0001.ToString(),
                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }
            
            return response;
        }

        [Route("undefinedreasons")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetUndefinedReasons()
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetUndefinedReasons();

                    var apiResp = new APIResponse<List<UndefinedReasonDTO>>(
                        EAPIResponseCode.TL0001.ToString(),
                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return response;
        }

        [Route("products")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetProducts([FromUri] ProductPag pag)
        {
            HttpResponseMessage httpResponse = null;

            //Validar AppId
            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if (string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
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

                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {
                    
                    if (pag == null)
                    {
                        var errorResponse = new APIResponse<TechnologyResponse>(EAPIResponseCode.TL0003.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));
                        httpResponse = Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                        return httpResponse;
                    }

                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var _products = ServiceManager<TechnologyMgtDAO>.Provider.GetProducts(pag, out int _totalRows);

                    var data = new ProductResponse()
                    {
                        total = _totalRows,
                        products = _products
                    };
                    var apiResp = new APIResponse<ProductResponse>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001), data);

                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return httpResponse;
        }

        [Route("products/{productCode}")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetProductById(string productCode)
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetProductById(productCode);

                    var apiResp = new APIResponse<ProductDTO>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                    data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }
            
            return response;
        }

        [Route("domains")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetDomains()
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetDomainTechnologies();

                    var apiResp = new APIResponse<List<DomainTechnologyDTO>>(
                        EAPIResponseCode.TL0001.ToString(),
                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                        data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return response;
        }

        [Route("domains/{domainId:int}/subdomains")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetSubdomains(int domainId)
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

                    var data = ServiceManager<TechnologyMgtDAO>.Provider.GetSubdomainsByDomainTechnologyId(domainId);

                    var apiResp = new APIResponse<List<SubdomainTechnologyDTO>>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                    data);

                    response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return response;
        }

        [Route("")]
        [HttpGet]
        [JwtAuthentication]
        public HttpResponseMessage GetTechnology([FromUri] TechnologyPag pag)
        {
            HttpResponseMessage httpResponse = null;

            //Validar AppId
            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if (string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
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

                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {
                    
                    if (pag == null)
                    {
                        var errorResponse = new APIResponse<TechnologyResponse>(EAPIResponseCode.TL0003.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));
                        httpResponse = Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                        return httpResponse;
                    }

                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    var _technologies = ServiceManager<TechnologyMgtDAO>.Provider.GetTechnologies(pag, out int _totalRows);

                    var data = new TechnologyResponse()
                    {
                        total = _totalRows,
                        technologies = _technologies
                    };
                    var apiResp = new APIResponse<TechnologyResponse>(
                                    EAPIResponseCode.TL0001.ToString(),
                                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001), data);

                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
            }

            return httpResponse;
        }

        [Route("")]
        [HttpPost]
        [JwtAuthentication]
        public HttpResponseMessage PostTechnology(TechnologyPostDTO objDTO)
        {
            HttpResponseMessage httpResponse = null;

            //Validar AppId
            var request = HttpContext.Current.Request;
            var headerApp = request.Headers.Get("applicationId");
            if (string.IsNullOrWhiteSpace(headerApp))
            {
                var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                    EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                    null);

                httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
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

                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                }
                else
                {

                    //Guardar auditoria de llamada a API
                    SaveAudit(this.ControllerContext, headerApp);

                    string description = string.Empty, detail = string.Empty;

                    var res = ServiceManager<TechnologyMgtDAO>.Provider.AddTechnology(objDTO);
                    var statusCode = int.Parse(res.Code);

                    if (statusCode == (int)EStatusCodeSP.OperacionExitosa)
                    {
                        var data = res.Message;
                        description = Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001);
                        var apiResp = new APIResponse<string>(
                                        EAPIResponseCode.TL0001.ToString(),
                                        description,
                                        data,
                                        detail);

                        httpResponse = Request.CreateResponse(HttpStatusCode.OK, apiResp);
                    }
                    else
                    {
                        detail = Utilitarios.GetEnumDescription2((EStatusCodeSP)statusCode);

                        var apiResp = new APIResponse<string>(
                                        EAPIResponseCode.TL0003.ToString(),
                                        Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003),
                                        null,
                                        detail);

                        httpResponse = Request.CreateResponse(HttpStatusCode.BadRequest, apiResp);
                    }

                }
            }

            return httpResponse;
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