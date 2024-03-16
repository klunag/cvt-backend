using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
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
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : BaseController
    {               
        [ResponseType(typeof(BaseUsuarioDTO))]
        [HttpGet]
        public IHttpActionResult GetTipoById(string correo)
        {
            var obj = ServiceManager<UsuarioDAO>.Provider.GetUsuarioByMail(correo);
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
        
        [HttpGet]
        public IHttpActionResult UpdateUsuario(string correo, string matricula, string nombres)
        {
            ServiceManager<UsuarioDAO>.Provider.UpdateBaseUsuarios(correo, matricula, nombres);            
            return Ok(true);
        }
    }
}
