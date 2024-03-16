using BCP.CVT.DTO;
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
    [RoutePrefix("api/BigFix")]
    public class BigFixController : BaseController
    {
        [Route("ExisteID")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetExisteID(string Id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<BigFixDAO>.Provider.ExisteID(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ConsultarEstadoTransaccion")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetConsultarEstadoTransaccion(string Id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<BigFixDAO>.Provider.ConsultarEstadoBigFix(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("SolicitarArchivosBigFix")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostSolicitarArchivosBigFix(BigFixDTO obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula;
            obj.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<BigFixDAO>.Provider.SolicitarArchivosBigFix(obj);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ActualizarPeticionBigFix")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ActualizarPeticionBigFix(int id, bool estado, string idBigFix)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<BigFixDAO>.Provider.UpdatePeticion(id, estado, idBigFix);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ListarPeticionesBigFix")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ListarPeticionesBigFix(bool flagProcesado)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<BigFixDAO>.Provider.ListarBixFixPorFlagProcesado(flagProcesado);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
    }
}
