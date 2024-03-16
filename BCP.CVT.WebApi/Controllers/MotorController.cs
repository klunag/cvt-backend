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
    [RoutePrefix("api/Motor")]
    public class MotorController : BaseController
    {
        [Route("ProcesoExcluir/Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoProceso(Paginacion pag)
        {

            var registros = ServiceManager<MotorDAO>.Provider.ProcesoExcluir_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<MotorExcluirDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ProcesoExcluir/{id:int}")]
        [ResponseType(typeof(MotorExcluirDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProcesoById(int id)
        {

            var objTipo = ServiceManager<MotorDAO>.Provider.ProcesoExcluir_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("ProcesoExcluir/PostProceso")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProceso(MotorExcluirDTO pag)
        {
            HttpResponseMessage response = null;
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;
            pag.UsuarioCreacion = Usuario;
            pag.UsuarioModificacion = Usuario;
            var dataRpta = "";
            if (pag.Id > 0)
            {
                dataRpta = ServiceManager<MotorDAO>.Provider.ProcesoExcluir_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<MotorDAO>.Provider.ProcesoExcluir_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }


        [Route("ServidorOrigenExcluir/Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoServidorOrigen(Paginacion pag)
        {

            var registros = ServiceManager<MotorDAO>.Provider.ServidorOrigenExcluir_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<MotorExcluirDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ServidorOrigenExcluir/{id:int}")]
        [ResponseType(typeof(MotorExcluirDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetServidorOrigenById(int id)
        {

            var objTipo = ServiceManager<MotorDAO>.Provider.ServidorOrigenExcluir_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("ServidorOrigenExcluir/PostServidor")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostServidorOrigen(MotorExcluirDTO pag)
        {
            HttpResponseMessage response = null;
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;
            pag.UsuarioCreacion = Usuario;
            pag.UsuarioModificacion = Usuario;
            var dataRpta = "";
            if (pag.Id > 0)
            {
                dataRpta = ServiceManager<MotorDAO>.Provider.ServidorOrigenExcluir_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<MotorDAO>.Provider.ServidorOrigenExcluir_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
    }
}
