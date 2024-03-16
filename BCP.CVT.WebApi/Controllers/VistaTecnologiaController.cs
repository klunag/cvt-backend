using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/VistaTecnologia")]
    public class VistaTecnologiaController : BaseController
    {
        // POST: api/VistaTecnologia/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListVistaTecnologia(PaginacionTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<VistaDAO>.Provider.GetVistaTecnologia(pag.domId,
                                                                          pag.subdomId,
                                                                          pag.nombre,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          pag.equipo,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCombos")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;           
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);          

            var dataRpta = new
            {                
                Dominio = listDominio              
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
    }
}
