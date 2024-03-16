using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Excepcion")]
    public class ExcepcionController : BaseController
    {
        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var listTipoRiesgo = Utilitarios.EnumToList<ETipoRiesgo>();
            var dataRpta = new
            {
                TipoRiesgo = listTipoRiesgo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                CodigoInterno = (int)ETablaProcedencia.CVT_Excepcion
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListExcepcion(PaginacionExcepcion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;

            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<ExcepcionDAO>.Provider.GetList(pag.nombre, pag.username, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.CodigoAPT, pag.TecnologiaId, pag.EquipoId, pag.TipoExcepcionId, out totalRows);

            if (registros != null)
            {
                registros.ForEach(x =>
                {
                    x.Comentario = HttpUtility.HtmlEncode(x.Comentario);
                    x.TecnologiaStr = HttpUtility.HtmlEncode(x.TecnologiaStr);
                    x.AplicacionStr = HttpUtility.HtmlEncode(x.AplicacionStr);
                    x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                    x.SubdominioStr= HttpUtility.HtmlEncode(x.SubdominioStr);
                    x.NombreAplicacion = HttpUtility.HtmlEncode(x.NombreAplicacion);
                });

                var dataRpta = new
                {
                    Total = totalRows,
                    Rows = registros
                };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostExcepcion(ExcepcionDTO request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int id = ServiceManager<ExcepcionDAO>.Provider.AddOrEdit(request);
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExcepcionById(int id)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<ExcepcionDAO>.Provider.GetById(id);
            if (dataRpta != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("Eliminar")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEliminarById(int id)
        {
            HttpResponseMessage response = null;
            bool dataRpta = ServiceManager<ExcepcionDAO>.Provider.DeleteById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportar(string filtro, string username, string sortName, string sortOrder, string codigoAPT, int? tecnologiaId, int? equipoId, int? tipoExcepcionId, string nombreExportar)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro)) filtro = null;
            if (string.IsNullOrEmpty(codigoAPT)) codigoAPT = null;
            var tituloExportar = (tipoExcepcionId == 2) ? "estandándares restringidos" : "excepciones por riesgo";
            var data = new ExportarData().ExportarExcepcion(filtro, username, sortName, sortOrder, codigoAPT, tecnologiaId, equipoId, tipoExcepcionId, tituloExportar);
            nomArchivo = string.Format("Listado{0}_{1}.xlsx", nombreExportar, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExcepcionRiesgoXTecnologiaId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExcepcionRiesgoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var totalRows = 0;
            var registros = ServiceManager<ExcepcionDAO>.Provider.GetExcepcionXTecnologiaId((int)ETipoExcepcion.Riesgo, tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ExcepcionDTO>() { Total = totalRows, Rows = registros };
            return Ok(reader);
        }

        [Route("ExcepcionEstandarXTecnologiaId")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExcepcionEstandarXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var totalRows = 0;
            var registros = ServiceManager<ExcepcionDAO>.Provider.GetExcepcionXTecnologiaId((int)ETipoExcepcion.Tipo, tecnologiaId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ExcepcionDTO>() { Total = totalRows, Rows = registros };
            return Ok(reader);
        }
    }
}
