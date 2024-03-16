using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Auditoria")]
    public class AuditoriaController : BaseController
    {
        [Route("ListarHistoricoModificaciones")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarHistoricoModificaciones(PaginacionHistoricoModificacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AuditoriaDAO>.Provider.GetHistoricoModificacion(pag.Accion, pag.Entidad, pag.FechaActualizacion, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.EntidadClave = HttpUtility.HtmlEncode(x.EntidadClave); });

            dynamic reader = new BootstrapTable<AuditoriaDataDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarVisitaSite")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarVisitaSite(PaginacionVisitaSite pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AuditoriaDAO>.Provider.GetVisitaSite(pag.Matricula, pag.Nombre, pag.FechaDesde, pag.FechaHasta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<VisitaSiteDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var listAccion = Utilitarios.EnumToList<EAccion>();

            var dataRpta = new
            {
                Accion = listAccion.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarHistoricoModificacion()
        {
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarHistoricoModificacion(string.Empty, string.Empty, null, string.Empty, string.Empty);
            nomArchivo = string.Format("ListaHistoricoModificacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarVisitaSite")]
        [HttpGet]
        public IHttpActionResult GetExportarVisitaSite(string Matricula, string Nombre, DateTime? FechaDesde, DateTime? FechaHasta, string sortName, string sortOrder)
        {
            string nomArchivo = string.Empty;
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarVisitaSite(Matricula, Nombre, FechaDesde, FechaHasta, sortName, sortOrder);
            nomArchivo = string.Format("ListaVisitaSite_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("ListarAuditoria")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarAuditoriaData(PaginacionAuditoriaData pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AuditoriaDAO>.Provider.GetAuditoriaLista(pag.Matricula, pag.Entidad, pag.Campo, pag.Accion, pag.FechaDesde, pag.FechaHasta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DTO.Auditoria.AuditoriaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAuditoriaData")]
        [HttpGet]
        public IHttpActionResult GetExportarAuditoriaData(string matricula, string entidad, string campo, string accion, DateTime? fechaDesde, DateTime? fechaHasta, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarAuditoriaData(matricula ?? "", entidad ?? "", campo ?? "", accion ?? "", fechaDesde, fechaHasta, sortName, sortOrder);
            nomArchivo = string.Format("ListaAuditoriaData_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListarAuditoriaAPI")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarAuditoriaAPIData(PaginacionAuditoriaAPIData pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AuditoriaDAO>.Provider.GetAuditoriaAPILista(pag.APIUsuario, pag.APINombre, pag.APIMetodo, pag.FechaDesde, pag.FechaHasta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DTO.Auditoria.AuditoriaAPIDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCombosAuditoriaAPI")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosAuditoriaAPI()
        {
            HttpResponseMessage response = null;
            var listAccion = Utilitarios.EnumToList<EAPIMetodo>();

            var dataRpta = new
            {
                Accion = listAccion.Select(x => new { Id = Utilitarios.GetEnumDescription2(x), Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ExportarAuditoriaAPI")]
        [HttpGet]
        public IHttpActionResult GetExportarAuditoriaAPI(string APIUsuario, string APINombre, string APIMetodo, DateTime? fechaDesde, DateTime? fechaHasta, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarAuditoriaAPI(APIUsuario ?? "", APINombre ?? "", APIMetodo ?? "", fechaDesde, fechaHasta, sortName, sortOrder);
            nomArchivo = string.Format("ListaAuditoriaAPI_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}
