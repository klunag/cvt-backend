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
    [RoutePrefix("api/TipoArquetipo")]
    public class TipoArquetipoController : BaseController
    {
        // GET: api/TipoArquetipo/5
        [Route("{id:int}")]
        [ResponseType(typeof(TipoArquetipoDTO))]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetEntornoById(int id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<TipoArquetipoDAO>.Provider.GetTipoArquetipoById(id);
            if (entidad != null)
                response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        // GET: api/TipoArquetipo/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<TipoArquetipoDAO>.Provider.GetTipoArquetipoById(Id);
            var retorno = ServiceManager<TipoArquetipoDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/TipoArquetipo
        [Route("")]
        [HttpPost]
		[Authorize]
		//[ResponseType(typeof(EntornoDTO))]
		public HttpResponseMessage PostTipo(TipoArquetipoDTO tipoDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            tipoDTO.UsuarioCreacion = user.Matricula;
            tipoDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            int EntornoId = ServiceManager<TipoArquetipoDAO>.Provider.AddOrEditTipoArquetipo(tipoDTO);

            if (EntornoId == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }

            response = Request.CreateResponse(HttpStatusCode.OK, EntornoId);
            return response;
        }

        // POST: api/TipoArquetipo/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListTipos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TipoArquetipoDAO>.Provider.GetTipoArquetipo(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<TipoArquetipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}
