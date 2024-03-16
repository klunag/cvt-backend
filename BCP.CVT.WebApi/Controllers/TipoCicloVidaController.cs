using BCP.CVT.DTO;
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
    [RoutePrefix("api/TipoCicloVida")]
    public class TipoCicloVidaController : BaseController
    {
        // POST: api/TipoCicloVida/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTipoCicloVida(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVida(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<TipoCicloVidaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/TipoCicloVida
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TipoCicloVidaDTO))]
        [Authorize]
        public IHttpActionResult PostTipoCicloVida(TipoCicloVidaDTO TCVDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            TCVDTO.UsuarioCreacion = user.Matricula;
            TCVDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int Id = ServiceManager<TipoCicloVidaDAO>.Provider.AddOrEditTipoCicloVida(TCVDTO);
            return Ok(Id);
        }

        // GET: api/TipoCicloVida/{id}
        [Route("{id:int}")]
        [ResponseType(typeof(TipoCicloVidaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTipoCicloVidaById(int id)
        {
            var objTipoCiclo = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVidaById(id);
            if (objTipoCiclo == null)
                return NotFound();

            return Ok(objTipoCiclo);
        }

        // GET: api/TipoCicloVida/CambiarEstado
        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)

        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVidaById(Id);
            var retorno = ServiceManager<TipoCicloVidaDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/TipoCicloVida/CambiarDefault
        [Route("CambiarDefault")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarDefault(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<TipoCicloVidaDAO>.Provider.CambiarDefault(Id, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }
    }
}