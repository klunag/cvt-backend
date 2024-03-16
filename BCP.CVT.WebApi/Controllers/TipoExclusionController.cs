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
    [RoutePrefix("api/TipoExclusion")]
    public class TipoExclusionController : BaseController
    {
        // GET: api/Tipo/5
        [Route("{id:int}")]
        [ResponseType(typeof(TipoExclusionDTO))]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetTipoById(int id)
        {
            var objTipo = ServiceManager<TipoExclusionDAO>.Provider.GetTipoExclusionById(id);
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
            var entidad = ServiceManager<TipoExclusionDAO>.Provider.GetTipoExclusionById(Id);
            var retorno = ServiceManager<TipoExclusionDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/Tipo
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TipoExclusionDTO))]
		[Authorize]
		public IHttpActionResult PostTipo(TipoExclusionDTO objeto)
        {
            var user = TokenGenerator.GetCurrentUser();
            objeto.UsuarioCreacion = user.Matricula;
            objeto.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdTipo = ServiceManager<TipoExclusionDAO>.Provider.AddOrEditTipoExclusion(objeto);

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
            var registros = ServiceManager<TipoExclusionDAO>.Provider.GetTipoExclusion(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();
                
            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<TipoExclusionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}
