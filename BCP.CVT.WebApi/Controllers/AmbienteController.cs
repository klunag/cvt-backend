using BCP.CVT.Cross;
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
    [RoutePrefix("api/Ambiente")]
    public class AmbienteController : BaseController
    {
        // GET: api/Ambiente/5
        [Route("{id:int}")]
        [ResponseType(typeof(AmbienteDTO))]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetAmbienteById(int id)
        {
            var objAmb = ServiceManager<AmbienteDAO>.Provider.GetAmbienteById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
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
            var entidad = ServiceManager<AmbienteDAO>.Provider.GetAmbienteById(Id);
            var retorno = ServiceManager<AmbienteDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/Ambiente
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(AmbienteDTO))]
		[Authorize]
		public IHttpActionResult PostAmbiente(AmbienteDTO ambDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            ambDTO.UsuarioCreacion = user.Matricula;
            ambDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int AmbienteId = ServiceManager<AmbienteDAO>.Provider.AddOrEditAmbiente(ambDTO);

            //if (AmbienteId == 0)
            //    return NotFound();

            return Ok(AmbienteId);
        }

        // POST: api/Ambiente
        [Route("ActualizarVentana")]
        [HttpPost]
        [ResponseType(typeof(AmbienteDTO))]
		[Authorize]
		public IHttpActionResult PostVentanaMantenimiento(AmbienteDTO ambDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            ambDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int AmbienteId = ServiceManager<AmbienteDAO>.Provider.UpdateVentana(ambDTO);

            //if (AmbienteId == 0)
            //    return NotFound();

            return Ok(AmbienteId);
        }

        // POST: api/Ambiente/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAmbientes(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AmbienteDAO>.Provider.GetAmbiente(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<AmbienteDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Listado/Ventana")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListAmbientesVentana(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AmbienteDAO>.Provider.GetVentana(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<AmbienteDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExisteCodigoByFiltro")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ExisteCodigoByFiltro(int codigo, int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<AmbienteDAO>.Provider.ExisteCodigoByFiltro(codigo, id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var listDias = Utilitarios.EnumToList<EDias>();
            
            var dataRpta = new
            {
                Dias = listDias.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList()              
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
    }
}
