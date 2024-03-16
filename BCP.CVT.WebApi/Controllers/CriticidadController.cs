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
    [RoutePrefix("api/Criticidad")]
    public class CriticidadController : BaseController
    {
        // GET: api/Criticidad/5
        [Route("{id:int}")]
        [ResponseType(typeof(CriticidadDTO))]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetCriticidadById(int id)
        {
            var objCrit = ServiceManager<CriticidadDAO>.Provider.GetCriticidadById(id);
            if (objCrit == null)
                return NotFound();

            return Ok(objCrit);
        }

        // GET: api/Ambiente/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<CriticidadDAO>.Provider.GetCriticidadById(Id);
            var retorno = ServiceManager<CriticidadDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/Ambiente
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CriticidadDTO))]
		[Authorize]
		public IHttpActionResult PostAmbiente(CriticidadDTO criDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            criDTO.UsuarioCreacion = user.Matricula;
            criDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int CriticidadId = ServiceManager<CriticidadDAO>.Provider.AddOrEditCriticidad(criDTO);

            //if (AmbienteId == 0)
            //    return NotFound();

            return Ok(CriticidadId);
        }

        // POST: api/Criticidad/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListCriticidad(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<CriticidadDAO>.Provider.GetCriticidad(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.DetalleCriticidad = HttpUtility.HtmlEncode(x.DetalleCriticidad);
                x.PrefijoBase = HttpUtility.HtmlEncode(x.PrefijoBase);
            });

            dynamic reader = new BootstrapTable<CriticidadDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}
