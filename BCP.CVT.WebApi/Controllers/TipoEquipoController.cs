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
   [Authorize]
    [RoutePrefix("api/TipoEquipo")]
    public class TipoEquipoController : BaseController
    {
        // POST: api/TipoEquipo/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTipoEquipo(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TipoEquipoDAO>.Provider.GetTipoEquipo(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TipoEquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/TipoEquipo
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TipoEquipoDTO))]
        [Authorize]
        public IHttpActionResult PostTipoCicloVida(TipoEquipoDTO TCVDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            TCVDTO.ModificadoPor = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int Id = ServiceManager<TipoEquipoDAO>.Provider.AddOrEditTipoEquipo(TCVDTO);
            return Ok(Id);
        }

        // GET: api/TipoEquipo/{id}
        [Route("{id:int}")]
        [ResponseType(typeof(TipoEquipoDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTipoCicloVidaById(int id)
        {
            var objTipoEquipo = ServiceManager<TipoEquipoDAO>.Provider.GetTipoEquipoById(id);
            if (objTipoEquipo == null)
                return NotFound();

            return Ok(objTipoEquipo);
        }

        [Route("ObtenerCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ObtenerCombos()
        {
            HttpResponseMessage response = null;
            var listTipoCicloVida = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVidaByFiltro(false);
            var dataRpta = new
            {
                TipoCicloVida = listTipoCicloVida,
                
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
    }
}