using BCP.CVT.Cross;
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
    [RoutePrefix("api/public")]
    public class PublicController : BaseController
    {
        [Route("relacion/add")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRelacion(RelacionPublicDTO request)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.AddOrEditRelacionPublic(request);
            var HttpStatus = retorno.Codigo == (int)ECodigoRetorno.OK ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            response = Request.CreateResponse(HttpStatus, retorno);

            return response;
        }

        [Route("equipo/add")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEquipo(EquipoPublicDTO request)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<RelacionDAO>.Provider.AddOrEditEquipoPublic(request);
            var HttpStatus = retorno.Codigo == (int)ECodigoRetorno.OK ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            response = Request.CreateResponse(HttpStatus, retorno);

            return response;
        }

        [Route("equipo/list")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListadoEquipo(Paginacion request)
        {
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
            var totalRows = 0;
            var registros = ServiceManager<EquipoDAO>.Provider.GetEquipos(request.nombre, request.so, request.ambienteId, request.tipoId, idSubdominio, request.desId, request.subsiId,  request.pageNumber, request.pageSize, request.sortName, request.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("equipo/list/norelationapp")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEquipoSinRelaciones(PaginaReporteHuerfanos request)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            DateTime fechaActual = DateTime.Parse(request.FechaConsulta);
            var parametroSO = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("SUBDOMINIO_SISTEMA_OPERATIVO").Valor;

            var registros = ServiceManager<ReporteDAO>.Provider.GetServidoresHuerfanos(request, fechaActual, int.Parse(parametroSO), out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("relacion/list")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListadoRelacion(PaginacionRelacion request)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<RelacionDAO>.Provider.GetRelacionSP(request.CodigoAPT
                , request.Componente
                , request.pageNumber
                , request.pageSize
                , request.sortName
                , request.sortOrder
                , request.username
                , request.TipoRelacionId
                , request.EstadoId
                , 1
                , out totalRows);

            if (registros != null)
            {
                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }
    }
}
