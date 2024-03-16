using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers.API
{
   [Authorize]
    [RoutePrefix("api/it-management/plataform-operations/v2/siga")]
    public class SigaController : BaseController
    {
        [Route("employee/{codMatricula}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTypes(string codMatricula)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<SigaMgtDAO>.Provider.GetEmployeeSigaMgtDTOByMatricula(codMatricula);

            var apiResp = data == null ? new { } : (object)data;

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("employee/{correoElectronico}/info")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTypesByEmail(string correoElectronico)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<SigaMgtDAO>.Provider.GetEmployeeSigaMgtDTOByMatricula(correoElectronico);

            var apiResp = data == null ? new { } : (object)data;

            response = Request.CreateResponse(HttpStatusCode.OK, apiResp);
            return response;
        }

        [Route("unidadorganizativa/relacionproducto/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUnidadOrganizativaProductoByFiltro(string filtro)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadTribuCoeProductoPorFiltro(filtro);

            return Ok(data);
        }

        [Route("squad/relacionproducto/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSquadProductoByFiltro(string codigoUnidad, string filtro = null)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadSquadProductoPorFiltro(codigoUnidad, filtro);

            return Ok(data);
        }

        [Route("unidadorganizativa/relaciontecnologia/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUnidadOrganizativaTecnologiaByFiltro(string filtro)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadTribuCoeTecnologiaPorFiltro(filtro);

            return Ok(data);
        }

        [Route("squad/relaciontecnologia/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSquadTecnologiaByFiltro(string codigoUnidad, string filtro = null)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadSquadTecnologiaPorFiltro(codigoUnidad, filtro);

            return Ok(data);
        }

        [Route("unidadorganizativa/relaciontecnologiakpi/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUnidadOrganizativaTecnologiaKPIByFiltro(string filtro)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadTribuCoeTecnologiaKPIPorFiltro(filtro);

            return Ok(data);
        }

        [Route("squad/relaciontecnologiakpi/listarcomboxfiltro")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSquadTecnologiaKPIByFiltro(string codigoUnidad, string filtro = null)
        {
            var data = ServiceManager<SigaMgtDAO>.Provider.BuscarUnidadSquadTecnologiaKPIPorFiltro(codigoUnidad, filtro);

            return Ok(data);
        }
    }
}
