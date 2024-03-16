using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/RoadMap")]
    public class RoadMapController : BaseController
    {
        [Route("{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRoadMapById(int id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<RoadMapDAO>.Provider.GetRoadMapById(id);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostListRoadMap(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<RoadMapDAO>.Provider.GetRoadMap(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros != null)
            {
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostRoadMap(RoadMapDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int IdRoadMap = ServiceManager<RoadMapDAO>.Provider.AddOrEdit(request);
            bool estado = IdRoadMap > 0;
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }
    }
}
