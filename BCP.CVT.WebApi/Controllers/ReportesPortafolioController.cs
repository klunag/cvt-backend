using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.WebApi.Auth;
using BCP.PAPP.Common.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/reportesportfolio")]
    public class ReportesPortafolioController : ApiController
    {
        [Route("reporte/estado/lists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsAdmin()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsAdmin(true);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }
        [Route("reporte/ValidarNroperiodos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFlagValidPeriodo(int nroPeriodos, string fechaBase, int frecuencia)
        {
            HttpResponseMessage response = null;
            int nroPeriodosBD = ServiceManager<ReportePortafolioDAO>.Provider.GetNroPeriodos(fechaBase, frecuencia);

            bool flag = nroPeriodos <= nroPeriodosBD;
            response = Request.CreateResponse(HttpStatusCode.OK, flag);
            return response;
        }
        #region Reporte Estado
        [Route("reporte/estado/listDivisionbyGerencia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListDivision(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsDivisionByGerente(id);
            response = Request.CreateResponse(HttpStatusCode.OK, objListas.Division);
            return response;
        }

        [Route("reporte/estado/listAreabyDivision")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListArea(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsAreaByDivision(id);
            response = Request.CreateResponse(HttpStatusCode.OK, objListas.Area);
            return response;
        }

        [Route("reporte/estado/portafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EstadoPortafolioReporte(FiltrosReporteEstadoPortafolio reporte)
        {
            HttpResponseMessage response = null;
            var lst = new List<int>();
            reporte.ListEstadoAplicacion = string.IsNullOrEmpty(reporte.EstadoAplicacion) ? lst : reporte.EstadoAplicacion.Split('|').Select(Int32.Parse).ToList();
            reporte.ListTipoActivo = string.IsNullOrEmpty(reporte.TipoActivo) ? lst : reporte.TipoActivo.Split('|').Select(Int32.Parse).ToList();
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteEstadoPortafolio(reporte);
            if (registros != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        [Route("reporte/estado/portafolio/roles")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EstadoPortafolioRolesReporte(FiltrosReporteEstadoPortafolio reporte)
        {
            HttpResponseMessage response = null;

            var registros = ServiceManager<ReportePortafolioDAO>.Provider.PortafolioListadoRolesAplicaciones(reporte);
            if (registros != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        [Route("reporte/estado/portafolio/exportar")]
        [HttpGet]
        public IHttpActionResult ExportarRelacionAplicaciones(int gerencia, int division, int area, string estado, string tipoActivo)
        {
            string filename = "";

            var reporte = new FiltrosReporteEstadoPortafolio()
            {
                GerenciaId = gerencia,
                DivisionId = division,
                AreaId = area,
                EstadoAplicacion = estado,
                TipoActivo = tipoActivo,
                FlgChange = false,
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            var lst = new List<int>();
            reporte.ListEstadoAplicacion = string.IsNullOrEmpty(estado) ? lst : estado.Split('|').Select(Int32.Parse).ToList();
            reporte.ListTipoActivo = string.IsNullOrEmpty(tipoActivo) ? lst : tipoActivo.Split('|').Select(Int32.Parse).ToList();

            var data = new ExportarData().ExportarReportePortafolioEstado_RelacionAplicaciones(reporte);
            filename = string.Format("ReportePortafolioDeEstadoBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("reporte/estado/portafolio/exportarData")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarDistribucionAplicaciones(int gerencia, int division, int area, string estado, string tipoActivo, int typeReport, string titulo)
        {
            string filename = "";

            var reporte = new FiltrosReporteEstadoPortafolio()
            {
                GerenciaId = gerencia,
                DivisionId = division,
                AreaId = area,
                EstadoAplicacion = estado ?? "",
                TipoActivo = tipoActivo ?? "",
                FlgChange = false,
                pageNumber = 1,
                pageSize = int.MaxValue,
                TituloReporte = titulo
                //TipoReporte = typeReport
            };

            var lst = new List<int>();
            reporte.ListEstadoAplicacion = string.IsNullOrEmpty(estado) ? lst : estado.Split('|').Select(Int32.Parse).ToList();
            reporte.ListTipoActivo = string.IsNullOrEmpty(tipoActivo) ? lst : tipoActivo.Split('|').Select(Int32.Parse).ToList();

            byte[] data = new byte[] { };

            switch (typeReport)
            {
                case (int)Cross.ETipoExportarReportePortafolioEstado.DistribucionAplicacionesBanco:
                    data = new ExportarData().ExportarReportePortafolioEstado_DistribucionAplicacionesBanco(reporte);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioEstado.DistribucionAplicaciones:
                    data = new ExportarData().ExportarReportePortafolioEstado_DistribucionAplicaciones(reporte);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioEstado.DistribucionAplicacionesByCategoria:
                    data = new ExportarData().ExportarReportePortafolioEstado_DistribucionAplicacionesByCategoria(reporte);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioEstado.DistribucionAplicacionesByCriticidad:
                    data = new ExportarData().ExportarReportePortafolioEstado_DistribucionAplicacionesByCriticidad(reporte);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioEstado.DistribucionAplicacionesByEstado:
                    data = new ExportarData().ExportarReportePortafolioEstado_DistribucionAplicacionesByEstado(reporte);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioEstado.SaludAplicacion:
                    data = new ExportarData().ExportarReportePortafolioEstado_SaludAplicacion(reporte);
                    break;
            }


            filename = string.Format("ReportePortafolioDeEstadoBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        #endregion

        #region Reporte Campos 
        [Route("reporte/campos/combosFiltros")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListasReporteCampos()
        {
            HttpResponseMessage response = null;
            var objListas1 = ServiceManager<CommonDAO>.Provider.GetListsAdmin(true);

            var objListas3 = ServiceManager<ReportePortafolioDAO>.Provider.GetListAgrupadoPor();

            var valoresIncluir = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("REPORTE_CAMPO_ESPECIFICO_OPCIONES");
            List<CustomAutocompleteApplication> objListasFiltro = null;
            if (valoresIncluir != null)
            {
                var listaValoresIncluir = valoresIncluir.Valor.Split('|').ToList();
                objListasFiltro = objListas3.Where(x => listaValoresIncluir.Contains(x.Descripcion))
                  .Select(z => new CustomAutocompleteApplication { Descripcion = z.Descripcion, Id = z.Id }).ToList();
            }

            var rpta = new FilterReporteCampos
            {
                FilterAdmin = objListas1,
                ListaCamposAgrupar = objListasFiltro
            };

            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }

        [Route("reporte/campos/combosFiltros/valoresFiltroPor")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListaValoresFiltroPor(int idTablaFiltrar)
        {
            HttpResponseMessage response = null;

            var rpta = ServiceManager<ReportePortafolioDAO>.Provider.GetListValoresFiltros(idTablaFiltrar);


            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }

        [Route("reporte/campos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetReporteCampos(FiltrosReporteCamposPortafolio reporte)
        {
            HttpResponseMessage response = null;

            if (reporte.FechaDesde == DateTime.MinValue)
            {
                reporte.FechaDesde = reporte.FechaHasta;
                reporte.FechaHasta = DateTime.MinValue;
            }
            var data = ServiceManager<ReportePortafolioDAO>.Provider.ReportePortafolioCampos(reporte);
            if (data.Columns.Count <= 3)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, "");
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, data);
            }



            return response;
        }

        [Route("reporte/campos/exportarData")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarReporteCampos(string gerencia, string division, string area, string unidad, DateTime? fechaDesde,
            DateTime fechaHasta, int frecuencia, int nroPeriodos, int agrupadoPor, string agruparPorValores, int filtrarPor, string filtrarPorValores, string periodoTiempo)
        {
            string filename = "";

            var fechaDesdeFinal = fechaDesde == DateTime.MinValue || !fechaDesde.HasValue ? fechaHasta : fechaDesde ?? DateTime.MinValue;
            var fechaHastaFinal = fechaDesde == DateTime.MinValue || !fechaDesde.HasValue ? DateTime.MinValue : fechaHasta;


            var reporte = new FiltrosReporteCamposPortafolio()
            {
                ListGerencia = gerencia ?? "",
                ListDivision = division ?? "",
                ListArea = area ?? "",
                ListUnidad = unidad ?? "",
                FechaDesde = fechaDesdeFinal,
                FechaHasta = fechaHastaFinal,
                Frecuencia = frecuencia,
                NroPeriodos = nroPeriodos,
                AgruparPor = agrupadoPor,
                AgruparPorValores = agruparPorValores ?? "",
                FiltrarPor = filtrarPor,
                FiltrarPorValores = filtrarPorValores ?? "",

            };
            var fechaConsulta = GenerarTextPeriodoBusqueda(nroPeriodos, fechaDesdeFinal, fechaHastaFinal, periodoTiempo);

            var data = new ExportarData().ExportarReportePortafolioCampos_Reporte(reporte, fechaConsulta, periodoTiempo);

            filename = string.Format("ReportePortafolioPorCampo_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { data = data, name = filename });
        }

        #endregion

        #region Reporte Variacion
        [Route("reporte/variacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetReporteVariacion(FiltrosReporteVariacionPortafolio reporte)
        {
            HttpResponseMessage response = null;

            if (reporte.FechaDesde == DateTime.MinValue)
            {
                reporte.FechaDesde = reporte.FechaHasta;
                reporte.FechaHasta = DateTime.MinValue;
            }

            var solicitudesCreadas = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioVariacion_SolicitudesCreadas(reporte);
            var solicitudesEliminadas = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioVariacion_SolicitudesEliminadas(reporte);
            var solicitudesCreadasEliminadas = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(reporte);
            var Estados = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioVariacion_Estados(reporte);
            var distribucionesXGerencia = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioVariacion_DistrXGerencia(reporte);

            if (solicitudesCreadas != null || solicitudesEliminadas != null || solicitudesCreadasEliminadas != null || Estados != null || distribucionesXGerencia != null)
            {
                var dataReportDistribuciones = distribucionesXGerencia.Solicitudes;
                var lstDistribuciones = new List<DistribucionReponse>();

                if (dataReportDistribuciones.Any())
                {
                    var lstIdsGerencia = dataReportDistribuciones.Where(z => z.GrupoId != 0).GroupBy(x => x.GrupoId).Select(g => g.Key);
                    var lstIdsPeriodosSinDatos = dataReportDistribuciones.Where(z => z.GrupoId == 0).ToList();

                    //var nose = dataReportDistribuciones;
                    foreach (var id in lstIdsGerencia)
                    {
                        var lstGerencia = new List<ReportePortafolioGrafico>();
                        lstGerencia = dataReportDistribuciones.Where(x => x.GrupoId == id).Union(lstIdsPeriodosSinDatos).ToList();

                        var objDistribucion = new DistribucionReponse();
                        objDistribucion.GerenciaId = id;
                        objDistribucion.Gerencia = lstGerencia.FirstOrDefault().Grupo;
                        objDistribucion.Chart = lstGerencia;

                        lstDistribuciones.Add(objDistribucion);
                    }
                }

                response = Request.CreateResponse(HttpStatusCode.OK, new { solicitudesCreadas, solicitudesEliminadas, solicitudesCreadasEliminadas, Estados, lstDistribuciones });
            }

            return response;
        }

        [Route("reporte/variacion/exportarData")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarReporteVariacion(string tipoActivo, string gestionadoPor, string estado, int frecuencia, DateTime? fechaDesde, DateTime fechaHasta,
            int periodo, int typeReport, int hidden, string nomGerencia, string periodoTiempo)
        {
            string filename = ""; 
            var fechaDesdeFinal = fechaDesde == DateTime.MinValue || !fechaDesde.HasValue ? fechaHasta : fechaDesde ?? DateTime.MinValue;
            var fechaHastaFinal = fechaDesde == DateTime.MinValue || !fechaDesde.HasValue ? DateTime.MinValue : fechaHasta;

            var reporte = new FiltrosReporteVariacionPortafolio()
            {
                ListTipoActivo = tipoActivo ?? "",
                ListGestionadoPor = gestionadoPor ?? "",
                ListEstadoAplicacion = estado ?? "",
                Frecuencia = frecuencia,
                FechaDesde = fechaDesdeFinal,
                FechaHasta = fechaHastaFinal,
                nroPeriodos = periodo,
                periodoTiempo = periodoTiempo
            };

            byte[] data = new byte[] { };
            var nomReporte = "";
            var fechaConsulta = GenerarTextPeriodoBusqueda(periodo, fechaDesdeFinal, fechaHastaFinal, periodoTiempo);

            switch (typeReport)
            {
                case (int)Cross.ETipoExportarReportePortafolioVariacion.SolicitudesCreadas:
                    data = new ExportarData().ExportarReportePortafolioVariacion_SolicitudesCreadas(reporte, fechaConsulta, periodoTiempo);
                    nomReporte = "Creadas";
                    break;
                case (int)Cross.ETipoExportarReportePortafolioVariacion.SolicitudesEliminadas:
                    data = new ExportarData().ExportarReportePortafolioVariacion_SolicitudesEliminadas(reporte, fechaConsulta, periodoTiempo);
                    nomReporte = "Eliminadas";
                    break;
                case (int)Cross.ETipoExportarReportePortafolioVariacion.SolicitudesCreadasEliminadas:
                    data = new ExportarData().ExportarReportePortafolioVariacion_SolicitudesCreadasEliminadas(reporte, fechaConsulta, periodoTiempo);
                    nomReporte = string.Format("CreadasEliminadasPor{0}", periodoTiempo);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioVariacion.SolicitudesXEstado:
                    data = new ExportarData().ExportarReportePortafolioVariacion_SolicitudesXEstado(reporte, fechaConsulta, periodoTiempo);
                    nomReporte = string.Format("EstadosPor{0}", periodoTiempo);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioVariacion.DistribucionesXGerencia:
                    data = new ExportarData().ExportarReportePortafolioVariacion_DistribucionXGerencia(reporte, hidden, nomGerencia, fechaConsulta, periodoTiempo);
                    nomReporte = string.Format("DistribucionesXGerencia_{0}", nomGerencia);
                    break;
            }


            filename = string.Format("ReportePortafolioDeVariacionBCP_{1}_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"), nomReporte);

            return Ok(new { data = data, name = filename });
        }
        #endregion

        #region Reporte Pedidos

        [Route("reporte/portafolio/listGerencias")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListGerencia()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsAdminConditions(false, false, false, true);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("reporte/pedidos/listDivisionbyGerencia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListDivisionMulti(string strIds)
        {
            HttpResponseMessage response = null;
            var ids = Array.ConvertAll(strIds.Split('|'), s => int.Parse(s));
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsDivisionByGerenteMulti(ids);
            response = Request.CreateResponse(HttpStatusCode.OK, objListas.Division);
            return response;
        }

        [Route("reporte/pedidos/listAreabyDivision")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListAreaMulti(string strIds)
        {
            HttpResponseMessage response = null;
            var ids = Array.ConvertAll(strIds.Split('|'), s => int.Parse(s));
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsAreaByDivisionMulti(ids);
            response = Request.CreateResponse(HttpStatusCode.OK, objListas.Area);
            return response;
        }

        [Route("reporte/pedidos/listUnidadesbyArea")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListUnidadesMulti(string strIds)
        {
            HttpResponseMessage response = null;
            var ids = Array.ConvertAll(strIds.Split('|'), s => int.Parse(s));
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsUnidadesByAreaMulti(ids);
            response = Request.CreateResponse(HttpStatusCode.OK, objListas.Area);
            return response;
        }

        [Route("reporte/pedidos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetReportePedidos(FiltrosReportePedidosPortafolio reporte)
        {
            HttpResponseMessage response = null;
            var lstCampos = new List<string>();
            var estadoAtencion = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_DistrByTipoAtencionAcumulada(reporte);
            var estadoAtencionXTiempo = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_DistrByTipoAtencionPeriodo(reporte);
            var estadoAtencionConsultas = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_Consultas(reporte);
            var consultasPortafolio = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_ConsultasPortafolio(reporte);
            var camposMasActualizados = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_CamposMasRequeridos(reporte);
            var SLA = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_SLA(reporte);
            var registroAPP = ServiceManager<ReportePortafolioDAO>.Provider.GetReportePortafolioPedido_RegistroAPP(reporte);

            if (estadoAtencion != null || estadoAtencionXTiempo != null || consultasPortafolio != null || camposMasActualizados != null
                || SLA != null || registroAPP != null || estadoAtencionConsultas != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new { estadoAtencion, estadoAtencionXTiempo, estadoAtencionConsultas,consultasPortafolio, camposMasActualizados, SLA, registroAPP });
            }

            return response;
        }

        [Route("reporte/pedidos/exportarData")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarReportePedidos(string gerencia, string division, string area, string unidad, string tipoActivo, string gestionadoPor,
            DateTime fechaDesde, DateTime fechaHasta, int typeReport)
        {
            string filename = "";

            var reporte = new FiltrosReportePedidosPortafolio()
            {
                ListGerencia = gerencia ?? "",
                ListDivision = division ?? "",
                ListArea = area ?? "",
                ListUnidad = unidad ?? "",
                ListTipoActivo = tipoActivo ?? "",
                ListGestionadoPor = gestionadoPor ?? "",
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
            };

            var fechaConsulta = GenerarTextPeriodoBusqueda(0, fechaDesde, fechaHasta, "Mes");

            byte[] data = new byte[] { };

            switch (typeReport)
            {
                case (int)Cross.ETipoExportarReportePortafolioPedidos.EstadoAtencion:
                    data = new ExportarData().ExportarReportePortafolioPedido_EstadoAtencion(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.EstadoAtencionXTiempo:
                    data = new ExportarData().ExportarReportePortafolioPedido_EstadoAtencionXTiempo(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.ConsultasPortafolio:
                    data = new ExportarData().ExportarReportePortafolioPedido_ConsultasPortafolio(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.RegistroApp:
                    data = new ExportarData().ExportarReportePortafolioPedido_RegistroAPP(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.CamposMasActualizados:
                    data = new ExportarData().ExportarReportePortafolioPedido_CamposRequeridos(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.ReporteSLA:
                    data = new ExportarData().ExportarReportePortafolioPedido_SLA(reporte, fechaConsulta);
                    break;
                case (int)Cross.ETipoExportarReportePortafolioPedidos.Consultas:
                    data = new ExportarData().ExportarReportePortafolioPedido_Consultas(reporte, fechaConsulta);
                    break;
            }


            filename = string.Format("ReportePortafolioDePedidosBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { data = data, name = filename});
        }
        #endregion

        #region Reporte Cambio o baja Responsable GDH
        [Route("reporte/CambioBajaResponsablesGDH")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetReporteCambioBajaResponsables(FiltrosReporteCambioBajaResponsableGDH reporte)
        {
            HttpResponseMessage response = null;
            var lstCampos = new List<string>();
            var cambioResponsablesGDH = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteCambioBajaResponsableGDH(reporte);
            if (cambioResponsablesGDH != null )
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new { cambioResponsablesGDH });
            }

            return response;
        }

        [Route("reporte/CambioBajaResponsablesGDH/portafolio/exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarRelacionCambiosBajaResponsablesGDH(string fechaInicio, string fechaFin, string matricula, string IdAplicacion, string tipoCambio)
        {
            string filename = "";
            var reporte = new FiltrosReporteCambioBajaResponsableGDH()
            {
                fechaInicio = fechaInicio,
                fechaFin = fechaFin,
                matricula = (matricula == null) ? "" : matricula,
                IdAplicacion = (IdAplicacion == null) ? "" : IdAplicacion,
                tipoCambio = (tipoCambio == null) ? "" : tipoCambio,
                FlgChange = false,
                pageNumber = 1,
                pageSize = int.MaxValue,

            };
            var lst = new List<int>();

            var data = new ExportarData().ExportarReportePortafolioCambioBajaResponsablesGDH(reporte);
            filename = string.Format("ReportePortafolioCambioBajaResponsablesGDH_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { data = data, name = filename });
        }
        #endregion
        private string GenerarTextPeriodoBusqueda(int nroPeriodos, DateTime? fechaDesde, DateTime fechaHasta, string periodoTiempo)
        {
            var fechaConsulta = nroPeriodos == 0 ?
                                 string.Format("Por {2} - Desde {0} hasta {1}", fechaDesde.Value.ToString("dd/MM/yyyy"), fechaHasta.ToString("dd/MM/yyyy"), periodoTiempo)
                                 : string.Format("Por {2} - Fecha base: {1}, nro. periodos: {0}", nroPeriodos, fechaHasta.ToString("dd/MM/yyyy"), periodoTiempo);
            return fechaConsulta;
        }
    }
}
