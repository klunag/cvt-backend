using BCP.CVT.DTO;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
    [RoutePrefix("api/[controller]")]
    [Authorize]
    public class LoginExternoController : BaseController
    {
        // GET: LoginExterno
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult LoginExterno(UserExternoDTO userExterno)
        {

            var data = TokenGenerator.GenerateTokenJwtExterno(userExterno);

            return Ok(data);
        }
    }
}