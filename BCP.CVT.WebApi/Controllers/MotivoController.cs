using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Motivo")]
    public class MotivoController : BaseController
    {
        // GET: api/Motivo/5
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(MotivoDTO))]
        [Authorize]
        public IHttpActionResult GetMotivoById(int id)
        {
            var objMotivo = ServiceManager<MotivoDAO>.Provider.GetMotivoById(id);
            if (objMotivo == null)
                return NotFound();

            return Ok(objMotivo);
        }

        // GET: api/Motivo
        [Route("")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllMotivo()
        {
            var listMotivo = ServiceManager<MotivoDAO>.Provider.GetAllMotivo();
            if (listMotivo == null)
                return NotFound();

            return Ok(listMotivo);
        }

        // POST: api/Motivo
        [Route("")]
        [HttpPost]
        [Authorize]
        //[ResponseType(typeof(MotivoDTO))]      
        public IHttpActionResult PostMotivo(MotivoDTO familiaDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            familiaDTO.UsuarioCreacion = user.Matricula;
            familiaDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdMotivo = ServiceManager<MotivoDAO>.Provider.AddOrEditMotivo(familiaDTO);

            if (IdMotivo == 0)
                return NotFound();

            return Ok(IdMotivo);
        }

        // POST: api/Motivo/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListMotivos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<MotivoDAO>.Provider.GetMotivo(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<MotivoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Motivo/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<MotivoDAO>.Provider.GetMotivoById(Id);
            var retorno = ServiceManager<MotivoDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ListarValoresPorDefecto")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarValoresPorDefecto()
        {
            HttpResponseMessage response = null;

            var dataRpta = new
            {
                ValDefectoFamExistencia = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FAMILIA_EXISTENCIA").Valor,
                ValDefectoFamFacilidad = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FAMILIA_FACILIDAD").Valor,
                ValDefectoFamRiesgo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FAMILIA_RIESGO").Valor,
                ValDefectoFamVulnerabilidad = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FAMILIA_VULNERABILIDAD").Valor
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetAllMotivoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllMotivoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listTec = ServiceManager<MotivoDAO>.Provider.GetAllMotivoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listTec);
            return response;
        }

        [Route("ExisteMotivo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteMotivo(int? Id, string nombre)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<MotivoDAO>.Provider.ExisteMotivoById(Id, nombre);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteMotivoByNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExisteMotivoByNombre(int? Id, string nombre)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<MotivoDAO>.Provider.ExisteMotivoByNombre(Id, nombre);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExportarTecnologiasAsociadas")]
        [HttpGet]
        public IHttpActionResult PostExportTecnologiasAsociadas(int id, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            //if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarTecAsocAFam(id, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologiasAsociadasMotivo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}
