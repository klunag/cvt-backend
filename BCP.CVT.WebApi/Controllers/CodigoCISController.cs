using BCP.CVT.DTO;
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
    [RoutePrefix("api/CodigoCIS")]
    public class CodigoCISController : BaseController
    {
        // POST: api/CodigoCIS/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListCodigoCIS(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<CodigoCISDAO>.Provider.GetCodigoCIS(pag.username, pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion); });

            dynamic reader = new BootstrapTable<CodigoCISDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/CodigoCIS
        [Route("")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostCodigoCIS(CodigoCISDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var CodId = ServiceManager<CodigoCISDAO>.Provider.AddOrEditCodigoCIS(request);
            response = Request.CreateResponse(HttpStatusCode.OK, CodId);
            return response; 
        }

        // GET: api/CodigoCIS/5
        [Route("{id:int}")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetCodigoCISById(int id)
        {
            var objCodCis = ServiceManager<CodigoCISDAO>.Provider.GetCodigoCISById(id);
            if (objCodCis == null)
                return NotFound();

            return Ok(objCodCis);
        }

        // GET: api/CodigoCIS/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<CodigoCISDAO>.Provider.GetCodigoCISById(Id);
            var retorno = ServiceManager<CodigoCISDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ObtenerPlantillaCIS")]
        [HttpGet]
        public IHttpActionResult PostPlantillaCodCis()
        {
            string nomArchivo = "PlantillaCargaServidores.xlsx";
            var data = new ExportarData().ObtenerPlantillaCIS();
            //nomArchivo = string.Format("PlantillaCargaServidores_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        // POST: api/CodigoCIS/ListarServidoresNoRegistrados
        [Route("ListarServidoresNoRegistrados")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostServidoresNoRegistrados(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<CodigoCISDAO>.Provider.GetServidoresNoRegistrados(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ServidorCISDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        // POST: api/CodigoCIS/ListarServidoresRelacionados
        [Route("ListarServidoresRelacionados")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostServidoresRelacionados(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<CodigoCISDAO>.Provider.GetServidoresCIS(pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<CodigoCISDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

    }
}
