using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BCP.CVT.DTO;
using BCP.CVT.Cross;
using System.Web.Http.Description;
using System.Security.Claims;

namespace BCP.CVT.WebApi.Controllers
{
    [RoutePrefix("api/[controller]")]
    [Authorize]
    public class LoginFrontController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // POST: LoginFront
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(UsuarioDTO usuario)
        {
            UsuarioDTO_Storage UserStorage = new UsuarioDTO_Storage();

            if (!string.IsNullOrEmpty(usuario.UsuarioBCP_Dto.Matricula))
            {
                var token = TokenGenerator.GenerateTokenJwt(usuario);
                UserStorage = new UsuarioDTO_Storage
                {
                    UsuarioBCP = usuario.UsuarioBCP,
                    UserName = usuario.UserName,
                    Proyeccion1 = usuario.Proyeccion1,
                    Proyeccion2 = usuario.Proyeccion2,
                    FechaActualizacion = usuario.FechaActualizacion,
                    SiteTitle = usuario.SiteTitle,
                    CorreoElectronico = usuario.CorreoElectronico,
                    Token = token,
                    UsuarioBCP_Dto = new UsuarioBCP_DTO_Storage
                    {
                        SamAccountName = usuario.UsuarioBCP_Dto.SamAccountName,
                        Name = usuario.UsuarioBCP_Dto.Name,
                        Matricula = usuario.UsuarioBCP_Dto.Matricula,
                        Bandeja = usuario.UsuarioBCP_Dto.Bandeja
                    },
                };

            }

            return Ok(UserStorage);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Get ()
        {
            return Ok(TokenGenerator.GetCurrentUser());
        }
    }
}