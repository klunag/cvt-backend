using BCP.CVT.Cross;
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
	[RoutePrefix("api/Parametricas")]
	public class ParametricasController : BaseController
	{
		// GET: api/Parametricas/5
		[Route("{id:int}")]
		[ResponseType(typeof(ParametricasDTO))]
		[HttpGet]
        [Authorize]
        public IHttpActionResult GetParametricasById(int id)
		{
			var objPar = ServiceManager<ParametricasDAO>.Provider.GetParametricasById(id);
			if (objPar == null)
				return NotFound();

			return Ok(objPar);
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
			var entidad = ServiceManager<ParametricasDAO>.Provider.GetParametricasById(Id);
			var retorno = ServiceManager<ParametricasDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

			response = Request.CreateResponse(HttpStatusCode.OK, retorno);
			return response;
		}

		// POST: api/Parametricas
		[Route("")]
		[HttpPost]
		[ResponseType(typeof(ParametricasDTO))]
        [Authorize]
        public IHttpActionResult PostParametricas(ParametricasDTO parDTO)
		{
			var user = TokenGenerator.GetCurrentUser();
			parDTO.UsuarioCreacion = user.Matricula;
			parDTO.UsuarioModificacion = user.Matricula;
			//if (!ModelState.IsValid)
			//	return BadRequest(ModelState);

			int ParametricaDetalleId = ServiceManager<ParametricasDAO>.Provider.AddOrEditParametricas(parDTO);

			//if (ParametricaDetalleId == 0)
			//	return NotFound();

			return Ok(ParametricaDetalleId);
		}

		//// POST: api/Ambiente
		//[Route("ActualizarVentana")]
		//[HttpPost]
		//[ResponseType(typeof(ParametricasDTO))]
		//public IHttpActionResult PostVentanaMantenimiento(ParametricasDTO parDTO)
		//{
		//	if (!ModelState.IsValid)
		//		return BadRequest(ModelState);

		//	int AmbienteId = ServiceManager<ParametricasDAO>.Provider.UpdateVentana(parDTO);

		//	//if (AmbienteId == 0)
		//	//    return NotFound();

		//	return Ok(AmbienteId);
		//}

		// POST: api/Ambiente/Listado
		[Route("Listado")]
		[HttpPost]
        [Authorize]
        public IHttpActionResult PostListParametricas(Paginacion pag)
		{
			var totalRows = 0;
			var registros = ServiceManager<ParametricasDAO>.Provider.GetParametricas(pag, out totalRows);

			if (registros == null)
				return NotFound();

            registros.ForEach(x => { x.Valor = HttpUtility.HtmlEncode(x.Valor); });

            dynamic reader = new BootstrapTable<ParametricasDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

			return Ok(reader);
		}

		//[Route("Listado/Ventana")]
		//[HttpPost]
		//public IHttpActionResult PostListParametricasVentana(Paginacion pag)
		//{
		//	var totalRows = 0;
		//	var registros = ServiceManager<AmbienteDAO>.Provider.GetVentana(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

		//	if (registros == null)
		//		return NotFound();

		//	dynamic reader = new BootstrapTable<ParametricasDTO>()
		//	{
		//		Total = totalRows,
		//		Rows = registros
		//	};

		//	return Ok(reader);
		//}

		//[Route("ExisteCodigoByFiltro")]
		//[HttpGet]
		//public HttpResponseMessage ExisteCodigoByFiltro(int codigo, int id)
		//{
		//	HttpResponseMessage response = null;
		//	var data = ServiceManager<AmbienteDAO>.Provider.ExisteCodigoByFiltro(codigo, id);
		//	response = Request.CreateResponse(HttpStatusCode.OK, data);
		//	return response;
		//}

		[Route("ListarCombos")]
		[HttpGet]
        [Authorize]
        public HttpResponseMessage PostListCombos(int idParametrica)
		{
			HttpResponseMessage response = null;

			//var listDias = Utilitarios.EnumToList<EDias>();

			var dataRpta = new
			{
                Entidades = ServiceManager<ParametricasDAO>.Provider.GetEntidades(idParametrica),
                Tablas = ServiceManager<ParametricasDAO>.Provider.GetTablas(idParametrica).Select(x => x.Descripcion).ToArray()
            };

			response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
			return response;
		}
	}
}
