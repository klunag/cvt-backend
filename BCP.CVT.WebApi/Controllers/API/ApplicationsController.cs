using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers.API
{
   [Authorize]
    [RoutePrefix("api/it-management/plataform-operations/v2/applications")]
    public class ApplicationsController : BaseController
    {
        [Route("")]
        [HttpGet]
        [ValidateModel]
        [Authorize]
        public HttpResponseMessage GetApplication([FromUri] ApplicationPag pag)
        {
            HttpResponseMessage response = null;
            if (pag == null)
            {
                var errorResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003));

                response = Request.CreateResponse(HttpStatusCode.BadRequest, errorResp);
                return response;
            }

            var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplications(pag, out int totalRows);

            var apiResp = new APIResponse<List<ApplicationMgtDTO>>(
                                EAPIResponseCode.TL0001.ToString(),
                                Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                                data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("{applicationId}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApplication(string applicationId)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationById(applicationId);

            var apiResp = new APIResponse<ApplicationIdMgtDTO>(
                    EAPIResponseCode.TL0001.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                    data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }
        
        [Route("{applicationId}/relations")]
        [HttpGet]
        [Authorize]
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
            var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationRelationsById(pag, out int totalRows);

            var apiResp = new APIResponse<List<ApplicationRelationsMgtDTO>>(
                            EAPIResponseCode.TL0001.ToString(),
                            Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                            data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [ValidateModel]
        [Route("{applicationId}/obsolescence")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApplicationObsolescence(string applicationId)
        {
            
            var pag = new ApplicationObsolescencePag()
            {
                applicationId = applicationId,
                date = DateTime.Now,                
            };
            HttpResponseMessage response = null;
            var data = ServiceManager<ApplicationMgtDAO>.Provider.GetApplicationObsolescenceById(pag.applicationId, pag.date.Value);

            var apiResp = new APIResponse<ApplicationObsMgtDTO>(
                    EAPIResponseCode.TL0001.ToString(),
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0001),
                    data);

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }
    }
}
