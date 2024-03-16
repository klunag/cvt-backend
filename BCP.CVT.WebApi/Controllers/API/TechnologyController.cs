using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers.API
{
   [Authorize]
    [RoutePrefix("api/it-management/plataform-operations/v2/technologies")]
    public class TechnologyController : BaseController
    {
        [Route("types")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTypes()
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetTypeTechnologies();

            var apiResp = new APIResponse<List<TypeTechnologyDTO>>(
                EAPIResponseCode.TL0001.ToString(), 
                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("reasons")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetReasons()
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetReasons();

            var apiResp = new APIResponse<List<ReasonDTO>>(
                EAPIResponseCode.TL0001.ToString(),
                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("undefinedreasons")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUndefinedReasons()
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetUndefinedReasons();

            var apiResp = new APIResponse<List<UndefinedReasonDTO>>(
                EAPIResponseCode.TL0001.ToString(),
                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("products")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetProducts([FromUri] ProductPag pag)
        {
            HttpResponseMessage httpResponse = null;
            if (pag == null)
            {
                var errorResponse = new APIResponse<TechnologyResponse>(EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));
                httpResponse = Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                return httpResponse;
            }

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
            return httpResponse;
        }

        [Route("products/{productCode}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetProductById(string productCode)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetProductById(productCode);

            var apiResp = new APIResponse<ProductDTO>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("domains")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDomains()
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetDomainTechnologies();

            var apiResp = new APIResponse<List<DomainTechnologyDTO>>(
                EAPIResponseCode.TL0001.ToString(),
                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("domains/{domainId:int}/subdomains")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubdomains(int domainId)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TechnologyMgtDAO>.Provider.GetSubdomainsByDomainTechnologyId(domainId);

            var apiResp = new APIResponse<List<SubdomainTechnologyDTO>>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTechnology([FromUri] TechnologyPag pag)
        {
            HttpResponseMessage httpResponse = null;
            if (pag == null)
            {
                var errorResponse = new APIResponse<TechnologyResponse>(EAPIResponseCode.TL0003.ToString(), 
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));
                httpResponse = Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                return httpResponse;
            }

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
            return httpResponse;
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTechnology(TechnologyPostDTO objDTO)
        {
            HttpResponseMessage httpResponse = null;
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

            return httpResponse;
        }
    }
}
