using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Exportar;
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
    [RoutePrefix("api/Seguridad")]
    public class SeguridadController : BaseController
    {
        [Route("ListadoAuthAplicacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetListadoAuth(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AuthDAO>.Provider.GetAuthAplicacion(pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<AuthAplicacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };


            return Ok(reader);
        }

        [Route("ExisteKeyAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteKeyAplicacion(string codAplicacion)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<AuthDAO>.Provider.ExisteAuthKeyByCodigoAPT(codAplicacion);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("ObtenerAuthAplicacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetObtenerAuth(string codAplicacion)
        {
            var registro = ServiceManager<AuthDAO>.Provider.ObtenerApiKey(codAplicacion);

            if (registro == null)
                return NotFound();

            var aplicacion = ServiceManager<AplicacionDAO>.Provider.GetAplicacionByCodigo(registro.CodigoAplicacion);

            dynamic data = new
            {
                registro,
                aplicacion
            };

            return Ok(data);
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostAplicacionAuth(AuthAplicacionDTO obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.UsuarioCreacion = user.Matricula.Length > 20 ? user.Matricula.Substring(0, 20) : user.Matricula;
            obj.UsuarioModificacion = user.Matricula.Length > 20 ? user.Matricula.Substring(0, 20) : user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(obj.AuthAplicacionId != 0) { 
                ServiceManager<AuthDAO>.Provider.ActualizarAuthApiKey(obj);
            }
            else
            {
                ServiceManager<AuthDAO>.Provider.RegistrarAuthApiKey(obj);
            }

            return Ok(obj.AuthAplicacionId);
        }
    }
}