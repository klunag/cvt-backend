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
    [RoutePrefix("api/UnidadFondeo")]
    public class UnidadFondeoController : BaseController
    {
        [Route("PostListadoUnidadFondeo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoUnidadFondeo(Paginacion pag)
        {

            var registros = ServiceManager<UnidadFondeoDAO>.Provider.Unidad_Fondeo_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Nombre = HttpUtility.HtmlEncode(x.Nombre));

            dynamic reader = new BootstrapTable<UnidadFondeoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("GetUnidadFondeoId/{id:int}")]
        [ResponseType(typeof(UnidadFondeoDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUnidadFondeoById(int id)
        {

            var objTipo = ServiceManager<UnidadFondeoDAO>.Provider.Unidad_Fondeo_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostUnidadFondeo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUnidadFondeo(UnidadFondeoDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            if (pag.UnidadFondeoId > 0)
            {
                dataRpta = ServiceManager<UnidadFondeoDAO>.Provider.Unidad_Fondeo_Update(pag);
            }
            else
            {
                dataRpta = ServiceManager<UnidadFondeoDAO>.Provider.Unidad_Fondeo_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listUnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();
            var listSegundoNivel = ServiceManager<AplicacionDAO>.Provider.GetListSegundoNivel();

            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
                UnidadFondeo = listUnidadFondeo,
                SegundoNivel = listSegundoNivel
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;

        }

        [Route("Lista/AppsPorSegundoNivel")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GerenciaDivision(PaginaReporteGerencia filtros)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReporteDAO>.Provider.GetAppsPorSegundNivel(filtros, out totalRows);
            if (registros != null)
            {
                registros.ForEach(x => { x.Aplicacion = HttpUtility.HtmlEncode(x.Aplicacion); });
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("AsignarUdF")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage AsignarUdF(SegundoNivelDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            dataRpta = ServiceManager<UnidadFondeoDAO>.Provider.Unidad_Fondeo_Asignar(pag);

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }




    }
}
