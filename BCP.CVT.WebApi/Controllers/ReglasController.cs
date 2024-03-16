using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.Cross;
using BCP.CVT.Services.Exportar;
using System.Web;
using BCP.CVT.Services.CargaMasiva;
using System.Threading.Tasks;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Reglas")]
    public class ReglasController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListMotorReglas(PaginacionMotorReglas pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<MotorReglasDAO>.Provider.GetMotorReglas(pag, out totalRows);
            

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<MotorReglasDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var listProceso = ServiceManager<MotorProcesoDAO>.Provider.Proceso_List_Combo();
            var listProtocolo = ServiceManager<ProtocoloDAO>.Provider.Protocolo_List_Combo();
            var listPuerto = ServiceManager<PuertoDAO>.Provider.Puerto_List_Combo();

            var dataRpta = new
            {
                Proceso = listProceso,
                Puerto = listPuerto,
                Protocolo = listProtocolo
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id, bool Activo, string Motivo)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<MotorReglasDAO>.Provider.CambiarEstado(Id, !Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }


        [Route("")]
        [HttpPost]
        [ResponseType(typeof(MotorReglasDTO))]
        [Authorize]
        public HttpResponseMessage PostMotorReglas(MotorReglasDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            
            HttpResponseMessage response = null;

            //if (!ModelState.IsValid)
            //    return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var  res = ServiceManager<MotorReglasDAO>.Provider.AddOrEditMotorReglas(entidad);
            int entidadId = Int32.Parse(res.Split('|')[0].ToString());

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }




    }
}