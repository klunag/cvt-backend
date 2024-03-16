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
    [RoutePrefix("api/Proceso")]
    public class ProcesoController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoProceso(Paginacion pag)
        {

            var registros = ServiceManager<MotorProcesoDAO>.Provider.Proceso_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<MotorProcesoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(MotorProcesoDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProcesoById(int id)
        {

            var objTipo = ServiceManager<MotorProcesoDAO>.Provider.Proceso_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostProceso")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProceso(MotorProcesoDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            if (pag.ProcesoId > 0)
            {
                dataRpta = ServiceManager<MotorProcesoDAO>.Provider.Proceso_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<MotorProcesoDAO>.Provider.Proceso_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var list = ServiceManager<MotorProcesoDAO>.Provider.GetByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, list);
            return response;
        }



    }
}
