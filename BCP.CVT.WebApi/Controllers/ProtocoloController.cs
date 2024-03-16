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
    [RoutePrefix("api/Protocolo")]
    public class ProtocoloController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoProtocolo(Paginacion pag)
        {

            var registros = ServiceManager<ProtocoloDAO>.Provider.Protocolo_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<ProtocoloDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(ProtocoloDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProtocoloById(int id)
        {

            var objTipo = ServiceManager<ProtocoloDAO>.Provider.Protocolo_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostProtocolo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProtocolo(ProtocoloDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            if (pag.ProtocoloId > 0)
            {
                dataRpta = ServiceManager<ProtocoloDAO>.Provider.Protocolo_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<ProtocoloDAO>.Provider.Protocolo_Insert(pag);
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
            var list = ServiceManager<ProtocoloDAO>.Provider.GetByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, list);
            return response;
        }



    }
}
