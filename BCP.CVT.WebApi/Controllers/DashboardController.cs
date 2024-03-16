using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : BaseController
    {
        [Route("Aplicacion/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage AplicacionCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosDashboardAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.ListFiltros();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Aplicacion/ListarCombosReporteAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ReporteAplicacionCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosReporteAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.GetFiltrosReporteAplicacion();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Aplicacion/ListarCombos/Resumen")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage AplicacionCargarCombosResumen()
        {
            HttpResponseMessage response = null;
            FiltrosDashboardAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.ListFiltrosResumen();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }


        [Route("Aplicacion/ListarCombos/GestionadoPor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AplicacionCargarCombosGestionadoPor(FiltrosDashboardAplicacion filtros)
        {
            HttpResponseMessage response = null;
            filtros.GestionadoPorFiltrar = filtros.GestionadoPorFiltrar == null ? new List<string>() : filtros.GestionadoPorFiltrar;
            FiltrosDashboardAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.ListFiltrosXGestionadoPor(filtros.GestionadoPorFiltrar);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Aplicacion/ListarCombos/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AplicacionCargarComboAplicaciones(FiltrosDashboardAplicacion filtros)
        {
            HttpResponseMessage response = null;
            FiltrosDashboardAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.ListAplicacionesXFiltros(filtros);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }


        [Route("Aplicacion/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AplicacionReporte(FiltrosDashboardAplicacion filtros)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetReporte(filtros);

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage TecnologiaCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosDashboardTecnologia dataRpta = ServiceManager<TecnologiaDAO>.Provider.ListFiltrosDashboard();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ListarCombos/Subdominio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaCargarCombosSubdominio(List<int> idsSubdominio)
        {
            HttpResponseMessage response = null;
            FiltrosDashboardTecnologia dataRpta = ServiceManager<TecnologiaDAO>.Provider.ListFiltrosDashboardXSubdominio(idsSubdominio);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ListarCombos/Dominio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaCargarCombosDominio(List<int> idsDominio)
        {
            HttpResponseMessage response = null;
            FiltrosDashboardTecnologia dataRpta = ServiceManager<TecnologiaDAO>.Provider.ListFiltrosDashboardXDominio(idsDominio);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ListarCombos/Equipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaCargarCombosEquipos(List<int> idsGestionados)
        {
            HttpResponseMessage response = null;
            FiltrosDashboardTecnologia dataRpta = ServiceManager<TecnologiaDAO>.Provider.ListFiltrosDashboardXGestionadoPor(idsGestionados);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ListarCombos/Subdominio/Fabricante")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaCargarCombosSubdominioFabricante(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            FiltrosDashboardTecnologia dataRpta = ServiceManager<TecnologiaDAO>.Provider.ListFiltrosDashboardXSubdominioXFabricante(filtros.SubdominioFiltrar, filtros.FabricanteFiltrar);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaReporte(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<TecnologiaDAO>.Provider.GetReport(filtros, false);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ReporteTecnologiaInstalaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaInstalacionesReporte(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var rptInstalaciones = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaInstalaciones(filtros);
            var rptInstalacionesTipo = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaInstalaciones_Tipo(filtros);
            var rptInstalacionesTipoEquipo = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaInstalaciones_Tipo_Equipo(filtros);
            var rptInstalacionesEquipoAgrupacion = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaInstalaciones_Equipo_Agrupacion(filtros);

            var dataRpta = new
            {
                rptInstalaciones,
                rptInstalacionesTipo,
                rptInstalacionesTipoEquipo,
                rptInstalacionesEquipoAgrupacion
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/ReporteTecnologiaInstalaciones/DetalleSubdominio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaInstalacionesReporteSubdominio(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var rptBar = new List<PieChart>();
            //string Subdominio = ServiceManager<SubdominioDAO>.Provider.GetSubdomById(filtros.SubdominioId).Nombre;
            //string TipoEquipo = ServiceManager<EquipoDAO>.Provider.GetTipoEquipoById(filtros.TipoEquipoId).Nombre;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var rptInstalacionesSO = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaInstalaciones_SO(filtros);
            if (rptInstalacionesSO != null && rptInstalacionesSO.Count() > 0)
            {
                var rptPie = (from x in rptInstalacionesSO
                              group x by new { x.Fabricante, x.Nombre } into grp
                              select new PieChart
                              {
                                  categoria = grp.Key.Fabricante + " " + grp.Key.Nombre,
                                  cantidad = grp.Sum(x => x.Total),
                                  color = string.Empty
                              });

                rptBar = rptPie.Take(10).ToList();
                rptBar.Add(new PieChart
                {
                    categoria = "Otros",
                    cantidad = rptPie.Skip(10).Sum(x => x.cantidad),
                    color = string.Empty
                });
            }

            var dataRpta = new
            {
                rptInstalacionesSO,
                //Subdominio,
                //TipoEquipo,
                rptBar
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Tecnologia/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologia(string dominio, string subdominio, string familia, string fabricante, string clave, string tipoEquipo, string subsidiaria, string fecha)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarKPITecnologia(dominio, subdominio, familia, fabricante, clave, tipoEquipo, subsidiaria, fecha);
            nomArchivo = string.Format("ListaKPITecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Estandares/ExisteArchivoConsolidado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteArchivoConsolidado(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ReporteDAO>.Provider.ExisteArchivoConsolidado(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("Estandares/DescargarConsolidado")]
        [HttpGet]
        public HttpResponseMessage DescargarConsolidado(int tipoId)
        {
            string filename = string.Empty;
            var buffer = ServiceManager<ReporteDAO>.Provider.ObtenerArrConsolidado(tipoId);
            string filePath = ServiceManager<ReporteDAO>.Provider.GetRutaConsolidadoByTipo(tipoId);
            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            filename = Utilitarios.GetEnumDescription2((ENombreConsolidado)(tipoId));

            int bytesRead = 0;
            FileStream streamToReadFrom = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            httpResponseMessage.Content = new PushStreamContent((stream, content, context) =>
            {
                while ((bytesRead = streamToReadFrom.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }

                stream.Close();
            });

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = filename;
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }


        [Route("Equipo/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EquipoCargarCombos()
        {
            HttpResponseMessage response = null;
            FiltrosDashboardEquipo dataRpta = ServiceManager<EquipoDAO>.Provider.ListFiltrosDashboard();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Equipo/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EquipoReporte(FiltrosDashboardEquipo filtros)
        {
            HttpResponseMessage response = null;

            DashboardEquipoData dashboardBase = new DashboardEquipoData();

            var dataFiltros = ServiceManager<EquipoDAO>.Provider.ListFiltrosDashboardByEquipo(filtros.EquipoId);
            var dataAplicaciones = ServiceManager<EquipoDAO>.Provider.GetReportAplicacionesXEquipo(filtros);
            var dataTecnologias = ServiceManager<EquipoDAO>.Provider.GetReportTecnologiasXEquipo(filtros);
            var dataTecnologiasNoRegistradas = ServiceManager<EquipoDAO>.Provider.GetTecnologiasNoRegistradasXEquipo(filtros);

            dataFiltros.AplicacionEstado = dataAplicaciones.Select(x => x.FiltroEstado).Distinct().Select(y => new CustomAutocomplete { Id = y, Descripcion = y }).ToList();

            dashboardBase.DataFiltros = dataFiltros;
            dashboardBase.DataPlotly = dataAplicaciones;
            dashboardBase.DataPie = dataTecnologias;
            dashboardBase.DataTecnologiasNoRegistradas = dataTecnologiasNoRegistradas;


            var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
            var proyeccionMeses1 = paramProyeccion1 != null ? paramProyeccion1.Valor : "12";
            var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
            var proyeccionMeses2 = paramProyeccion2 != null ? paramProyeccion2.Valor : "24";

            dashboardBase.Proyeccion1Meses = proyeccionMeses1;
            dashboardBase.Proyeccion2Meses = proyeccionMeses2;

            response = Request.CreateResponse(HttpStatusCode.OK, dashboardBase);
            return response;
        }

        [Route("Equipo/ReporteTecnologias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EquipoReporteTecnologias(FiltrosDashboardEquipo filtros)
        {
            HttpResponseMessage response = null;

            DashboardEquipoData dashboardBase = null;

            var dataTecnologias = ServiceManager<EquipoDAO>.Provider.GetReportTecnologiasXEquipo(filtros);

            dashboardBase.DataPie = dataTecnologias;

            response = Request.CreateResponse(HttpStatusCode.OK, dashboardBase);
            return response;
        }

        [Route("Equipo/GetEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro(string filtro, int idTipoEquipo)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, idTipoEquipo);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("Equipo/GetEquipoById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoById(int idEquipo)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoDetalleById(idEquipo);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("Principal/RenovacionTecnologicaTI")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage RenovacionTecnologicaTI(FiltrosDashboardRenovacionTecnologicaTI filtros)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<GraficoDAO>.Provider.GetDataGraficoRenovacionTecnologica(filtros.Anio, filtros.Mes, 0);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Reportes/AplicacionTecnologia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AplicacionTecnologia(PaginaReporteGerencia filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            filtros.DominioId = filtros.DominioIds == null ? string.Empty : string.Join("|", filtros.DominioIds);
            filtros.SubdominioId = filtros.SubdominioIds == null ? string.Empty : string.Join("|", filtros.SubdominioIds);
            var registros = ServiceManager<ReporteDAO>.Provider.GetAplicacionTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.Aplicacion = HttpUtility.HtmlEncode(x.Aplicacion);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/AplicacionTecnologia/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarAplicacionTecnologia(string Aplicacion
            , string Tecnologia
            , string DominioId
            , string SubdominioId
            , string sortName
            , string sortOrder
            , string fecha)
        {
            string nomArchivo = "";

            var filtros = new PaginaReporteGerencia();
            filtros.Aplicacion = Aplicacion == null ? "" : Aplicacion;
            filtros.Tecnologia = Tecnologia == null ? "" : Tecnologia;
            filtros.DominioId = DominioId == null ? string.Empty : DominioId;
            filtros.SubdominioId = SubdominioId == null ? string.Empty : SubdominioId;
            filtros.sortName = sortName;
            filtros.sortOrder = sortOrder;
            filtros.pageNumber = 1;
            filtros.pageSize = int.MaxValue;
            filtros.Fecha = fecha;

            var data = new ExportarData().ExportarReporteAplicacionTecnologia(filtros);
            nomArchivo = string.Format("ReporteAplicacionTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("AplicacionTecnologia/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            //var listFuente = Utilitarios.EnumToList<Fuente>();
            //var listFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>();
            //var listEstadoObs = Utilitarios.EnumToList<ETecnologiaEstado>();
            //var listTipo = Utilitarios.EnumToList<ETecnologiaTipo>();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubdominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            //var listTipoTec = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);
            //var listFechaFinSoporte = Utilitarios.EnumToList<EFechaFinSoporte>();
            //var listEstadoObsolescencia = Utilitarios.EnumToList<>();

            var dataRpta = new
            {
                //Fuente = listFuente.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //FechaCalculo = listFechaCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //EstadoObs = listEstadoObs.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }),
                //Tipo = listTipo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }),
                Dominio = listDominio,
                Subdominio = listSubdominio
                //TipoTec = listTipoTec,              
                //FechaFinSoporte = listFechaFinSoporte.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Reportes/GerenciaDivisionDetallado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerenciaDivisionDetallado(PaginaReporteGerencia filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetGerenciaDivisionDetalle(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GerenciaDivision")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerenciaDivision(PaginaReporteGerencia filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetGerenciaDivision(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Aplicacion = HttpUtility.HtmlEncode(x.Aplicacion);
                    x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
                    x.GestionadoPor = HttpUtility.HtmlEncode(x.GestionadoPor);
                    x.GerenciaCentral = HttpUtility.HtmlEncode(x.GerenciaCentral);
                    x.Division = HttpUtility.HtmlEncode(x.Division);
                    x.Area = HttpUtility.HtmlEncode(x.Area);
                    x.Unidad = HttpUtility.HtmlEncode(x.Unidad);
                    x.DetalleCriticidad  = HttpUtility.HtmlEncode(x.DetalleCriticidad);
                    x.RoadMap = HttpUtility.HtmlEncode(x.RoadMap);
                    x.ExpertoEspecialista = HttpUtility.HtmlEncode(x.ExpertoEspecialista);
                    x.Owner_LiderUsuario_ProductOwner = HttpUtility.HtmlEncode(x.Owner_LiderUsuario_ProductOwner);
                    x.JefeEquipo_ExpertoAplicacion = HttpUtility.HtmlEncode(x.JefeEquipo_ExpertoAplicacion);
                    x.BrokerSistemas = HttpUtility.HtmlEncode(x.BrokerSistemas);
                    x.TribeTechnicalLead = HttpUtility.HtmlEncode(x.TribeTechnicalLead);
                    x.ListaPCI = HttpUtility.HtmlEncode(x.ListaPCI);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GerenciaDivisionConsultor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerenciaDivisionConsultor(PaginaReporteGerencia filtros)
        {
            var user = TokenGenerator.GetCurrentUser();
            filtros.Matricula = user.Matricula;
            filtros.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetGerenciaDivisionConsultor(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GerenciaDivision/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivision(string gerencia
            , string division
            , string gestionado
            , string aplicacion
            , string jefe
            , string owner
            , string experto
            , string estado
            , string sortName
            , string sortOrder
            , string area
            , string unidad
            , string tipoActivo
            , string gestor
            , string fecha
            , string ttl
            , string broker
            , string clasificacion
            , string subclasificacion
            , string unidadFondeo
            , bool flagProyeccion
            , string fechaProyeccion)
        {
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;

            string nomArchivo = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? "" : gerencia;
            division = string.IsNullOrEmpty(division) ? "" : division;
            gestionado = string.IsNullOrEmpty(gestionado) ? string.Empty : gestionado;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? "" : aplicacion;
            jefe = string.IsNullOrEmpty(jefe) ? "" : jefe;
            owner = string.IsNullOrEmpty(owner) ? "" : owner;
            experto = string.IsNullOrEmpty(experto) ? "" : experto;
            tipoActivo = string.IsNullOrEmpty(tipoActivo) ? "" : tipoActivo;
            gestor = string.IsNullOrEmpty(gestor) ? "" : gestor;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            ttl = string.IsNullOrEmpty(ttl) ? string.Empty : ttl;
            broker = string.IsNullOrEmpty(broker) ? string.Empty : broker;
            clasificacion = string.IsNullOrEmpty(clasificacion) ? string.Empty : clasificacion;
            subclasificacion = string.IsNullOrEmpty(subclasificacion) ? string.Empty : subclasificacion;
            perfil = string.IsNullOrEmpty(perfil) ? string.Empty : perfil;
            unidadFondeo = string.IsNullOrEmpty(unidadFondeo) ? string.Empty : unidadFondeo;
            var data = new ExportarData().ExportarReporteGerenciaDivisiones(gerencia, division, gestionado, aplicacion, jefe, owner, experto, estado, sortName, sortOrder, area, unidad, tipoActivo, gestor, fecha, ttl, broker, clasificacion, subclasificacion, perfil, unidadFondeo, flagProyeccion, fechaProyeccion);
            nomArchivo = string.Format("ReporteGerenciaDivision_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/GerenciaDivision/ExportarResponsable")]
        [HttpGet]
        public IHttpActionResult ExportarResponsable(string matricula)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarReporteGerenciaResponsable(matricula);
            nomArchivo = string.Format("ReporteObsolescenciaAplicaciones_{0}.xlsx", matricula);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/GerenciaDivisionConsultor/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivisionConsultor(string aplicacion, string estado, string sortName
            , string sortOrder, string fecha, bool flagProyeccion, string fechaProyeccion)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            var perfilId = user.PerfilId;

            string nomArchivo = "";

            aplicacion = string.IsNullOrEmpty(aplicacion) ? "" : aplicacion;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;

            var data = new ExportarData().ExportarReporteGerenciaDivisionesConsultor(aplicacion, estado, sortName, sortOrder, fecha, matricula, perfilId, flagProyeccion, fechaProyeccion);
            nomArchivo = string.Format("ReporteAplicacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/SinRelaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage SinRelaciones(PaginaReporteHuerfanos filtros)
        {
            if (filtros.TipoEquipoFiltrar != null)
            {
                var ids = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());
                filtros.TipoEquipoToString = ids;
            }

            HttpResponseMessage response = null;
            var totalRows = 0;
            DateTime fechaActual = DateTime.Now;
            try
            {
                fechaActual = DateTime.ParseExact(filtros.FechaConsulta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                fechaActual = DateTime.Now;
            }

            var parametroSO = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("SUBDOMINIO_SISTEMA_OPERATIVO").Valor;

            var registros = ServiceManager<ReporteDAO>.Provider.GetServidoresHuerfanos(filtros, fechaActual, int.Parse(parametroSO), out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Equipo = HttpUtility.HtmlEncode(x.Equipo);


                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/Tecnologias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage Tecnologias(PaginaReporteTecnologiasCustom filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            var registros = ServiceManager<ReporteDAO>.Provider.GetTecnologias(filtros, out totalRows, false);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                    x.Familia = HttpUtility.HtmlEncode(x.Familia);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/Tecnologias/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologias(string Dominio
            , string Subdominio
            , string EstadoAprobacion
            , string Familia
            , string Tipo
            , string EstadoObsolescencia
            , string Clave
            , string sortName
            , string sortOrder)
        {
            string nomArchivo = "";
            var filtros = new PaginaReporteTecnologiasCustom();
            filtros.DominioIds = Dominio == null ? string.Empty : Dominio;
            filtros.SubdominioIds = Subdominio == null ? string.Empty : Subdominio;
            filtros.EstadoAprobacionIds = EstadoAprobacion == null ? string.Empty : EstadoAprobacion;
            filtros.Familia = string.IsNullOrEmpty(Familia) ? string.Empty : Familia;  //Familia;
            filtros.Tipos = Tipo == null ? string.Empty : Tipo;
            filtros.EstadoObsolescencias = EstadoObsolescencia == null ? string.Empty : EstadoObsolescencia;
            filtros.Clave = string.IsNullOrEmpty(Clave) ? string.Empty : Clave;  //Clave == null ? "" : Clave;
            filtros.sortName = sortName;
            filtros.sortOrder = sortOrder;
            filtros.pageNumber = 1;
            filtros.pageSize = int.MaxValue;

            var data = new ExportarData().ExportarReporteTecnologia(filtros);
            nomArchivo = string.Format("ReporteTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/Tecnologias/SinRelaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiasSinRelaciones(PaginaReporteTecnologiasCustom filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            var registros = ServiceManager<ReporteDAO>.Provider.GetTecnologiasSinRelaciones(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        //[Route("Reportes/Tecnologias/SinRelaciones/Exportar")]
        //[HttpGet]
        //public HttpResponseMessage TecnologiasSinRelacionesExportar(int dominio, int subdominio, int estadoAprobacion, string familia, int tipo, int estadoObsolescencia, string clave)
        //{
        //    string nomArchivo = "";

        //    clave = string.IsNullOrEmpty(clave) ? string.Empty : clave;

        //    var data = new ExportarData().ExportarTecnologiasSinRelaciones(dominio, subdominio, estadoAprobacion, familia, tipo, estadoObsolescencia, clave);
        //    nomArchivo = string.Format("ReporteTecnologiasSinRelaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}

        [Route("Reportes/Tecnologias/SinRelaciones/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiasSinRelaciones(string dominio
            , string subdominio
            , string estadoAprobacion
            , string familia
            , string tipo
            , string estadoObsolescencia
            , string clave
            , string sortName
            , string sortOrder
            , string fecha)
        {
            string nomArchivo = "";
            var filtros = new PaginaReporteTecnologiasCustom();
            filtros.DominioIds = dominio == null ? string.Empty : dominio;
            filtros.SubdominioIds = subdominio == null ? string.Empty : subdominio;
            filtros.EstadoAprobacionIds = estadoAprobacion == null ? string.Empty : estadoAprobacion;
            filtros.Familia = string.IsNullOrEmpty(familia) ? string.Empty : familia;
            filtros.Tipos = tipo == null ? string.Empty : tipo;
            filtros.EstadoObsolescencias = estadoObsolescencia == null ? string.Empty : estadoObsolescencia;
            filtros.Clave = string.IsNullOrEmpty(clave) ? string.Empty : clave;
            filtros.sortName = sortName;
            filtros.sortOrder = sortOrder;
            filtros.pageNumber = 1;
            filtros.pageSize = int.MaxValue;
            filtros.Fecha = fecha;

            var data = new ExportarData().ExportarReporteTecnologiasSinRelaciones(filtros);
            nomArchivo = string.Format("ReporteTecnologiaSinRelacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/Tecnologias/Evolucion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiasEvolucion(PaginaReporteTecnologias filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            var registros = ServiceManager<ReporteDAO>.Provider.GetEvolucionTecnologia(filtros.Tecnologia, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GraficoSubdominios")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage KPISubdominios(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            DateTime fechaActual = DateTime.Now;
            try
            {
                fechaActual = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                fechaActual = filtros.FechaFiltro;
            }

            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipoTecnologias = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());

            var registros = ServiceManager<ReporteDAO>.Provider.GetGrafico(tipos, reds, ids, fechaActual, tipoTecnologias);

            int idNoVigente = ((int)EDashboardEstadoTecnologia.NoVigente);
            int idVigente = ((int)EDashboardEstadoTecnologia.Vigente);
            int idCercaFin = ((int)EDashboardEstadoTecnologia.CercaFinSoporte);
            string colorNoVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idNoVigente);
            string colorVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idVigente);
            string colorCerca = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idCercaFin);

            if (registros != null)
            {
                var retorno = new List<CustomAutocomplete>();
                retorno.Add(new CustomAutocomplete()
                {
                    Color = colorVigente,
                    Descripcion = "Vigentes",
                    Id = "Vigentes",
                    TipoDescripcion = "Vigentes",
                    Valor = registros.TotalVerde
                });
                retorno.Add(new CustomAutocomplete()
                {
                    Color = colorNoVigente,
                    Descripcion = "Obsoletos",
                    Id = "Obsoletos",
                    TipoDescripcion = "Obsoletos",
                    Valor = registros.TotalRojo
                });
                retorno.Add(new CustomAutocomplete()
                {
                    Color = colorCerca,
                    Descripcion = "Fin de soporte próximo",
                    Id = "Fin de soporte próximo",
                    TipoDescripcion = "Fin de soporte próximo",
                    Valor = registros.TotalAmbar
                });

                response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            return response;
        }

        [Route("Reportes/GraficoSubdominios/VigentesObsoletos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage KPISubdominios_0(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            DateTime fechaActual = DateTime.Now;
            try
            {
                fechaActual = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                fechaActual = filtros.FechaFiltro;
            }

            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tiposTecnologias = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());

            var registros = ServiceManager<ReporteDAO>.Provider.GetGrafico(tipos, reds, ids, fechaActual, tiposTecnologias);


            int idNoVigente = ((int)EDashboardEstadoTecnologia.NoVigente);
            int idVigente = ((int)EDashboardEstadoTecnologia.Vigente);
            string colorNoVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idNoVigente);
            string colorVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idVigente);

            if (registros != null)
            {
                var retorno = new List<CustomAutocomplete>();
                retorno.Add(new CustomAutocomplete()
                {
                    Color = colorVigente,
                    Descripcion = "Vigentes",
                    Id = "Vigentes",
                    TipoDescripcion = "Vigentes",
                    Valor = registros.TotalVerde + registros.TotalAmbar
                });
                retorno.Add(new CustomAutocomplete()
                {
                    Color = colorNoVigente,
                    Descripcion = "Obsoletos",
                    Id = "Obsoletos",
                    TipoDescripcion = "Obsoletos",
                    Valor = registros.TotalRojo
                });

                response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            return response;
        }

        [Route("Reportes/GraficoSubdominios/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage KPISubdominiosDetalle(PaginacionDetalleGraficoSubdominio filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());
            var tipoEquipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());

            filtros.SubdominioToString = ids;
            filtros.SubsidiariaToString = reds;
            filtros.TipoTecnologiaToString = tipos;
            filtros.TipoEquipoToString = tipoEquipos;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetDetalleGrafico(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                });

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GraficoSubdominios/DetalleEquipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage KPISubdominiosDetalleEquipos(PaginacionDetalleGraficoSubdominio filtros)
        {
            HttpResponseMessage response = null;
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipoequipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());
            var tipoTecnologia = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());

            filtros.TipoTecnologiaToString = tipoTecnologia;
            filtros.TipoEquipoToString = tipoequipos;
            filtros.SubsidiariaToString = reds;
            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetDetalleTecnologiasGrafico(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);

                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GraficoSubdominios/Exportar")]
        [HttpGet]
        public IHttpActionResult KPISubdominiosExportar(string fecha, string tipoEquipoFiltrar, string subdominioFiltrar, string subsidiaria, string tipotecnologia)
        {
            string nomArchivo = "";

            fecha = string.IsNullOrEmpty(fecha) ? string.Empty : fecha;
            subdominioFiltrar = string.IsNullOrEmpty(subdominioFiltrar) ? string.Empty : subdominioFiltrar;
            subsidiaria = string.IsNullOrEmpty(subsidiaria) ? string.Empty : subsidiaria;
            tipotecnologia = string.IsNullOrEmpty(tipotecnologia) ? string.Empty : tipotecnologia;
            var tipoEquipoIds = string.IsNullOrEmpty(tipoEquipoFiltrar) ? string.Empty : tipoEquipoFiltrar;

            var data = new ExportarData().ExportarReporteSubdominioObsolescencia(fecha, tipoEquipoIds, subsidiaria, subdominioFiltrar, tipotecnologia);
            nomArchivo = string.Format("ReporteSubdominioObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/GraficoSubdominios/ExportarEquipos")]
        [HttpGet]
        public IHttpActionResult KPISubdominiosExportarEquipos(DateTime fecha, string tipoEquipoFiltrar, string subsidiaria, string subdominioFiltrar, string tipotecnologia)
        {
            string nomArchivo = "";

            //fecha = string.IsNullOrEmpty(fecha) ? string.Empty : fecha;
            subdominioFiltrar = string.IsNullOrEmpty(subdominioFiltrar) ? string.Empty : subdominioFiltrar;
            subsidiaria = string.IsNullOrEmpty(subsidiaria) ? string.Empty : subsidiaria;
            var tipoEquipoIds = string.IsNullOrEmpty(tipoEquipoFiltrar) ? string.Empty : tipoEquipoFiltrar;
            var tipoTecnologiaIds = string.IsNullOrEmpty(tipotecnologia) ? string.Empty : tipotecnologia;

            var data = new ExportarData().ExportarReporteEquipoObsolescencia(fecha, tipoEquipoIds, subsidiaria, subdominioFiltrar, tipoTecnologiaIds);
            nomArchivo = string.Format("ReporteEquipoObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/GraficoEvolucion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GraficoEvolucion(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());

            try
            {
                filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                filtros.FechaFiltro = DateTime.Now;
            }

            if (filtros.TipoConsultaId == 1)//Instalaciones
            {
                var registros = ServiceManager<ReporteDAO>.Provider.GetEvolucionSubdominios(tipos, ids, reds, filtros.FechaFiltro);

                if (registros != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, registros);
                }
                return response;
            }
            else //Equipos
            {
                var registros = ServiceManager<ReporteDAO>.Provider.GetEvolucionSubdominiosEquipo(tipos, ids, reds, filtros.FechaFiltro);

                if (registros != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, registros);
                }
                return response;
            }
        }

        [Route("Reportes/GraficoEvolucion/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GraficoEvolucionDetalle(PaginacionDetalleGraficoSubdominio filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoEquipoFiltrar.Select(n => n.ToString()).ToArray());
            filtros.TipoEquipoToString = tipos;
            filtros.SubdominioToString = ids;
            filtros.SubsidiariaToString = reds;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetDetalleEvolucionSubdominios(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                });

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Reportes/GraficoEvolucion/Exportar")]
        [HttpGet]
        public IHttpActionResult GraficoEvolucionExportar(string tipoEquipoFiltrar, string subsidiaria, string subdominioFiltrar, string fecha)
        {
            string nomArchivo = "";
            subdominioFiltrar = string.IsNullOrEmpty(subdominioFiltrar) ? string.Empty : subdominioFiltrar;
            subsidiaria = string.IsNullOrEmpty(subsidiaria) ? string.Empty : subsidiaria;

            var data = new ExportarData().ExportarReporteSubdominioObsolescencia(fecha, tipoEquipoFiltrar, subsidiaria, subdominioFiltrar, string.Empty);
            nomArchivo = string.Format("ReporteSubdominioObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reportes/GraficoAgrupacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GraficoAgrupacion(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var estados = string.Join(",", filtros.EstadoAplicacionFiltrar.Select(n => n.ToString()).ToArray());

            try
            {
                filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                filtros.FechaFiltro = DateTime.Now;
            }

            if (filtros.TipoEquipoIds == null)
                filtros.TipoEquipoIds = string.Empty;

            var registros = ServiceManager<ReporteDAO>.Provider.GetAgrupacion(filtros.TipoEquipoIds, ids, filtros.Agrupacion, reds, estados, filtros.FechaFiltro);

            if (registros != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        [Route("Reportes/GraficoAgrupacion/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarGraficoAgrupacion(string subdominio, string subsidiaria, string estado, string tipoequipo, string agrupacion, string fecha)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarReporteObsolescenciaAplicaciones(subdominio, subsidiaria, estado, tipoequipo, agrupacion, fecha);
            nomArchivo = string.Format("AgrupadoObsolescenciaAplicaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("TecnologiaEquipo/GetTecnologiaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("TecnologiaEquipo/GetTecnologiaByFabricanteNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByFabricanteNombre(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaByFabricanteNombre(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("TecnologiaEquipo/GetTecnologiaByNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByNombre(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaByNombre(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("TecnologiaEquipo/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListarCombos()
        {
            HttpResponseMessage response = null;
            //var listFuente = Utilitarios.EnumToList<Fuente>();
            //var listFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>();
            //var listEstadoObs = Utilitarios.EnumToList<ETecnologiaEstado>();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            //var listSubdominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            //var listFamilia = ServiceManager<FamiliaDAO>.Provider.GetAllFamiliaByFiltro(null);
            //var listTipoTec = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);
            //var listFechaFinSoporte = Utilitarios.EnumToList<EFechaFinSoporte>();
            //var listEstadoTecnologia = Utilitarios.EnumToList<EstadoTecnologia>();

            var dataRpta = new
            {
                //Fuente = listFuente.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //FechaCalculo = listFechaCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //EstadoObs = listEstadoObs.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }),
                //Tipo = listTipoTec,
                Dominio = listDominio
                //Familia = listFamilia,
                //TipoTec = listTipoTec,
                //CodigoInterno = (int)ETablaProcedencia.CVT_Tecnologia,
                //FechaFinSoporte = listFechaFinSoporte.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //EstadoTecnologia = listEstadoTecnologia.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("TecnologiaEquipo/Reporte")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoReporte(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetReporteTecnologiaEquipos(filtros);
            if (registros.Tecnologia != null)
            {
                registros.Tecnologia.SubdominioNomb = HttpUtility.HtmlEncode(registros.Tecnologia.SubdominioNomb);
                registros.Tecnologia.DominioNomb = HttpUtility.HtmlEncode(registros.Tecnologia.DominioNomb);
                registros.Tecnologia.ClaveTecnologia = HttpUtility.HtmlEncode(registros.Tecnologia.ClaveTecnologia);
                registros.Tecnologia.TipoTecNomb = HttpUtility.HtmlEncode(registros.Tecnologia.TipoTecNomb);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("TecnologiaEquipo/ReporteFabricanteNombre")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoReporteFabricenteNombre(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetReporteTecnologiaEquiposFabricanteNombre(filtros);
            if (registros != null)
            {
                registros.TecnologiaList.ForEach(x =>
                {
                    x.SubdominioNomb = HttpUtility.HtmlEncode(x.SubdominioNomb);
                    x.DominioNomb = HttpUtility.HtmlEncode(x.DominioNomb);
                    x.TipoTecNomb = HttpUtility.HtmlEncode(x.TipoTecNomb);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.TipoTecNomb = HttpUtility.HtmlEncode(x.TipoTecNomb);
                });

                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        [Route("TecnologiaEquipo/Reporte/{id:int}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoReporteById(int id)
        {
            var filtros = new FiltrosDashboardTecnologiaEquipos();
            filtros.TecnologiaIdFiltrar = id;

            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetReporteTecnologiaEquipos(filtros);
            if (registros != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        [Route("TecnologiaEquipo/ListadoEquipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoEquipos(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiaEquipos(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ListadoEquiposFabricanteNombre")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoEquiposFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiaEquiposFabricanteNombre(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ListadoAplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoAplicaciones(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiaAplicaciones(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ListadoAplicacionesFabricanteNombre")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoAplicacionesFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiaAplicacionesFabricanteNombre(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x => { x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia); });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ListadoTecnologiasVinculadas")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiasVinculadas(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ListadoTecnologiasVinculadasFabricanteNombre")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage TecnologiaEquipoListadoTecnologiasVinculadasFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListadoTecnologiasVinculadasFabricanteNombre(filtros, out totalRows);
            if (registros != null)
            {

                registros.ForEach(x =>
                {
                    x.DominioNomb = HttpUtility.HtmlEncode(x.DominioNomb);
                    x.SubdominioNomb = HttpUtility.HtmlEncode(x.SubdominioNomb);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.TipoTecNomb = HttpUtility.HtmlEncode(x.TipoTecNomb);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("TecnologiaEquipo/ExportarEquipos")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiaEquipoListadoEquipos(int idTecnologia, DateTime fecha, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiaEquipoListadoEquipos(idTecnologia, fecha, sortName, sortOrder);
            nomArchivo = string.Format("ListaEquiposXTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("TecnologiaEquipo/ExportarAplicaciones")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiaEquipoListadoAplicaciones(int idTecnologia, DateTime fecha, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;

            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiaEquipoListadoAplicaciones(idTecnologia, fecha, sortName, sortOrder, perfil);
            nomArchivo = string.Format("ListaAplicacionesXTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("TecnologiaEquipo/ExportarTV")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiaEquipoListadoTV(int idTecnologia, DateTime fecha, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiaEquipoListadoTV(idTecnologia, fecha, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologíasVinculadasXTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Tecnologia/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaDetalle(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            filtros.OwnerFiltro = string.IsNullOrWhiteSpace(filtros.OwnerFiltro) ? "" : filtros.OwnerFiltro;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Detalle/Equipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaDetalle_Equipos(FiltrosDashboardTecnologiaEquipos filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoXTecnologiaIdXFecha(filtros.TecnologiaFiltrar, filtros.FechaConsulta.Value, string.Join(",", filtros.SubdominiosId), filtros.Owner, filtros.IndexObs, filtros.pageNumber, filtros.pageSize, filtros.sortName, filtros.sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/SinFechaFin")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTecnologiaSinFechaFin(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var idsTipo = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());

            filtros.SubdominioToString = ids;
            filtros.TipoTecnologiaToString = idsTipo;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaSinFechaFin(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                });
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Tecnologia/SinFechaFin/Exportar")]
        [HttpGet]
        public IHttpActionResult GetTecnologiaSinFechaFinExportar(string fecha, string subdominioFiltrar, string tipotecnologiaId, int tipoConsultaId)
        {
            fecha = string.IsNullOrEmpty(fecha) ? string.Empty : fecha;
            subdominioFiltrar = string.IsNullOrEmpty(subdominioFiltrar) ? string.Empty : subdominioFiltrar;
            DateTime fechaFiltro = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string nomArchivo = "";
            if (tipotecnologiaId == null) tipotecnologiaId = "";
            tipotecnologiaId = tipotecnologiaId.Replace("null", "");
            var data = new ExportarData().ExportarTecnologiaSinFechaFin(subdominioFiltrar, fechaFiltro, tipotecnologiaId ?? "", tipoConsultaId);
            nomArchivo = string.Format("TecnologiaSinFechaFin_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Tecnologia/FechaIndefinida")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTecnologiaFechaIndefinida(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            var ids = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            var idsTipo = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());

            filtros.SubdominioToString = ids;
            filtros.TipoTecnologiaToString = idsTipo;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteTecnologiaFechaIndefinida(filtros, out totalRows);
            if (registros != null)
            {

                registros.ForEach(x =>
                {
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                });
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Tecnologia/Aplicacion")] //TODO
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaAplicacion(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            filtros.OwnerFiltro = string.IsNullOrWhiteSpace(filtros.OwnerFiltro) ? "" : filtros.OwnerFiltro;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologia_Aplicacion(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Aplicacion/ByTecnologia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaAplicacionByTecnologia(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            //filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            //filtros.OwnerFiltro = string.IsNullOrWhiteSpace(filtros.OwnerFiltro) ? "" : filtros.OwnerFiltro;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologia_AplicacionByTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Equipo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEquipo(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            filtros.OwnerFiltro = string.IsNullOrWhiteSpace(filtros.OwnerFiltro) ? "" : filtros.OwnerFiltro;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologia_Equipo(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Equipo/ByTecnologia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEquipoByTecnologia(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            //filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            //filtros.OwnerFiltro = string.IsNullOrWhiteSpace(filtros.OwnerFiltro) ? "" : filtros.OwnerFiltro;

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologia_EquipoByTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Detalle/Exportar")]
        [HttpGet]
        public IHttpActionResult GetTecnologiaDetalleExportar(string subdominios, string fecha, string owner)
        {
            DateTime fechaFiltro = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            owner = string.IsNullOrWhiteSpace(owner) ? "" : owner;

            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiaDetalle(subdominios, fechaFiltro, owner);
            nomArchivo = string.Format("DetalleTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("Tecnologia/Instalaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaInstalaciones(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologiaInstalaciones(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                });

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Instalaciones/ByTecnologia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaInstalacionesByTecnologia(FiltrosDashboardTecnologia filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            //filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteDetalleTecnologiaInstalacionesByTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Tecnologia/Instalaciones/Exportar")]
        [HttpGet]
        public IHttpActionResult GetTecnologiaInstalacionesExportar(string subdominios, string fecha)
        {
            DateTime fechaFiltro = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiaInstalaciones(subdominios, fechaFiltro);
            nomArchivo = string.Format("DetalleTecnologiaInstalaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Owner")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaOwner(PaginacionOwner filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            filtros.Tecnologia = string.IsNullOrEmpty(filtros.Tecnologia) ? "" : filtros.Tecnologia;
            filtros.SubdominioToString = string.Join(",", filtros.SubdominioFiltrar.Select(n => n.ToString()).ToArray());
            filtros.TipoTecnologiaToString = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteOwner(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                    x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                    x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
                });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Owner/Tecnologia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaOwnerById(PaginacionOwner filtros)
        {
            HttpResponseMessage response = null;
            filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetReporteOwnerByTecnologia(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Owner/Exportar")]
        [HttpGet]
        public IHttpActionResult GetTecnologiaOwnerExportar(string owner, string fecha, string tecnologia, string tipoTecnologia, string subdominio)
        {
            owner = string.IsNullOrWhiteSpace(owner) ? "" : owner;
            tecnologia = string.IsNullOrWhiteSpace(tecnologia) ? "" : tecnologia;
            subdominio = string.IsNullOrWhiteSpace(subdominio) ? "" : subdominio;
            tipoTecnologia = string.IsNullOrWhiteSpace(tipoTecnologia) ? "" : tipoTecnologia;

            string filename = "";
            var data = new ExportarData().ExportarReporteOwnerv2(subdominio, owner, fecha, tipoTecnologia, tecnologia);
            filename = string.Format("ReporteOwner_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Reportes/IndicadorObsolescenciaSoBd")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRptIndicadorObsolescenciaSoBd(PaginacionDetalleGraficoSubdominio filtros)
        {
            HttpResponseMessage response = null;
            IndicadorObsolescenciaSoBd data = new IndicadorObsolescenciaSoBd();

            DateTime fechaActual = DateTime.Now;
            try
            {
                fechaActual = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                fechaActual = DateTime.Now;
            }

            var subdominioIds = Utilitarios.SUBDOMINIO_INDICADOR_OBS;
            int subdominioSO = Utilitarios.SUBDOMINIO_SO;

            var idSoBd = string.Join(",", subdominioIds.Select(n => n.ToString()).ToArray());
            var idSo = string.Join(",", subdominioIds.Select(n => n.ToString()).Where(x => int.Parse(x) == subdominioSO).ToArray());
            var idBd = string.Join(",", subdominioIds.Select(n => n.ToString()).Where(x => int.Parse(x) != subdominioSO).ToArray());

            var reds = string.Join(",", filtros.SubsidiariaFiltrar.Select(n => n.ToString()).ToArray());
            var tipos = string.Join(",", filtros.TipoTecnologiaFiltrar.Select(n => n.ToString()).ToArray());

            int idNoVigente = ((int)EDashboardEstadoTecnologia.NoVigente);
            int idVigente = ((int)EDashboardEstadoTecnologia.Vigente);
            int idCercaFin = ((int)EDashboardEstadoTecnologia.CercaFinSoporte);
            string colorNoVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idNoVigente);
            string colorVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idVigente);
            string colorCerca = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idCercaFin);

            var resultadoSOBD = ServiceManager<ReporteDAO>.Provider.GetGrafico(filtros.TipoEquipoToString, reds, idSoBd, fechaActual, tipos);
            if (resultadoSOBD != null)
            {
                data.PieSoBd = new List<CustomAutocomplete>();
                data.PieSoBd.Add(new CustomAutocomplete()
                {
                    Color = colorVigente,
                    Descripcion = "Vigentes",
                    Id = "Vigentes",
                    TipoDescripcion = "Vigentes",
                    Valor = resultadoSOBD.TotalVerde
                });
                data.PieSoBd.Add(new CustomAutocomplete()
                {
                    Color = colorNoVigente,
                    Descripcion = "Obsoletos",
                    Id = "Obsoletos",
                    TipoDescripcion = "Obsoletos",
                    Valor = resultadoSOBD.TotalRojo
                });
                data.PieSoBd.Add(new CustomAutocomplete()
                {
                    Color = colorCerca,
                    Descripcion = "Fin de soporte próximo",
                    Id = "Fin de soporte próximo",
                    TipoDescripcion = "Fin de soporte próximo",
                    Valor = resultadoSOBD.TotalAmbar
                });
            }

            var resultadoSO = ServiceManager<ReporteDAO>.Provider.GetGrafico(filtros.TipoEquipoToString, reds, idSo, fechaActual, tipos);
            if (resultadoSO != null)
            {
                data.PieSo = new List<CustomAutocomplete>();
                data.PieSo.Add(new CustomAutocomplete()
                {
                    Color = colorVigente,
                    Descripcion = "Vigentes",
                    Id = "Vigentes",
                    TipoDescripcion = "Vigentes",
                    Valor = resultadoSO.TotalVerde
                });
                data.PieSo.Add(new CustomAutocomplete()
                {
                    Color = colorNoVigente,
                    Descripcion = "Obsoletos",
                    Id = "Obsoletos",
                    TipoDescripcion = "Obsoletos",
                    Valor = resultadoSO.TotalRojo
                });
                data.PieSo.Add(new CustomAutocomplete()
                {
                    Color = colorCerca,
                    Descripcion = "Fin de soporte próximo",
                    Id = "Fin de soporte próximo",
                    TipoDescripcion = "Fin de soporte próximo",
                    Valor = resultadoSO.TotalAmbar
                });
            }

            var resultadoBD = ServiceManager<ReporteDAO>.Provider.GetGrafico(filtros.TipoEquipoToString, reds, idBd, fechaActual, tipos);
            if (resultadoBD != null)
            {
                data.PieBd = new List<CustomAutocomplete>();
                data.PieBd.Add(new CustomAutocomplete()
                {
                    Color = colorVigente,
                    Descripcion = "Vigentes",
                    Id = "Vigentes",
                    TipoDescripcion = "Vigentes",
                    Valor = resultadoBD.TotalVerde
                });
                data.PieBd.Add(new CustomAutocomplete()
                {
                    Color = colorNoVigente,
                    Descripcion = "Obsoletos",
                    Id = "Obsoletos",
                    TipoDescripcion = "Obsoletos",
                    Valor = resultadoBD.TotalRojo
                });
                data.PieBd.Add(new CustomAutocomplete()
                {
                    Color = colorCerca,
                    Descripcion = "Fin de soporte próximo",
                    Id = "Fin de soporte próximo",
                    TipoDescripcion = "Fin de soporte próximo",
                    Valor = resultadoBD.TotalAmbar
                });
            }


            response = Request.CreateResponse(HttpStatusCode.OK, data);

            return response;
        }

        [Route("Storage/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageListado(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorage(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Listado/Exportar")]
        [HttpGet]
        public IHttpActionResult GetStorageListadoExportar(string nombre, DateTime fecha, string softwareBase)
        {
            var filtro = new PaginacionStorage()
            {
                Nombre = string.IsNullOrEmpty(nombre) ? "" : nombre,
                Fecha = fecha,
                SoftwareBase = string.IsNullOrEmpty(softwareBase) ? "" : softwareBase,
                PageSize = int.MaxValue,
                PageNumber = 1,
                OrderBy = "Nombre",
                OrderByDirection = "asc"
            };

            string filename = "";
            var data = new ExportarData().ExportarStorage(filtro);
            filename = string.Format("ReporteStorage_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }


        [Route("Storage/Listado/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageListadoAplicaciones(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageAplicacion(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Listado/Aplicaciones/Exportar")]
        [HttpGet]
        public IHttpActionResult GetStorageAplicacionesExportar(string aplicacion, string equipo, string storage, DateTime fecha, string softwareBase, int ambiente)
        {
            var filtro = new PaginacionStorage()
            {
                Aplicacion = string.IsNullOrEmpty(aplicacion) ? "" : aplicacion,
                Ambiente = ambiente,
                Equipo = string.IsNullOrEmpty(equipo) ? "" : equipo,
                Storage = string.IsNullOrEmpty(storage) ? "" : storage,
                Fecha = fecha,
                SoftwareBase = string.IsNullOrEmpty(softwareBase) ? "" : softwareBase,
                PageSize = int.MaxValue,
                PageNumber = 1,
                OrderBy = "CodigoAPT",
                OrderByDirection = "asc"
            };

            string filename = "";
            var data = new ExportarData().ExportarStorageAplicaciones(filtro);
            filename = string.Format("ReporteStorageAplicaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Storage/Listado/Equipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageListadoEquipos(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageEquipo(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Listado/Equipos/Exportar")]
        [HttpGet]
        public IHttpActionResult GetStorageEquiposExportar(string equipo, string storage, DateTime fecha, string softwareBase, int replica)
        {
            var filtro = new PaginacionStorage()
            {
                Equipo = string.IsNullOrEmpty(equipo) ? "" : equipo,
                Storage = string.IsNullOrEmpty(storage) ? "" : storage,
                Fecha = fecha,
                SoftwareBase = string.IsNullOrEmpty(softwareBase) ? "" : softwareBase,
                PageSize = int.MaxValue,
                PageNumber = 1,
                OrderBy = "CodigoAPT",
                OrderByDirection = "asc",
                TieneReplica = replica
            };

            string filename = "";
            var data = new ExportarData().ExportarStorageEquipos(filtro);
            filename = string.Format("ReporteStorageEquipos_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Storage/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostStorageListarCombos()
        {
            HttpResponseMessage response = null;
            var lAmbiente = ServiceManager<AmbienteDAO>.Provider.GetAmbienteByFiltro(null);

            var dataRpta = new
            {
                Ambiente = lAmbiente
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("IP/Consolidado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidado(Paginacion filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidado(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Nivel1")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidadoNivel1(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidadoNivel1(filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Nivel2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidadoNivel2(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidadoNivel2(filtros.Identificacion, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Nivel3")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidadoNivel3(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidadoNivel3(filtros.Identificacion, filtros.CMDB, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Nivel4")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidadoNivel4(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidadoNivel4(filtros.Identificacion, filtros.CMDB, filtros.Zona, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Nivel5")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPConsolidadoNivel5(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaConsolidadoNivel5(filtros.Identificacion, filtros.CMDB, filtros.Zona, filtros.Ips, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Consolidado/Exportar")]
        [HttpGet]
        public IHttpActionResult GetIPConsolidadoExportar()
        {
            string filename = "";
            var data = new ExportarData().ExportarIPConsolidado();
            filename = string.Format("RelevamientoIP_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("IP/Estado/Resumen")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoResumen(Paginacion filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 1;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoResumen();
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoDetalle(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoDetalle(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Detalle/Exportar")]
        [HttpGet]
        public IHttpActionResult GetIPDetalleExportar(DateTime fecha)
        {
            string filename = "";
            var data = new ExportarData().ExportarIPDetalle(fecha);
            filename = string.Format("DetalleIP_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Storage/Nivel1")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageNivel1(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumenNivel1(out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Nivel2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageNivel2(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumenNivel2(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Nivel3")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageNivel3(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumenNivel3(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Nivel4")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageNivel4(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumenNivel4(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Resumen2/Nivel1")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageResumen2Nivel1(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumen2Nivel1(out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Resumen2/Nivel2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageResumen2Nivel2(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumen2Nivel2(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Resumen2/Nivel3")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageResumen2Nivel3(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumen2Nivel3(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Resumen2/Nivel4")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageResumen2Nivel4(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumen2Nivel4(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Resumen2/Nivel5")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageResumen2Nivel5(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetStorageResumen2Nivel5(filtros, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("Storage/Indicadores")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetStorageIndicaores(PaginacionStorage filtros)
        {
            HttpResponseMessage response = null;

            var datosGlobales = ServiceManager<StorageDAO>.Provider.GetIndicadorGlobal();
            var datosTier1 = ServiceManager<StorageDAO>.Provider.GetIndicadorTier(1);
            var datosTier2 = ServiceManager<StorageDAO>.Provider.GetIndicadorTier(2);

            var dataRpta = new
            {
                Totales = datosGlobales[1],
                Replica = datosGlobales[2],
                Obsolescencia = datosGlobales[3],
                Aplicaciones = datosGlobales[4],
                Actual = datosGlobales[5],
                Ambiente = datosGlobales[6],
                Fuente = datosGlobales[7],
                datosTier1 = new
                {
                    Totales = datosTier1[1],
                    Replica = datosTier1[2],
                    Aplicaciones = datosTier1[3]
                },
                datosTier2 = new
                {
                    Totales = datosTier2[1],
                    Replica = datosTier2[2],
                    Aplicaciones = datosTier2[3]
                }
            };


            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);

            return response;
        }

        [Route("IP/Estado/Nivel1")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoNivel1(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoNivel1(filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Nivel2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoNivel2(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoNivel2(filtros.Zona, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Nivel3")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoNivel3(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoNivel3(filtros.Zona, filtros.Addm, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Nivel4")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoNivel4(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoNivel4(filtros.Zona, filtros.Addm, filtros.Fuente, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Nivel5")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetIPEstadoNivel5(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetVistaEstadoNivel5(filtros.Zona, filtros.Addm, filtros.Fuente, filtros.Ips, filtros.Fecha);
            if (registros != null)
            {
                totalRows = registros.Count;

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }

        [Route("IP/Estado/Exportar")]
        [HttpGet]
        public IHttpActionResult GetIPEstadoExportar()
        {
            string filename = "";
            var data = new ExportarData().ExportarIPEstado();
            filename = string.Format("EstadoResponsablesIP_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("IP/Seguimiento")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetSeguimientoIP(PaginacionIP filtros)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<IPDAO>.Provider.GetSeguimiento(filtros);
            var totalIps = 0;
            if (registros != null)
            {
                totalRows = registros.Count;
                if (totalRows > 0)
                {
                    totalIps = registros[0].Total;
                }

                var dataRpta = new { Total = totalRows, Rows = registros, TotalIps = totalIps };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }

            return response;
        }
    }
}

