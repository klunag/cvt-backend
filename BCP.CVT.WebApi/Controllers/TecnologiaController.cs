using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.CargaMasiva;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.Services.Interface.TechnologyConfiguration;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tecnologia")]
    public class TecnologiaController : BaseController
    {
        //Asociados a los nuevos endpoints
        [Route("Dominios")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllDominiosActivos()
        {
            var listTec = ServiceManager<DominioDAO>.Provider.GetDominios();
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        [Route("Dominios/{domId:int}/Subdominios")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllSubdominiosActivos(int domId)
        {
            var listTec = ServiceManager<DominioDAO>.Provider.GetSubdominios(domId);
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        [Route("Tipos")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllTiposActivos()
        {
            var listTec = ServiceManager<TipoDAO>.Provider.GetAllTipoActivos();
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        [Route("Base")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpdateAtributos(DtoTecnologia tecnologia)
        {
            HttpResponseMessage response = null;
            var codeId = ServiceManager<TecnologiaDAO>.Provider.AddOrEditTecnologiaPowerApps(tecnologia);
            response = Request.CreateResponse(codeId > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest, codeId);

            return response;
        }

        // GET: api/Tecnologia
        [Route("")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllTecnologia()
        {
            var listTec = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologia();
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        // GET: api/Tecnologia/5
        [Route("{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTecById(int id, bool withAplicaciones = false)
        {
            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetTecById(id);
            if (objTec == null)
                return NotFound();

            if (withAplicaciones)
            {
                objTec.ListAplicaciones = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaAplicaciones(objTec.Id);
            }

            return Ok(objTec);
        }

        // GET: api/Tecnologia/Subdominios/3
        [Route("Subdominios/{domId:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSubByDom(int domId)
        {
            var itemsSubdom = ServiceManager<TecnologiaDAO>.Provider.GetSubByDom(domId);
            if (itemsSubdom == null)
                return NotFound();

            return Ok(itemsSubdom);
        }

        // POST: api/Tecnologia/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTec(PaginacionTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTec(pag.domId,
                                                                          pag.subdomId,
                                                                          pag.nombre,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          pag.equipo,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/Tecnologia
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostTecnologia(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioCreacion = user.Matricula;
            tecDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.AddOrEditTecnologia(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        // GET: api/Tecnologia/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<TecnologiaDAO>.Provider.GetTecById(Id);
            var retorno = ServiceManager<TecnologiaDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/Tecnologia/ValidarEquivalencia
        [Route("ValidarEquivalencia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostValidarEquivalencia(string equivalencia)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<TecnologiaDAO>.Provider.ExisteEquivalencia(equivalencia);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/Tecnologia/CambiarEstadoTec
        [Route("CambiarEstadoTec")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetCambiarEstadoTec(ParametroEstadoTec obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            //var entidad = ServiceManager<TecnologiaDAO>.Provider.GetTecById(id);
            var retorno = ServiceManager<TecnologiaDAO>.Provider.CambiarEstadoSTD(obj.id, obj.est, obj.obs, obj.UsuarioModificacion);
            return Ok(retorno);
        }


        [Route("GetValidateListData")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetValidateListData(string key, string value)
        {
            var response = ServiceManager<TechnologyConfigurationDAO>.Provider.GetValidListData(key, value);
            return Ok(response);
        }


        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var paramCompatibilidadSO = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_COMPATIBILIDAD_SO");
            var paramCompatibilidadCloud = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_PROVEEDOR_CLOUD");
            var paramSustentoMotivo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_SUSTENTO_MOTIVO");
            var paramTipoFechaInterna = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_TIPO_FECHA_INTERNA");

            var listCompatibilidadSO = (paramCompatibilidadSO.Valor ?? "").Split('|');
            var listCompatibilidadCloud = (paramCompatibilidadCloud.Valor ?? "").Split('|');
            //var listSustentoMotivo = (paramSustentoMotivo.Valor ?? "").Split('|');
            var listSustentoMotivo = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetail("MFI");
            var listTipoFechaInterna = (paramTipoFechaInterna.Valor ?? "").Split('|');
            //var listProducto = ServiceManager<ProductoDAO>.Provider.GetAllProducto();
            var listFuente = Utilitarios.EnumToList<Fuente>();
            var listFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>();
            var listEstadoObs = Utilitarios.EnumToList<ETecnologiaEstado>();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            //var listDominio = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetail("MFI");
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listFamilia = ServiceManager<FamiliaDAO>.Provider.GetAllFamiliaByFiltro(null);
            var listTipoTec = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);
            var listFechaFinSoporte = Utilitarios.EnumToList<EFechaFinSoporte>();
            //var listTipoFechaInterna = Utilitarios.EnumToList<ETipoFechaInterna>();
            var listEstadoTecnologia = Utilitarios.EnumToList<EstadoTecnologia>();
            var lAplicaTecnologia = Utilitarios.EnumToList<EAplicaATecnologia>();//ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetail("IPA");
            var listMotivo = ServiceManager<MotivoDAO>.Provider.GetAllMotivo();
            var listImplementacionAutomatizada = Utilitarios.EnumToList<EAutomatizacionImplementada>();
            var listRevisionSeguridad = Utilitarios.EnumToList<RevisionSeguridad>();
            var listUrlConfluence = Utilitarios.EnumToList<UrlConfluence>();
            var listValores = new int[] { 0, 1, 2, 3, 4, 5 }.Select(x => new { Id = x, Descripcion = x.ToString() }).ToList();
            //var listEstadoObsolescencia = Utilitarios.EnumToList<>();
            var listEsquemaLicenciamientoSuscripcion = Utilitarios.EnumToList<EEsquemaLicenciamientoSuscripcion>().Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) });
            var listTipoCicloVida = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVidaByFiltro(true);
            //var listaTipoExperto = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoByFiltro(null); 
            var listaTipoExperto = ServiceManager<AplicacionDAO>.Provider.GetProductoManagerFiltro(null);
            var listMotivoDesactiva = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetail("MDN");
            var listEstadoResolucionCambioBajaOwner = ServiceManager<TechnologyConfigurationDAO>.Provider.EstadoResolucionCambioBajaOwner();

            var dataRpta = new
            {
                //Producto = listProducto,
                Fuente = listFuente.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                FechaCalculo = listFechaCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                EstadoObs = listEstadoObs.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }),
                Tipo = listTipoTec,
                Dominio = listDominio,
                SubDominio = listSubDominio,
                Familia = listFamilia,
                TipoTec = listTipoTec,
                CodigoInterno = (int)ETablaProcedencia.CVT_Tecnologia,
                FechaFinSoporte = listFechaFinSoporte.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                TipoFechaInterna = listTipoFechaInterna,
                EstadoTecnologia = listEstadoTecnologia.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription3(x) }).ToList(),
                AplicaTecnologia = lAplicaTecnologia.Select(x => Utilitarios.GetEnumDescription3(x)).ToArray(), //lAplicaTecnologia.Select(x => new { Id = x.Id, Descripcion = x.Descripcion }).ToList(), //CORRECCION ENUM
                RevisionSeguridad = listRevisionSeguridad.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription3(x) }).ToList(), //CORRECCION ENUM
                UrlConfluence = listUrlConfluence.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(), //CORRECCION ENUM
                ImplementacionAutomatizada = listImplementacionAutomatizada.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Motivo = listMotivo,
                CompatibilidadSO = listCompatibilidadSO,
                CompatibilidadCloud = listCompatibilidadCloud,
                SustentoMotivo = listSustentoMotivo.Select(x => new { Id = x.Descripcion, Descripcion = x.Descripcion }).ToList(), //listSustentoMotivo.Select(k => new { Id = k.Id, Descripcion = k.Descripcion}).ToList(),
                Valores = listValores,
                EsquemaLicenciamientoSuscripcion = listEsquemaLicenciamientoSuscripcion,
                TipoCicloVida = listTipoCicloVida,
                TipoExperto = listaTipoExperto,
                MotivoDesactiva = listMotivoDesactiva.Select(x => new { Id = x.Id, Descripcion = x.Descripcion }).ToList(),
                EstadoResolucionCambioBajaOwner = listEstadoResolucionCambioBajaOwner
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarTec(string nombre, int dominioId, int subdominioId, string aplica, string codigo, string dueno, string equipo, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarTecnologia(nombre, dominioId, subdominioId, aplica, codigo, dueno, equipo, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        #region Usuario STD
        // POST: api/Tecnologia/ListadoSTD
        [Route("ListadoSTD")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTecSTD(PaginacionTecSTD pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecSTD(pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.casoUso,
                                                                          pag.nombre,
                                                                          pag.estadoIds,
                                                                          pag.famId,
                                                                          pag.fecId,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          pag.equipo,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          pag.flagActivo,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.MotivoDesactiva = HttpUtility.HtmlEncode(x.MotivoDesactiva);
            });

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarSTD")]
        [HttpGet]
        public IHttpActionResult GetExportarTecSTD(string nombre, string dominioIds, string subdominioIds, string familiaId, int estadoFecSop, string casoUso, string estadoTecs, string aplica, string codigo, string dueno, string equipo, string tipoTecIds, string estObsIds, int? flagActivo, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;

            var listaDominiosIds = dominioIds == null ? new List<int>() : dominioIds.Split('|').Select(Int32.Parse).ToList();
            var listaSubDominiosIds = subdominioIds == null ? new List<int>() : subdominioIds.Split('|').Select(Int32.Parse).ToList();
            var listaEstadoTecIds = estadoTecs == null ? new List<int>() : estadoTecs.Split('|').Select(Int32.Parse).ToList();
            var listaTipoTecIds = tipoTecIds == null ? new List<int>() : tipoTecIds.Split('|').Select(Int32.Parse).ToList();
            var listaEstadoObsIds = estObsIds == null ? new List<int>() : estObsIds.Split('|').Select(Int32.Parse).ToList();

            var data = new ExportarData().ExportarTecnologiaSTD(nombre, listaDominiosIds, listaSubDominiosIds, familiaId, estadoFecSop, casoUso, listaEstadoTecIds, aplica, codigo, dueno, equipo, listaTipoTecIds, listaEstadoObsIds, flagActivo, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologiaSTD_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarReporteTecnologia")]
        [HttpGet]
        public IHttpActionResult GetExportarReporteTecnologia(string nombre, string dominioIds, string subdominioIds, string familiaId, int estadoFecSop, string casoUso, string estadoTecs, string aplica, string codigo, string dueno, string equipo, string tipoTecIds, string estObsIds, int? flagActivo, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;

            List<int> listaDominiosIds = dominioIds == null ? new List<int>() : dominioIds.Split('|').Select(Int32.Parse).ToList();
            List<int> listaSubDominiosIds = subdominioIds == null ? new List<int>() : subdominioIds.Split('|').Select(Int32.Parse).ToList();
            List<int> listaEstadoTecIds = estadoTecs == null ? new List<int>() : estadoTecs.Split('|').Select(Int32.Parse).ToList();
            List<int> listaTipoTecIds = tipoTecIds == null ? new List<int>() : tipoTecIds.Split('|').Select(Int32.Parse).ToList();
            List<int> listaEstadoObsIds = estObsIds == null ? new List<int>() : estObsIds.Split('|').Select(Int32.Parse).ToList();

            var data = new ExportarData().ExportarReporteTecnologia(nombre, listaDominiosIds, listaSubDominiosIds, familiaId, estadoFecSop, casoUso, listaEstadoTecIds, aplica, codigo, dueno, equipo, listaTipoTecIds, listaEstadoObsIds, flagActivo, sortName, sortOrder);
            nomArchivo = string.Format("ReporteTecnologiaTotal_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListarReporteTecnologia")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarReporteTecnologia(PaginacionTecSTD pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetReporteTecnologia(pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.casoUso,
                                                                          pag.nombre,
                                                                          pag.estadoIds,
                                                                          pag.famId,
                                                                          pag.fecId,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          pag.equipo,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          pag.flagActivo,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.ComentariosFechaFin = HttpUtility.HtmlEncode(x.ComentariosFechaFin);
                x.CasoUso = HttpUtility.HtmlEncode(x.CasoUso);
                x.EsqMonitoreo = HttpUtility.HtmlEncode(x.EsqMonitoreo);
                x.Versiones = HttpUtility.HtmlEncode(x.Versiones);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Tipo = HttpUtility.HtmlEncode(x.Tipo);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Requisitos = HttpUtility.HtmlEncode(x.Requisitos);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.EqAdmContacto = HttpUtility.HtmlEncode(x.EqAdmContacto);
                x.GrupoSoporteRemedy = HttpUtility.HtmlEncode(x.GrupoSoporteRemedy);
                x.Referencias = HttpUtility.HtmlEncode(x.Referencias);
                x.PlanTransConocimiento = HttpUtility.HtmlEncode(x.PlanTransConocimiento);
                x.EsqPatchManagement = HttpUtility.HtmlEncode(x.EsqPatchManagement);
                x.ConfArqSeg = HttpUtility.HtmlEncode(x.ConfArqSeg);
                x.ConfArqTec = HttpUtility.HtmlEncode(x.ConfArqTec);
            });

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ActualizarTipo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostActualizarTipo(ActualizarTecnologia pag)
        {
            var retorno = ServiceManager<TecnologiaDAO>.Provider.ActualizarRetorno(pag);

            dynamic reader = new EntidadRetorno()
            {
                CodigoRetorno = retorno.CodigoRetorno,
                Descripcion = retorno.Descripcion
            };

            return Ok(reader);
        }


        // GET: api/Tecnologia/ObtenerTecnologiaEquivalencia/5
        [Route("ListarTecnologiasEquivalentes")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetTecEquivalentesByTec(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecEqByTec(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.NombreTecnologia = HttpUtility.HtmlEncode(x.NombreTecnologia);
                x.DominioTecnologia = HttpUtility.HtmlEncode(x.DominioTecnologia);
                x.SubdominioTecnologia = HttpUtility.HtmlEncode(x.SubdominioTecnologia);
            });

            dynamic reader = new BootstrapTable<TecnologiaEquivalenciaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Tecnologia/ObtenerTecnologia
        [Route("ObtenerTecnologia")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTec()
        {
            var listTec = ServiceManager<TecnologiaDAO>.Provider.GetTec();
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        // POST: api/Tecnologia
        [Route("AsociarTecnologiaEquivalencia")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostAsocTecnologiasEq(ParametroTecEq request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var retorno = ServiceManager<TecnologiaDAO>.Provider.AsociarTecEq(request.tecId, request.Equivalencia, request.Usuario);

            return Ok(retorno);
        }
        #endregion

        [Route("GetTecnologiaByClave")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByClave(string filtro, string subdominioIds = null, int? id = null)
        {
            HttpResponseMessage response = null;
            var listTec = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologiaByClaveTecnologia(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listTec);
            return response;
        }

        [Route("GetTecnologiaEstandarByClave")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEstandarByClave(FiltroTecnologiaEstandarDTO filtro)
        {
            HttpResponseMessage response = null;
            var dataResp = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaEstandarByClaveTecnologia(filtro.filtro, filtro.getAll);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResp);
            return response;
        }

        [Route("GetTecnologiaAutocompleteById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaAutocompleteById(int Id)
        {
            HttpResponseMessage response = null;
            var item = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }

        [Route("GetTecnologiaArquetipoByClave")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaArquetipoByClave(string filtro)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaArquetipoByClaveTecnologia(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("GetTecnologiaByClaveById")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByClaveById(PostAutocomplete obj)
        {
            HttpResponseMessage response = null;
            var listTec = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologiaByClaveTecnologia(obj.filtro, obj.id, obj.dominioIds, obj.subDominioIds);
            response = Request.CreateResponse(HttpStatusCode.OK, listTec);
            return response;
        }

        [Route("GetTecnologiaByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listTec = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologia(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listTec);
            return response;
        }

        [Route("GetTecnologiaEstandarByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEstandarByFiltro(string filtro, string subdominioList, string soPcUsuarioList = null)
        {
            HttpResponseMessage response = null;
            var lista = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaEstandarByFiltro(filtro, subdominioList, soPcUsuarioList);
            response = Request.CreateResponse(HttpStatusCode.OK, lista);
            return response;
        }

        [Route("GetTecnologiaForBusqueda")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaForBusqueda(string filtro, int? id, bool flagActivo)
        {
            HttpResponseMessage response = null;
            var lista = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaForBusqueda(filtro, id, flagActivo);
            response = Request.CreateResponse(HttpStatusCode.OK, lista);
            return response;
        }

        [Route("ExisteTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteTecnologia(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.ExisteTecnologiaById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteEquivalenciaTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteEquivalenciaTecnologia(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.ExisteEquivalenciaByTecnologiaId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteRelacionByTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteRelacionByTecnologia(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.ExisteRelacionByTecnologiaId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("CambiarFlagEquivalencia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambiarFlagExperto(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            bool retorno = ServiceManager<TecnologiaDAO>.Provider.CambiarFlagEquivalencia(int.Parse(request.Id), request.Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ExisteClaveTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteClaveTecnologia(string clave, int? id, int? flagActivo)
        {
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(clave)) clave = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.ExisteClaveTecnologia(clave, id, flagActivo);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("GetTecnologiaByFiltroById")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTecnologiaByFiltroById(ObjTecnologiaVinculada obj)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologia(obj.Filtro, obj.Id, obj.IdsTec);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("ExportarEquivalenciaGeneral")]
        [HttpGet]
        public IHttpActionResult GetExportarEquivalenciaGeneral(string sortName, string sortOrder)
        {
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarTecnologiasEquivalentes(sortName, sortOrder);
            nomArchivo = string.Format("ListaEquivalenciasTecnologias_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GetSistemasOperativoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSistemasOperativoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetSistemasOperativoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetTecnologiasXAplicacionByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiasXAplicacion(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder)
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

            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasXAplicacionByCodigoAPT(codigoAPT, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                dataRpta.Tecnologias = new BootstrapTable<TecnologiaDTO>() { Rows = registros, Total = totalRows };

                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("TecnologiaVinculadaXTecnologiaId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTecnologiaVinculadaXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaVinculadaXTecnologiaId(tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>() { Total = totalRows, Rows = registros };
            return Ok(reader);
        }

        [Route("TecnologiaEstandar")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEstandar(string subdominioIds = null, string dominiosIds = null)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaEstandar(subdominioIds, dominiosIds);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("TecnologiaEstandar/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetEstandarTecnologia(FiltroTecnologiaEstandarDTO filtro)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaEstandar_2(
                filtro.nombre,
                filtro.tipoTecnologiaIds,
                filtro.estadoTecnologiaIds,
                filtro.getAll,
                filtro.subdominioIds,
                filtro.dominiosIds,
                filtro.aplicaIds,
                filtro.compatibilidadSOIds,
                filtro.compatibilidadCloudIds);

            registros.ForEach(x =>
            {
                x.DominioNomb = HttpUtility.HtmlEncode(x.DominioNomb);
                x.SubdominioNomb = HttpUtility.HtmlEncode(x.SubdominioNomb);
            });

            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("TecnologiaEstandar/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarCombosTecnologiaEstandar()
        {
            HttpResponseMessage response = null;
            var lDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var lTipos = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);
            var listEstadoObs = Utilitarios.EnumToList<ETecnologiaEstadoEstandar>().ToList();

            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("URL_PREGUNTAS_FRECUENTES");
            var dataPar = parametro != null ? parametro.Valor : "#";

            var parametro2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FILTRO_TIPO_ESTANDAR_TEC");
            var dataPar2 = parametro2 != null ? parametro2.Valor : "1|2";

            var paramCompatibilidadSO = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_COMPATIBILIDAD_SO");
            var paramCompatibilidadCloud = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_PROVEEDOR_CLOUD");

            var listCompatibilidadSO = (paramCompatibilidadSO.Valor ?? "").Split('|');
            var listCompatibilidadCloud = (paramCompatibilidadCloud.Valor ?? "").Split('|');
            var lAplicaTecnologia = Utilitarios.EnumToList<EAplicaATecnologia>().Select(x => new { Id = Utilitarios.GetEnumDescription2(x), Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();

            var dataRpta = new
            {
                Dominio = lDominio,
                TipoTecnologia = lTipos,
                EstadoObs = listEstadoObs.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }),
                UrlFAQ = dataPar,
                FiltroTipoTecnologia = dataPar2,
                CompatibilidadSO = listCompatibilidadSO,
                CompatibilidadCloud = listCompatibilidadCloud,
                PlataformaAplica = lAplicaTecnologia
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("TecnologiaEstandar/GetById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaEstandarById(int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaEstandarById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("TecnologiaEstandar/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiaEstandar(string subdominioIds, string dominiosIds)
        {
            string fileName = "";
            subdominioIds = !string.IsNullOrEmpty(subdominioIds) ? subdominioIds : "";
            dominiosIds = !string.IsNullOrEmpty(dominiosIds) ? dominiosIds : "";
            var data = new ExportarData().ExportarTecnologiaEstandar(subdominioIds, dominiosIds);
            fileName = string.Format("ListaTecnologíasEstandar_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("TecnologiaEstandar/Exportar_2")]
        [HttpPost]
        public IHttpActionResult ExportarEstandarTecnologia(FiltroTecnologiaEstandarDTO filtro)
        {
            filtro.subdominioIds = !string.IsNullOrEmpty(filtro.subdominioIds) ? filtro.subdominioIds : string.Empty;
            filtro.dominiosIds = !string.IsNullOrEmpty(filtro.dominiosIds) ? filtro.dominiosIds : string.Empty;
            filtro.nombre = !string.IsNullOrEmpty(filtro.nombre) ? filtro.nombre : string.Empty;

            var data = new ExportarData().ExportarTecnologiaEstandar_2(filtro.subdominioIds, filtro.dominiosIds, filtro.tipoTecnologiaIds, filtro.estadoTecnologiaIds, filtro.nombre, filtro.getAll, filtro.aplicaIds, filtro.compatibilidadSOIds, filtro.compatibilidadCloudIds);
            var fileName = string.Format("ListaTecnologíasEstandar_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("MigrarEquivalenciasTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetMigrarEquivalenciasTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.MigrarEquivalenciasTecnologia(TecnologiaEmisorId, TecnologiaReceptorId, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("MigrarDataTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetMigrarInfoTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.MigrarDataTecnologia(TecnologiaEmisorId, TecnologiaReceptorId, Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExportarTecnologiasXAplicacionByCodigoAPT")]
        [HttpGet]
        public IHttpActionResult ExportarTecnologiasXAplicacionByCodigoAPT(string codigoAPT, string sortName, string sortOrder)
        {
            string fileName = "";
            var data = new ExportarData().ExportarTecnologiasByAplicacion(codigoAPT, sortName, sortOrder);
            fileName = string.Format("ListaTecnologíasXAplicación_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = fileName });
        }

        [Route("Exportar/Actualizar")]
        [HttpGet]
        public IHttpActionResult GetExportarGestionTecnologiasActualizar()
        {
            string filename = "";

            var data = new ExportarData().ExportarGestionTecnologiaActualizar();
            filename = "ListadoTecnologia";
            filename = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ActualizarTecnologias")]
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
                    var user = TokenGenerator.GetCurrentUser();

                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    var usuario = user.Matricula;
                    EstadoCargaMasiva estadoCM = null;
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);

                    estadoCM = new CargaData().CargaMasivaTecnologias(extension, inputStream, usuario);
                    response = Request.CreateResponse(HttpStatusCode.OK, estadoCM);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        [Route("EvolucionEquipo/Instalaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetReporteEvolucionInstalacionTecnologias(PaginaEvolucionInstalaciones paramsDatos)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ReporteDAO>.Provider.ReporteEvolucionInstalacionTecnologias(paramsDatos.TipoEquipoToString, paramsDatos.SubsidariasToString, paramsDatos.Fecha, paramsDatos.NroMeses, paramsDatos.FlagAgruparFamilia, paramsDatos.IdTecnologia, paramsDatos.Fabricante, paramsDatos.NombreTecnologia);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }


        [Route("EvolucionEquipo/Instalaciones/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFiltrosEvolucionInstalacionEquipos()
        {
            HttpResponseMessage response = null;
            FiltrosIndicadoresGerencialEquipo dataRpta = new FiltrosIndicadoresGerencialEquipo();

            dataRpta = ServiceManager<EquipoDAO>.Provider.ListFiltrosEvolucionInstalacionEquipos();


            var nroMeses = Settings.Get<string>("Indicadores.Equipos.NroMeses");
            var arrMeses = nroMeses.Split('|');

            var listaMeses = new List<CustomAutocomplete>();
            foreach (var nroMes in arrMeses)
            {

                listaMeses.Add(new CustomAutocomplete { Id = nroMes, Descripcion = string.Format("{0} {1}", nroMes, nroMes == "1" ? "mes" : "meses") });
            }
            dataRpta.ListaPeriodoTiempo = listaMeses;

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        // POST: api/Tecnologia
        [Route("New")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostNewTecnologia(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioCreacion = user.Matricula;
            tecDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.AddOrEditNewTecnologia(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        // POST: api/Tecnologia/ListadoNew
        [Route("ListadoNew")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListNewTec(PaginacionNewTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetNewTec(pag.prodId,
                                                                          pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.nombre,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          //pag.equipo,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Tipo = HttpUtility.HtmlEncode(x.Tipo);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.Versiones = HttpUtility.HtmlEncode(x.Versiones);
                x.UrlConfluence = HttpUtility.HtmlEncode(x.UrlConfluence);
                x.RevisionSeguridadDescripcion = HttpUtility.HtmlEncode(x.RevisionSeguridadDescripcion);
                x.ComentariosFechaFin = HttpUtility.HtmlEncode(x.ComentariosFechaFin);
                x.CasoUso = HttpUtility.HtmlEncode(x.CasoUso);
                x.Requisitos = HttpUtility.HtmlEncode(x.Requisitos);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.TipoProductoStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
                x.ProductoNombre = HttpUtility.HtmlEncode(x.ProductoNombre);
            });

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarConsolidadoNew")]
        [HttpPost]
        public IHttpActionResult GetExportarConsolidadoNewTec(FiltroTecnologiaDTO filtro)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarConsolidadoNewTecnologia(filtro.nombre, filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.aplica, filtro.codigo, filtro.dueno, filtro.tipoTecIds, filtro.estObsIds, filtro.sortName, filtro.sortOrder);
            nomArchivo = string.Format("ListaTecnologiaConsolidado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarDetalladoNew")]
        [HttpPost]
        public IHttpActionResult GetExportarDetalladoNewTec(FiltroTecnologiaDTO filtro)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarDetalladoNewTecnologia(filtro.nombre, filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.aplica, filtro.codigo, filtro.dueno, filtro.tipoTecIds, filtro.estObsIds, filtro.sortName, filtro.sortOrder);
            nomArchivo = string.Format("ListaTecnologiaDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        // POST: api/Tecnologia/ListadoNew
        [Route("ListadoCatalogo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListCatalogoTec(PaginacionNewTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetNewTecSP(pag.prodId,
                                                                          pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.nombre,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          //pag.equipo,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          false,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows,
                                                                          pag.tribuCoeStr,
                                                                          pag.squadStr);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Versiones = HttpUtility.HtmlEncode(x.Versiones);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.ProductoNombre = HttpUtility.HtmlEncode(x.ProductoNombre);
                x.Tipo = HttpUtility.HtmlEncode(x.Tipo);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.UrlConfluence = HttpUtility.HtmlEncode(x.UrlConfluence);
                x.RevisionSeguridadDescripcion = HttpUtility.HtmlEncode(x.RevisionSeguridadDescripcion);
                x.ComentariosFechaFin = HttpUtility.HtmlEncode(x.ComentariosFechaFin);
                x.CasoUso = HttpUtility.HtmlEncode(x.CasoUso);
                x.Requisitos = HttpUtility.HtmlEncode(x.Requisitos);
                x.TipoProductoStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
            });

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCambioBajasOwnerTecnologia")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ListarCambioBajasOwnerTecnologia(PaginacionNewTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetBajasOwnerTecSP(pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.prodId,
                                                                          pag.codigo,
                                                                          pag.resolucionCambio,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows,
                                                                          pag.tribuCoeStr,
                                                                          pag.squadStr);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
            });

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarDetalladoCatalogo")]
        [HttpPost]
        public IHttpActionResult GetExportarDetalladoCatalogoTec(FiltroTecnologiaDTO filtro)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarDetalladoCatalogoTecnologia(filtro.nombre, filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.aplica, filtro.codigo, filtro.dueno, filtro.tipoTecIds, filtro.estObsIds, filtro.sortName, filtro.sortOrder, filtro.tribuCoe, filtro.squad);
            nomArchivo = string.Format("ListaTecnologiaDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = Convert.ToBase64String(data, 0, data.Length), name = nomArchivo });
        }

        [Route("ExportarCambioBajasOwnerTec")]
        [HttpPost]
        public IHttpActionResult GetExportarCambioBajasOwnerTec(FiltroTecnologiaDTO filtro)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarCambioBajasOwnerTecnologia(filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.codigo,filtro.resolucionCambio, filtro.sortName, filtro.sortOrder, filtro.tribuCoe, filtro.squad);
            nomArchivo = string.Format("ListaProductosCambioBajaSquad_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = Convert.ToBase64String(data, 0, data.Length), name = nomArchivo });
        }

        // GET: api/Tecnologia/5
        [Route("New/{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetNewTecById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetNewTecById(id, withAutorizadores, withArquetipos, withAplicaciones, withEquivalencias);
            if (objTec == null)
                return NotFound();

            return Ok(objTec);
        }

        #region Tecnologia Por Producto
        [Route("ListadoByProducto")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListTecnologiaByProducto(int productoId)
        {
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaByProducto(productoId);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.TipoTecnologiaStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.SubdominioNomb = HttpUtility.HtmlEncode(x.SubdominioNomb);
                x.DominioNomb = HttpUtility.HtmlEncode(x.DominioNomb);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.TipoProductoStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
            });

            return Ok(registros);
        }

        [Route("Listado/TecnologiasByProducto")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTecnologiasByProducto(PaginacionNewTec pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<TecnologiaDAO>.Provider.PostListTecnologiasByProducto(pag, out totalRows);

            if (registros == null)
                return NotFound();


            registros.ForEach(x =>
            {
                x.Tipo = HttpUtility.HtmlEncode(x.Tipo);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.TipoProductoStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
            });

            var reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListadoByProductoWithPagination")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTecnologiaByProducto(Paginacion pag)
        {
            int totalRows;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaByProductoWithPagination(pag.productoId,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/Tecnologia
        [Route("FromProducto")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostTecnologiaFromProducto(TecnologiaDTO tecDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EditTecnologiaFromProducto(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("Eliminar/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult DeleteProductoTecnologiaById(int id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string userName = user.Matricula;

            bool isDelete = ServiceManager<TecnologiaDAO>.Provider.DeleteTecnologiaById(id, userName);

            return Ok(isDelete);
        }

        #endregion

        #region Producto Tecnología Aplicación
        [Route("TecnologiaAplicacion/Listado")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListAplicacionesTecnologia(int tecnologiaId)
        {
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaAplicaciones(tecnologiaId);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("TecnologiaAplicacion/ExportarListado")]
        [HttpGet]
        public IHttpActionResult PostExportarListAplicacionesTecnologia(int tecnologiaId, string tecnologiaStr, string filtro)
        {
            string nomArchivo = string.Format("ListaAplicacionesXTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListAplicacionesTecnologia(tecnologiaId, tecnologiaStr, filtro);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("TecnologiaAplicacion/Eliminar/{id:int}/{userName}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult DeleteProductoTecnologiaAplicacionById(int id, string userName)
        {
            bool isDelete = ServiceManager<TecnologiaDAO>.Provider.DeleteTecnologiaAplicacionById(id, userName);

            return Ok(isDelete);
        }

        [Route("TecnologiaAplicacion/GuardarMasivo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GuardarMasivoProductoTecnologiaAplicacion(TecnologiaDTO productoTecnologiaDTO)
        {
            productoTecnologiaDTO.ItemsRemoveAppId = productoTecnologiaDTO.ItemsRemoveAppId ?? new List<int>();

            bool isDelete = ServiceManager<TecnologiaDAO>.Provider.GuardarMasivoTecnologiaAplicacion(productoTecnologiaDTO.ListAplicaciones, productoTecnologiaDTO.ItemsRemoveAppId.ToArray(), productoTecnologiaDTO.UsuarioModificacion);

            return Ok(isDelete);
        }
        #endregion

        #region Motivo
        [Route("ExisteMotivoRelacionado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostExisteMotivoRelacionado(int motivoId)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<TecnologiaDAO>.Provider.ExisteTecnologiaAsociadaAlMotivo(motivoId);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
        #endregion

        #region Tecnología Owner
        [Route("TecnologiaOwner/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosTecnologiaOwner()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);

            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListadoXOwner")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListTecnologiaOwner(string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, bool? flagTribuCoe, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            int perfilId = user.PerfilId;
            string correo = user.CorreoElectronico;

            var registros = ServiceManager<TecnologiaDAO>.Provider.BuscarTecnologiaXOwner(correo, perfilId, dominioIds, subDominioIds, productoStr, tribuCoeId, squadId, flagTribuCoe, pageNumber, pageSize, sortName, sortOrder, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.TipoTecnologia = HttpUtility.HtmlEncode(x.TipoTecnologia);
                x.ProductoStr = HttpUtility.HtmlEncode(x.ProductoStr);
            });

            var reader = new BootstrapTable<TecnologiaOwnerDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarListadoXOwner")]
        [HttpGet]
        public IHttpActionResult PostExportarListTecnologiaOwner(string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, bool? flagTribuCoe, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;
            string nomArchivo = string.Format("ListaTecnologiasPorOwner_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListTecnologiaOwner(correo, perfilId, dominioIds, subDominioIds, productoStr, tribuCoeId, squadId, flagTribuCoe, sortName, sortOrder);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListadoXOwnerProducto")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListTecnologiaOwnerProducto(int productoId)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;

            var registros = ServiceManager<TecnologiaDAO>.Provider.BuscarTecnologiaXOwnerProducto(correo, perfilId, productoId);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("ExportarListadoXOwnerProducto")]
        [HttpGet]
        public IHttpActionResult PostExportarListadoXOwnerProducto(int productoId, string productoStr, string filtro)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;

            string nomArchivo = string.Format("ListaTecnologias_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListadoXOwnerProducto(correo, perfilId, productoId, productoStr, filtro);

            return Ok(new { excel = data, name = nomArchivo });
        }
        #endregion

        #region Tecnología Owner Consolidado
        [Route("TecnologiaOwnerConsolidadoObsolescencia/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosTecnologiaOwnerConsolidadoObsolescencia()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listEstado = Utilitarios.EnumToList<ETecnologiaEstado>().Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var listTipoEquipo = ServiceManager<TecnologiaDAO>.Provider.GetAllTipoEquipoyFiltro(null);
            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
                Estado = listEstado,
                TipoEquipo = listTipoEquipo
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListadoConsolidadoObsolescenciaXNivel")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetListarTecnologiaOwnerConsolidadoObsolescencia(FiltroTecnologiaObsolescenciaDTO filtro)
        {
            filtro.dominioIds = filtro.dominioIds == "" ? null : filtro.dominioIds;
            filtro.subDominioIds = filtro.subDominioIds == "" ? null : filtro.subDominioIds;
            filtro.productoStr = filtro.productoStr == "" ? null : filtro.productoStr;
            filtro.tecnologiaStr = filtro.tecnologiaStr == "" ? null : filtro.tecnologiaStr;
            filtro.ownerStr = filtro.ownerStr == "" ? null : filtro.ownerStr;
            filtro.squadId = filtro.squadId == "" ? null : filtro.squadId;
            filtro.ownerParentIds = filtro.ownerParentIds == "" ? null : filtro.ownerParentIds;
            filtro.tipoEquipoIds = filtro.tipoEquipoIds == "" ? null : filtro.tipoEquipoIds;

            var registros = ServiceManager<TecnologiaDAO>.Provider.ListarTecnologiaOwnerConsolidadoObsolescencia(filtro.dominioIds, filtro.subDominioIds, filtro.productoStr, filtro.tecnologiaStr, filtro.ownerStr, filtro.squadId, filtro.nivel, filtro.ownerParentIds, filtro.tipoEquipoIds, filtro.fechaFiltro);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Owner = HttpUtility.HtmlEncode(x.Owner); });

            return Ok(registros);
        }

        [Route("ExportarListadoConsolidadoObsolescencia")]
        [HttpPost]
        public IHttpActionResult GetExportarListarTecnologiaOwnerConsolidadoObsolescencia(FiltroTecnologiaObsolescenciaDTO filtro)
        {
            string nomArchivo = string.Format("ListaTecnologiasPorOwnerConsolidadoObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListarTecnologiaOwnerConsolidadoObsolescencia(filtro.dominioIds, filtro.subDominioIds, filtro.productoStr, filtro.tecnologiaStr, filtro.ownerStr, filtro.squadId, filtro.tipoEquipoIds, filtro.fechaFiltro);

            return Ok(new { excel = data, name = nomArchivo });
        }
        #endregion

        #region Tecnología SoportadoPor Consolidado
        [Route("TecnologiaSoportadoPorConsolidadoObsolescencia/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosTecnologiaSoportadoPorConsolidadoObsolescencia()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listUnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();

            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
                UnidadFondeo = listUnidadFondeo
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListadoConsolidadoSoportadoPorObsolescenciaXNivel")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetListarTecnologiaSoportadoPorConsolidadoObsolescencia(string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int nivel, string soportadoPorParents = null)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correoOwner = user.Matricula;
            int perfilId = user.PerfilId;
            var validaGestorUnit = user.Perfil.IndexOf("E195_GestorUnit");
            if (validaGestorUnit > -1)
            {
                user.PerfilId = 20;
            }

            var registros = ServiceManager<TecnologiaDAO>.Provider.ListarTecnologiaSoportadoPorConsolidadoObsolescencia(correoOwner, perfilId, dominioIds, subDominioIds, aplicacionStr, gestionadoPor, nivel, soportadoPorParents);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("ListadoConsolidadoSoportadoPorObsolescenciaXNivelUdF")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetListarTecnologiaSoportadoPorConsolidadoObsolescenciaUdF(string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int nivel, string unidadFondeo, bool flagProyeccion, string fechaProyeccion, string soportadoPorParents = null)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correoOwner = user.Matricula;
            int perfilId = user.PerfilId;
            var validaGestorUnit = user.Perfil.IndexOf("E195_GestorUnit");
            if (validaGestorUnit > -1)
            {
                user.PerfilId = 20;
            }

            var registros = ServiceManager<TecnologiaDAO>.Provider.ListarTecnologiaSoportadoPorConsolidadoObsolescenciaUdF(correoOwner, perfilId, dominioIds, subDominioIds, aplicacionStr, gestionadoPor, nivel, soportadoPorParents, unidadFondeo, flagProyeccion, fechaProyeccion);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.UnidadFondeo = HttpUtility.HtmlEncode(x.UnidadFondeo); });

            return Ok(registros);
        }

        [Route("ExportarListadoConsolidadoSoportadoPorObsolescencia")]
        [HttpGet]
        public IHttpActionResult GetExportarListarTecnologiaSoportadoPorConsolidadoObsolescencia(string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correoOwner = user.Matricula;
            int perfilId = user.PerfilId;
            string nomArchivo = string.Format("ListaTecnologiasPorSoportadoPorConsolidadoObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListarTecnologiaSoportadoPorConsolidadoObsolescencia(correoOwner, perfilId, dominioIds, subDominioIds, aplicacionStr, gestionadoPor);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarListadoConsolidadoObsolescenciaUdF")]
        [HttpGet]
        public IHttpActionResult GetExportarListarTecnologiaUdfConsolidadoObsolescencia(string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, string unidadFondeo, bool flagProyeccion, string fechaProyeccion)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correoOwner = user.Matricula;
            int perfilId = user.PerfilId;
            string nomArchivo = string.Format("ListaTecnologiasUdfConsolidadoObsolescencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListarTecnologiaUdfConsolidadoObsolescencia(correoOwner, perfilId, dominioIds, subDominioIds, aplicacionStr, gestionadoPor, unidadFondeo, flagProyeccion, fechaProyeccion);

            return Ok(new { excel = data, name = nomArchivo });
        }
        #endregion

        #region Roles por Producto
        [Route("DetalleProductosRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleProductosRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetProductoRolesDetalleCatalogo(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<RolesProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("DetalleProductosFunciones")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleProductosFunciones(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetProductoFuncionesDetalleCatalogo(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        #endregion

        #region Flujo de Aprobación de Tecnologías

        [Route("DatosTecnologia/{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDatosecnologiaById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(id, withAutorizadores, withArquetipos, withAplicaciones, withEquivalencias);
            if (objTec == null)
                return NotFound();

            return Ok(objTec);
        }

        [Route("TecnologiaId/{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDatosTecnologiaId(int id)
        {
            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaId(id);
            if (objTec == null)
                return NotFound();

            return Ok(objTec);
        }

        [Route("DatosProducto/{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDatosProductoById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            var objeto = ServiceManager<TecnologiaDAO>.Provider.GetDatosProductoById(id, withAutorizadores, withArquetipos, withAplicaciones, withEquivalencias);
            if (objeto == null)
                return NotFound();

            return Ok(objeto);
        }

        [Route("GetTribuCoeResponsable")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTribuResponsableId(string codigo)
        {
            //var objProd = ServiceManager<TecnologiaDAO>.Provider.GetDatosResponsablePorId(codigo, 1);
            //if (objProd == null)
            //    return NotFound();

            //return Ok(objProd);

            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetDatosResponsablePorId(codigo, 1);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }


        [Route("GetArquitectoSeguridad")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetArquitectoSeguridad(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<TecnologiaDAO>.Provider.GetDatosResponsablePorId(filtro, 3);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        //[Route("Equipo/GetEquipoByFiltro")]
        //[HttpGet]
        //[Authorize]
        //public HttpResponseMessage GetEquipoByFiltro(string filtro, int idTipoEquipo)
        //{
        //    HttpResponseMessage response = null;
        //    var data = ServiceManager<EquipoDAO>.Provider.GetEquipoByFiltro(filtro, idTipoEquipo);
        //    response = Request.CreateResponse(HttpStatusCode.OK, data);
        //    return response;
        //}

        [Route("ObtenerCodigoSugerido")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ObtenerCodigoSugerido()
        {
            HttpResponseMessage response = null;
            bool estado = true;
            string data = null;

            do
            {
                data = ServiceManager<ProductoDAO>.Provider.ObtenerCodigoSugerido();
                estado = ServiceManager<ApplicationDAO>.Provider.ExisteCodigoAPT(data);

            } while (estado);

            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("EnviarSolicitudAprobacionTecnologia")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostEnviarSolicitudAprobacionTecnologia(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioMatricula = user.Matricula;
            tecDTO.UsuarioNombre = user.Nombres;
            tecDTO.UsuarioMail = user.CorreoElectronico;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EnviarSolicitudAprobacionTecnologia(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("EnviarSolicitudAprobacionProducto")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostEnviarSolicitudAprobacionProducto(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioMatricula = user.Matricula;
            tecDTO.UsuarioNombre = user.Nombres;
            tecDTO.UsuarioMail = user.CorreoElectronico;
            tecDTO.UsuarioCreacion = user.Matricula;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EnviarSolicitudAprobacionProducto(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("EnviarSolicitudAprobacionEquivalencia")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostEnviarSolicitudAprobacionEquivalencia(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioMatricula = user.Matricula;
            tecDTO.UsuarioNombre = user.Nombres;
            tecDTO.UsuarioMail = user.CorreoElectronico;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EnviarSolicitudAprobacionEquivalencia(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("EnviarSolicitudAprobacionDesactivacion")]
        [HttpPost]
        [ResponseType(typeof(TecnologiaDTO))]
        [Authorize]
        public IHttpActionResult PostEnviarSolicitudAprobacionDesactivacion(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioMatricula = user.Matricula;
            tecDTO.UsuarioNombre = user.Nombres;
            tecDTO.UsuarioMail = user.CorreoElectronico;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EnviarSolicitudAprobacionDesactivacion(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("VerificarDiferenciaDeDatos")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetVerificarDiferenciaDeDatos(TecnologiaDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioMatricula = user.Matricula;
            tecDTO.UsuarioNombre = user.Nombres;
            tecDTO.UsuarioMail = user.CorreoElectronico;

            var objRol = ServiceManager<TecnologiaDAO>.Provider.VerificarDiferenciaDeDatos(tecDTO);
            return Ok(objRol);
        }

        [Route("FlujoAprobacion/SolicitudesFlujo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage SolicitudesFlujo(PaginacionSolicitud objDTO)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;

            if (objDTO.FechaRegistroSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaRegistroSolicitud2 = null;
            }
            else objDTO.FechaRegistroSolicitud2 = DateTime.Today;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetFlujosSolicitudes(objDTO, out totalRows);

            var reader = new BootstrapTable<SolicitudFlujoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }
        [Route("FlujoAprobacion/BandejaSolicitud")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetBandejaSolicitudes(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Matricula = user.Matricula;
            objDTO.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var totalRows = 0;
            if (objDTO.FechaRegistroSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaRegistroSolicitud2 = null;
            }
            else objDTO.FechaRegistroSolicitud2 = DateTime.Today;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetBandejaSolicitudes(objDTO, out totalRows);

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.SubDominio = HttpUtility.HtmlEncode(x.SubDominio);
                x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
            });

            var reader = new BootstrapTable<SolicitudFlujoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        // POST: api/Tecnologia
        [Route("FlujoAprobacion/AprobarActualizarDatos")]
        [HttpPost]
        [ResponseType(typeof(FlujoActualizacionTecnologiaCamposDTO))]
        [Authorize]
        public IHttpActionResult PostActualizarDatos(FlujoActualizacionTecnologiaCamposDTO tecDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tecDTO.UsuarioModificacion = user.Matricula;
            tecDTO.UsuarioMail = user.CorreoElectronico;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTec = ServiceManager<TecnologiaDAO>.Provider.EditNewTecnologia(tecDTO);

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("FlujoAprobacion/ObservarSolicitud")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObservarSolicitud(PaginacionSolicitudFlujos pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.UsuarioMail = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<TecnologiaDAO>.Provider.ObservarSolicitud(pag.IdSolicitud, pag.Matricula, pag.Comentario, pag.IdTecnologia, pag.productoId, pag.UsuarioMail);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("FlujoAprobacion/DetalleSolicitudes")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostDetalleSolicitudFlujo(PaginacionSolicitud pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetDetalleSolicitudFlujo(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ValorAnterior = HttpUtility.HtmlEncode(x.ValorAnterior);
                x.ValorNuevo = HttpUtility.HtmlEncode(x.ValorNuevo);
            });

            var reader = new BootstrapTable<SolicitudFlujoDetalleDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarTecnologiasEquivalentesSolicitud")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetTecEquivalentesPropuestaByTec(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecEqByTec(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaEquivalenciaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarTecnologiasEquivalentesPropuesta/{id:int}")]
        [ResponseType(typeof(TecnologiaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTecEquivalentesPropuestaByTec(int id, bool withEquivalencias = false)
        {
            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetEquivalenciasPropuestaById(id, withEquivalencias);
            if (objTec == null)
                return NotFound();

            return Ok(objTec);
        }

        [Route("ExisteInstanciasTecnologia")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetInstanciasTecnologia(int Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TecnologiaDAO>.Provider.ExisteInstanciasByTecnologiaId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ReporteRestriccionesTecnologia")]
        [HttpGet]
        public IHttpActionResult getRestriccionesTecnologia(int tecnologiaId)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarRestriccionesEliminarTecnologia(tecnologiaId);
            nomArchivo = string.Format("ListaRestriccionesEliminarTecnologia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListadoTecnologiasModificar")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTecnologiasModificar(PaginacionNewTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaModificar(pag.prodId,
                                                                          pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.nombre,
                                                                          pag.codigo,
                                                                          pag.tribuCoeStr,
                                                                          pag.squadStr,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          pag.Matricula,
                                                                          pag.Perfil,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListadoProductosModificar")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListaProductosModificar(PaginacionNewTec pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.Perfil = user.Perfil;

            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetProductoModificar(pag.prodId,
                                                                          pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.nombre,
                                                                          pag.codigo,
                                                                          pag.tribuCoeStr,
                                                                          pag.squadStr,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          pag.Matricula,
                                                                          pag.Perfil,
                                                                          out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.ProductoNombre = HttpUtility.HtmlEncode(x.ProductoNombre);
            });

            dynamic reader = new BootstrapTable<ProductoList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarTecnologiaModificadaConsolidado")]
        [HttpPost]
        public IHttpActionResult GetExportarTecnologiaModificadaConsolidado(FiltroTecnologiaDTO filtro)
        {
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;
            string matricula = user.Matricula;

            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarConsolidadoTecnologiaModificada(filtro.nombre, filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.codigo, filtro.tribuCoe, filtro.squad, filtro.tipoTecIds, filtro.estObsIds, filtro.sortName, filtro.sortOrder, perfil, matricula);
            nomArchivo = string.Format("ListaTecnologiaConsolidado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = Convert.ToBase64String(data, 0, data.Length), name = nomArchivo });
        }

        [Route("ExportarTecnologiaModificadaDetallado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetExportarTecnologiaModificadaDetallado(FiltroTecnologiaDTO filtro)
        {
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;
            string matricula = user.Matricula;

            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarDetalladoTecnologiaModificada(filtro.nombre, filtro.dominioIds, filtro.subdominioIds, filtro.productoId, filtro.codigo, filtro.tribuCoe, filtro.squad, filtro.tipoTecIds, filtro.estObsIds, filtro.sortName, filtro.sortOrder, perfil, matricula);
            nomArchivo = string.Format("ListaTecnologiaDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = Convert.ToBase64String(data, 0, data.Length), name = nomArchivo });
        }

        #endregion
    }

}
