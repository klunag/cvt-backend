using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Parametro")]
    public class ParametroController : ApiController
    {

        [Route("ObtenerParametro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ObtenerParametro(string codigo)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(codigo);
            var HttpStatus = retorno != null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            response = Request.CreateResponse(HttpStatus, retorno);

            return response;
        }
    }
}
