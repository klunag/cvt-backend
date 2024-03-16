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
using BCP.CVT.WebApi.Auth;
using BCP.CVT.Services.Exportar;
using System.Globalization;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/resources")]
    public class ResourceController : BaseController
    {
        [Route("types")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetAzureResourcesTypes(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<AzureResourceDAO>.Provider.GetAzureTypes(pag, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<AzureResourceTypeDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("updateType")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateStateType(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<AzureResourceDAO>.Provider.UpdateActiveStatus(request.ObjetoId, request.Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;                      
        }

        [Route("updateVirtualMachine")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateVirtualMachine(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var state = ServiceManager<AzureResourceDAO>.Provider.UpdateVirtualMachineStatus(request.ObjetoId, request.Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, state);
            return response;
        }

        [Route("types/export")]
        [HttpGet]        
        public IHttpActionResult ExportTypes(string name)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(name)) name = null;
            var data = new ExportarData().ExportarTipoRecursosAzure(name);
            nomArchivo = string.Format("ListarTipoRecursosAzure_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("azureResources/export")]
        [HttpGet]
        public IHttpActionResult ExportAzureResources(string dateFilter)
        {
            var _dateFilter = DateTime.Now;
            try
            {
                _dateFilter = DateTime.ParseExact(dateFilter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
               
            }
            
            var data = new ExportarData().ExportarAzureResources(_dateFilter);
            var filename = string.Format("ListarAzureResources_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }
    }
}