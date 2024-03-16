using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using BCP.PAPP.Common.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static log4net.Appender.RollingFileAppender;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Relacion")]
    public class RelacionController : BaseController
    {
        [Route("ListarDetalle/{Id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListarDetalle(int Id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<RelacionDAO>.Provider.GetRelacionDetalle(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("VerificarEliminada")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage VerificarEliminada(RelacionDTO obj)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<RelacionDAO>.Provider.VerificarEliminada(obj.CodigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRelacionById(int id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<RelacionDAO>.Provider.GetRelacionById(id);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("CambiarEstado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambioEstado(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            RelacionDTO objRegistro = new RelacionDTO();
            objRegistro.Id = Convert.ToInt32(request.Id);
            objRegistro.UsuarioModificacion = request.Usuario;
            objRegistro.UsuarioCreacion = request.Usuario;
            objRegistro.EstadoId = request.EstadoId;
            objRegistro.Observacion = request.Observacion;
            objRegistro.EquipoId = request.ObjetoId;
            objRegistro.FlagRemoveEquipo = request.Flag;

            var retorno = ServiceManager<RelacionDAO>.Provider.CambiarEstado(objRegistro, request.FechaFiltro, request.FlagAdmin);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("EliminarRelacionDetalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEliminarRelacionDetalle(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            RelacionDTO objRegistro = new RelacionDTO();
            objRegistro.Id = Convert.ToInt32(request.Id);
            objRegistro.UsuarioModificacion = request.Usuario;

            var retorno = ServiceManager<RelacionDAO>.Provider.EliminarRelacionDetalle(objRegistro);
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FUNCION_RELACIONES");
            var dataParametro = parametro != null ? parametro.Valor : "File server;Base de datos";

            var listFuncion = dataParametro.Split(';');
            var listTipoEquipo = Utilitarios.EnumToList<ETipoRelacion>();
            var listTipoEquipo_2 = Utilitarios.EnumToList<ETipoEquipo>();
            var listTipoAmbiente = ServiceManager<AmbienteDAO>.Provider.GetAmbienteByFiltro(null); //Utilitarios.EnumToList<ETipoAmbiente>();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubdominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listRelevancia = ServiceManager<RelevanciaDAO>.Provider.GetAllRelevancia();
            var listEstado = Utilitarios.EnumToList<EEstadoRelacion>();
            var lMotivoEliminacion = Utilitarios.EnumToList<EMotivoEliminacionRelacion>();
            var lMotivoEliminacionAplicaciones = Utilitarios.EnumToList<EMotivoEliminacionRelacionAplicaciones>();
            var parametroCuarentena = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("RELACION_PERIODO_CUARENTENA");

            var listRelacionAplicacion = Utilitarios.EnumToList<ERelacionAplicacion>();

            PaginacionTiposRelacion pag = new PaginacionTiposRelacion();
            pag.Descripcion = "";
            pag.pageSize = 100;
            pag.pageNumber = 1;
            var listTiposRelacionApps = ServiceManager<DependenciasAppsDAO>.Provider.ListaTiposRelacion(pag, out int totalRows);

            var dataRpta = new
            {
                TipoEquipo = listTipoEquipo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoEquipo_2 = listTipoEquipo_2.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoAmbiente = listTipoAmbiente,
                Dominio = listDominio,
                Subdominio = listSubdominio,
                Relevancia = listRelevancia.Select(x => new { Id = x.Id, Descripcion = x.Nombre }).ToList(),
                Estado = listEstado.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                MotivoEliminacion = lMotivoEliminacion.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Funcion = listFuncion,
                ParametroPeriodoCuarentena = parametroCuarentena.Valor
                ,
                TiposRelacionApps = listTiposRelacionApps.Where(x => x.ParaAplicaciones == true && x.Activo == true).ToList().Select(s => new { Id = s.TipoRelacionId, Descripcion = s.Descripcion })
                ,
                TiposRelacionComponentes = listTiposRelacionApps.Where(x => x.Activo == true).ToList().Select(s => new { Id = s.TipoRelacionId, Descripcion = s.Descripcion })
                ,
                MotivoEliminacionApps = lMotivoEliminacionAplicaciones.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                RelacionAplicacion = listRelacionAplicacion.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListarDataAutocomplete")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListDataAutocomplete()
        {
            HttpResponseMessage response = null;
            var listTecnologia = ServiceManager<TecnologiaDAO>.Provider.GetTec();
            //var listAplicacion = ServiceManager<RelacionDAO>.Provider.GetAplicacion();
            var listEquipo = ServiceManager<RelacionDAO>.Provider.GetEquipo();
            var dataRpta = new
            {
                Tecnologia = listTecnologia.Select(x => new { Id = x.Id, Descripcion = x.Nombre, value = x.Nombre }).ToList(),
                //Aplicacion = listAplicacion,
                Equipo = listEquipo
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListarEquipoTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListEquipoTecnologia()
        {
            HttpResponseMessage response = null;
            var listado = ServiceManager<RelacionDAO>.Provider.GetEquipo_Tecnologia();
            response = Request.CreateResponse(HttpStatusCode.OK, listado);
            return response;
        }

        [Route("ListarEquipoTecnologiaByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListEquipoTecnologiaByEquipoId(int Id, string subdominioIds = null)
        {
            HttpResponseMessage response = null;
            var listado = ServiceManager<RelacionDAO>.Provider.GetEquipoTecnologiaByEqId(Id, subdominioIds);
            response = Request.CreateResponse(HttpStatusCode.OK, listado);
            return response;
        }

        [Route("ListarEquipoInstanciasBDByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListEquipoInstanciasBDByEquipoId(int Id)
        {
            HttpResponseMessage response = null;
            var listado = ServiceManager<RelacionDAO>.Provider.GetInstanciasBD_Relacion(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, listado);
            return response;
        }

        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListRelacion(PaginacionRelacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            if (pag.PerfilId == (int)EPerfilBCP.Seguridad || pag.PerfilId == (int)EPerfilBCP.ETICMDB) pag.PerfilId = (int)EPerfilBCP.Administrador;

            var totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetRelacionSP(pag.CodigoAPT
                , pag.Componente
                , pag.pageNumber
                , pag.pageSize
                , pag.sortName
                , pag.sortOrder
                , pag.username
                , pag.TipoRelacionId
                , pag.EstadoId
                , pag.PerfilId
                , out totalRows);

            if (registros != null)
            {
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Bandeja/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListRelacionSP(PaginacionRelacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

            if (pag.PerfilId == (int)EPerfilBCP.Seguridad) pag.PerfilId = (int)EPerfilBCP.Administrador;

            var totalRows = 0;

            if (paramCD == pag.TipoEquipoId)
            {
                var registros = ServiceManager<RelacionDAO>.Provider.GetRelacionSP_3(pag, out totalRows);

                registros.ForEach(x =>
                {
                    x.Componente = HttpUtility.HtmlEncode(x.Componente);
                    x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                    x.SubDominioStr = HttpUtility.HtmlEncode(x.SubDominioStr);
                    x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
                    x.AplicacionStr = HttpUtility.HtmlEncode(x.AplicacionStr);
                    x.AmbienteStr = HttpUtility.HtmlEncode(x.AmbienteStr);
                });

                if (registros != null)
                {
                    var dataRpta = new
                    {
                        Total = totalRows,
                        Rows = registros
                    };

                    response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
                }
            }
            else
            {
                // Busca relaciones de app-app
                if (pag.RelacionApp == 1)
                {
                    var registros = ServiceManager<RelacionDAO>.Provider.GetList_DependenciaAplicacion(pag, out totalRows);

                    if (registros == null)
                        return Request.CreateResponse(HttpStatusCode.NotFound);

                    registros.ForEach(x =>
                    {
                        x.Componente = HttpUtility.HtmlEncode(x.Componente);
                        x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                        x.SubDominioStr = HttpUtility.HtmlEncode(x.SubDominioStr);
                        x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
                        x.AplicacionStr = HttpUtility.HtmlEncode(x.AplicacionStr);
                        x.AmbienteStr = HttpUtility.HtmlEncode(x.AmbienteStr);
                    });

                    var reader = new BootstrapTable<RelacionDTO>()
                    {
                        Total = totalRows,
                        Rows = registros
                    };

                    response = Request.CreateResponse(HttpStatusCode.OK, reader);
                }
                else
                {
                    var registros = ServiceManager<RelacionDAO>.Provider.GetRelacionSP_2(pag, out totalRows);

                    if (registros != null)
                    {
                        registros.ForEach(x =>
                        {
                            x.Componente = HttpUtility.HtmlEncode(x.Componente);
                            x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                            x.SubDominioStr = HttpUtility.HtmlEncode(x.SubDominioStr);
                            x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
                            x.AplicacionStr = HttpUtility.HtmlEncode(x.AplicacionStr);
                            x.AmbienteStr = HttpUtility.HtmlEncode(x.AmbienteStr);
                        });
                        var dataRpta = new
                        {
                            Total = totalRows,
                            Rows = registros
                        };

                        response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
                    }
                }

            }


            return response;
        }

        [Route("GetCertificadoDigitalID")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCertificadoDigitalID()
        {
            HttpResponseMessage response = null;
            var paramCD = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor;
            var dataRpta = new ParametroCertificadoDigital();
            if (paramCD != null)
            {
                dataRpta.parametro = Convert.ToInt32(paramCD);
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GetApisID")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApisID()
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.GetTipoEquipoIdByNombre("Api");
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("Actualizacion/ListarByFiltros")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarByFiltros(PaginacionRelacionFilter pag)
        {
            HttpResponseMessage response = null;

            var totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetAplicacionByFilter(pag, out totalRows);

            if (registros != null)
            {
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Listado/AplicacionServidor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListAplicacionServidor(PaginacionAplicacionServidor pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            var totalRows = 0;
            if (pag.PerfilId == (int)EPerfilBCP.Administrador
                || pag.PerfilId == (int)EPerfilBCP.Coordinador
                || pag.PerfilId == (int)EPerfilBCP.ETICMDB
                || pag.PerfilId == (int)EPerfilBCP.GestorTecnologia
                || pag.PerfilId == (int)EPerfilBCP.Seguridad
                || pag.PerfilId == (int)EPerfilBCP.Auditoria
                || pag.PerfilId == (int)EPerfilBCP.Operador
                || pag.PerfilId == (int)EPerfilBCP.ArquitectoTecnologia
                || pag.PerfilId == (int)EPerfilBCP.GestorCVT_CatalogoTecnologias)
            {

                var registros = ServiceManager<RelacionDAO>.Provider.GetVistaRelacion(pag.Aplicacion
                    , pag.Equipo
                    , pag.AmbienteIds
                    , pag.Jefe
                    , pag.GestionadoPor
                    , pag.PCIS
                    , pag.RelacionAplicacion
                    , pag.EstadoId
                    , pag.TipoRelacion
                    , pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

                if (registros != null)
                {

                    registros.ForEach(x =>
                    {
                        x.Equipo = HttpUtility.HtmlEncode(x.Equipo);
                        x.TipoActivoTI = HttpUtility.HtmlEncode(x.TipoActivoTI);
                        x.DetalleAmbiente = HttpUtility.HtmlEncode(x.DetalleAmbiente);
                        x.Aplicacion = HttpUtility.HtmlEncode(x.Aplicacion);
                        x.ListaPCI = HttpUtility.HtmlEncode(x.ListaPCI);
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
            else
            {

                List<int> AmbienteIds = string.IsNullOrEmpty(pag.AmbienteIds) ? new List<int>() : pag.AmbienteIds.Split('|').Select(Int32.Parse).ToList();
                List<int> EstadoIds = string.IsNullOrEmpty(pag.EstadoId) ? new List<int>() : pag.EstadoId.Split('|').Select(Int32.Parse).ToList();

                var registros = ServiceManager<RelacionDAO>.Provider.GetVistaRelacionConsultor(pag.Aplicacion
                    , pag.Equipo
                    , AmbienteIds
                    , pag.Jefe
                    , pag.GestionadoPor
                    , pag.RelacionAplicacion
                    , EstadoIds
                    , pag.TipoRelacion
                    , pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.Matricula, out totalRows);

                if (registros != null)
                {
                    registros.ForEach(x =>
                    {
                        x.Equipo = HttpUtility.HtmlEncode(x.Equipo);
                        x.TipoActivoTI = HttpUtility.HtmlEncode(x.TipoActivoTI);
                        x.DetalleAmbiente = HttpUtility.HtmlEncode(x.DetalleAmbiente);
                        x.Aplicacion = HttpUtility.HtmlEncode(x.Aplicacion);
                        x.ListaPCI = HttpUtility.HtmlEncode(x.ListaPCI);
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
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRelacion(RelacionDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;
            if (request.ListRelacionDetalle != null && request.ListRelacionDetalle.Count > 0)
            {
                foreach (var item in request.ListRelacionDetalle)
                {
                    item.UsuarioCreacion = user.Matricula;
                    item.UsuarioModificacion = user.Matricula;
                }
            }

            HttpResponseMessage response = null;
            long IdRelacion = ServiceManager<RelacionDAO>.Provider.AddOrEditRelacion(request);
            bool estado = IdRelacion > 0;
            if (estado)
                response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("Actualizacion/Registrar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizar(AplicacionTecnologiaDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var state = ServiceManager<RelacionDAO>.Provider.MassiveUpdateAplicacion(request);
            response = Request.CreateResponse(HttpStatusCode.OK, state);

            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportar(string codigoAPT,
            string componente,
            string nombre,
            string sortName,
            string sortOrder,
            int? tipoRelacionId,
            int? estadoId)
        {
            string nomArchivo = "";
            codigoAPT = codigoAPT == null ? "" : codigoAPT;
            componente = componente == null ? "" : componente;

            //var data = new ExportarData().ExportarReporteRelaciones(codigoAPT, componente, tipoRelacionId.Value, estadoId.Value, perfil, username);
            var data = new byte[0];
            nomArchivo = string.Format("ReporteRelacionDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Bandeja/Exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportarSP(string codigoAPT,
            string componente,
            string tecnologia,
            string sortName,
            string sortOrder,
            int? tipoRelacionId,
            string estadoId,
            string subdominioIds,
            string ambienteId,
            DateTime fechaConsulta,
            int TipoEquipoId)
        {
            var user = TokenGenerator.GetCurrentUser();
            string username = user.Matricula;
            int perfil = user.PerfilId;

            codigoAPT = string.IsNullOrEmpty(codigoAPT) ? string.Empty : codigoAPT;
            componente = string.IsNullOrEmpty(componente) ? string.Empty : componente;
            tecnologia = string.IsNullOrEmpty(tecnologia) ? string.Empty : tecnologia;
            subdominioIds = string.IsNullOrEmpty(subdominioIds) ? string.Empty : subdominioIds;
            tipoRelacionId = tipoRelacionId ?? -1;
            //estadoId = estadoId ?? -2;
            //ambienteId = ambienteId ?? -1
            estadoId = string.IsNullOrEmpty(estadoId) ? string.Empty : estadoId;
            ambienteId = string.IsNullOrEmpty(ambienteId) ? string.Empty : ambienteId;

            var data = new ExportarData().ExportarReporteRelaciones(codigoAPT,
            componente,
            tipoRelacionId.Value,
            estadoId,
            perfil,
            username,
            tecnologia,
            subdominioIds,
            ambienteId,
            fechaConsulta, TipoEquipoId);

            var filename = string.Format("ReporteRelacionDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("GetEquipoFiltros")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, 0);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoFiltros(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltroSinServicioNube(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetEquipoServicioNubeByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoServicioNubeByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_TIPO_EQUIPO_SERVICIONUBE);
            var id_ServicioNube = parametro != null ? int.Parse(parametro.Valor) : 0;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, id_ServicioNube);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ExisteEquipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteEquipo(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<EquipoDAO>.Provider.ExisteEquipoById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteRelacionTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteRelacionTecnologia(int id, string codigoAPT, int tecnologiaId, int? equipoId)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<RelacionDAO>.Provider.ExisteRelacionTipoTecnologia(id, codigoAPT, tecnologiaId, equipoId);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteRelacionEquipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteRelacionEquipo(int id, string codigoAPT, int ambienteId, int equipoId)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<RelacionDAO>.Provider.ExisteRelacionTipoEquipo(id, codigoAPT, ambienteId, equipoId);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteRelacionAplicacionEquipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteRelacionAplicacionEquipo(string codigoAPT, int equipoId)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<RelacionDAO>.Provider.ExisteRelacionAplicacionEquipo(codigoAPT, equipoId);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteTecnologiaActivaById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteTecnologiaActivaById(int Id, int TipoRelacionId)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<RelacionDAO>.Provider.ExisteTecnologiaEquipoActivaById(Id, TipoRelacionId);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GetRelacionesByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionRelacionadaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetAplicacionRelacionadaByEquipoId(equipoId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GetCertificadoDigitalByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCertificadoDigitalByEquipoId(int equipoId, int pageNumber, int pageSize)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetCertificadoDigitalByEquipoId(equipoId, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ListarEquiposByAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquiporByAplicacion(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<RelacionDAO>.Provider.GetEquipoByAplicacion(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        #region APLICACIONES_VINCULADAS

        [Route("RegistrarAplicacionVinculada")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRegistrarAplicacionVinculada(AplicacionVinculadaDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<RelacionDAO>.Provider.RegistrarAplicacionVinculada(request);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ListarAplicacionesVinculadas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListAplicacionesVinculadas(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetAplicacionVinculada(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<AplicacionVinculadaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAplicacionesVinculadas")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostExportAplicacionesVinculadas(string nombre, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarAplicacionesVinculadas(nombre, sortName, sortOrder);
            nomArchivo = string.Format("ListaAplicacionRelacionada_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        #endregion


        [Route("ExportarVistaPorRelaciones")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportarVistaPorRelaciones(string Aplicacion, string Equipo, string AmbienteId, string Jefe, string GestionadoPor, string sortName, string sortOrder, string pci, string RelacionAplicacion, string estadoId, string TipoRelacion)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfilId = user.PerfilId;
            string matricula = user.Matricula;
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            //if (string.IsNullOrEmpty(username)) username = null;

            //List<int> AmbienteIds = AmbienteId == null ? new List<int>() : AmbienteId.Split('|').Select(Int32.Parse).ToList();
            //List<string> pci2 = pci == null ? new List<string>() : pci.Split('|').Select(Convert.ToString).ToList();
            //List<int> EstadoIds = estadoId == null ? new List<int>() : estadoId.Split('|').Select(Int32.Parse).ToList(); 
            //var data = new ExportarData().ExportarVistaPorRelaciones(Aplicacion, Equipo, AmbienteIds, Jefe, GestionadoPor, sortName, sortOrder, perfilId, matricula, pci2, RelacionAplicacion, EstadoIds);
            var data = new ExportarData().ExportarVistaPorRelaciones(Aplicacion, Equipo, AmbienteId, Jefe, GestionadoPor, sortName, sortOrder, perfilId, matricula, pci, RelacionAplicacion, estadoId, TipoRelacion);

            nomArchivo = string.Format("ListadoVistaPorRelacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("RelacionXTecnologiaId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRelacionXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetRelacionXTecnologiaId(tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<RelacionDTO>() { Total = totalRows, Rows = registros };
            return Ok(reader);
        }

        [Route("GetComentarioByRelacionId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetComentarioByRelacionId(int Id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.GetComentarioByRelacionId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ValidarRelacionEquipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFlagRelacionEquipo(int Id, DateTime Fecha)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.ValidarRelacionEquipo(Id, Fecha);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("infoapli/Listado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostInfoAplicacionListado(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber, int pageSize)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfil = user.PerfilId;
            string username = user.Matricula;

            if (perfil == (int)EPerfilBCP.Administrador)
            {
                username = null;
            }

            var data = ServiceManager<InfoAplicacionDAO>.Provider.PostInfoApliListado(username, soportado, codigoAPT, dominio, subdominio, comboJE, comboLU, pageNumber, pageSize, out int totalRows);

            var reader = new BootstrapTable<InfoAplicacionDTO>()
            {
                Total = totalRows,
                Rows = data
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, reader);

            return response;
        }

        [Route("infoapli/ComboSoportado")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetComboSoportado()
        {
            var combo = new InfoAplicacion();
            var datosTipo = ServiceManager<InfoAplicacionDAO>.Provider.GetInfoApliComboSoportado();
            combo.soportado = datosTipo;
            return Ok(combo);
        }

        [Route("infoapli/ComboJefe")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetComboJefe()
        {
            var combo = new InfoAplicacion();
            var datosTipo = ServiceManager<InfoAplicacionDAO>.Provider.GetInfoApliComboJefe();
            combo.soportado = datosTipo;
            return Ok(combo);
        }

        [Route("infoapli/ComboLider")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetComboLider()
        {
            var combo = new InfoAplicacion();
            var datosTipo = ServiceManager<InfoAplicacionDAO>.Provider.GetInfoApliComboLider();
            combo.soportado = datosTipo;
            return Ok(combo);
        }

        [Route("infoapli/Reporte/ExportarAplicacionValidacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAplicacionValidaExportar(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfil = user.PerfilId;
            string username = user.Matricula;

            if (perfil == (int)EPerfilBCP.Administrador)
            {
                username = null;
            }

            string nomArchivo = "";

            var data = new ExportarData().ExportarAplicacionesValidadas(username, soportado, codigoAPT, dominio, subdominio, comboJE, comboLU);
            nomArchivo = "ReporteConsultaAplicacionValidacion";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("infoapli/Historico/Listado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostInfoAplicacionHistoricoListado(string codigoAPT, int pageNumber, int pageSize)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfil = user.PerfilId;
            string username = user.Matricula;

            if (perfil == (int)EPerfilBCP.Administrador)
            {
                username = null;
            }

            var data = ServiceManager<InfoAplicacionDAO>.Provider.GetHistoricoAplicacion(username, codigoAPT, pageNumber, pageSize, out int totalRows);

            var reader = new BootstrapTable<InfoAplicacionDTO>()
            {
                Total = totalRows,
                Rows = data
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, reader);

            return response;
        }

        [Route("infoapli/Detalle/Listado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostInfoAplicacionDetalleListado(string codigoAPT)
        {
            var datosResultados = new List<DetalleTotalNiveles>();

            var data = ServiceManager<InfoAplicacionDAO>.Provider.GetDetalleTecnicoNivel0(codigoAPT);

            var subdominios = (from d in data
                               select new
                               {
                                   d.idSubdominio,
                                   d.subdominio
                               }).Distinct().ToList();

            foreach (var item in subdominios)
            {
                var obj = new DetalleTotalNiveles();
                obj.idSubdominio = item.idSubdominio;
                obj.subdominio = HttpUtility.HtmlEncode(item.subdominio);
                obj.detalle = (from d in data
                               where d.idSubdominio == item.idSubdominio
                               select new DetalleInforAplicacionNivel1
                               {
                                   tipoComponente = d.tipoComponente,
                                   nombreComponente = d.nombreComponente,
                                   ambiente = d.ambiente,
                                   tecnologia = d.tecnologia
                               }).ToList();
                obj.cantidad = obj.detalle.Count();
                datosResultados.Add(obj);
            }

            var reader = new BootstrapTable<DetalleTotalNiveles>()
            {
                Total = datosResultados.Count(),
                Rows = datosResultados
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, reader);

            return response;
        }

        [Route("infoapli/Detalle/DetalleApp")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostInfoAplicacionDetalleDetalleApp(string username, string codigoAPT, int subdominio, int pageNumber, int pageSize)
        {
            var data = ServiceManager<InfoAplicacionDAO>.Provider.GetDetalleTecnicoNivel1(username, codigoAPT, subdominio, pageNumber, pageSize, out int totalRows);

            var reader = new BootstrapTable<DetalleInforAplicacionNivel1>()
            {
                Total = totalRows,
                Rows = data
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, reader);

            return response;
        }

        [Route("infoapli/Detalle/ValidarApp")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostInfoAplicacionDetalleValidarApp(string codigoAPT)
        {
            var user = TokenGenerator.GetCurrentUser();
            string username = user.Matricula;

            var data = ServiceManager<InfoAplicacionDAO>.Provider.GetValidarInfoApp(username, codigoAPT);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, data);

            return response;
        }

        [Route("consultapli/Listado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetConsultaListado(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber, int pageSize)
        {
            var data = ServiceManager<InfoAplicacionDAO>.Provider.GetConsultaValidadaHistorico(soportado, codigoAPT, dominio, subdominio, comboJE, comboLU, pageNumber, pageSize, out int totalRows);

            var reader = new BootstrapTable<InfoAplicacionDTO>()
            {
                Total = totalRows,
                Rows = data
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, reader);

            return response;
        }

        [Route("consultapli/Reporte/ExportarAplicacionValidacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetConsultaAppValidaExportar(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU)
        {
            string nomArchivo = string.Format("ReporteConsultaAplicacionValidacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss")); ;

            var data = new ExportarData().GetConsultaAppValidaExportar(soportado, codigoAPT, dominio, subdominio, comboJE, comboLU);

            return Ok(new { excel = data, name = nomArchivo });
        }

        // --> Relaciones entre Apps
        [Route("Get_DependenciaAplicacion/{Id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Get_DependenciaAplicacion(int Id)
        {
            var registros = ServiceManager<RelacionDAO>.Provider.Get_DependenciaAplicacion(Id);

            if (registros == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, registros);

            return response;
        }

        [Route("ExisteRelacionApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ExisteRelacionApp(RelacionDTO objRegistro)
        {
            HttpResponseMessage response = null;
            var existe = ServiceManager<RelacionDAO>.Provider.ExisteRelacionApp(objRegistro);

            response = Request.CreateResponse(HttpStatusCode.OK, existe);

            return response;
        }

        [Route("RegistrarRelacionApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage RegistrarRelacionApp(RelacionDTO objRegistro)
        {
            var user = TokenGenerator.GetCurrentUser();
            objRegistro.UsuarioCreacion = user.Matricula;

            HttpResponseMessage response = null;
            var resultado = 0;
            var state = ServiceManager<RelacionDAO>.Provider.RegistrarRelacionApp(objRegistro, out resultado);

            if (resultado == 0 || !state)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            response = Request.CreateResponse(HttpStatusCode.OK, state);

            return response;
        }

        [Route("ActualizarRelacionApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ActualizarRelacionApp(RelacionDTO objRegistro)
        {
            var user = TokenGenerator.GetCurrentUser();
            objRegistro.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var resultado = 0;
            var state = ServiceManager<RelacionDAO>.Provider.ActualizarRelacionApp(objRegistro, out resultado);

            if (resultado == 0 || !state)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            response = Request.CreateResponse(HttpStatusCode.OK, state);

            return response;
        }

        [Route("Bandeja/ExportarReporteRelacionesApps")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarReporteRelacionesApps(string CodigoAPT, string EstadoIdStr, DateTime? FechaConsulta = null)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId;

            CodigoAPT = string.IsNullOrEmpty(CodigoAPT) ? string.Empty : CodigoAPT;
            EstadoIdStr = string.IsNullOrEmpty(EstadoIdStr) ? string.Empty : EstadoIdStr;
            matricula = string.IsNullOrEmpty(matricula) ? string.Empty : matricula;

            var data = new ExportarData().ExportarReporteRelacionesApps(CodigoAPT, EstadoIdStr, perfilId, matricula, FechaConsulta);

            var filename = string.Format("ReporteRelacionAplicacionesDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("EliminarRelacionApp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EliminarRelacionApp(RelacionDTO objRegistro)
        {
            var user = TokenGenerator.GetCurrentUser();
            objRegistro.UsuarioCreacion = user.Matricula;

            HttpResponseMessage response = null;
            var resultado = 0;
            var state = ServiceManager<RelacionDAO>.Provider.EliminarRelacionApp(objRegistro, out resultado);

            if (resultado == 0 || !state)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            response = Request.CreateResponse(HttpStatusCode.OK, state);

            return response;
        }

        [Route("GetComentarioEliminacionRelacionApp/{Id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetComentarioEliminacionRelacionApp(int Id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.GetComentarioEliminacionRelacionApp(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // <--
    }
}
