using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.Cross;
using BCP.CVT.Services.Exportar;
using System.Web;
using BCP.CVT.Services.CargaMasiva;
using System.Threading.Tasks;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Equipo")]
    public class EquipoController : BaseController
    {
        // POST: api/Equipo/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDominios(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;

            var totalRows = 0;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

            if (pag.PerfilId == (int)EPerfilBCP.Administrador || pag.PerfilId == (int)EPerfilBCP.GestorCVT_CatalogoTecnologias)
            {
                var registros = ServiceManager<EquipoDAO>.Provider.GetEquipos(pag.nombre, pag.IP, pag.so, pag.ambienteIds, pag.tipoIds, idSubdominio, pag.desIds, pag.subsiIds, pag.flagActivo, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

                if (registros == null)
                    return NotFound();

                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Subsidiaria = HttpUtility.HtmlEncode(x.Subsidiaria);
                });

                dynamic reader = new BootstrapTable<EquipoDTO>()
                {
                    Total = totalRows,
                    Rows = registros
                };

                return Ok(reader);
            }
            else
            {
                // PENDIENTE POR CONCLUIR
                var registros = ServiceManager<EquipoDAO>.Provider.GetEquiposConsultor(pag.nombre, pag.IP, pag.so, pag.ambienteIds, pag.tipoIds, idSubdominio, pag.desIds, pag.subsiIds, pag.flagActivo, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.Matricula, out totalRows);

                if (registros == null)
                    return NotFound();

                registros.ForEach(x =>
                {
                    x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                    x.Subsidiaria = HttpUtility.HtmlEncode(x.Subsidiaria);
                });

                dynamic reader = new BootstrapTable<EquipoDTO>()
                {
                    Total = totalRows,
                    Rows = registros
                };

                return Ok(reader);
            }
        }

        // GET: api/Equipo/ObtenerTipoEquipo
        [Route("ObtenerTipoEquipo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTipoEquipo()
        {
            var listSubdom = ServiceManager<EquipoDAO>.Provider.GetTipoEquipos();
            if (listSubdom == null)
                return NotFound();

            return Ok(listSubdom);
        }

        // GET: api/Equipo/ObtenerAmbiente
        [Route("ObtenerAmbiente")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAmbiente()
        {
            var listSubdom = ServiceManager<AmbienteDAO>.Provider.GetAmbiente();
            if (listSubdom == null)
                return NotFound();

            return Ok(listSubdom);
        }

        [Route("GetTicketByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTicketByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetTicketByFiltro(filtro, 0);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }
        [Route("GetEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, 0);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }


        [Route("GetEquipoByFiltro_ComponenteDiagramaInfra")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro_ComponenteDiagramaInfra(string filtro, string codigoApt)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro_ComponenteDiagramaInfra(filtro, codigoApt);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetEquipoByFiltroActivo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltroActivo(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltroActivo(filtro, 0);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetEquipoByFiltroAndTipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltroAndTipo(string filtro, int tipo)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, tipo);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetSOByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSOByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<EquipoDAO>.Provider.GetSOTecnologias(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("BajaServidores")]
        [HttpPost]
        [Authorize]
        //public HttpResponseMessage BajaServidores(PaginacionEquipo pag)
        public IHttpActionResult BajaServidores(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;

            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetServidoresBaja(pag.lstServidores, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Ticket = HttpUtility.HtmlEncode(x.Ticket);
                x.NombreArchivo = HttpUtility.HtmlEncode(pag.NombreArchivo);
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

        [Route("ListarPendienteBajaServidores")]
        [HttpPost]
        [Authorize]
        //public HttpResponseMessage BajaServidores(PaginacionEquipo pag)
        public IHttpActionResult ListarPendienteBajaServidores(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;

            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetListarPendienteBajaServidores(pag.nombre, pag.Ticket, pag.FechaBusIni, pag.FechaBusFin, pag.EstadoSolicitudBaja, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Ticket = HttpUtility.HtmlEncode(x.Ticket);
                x.NombreArchivo = HttpUtility.HtmlEncode(pag.NombreArchivo);
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

        [Route("InsertarBajaServidores")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertarBajaServidores(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;
            var totalRows = 0;
            HttpResponseMessage response = null;

            var dataResult = ServiceManager<EquipoDAO>.Provider.InsertarBajaServidores(pag.lstServidores, pag.Matricula, pag.NombreArchivo, out totalRows);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }
        [Route("ProcesarBajaServidores")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ProcesarBajaServidores(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;
            var totalRows = 0;
            HttpResponseMessage response = null;

            var dataResult = ServiceManager<EquipoDAO>.Provider.ProcesarBajaServidores(pag.lstServidores, pag.Matricula, pag.isApprovedBaja, out totalRows);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

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
        #region GESTION EQUIPOS

        [Route("AddOrEditEquipoSoftwareBase")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEquipoSoftwareBase(EquipoSoftwareBaseDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;
            entidad.CorreoSolicitante = user.CorreoElectronico;
            entidad.NombreCreadoPor = user.Nombres;

            HttpResponseMessage response = null;
            int entidadId = ServiceManager<EquipoDAO>.Provider.AddOrEditEquipoSoftwareBase(entidad);
            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("BuscarEBS")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetESBSearch(PaginacionEquipo pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetESBSearch(pag, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Equipo/5
        [Route("ObtenerEBS/{id:int}")]
        [ResponseType(typeof(EquipoSoftwareBaseDTO))]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoSoftwareBaseById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<EquipoDAO>.Provider.GetEquipoSoftwareBaseById(id);
            if (entidad == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("ObtenerESBConfigurado/{id:int}")]
        [ResponseType(typeof(EquipoSoftwareBaseDTO))]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoSoftwareBaseConfiguradoById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<EquipoDAO>.Provider.GetEquipoSoftwareBaseConfiguradoById(id);
            if (entidad == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }


        // POST: api/Equipo/ListadoGestionEquipo
        [Route("ListadoGestionEquipo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListGestionEquipo(PaginacionEquipo pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipo(pag.nombre, pag.tipoEquipoId, pag.tipoEquipoIds, pag.desId, pag.exCalculoId, pag.flagActivo, pag.subsidiariaId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Subsidiaria = HttpUtility.HtmlEncode(x.Subsidiaria);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Ambiente = HttpUtility.HtmlEncode(x.Ambiente);
                x.SistemaOperativo = HttpUtility.HtmlEncode(x.SistemaOperativo);
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

        // POST: api/Equipo/ListadoGestionEquipo
        [Route("ListadoEquipoDesactivados")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListEquipoDesactivados(PaginacionEquipo pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquiposDesactivados(pag.nombre, pag.tipoEquipoIds, pag.subsidiariaIds, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Subsidiaria = HttpUtility.HtmlEncode(x.Subsidiaria);

            });

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Equipo/5
        [Route("{id:int}")]
        [ResponseType(typeof(EquipoDTO))]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<EquipoDAO>.Provider.GetEquipoById(id);
            if (entidad == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var listAmbiente = ServiceManager<AmbienteDAO>.Provider.GetAmbienteByFiltro(null);
            var listPCI = ServiceManager<EquipoDAO>.Provider.GetPCIByFiltro(null);
            var listTipoEquipo = ServiceManager<EquipoDAO>.Provider.GetTipoEquipoByFiltro(null);
            var listSO = ServiceManager<EquipoDAO>.Provider.GetSOTecnologias(null);
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listDominioRed = ServiceManager<DominioRedDAO>.Provider.GetDominioRedByFiltro(null);
            var listTipoExclusion = ServiceManager<TipoExclusionDAO>.Provider.GetTipoExclusionByFiltro(null);
            var listDescubrimiento = Utilitarios.EnumToList<EDescubrimiento>();
            var listDescubrimientoAnterior = Utilitarios.EnumToList<EDescubrimientoAnterior>();
            var listEstadoCalculo = Utilitarios.EnumToList<EEstadoCalculo>();
            var listCaracteristicaEquipo = Utilitarios.EnumToList<ECaracteristicaEquipo>();
            var lEstadoEquipo = Utilitarios.EnumToList<EEstadoEquipo>();
            var lFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>();

            //var lTipoActivo = Utilitarios.EnumToList<ETipoActivoEquipo>();
            //var lDimension = Utilitarios.EnumToList<EDimensionEquipo>();
            //var lBackup = Utilitarios.EnumToList<EBackup>();
            //var lBackupFrecuencia = Utilitarios.EnumToList<EBackupFrecuencia>();
            //var lCona = Utilitarios.EnumToList<ECONA>();
            //var lSede = Utilitarios.EnumToList<ESedeEquipo>();
            //var lIntegracion = Utilitarios.EnumToList<EIntegracionGestorInteligencia>();
            //var lCriticidad = Utilitarios.EnumToList<ECriticidadEquipo>();
            //var lCyberSoc = Utilitarios.EnumToList<ECyberSoc>();

            var lTipoActivo = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.TipoActivo), (int)EEntidadParametrica.APPLIANCE);
            var lDimension = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.Dimension), (int)EEntidadParametrica.APPLIANCE);
            var lBackup = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.Backup), (int)EEntidadParametrica.APPLIANCE);
            var lBackupFrecuencia = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.BackupFrecuencia), (int)EEntidadParametrica.APPLIANCE);
            var lCona = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.CONA), (int)EEntidadParametrica.APPLIANCE);
            var lSede = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.Sede), (int)EEntidadParametrica.APPLIANCE);
            var lIntegracion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.IntegracionGestorInteligencia), (int)EEntidadParametrica.APPLIANCE);
            var lCriticidad = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.Criticidad), (int)EEntidadParametrica.APPLIANCE);
            var lCyberSoc = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.CyberSoc), (int)EEntidadParametrica.APPLIANCE);

            var lEstadoIntegracionSIEM = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.EstadoIntegracionSIEM), (int)EEntidadParametrica.APPLIANCE);
            var lConaAvanzado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.CONAAvanzado), (int)EEntidadParametrica.APPLIANCE);
            var lEstadoAppliance = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadAppliance.EstadoAppliance), (int)EEntidadParametrica.APPLIANCE);

            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_OTROS_EQUIPOS");
            var valor = parametro != null ? bool.Parse(parametro.Valor) : false;
            var lEstadoBajaServidor = ServiceManager<EquipoDAO>.Provider.GetEstadoBajaServidorByFiltro();
            

            var dataRpta = new
            {
                Ambiente = listAmbiente,
                TipoEquipo = listTipoEquipo,
                Descubrimiento = listDescubrimiento.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                DescubrimientoAnterior = listDescubrimientoAnterior.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                SO = listSO,
                EstadoCalculo = listEstadoCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                DominioRed = listDominioRed,
                TipoExclusion = listTipoExclusion,
                CaracteristicaEquipo = listCaracteristicaEquipo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                EstadoEquipo = lEstadoEquipo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Dominio = listDominio,
                FechaCalculo = lFechaCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                ParametroEquipo = valor,
                CyberSoc = lCyberSoc, //.Select(x => Utilitarios.GetEnumDescription2(x)).ToArray(),
                TipoActivo = lTipoActivo,
                Dimension = lDimension,
                Backup = lBackup,
                BackupFrecuencia = lBackupFrecuencia,
                Cona = lCona, //.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Sede = lSede, //.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                IntegracionGestor = lIntegracion, //.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Criticidad = lCriticidad, //.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()
                EstadoIntegracionSIEM = lEstadoIntegracionSIEM,
                ConaAvanzado = lConaAvanzado,
                EstadoAppliance = lEstadoAppliance,
                TipoPCI = listPCI,
                EstadoBajaServidor = lEstadoBajaServidor
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        // POST: api/Equipo
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(EquipoDTO))]
        [Authorize]
        public HttpResponseMessage PostEquipo(EquipoDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;
            entidad.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<EquipoDAO>.Provider.AddOrEditEquipo(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        // POST: api/Equipo/AsignarSO
        [Route("AsignarSO")]
        [HttpPost]
        [ResponseType(typeof(EquipoTecnologiaDTO))]
        [Authorize]
        public HttpResponseMessage PostAsignarSO(EquipoTecnologiaDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<EquipoDAO>.Provider.AsignarSOTecnologias(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("AsignarFechaFin")]
        [HttpPost]
        [ResponseType(typeof(EquipoDTO))]
        [Authorize]
        public HttpResponseMessage PostAsignarFechaFin(EquipoDTO entidad)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<EquipoDAO>.Provider.AsignarFechaFin(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        // GET: api/Equipo/CambiarEstado/5
        [Route("ObtenerSOById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSOById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<EquipoDAO>.Provider.GetSOById(Id);

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);
            return response;
        }

        [Route("GetEquipoDetalle/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoDetalle(int id)
        {
            HttpResponseMessage response = null;


            var dataRpta = ServiceManager<EquipoDAO>.Provider.GetEquipoDetalleById(id);
            if (dataRpta != null)
            {
                //dataRpta.Storage = ServiceManager<StorageDAO>.Provider.GetStorageEquipo(id);

                if (dataRpta.AmbienteId != 0)
                    dataRpta.AmbienteDTO = ServiceManager<AmbienteDAO>.Provider.GetAmbienteById(dataRpta.AmbienteId.Value);

                var rptaDetalle = ServiceManager<EquipoDAO>.Provider.GetEquipoDetalleAdicional(dataRpta.Nombre);
                if (rptaDetalle != null)
                {
                    dataRpta.MemoriaRam = rptaDetalle.MemoriaRam;
                    dataRpta.Procesadores = rptaDetalle.Procesadores;
                    dataRpta.Discos = rptaDetalle.Discos;
                }

                dataRpta.CantidadVulnerabilidades = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetCantidadVulnerabilidadesXEquipoSP(id);
            }

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetCertificadoDigitalByDetalleEquipoId/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCertificadoDigitalByDetalleEquipoId(int id)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            int pageNumber = 1;
            int pageSize = 10;
            var registros = ServiceManager<RelacionDAO>.Provider.GetCertificadoDigitalByEquipoId(id, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GetEquipoDetalleEscaneadasVsRegistradas")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoDetalleEscaneadasVsRegistradas(int equipoId, DateTime fecha)
        {
            HttpResponseMessage response = null;

            var dataPie = ServiceManager<EquipoDAO>.Provider.GetEquipoDetalleEscaneadasVsRegistradas(equipoId, fecha);

            response = Request.CreateResponse(HttpStatusCode.OK, dataPie);
            return response;
        }

        [Route("GetTecnologiaByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByEquipoId(int equipoId, string fecha, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
            var proyeccionMeses1 = paramProyeccion1 != null ? paramProyeccion1.Valor : "12";
            var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
            var proyeccionMeses2 = paramProyeccion2 != null ? paramProyeccion2.Valor : "24";

            var dataRpta = new DetalleEquipoDataTecnologias();
            dataRpta.Proyeccion1Meses = proyeccionMeses1;
            dataRpta.Proyeccion2Meses = proyeccionMeses2;

            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaByEquipoId(equipoId, fecha, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.SubdominioNomb = HttpUtility.HtmlEncode(x.SubdominioNomb);
                    x.DominioNomb = HttpUtility.HtmlEncode(x.DominioNomb);
                    x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                });

                dataRpta.Tecnologias = new BootstrapTable<TecnologiaDTO>() { Rows = registros, Total = totalRows };

                //var dataRpta = new { Total = totalRows, Rows = registros };
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        #endregion
        [Route("GestionEquipos/Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionEquipos(string filtro, int tipoEquipoId, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, string sortName, string sortOrder, string tipoEquipoIds)
        {
            string nomArchivo = "";
            var idsTipoEquipo = new List<int>();

            if (!string.IsNullOrEmpty(tipoEquipoIds))
            {
                var arr_tipoEquipoIds = tipoEquipoIds.Split(';');
                idsTipoEquipo = arr_tipoEquipoIds.Select(x => int.Parse(x)).ToList();
            }

            var data = new ExportarData().ExportarGestionEquipos(filtro, tipoEquipoId, idsTipoEquipo, desId, exCalculoId, flagActivo, subsidiariaId, sortName, sortOrder, (int)EPerfilBCP.Administrador, string.Empty);
            nomArchivo = flagActivo != null ? "ListadoGestionEquipo" : "ListadoEquipoDesactivado";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionEquipos/ExportarAdicionales")]
        [HttpGet]
        public IHttpActionResult GetExportarAdicionales()
        {

            var data = new ExportarData().ExportarDatosAdicionales();
            var nomArchivo = "ListadoDatosAdicionalesEquipo";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        // POST: api/Equipo/ListadoGestionEquipo
        [Route("GestionEquipos/Appliance/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListAppliance(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoAppliance(pag, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.EquipoSoftwareBase.Serial = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Serial);
                x.EquipoSoftwareBase.Modelo = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Modelo);
                x.EquipoSoftwareBase.Vendor = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Vendor);
                x.EquipoSoftwareBase.Tecnologia = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Tecnologia);
                x.EquipoSoftwareBase.Version = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Version);
                x.EquipoSoftwareBase.Hostname = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Hostname);
                x.EquipoSoftwareBase.IP = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.IP);
                x.EquipoSoftwareBase.ArquitectoSeguridad = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.ArquitectoSeguridad);
                x.EquipoSoftwareBase.SoportePrimerNivel = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.SoportePrimerNivel);
                x.EquipoSoftwareBase.Proveedor = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Proveedor);
                x.EquipoSoftwareBase.ComentariosFechaFin = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.ComentariosFechaFin);
                x.EquipoSoftwareBase.FormaLicenciamiento = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.FormaLicenciamiento);
                x.EquipoSoftwareBase.Sala = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Sala);
                x.EquipoSoftwareBase.RACK = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.RACK);
                x.EquipoSoftwareBase.Ubicacion = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Ubicacion);
                x.EquipoSoftwareBase.BackupDescripcion = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.BackupDescripcion);
                x.EquipoSoftwareBase.ConectorSIEM = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.ConectorSIEM);
                x.EquipoSoftwareBase.UmbralCPU = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.UmbralCPU);
                x.EquipoSoftwareBase.UmbralMemoria = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.UmbralMemoria);
                x.EquipoSoftwareBase.UmbralDisco = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.UmbralDisco);
                x.EquipoSoftwareBase.EquipoDetalle = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.EquipoDetalle);
                x.EquipoSoftwareBase.Ventana = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Ventana);
            });

            var reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("GestionEquipos/ConfiguracionAppliance/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListConfiguracionAppliance(PaginacionEquipo pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId == 2 ? 0 : 1;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
            if (pag.PerfilId == (int)EPerfilBCP.Administrador)
            {
                var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoConfiguracionAppliance(pag, 1, out totalRows);
                registros.ForEach(x =>
                {
                    x.EquipoSoftwareBase.Tecnologia = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Tecnologia);
                });
                var reader = new BootstrapTable<EquipoDTO>()
                {
                    Total = totalRows,
                    Rows = registros
                };
                response = Request.CreateResponse(HttpStatusCode.OK, reader);
                return response;
            }
            else
            {
                var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoConfiguracionAppliance(pag, 2, out totalRows);
                registros.ForEach(x =>
                {
                    x.EquipoSoftwareBase.Tecnologia = HttpUtility.HtmlEncode(x.EquipoSoftwareBase.Tecnologia);
                });
                var reader = new BootstrapTable<EquipoDTO>()
                {
                    Total = totalRows,
                    Rows = registros
                };
                response = Request.CreateResponse(HttpStatusCode.OK, reader);
                return response;
            }
        }

        [Route("GestionEquipos/ConfiguracionAppliance/Desactivar")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDesactivarConfiguracion(int Id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<EquipoDAO>.Provider.RemoveEquipoSoftwareBase(Id);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GestionEquipos/ConfiguracionAppliance/Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionEquipoApplianceConfiguracion(string filtro, string filtroEquipo, int? flagActivo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId == 2 ? 0 : 1;

            string filename = "";
            var idsTipoEquipo = new List<int>();

            var data = new ExportarData().ExportarGestionEquipoApplianceConfiguracion(filtro, filtroEquipo, perfilId, matricula, flagActivo);
            filename = "ListadoEquiposAppliance";
            filename = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }


        [Route("GestionEquipos/Appliance/ExisteEquipo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteEquipo(string clave)
        {
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(clave)) clave = null;
            int estado = ServiceManager<EquipoDAO>.Provider.ExisteEquipoApplianceByNombre(clave);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GestionEquipos/Appliance/ExisteEquipoAsociado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteEquipoAsociado(int id)
        {
            HttpResponseMessage response = null;
            //bool estado = ServiceManager<EquipoDAO>.Provider.ExisteEquipoAsociadoById(id);
            string resultado = ServiceManager<EquipoDAO>.Provider.ExisteEquipoAsociadoSoftwareBaseById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, resultado);
            return response;
        }

        [Route("GestionEquipos/Appliance/Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionEquipoAppliance(string filtro, string filtroEquipo, int? flagActivo)
        {
            string filename = "";
            var idsTipoEquipo = new List<int>();

            var data = new ExportarData().ExportarGestionEquipoAppliance(filtro, filtroEquipo, flagActivo);
            filename = "ListadoActivosTSI";
            filename = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("GestionEquipos/Exportar/Actualizar")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionEquiposActualizar(int tipoEquipoId)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarGestionEquiposActualizar(string.Empty, tipoEquipoId, -1, -1, 0, -1, "Id", "asc", (int)EPerfilBCP.Administrador, string.Empty);
            nomArchivo = "ListadoEquipo";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportar(string nombre, string so, string ambiente, string tipo, string desId, string subsiId, int? flagActivo, string sortName, string sortOrder, string IP)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            int perfilId = user.PerfilId == 2 ? 0 : 1;
            string perfil = user.Perfil;

            string nomArchivo = "";
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
            var data = new ExportarData().ExportarEquipos(nombre, so, ambiente, tipo, idSubdominio, desId, subsiId, flagActivo, sortName, sortOrder, perfil, perfilId, matricula, IP);
            nomArchivo = string.Format("ListadoEquipo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GetServidoresRelacionadosByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetServidoresRelacionados(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetServidoresRelacionadosByCodigoAPT(codigoAPT, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }


        [Route("GetCertificadosDigitalesRelacionadosByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCertificadoDigitalRelacionados(string codigoAPT, int pageNumber, int pageSize)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
            var proyeccionMeses1 = paramProyeccion1 != null ? paramProyeccion1.Valor : "12";
            var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
            var proyeccionMeses2 = paramProyeccion2 != null ? paramProyeccion2.Valor : "24";

            var dataRpta = new DetalleEquipoDataCertificadosDigitales();
            dataRpta.Proyeccion1Meses = proyeccionMeses1;
            dataRpta.Proyeccion2Meses = proyeccionMeses2;

            var registros = ServiceManager<EquipoDAO>.Provider.GetCertificadosDigitalesByCodigoAPT(codigoAPT, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                //var dataRpta = new { Total = totalRows, Rows = registros };
                dataRpta.CertificadosDigitales = new BootstrapTable<CertificadoDigitalDTO>() { Rows = registros, Total = totalRows };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }


        [Route("GetApisRelacionadasByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApisRelacionadas(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetApisRelacionadasByCodigoAPT(codigoAPT, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }


        [Route("GetClientSecretsRelacionadasByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetClientSecretsRelacionadas(string codigoAPT, int pageNumber, int pageSize)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
            var proyeccionMeses1 = paramProyeccion1 != null ? paramProyeccion1.Valor : "12";
            var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
            var proyeccionMeses2 = paramProyeccion2 != null ? paramProyeccion2.Valor : "24";

            var dataRpta = new DetalleEquipoDataClientSecrets();
            dataRpta.Proyeccion1Meses = proyeccionMeses1;
            dataRpta.Proyeccion2Meses = proyeccionMeses2;

            var registros = ServiceManager<EquipoDAO>.Provider.GetClientSecretsByCodigoAPT(codigoAPT, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                dataRpta.ClientSecrets = new BootstrapTable<ClientSecretDTO>() { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }


        [Route("GetEquipoByTecnologiaId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoByTecnologiaId(tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ExportarReporteSinRelaciones")]
        [HttpGet]
        public IHttpActionResult GetExportarReporteSinRelaciones(string equipo, string tipoEquipoFiltrar, string so, string fechaConsulta, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            //tipoEquipoFiltrar = string.IsNullOrEmpty(tipoEquipoFiltrar) ? string.Empty : tipoEquipoFiltrar;
            //equipo = string.IsNullOrEmpty(equipo) ? string.Empty : equipo;
            //so = string.IsNullOrEmpty(so) ? string.Empty : so;

            //var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            //var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
            var filtros = new PaginaReporteHuerfanos();
            filtros.Equipo = equipo ?? string.Empty;
            filtros.TipoEquipoToString = tipoEquipoFiltrar ?? string.Empty;
            filtros.SistemaOperativo = so ?? string.Empty;
            filtros.FechaConsulta = fechaConsulta;
            filtros.sortName = sortName;
            filtros.sortOrder = sortOrder;
            filtros.pageNumber = 1;
            filtros.pageSize = int.MaxValue;

            var data = new ExportarData().ExportarReporteSinRelaciones(filtros);
            nomArchivo = string.Format("ListadoEquipoSinRelacionConAplicacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListarEquipoTecnologiaByEquipoId/{Id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListEquipoTecnologiaByEquipoId(int Id)
        {
            HttpResponseMessage response = null;
            var listado = ServiceManager<EquipoDAO>.Provider.GetEquipoTecnologiaByEqId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, listado);
            return response;
        }

        [Route("GestionEquipos/ExportarDetalle")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionEquiposDetalle(string filtro, int tipoEquipoId, int desId, int exCalculoId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarGestionEquiposDetalle(filtro, tipoEquipoId, desId, exCalculoId, sortName, sortOrder);
            nomArchivo = string.Format("ListadoGestionEquipoDetallado_{0}.csv", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("EquipoXTecnologiaId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoXTecnologiaId(tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>() { Total = totalRows, Rows = registros };
            return Ok(reader);
        }

        [Route("GestionEquipos/ObtenerPlantillaEquipos")]
        [HttpGet]
        public IHttpActionResult PostPlantillaEquipos()
        {
            string nomArchivo = "PlantillaCargaEquipos.xlsx";
            var data = new ExportarData().ObtenerPlantillaEquipos();

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionEquipos/CargarEquipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCargarEquipos()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    EstadoCargaMasiva estadoCM = null;
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);

                    estadoCM = new CargaData().CargaMasivaEquipos(extension, inputStream);
                    response = Request.CreateResponse(HttpStatusCode.OK, estadoCM);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("GestionEquipos/ActualizarEquipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarEquipos()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;
                var user = TokenGenerator.GetCurrentUser();
                string username = user.Matricula;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    EstadoCargaMasiva estadoCM = null;
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);

                    estadoCM = new CargaData().UpdateMasivoEquipos(extension, inputStream, username);
                    response = Request.CreateResponse(HttpStatusCode.OK, estadoCM);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("GestionEquipos/EjecutarSP")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEjecutarSPServidorIntoEquipo()
        {
            try
            {
                HttpResponseMessage response = null;
                var data = new CargaData().EjecutarSPServidorEquipo();
                response = Request.CreateResponse(HttpStatusCode.OK, data);

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/Tipo/CambiarEstado/5
        [Route("GestionEquipos/CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id, bool Activo, string Motivo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<EquipoDAO>.Provider.CambiarEstado(Id, !Activo, Usuario, Motivo);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/Tipo/CambiarEstado/5
        [Route("GestionEquipos/CambiarEstadoEquipoSoftwareBase")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CambiarEstadoEquipoSoftwareBase(int Id, bool Activo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;
            string NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<EquipoDAO>.Provider.CambiarEstadoEquipoSoftwareBase(Id, !Activo, Usuario, NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }



        [Route("EquipoExclusion/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEquipoExclusionListCombos()
        {
            HttpResponseMessage response = null;

            var listTipoExclusion = ServiceManager<TipoExclusionDAO>.Provider.GetTipoExclusionByFiltro(null);

            var dataRpta = new
            {
                TipoExclusion = listTipoExclusion
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("EquipoExclusion/ListarEquiposExcluidos")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListEquipoExclusion(PaginacionEquipo pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoExclusion(pag.nombre, pag.tipoExclusionId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<HistoricoExclusionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("EquipoExclusion/Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarEquipoExclusion(string filtro, int tipoExclusionId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarEquipoExclusion(filtro, tipoExclusionId, sortName, sortOrder);
            nomArchivo = string.Format("ListadoEquipoExclusion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionEquipos/Exportar/Desactivados")]
        [HttpGet]
        public IHttpActionResult GetExportarEquiposDesactivados(string filtro, string tipoEquipoId, string subsidiariaId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            List<int> tipoEquipoIds = tipoEquipoId == null ? new List<int>() : tipoEquipoId.Split('|').Select(Int32.Parse).ToList();
            List<int> subsidiariaIds = subsidiariaId == null ? new List<int>() : subsidiariaId.Split('|').Select(Int32.Parse).ToList();
            var data = new ExportarData().ExportarEquiposDesactivados(filtro, tipoEquipoIds, subsidiariaIds, sortName, sortOrder);
            nomArchivo = "ListadoEquipoDesactivado";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarServidoresRelacionadosByCodigoAPT")]
        [HttpGet]
        public IHttpActionResult ExportarServidoresRelacionadosByCodigoAPT(string codigoAPT, string sortName, string sortOrder)
        {
            string fileName = "";
            var data = new ExportarData().ExportarServidoresRelacionadosByAplicacion(codigoAPT, sortName, sortOrder);
            fileName = string.Format("ListaServidoresRelacionadosXAplicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("ExportarCertificadosDigitalesRelacionadosByCodigoAPT")]
        [HttpGet]
        public IHttpActionResult ExportarCertificadosDigitalesRelacionadosByCodigoAPT(string codigoAPT)
        {
            string fileName = "";
            var data = new ExportarData().ExportarCertificadosDigitalesRelacionadosByAplicacion(codigoAPT);
            fileName = string.Format("ListaCertificadosDigitalesRelacionadosXAplicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("ExportarClientSecretsRelacionadosByCodigoAPT")]
        [HttpGet]
        public IHttpActionResult ExportarClientSecretsRelacionadosByCodigoAPT(string codigoAPT)
        {
            string fileName = "";
            var data = new ExportarData().ExportarClientSecretsRelacionadosByAplicacion(codigoAPT);
            fileName = string.Format("ListaClientSecretsRelacionadosXAplicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("ExportarApisRelacionadasByCodigoAPT")]
        [HttpGet]
        public IHttpActionResult ExportarApisRelacionadasByCodigoAPT(string codigoAPT, string sortName, string sortOrder)
        {
            string fileName = "";
            var data = new ExportarData().ExportarApisRelacionadasByAplicacion(codigoAPT, sortName, sortOrder);
            fileName = string.Format("ListaApisRelacionadasXAplicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("TecnologiaOwner/ListarEquipoByTecnologiaTipoEquipo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListarEquipoByTecnologiaTipoEquipoTecnologiaOwner(int tecnologiaId, int tipoEquipoId)
        {
            var registros = ServiceManager<EquipoDAO>.Provider.ListarEquiposXTecnologiaTipoEquipo(tecnologiaId, tipoEquipoId);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("TecnologiaOwner/ExportarListarEquipoByTecnologiaTipoEquipo")]
        [HttpGet]
        public IHttpActionResult PostExportarListarEquipoByTecnologiaTipoEquipoTecnologiaOwner(int tecnologiaId, int tipoEquipoId, string tecnologiaStr, string tipoEquipoStr, string filtro)
        {
            string nomArchivo = string.Format("ListaTecnologias_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListarEquipoByTecnologiaTipoEquipoTecnologiaOwner(tecnologiaId, tipoEquipoId, tecnologiaStr, tipoEquipoStr, filtro);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ProductoOwner/ListarEquipoByProductoTipoEquipo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListarEquipoByProductoTipoEquipoProductoOwner(int productoId, int tipoEquipoId)
        {
            var registros = ServiceManager<EquipoDAO>.Provider.ListarEquiposXProductoTipoEquipo(productoId, tipoEquipoId);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("ProductoOwner/ExportarListarEquipoByProductoTipoEquipo")]
        [HttpGet]
        public IHttpActionResult PostExportarListarEquipoByProductoTipoEquipoProductoOwner(int productoId, int tipoEquipoId, string productoStr, string tipoEquipoStr, string filtro)
        {
            string nomArchivo = string.Format("ListaTecnologias_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListarEquipoByProductoTipoEquipoProductoOwner(productoId, tipoEquipoId, productoStr, tipoEquipoStr, filtro);

            return Ok(new { excel = data, name = nomArchivo });
        }



        //[Route("ExportarListadoByProducto")]
        //[HttpGet]
        //public HttpResponseMessage PostExportarListTecnologiaByProducto(int productoId, string productoStr)
        //{
        //    string nomArchivo = string.Format("ListaTecnologias_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        //    var data = new ExportarData().ExportarListarEquipoByProductoTipoEquipoProductoOwner(productoId, productoStr);

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}


        [Route("GetEquipoServidorVirtual")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoServidorVirtual(int equipoId, int pageNumber, int pageSize)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoServidorVirtual(equipoId, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("GetEquipoServidorVirtualDetalle")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoServidorVirtualDetalle(int equipoId, int pageNumber, int pageSize)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;

            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipoServidorVirtualDetalle(equipoId, pageNumber, pageSize, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("PostExportarEquipoServidorVirtual")]
        [HttpGet]
        public IHttpActionResult PostExportarEquipoServidorVirtual(int equipoId)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarEquipoServidorVirtual(equipoId);
            nomArchivo = string.Format("ServidorVirtualEquipo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

    }
}