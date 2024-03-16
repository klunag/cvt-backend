using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.CVT.Services.Exportar;
using System.Web;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Tipo")]
    public class TipoController : BaseController
    {   
        // GET: api/Tipo/5
        [Route("{id:int}")]
        [ResponseType(typeof(TipoDTO))]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetTipoById(int id)
        {
            var objTipo = ServiceManager<TipoDAO>.Provider.GetTipoById(id);
            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        // GET: api/Tipo/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<TipoDAO>.Provider.GetTipoById(Id);
            var retorno = ServiceManager<TipoDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;     
        }

        // GET: api/Tipo
        [Route("")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetAllTipo()
        {
            var listTipo = ServiceManager<TipoDAO>.Provider.GetAllTipo();
            if (listTipo == null)
                return NotFound();

            return Ok(listTipo);
        }

        // POST: api/Tipo
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TipoDTO))]
        [Authorize]
        public IHttpActionResult PostTipo(TipoDTO tipoDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tipoDTO.UsuarioCreacion = user.Matricula;
            tipoDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTipo = ServiceManager<TipoDAO>.Provider.AddOrEditTipo(tipoDTO);

            if (IdTipo == 0)
                return NotFound();

            return Ok(IdTipo);
        }

        // POST: api/Tipo/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListTipos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TipoDAO>.Provider.GetTipo(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { 
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<TipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("TieneFlagEstandar/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage TieneFlagEstandar(int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TipoDAO>.Provider.TieneFlagEstandar();
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        // GET: api/Tipo
        [Route("ObtenerFlagEstandar/{id:int}")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetFlagEstandar(int id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<TipoDAO>.Provider.GetFlagEstandar(id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportTipos(string nombre, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarTipo(nombre, sortName, sortOrder);
            nomArchivo = string.Format("ListaTipo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}