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
    [RoutePrefix("api/Aplicacion")]
    public class AplicacionController : BaseController
    {
        [Route("Obtener")]
        [ResponseType(typeof(AplicacionPublicDto))]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionByCodigo(string codigoApt)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetAplicacionByCodigo(codigoApt);
            if (entidad == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Aplicación no existe en el portafolio");
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        //POST: api/Aplicacion/TipoExperto
        [Route("TipoExperto")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTipoExperto()
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetTipoExperto();
            if (registros != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, registros);
            }
            return response;
        }

        // POST: api/Aplicacion/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListAplicacion(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;
            pag.PerfilId = user.PerfilId;
            pag.Matricula = user.Matricula;
            HttpResponseMessage response = null;

            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionConfiguracion(pag, out int totalRows, out string arrCodigoAPTs);
            if (registros != null)
            {
                registros.ForEach(x => { x.Agrupacion = HttpUtility.HtmlEncode(x.Agrupacion); });
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros,
                    ArrCodigoAPT = arrCodigoAPTs
                };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;

        }

        [Route("Consultor/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListConsultorAplicacion(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            pag.pageSize = int.MaxValue;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionConsultorByFilter(pag, out int totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Listado/Vista")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListAplicacionVista(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;

            HttpResponseMessage response = null;
            var totalRows = 0;
            if (pag.PerfilId == (int)EPerfilBCP.Administrador || pag.PerfilId == (int)EPerfilBCP.GestorCVT_CatalogoTecnologias)
            {
                var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacion(pag, out totalRows);
                if (registros != null)
                {
                    registros.ForEach(x =>
                    {
                        x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                        x.ListaPCI = HttpUtility.HtmlEncode(x.ListaPCI);
                        x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
                    });
                    var dataRpta = new { Total = totalRows, Rows = registros };
                    response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
                }
                return response;
            }
            else
            {
                var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionVistaConsultor(pag, out totalRows);
                if (registros != null)
                {
                    registros.ForEach(x =>
                    {
                        x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                        x.ListaPCI = HttpUtility.HtmlEncode(x.ListaPCI);
                        x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
                    });
                    var dataRpta = new { Total = totalRows, Rows = registros };
                    response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
                }
                return response;
            }
        }

        // GET: api/Aplicacion/ObtenerAplicacion
        [Route("ObtenerAplicacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAplicacion()
        {
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacion();
            if (listApp == null)
                return NotFound();

            return Ok(listApp);
        }

        [Route("ObtenerAplicacionExperto")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionExperto(string Id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetAplicacionExperto(Id);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ObtenerEtiquetas")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListEtiquetas()
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetListEtiquetas();
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ObtenerAplicacionEtiqueta")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionEtiqueta(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetAplicacionEtiqueta(codigoAPT);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ObtenerUnidadFondeo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListUnidadFondeo()
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }


        [Route("ObtenerAplicacionUnidadFondeo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionUnidadFondeo(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetAplicacionUnidadFondeo(codigoAPT);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GuardarUnidadFondeo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GuardarUnidadFondeo(GuardarUnidadFondeoAplicacionDTO item)
        {
            var user = TokenGenerator.GetCurrentUser();
            item.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            bool result = ServiceManager<AplicacionDAO>.Provider.GuardarUnidadFondeo(item);
            response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("GuardarEtiqueta")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GuardarEtiqueta(GuardarEtiquetaAplicacionDTO item)
        {
            var user = TokenGenerator.GetCurrentUser();
            item.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            bool result = ServiceManager<AplicacionDAO>.Provider.GuardarEtiqueta(item);
            response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("ObtenerAplicacionExpertoPortafolio")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionExpertoPortafolio(string Id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetAplicacionExpertoPortafolio(Id);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("CambiarFlagRelacionar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambiarFlagRelacionar(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            AplicacionDTO objRegistro = new AplicacionDTO();
            objRegistro.CodigoAPT = request.Id;
            objRegistro.UsuarioModificacion = request.Usuario;
            objRegistro.FlagRelacionar = request.Flag;
            bool retorno = ServiceManager<AplicacionDAO>.Provider.CambiarFlagRelacionar(objRegistro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("CambiarFlagExperto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambiarFlagExperto(ObjCambioEstado request)
        {
            HttpResponseMessage response = null;
            bool retorno = ServiceManager<AplicacionDAO>.Provider.CambiarFlagExperto(int.Parse(request.Id), request.Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("CambiarFlagExpertoPortafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambiarFlagExpertoPortafolio(ObjCambioEstado request)
        {
            HttpResponseMessage response = null;
            bool retorno = ServiceManager<AplicacionDAO>.Provider.CambiarFlagExperto(int.Parse(request.Id), request.Usuario, 1);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ActualizarExperto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarExperto(ParametroList request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.AddOrEditAplicacionExperto(request);
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("Consultor/ActualizarExperto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarExpertoConsultor(ParametroConsultor request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<AplicacionDAO>.Provider.AddOrEditAplicacionExpertoConsultor(request);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ActualizarExpertoPortafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarExpertoPortafolio(ParametroList request)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.AddOrEditAplicacionExpertoPortafolio(request);
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GetAplicacionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacionByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetNombreAplicacionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetNombreAplicacionByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetNombreAplicacionByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetAplicacionRelacionarByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionRelacionarByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacionByFiltro(filtro, true);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetAplicacionAprobadaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionAprobadaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacionAprobadaByFiltro(filtro, true);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetAplicacionAprobadaCatalogoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionAprobadaCatalogoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacionAprobadaCatalogoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("ExisteAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteAplicacion(string Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteAplicacionRelacionar")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteAplicacionRelacionar(string Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionById(Id, true);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            FiltrosAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.GetFiltros();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }


        [Route("ListarCombos2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos2()
        {
            HttpResponseMessage response = null;
            FiltrosAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.GetFiltros();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListarCombosConsultor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosConsultor()
        {
            HttpResponseMessage response = null;
            var dataRpta = new
            {
                TipoExperto = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoByFiltro(null)
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }


        [Route("ExportarConfiguracion")]
        [HttpGet]
        public IHttpActionResult PostExportAplicacionesConfiguracion(string nombre
            , string sortName
            , string sortOrder
            , string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string aplicacion
            , string jefeequipo
            , string owner
            , int unidadFondeoId)
        {
            string nomArchivo = "";
            if (nombre == "") nombre = null;
            if (gerencia == "null") gerencia = null;
            else if (gerencia == null) gerencia = "";
            if (division == "null") division = null;
            else if (division == null) division = "";
            if (unidad == "null") unidad = null;
            else if (unidad == null) unidad = "";
            if (area == "null") area = null;
            else if (area == null) area = "";
            if (estado == "null") estado = null;
            else if (estado == null) estado = "";
            if (aplicacion == "null") aplicacion = null;
            else if (aplicacion == null) aplicacion = "";
            if (jefeequipo == "null") jefeequipo = null;
            else if (jefeequipo == null) jefeequipo = "";
            if (owner == "null") owner = null;
            else if (owner == null) owner = "";

            var data = new ExportarData().ExportarAplicacionesConfiguracion(nombre, sortName, sortOrder, gerencia, division, unidad, area, estado, aplicacion, jefeequipo, owner, unidadFondeoId, (int)EPerfilBCP.Administrador, string.Empty, string.Empty);
            nomArchivo = string.Format("ListaAplicacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarResponsables")]
        [HttpGet]
        public IHttpActionResult PostExportAplicacionesResponsables(string nombre
            , string sortName
            , string sortOrder
            , string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string aplicacion
            , string jefeequipo
            , string owner)
        {
            string nomArchivo = "";
            if (nombre == "") nombre = null;
            if (gerencia == "null") gerencia = null;
            else if (gerencia == null) gerencia = "";
            if (division == "null") division = null;
            else if (division == null) division = "";
            if (unidad == "null") unidad = null;
            else if (unidad == null) unidad = "";
            if (area == "null") area = null;
            else if (area == null) area = "";
            if (estado == "null") estado = null;
            else if (estado == null) estado = "";
            if (aplicacion == "null") aplicacion = null;
            else if (aplicacion == null) aplicacion = "";
            if (jefeequipo == "null") jefeequipo = null;
            else if (jefeequipo == null) jefeequipo = "";
            if (owner == "null") owner = null;
            else if (owner == null) owner = "";

            var data = new ExportarData().ExportarResponsablesAplicacion(nombre, sortName, sortOrder, gerencia, division, unidad, area, estado, aplicacion, jefeequipo, owner);
            nomArchivo = string.Format("ListaAplicacionResponsables_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportAplicaciones(string nombre
            , string sortName
            , string sortOrder
            , string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string aplicacion
            , string jefeequipo
            , string owner
            , string pci)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId;
            string nomArchivo = "";
            if (nombre == "") nombre = null;
            if (gerencia == "null") gerencia = null;
            else if (gerencia == null) gerencia = "";
            if (division == "null") division = null;
            else if (division == null) division = "";
            if (unidad == "null") unidad = null;
            else if (unidad == null) unidad = "";
            if (area == "null") area = null;
            else if (area == null) area = "";
            if (estado == "null") estado = null;
            else if (estado == null) estado = "";
            if (aplicacion == "null") aplicacion = null;
            else if (aplicacion == null) aplicacion = "";
            if (jefeequipo == "null") jefeequipo = null;
            else if (jefeequipo == null) jefeequipo = "";
            if (owner == "null") owner = null;
            else if (owner == null) owner = "";
            if (pci == "null") pci = null;
            else if (pci == null) pci = "";

            var data = new ExportarData().ExportarAplicaciones(nombre, sortName, sortOrder, gerencia, division, unidad, area, estado, aplicacion, jefeequipo, owner, perfilId, matricula, pci);
            nomArchivo = string.Format("ListaAplicacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GetJefeEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetJefeEquipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetJefeEquipoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetOwnerByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetOwnerByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetOwnerByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetExpertoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExpertoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetExpertoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetGerenciaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGerenciaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetGerenciaByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetDivisionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDivisionByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetDivisionByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetGestionadoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGestionadoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetGestionadoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetGestorByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGestorByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetGestorByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetAplicacionDetalle")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionDetalle(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<AplicacionDAO>.Provider.GetAplicacionDetalleById(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetAplicacionExpertoByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionExperto(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionExpertoByCodigoAPT(codigoAPT, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GetUnidadByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUnidadByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var lista = ServiceManager<AplicacionDAO>.Provider.GetUnidadByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, lista);
            return response;
        }

        [Route("GetAreaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAreaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var lista = ServiceManager<AplicacionDAO>.Provider.GetAreaByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, lista);
            return response;
        }

        [Route("GetTTLByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTTLByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetTTLByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("GetBrokerByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetBrokerByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetBrokerByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        #region GestionAplicacion

        [Route("GestionAplicacion/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosGestionAplicacion()
        {
            HttpResponseMessage response = null;
            FiltrosAplicacion dataRpta = ServiceManager<AplicacionDAO>.Provider.GetFiltrosGestionAplicacion();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GestionAplicacion/ExisteCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteCodipoAPT(string codigoAPT, int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionById(codigoAPT, Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ExisteCodigoAPT_Nombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteCodigoAPT_Nombre(string filtro, int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionByCodigoNombre(filtro, Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ExisteNombreAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteNombreAplicacion(string nombre, int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionByNombre(nombre, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ExisteCodigoInterfaz")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteCodigoInterfaz(string filtro, int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteCodigoInterfaz(filtro, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ObtenerCodigoInterfazDisponibles")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCodigoInterfazDisponible(string caracter, int id)
        {
            HttpResponseMessage response = null;

            var codigosUsados = ServiceManager<AplicacionDAO>.Provider.GetCodigoInterfazUsados(id);
            char[] combination = ("0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ").ToArray();
            string[] character = { caracter.ToUpper() };

            var codigosUniverso = character.Where(x => x != null)
                                    .SelectMany(g => combination
                                    .Select(c => string.Concat(g, c)));

            var codigosDisponibles = codigosUniverso.Where(x => !codigosUsados.Contains(x.ToUpper())).ToList();

            response = Request.CreateResponse(HttpStatusCode.OK, codigosDisponibles);
            return response;
        }

        [Route("GestionAplicacion/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAplicacion(AplicacionDTO obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;
            obj.AplicacionDetalle.UsuarioCreacion = user.Matricula;
            obj.AplicacionDetalle.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<AplicacionDAO>.Provider.AddOrEditAplicacion(obj);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("GestionAplicacion/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListGestionAplicacion(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetGestionAplicacion(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/GetAplicacionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionById(int Id, int TipoSolicitudId = (int)ETipoSolicitudAplicacion.CreacionAplicacion)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(Id, TipoSolicitudId);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/GetEstadoSolicitudById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEstadoSolicitudById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetEstadoSolicitudAppById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/CambiarEstado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(ParametroEstadoTec obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<AplicacionDAO>.Provider.CambiarEstadoApp(obj.id, obj.est, obj.obs, obj.UsuarioModificacion);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("GestionAplicacion/EliminarAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id, string Estado, string Usuario)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<AplicacionDAO>.Provider.CambiarEstado(Id, Estado, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }


        [Route("GestionAplicacion/ListarColumnaAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListColumnaAplicacion(PaginacionAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetColumnaAplicacion(pag, out totalRows);
            if (registros != null)
            {

                registros.ForEach(x =>
                {
                    x.DescripcionCampo = HttpUtility.HtmlEncode(x.DescripcionCampo);
                    x.NombreExcel = HttpUtility.HtmlEncode(x.NombreExcel);
                    x.RolAprueba = HttpUtility.HtmlEncode(x.RolAprueba);
                    x.RolRegistra = HttpUtility.HtmlEncode(x.RolRegistra);
                    x.RolResponsableActualizacion = HttpUtility.HtmlEncode(x.RolResponsableActualizacion);
                });


                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/ReordenarColumnaAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListColumnaAplicacionReordenar(List<ConfiguracionColumnaAplicacionOrdenDTO> lista)
        {
            HttpResponseMessage response = null;
            HttpRequest request = HttpContext.Current.Request;
            string username = string.Empty;
            username = request.Params["Usuario"];
            var rpta = ServiceManager<AplicacionDAO>.Provider.ReordenarColumnaApp(lista, username);

            response = Request.CreateResponse(HttpStatusCode.OK, rpta);

            return response;
        }

        [Route("GestionAplicacion/AddOrEditColumnaApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostColumnaApp(ConfiguracionColumnaAplicacionDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;
            entidad.InfoCampoPortafolio.UsuarioCreacion = user.Matricula;
            entidad.InfoCampoPortafolio.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            long entidadId = ServiceManager<AplicacionDAO>.Provider.AddOrEditColumnaApp(entidad);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("GestionAplicacion/GetColumnaAppById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaAppById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetColumnaAppById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/ListarPublicacionAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPublicacionAplicacion(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetPublicacionAplicacion(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/ListarPublicacionAplicacion2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPublicacionAplicacion2(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetPublicacionAplicacion2(pag, out totalRows);
            if (registros != null)
            {
                foreach (DataRow row in registros.Rows)
                {
                    row["applicationName"] = HttpUtility.HtmlEncode(row["applicationName"]);
                    row["description"] = HttpUtility.HtmlEncode(row["description"]);
                    row["BIANdomain"] = HttpUtility.HtmlEncode(row["BIANdomain"]);
                    row["BIANarea"] = HttpUtility.HtmlEncode(row["BIANarea"]);
                    row["webDomain"] = HttpUtility.HtmlEncode(row["webDomain"]);
                    row["functionalLayer"] = HttpUtility.HtmlEncode(row["functionalLayer"]);
                    row["assetType"] = HttpUtility.HtmlEncode(row["assetType"]);
                    row["TipoPCI"] = HttpUtility.HtmlEncode(row["TipoPCI"]);
                }

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/ListarPublicacionAplicacionAsignada")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPublicacionAplicacionAsignada(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetPublicacionAplicacionAsignada(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/ListarCatalogoAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCataogoAplicacion(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetPublicacionAplicacionCatalogo(pag, out totalRows);
            if (registros != null)
            {

                foreach (DataRow row in registros.Rows)
                {
                    row["applicationName"] = HttpUtility.HtmlEncode(row["applicationName"]);
                    row["description"] = HttpUtility.HtmlEncode(row["description"]);
                    row["BIANdomain"] = HttpUtility.HtmlEncode(row["BIANdomain"]);
                    row["BIANarea"] = HttpUtility.HtmlEncode(row["BIANarea"]);
                    row["webDomain"] = HttpUtility.HtmlEncode(row["webDomain"]);
                    row["functionalLayer"] = HttpUtility.HtmlEncode(row["functionalLayer"]);
                    row["assetType"] = HttpUtility.HtmlEncode(row["assetType"]);
                    row["TipoPCI"] = HttpUtility.HtmlEncode(row["TipoPCI"]);
                    row["technicalClassification"] = HttpUtility.HtmlEncode(row["technicalClassification"]);
                }

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }



        [Route("GestionAplicacion/Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportReportePublicacionAplicaciones(string TablaProcedencia, int Procedencia)
        {
            string filename = string.Empty;
            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            var data = new ExportarData().ExportarReportePublicacionAplicacion(paginacion);
            filename = string.Format("ReporteAplicacionPublicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }


        [Route("GestionAplicacion/GetColumnaAplicacionToJS")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaAplicacionToJS(string tablaProcedencia)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetColumnasPublicacionAplicacionToJS(tablaProcedencia);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }


        [Route("GestionAplicacion/GetColumnaAplicacionToJS2")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaAplicacionToJS2(string tablaProcedencia)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetColumnasPublicacionAplicacionToJS2(tablaProcedencia);

            entidad.ForEach(x => { x.title = HttpUtility.HtmlEncode(x.title); });

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/GetColumnaAplicacionToJSCatalogo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaAplicacionToJSCatalogo(string tablaProcedencia)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetColumnasPublicacionAplicacionToJSCatalogo(tablaProcedencia);
            entidad.ForEach(x => { x.title = HttpUtility.HtmlEncode(x.title); });
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/ExisteOrden")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetOrden(int ordenNuevo, int ordenActual)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteOrden(ordenNuevo, ordenActual);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ExisteNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetNombre(string nombre, int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteNombre(nombre, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionAplicacion/ListarCombosToColumnaAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostListCombosColumnaAplicacion(string tablaProcedencia)
        {
            HttpResponseMessage response = null;
            //var lTablaProcedencia = string.Format("{0};{1}", (int)ETablaProcedenciaAplicacion.Aplicacion, (int)ETablaProcedenciaAplicacion.AplicacionDetalle);
            var lColumnaAplicacion = ServiceManager<SolicitudAplicacionDAO>.Provider.GetColumnaAplicacionByFiltro(null, tablaProcedencia);
            //var listaEstados = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla("Estado", null);
            //var listaEstadosRegistros = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ESTADOS_APLICACION);

            var lGenericFlag = Utilitarios.EnumToList<EGenericFlag>();
            var lTipoFlujo = Utilitarios.EnumToList<EFlujoRegistro>();
            var lTipoInput = Utilitarios.EnumToList<ETipoInputHTML>();
            var activoAplica = Utilitarios.EnumToList<ActivoAplica>();
            var modoLlenado = Utilitarios.EnumToList<ModoLlenado>();
            var tipoRegistro = Utilitarios.EnumToList<TippRegistroDato>();
            var nivelConfiabilidad = Utilitarios.EnumToList<NivelConfiabilidad>();

            var dataRpta = new
            {
                ColumnaAplicacion = lColumnaAplicacion,
                TipoFlujo = lTipoFlujo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                GenericFlag = lGenericFlag.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoInput = lTipoInput.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                ActivoAplica = activoAplica.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                ModoLlenado = modoLlenado.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoRegistro = tipoRegistro.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                NivelConfiabilidad = nivelConfiabilidad.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //Estado = listaEstados,
                //EstadoFiltro = listaEstadosRegistros.Valor
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GestionAplicacion/GetParametroApp")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetParametroApp(string Codigo)
        {
            HttpResponseMessage response = null;
            string retorno = string.Empty;
            //var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.CODIGO_MATRICULAS_ADMIN_PORTAFOLIO);
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Codigo);
            retorno = parametro != null ? parametro.Valor : "";
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GestionAplicacion/ListarModuloAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListModuloAplicacion(PaginacionAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetModuloAplicacion(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/CambiarEstadoModulo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id, string Usuario, bool Estado)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<AplicacionDAO>.Provider.CambiarEstadoModulo(Id, !Estado, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GestionAplicacion/GetCodigoModulo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCodigoModulo(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<AplicacionDAO>.Provider.GetCodigoModulo(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GestionAplicacion/ListarParametroAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListParametroAplicacion(PaginacionAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetParametroAplicacion(pag, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x => { x.Valor = HttpUtility.HtmlEncode(x.Valor); });

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/AddOrEditParametro")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostParametroApp(ParametroDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<AplicacionDAO>.Provider.AddOrEditParametro(entidad);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("GestionAplicacion/GetModuloById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetModuloById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetModuloById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GestionAplicacion/AddOrEditModulo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AddOrEditModulo(ModuloAplicacionDTO entidad)
        {
            HttpResponseMessage response = null;
            int entidadId = ServiceManager<AplicacionDAO>.Provider.AddOrEditModuloAplicacion(entidad);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("GestionAplicacion/ListadoCatalogo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCatalogo(PaginacionAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionPortafolio(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/Exportar/Actualizar")]
        [HttpGet]
        public IHttpActionResult GetExportarAplicacionPortafolioActualizar()
        {
            var data = new ExportarData().ExportarAplicacionPortafolioUpdate();
            string filename = "ListadoPortafolioAplicaciones";
            string nomArchivo = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionAplicacion/Exportar/ValidacionCargaMasiva")]
        [HttpGet]
        public IHttpActionResult GetExportarAplicacionPortafolioValidacionCargaMasiva()
        {
            var data = new ExportarData().ExportarAplicacionPortafolioValidacionesCargaMasiva();
            string filename = "ValidacionesCargaMasiva";
            string nomArchivo = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionAplicacion/Portafolio/Backup")]
        [HttpPost]
        public HttpResponseMessage GetExportarAplicacionPortafolioBackup(DataPortafolioBackup data)
        {
            var user = TokenGenerator.GetCurrentUser();
            data.UsuarioCreacion = user.Matricula;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = "4",
                Procedencia = 4,
                Gerencia = string.Empty,
                Division = string.Empty,
                Estado = string.Empty,
                Area = string.Empty,
                Unidad = string.Empty,
                ClasificacionTecnica = string.Empty,
                SubclasificacionTecnica = string.Empty,
                TipoActivo = string.Empty,
                Aplicacion = string.Empty,
                pageNumber = 1,
                pageSize = int.MaxValue,
                LiderUsuario = string.Empty,
                GestionadoPor = string.Empty,
                Exportar = "1",
                TipoPCI = string.Empty,
                AppReactivadas = -1
            };

            var dataBytes = new ExportarData().ExportarReportePublicacionAplicacionCatalogoAplicaciones(paginacion);

            ServiceManager<AplicacionDAO>.Provider.GetResultsCargaMasivaPortafolio(new PortafolioBackupDTO
            {
                Comentario = data.Comentarios,
                UsuarioCreacion = data.UsuarioCreacion == null ? "AUTO" : data.UsuarioCreacion,
                BackupBytes = dataBytes,
                FechaCreacion = DateTime.Now
            });

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            return httpResponseMessage;
        }

        [Route("GestionAplicacion/Portafolio/Backup/Listar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPortafolioBackup(PaginacionPortafolioBackup pag)
        {
            HttpResponseMessage response = null;

            pag.FechaDesde = pag.FechaDesde.HasValue ? pag.FechaDesde.Value.Date : (DateTime?)null;
            pag.FechaHasta = pag.FechaHasta.HasValue ? pag.FechaHasta.Value.Date.AddDays(1).AddMinutes(-1) : (DateTime?)null;

            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionPortafolioBackups(pag, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x => { x.Comentario = HttpUtility.HtmlEncode(x.Comentario); });

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/Portafolio/Backup/Obtener")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListPortafolioBackup(int idBackup)
        {

            var data = ServiceManager<AplicacionDAO>.Provider.GetAplicacionPortafolioBackupById(idBackup);
            string filename = "BackupPortafolioAplicaciones";
            string nomArchivo = string.Format("{0}_{1}.xlsx", filename, data.FechaCreacion.ToString("ddMMyyyy_HHmmss"));

            return Ok(new { excel = data.BackupBytes, name = nomArchivo });
        }


        [Route("GestionAplicacion/ActualizarAplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarAplicaciones()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;
                string username = string.Empty;
                username = request.Params["Usuario"];

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);

                    var retorno = new CargaDataAplicacion().CargaMasivaAplicacionUpdate(extension, inputStream, username, null, null);
                    response = Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region Portafolio
        [Route("Portafolio/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPortafolioAplicacion(PaginacionAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetReporteAplicacionData(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;

        }

        [Route("Portafolio/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivision(string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string clasificacionTecnica
            , string subclasificacionTecnica
            , string aplicacion
            , string sortName
            , string sortOrder
            , string TablaProcedencia
            , int Procedencia
            , string TipoActivo
            )
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue
            };

            var data = new ExportarData().ExportarReportePublicacionAplicacion(paginacion);
            filename = string.Format("PortafolioDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("Portafolio/Exportar2")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivisio2(string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string clasificacionTecnica
            , string subclasificacionTecnica
            , string aplicacion
            , string sortName
            , string sortOrder
            , string TablaProcedencia
            , int Procedencia
            , string TipoActivo
            , string TipoPCI
            , string Exportar
            , string GestionadoPor
            , string LiderUsuario)
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            TipoPCI = string.IsNullOrEmpty(TipoPCI) ? string.Empty : TipoPCI;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;
            GestionadoPor = string.IsNullOrEmpty(GestionadoPor) ? string.Empty : GestionadoPor;
            LiderUsuario = string.IsNullOrEmpty(LiderUsuario) ? string.Empty : LiderUsuario;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                TipoPCI = TipoPCI,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue,
                Exportar = Exportar,
                GestionadoPor = GestionadoPor,
                LiderUsuario = LiderUsuario
            };

            var data = new ExportarData().ExportarReportePublicacionAplicacionNuevo(paginacion);
            filename = string.Format("PortafolioDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("TAI/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetActivosById(int id)
        {
            var objPar = ServiceManager<ActivosDAO>.Provider.GetActivosById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }

        [Route("TAI/GetActivosByNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetActivosByNombre(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetActivosByNombre(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("TAI/CambiarEstadoActivos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoActivos(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<ActivosDAO>.Provider.GetActivosById(Id);
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("TAI/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActivos(ActivosDTO actDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            actDTO.UsuarioCreacion = user.Matricula;
            actDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<ActivosDAO>.Provider.AddOrEditActivos(actDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }

        [Route("TAI/ListadoActivos")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListActivos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetActivos(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
            });

            dynamic reader = new BootstrapTable<ActivosDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("TAI/ListarCombosActivos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosActivos()
        {
            HttpResponseMessage response = null;

            var dataRpta = new
            {
                Flujos = ServiceManager<ActivosDAO>.Provider.GetFlujos(),
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("CodigoReservado/ListadoCodigo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListCodigoReservado(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetCodigoReservado(pag.nombre, pag.tipoCodigo, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Comentarios = HttpUtility.HtmlEncode(x.Comentarios); });

            dynamic reader = new BootstrapTable<CodigoReservadoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        [Route("CodigoReservado/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCodigoReservadoById(int id)
        {
            var objPar = ServiceManager<AplicacionDAO>.Provider.GetCodigoReservadoById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }

        [Route("CodigoReservado/CambiarEstadoCodigo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoCodigo(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetCodigoReservadoById(Id);
            var retorno = ServiceManager<AplicacionDAO>.Provider.CambiarEstadoCodigo(Id, !entidad.FlagActivo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
        
        [Route("CodigoReservado/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCodigoReservado(CodigoReservadoDTO codDTO)
        {
            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<AplicacionDAO>.Provider.AddOrEditCodigo(codDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }
        
        [Route("CodigoReutilizado/ListadoCodigo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListCodigoReutilizado(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetCodigoReutilizado(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Comentarios = HttpUtility.HtmlEncode(x.Comentarios); });

            dynamic reader = new BootstrapTable<CodigoReservadoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        [Route("CodigoReutilizado/CodigoReservadoExists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CodigoReservadoExists(string codigo)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.GetCodigoReutilizadoExists(codigo);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }
        [Route("CodigoReutilizado/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCodigoReutilizadoById(int id)
        {
            var objPar = ServiceManager<AplicacionDAO>.Provider.GetCodigoReutilizadoById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }
        [Route("CodigoReutilizado/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCodigoReutilizado(CodigoReservadoDTO codDTO)
        {
            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<AplicacionDAO>.Provider.AddOrEditCodigoReutilizado(codDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }
        [Route("CodigoReutilizado/CambiarEstadoCodigo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CambiarEstadoCodigoReutilizado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetCodigoReutilizadoById(Id);
            var retorno = ServiceManager<AplicacionDAO>.Provider.GetCambiarEstadoCodigoReutilizado(Id, !entidad.FlagActivo, entidad.FlagEliminado, entidad.Codigo, entidad.Comentarios, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
        [Route("CodigoReutilizado/EliminarCodigo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EliminarCodigoReutilizado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetCodigoReutilizadoById(Id);
            var retorno = ServiceManager<AplicacionDAO>.Provider.GetEliminarCodigoReutilizado(Id, false, true, entidad.Codigo, entidad.Comentarios, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GrupoRemedy/ListadoGrupo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListGrupo(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetGrupo(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<GrupoTicketRemedyDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("GrupoRemedy/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetGrupoById(int id)
        {
            var objPar = ServiceManager<AplicacionDAO>.Provider.GetGrupoById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }

        [Route("GrupoRemedy/CambiarEstadoGrupo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoGrupo(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetGrupoById(Id);
            var retorno = ServiceManager<AplicacionDAO>.Provider.CambiarEstadoGrupo(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
        [Route("GrupoRemedy/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostGrupo(GrupoTicketRemedyDto grpDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            grpDTO.UsuarioCreacion = user.Matricula;
            grpDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<AplicacionDAO>.Provider.AddOrEditGrupo(grpDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }
        #endregion

        #region CONFIGURACION_PORTAFOLIO

        [Route("ConfiguracionPortafolio/ListarAreaBian")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarAreaBian(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetAreaBian(pag, out totalRows);

            registros.ForEach(x => { x.Nombre = HttpUtility.HtmlEncode(x.Nombre); });

            var reader = new BootstrapTable<AreaBianDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarDominioBian")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarDominioBian(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetDominioBian(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            var reader = new BootstrapTable<DominioBianDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarJefaturaAti")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarJefaturaAti(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetJefaturaAti(pag, out totalRows);

            var reader = new BootstrapTable<JefaturaAtiDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarGerencia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarGerencia(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetGerencia(pag, out totalRows);

            registros.ForEach(x => { x.Nombre = HttpUtility.HtmlEncode(x.Nombre); });

            var reader = new BootstrapTable<GerenciaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarArea")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarArea(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetArea(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });
            var reader = new BootstrapTable<AreaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarDivision")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarDivision(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetDivision(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            var reader = new BootstrapTable<DivisionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarUnidad")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarUnidad(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetUnidad(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            var reader = new BootstrapTable<UnidadDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarCuestionarioPae")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarCuestionarioPae(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetCuestionarioPae(pag, out totalRows);

            var reader = new BootstrapTable<CuestionarioPaeDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarPreguntaPae")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarPreguntaPae(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetPreguntaPae(pag, out totalRows);

            var reader = new BootstrapTable<PreguntaPaeDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarBandeja")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarBandeja(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetBandeja(pag, out totalRows);

            var reader = new BootstrapTable<BandejaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarBandejaAprobacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarBandejaAprobacion(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetBandejaAprobacion(pag, out totalRows);

            var reader = new BootstrapTable<BandejaAprobacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditAreaBian")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditAreaBian(AreaBianDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditAreaBian(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditDominioBian")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditDominioBian(DominioBianDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditDominioBian(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditJefaturaAti")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditJefaturaAti(JefaturaAtiDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditJefaturaAti(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditGerencia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditGerencia(GerenciaDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditGerencia(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditArea")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditArea(AreaDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditArea(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditDivision")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditDivision(DivisionDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditDivision(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditUnidad")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditUnidad(UnidadDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditUnidad(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditCuestionarioPae")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditCuestionarioPae(CuestionarioPaeDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditCuestionarioPae(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditPreguntaPae")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditPreguntaPae(PreguntaPaeDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditPreguntaPae(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditBandeja")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditBandeja(BandejaDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditBandeja(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditBandejaAprobacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditBandejaAprobacion(BandejaAprobacionDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditBandejaAprobacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/GetAreaBianById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAreaBianById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetAreaBianById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetDominioBianById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDominioBianById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDominioBianById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetJefaturaAtiById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetJefaturaAtiById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetGerenciaById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGerenciaById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetGerenciaById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetAreaById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAreaById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetAreaById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetDivisionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDivisionById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDivisionById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetUnidadById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUnidadById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetUnidadById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetCuestionarioPaeById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCuestionarioPaeById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetCuestionarioPaeById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetPreguntaPaeById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetPreguntaPaeById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetPreguntaPaeById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetBandejaById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetBandejaById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetBandejaById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetBandejaAprobacionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetBandejaAprobacionById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetBandejaAprobacionById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetGerenciaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGerenciaPortafolioByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetGerenciaByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetDivisionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDivisionPortafolioByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDivisionByFiltro(filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetAreaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAreaPortafolioByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetAreaByFiltro(filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetUnidadByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUnidadPortafolioByFiltro(string filtro, string filtroPadre)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetUnidadByFiltro(filtro, filtroPadre);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetUnidadByFiltro/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUnidadPortafolioById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetUnidadById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetChapterByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetChapter(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetChapter(pag.Chapter, pag.Tribu);


            return Ok(colorStatus);
        }

        [Route("ConfiguracionPortafolio/GetTribuByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetTribu(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetTribu(pag.Tribu);


            return Ok(colorStatus);
        }

        [Route("ConfiguracionPortafolio/GetGrupoRedByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetGrupoRed(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetGrupoRed(pag.GrupoRed);


            return Ok(colorStatus);
        }


        [Route("ConfiguracionPortafolio/GetProductoByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetProd(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProd(pag.Producto);


            return Ok(colorStatus);
        }

        [Route("ConfiguracionPortafolio/ListadoByDescripcion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListProductosByDescripcion(string descripcion, string dominioIds = null, string subDominioIds = null)
        {
            var registros = ServiceManager<ProductoDAO>.Provider.GetProductoByDescripcion(descripcion, dominioIds, subDominioIds);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }


        [Route("ConfiguracionPortafolio/GetRolByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetRol(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRol(pag.Rol);


            return Ok(colorStatus);
        }

        //[Route("ConfiguracionPortafolio/GetTribuByFiltro")]
        //[HttpPost]
        //[Authorize]
        //public IHttpActionResult GetTribu2(PaginacionSolicitud pag)
        //{

        //    var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetTribu2(pag.Tribu);


        //    return Ok(colorStatus);
        //}



        [Route("ConfiguracionPortafolio/GetFuncionByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetFuncion(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncion(pag.Chapter, pag.Funcion);


            return Ok(colorStatus);
        }

        [Route("ConfiguracionPortafolio/existsRolNuevo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsRolNuevo(string id, string prod)
        {
            HttpResponseMessage response = null;

            int pro = Convert.ToInt32(prod);
            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsRolNuevo(id, pro);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }


        [Route("ConfiguracionPortafolio/existsRol")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsRol(string id, string prod, string rolid)
        {
            HttpResponseMessage response = null;

            int pro = Convert.ToInt32(prod);
            int roli = Convert.ToInt32(rolid);
            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsRol(id, pro, roli);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/existsRolEnFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetExistsRolEnFuncion(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;


            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsRolEnFuncion(pag.Chapter, pag.Funcion, pag.ProductoId, pag.RolProductoId);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/ValidarProductoPerteneciente")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ValidarProductoPerteneciente(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;


            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ValidarProductoPerteneciente(pag.ProductoId, pag.CodigoTribu);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/existsFuncionEnRol")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetExistsFuncionEnRol(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;


            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsFuncionEnRol(pag.Chapter, pag.FuncionMultiple, pag.Producto, pag.Rol);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }
        [Route("ConfiguracionPortafolio/existsGrupoRedNuevo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsGrupoRedNuevo(string id, string prod)
        {
            HttpResponseMessage response = null;
            int pro = Convert.ToInt32(prod);

            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsGrupoRedNuevo(id, pro);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/existsGrupoRed")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsGrupoRed(string id, string prod, string rolid)
        {
            HttpResponseMessage response = null;
            int pro = Convert.ToInt32(prod);
            int roli = Convert.ToInt32(rolid);
            bool estado = ServiceManager<SolicitudAplicacionDAO>.Provider.ExistsGrupoRed(id, pro, roli);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }


        [Route("ConfiguracionPortafolio/GetAreaBianByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAreaBianByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetAreaBianByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetDominioBianByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDominioBianByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDominioBianByFiltro(filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetJefaturaAtiByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetJefaturaAtiByFiltro(string filtro1, string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(filtro1, filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        //CuestionarioTODO
        [Route("ConfiguracionPortafolio/AddOrEditCuestionarioAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditCuestionarioAplicacion(CuestionarioAplicacionDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;
            //DANIELQG93
            if (objDTO.CuestionarioAplicacionDetalle != null && objDTO.CuestionarioAplicacionDetalle.Count > 0)
            {
                foreach (var item in objDTO.CuestionarioAplicacionDetalle)
                {
                    item.UsuarioCreacion = user.Matricula;
                    item.UsuarioModificacion = user.Matricula;
                }
            }

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditCuestionarioAplicacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/GetCuestionarioByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCuestionarioAplicacionByCodigoAPT(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetCuestionarioAplicacionByCodigoAPT(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoGerencia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoGerencia(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoGerencia(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoDivision")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoDivision(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoDivision(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoArea")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoArea(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoArea(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoUnidad")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoUnidad(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoUnidad(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoAreaBian")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoAreaBian(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoAreaBian(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoDominioBian")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoDominioBian(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoDominioBian(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoBandejaAprobacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoBandejaAprobacion(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoBandejaAprobacion(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoJefaturaAti")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoJefaturaAti(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoJefaturaAti(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExisteMatriculaEnBandeja")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteMatriculaEnBandeja(string filtro, int bandejaId, int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ActivosDAO>.Provider.ExisteMatriculaEnBandeja(filtro, bandejaId, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarProcesoVital")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarProcesoVital(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetProcesoVital(pag, out totalRows);

            var reader = new BootstrapTable<ProcesoVitalDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetProcesoVitalById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetProcesoVitalById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetProcesoVitalById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditProcesoVital")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditProcesoVital(ProcesoVitalDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditProcesoVital(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoProcesoVital")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoProcesoVital(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoProcesoVital(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetProcesoClaveEsVital")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetProcesoClaveEsVital(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.ProcesoClaveEsVital(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetPuntuacionByEstandar")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetPuntuacionByEstandar(int tipoId, string nombre, int flagPc)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetPuntuacionByEstandar(tipoId, nombre, flagPc);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CalculoPorcentajeEstandares")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetCalculoPorcentajeEstandares(ObjCalculoNiveles obj)
        {
            HttpResponseMessage response = null;

            //Valores por default
            var L = new double[] { 0.33, 0.25 };
            var M = new double[] { 0.5, 0 };
            double N = 1;
            int FlagPC = 1;

            var parametroL = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.CODIGO_L_CUESTIONARIO_PORTAFOLIO);
            string strValorL = parametroL != null ? parametroL.Valor : "";

            var parametroM = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.CODIGO_M_CUESTIONARIO_PORTAFOLIO);
            string strValorM = parametroM != null ? parametroM.Valor : "";

            var parametroN = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.CODIGO_N_CUESTIONARIO_PORTAFOLIO);
            string strValorN = parametroN != null ? parametroN.Valor : "";

            var parametroPC = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.CODIGO_PC_CUESTIONARIO_PORTAFOLIO);
            string strValorPC = parametroPC != null ? parametroPC.Valor : "";

            if (!string.IsNullOrEmpty(strValorL))
                L = strValorL.Split('|').Select(double.Parse).ToArray();

            if (!string.IsNullOrEmpty(strValorM))
                M = strValorM.Split('|').Select(double.Parse).ToArray();

            if (!string.IsNullOrEmpty(strValorN))
                N = double.TryParse(strValorN, out N) ? N : N;

            if (!string.IsNullOrEmpty(strValorPC))
                FlagPC = int.TryParse(strValorPC, out FlagPC) ? FlagPC : FlagPC;

            var retorno = ServiceManager<ActivosDAO>.Provider.CalcularPorcentajeEstandar(obj.AplicacionId, FlagPC, L, M, N,
                obj.SO, obj.HP, obj.BD, obj.FW, obj.NLCS);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarEstandarPortafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEstandarPortafolio(PaginacionEstandar pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            //var registros = ServiceManager<ActivosDAO>.Provider.GetEstandarPortafolio(pag, out totalRows);
            var registros = ServiceManager<ActivosDAO>.Provider.GetEstandarPortafolioTecnologia(pag, out totalRows);

            var reader = new BootstrapTable<EstandarPortafolioDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetEstandarPortafolioById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEstandarPortafolioById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetEstandarPortafolioById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditEstandarPortafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditEstandarPortafolio(EstandarDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditEstandarPortafolio(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoEstandarPortafolio")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoEstandarPortafolio(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoEstandarPortafolio(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CargarCombosEstandarPortafolio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEstandarPortafolioCargarCombos()
        {
            HttpResponseMessage response = null;
            var lTipoEstandar = Utilitarios.EnumToList<ETipoEstandarPortafolio>();
            var lEstado = Utilitarios.EnumToList<ETecnologiaEstado>();
            var lTipoTecnologia = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);

            var dataRpta = new
            {
                TipoEstandar = lTipoEstandar.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Estado = lEstado.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoTecnologia = lTipoTecnologia,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarServidorAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarServidorAplicacion(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetServidorAplicacion(pag, out totalRows);

            var reader = new BootstrapTable<ServidorAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditServidorAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditServidorAplicacion(ServidorAplicacionDTO objDTO)
        {
            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditServidorAplicacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditListServidorAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditListServidorAplicacion(DataServidorAplicacion objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.AddOrEditListServidorAplicacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoServidorAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoServidorAplicacion(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoServidorAplicacion(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExisteServidorByCodigoApt")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteServidorByCodigoApt(string codigoAPT, string nombreServidor)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ActivosDAO>.Provider.ExisteServidorByCodigApt(codigoAPT, nombreServidor);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoCuestionarioPae")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoCuestionarioPae(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoCuestionarioPae(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoPreguntaPae")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoPreguntaPae(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoPreguntaPae(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        //Crud Arquitecto TI
        [Route("ConfiguracionPortafolio/ListarArquitectoTi")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarArquitectoTi(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetArquitectoTi(pag, out totalRows);

            var reader = new BootstrapTable<ArquitectoTiDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoArquitectoTi")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoArquitectoTi(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoArquitectoTi(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditArquitectoTi")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditArquitectoTi(ArquitectoTiDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditArquitectoTi(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetArquitectoTiById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetArquitectoTiById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetArquitectoTiById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetArquitectoTiByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetArquitectoTiByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetArquitectoTiByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExisteMatriculaEnJefaturaAti")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteMatriculaEnJefaturaAti(string filtro, int entidadRelacionId, int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ActivosDAO>.Provider.ExisteMatriculaEnJefaturaAti(filtro, entidadRelacionId, id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        //End Crud Arquitecto TI

        [Route("ConfiguracionPortafolio/GetUltimoCodigoAptPAE")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUltimoCodigoAptPAE()
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetUltimoCodigoAptPAE();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarGestionadoPor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarGestionadoPor(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetGestionadoPor(pag, out totalRows);

            registros.ForEach(x => { x.Nombre = HttpUtility.HtmlEncode(x.Nombre); });

            var reader = new BootstrapTable<GestionadoPorDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExportarGestionadoPor")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionadoPor(string sortName, string sortOrder)
        {
            var data = new ExportarData().ExportarGestionadoPor(sortName, sortOrder);
            string filename = string.Format("ListadoGestionadoPorInactivos_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ConfiguracionPortafolio/ExisteCodigoSIGAByFilter")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteCodigoSIGAByFilter(string filter)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.ExisteCodigoSIGAByFilter(filter);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetGestionadoPorById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGestionadoPorById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditGestionadoPor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditGestionadoPor(GestionadoPorDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditGestionadoPor(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoGestionadoPor")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoGestionadoPor(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoGestionadoPor(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetGestionadoPorByNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGestionadoPorByNombre(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorByNombre(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetTeamSquadByGestionadoPorId")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTeamSquadByGestionadoPorId(PaginationTeamSquad pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<ActivosDAO>.Provider.GetTeamSquadByGestionadoPor(pag, out int totalRows);

            var reader = new BootstrapTable<TeamSquadDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetTribeLeaderByGestionadoPorId")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTribeLeaderByGestionadoPorId(PaginationTeamSquad pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<ActivosDAO>.Provider.GetTribeLeaderByGestionadoPor(pag, out int totalRows);

            var reader = new BootstrapTable<TribeLeaderDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/UpdateResponsibleTeamSquad")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpdateResponsibleTeamSquad(TeamSquadDTO obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.UpdateResponsibleTeamSquad(obj);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/UpdateResponsibleTribeLeader")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpdateResponsibleTribeLeader(TribeLeaderDTO obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var state = ServiceManager<ActivosDAO>.Provider.UpdateResponsibleTribeLeader(obj, out string message);
            var retorno = new
            {
                state,
                message
            };

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("TAI/ListarHistoricoModificaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarHistoricoModificaciones(PaginacionHistoricoModificacion pag)
        {
            var totalRows = 0;
            HttpResponseMessage response = null;
            var registros = ServiceManager<ActivosDAO>.Provider.GetHistoricoModificacionTAI(pag, out totalRows);

            var retorno = new BootstrapTable<AuditoriaTipoActivoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }


        [Route("ConfiguracionPortafolio/ListarPlataformaBcp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarPlataformaBcp(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetPlataformaBcp(pag, out totalRows);
            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });
            var reader = new BootstrapTable<PlataformaBcpDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditPlataformaBcp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditPlataformaBcp(PlataformaBcpDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditPlataformaBcp(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoPlataformaBcp")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoPlataformaBcp(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoPlataformaBcp(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetPlataformaBcpById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetPlataformaBcpById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetPlataformaBcpById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetPlataformaBcpByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetPlataformaBcpByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetPlataformaBcpByFiltro(filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExisteRelacionByConfiguracion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteRelacionByConfiguracion(int id, int idConfiguracion, int? idEntidadRelacion)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.ExisteRelacionByConfiguracion(id, idConfiguracion, idEntidadRelacion);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/EliminarRegistroByConfiguracion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEliminarRegistroByConfiguracion(int id, int idConfiguracion)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;
            string NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.EliminarRegistroConfiguracion(id, idConfiguracion, Usuario, NombreUsuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetGestionadoPorByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGestionadoPorByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorByFiltro(filtro).Select(x => x.Descripcion).ToArray();
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetNewFieldsPortafolioByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetNewFieldsByFiltro(int idTipoFlujo, string codigoAPT)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetAllToolTipPortafolio(idTipoFlujo, true, codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExportarEstandarPortafolio")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostExportar(int? TipoTecnologiaId, int? EstadoId, string sortName, string sortOrder)
        {
            string filename = "";

            var data = new ExportarData().ExportarEstandarPortafolio(TipoTecnologiaId, EstadoId, sortName, sortOrder);
            filename = string.Format("ListadoEstándarPortafolio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ConfiguracionPortafolio/GetDataInputPortafolioByConfiguracion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDataInputPortafolioByConfiguracion(int filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDataInfoCampoPortafolioByConfiguracion(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetDataListBoxByConfiguracion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDataListBoxByConfiguracion(int filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetDataListBoxByConfiguracion(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExisteNombreEntidadByConfiguracion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteNombreEntidadByConfiguracion(int id, string filtro, int idConfiguracion, int? entidadRelacionId = null)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.ExisteNombreEntidadByConfiguracion(id, filtro, idConfiguracion, entidadRelacionId);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/Bitacora")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostBitacora(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetBitacora(objDTO, out totalRows);

            var reader = new BootstrapTable<BitacoraDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }


        [Route("ConfiguracionPortafolio/Bitacora2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostBitacora2(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetBitacora2(objDTO, out totalRows);

            registros.ForEach(x => { x.NombreAplicacion = HttpUtility.HtmlEncode(x.NombreAplicacion); });

            var reader = new BootstrapTable<BitacoraDto2>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/Productos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProductos(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Matricula = user.Matricula;
            objDTO.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProductos(objDTO, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                x.SubDominioStr = HttpUtility.HtmlEncode(x.SubDominioStr);
            });

            var reader = new BootstrapTable<ProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/SolicitudesFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage SolicitudesFuncion(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Matricula = user.Matricula;
            objDTO.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var totalRows = 0;
            if (objDTO.FechaAtencionSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaAtencionSolicitud2 = null;
            }
            else objDTO.FechaAtencionSolicitud2 = DateTime.Today;
            if (objDTO.FechaRegistroSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaRegistroSolicitud2 = null;
            }
            else objDTO.FechaRegistroSolicitud2 = DateTime.Today;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionSolicitudes(objDTO, out totalRows);

            var reader = new BootstrapTable<SolicitudFuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }


        [Route("ConfiguracionPortafolio/ProductosAdmin")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProductosAdmin(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProductosAdmin(objDTO, out totalRows);

            var reader = new BootstrapTable<ProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/Exportar/ExportarProductoRoles")]
        [HttpGet]
        public IHttpActionResult GetExportarProductosRoles(string producto, int dominioId, int subDominioId, string tribu, string squad, string codigoTribu, string codigo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            string perfil = user.Perfil;

            if (producto == null)
            {
                producto = "";
            }
            string nomArchivo = string.Format("ReporteRolesProductoConsolidado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss")); ;

            var data = new ExportarData().ExportarMatrizRolesPorProducto(producto, dominioId, subDominioId, matricula, perfil, tribu, squad, codigoTribu, codigo);
            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/Exportar/ExportarProductoRolesDetallado")]
        [HttpGet]
        public IHttpActionResult GetExportarProductosRolesDetallado(string producto, int dominioId, int subDominioId, string tribu, string squad, string codigoTribu, string codigo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            string perfil = user.Perfil;

            if (producto == null)
            {
                producto = "";
            }
            string nomArchivo = string.Format("ReporteRolesProductoDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss")); ;

            var data = new ExportarData().ExportarMatrizRolesPorProductoDetallado(producto, dominioId, subDominioId, matricula, perfil, tribu, squad, codigoTribu, codigo);
            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/Funciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostFunciones(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Matricula = user.Matricula;
            objDTO.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFunciones(objDTO, out totalRows);

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/FuncionesAdmin")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostFuncionesAdmin(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesAdmin(objDTO, out totalRows);

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/FuncionesConsolidado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostFuncionesConsolidado(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesConsolidado(objDTO, out totalRows);

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/PersonasFunciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostPersonasFunciones(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetPersonasFunciones(objDTO, out totalRows);

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/Listado/DetalleBitacora")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleBitacora(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetBitacoraDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.DetalleBitacora = HttpUtility.HtmlEncode(x.DetalleBitacora); });

            var reader = new BootstrapTable<BitacoraDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ConfiguracionPortafolio/Listado/DetalleProductosRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleProductosRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProductoRolesDetalleDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.Rol = HttpUtility.HtmlEncode(x.Rol);
                x.Ambiente.ForEach(y =>
                {
                    y.GrupoRed = HttpUtility.HtmlEncode(y.GrupoRed);
                });
            });

            var reader = new BootstrapTable<RolesProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ConfiguracionPortafolio/Listado/DetalleFuncionesProductosRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleFuncionesProductosRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetDetalleFuncionesProductosRoles(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ConfiguracionPortafolio/Listado/DetalleSolicitudesFuncion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleSolicitudesFuncion(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudFuncionDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<SolicitudFuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("ConfiguracionPortafolio/Listado/DetalleProductosFunciones")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleProductosFunciones(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionProductoRolesDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<RolesProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ConfiguracionPortafolio/AgregarFuncionProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAgregarFuncionProducto(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.AgregarFuncionProducto(pag.Tribu, pag.Chapter, pag.Funcion, pag.RolProductoId, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/AgregarRolProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAgregarRolProducto(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.AgregarRolProducto(pag.ProductoId, pag.Rol, pag.GrupoRed, pag.Descripcion, pag.TipoCuenta, pag.Ambiente, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }
        [Route("ConfiguracionPortafolio/AgregarFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAgregarFuncion(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.AgregarFuncion(pag.Tribu, pag.RolProductoId, pag.Chapter, pag.FuncionMultiple, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }


        [Route("ConfiguracionPortafolio/EditarRolProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEditarRolProducto(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EditarRolProducto(pag.RolProductoId, pag.Rol, pag.GrupoRed, pag.Descripcion, pag.TipoCuenta, pag.Ambiente, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/ExportarFunciones")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion(
         string Producto
          , string sortName
          , string sortOrder)
        {
            string nomArchivo = "";



            var data = new ExportarData().ExportarFunciones(Producto, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("ConfiguracionPortafolio/ExportarFunciones2")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion2(
            string nombreTribu,
               string codigoTribu,
         string funcion,
          string tribu,
           string grupored,
            string rol,
             string producto,
           string chapter
          , string sortName
          , string sortOrder)
        {
            string nomArchivo = "";
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            string perfil = user.Perfil;

            if (funcion == null) { funcion = ""; }
            if (chapter == null) { chapter = ""; }
            if (tribu == null) { tribu = ""; }
            if (grupored == null) { grupored = ""; }
            if (rol == null) { rol = ""; }
            if (producto == null) { producto = ""; }

            var data = new ExportarData().ExportarFunciones2(nombreTribu, codigoTribu, chapter, tribu, grupored, rol, producto, matricula, perfil, funcion, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFunciones2Admin")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion2Admin(
         string funcion,
           string chapter,
            string tribu,
           string grupoRed,
            string producto,
           string rol
          , string sortName
          , string sortOrder)
        {
            string nomArchivo = "";

            //if (funcion == null) { funcion = ""; }
            //if (chapter == null) { chapter = ""; }

            var data = new ExportarData().ExportarFunciones2Admin(chapter, funcion, tribu, grupoRed, producto, rol, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
        [Route("ConfiguracionPortafolio/ExportarFunciones3")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion3(
    string funcion,
     string matricula,
      int dominioId,
      int subdominioId
     , string sortName
     , string sortOrder)
        {
            string nomArchivo = "";

            if (funcion == null) { funcion = ""; }
            //if (chapter == null) { chapter = ""; }

            var data = new ExportarData().ExportarFunciones3(funcion, matricula, dominioId, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFunciones3Admin")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion3Admin(string funcion, string tribu, string grupoRed, string producto, int rol, int dominioId, int subdominioId, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            if (funcion == null) { funcion = ""; }
            if (tribu == null) { tribu = ""; }
            if (tribu == "-1") { tribu = ""; }
            if (grupoRed == null) { grupoRed = ""; }
            if (producto == null) { producto = ""; }
            //if (chapter == null) { chapter = ""; }

            var data = new ExportarData().ExportarFunciones3Admin(funcion, tribu, grupoRed, producto, rol, dominioId, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFunciones4")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesConfiguracion4(
string funcion,
string chapter
, string sortName
, string sortOrder)
        {
            string nomArchivo = "";

            //if (funcion == null) { funcion = ""; }
            //if (chapter == null) { chapter = ""; }

            var data = new ExportarData().ExportarFunciones4(chapter, funcion, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRoles_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFuncionesPersonas")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesPersonasConfiguracion(
  string Producto,
  string matricula,
  int dominioId,
  int subdominioId
 , string sortName
 , string sortOrder)
        {
            string nomArchivo = "";

            if (Producto == null) { Producto = ""; }
            if (matricula == null) { matricula = ""; }

            var data = new ExportarData().ExportarFuncionesPersonas(Producto, matricula, dominioId, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRolesPersonas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFuncionesPersonas2")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesPersonasConfiguracion2(
               string codigoTribu,
          string funcion,
                  string tribu,
           string grupoRed,
            string producto,
           int rol,
           string chapter
         , string sortName
         , string sortOrder)
        {
            string nomArchivo = "";
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;
            string matricula = user.Matricula.Contains("E195_Administrador") ? "" : user.Matricula;

            if (funcion == null) { funcion = ""; }
            if (chapter == null) { chapter = ""; }
            if (matricula == null) { matricula = ""; }
            if (perfil == null) { perfil = ""; }

            if (tribu == null) { tribu = ""; }
            if (grupoRed == null) { grupoRed = ""; }
            if (producto == null) { producto = ""; }
            if (codigoTribu == null) { codigoTribu = ""; }

            var data = new ExportarData().ExportarFuncionesPersonas2(codigoTribu, funcion, tribu, grupoRed, producto, rol, matricula, perfil, chapter, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRolesPersonas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("ConfiguracionPortafolio/ExportarFuncionesPersonas2Admin")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesPersonasConfiguracion2Admin(
          string funcion,

           string tribu,
           string grupoRed,
            string producto,
           int rol,
             int dominioId,
              int subdominioId
         , string sortName
         , string sortOrder)
        {
            string nomArchivo = "";

            if (funcion == null) { funcion = ""; }

            if (tribu == null) { tribu = ""; }
            if (tribu == "-1") { tribu = ""; }
            if (grupoRed == null) { grupoRed = ""; }
            if (producto == null) { producto = ""; }

            var data = new ExportarData().ExportarFuncionesPersonas2Admin(funcion, tribu, grupoRed, producto, rol, dominioId, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRolesPersonas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFuncionesPersonas2AdminFP")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesPersonasConfiguracion2AdminFP(string funcion, string tribu, string grupoRed, string producto, int rol, int dominioId, int subdominioId, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            if (funcion == null) { funcion = ""; }

            if (tribu == null) { tribu = ""; }
            if (tribu == "-1") { tribu = ""; }
            if (grupoRed == null) { grupoRed = ""; }
            if (producto == null) { producto = ""; }

            var data = new ExportarData().ExportarFuncionesPersonas2AdminFP(funcion, tribu, grupoRed, producto, rol, dominioId, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("FuncionesPorRolesPersonas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/ExportarFuncionesPersonasMatrizRoles")]
        [HttpGet]
        public IHttpActionResult PostExportFuncionesPersonasConfiguracion3(string funcion, string chapter, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            if (funcion == null) { funcion = ""; }
            if (chapter == null) { chapter = ""; }

            var data = new ExportarData().ExportarFuncionesPersonasMatrizRoles(funcion, chapter, sortName, sortOrder);
            nomArchivo = string.Format("ConsolidadoMatrizRolesPersonas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ConfiguracionPortafolio/EditarFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEditarFuncion(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EditarFuncion(pag.Tribu, pag.FuncionProductoId, pag.Chapter, pag.Funcion, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/EditarFuncionProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEditarFuncionProductoo(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EditarFuncionProducto(pag.FuncionProductoId, pag.RolProductoId, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("GestionAplicacion/EditarItem")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEditarItem(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EditarItem(pag.ItemId, pag.NuevoValor, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/RolesProductosCombo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRolesProductosCombo(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Perfil = user.Perfil;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosCombo(pag.Perfil, pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/VulnerabilidadesCombo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetVulnerabilidadesCombo(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetVulnerabilidadesCombo(pag.Perfil, pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }


        [Route("ConfiguracionPortafolio/RolesProductosComboDominio/{DominioId:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRolesProductosComboDominio(int DominioId)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboDominio(DominioId);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/RolesProductosTribus")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRolesProductosComboTribus(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Perfil = user.Perfil;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboTribus(pag.TribuId, pag.Perfil, pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/RolesProductosTribusAgregar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRolesProductosComboTribusAgregar(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboTribusAgregar(pag.TribuId);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/RolesProductosChapterAgregar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRolesProductosComboChapterAgregar(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;
            //var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboChapterAgregar(pag.TribuId);
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboFuncionAgregar(pag.TribuId, pag.Chapter, pag.RolProductoId);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }


        [Route("ConfiguracionPortafolio/CaptaTribuMatricula")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTribuMatricula(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetTribuMatricula(pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/CaptaTribuNombreMatricula")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTribuNombreMatricula(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetTribuNombreMatricula(pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/FuncionesProductosCombo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetFuncionesProductosCombo(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.Perfil = pag.Perfil;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesProductosCombo(pag.Matricula, pag.Perfil, pag.CodigoTribu);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }


        [Route("ConfiguracionPortafolio/FuncionesProductosComboConsolidado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFuncionesProductosComboConsolidado()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesProductosComboConsolidado();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/FuncionesProductosComboTribu")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFuncionesProductosComboTribu()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesProductosComboTribu();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }


        [Route("ConfiguracionPortafolio/FuncionesProductosCombosRoles/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFuncionesProductosComboRoles(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionesProductosComboRoles(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }


        [Route("ConfiguracionPortafolio/EliminarRolProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEliminarRolProducto(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EliminarRolProducto(pag.RolProductoId, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/EliminarFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEliminarFuncion(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EliminarFuncion(pag.RolProductoId, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }


        [Route("ConfiguracionPortafolio/EliminarProductoFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEliminarProductoFuncion(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EliminarProductoFuncion(pag.FuncionProductoId, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ConfiguracionPortafolio/ObtenerRolProducto/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRolProductoDetail(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolProductoById(id);
            if (objRol == null)
                return NotFound();

            return Ok(objRol);
        }

        [Route("ConfiguracionPortafolio/ExisteRolFuncion/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRolFuncion(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolFuncion(id);
            //if (objRol == null)
            //    return NotFound();

            return Ok(objRol);
        }

        [Route("ConfiguracionPortafolio/ObtenerFuncion/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetFuncionDetail(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionById(id);
            if (objRol == null)
                return NotFound();

            return Ok(objRol);
        }
        [Route("ConfiguracionPortafolio/ObtenerFuncionProducto/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetFuncionProductoDetail(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.GetFuncionProductoById(id);
            if (objRol == null)
                return NotFound();

            return Ok(objRol);
        }

        [Route("ConfiguracionPortafolio/ObtenerSolicitudAsignacion/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSolicitudAsignacionDetail(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudMDRById(id);
            if (objRol == null)
                return NotFound();

            return Ok(objRol);
        }

        [Route("ConfiguracionPortafolio/Bitacora/Exportar")]
        [HttpGet]
        public IHttpActionResult PostBitacoraExportar(string codigoApt)
        {
            string filename = "";

            var data = new ExportarData().ExportarBitacora(codigoApt, "FechaCreacion", "desc");
            filename = string.Format("Bitacora_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ConfiguracionPortafolio/CambiarFlagMostrarCampo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarFlagMostrarCampo(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;
            string NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarFlagMostrarCampo(id, !estadoActual, Usuario, NombreUsuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ActualizarParametro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetActualizarParametro(string valor)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var parametro = new ParametroDTO()
            {
                Codigo = Utilitarios.ESTADOS_APLICACION,
                Valor = valor,
                UsuarioModificacion = Usuario
            };
            ServiceManager<ParametroDAO>.Provider.ActualizarParametroApp(parametro);
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        /*rolesgestion*/
        [Route("RolGestion/ListadoRolGestion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListActivosRolGestion(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetRolesGestion(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<RolesGestionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("RolGestion/CambiarEstadoRolGestion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoRolGestion(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<ActivosDAO>.Provider.GetRolesGestionById(Id);
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoRolesGestion(Id, !entidad.IsActive, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }


        [Route("RolGestion/ListarCombosRoles")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage getListarCombosRoleS()
        {
            HttpResponseMessage response = null;
            var listRoles = Utilitarios.EnumToList<ERoles>();

            var dataRpta = new
            {
                roles = listRoles.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("RolGestion/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRoles(RolesGestionDTO rgDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            rgDTO.UsuarioCreacion = user.Matricula;
            rgDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<ActivosDAO>.Provider.AddOrEditRolesGestion(rgDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }

        [Route("RolGestion/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRolesGestionById(int id)
        {
            var objPar = ServiceManager<ActivosDAO>.Provider.GetRolesGestionById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }

        #endregion

        [Route("ConfiguracionPortafolio/ListarClasificacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarClasificacion(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetClasificacion(pag, out totalRows);

            registros.ForEach(x => { x.Nombre = HttpUtility.HtmlEncode(x.Nombre); });

            var reader = new BootstrapTable<ClasificacionTecnicaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetClasificacionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetClasificacionById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetClasificacionById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditClasificacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditClasificacion(ClasificacionTecnicaDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditClasificacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoClasificacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CambiarEstadoClasificacion(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoClasificacion(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ListarSubclasificacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarSubclasificacion(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ActivosDAO>.Provider.GetSubclasificacion(pag, out totalRows);

            registros.ForEach(x => { x.Nombre = HttpUtility.HtmlEncode(x.Nombre); });

            var reader = new BootstrapTable<SubClasificacionTecnicaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ConfiguracionPortafolio/GetSubclasificacionById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubclasificacionById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.GetSubclasificacionById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/AddOrEditSubclasificacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditSubclasificacion(SubClasificacionTecnicaDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioCreacion = user.Matricula;
            objDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var entidadId = ServiceManager<ActivosDAO>.Provider.AddOrEditSubclasificacion(objDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ConfiguracionPortafolio/CambiarEstadoSubclasificacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CambiarEstadoSubclasificacion(int id, bool estadoActual)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ActivosDAO>.Provider.CambiarEstadoSubclasificacion(id, !estadoActual, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ConfiguracionPortafolio/ExportarGerencia")]
        [HttpGet]
        public IHttpActionResult PostExportGDAU()
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarGDAU();
            nomArchivo = string.Format("ListaGerencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionAplicacion/GetColumnasPublicacionAplicacionToJSAppsDesestimadas")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnasPublicacionAplicacionToJSAppsDesestimadas(string tablaProcedencia)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<AplicacionDAO>.Provider.GetColumnasPublicacionAplicacionToJSAppsDesestimadas(tablaProcedencia);

            entidad.ForEach(x => { x.title = HttpUtility.HtmlEncode(x.title); });

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }
        [Route("GestionAplicacion/ListarAplicacionesDesestimadas")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListAplicacionDesestimadas(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetAplicacionesDesestimadas(pag, out totalRows);
            if (registros != null)
            {

                foreach (DataRow row in registros.Rows)
                {
                    row["applicationName"] = HttpUtility.HtmlEncode(row["applicationName"]);
                    row["description"] = HttpUtility.HtmlEncode(row["description"]);
                    row["BIANdomain"] = HttpUtility.HtmlEncode(row["BIANdomain"]);
                    row["BIANarea"] = HttpUtility.HtmlEncode(row["BIANarea"]);
                    row["webDomain"] = HttpUtility.HtmlEncode(row["webDomain"]);
                    row["assetType"] = HttpUtility.HtmlEncode(row["assetType"]);
                }

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GestionAplicacion/ListarFormatosRegistro")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListFormatosRegistro(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetFormatosRegistro(pag, out totalRows);
            if (registros != null)
            {

                foreach (DataRow row in registros.Rows)
                {
                    row["applicationName"] = HttpUtility.HtmlEncode(row["applicationName"]);
                    row["description"] = HttpUtility.HtmlEncode(row["description"]);
                    row["BIANdomain"] = HttpUtility.HtmlEncode(row["BIANdomain"]);
                    row["BIANarea"] = HttpUtility.HtmlEncode(row["BIANarea"]);
                    row["webDomain"] = HttpUtility.HtmlEncode(row["webDomain"]);
                    row["assetType"] = HttpUtility.HtmlEncode(row["assetType"]);
                }

                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ExportarResponsablePort")]
        [HttpGet]
        public IHttpActionResult GetExportarResponsablesPort()
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarResponsablePortafolio();
            nomArchivo = string.Format("ListaResponsablesPortafolio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        #region Enviar Solicitud

        [Route("ConfiguracionPortafolio/EnviarSolicitud")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEnviarSolicitud(PaginacionSolicitudRol pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.nombre = user.Nombres;
            pag.Matricula = user.Matricula;
            pag.MailEspecialista = user.CorreoElectronico;

            HttpResponseMessage response = null;
            string[] strSeguridad = pag.IdSeguridad.Split('-');
            pag.IdSeguridad = strSeguridad[0];
            pag.MailSeguridad = strSeguridad[1];
            var dataResult = ServiceManager<SolicitudAplicacionDAO>.Provider.EnviarSolicitud(pag);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);
            return response;
        }

        [Route("ConfiguracionPortafolio/SeguridadCombo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSeguridadCombo()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.DevolverRolesSeguridad();
            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/VerificarFuncionesRol/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetFuncionesRolPorProducto(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.ExisteFuncionesRolPorProducto(id);
            return Ok(objRol);
        }

        [Route("ConfiguracionPortafolio/VerificarSolicitudProducto")]
        [HttpPost]//[HttpGet]
        [Authorize]
        public HttpResponseMessage GetVerificarSolicitudProducto(PaginacionSolicitudRol pag)
        {
            HttpResponseMessage response = null;
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.VerificarSolicitudProducto(pag.IdProducto);
            response = Request.CreateResponse(HttpStatusCode.OK, objRol);
            return response;
        }

        [Route("ConfiguracionPortafolio/VerificarTipoMensaje")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTipoMensaje(PaginacionSolicitudRol mensaje)
        {
            HttpResponseMessage response = null;
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.ValidarMensajeSolicitudes(mensaje.IdProducto, mensaje.IdSolicitud);
            response = Request.CreateResponse(HttpStatusCode.OK, objRol);
            return response;
        }

        //SolicitudPorProducto
        [Route("ConfiguracionPortafolio/SolicitudPorProducto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetSolicitudPorProducto(PaginacionSolicitudRol pag)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudPorProducto(pag);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ConfiguracionPortafolio/Producto/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(ProductoDTO))]
        [Authorize]
        public IHttpActionResult GetProductoById(int id)
        {
            var objProd = ServiceManager<ProductoDAO>.Provider.GetProductoById(id);
            if (objProd == null)
                return NotFound();

            //if (objProd.AplicacionId.HasValue)
            //    objProd.Aplicacion = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(objProd.AplicacionId.Value);

            return Ok(objProd);
        }

        [Route("ConfiguracionPortafolio/ProductoObservado/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(SolicitudRolDTO))]
        [Authorize]
        public IHttpActionResult GetProductoObservadoById(int id)
        {
            var objProd = ServiceManager<SolicitudRolDAO>.Provider.GetProductoObservadoById(id);
            if (objProd == null)
                return NotFound();

            return Ok(objProd);
        }

        [Route("ConfiguracionPortafolio/VerificarFuncionesActivasPorRol/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetVerificarFuncionesActivas(int id)
        {
            var objRol = ServiceManager<SolicitudAplicacionDAO>.Provider.VerificarFuncionesActivarPorRol(id);
            return Ok(objRol);
        }

        #endregion

        [Route("GestionAplicacion/GetColumaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<ActivosDAO>.Provider.GetCamposByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("ConfiguracionPortafolio/ComboTipoCuentaAmbiente")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetComboTipoCuentaAmbiente()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetComboTipoCuentaAmbiente();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("ListAplicaciones_ByPerfil")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ListAplicaciones_ByPerfil(string aplicacion)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfil = user.PerfilId;
            string usuario = user.Matricula;

            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.ListAplicaciones_ByPerfil(perfil, aplicacion, usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("ObtenerComponentesApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetComponentesApp(Paginacion pag)
        {
            //var dataRpta = "";
            //dataRpta = ServiceManager<DependenciasDAO>.Provider.ActualizarEstadoServidores(pag);
            HttpResponseMessage response = null;
            var lstReglasPorApp = ServiceManager<DependenciasDAO>.Provider.GetReglasPorApp(pag).FindAll(f => f.Relacionado == true);
            if (lstReglasPorApp != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, lstReglasPorApp);
            }
            return response;
        }
        [Route("ActualizarComponentesApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarComponentesApp(ComponenteConectaCon request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            bool estado = ServiceManager<AplicacionDAO>.Provider.AddOrEditComponentesApp(request);
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        // POST: api/Aplicacion/Listado_Infra
        [Route("Listado_Infra")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage DiagramaInfra_PostListAplicacion(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;
            pag.PerfilId = user.PerfilId;
            pag.Matricula = user.Matricula;
            HttpResponseMessage response = null;

            var registros = ServiceManager<AplicacionDAO>.Provider.DiagramaInfra_GetAplicacionConfiguracion(pag, out int totalRows, out string arrCodigoAPTs);
            if (registros != null)
            {
                registros.ForEach(x => { x.Agrupacion = HttpUtility.HtmlEncode(x.Agrupacion); });
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros,
                    ArrCodigoAPT = arrCodigoAPTs
                };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;

        }
        [Route("GetComponenteByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetComponenteByFiltro(string filtro, string codigoApt)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasDAO>.Provider.GetComponenteByFiltro_RelacionamientoComponentes(filtro, codigoApt);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }
    }
}
