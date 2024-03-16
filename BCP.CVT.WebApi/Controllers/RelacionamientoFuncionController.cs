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
    [RoutePrefix("api/RelacionamientoFuncion")]
    public class RelacionamientoFuncionController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoRelacionamientoFuncion(PaginacionRelacionamientoFuncion pag)
        {

            var registros = ServiceManager<RelacionamientoFuncionDAO>.Provider.RelacionamientoFuncion_List(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => x.Origen = HttpUtility.HtmlEncode(x.Origen));
            registros.ForEach(x => x.Destino = HttpUtility.HtmlEncode(x.Destino));

            dynamic reader = new BootstrapTable<RelacionamientoFuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(RelacionamientoFuncionDTO))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRelacionamientoFuncionById(int id)
        {

            var objTipo = ServiceManager<RelacionamientoFuncionDAO>.Provider.RelacionamientoFuncion_Id(id);

            if (objTipo == null)
                return NotFound();

            return Ok(objTipo);
        }

        [Route("PostRelacionamientoFuncion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRelacionamientoFuncion(RelacionamientoFuncionDTO pag)
        {
            HttpResponseMessage response = null;
            var dataRpta = "";
            if (pag.RelacionReglasGeneralesId > 0)
            {
                var user = TokenGenerator.GetCurrentUser();
                pag.ModificadoPor = user.Matricula;
                dataRpta = ServiceManager<RelacionamientoFuncionDAO>.Provider.RelacionamientoFuncion_Update(pag);
            }
            else
            {
                var user = TokenGenerator.GetCurrentUser();
                pag.CreadoPor = user.Matricula;
                dataRpta = ServiceManager<RelacionamientoFuncionDAO>.Provider.RelacionamientoFuncion_Insert(pag);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var list = ServiceManager<RelacionamientoFuncionDAO>.Provider.GetByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, list);
            return response;
        }
        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FUNCION_RELACIONES");
            var dataParametro = parametro != null ? parametro.Valor : "File server;Base de datos";

            var listFuncion = dataParametro.Split(';');

            var dataRpta = new
            {
                Funcion = listFuncion,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
        [Route("ListarTecnologiasAzure")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListTecnologiasAzure()
        {
            HttpResponseMessage response = null;
            List<string> dataRpta = new List<string>();
            var listTecAzure = ServiceManager<TecnologiaDAO>.Provider.GetAllTecnologiaAzure();
            if (listTecAzure.Count > 0)
            {
                foreach (var item in listTecAzure)
                {
                    dataRpta.Add(item.Descripcion);
                }
            }
            var dataRptaF = new
            {
                Funcion = dataRpta,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRptaF);
            return response;
        }
    }
}