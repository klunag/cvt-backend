using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]    
    [RoutePrefix("api/Familia")]
    public class FamiliaController : BaseController
    {
        // GET: api/Familia/5
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(FamiliaDTO))]
		[Authorize]
		public IHttpActionResult GetFamiliaById(int id)
        {
            var objFamilia = ServiceManager<FamiliaDAO>.Provider.GetFamiliaById(id);
            if (objFamilia == null)
                return NotFound();

            return Ok(objFamilia);
        }

        // GET: api/Familia
        [Route("")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetAllFamilia()
        {
            var listFamilia = ServiceManager<FamiliaDAO>.Provider.GetAllFamilia();
            if (listFamilia == null)
                return NotFound();

            return Ok(listFamilia);
        }

        // POST: api/Familia
        [Route("")]
        [HttpPost]
		[Authorize]
		//[ResponseType(typeof(FamiliaDTO))]      
		public IHttpActionResult PostFamilia(FamiliaDTO familiaDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            familiaDTO.UsuarioCreacion = user.Matricula;
            familiaDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdFamilia = ServiceManager<FamiliaDAO>.Provider.AddOrEditFamilia(familiaDTO);

            if (IdFamilia == 0)
                return NotFound();

            return Ok(IdFamilia);
        }

        // POST: api/Familia/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListFamilias(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<FamiliaDAO>.Provider.GetFamilia(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<FamiliaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Familia/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<FamiliaDAO>.Provider.GetFamiliaById(Id);
            var retorno = ServiceManager<FamiliaDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ListarTecnologiasAsociadas")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostTecByFam(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<FamiliaDAO>.Provider.GetTecByFamilia(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarTecnologiasVinculadas")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostTecVinculadas(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<FamiliaDAO>.Provider.GetTecVinculadas(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
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

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportFamilias(string nombre, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarFamilia(nombre, sortName, sortOrder);
            nomArchivo = string.Format("ListaFamilia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GetAllFamiliaByFiltro")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetAllFamiliaByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listTec = ServiceManager<FamiliaDAO>.Provider.GetAllFamiliaByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listTec);
            return response;
        }

        [Route("ExisteFamilia")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetExisteFamilia(int? Id, string nombre)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<FamiliaDAO>.Provider.ExisteFamiliaById(Id, nombre);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ExisteFamiliaByNombre")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetExisteFamiliaByNombre(int? Id, string nombre)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<FamiliaDAO>.Provider.ExisteFamiliaByNombre(Id, nombre);
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
            nomArchivo = string.Format("ListaTecnologiasAsociadasFamilia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("AsociarTecnologiaByFamilia")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostAsocTecnologiasEq(FamiliaTecnologiaParam obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioModificacion = user.Matricula;
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            HttpResponseMessage response = null;
            var retorno = ServiceManager<FamiliaDAO>.Provider.AsociarTecnologiasByFamilia(obj.TecnologiaId, obj.FamiliaId, obj.UsuarioModificacion);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
    }
}
