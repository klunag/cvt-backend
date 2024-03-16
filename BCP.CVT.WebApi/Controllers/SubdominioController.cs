using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Subdominio")]
    public class SubdominioController : BaseController
    {
        // POST: api/Subdominio/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSubdom(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SubdominioDAO>.Provider.GetSubdom(pag.id,
                                                                             pag.nombre,
                                                                             pag.pageNumber,
                                                                             pag.pageSize,
                                                                             pag.sortName,
                                                                             pag.sortOrder,
                                                                             out totalRows);
            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.DomNomAsociado = HttpUtility.HtmlEncode(x.DomNomAsociado);
                x.MatriculaDueno = HttpUtility.HtmlEncode(x.MatriculaDueno);
            });

            dynamic reader = new BootstrapTable<SubdominioDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/Subdominio/ListarSubdominiosEquivalentes
        [Route("ListarSubdominiosEquivalentes")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarSubdominiosEquivalentes(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SubdominioDAO>.Provider.GetSubdominiosEquivalentes(pag.id,
                                                                             pag.pageNumber,
                                                                             pag.pageSize,
                                                                             pag.sortName,
                                                                             pag.sortOrder,
                                                                             out totalRows);
            if (registros == null)
                return NotFound();


            registros.ForEach(x =>
            {
                x.EquivalenciaSubdomNombre = HttpUtility.HtmlEncode(x.EquivalenciaSubdomNombre);
                x.SubdominioNombre = HttpUtility.HtmlEncode(x.SubdominioNombre);
                x.DominioNombre = HttpUtility.HtmlEncode(x.DominioNombre);
            });

            dynamic reader = new BootstrapTable<SubdominioEquivalenciaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // GET: api/Subdominio/5
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(SubdominioDTO))]
        [Authorize]
        public IHttpActionResult GetSubdomById(int id)
        {
            var objSubdom = ServiceManager<SubdominioDAO>.Provider.GetSubdomById(id);
            if (objSubdom == null)
                return NotFound();

            return Ok(objSubdom);
        }

        // GET: api/Subdominio/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<SubdominioDAO>.Provider.GetSubdomById(Id);
            var retorno = ServiceManager<SubdominioDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/Subdominio/ValidarRegistrosAsociados/5
        [Route("ValidarRegistrosAsociados/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetValidarRegAsociados(int id)
        {
            //var entidad = ServiceManager<SubdominioDAO>.Provider.GetSubdomById(id);
            //var retorno = ServiceManager<SubdominioDAO>.Provider.CambiarEstado(id, !entidad.Activo, "Usuario modificacion");
            var listAsoc = ServiceManager<SubdominioDAO>.Provider.ValidarRegistrosAsociados(id);

            return Ok(listAsoc);
        }

        // GET: api/Subdominio/ObtenerSubdominioEquivalencia/5
        [Route("ObtenerSubdominioEquivalencia/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSubdominiosEqBySubdom(int id)
        {
            var listSubdomEq = ServiceManager<SubdominioDAO>.Provider.GetSubdominiosEqBySubdom(id);
            if (listSubdomEq == null)
                return NotFound();

            return Ok(listSubdomEq);
        }

        // GET: api/Subdominio/ObtenerTecnologia
        [Route("ObtenerSubdominio")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSubdominio()
        {
            var listSubdom = ServiceManager<SubdominioDAO>.Provider.GetSubdominio();
            if (listSubdom == null)
                return NotFound();

            return Ok(listSubdom);
        }

        // GET: api/Subdominio/ListarSubdominios
        [Route("ListarSubdominios")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubdominioMultiSelect()
        {
            HttpResponseMessage response = null;
            var listSubdom = ServiceManager<SubdominioDAO>.Provider.GetSubdominiosMultiSelect();
            if (listSubdom != null)
                response = Request.CreateResponse(HttpStatusCode.OK, listSubdom);

            return response;
        }

        // GET: api/Subdominio
        [Route("ObtenerMatricula/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetMatriculaSubdominio(int id)
        {
            var matriculaSub = ServiceManager<SubdominioDAO>.Provider.GetMatriculaSubDominio(id);
            if (matriculaSub == null)
                return NotFound();

            return Ok(matriculaSub);
        }

        [Route("ObtenerSubdominiosEquivalentesByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetSubdominiosEquivalentesByFiltro(ParametroSubdomEquivalencia obj)
        {
            var listSubdom = ServiceManager<SubdominioDAO>.Provider.GetSubdominiosEquivalentesByFiltro(obj.filtro, obj.id);
            if (listSubdom == null)
                return NotFound();

            return Ok(listSubdom);
        }

        // POST: api/Subdominio
        [Route("AsociarSubdominioEquivalencia")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostAsocTecnologiasEq(ParametroTecEq request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var retorno = ServiceManager<SubdominioDAO>.Provider.AsociarSubdominiosEquivalentes(request.Id, request.Equivalencia, request.Usuario);

            return Ok(retorno);
        }

        // POST: api/Subdominio
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(SubdominioDTO))]
        [Authorize]
        public IHttpActionResult PostSubdominio(SubdominioDTO subdomDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            subdomDTO.UsuarioCreacion = user.Matricula;
            subdomDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdSubdom = ServiceManager<SubdominioDAO>.Provider.AddOrEditSubdom(subdomDTO);

            if (IdSubdom == 0)
                return NotFound();

            return Ok(IdSubdom);
        }

        // POST: api/Subdominio/Tecnologias
        [Route("Tecnologias")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostTecBySub(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SubdominioDAO>.Provider.GetTecBySubdom(pag.id, pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportSubdominios(string nombre, int dominioId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarSubdominio(nombre, dominioId, sortName, sortOrder);
            nomArchivo = string.Format("ListaSubdominio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarTecAsociadas")]
        [HttpGet]
        public IHttpActionResult PostExportarTecAsociadas(string nombre, int subdominioId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarTecAsocASubdom(nombre, subdominioId, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologiasAsociadas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("CambiarFlagEquivalencia")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCambiarFlagExperto(ObjCambioEstado request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            bool retorno = ServiceManager<SubdominioDAO>.Provider.CambiarFlagEquivalencia(int.Parse(request.Id), request.Usuario);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ListarSubdominiosByDominioId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubdominiosByDominioId(int Id)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<SubdominioDAO>.Provider.GetSubdominioByDominioId(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }
    }
}
