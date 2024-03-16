using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Mep")]
    public class MepController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListado(PaginacionMep pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<MepDAO>.Provider.GetListado(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<NotaAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("GetNotaAplicacionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetNotaAplicacionById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<MepDAO>.Provider.GetNotaAplicacionById(id);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("AddOrEditNotaAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostNotaAplicacion(NotaAplicacionDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<MepDAO>.Provider.AddOrEditNotaAplicacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportar(string tribu, string codigoAPT, string jde, string experto, string sortName, string sortOrder)
        {
            string filename = "";
            if (string.IsNullOrEmpty(tribu)) tribu = null;
            if (string.IsNullOrEmpty(codigoAPT)) codigoAPT = null;
            if (string.IsNullOrEmpty(jde)) jde = null;
            if (string.IsNullOrEmpty(experto)) experto = null;

            var data = new ExportarData().ExportarNotaAplicacion(tribu, codigoAPT, jde, experto, sortName, sortOrder);
            filename = string.Format("ListadoReporte_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ExisteCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteCodigoAPT(string codigoAPT, int? id)
        {
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(codigoAPT)) codigoAPT = null;
            bool estado = ServiceManager<MepDAO>.Provider.ExisteCodigoAPT(codigoAPT, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }
    }
}
