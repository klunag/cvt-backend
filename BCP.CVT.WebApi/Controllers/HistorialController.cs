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
    [RoutePrefix("api/Historial")]
    public class HistorialController : ApiController
    {
        [Route("ListadoByEntidadId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListHistorialByEntidadId(string entidad, string id)
        {
            var registros = ServiceManager<HistorialDAO>.Provider.GetHistorialModificacionByEntidadId(entidad, id);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Data = HttpUtility.HtmlEncode(x.Data); });

            return Ok(registros);
        }
    }
}
