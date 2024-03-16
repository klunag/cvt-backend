using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.Services.Exportar;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Configuracion")]
    public class ConfiguracionController : BaseController
    {
        [Route("LogsProcesos/Listar")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostLogsListar(PaginaReporteLogs pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ParametroDAO>.Provider.GetProcesosLogs(pag, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ProcesosLogsDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("LogsProcesos/ExportarProcesosLogs")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportarProcesosLogsData(int procesoid, string fechaFiltro, string tipo, string capa)
        {
            string nomArchivo = "";
            if (tipo == null)
            {
                tipo = "";
            }
            if (capa == null)
            {
                capa = "";
            }
            var data = new ExportarData().ExportarProcesosLog(procesoid, fechaFiltro, tipo, capa);
            nomArchivo = string.Format("ListaLogs_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var listTipoProceso = ServiceManager<ParametroDAO>.Provider.GetTipoProceso(1);

            var dataRpta = new
            {
                TipoProceso = listTipoProceso
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        // POST: api/Configuracion/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTipos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ParametroDAO>.Provider.GetParametros(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.TipoParametro = HttpUtility.HtmlEncode(x.TipoParametro);
                x.Valor = HttpUtility.HtmlEncode(x.Valor);
            });

            var listaTmp = (from a in registros
                            select new
                            {
                                Nombre = a.TipoParametro,
                                Id = a.TipoParametroId.Value
                            }).Distinct().ToList();

            var lista = new List<TipoParametroDTO>();
            foreach (var item in listaTmp)
            {
                lista.Add(new TipoParametroDTO
                {
                    Nombre = item.Nombre,
                    Id = item.Id,
                    Parametros = (from a in registros
                                  where a.TipoParametroId == item.Id
                                  select a).ToList()

                });

            }

            if (lista == null)
                return NotFound();


            dynamic reader = new BootstrapTable<TipoParametroDTO>()
            {
                Total = lista.Count(),
                Rows = lista
            };

            return Ok(reader);
        }

        // POST: api/Configuracion
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(ParametroDTO))]
        [Authorize]
        public IHttpActionResult PostConfiguracion(ParametroDTO objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceManager<ParametroDAO>.Provider.ActualizarParametro(objDTO);

            var parametroId = objDTO.Id;

            if (parametroId == 0)
                return NotFound();

            return Ok(parametroId);
        }


        // GET: api/Configuracion/5
        [Route("{id:int}")]
        [ResponseType(typeof(ConfiguracionDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetParametroById(int id)
        {
            var objConfig = ServiceManager<ParametroDAO>.Provider.GetPrametroById(id);
            if (objConfig == null)
                return NotFound();

            return Ok(objConfig);
        }

        // GET: api/Configuracion/CambiarEstado/5
        [Route("CambiarEstado/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCambiarEstado(int id)
        {
            var entidad = ServiceManager<ParametroDAO>.Provider.GetPrametroById(id);
            //var retorno = ServiceManager<ParametroDAO>.Provider.CambiarEstado(id, !entidad.Activo, "Usuario modificacion");
            var retorno = true;

            return Ok(retorno);
        }

        // POST: api/Tecnologia
        [Route("NuevoModelo")]
        [HttpPost]
        [ResponseType(typeof(ModeloDTO))]
        [Authorize]
        public IHttpActionResult PostNewTecnologia(ModeloDTO modDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            modDTO.Usuario = user.Matricula;

            int IdTec = 0;
            if (modDTO.idmodelo == 0)
                IdTec = ServiceManager<ModeloDAO>.Provider.Agregarodelo(modDTO);
            else
            {
                IdTec = 1;
                ServiceManager<ModeloDAO>.Provider.EditarModelo(modDTO);
            }

            if (IdTec == 0)
                return NotFound();

            return Ok(IdTec);
        }

        [Route("LeerModelo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetModeloHardware(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ModeloDAO>.Provider.LeerModelo(pag.fabricante, pag.tipoId, pag.nombre, pag.nroSerie, pag.hostName, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ModeloDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("EliminarModelo")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult DeleteModeloHardware(int id, string usuario)
        {
            ServiceManager<ModeloDAO>.Provider.DeleteModelo(id, usuario);

            return Ok("");
        }

        [Route("ActualizarCodigo")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ActualizarCodigo(int id, string codigo)
        {
            ServiceManager<ModeloDAO>.Provider.ActualizarCodigo(id, codigo);

            return Ok("");
        }

        [Route("BuscarModelo")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult SearchModeloHardware(int id)
        {
            var data = ServiceManager<ModeloDAO>.Provider.BuscarModelo(id);

            return Ok(data);
        }

        [Route("ObtenerEquiposModelo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ObtenerEquiposModelo(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ModeloDAO>.Provider.ObtenerEquipos(pag.id, pag.fabricante, pag.tipoId, pag.nombre, pag.nroSerie, pag.hostName, 1, 1, string.Empty, string.Empty, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarModelo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarModeloHardware(string criterio, int tipo, string nroSerie, string hostName)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarModeloHardware(criterio, tipo, nroSerie, hostName);
            nomArchivo = string.Format("ReporteModeloHardware_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}