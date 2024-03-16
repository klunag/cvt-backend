using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Login")]
    public class LoginController : BaseController
    {        
        [Route("ObtenerProyeccion")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ObtenerProyeccion()
        {
            HttpResponseMessage response = null;
            var proyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
            var proyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
            var fecha = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FECHA_ACTUALIZACION_PLATAFORMA").Valor;
            var rpta = new { Proyeccion1 = proyeccion1, Proyeccion2 = proyeccion2, FechaActualizacion = fecha };
            response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            return response;
        }

        [Route("ValidarFlagAprobacion")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage ObtenerFlagAprobacion(BaseUsuarioDTO usuario)
        {
            var user = TokenGenerator.GetCurrentUser();
            usuario.Matricula = user.Matricula;
            HttpResponseMessage response = null;
            var totalAplicaciones = ServiceManager<AplicacionDAO>.Provider.GetAplicacionesExperto(usuario.Matricula);
            //var bandeja = 1;
            var bandeja = ServiceManager<UsuarioDAO>.Provider.DevolverBandejaRegistro(usuario.Matricula);

            if(totalAplicaciones.Count > 0)
            {
                var rpta = new { FlagAprobador = true, TotalAplicaciones = totalAplicaciones.Count, Bandeja = bandeja };
                response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            }
            else
            {
                var rpta = new { FlagAprobador = false, TotalAplicaciones = 0, Bandeja = bandeja };
                response = Request.CreateResponse(HttpStatusCode.OK, rpta);
            }
            
            return response;
        }

        [Route("RegistrarBaseUsuario")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostRegistrarBaseUsuario(BaseUsuarioDTO entidad)
        {
            HttpResponseMessage response = null;
            var EntidadId = ServiceManager<UsuarioDAO>.Provider.AddOrEditUsuario(entidad);
            
            response = Request.CreateResponse(HttpStatusCode.OK, EntidadId);
            return response;
        }

        [Route("RegistrarVisitaSite")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRegistrarVisitaSite(VisitaSiteDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.Matricula = user.Matricula;
            entidad.Nombre = user.Nombres;

            HttpResponseMessage response = null;
            var EntidadId = ServiceManager<UsuarioDAO>.Provider.AddOrEditVisitaSite(entidad);
            if (EntidadId != 0)
            {                
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                response = Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Error al procesar el inicio de sesión");

            
            return response;
        }

        [Route("DevolverMatricula")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage DevolverMatricula(string Correo)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<UsuarioDAO>.Provider.DevolverMatriculaByCorreo(Correo);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }
    }
}
