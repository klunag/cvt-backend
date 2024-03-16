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

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Relevancia")]
    public class RelevanciaController : BaseController
    {
        [Route("{id:int}")]
        [ResponseType(typeof(RelevanciaDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRelevanciaById(int id)
        {
            var objEntidad = ServiceManager<RelevanciaDAO>.Provider.GetRelevanciaById(id);
            if (objEntidad == null)
                return NotFound();

            return Ok(objEntidad);
        }

        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetCambiarEstado(int id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string usuario = user.Matricula;
            bool estado = ServiceManager<RelevanciaDAO>.Provider.EditRelevanciaFlagActivo(id,usuario);
            return Ok(estado);
        }
        
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(RelevanciaDTO))]
		[Authorize]
		public IHttpActionResult PostRelevancia(RelevanciaDTO objRegistro)
        {
            var user = TokenGenerator.GetCurrentUser();
            objRegistro.UsuarioCreacion = user.Matricula;
            objRegistro.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int RelevanciaId = ServiceManager<RelevanciaDAO>.Provider.AddOrEditRelevancia(objRegistro);
            
            return Ok(RelevanciaId);
        }
        
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListRelevancia(Paginacion pag)
        {
            int totalRows = 0;
            var registros = ServiceManager<RelevanciaDAO>.Provider.GetRelevancia(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<RelevanciaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}
