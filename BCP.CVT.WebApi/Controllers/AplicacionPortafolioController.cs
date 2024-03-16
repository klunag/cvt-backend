using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
	//[RoutePrefix("api/AplicacionPortafolio")]
	//public class AplicacionPortafolioController : BaseController
	//{
	//	// GET: api/Parametricas/5
	//	[Route("{id:int}")]
	//	[ResponseType(typeof(AplicacionPortafolioDTO))]
	//	[HttpGet]
	//	[Authorize]
	//	public IHttpActionResult GetParametricasById(int id)
	//	{
	//		var objPar = ServiceManager<AplicacionPortafolioDAO>.Provider.GetAplicacionById(id);
	//		if (objPar == null)
	//			return NotFound();

	//		return Ok(objPar);
	//	}

	//	// GET: api/Ambiente/CambiarEstado/5
	//	[Route("CambiarEstado")]
	//	[HttpGet]
	//	[Authorize]
	//	public HttpResponseMessage GetCambiarEstado(int Id, string Usuario)
	//	{
	//		HttpResponseMessage response = null;
	//		var entidad = ServiceManager<AplicacionPortafolioDAO>.Provider.GetAplicacionById(Id);
	//		var retorno = ServiceManager<AplicacionPortafolioDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

	//		response = Request.CreateResponse(HttpStatusCode.OK, retorno);
	//		return response;
	//	}

	//	// POST: api/Parametricas
	//	[Route("")]
	//	[HttpPost]
	//	[ResponseType(typeof(AplicacionPortafolioDTO))]
	//	[Authorize]
	//	public IHttpActionResult PostParametricas(AplicacionPortafolioDTO appDTO)
	//	{
	//		//if (!ModelState.IsValid)
	//		//	return BadRequest(ModelState);

	//		int AplicacionDetalleId = ServiceManager<AplicacionPortafolioDAO>.Provider.AddOrEditAplicacion(appDTO);

	//		//if (ParametricaDetalleId == 0)
	//		//	return NotFound();

	//		return Ok(AplicacionDetalleId);
	//	}


	//	// POST: api/Ambiente/Listado
	//	[Route("Listado")]
	//	[HttpPost]
	//	[Authorize]
	//	public IHttpActionResult PostListAplicacion(Paginacion pag)
	//	{
	//		var totalRows = 0;
	//		var registros = ServiceManager<AplicacionPortafolioDAO>.Provider.GetAplicacion(pag, out totalRows);

	//		if (registros == null)
	//			return NotFound();

	//		dynamic reader = new BootstrapTable<AplicacionPortafolioDTO>()
	//		{
	//			Total = totalRows,
	//			Rows = registros
	//		};

	//		return Ok(reader);
	//	}

	

	//	[Route("ListarCombos")]
	//	[HttpGet]
	//	[Authorize]
	//	public HttpResponseMessage PostListCombos(int idAplicacion)
	//	{
	//		HttpResponseMessage response = null;

	//		//var listDias = Utilitarios.EnumToList<EDias>();

	//		var dataRpta = new
	//		{
	//			GestionadoPor = ServiceManager<AplicacionPortafolioDAO>.Provider.GetGestionadoPor(idAplicacion),
	//			TipoImplementacion = ServiceManager<AplicacionPortafolioDAO>.Provider.GetTipoImplementacion(idAplicacion).Select(x => x.Descripcion).ToArray(),
	//		ModeloEntrega = ServiceManager<AplicacionPortafolioDAO>.Provider.GetModeloEntrega(idAplicacion),
	//			EstadoAplicacion = ServiceManager<AplicacionPortafolioDAO>.Provider.GetEstadoAplicacion(idAplicacion).Select(x => x.Descripcion).ToArray()


	//		};

	//		response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
	//		return response;
	//	}
	//}
}
