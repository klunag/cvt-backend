using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers.API
{
   [Authorize]
    [RoutePrefix("api/it-management/plataform-operations/v2/it-resources")]
    public class ItResourceController : BaseController
    {
        [Route("")]
        [HttpGet]
        [ValidateModel]
        [Authorize]
        public HttpResponseMessage GetItResource([FromUri] ItResourcePag pag)
        {
            HttpResponseMessage response = null;
            if (pag == null)
            {
                var errorResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));

                response = Request.CreateResponse(HttpStatusCode.BadRequest, errorResp);
                return response;
            }

            var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResources(pag, out int totalRows);

            var apiResp = new APIResponse<List<ItResourceMgtDTO>>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("{itResourceId}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetItResource(string itResourceId)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResourceById(itResourceId);

            var apiResp = new APIResponse<ItResourceIdMgtDTO>(
                EAPIResponseCode.TL0001.ToString(),
                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [ValidateModel]
        [Route("relations")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetItResource([FromUri] ItResourceRelationsPag pag)
        {
            HttpResponseMessage response = null;
            if (pag == null)
            {
                var errorResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));

                response = Request.CreateResponse(HttpStatusCode.BadRequest, errorResp);
                return response;
            }

            var data = ServiceManager<ITResourceMgtDAO>.Provider.GetItResourceRelations(pag, out int totalRows);

            var apiResp = new APIResponse<List<ItResourceRelationsMgtDTO>>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }
    }
}
