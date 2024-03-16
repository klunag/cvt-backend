using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Puerto")]
    public class PuertoController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoPuerto(Paginacion pag)
        {

            var registros = ServiceManager<PuertoDAO>.Provider.Puerto_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<PuertoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(PuertoDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPuertoById(int id)
        {

            var objTipo = ServiceManager<PuertoDAO>.Provider.Puerto_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostPuerto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostPuerto(PuertoDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            if (pag.PuertoId > 0)
            {
                dataRpta = ServiceManager<PuertoDAO>.Provider.Puerto_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<PuertoDAO>.Provider.Puerto_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var list = ServiceManager<PuertoDAO>.Provider.GetByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, list);
            return response;
        }

    }
}
