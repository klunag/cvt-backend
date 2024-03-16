using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using BCP.CVT.Cross;
using System.Collections.Generic;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Indicadores")]
    public class IndicadoresController : BaseController
    {
        [Route("Gerencial/Equipos/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GerencialEquiposCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosIndicadoresGerencialEquipo dataRpta = new FiltrosIndicadoresGerencialEquipo();

            dataRpta = ServiceManager<EquipoDAO>.Provider.ListFiltrosIndicadores();


            var nroMeses = Settings.Get<string>("Indicadores.Equipos.NroMeses");
            dataRpta.NroMesesConsulta = nroMeses.Split('|');

            //var nroNroSubdominios = Settings.Get<string>("Indicadores.Equipos.NroSubdominios");
            //dataRpta.NroSubdominios = nroNroSubdominios.Split('|');


            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Gerencial/Equipos/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerencialEquiposReporte(FiltrosIndicadoresGerencialEquipo filtros)
        {
            HttpResponseMessage response = null;
            var dataRptaTodosEquipos = ServiceManager<IndicadoresDAO>.Provider.GetReporteIndicadoresGerencialEquipos(filtros, true);

            //SETEAR LOS SUBDOMINIOS DEL 2DO TAB.
            filtros.SubdominiosFiltroBase = new List<string>
                {
                    Settings.Get<string>("Subdominio.SO"),
                    Settings.Get<string>("Subdominio.BdNonRelational"),
                    Settings.Get<string>("Subdominio.BdRelational")
                };

            filtros.SubdominiosFiltro = new List<string>
                {
                    Settings.Get<string>("Subdominio.SO"),
                    Settings.Get<string>("Subdominio.BdNonRelational"),
                    Settings.Get<string>("Subdominio.BdRelational")
                };

            var dataRptaEquiposSO_BD = ServiceManager<IndicadoresDAO>.Provider.GetReporteIndicadoresGerencialEquipos(filtros, false);

            dataRptaTodosEquipos.DataActualPieOtros = dataRptaEquiposSO_BD.DataActualPie;
            dataRptaTodosEquipos.DataMesesAtrasPieOtros = dataRptaEquiposSO_BD.DataMesesAtrasPie;
            dataRptaTodosEquipos.DataActualSubdominiosPieOtros = dataRptaEquiposSO_BD.DataActualSubdominiosPie;
            dataRptaTodosEquipos.DataaMesesAtrasSubdominiosPieOtros = dataRptaEquiposSO_BD.DataaMesesAtrasSubdominiosPie;

            dataRptaTodosEquipos.DataActualSubdominiosPie.ForEach(x => { x.TipoDescripcion = HttpUtility.HtmlEncode(x.TipoDescripcion); });
            dataRptaTodosEquipos.DataaMesesAtrasSubdominiosPie.ForEach(x => { x.TipoDescripcion = HttpUtility.HtmlEncode(x.TipoDescripcion); });

            response = Request.CreateResponse(HttpStatusCode.OK, dataRptaTodosEquipos);
            return response;
        }

        [Route("Gerencial/Tecnologias/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GerencialTecnologiasCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosIndicadoresGerencialTecnologia dataRpta = new FiltrosIndicadoresGerencialTecnologia();

            var listaTipoTecnologias = ServiceManager<TipoDAO>.Provider.GetAllTipo();
            dataRpta.ListaTipoTecnologias = (from te in listaTipoTecnologias
                                             select new CustomAutocomplete()
                                             {
                                                 Id = te.Id.ToString(),
                                                 Descripcion = te.Nombre
                                             }).OrderBy(z => z.Descripcion).ToList();

            var nroMeses = Settings.Get<string>("Indicadores.Equipos.NroMeses");
            dataRpta.NroMesesConsulta = nroMeses.Split('|');


            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Gerencial/Tecnologias/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerencialTecnologiasReporte(FiltrosIndicadoresGerencialTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<IndicadoresDAO>.Provider.GetReporteIndicadoresGerencialTecnologias(filtros, true);

            dataRpta.DataTipoTecnologia.ForEach(x =>
            {
                x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                x.TipoEquipo = HttpUtility.HtmlEncode(x.TipoEquipo);
            });

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Gerencial/Aplicaciones/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GerencialAplicacionesCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosIndicadoresGerencialAplicacion dataRpta = new FiltrosIndicadoresGerencialAplicacion();

            var dataFiltros = ServiceManager<AplicacionDAO>.Provider.ListFiltros();


            dataRpta.ListaEstadoAplicacion = dataFiltros.EstadoAplicacion;
            dataRpta.ListaGestionadoPor = dataFiltros.GestionadoPor;


            var nroMeses = Settings.Get<string>("Indicadores.Equipos.NroMeses");
            dataRpta.NroMesesConsulta = nroMeses.Split('|');

            var nroNroSubdominios = Settings.Get<string>("Indicadores.Equipos.NroSubdominios");
            dataRpta.NroSubdominios = nroNroSubdominios.Split('|');


            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Gerencial/Aplicaciones/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerencialAplicacionesReporte(FiltrosIndicadoresGerencialAplicacion filtros)
        {
            HttpResponseMessage response = null;
            var dataRptaTodosEquipos = ServiceManager<IndicadoresDAO>.Provider.GetReporteIndicadoresGerencialAplicaciones(filtros, true);


            response = Request.CreateResponse(HttpStatusCode.OK, dataRptaTodosEquipos);
            return response;
        }



    }
}
