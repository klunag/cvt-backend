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
    [RoutePrefix("api/DependenciasAppsV2")]
    public class DependenciasAppsV2Controller : BaseController
    {
        [Route("PostProcesarN")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostProcesarDependenciasApps(PaginacionDependenciasApps pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasAppsDAO>.Provider.ProcesarDependenciasApps(pag.codigoAPT, pag.Activos, pag.FlagProceso);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
        [Route("PostRegistrarN")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRegistrarDependenciasApps(PaginacionDependenciasApps pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasAppsDAO>.Provider.RegistrarDependenciasApps(pag.codigoAPT, pag.Activos, pag.FlagProceso);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("PostListadoN")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListaDependenciasApps(PaginacionDependenciasApps pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasAppsDAO>.Provider.ListaDependenciasApps(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.NombreAPT = HttpUtility.HtmlEncode(x.NombreAPT); });

            dynamic reader = new BootstrapTable<DependenciasAppsDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("PostEtiquetas")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEtiquetas(PaginacionEtiquetas pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasAppsDAO>.Provider.ProcesarEtiquetas(pag.EtiquetaId, pag.Descripcion, pag.Activos, pag.FlagDefault);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("PostListadoEtiquetas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoEtiquetas(PaginacionEtiquetas pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasAppsDAO>.Provider.ListaEtiquetas(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion); });

            dynamic reader = new BootstrapTable<EtiquetasDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("GetEtiquetasId/{id:int}")]
        [ResponseType(typeof(EtiquetasDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetEtiquetasIdById(int id)
        {
            PaginacionEtiquetas pag = new PaginacionEtiquetas();
            pag.EtiquetaId = id;
            pag.Descripcion = "";
            var objTipo = ServiceManager<DependenciasAppsDAO>.Provider.ObtenerEtiquetas(pag);
            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostTiposRelacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTiposRelacion(PaginacionTiposRelacion pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasAppsDAO>.Provider.ProcesarTipoRelacion(pag.TipoRelacionId, pag.Descripcion, pag.Activos, pag.PorDefecto);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("PostListadoTiposRelacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoTiposRelacion(PaginacionTiposRelacion pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasAppsDAO>.Provider.ListaTiposRelacion(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion); });

            dynamic reader = new BootstrapTable<TiposRelacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("GetTiposRelacionId/{id:int}")]
        [ResponseType(typeof(TiposRelacionDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTiposRelacionIdById(int id)
        {
            PaginacionTiposRelacion pag = new PaginacionTiposRelacion();
            pag.TipoRelacionId = id;
            pag.Descripcion = "";
            var objTipo = ServiceManager<DependenciasAppsDAO>.Provider.ObtenerTiposRelacion(pag);
            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

    }
}
