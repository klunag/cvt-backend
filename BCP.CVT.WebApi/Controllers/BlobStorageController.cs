using BCP.CVT.DTO;
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
    [RoutePrefix("api/BlobStorage")]
    public class BlobStorageController : BaseController
    {
        [Route("CargarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCargarCombos()
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<BlobStorageDAO>.Provider.GetContainers();
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetBlobStorage(PaginationBlob pag)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<BlobStorageDAO>.Provider.GetBlobsByFilters(pag, out int totalRows);
            var reader = new BootstrapTable<BlobStorageDTO>()
            {
                Total = totalRows,
                Rows = data
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Descargar")]
        [HttpGet]
        public IHttpActionResult GetDescargar(string filename, string containerName, string prefix)
        {
            var data = ServiceManager<BlobStorageDAO>.Provider.DownloadFileByFilters(filename, containerName, prefix);

            return Ok(new { data = data, name = filename });
        }
    }
}
