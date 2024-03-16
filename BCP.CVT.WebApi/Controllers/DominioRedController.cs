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
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/DominioRed")]
    public class DominioRedController : BaseController
    {
        // GET: api/DominioRed/5
        [Route("{id:int}")]
        [ResponseType(typeof(DominioServidorDTO))]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetDominioRedById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<DominioRedDAO>.Provider.GetDominioRedById(id);
            if (entidad == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        // GET: api/DominioRed/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<DominioRedDAO>.Provider.GetDominioRedById(Id);
            var retorno = ServiceManager<DominioRedDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/DominioRed
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DominioServidorDTO))]
		[Authorize]
		public HttpResponseMessage PostDominioRed(DominioServidorDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<DominioRedDAO>.Provider.AddOrEditDominioRed(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        // POST: api/DominioRed/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListDominioRed(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<DominioRedDAO>.Provider.GetDominioRed(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Equivalencias = HttpUtility.HtmlEncode(x.Equivalencias);
            });

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DominioServidorDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/DominioRed/Listado
        [Route("Listado/Activo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDominioRedActivo(Paginacion pag)
        {
            var registros = ServiceManager<DominioRedDAO>.Provider.GetDominioRedActivos();

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<CustomAutocomplete>()
            {
                Total = registros.Count,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}
