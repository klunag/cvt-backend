using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Alerta")]
    public class AlertaController : BaseController
    {
        // POST: api/Alerta/Tecnicas
        [Route("Tecnicas")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasTecnicas(PaginacionAlerta pag)
        {
            var totalRows = 0;
            //var cache = new MemoryCacheManager<List<AlertaDTO>>();

            //var registros = cache.GetOrCreate("AlertasTecnicas"
            //       , ServiceManager<AlertaDAO>.Provider.GetAlertasXTipo((int)ETipoAlerta.Tecnica, pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows));

            //var registros = cache.GetCache("AlertasTecnicas");
            //if (registros == null)
            //{
            //    registros = cache.GetOrCreate("AlertasTecnicas"
            //       , ServiceManager<AlertaDAO>.Provider.GetAlertasXTipo((int)ETipoAlerta.Tecnica, pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows));
            //}

            var registros = ServiceManager<AlertaDAO>.Provider.GetAlertasXTipo((int)ETipoAlerta.Tecnica, pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<AlertaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/Alerta/Detalle
        [Route("Detalle")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostGetAlertasDetalle(PaginacionAlerta pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AlertaDAO>.Provider.GetAlertasDetalle(pag.id, pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<AlertaDetalleDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarTecnicas")]
        [HttpGet]
        public IHttpActionResult GetExportarTecnicas(DateTime fechaConsulta, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasTecnicas(fechaConsulta, sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasTecnicas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        // POST: api/Alerta/Funcionales
        [Route("Funcionales")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionales()
        {
            var cache = new MemoryCacheManager<AlertaDTO>();
            var fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var arrTipoEquipo = new int[]
            {
                (int)ETipoEquipo.Servidor,
                (int)ETipoEquipo.ServidorAgencia,
                (int)ETipoEquipo.ServicioNube,
            };

            var arrTipoEquipo_Alerta5 = string.Join("|", arrTipoEquipo.Where(x => x.Equals((int)ETipoEquipo.Servidor)));
            var arrTipoEquipo_Alerta7 = string.Join("|", arrTipoEquipo.Where(x => x.Equals((int)ETipoEquipo.ServicioNube)));

            var rptaAlertas = new AlertaFuncionalesModel();

            var datosAlerta1 = cache.GetCache("AlertasFuncionales_1");
            if (datosAlerta1 == null)
                datosAlerta1 = cache.GetOrCreate("AlertasFuncionales_1", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_TecnologiasEstadoPendiente());

            var datosAlerta2 = cache.GetCache("AlertasFuncionales_2");
            if (datosAlerta2 == null)
                datosAlerta2 = cache.GetOrCreate("AlertasFuncionales_2", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_TecnologiasSinEquivalencias());

            var datosAlerta3 = cache.GetCache("AlertasFuncionales_3");
            if (datosAlerta3 == null)
                datosAlerta3 = cache.GetOrCreate("AlertasFuncionales_3", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_EquiposSinSistemaOperativo(fechaActual));

            var datosAlerta4 = cache.GetCache("AlertasFuncionales_4");
            if (datosAlerta4 == null)
                datosAlerta4 = cache.GetOrCreate("AlertasFuncionales_4", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_EquiposSinTecnologias(fechaActual));

            var datosAlerta5 = cache.GetCache("AlertasFuncionales_5");
            if (datosAlerta5 == null)
                datosAlerta5 = cache.GetOrCreate("AlertasFuncionales_5", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_EquiposSinRelaciones(arrTipoEquipo_Alerta5, (int)EAlertaFuncional.AlertaFuncional5));

            var datosAlerta6 = cache.GetCache("AlertasFuncionales_6");
            if (datosAlerta6 == null)
                datosAlerta6 = cache.GetOrCreate("AlertasFuncionales_6", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_TecnologiasSinFechaSoporte());

            var datosAlerta7 = cache.GetCache("AlertasFuncionales_7");
            if (datosAlerta7 == null)
                datosAlerta7 = cache.GetOrCreate("AlertasFuncionales_7", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_EquiposSinRelaciones(arrTipoEquipo_Alerta7, (int)EAlertaFuncional.AlertaFuncional7));
            
            var datosAlerta8 = cache.GetCache("AlertasFuncionales_8");
            if (datosAlerta8 == null)
                datosAlerta8 = cache.GetOrCreate("AlertasFuncionales_8", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_UrlHuerfana());

            var datosAlerta9 = cache.GetCache("AlertasFuncionales_9");
            if (datosAlerta9 == null)
                datosAlerta9 = cache.GetOrCreate("AlertasFuncionales_9", ServiceManager<AlertaDAO>.Provider.GetAlertaFuncional_EquipoNoRegistrado());

            rptaAlertas.AlertaFuncional1 = datosAlerta1;
            rptaAlertas.AlertaFuncional2 = datosAlerta2;
            rptaAlertas.AlertaFuncional3 = datosAlerta3;
            rptaAlertas.AlertaFuncional4 = datosAlerta4;
            rptaAlertas.AlertaFuncional5 = datosAlerta5;
            rptaAlertas.AlertaFuncional6 = datosAlerta6;
            rptaAlertas.AlertaFuncional7 = datosAlerta7;
            rptaAlertas.AlertaFuncional8 = datosAlerta8;
            rptaAlertas.AlertaFuncional9 = datosAlerta9;

            if (rptaAlertas == null)
                return NotFound();

            return Ok(rptaAlertas);
        }

        // POST: api/Alerta/Funcionales
        [Route("Indicadores")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetIndicadores()
        {
            //var cache = new MemoryCacheManager<IndicadoresDto>();

            //var datos = cache.GetCache("AlertaIndicadores");
            //if (datos == null)
            //{
            //    datos = cache.GetOrCreate("AlertaIndicadores"
            //        , ServiceManager<AlertaDAO>.Provider.GetIndicadores());
            //}
            var datos = ServiceManager<AlertaDAO>.Provider.GetIndicadores();

            if (datos == null)
                return NotFound();


            return Ok(datos);
        }

        [Route("TecnologiasPorVencer")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostListTecnologiasPorVencer(PaginacionTecnologiaPorVencer pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasPorVencer(pag.subdominio, pag.tecnologia, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
            });

            var reader = new BootstrapTable<TecnologiaPorVencerDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("TecnologiasVencidas")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostListTecnologiasVencidas(PaginacionTecnologiaPorVencer pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasVencidas(pag.subdominio, pag.tecnologia, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);
            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
            });
            var reader = new BootstrapTable<TecnologiaPorVencerDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Funcionales/Alerta1")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo1(Paginacion pag)
        {
            var idsEstados = new List<int>{
                                (int)EstadoTecnologia.Registrado,
                                (int)EstadoTecnologia.EnRevision
                            };
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasXEstado(idsEstados, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo1")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo1(string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo1(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales1_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel= data, name= nomArchivo});
        }

        [Route("Funcionales/Alerta2")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo2(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasSinEquivalencia(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo2")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo2(string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo2(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales2_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Funcionales/Alerta3")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo3(PaginacionAlerta pag)
        {
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquiposSinSistemaOperativo(idSubdominio, pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo3")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo3(DateTime fechaConsulta, string sortName, string sortOrder)
        {
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo3(idSubdominio, fechaConsulta, sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales3_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Funcionales/Alerta4")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo4(PaginacionAlerta pag)
        {

            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquiposSinTecnologias(pag.fechaConsulta, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo4")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo4(DateTime fechaConsulta, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo4(fechaConsulta, sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales4_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Funcionales/Alerta5")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo5(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquiposSinRelaciones(pag.arrTipoEquipo, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo5")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo5(string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo5(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales3_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarAlertasFuncionalesTipo7")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo7(string sortName, string sortOrder)
        {
            var data = new ExportarData().ExportarAlertasFuncionalesTipo7(sortName, sortOrder);
            var filename = string.Format("ListadoAlertasFuncionales7_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Funcionales/Alerta6")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAlertasFuncionalesTipo6(Paginacion pag)
        {

            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasSinFechasSoporte(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Funcionales/Alerta8")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListAlertasFuncionalesTipo8(Paginacion pag)
        {

            var totalRows = 0;
            var registros = ServiceManager<UrlAplicacionDAO>.Provider.GetAlertaFuncional_UrlHuerfana_Detalle(pag.pageNumber,pag.pageSize,pag.sortName,pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<UrlAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Funcionales/Alerta9")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListAlertasFuncionalesTipo9(Paginacion pag)
        {

            var totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetEquipoNoRegSP("", 0, 0, -1, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.NombreEquipo = HttpUtility.HtmlEncode(x.NombreEquipo);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoNoRegistradoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAlertasFuncionalesTipo6")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo6(string sortName, string sortOrder)
        {

            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo6(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales6_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarAlertasFuncionalesTipo8")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo8(string sortName, string sortOrder)
        {

            string nomArchivo = "";

            var data = new ExportarData().ExportarAlertasFuncionalesTipo8(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales8_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarAlertasFuncionalesTipo9")]
        [HttpGet]
        public IHttpActionResult GetExportarFuncionalesTipo9(string sortName, string sortOrder)
        {

            string nomArchivo = "";
            var data = new ExportarData().ExportarAlertasFuncionalesTipo9(sortName, sortOrder);
            nomArchivo = string.Format("ListadoAlertasFuncionales9_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Tecnicas/CargarCombos")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetTecnicasCargarCombos()
        {
            var listFrecuencia = Utilitarios.EnumToList<ETipoFrecuencia>();
            var reader = new { Frecuencia = listFrecuencia.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList() };
            return Ok(reader);
        }

        [Route("Tecnicas/AddorEditAlertaProgramacion")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostAddorEditAlertaProgramacion(AlertaProgramacionDTO request)
        {
            int id = ServiceManager<AlertaDAO>.Provider.AddorEditAlertaProgramacion(request);
            if (id > 0)
            {
                return Ok(id);
            }
            return NotFound();
        }

        [Route("Tecnicas/GetAlertaProgramacion/{id:int}")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetAlertaProgramacionById(int id)
        {
            var dataRpta = ServiceManager<AlertaDAO>.Provider.GetAlertaProgramacion(id);
            return Ok(dataRpta);
        }

        [Route("Mensajes")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostMensajes(MensajeDTO mensajeDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            mensajeDTO.NombreUsuarioCreacion = user.Nombres;
            mensajeDTO.UsuarioCreacion = user.Matricula;
            mensajeDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            long EntidadId = ServiceManager<AlertaDAO>.Provider.AddMensaje(mensajeDTO);
            bool estado = EntidadId > 0;
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("Mensajes/Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListTipos(PaginacionMensaje pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AlertaDAO>.Provider.GetMensaje(pag.Matricula, pag.nombre, pag.FechaRegistro, pag.tipoId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<MensajeDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("Mensajes/ListarCombos")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetMensajesCargarCombos()
        {
            HttpResponseMessage response = null;

            var listTipoMensaje = Utilitarios.EnumToList<ETipoMensaje>();

            var dataRpta = new
            {
                TipoMensaje = listTipoMensaje.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
                //TipoExperto = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoByFiltro(null)
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Mensajes/GetMensajeById")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetMensajeById(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string usuario = user.Matricula;

            HttpResponseMessage response = null;

            var objMensaje = ServiceManager<AlertaDAO>.Provider.GetMensajeById(Id, usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, objMensaje);
            return response;
        }

        [Route("Mensajes/Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportMensajes(string matricula, string nombre, int tipoId, DateTime? fechaRegistro, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarMensajes(matricula, nombre, tipoId, fechaRegistro, sortName, sortOrder);
            nomArchivo = string.Format("ListaMensaje_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Responsables/Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListarResponsables(PaginacionMensaje pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AlertaDAO>.Provider.GetResponsablesIndicadores(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<IndicadorResponsableDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Responsables/Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportResponsables(string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarReporteResponsablesPortafolio();
            nomArchivo = string.Format("ListaResponsablesPortafolio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("TipoNotificaciones/Listado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetTipoNotificaciones()
        {
            HttpResponseMessage response = null;

            var dataRpta = new TipoNotificacionesData();
            var dataTipoNotificaciones = ServiceManager<NotificacionDAO>.Provider.ListarTipoNotificaciones(true);
            var idsTipoNotificacionesExcluir = Settings.Get<string>("TipoNotificacionesExcluir.Id").Split(',');


            dataRpta.ListaTipoNotificaciones = dataTipoNotificaciones.Where(x => !idsTipoNotificacionesExcluir.Contains(x.Id.ToString())).ToList();

            var dataTipoFrecuencia = Utilitarios.EnumToList<ETipoFrecuencia>();

            dataRpta.ListaFrecuenciaNotificacion = dataTipoFrecuencia.Select(x => new CustomAutocomplete
            {
                Descripcion = x.ToString(),
                Id = ((int)x).ToString()
            }).ToList();

            dataRpta.ListaTipoNotificaciones.ForEach(x => { x.Asunto = HttpUtility.HtmlEncode(x.Asunto); });    

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("TipoNotificaciones/Listado/Todo")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetTipoNotificacionesTodo()
        {
            HttpResponseMessage response = null;

            var dataRpta = new TipoNotificacionesData();
            var dataTipoNotificaciones = ServiceManager<NotificacionDAO>.Provider.ListarTipoNotificaciones(true);

            dataRpta.ListaTipoNotificaciones = dataTipoNotificaciones;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("TipoNotificaciones/Get/{id:int}")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ObtenerTipoNotificaciones(int id)
        {
            HttpResponseMessage response = null;

            var obj = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacion(id);

            response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [Route("TipoNotificaciones/Update")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage ActualizarTipoNotificaciones(TipoNotificacionDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            var rpta = ServiceManager<NotificacionDAO>.Provider.AddOrEditTipoNotificacion(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }


        [Route("NotificacionesResponsablesApps/CargarCombos")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetNotificacionesResponsablesAppsCombo()
        {
            HttpResponseMessage response = null;

            var dataRpta = new NotificacionResponsableAplicacionDataFiltros();


            var dataPortafolioResponsable = ServiceManager<NotificacionDAO>.Provider.ListarPortafolioResponsable();

            dataRpta.ListaPortafolioResponsables = dataPortafolioResponsable;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("NotificacionesResponsablesApps/Listado")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage GetNotificacionesResponsablesApps(PaginacionResponsableAplicacion pag)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var dataRpta = new NotificacionResponsableAplicacionData();

            var dataLista = ServiceManager<NotificacionDAO>.Provider.ListarNotificacionesResponsablesAplicaciones(pag.ResponsableAplicacionId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            var reader = new BootstrapTable<AplicacionResponsableDto>()
            {
                Total = totalRows,
                Rows = dataLista
            };

            dataRpta.ListaNotificaciones = reader;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("NotificacionesResponsablesApps/ListadoDetalle")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage GetNotificacionesResponsablesDetalleXApp(PaginacionResponsableAplicacion pag)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var dataRpta = new NotificacionResponsableAplicacionDetalleData();
            var dataLista = ServiceManager<NotificacionDAO>.Provider.ListarNotificacionesResponsablesAplicacionesDetalle(pag.ResponsableAplicacionId, pag.Matricula, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            var reader = new BootstrapTable<AplicacionDTO>()
            {
                Total = totalRows,
                Rows = dataLista
            };

            dataRpta.Aplicaciones = reader;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("NotificacionesResponsablesApps/ObtenerNotificacionResponsableAplicacion")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ObtenerNotificacionResponsableAplicacion(int idTipoResponsable, bool flagTTLJdE)
        {
            HttpResponseMessage response = null;

            var obj = ServiceManager<NotificacionDAO>.Provider.ObtenerNotificacionResponsableAplicacion(idTipoResponsable, flagTTLJdE);

            response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [Route("NotificacionesResponsablesApps/AddOrEditNotificacionResponsableAplicacion")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage AddOrEditNotificacionResponsableAplicacion(NotificacionResponsableAplicacionDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            var rpta = ServiceManager<NotificacionDAO>.Provider.AddOrEditNotificacionResponsableAplicacion(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }

        [Route("NotificacionesResponsablesApps/ListadoDetalleTTLJdE")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage GetNotificacionesResponsablesDetalleTTLJdE(PaginacionResponsableAplicacion pag)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var dataRpta = new NotificacionResponsableTTLJdE();
            var dataLista = ServiceManager<NotificacionDAO>.Provider.ListarNotificacionesResponsablesTTLJdE(pag.ResponsableAplicacionId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            var reader = new BootstrapTable<AplicacionPortafolioResponsablesDTO>()
            {
                Total = totalRows,
                Rows = dataLista
            };

            dataRpta.Responsables = reader;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("NotificacionesResponsablesApps/EnviarNotificacion")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage AddOrEditNotificacion(NotificacionDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var tipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacion((int)ETipoNotificacion.RiesgoObsolescenciaAplicaciones);

            entidad.TipoNotificacionId = tipoNotificacion.Id;
            entidad.Nombre = tipoNotificacion.Nombre;
            entidad.Asunto = tipoNotificacion.Asunto;
            entidad.FlagEnviado = false;
            entidad.De = "";
            entidad.CC = "";
            entidad.BCC = "";

            var entidadId = ServiceManager<NotificacionDAO>.Provider.AddOrEditNotificacion(entidad);

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("TipoNotificaciones/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetNotificaciones()
        {
            HttpResponseMessage response = null;

            var dataRpta = new TipoNotificacionesData();
            var dataTipoNotificaciones = ServiceManager<NotificacionDAO>.Provider.ListarTipoNotificaciones(true);
            var idsTipoNotificacionesExcluir = Settings.Get<string>("TipoNotificacionesExcluir.Id").Split(',');


            dataRpta.ListaTipoNotificaciones = dataTipoNotificaciones.Where(x => !idsTipoNotificacionesExcluir.Contains(x.Id.ToString())).ToList();

            var dataTipoFrecuencia = Utilitarios.EnumToList<ETipoFrecuencia>();

            dataRpta.ListaFrecuenciaNotificacion = dataTipoFrecuencia.Select(x => new CustomAutocomplete
            {
                Descripcion = x.ToString(),
                Id = ((int)x).ToString()
            }).ToList();

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Notificaciones/Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListNotificaciones(PaginacionNotificacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AlertaDAO>.Provider.GetNotificaciones(pag.Para, pag.Asunto, pag.MesesTrimestre, pag.AnioFiltro, pag.FechaFiltro, pag.TipoNotificacionId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<NotificacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Notificaciones/Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportNotificaciones(string para, string asunto, int tipoId, DateTime? fechaRegistro, string mesesTrimestre, int? anio)
        {
            string filename = "";
            para = string.IsNullOrWhiteSpace(para) ? "" : para;
            asunto = string.IsNullOrWhiteSpace(asunto) ? "" : asunto;
            mesesTrimestre = string.IsNullOrWhiteSpace(mesesTrimestre) ? "" : mesesTrimestre;
            tipoId = tipoId == -1 ? 0 : tipoId;

            var data = new ExportarData().ExportarNotificaciones(para, asunto, tipoId, fechaRegistro, mesesTrimestre, anio);
            filename = string.Format("ListaNotificaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }


        /*[Route("TipoNotificaciones/Portafolio/Listado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTipoNotificacionesPortafolio()
        {
            HttpResponseMessage response = null;

            var dataRpta = new TipoNotificacionesData();
            var dataTipoNotificaciones = ServiceManager<NotificacionDAO>.Provider.ListarTipoNotificacionesApp(true);            
            dataRpta.ListaTipoNotificaciones = dataTipoNotificaciones;

            var dataTipoFrecuencia = Utilitarios.EnumToList<ETipoFrecuencia>();
            dataRpta.ListaFrecuenciaNotificacion = dataTipoFrecuencia.Select(x => new CustomAutocomplete
            {
                Descripcion = x.ToString(),
                Id = ((int)x).ToString()
            }).ToList();

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }*/

        [Route("TipoNotificaciones/Portafolio/ListadoNotificaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarGestionadoPor(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<NotificacionDAO>.Provider.GetTipoNotificacionesApp(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Asunto = HttpUtility.HtmlEncode(x.Asunto);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            var reader = new BootstrapTable<TipoNotificacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("TipoNotificaciones/Portafolio/Listado/Todo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTipoNotificacionesTodoPortafolio()
        {
            HttpResponseMessage response = null;

            var dataRpta = new TipoNotificacionesData();
            var dataTipoNotificaciones = ServiceManager<NotificacionDAO>.Provider.ListarTipoNotificacionesApp(true);

            dataRpta.ListaTipoNotificaciones = dataTipoNotificaciones;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("TipoNotificaciones/Portafolio/Get/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ObtenerTipoNotificacionesPortafolio(int id)
        {
            HttpResponseMessage response = null;

            var obj = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(id);

            response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [Route("TipoNotificaciones/Portafolio/Update")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ActualizarTipoNotificacionesPortafolio(TipoNotificacionDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            var rpta = ServiceManager<NotificacionDAO>.Provider.AddOrEditTipoNotificacionApp(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }

        [Route("Notificaciones/Portafolio/Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListNotificacionesPortafolio(PaginacionNotificacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AlertaDAO>.Provider.GetNotificacionesPortafolio(pag.Para, pag.Asunto, pag.MesesTrimestre, pag.AnioFiltro, pag.FechaFiltro, pag.TipoNotificacionId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<NotificacionAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Notificaciones/Portafolio/Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportNotificacionesPortafolio(string para, string asunto, int tipoId, DateTime? fechaRegistro, string mesesTrimestre, int? anio)
        {
            string filename = "";
            para = string.IsNullOrWhiteSpace(para) ? "" : para;
            asunto = string.IsNullOrWhiteSpace(asunto) ? "" : asunto;
            mesesTrimestre = string.IsNullOrWhiteSpace(mesesTrimestre) ? "" : mesesTrimestre;
            tipoId = tipoId == -1 ? 0 : tipoId;

            var data = new ExportarData().ExportarNotificacionesPortafolio(para, asunto, tipoId, fechaRegistro, mesesTrimestre, anio);
            filename = string.Format("ListaNotificacionesPortafolio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        //UrlAplicacion

        [Route("UrlAplicacion/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUrlAppCargarCombos()
        {
            HttpResponseMessage response = null;

            //var listTipoMensaje = Utilitarios.EnumToList<ETipoMensaje>();
            var lstUrlFuente = ServiceManager<UrlAplicacionDAO>.Provider.GetUrlFuenteByFiltro(null, null);

            var dataRpta = new
            {
                UrlFuente = lstUrlFuente
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("UrlAplicacion/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUrlAppListado(PaginacionUrlApp pag)
        {
            HttpResponseMessage response = null;

            var registros = ServiceManager<UrlAplicacionDAO>.Provider.GetListado(pag, out int totalRows);

            var reader = new BootstrapTable<UrlAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("UrlAplicacion/ListarDetalle/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUrlAppListarDetalle(int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<UrlAplicacionDAO>.Provider.GetUrlAplicacionEquipo(id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);

            return response;
        }

        [Route("UrlAplicacion/CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUrlAppCambiarEstado(int id, string comentario)
        {
            var user = TokenGenerator.GetCurrentUser();
            string usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<UrlAplicacionDAO>.Provider.CambiarEstado(id, usuario, comentario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("UrlAplicacion/Exportar")]
        [HttpGet]
        public IHttpActionResult GetUrlAppExportar(string equipo,
            DateTime? fecha,
            int fuenteId,
            bool? isOrphan,
            bool isActive,
            string sortName, 
            string sortOrder)
        {
            equipo = string.IsNullOrEmpty(equipo) ? string.Empty : equipo;

            var data = new ExportarData().ExportarUrlAplicacion(equipo, fecha, fuenteId, isOrphan, isActive, sortName, sortOrder);
            string filename = string.Format("ListadoURLsAplicaciones_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("UrlAplicacion/ActualizarAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUrlAppActualizarAplicacion(int id, string codigoAPT)
        {
            var user = TokenGenerator.GetCurrentUser();
            string usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<UrlAplicacionDAO>.Provider.UpdateAplicacionByUrl(id, usuario, codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("UrlAplicacion/AppsCandidatas")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUrlAppsCandidatas(int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<UrlAplicacionDAO>.Provider.GetListAppCandidatasByUrl(id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);

            return response;
        }
    }
}