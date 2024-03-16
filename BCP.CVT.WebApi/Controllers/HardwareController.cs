using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.CargaMasiva;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Hardware")]
    public class HardwareController : BaseController
    {
        [Route("ListarCombos_Detallado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos_Detallado()
        {
            HttpResponseMessage response = null;
            FiltrosHardware dataRpta = ServiceManager<HardwareDAO>.Provider.GetFiltros_Detallado();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<HardwareDAO>.Provider.GetEquipoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ReporteDetallado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ReporteDetallado(PaginaHardwareDetallado filtros)
        {
            HttpResponseMessage response = null;
            var user = TokenGenerator.GetCurrentUser();
            filtros.Matricula = user.Matricula;
            filtros.PerfilId = user.PerfilId;
            var totalRows = 0;
            var registros = ServiceManager<HardwareDAO>.Provider.GetReporteDetallado(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ListarCombos_KPI")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos_KPI()
        {
            HttpResponseMessage response = null;
            FiltrosHardware dataRpta = ServiceManager<HardwareDAO>.Provider.GetFiltros_KPI();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ReporteKPI")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ReporteKPI(string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds, int nivel, string UnidadFondeoIds, string filtrosPadre = null)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId;
            var validaGestorUnit = user.Perfil.IndexOf("E195_GestorUnit");
            if (validaGestorUnit > -1)
            {
                user.PerfilId = 20;
            }

            var registros = ServiceManager<HardwareDAO>.Provider.GetReporteKPI(matricula, perfilId, UnidadFondeoIds, GestionadoPorIds, TeamSquadIds, FabricanteIds, ModeloIds, nivel, filtrosPadre);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.UnidadFondeo = HttpUtility.HtmlEncode(x.UnidadFondeo); });

            return Ok(registros);
        }


        [Route("Exportar_Detallado")]
        [HttpGet]
        public IHttpActionResult Exportar_Detallado(string UnidadFondeoIds, string GestionadoPorIds, string TeamSquadIds
            , string EquipoId, string FabricanteIds, string ModeloIds, string EstadoObsolescenciaIds, string TipoHardwareIds
            , string sortName, string sortOrder, string fecha)
        {
            var user = TokenGenerator.GetCurrentUser();

            PaginaHardwareDetallado filtros = new PaginaHardwareDetallado
            {
                UnidadFondeo = UnidadFondeoIds,
                GestionadoPor = GestionadoPorIds,
                TeamSquad = TeamSquadIds,
                Fabricante = FabricanteIds,
                Modelo = ModeloIds,
                EstadoObsolescencia = EstadoObsolescenciaIds,
                TipoHardware = TipoHardwareIds,
                Fecha = fecha,
                sortName = sortName,
                sortOrder = sortOrder,
                pageNumber = 1,
                pageSize = int.MaxValue,
                Matricula = user.Matricula,
                PerfilId = user.PerfilId
            };
            
            string nomArchivo = "";

            var data = new ExportarData().ExportarHardware_Detallado(filtros);
            nomArchivo = string.Format("ReporteHardwareDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Exportar_KPI")]
        [HttpGet]
        public IHttpActionResult Exportar_KPI(string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds, string UnidadFondeoIds)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId;
            var validaGestorUnit = user.Perfil.IndexOf("E195_GestorUnit");
            if (validaGestorUnit > -1)
            {
                user.PerfilId = 20;
            }

            string nomArchivo = string.Format("ReporteHardwareKPI_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarHardware_KPI(matricula, perfilId, UnidadFondeoIds, GestionadoPorIds, TeamSquadIds, FabricanteIds, ModeloIds);

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}
