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
using BCP.CVT.Cross;
using BCP.CVT.Services.Exportar;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Dominio")]
    public class DominioController : BaseController
    {
        // GET: api/Dominio/5
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(DominioDTO))]
		[Authorize]
		public IHttpActionResult GetDominioById(int id)
        {
            var objDominio = ServiceManager<DominioDAO>.Provider.GetDominioById(id);
            if (objDominio == null)
                return NotFound();

            return Ok(objDominio);
        }

        // GET: api/Dominio
        [Route("")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetAllDominio()
        {
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominio();
            if (listDominio == null)
                return NotFound();

            return Ok(listDominio);
        }

        // GET: api/Dominio
        [Route("ObtenerMatricula/{id:int}")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetMatriculaDominio(int id)
        {
            var matriculaDominio = ServiceManager<DominioDAO>.Provider.GetMatriculaDominio(id);
            if (matriculaDominio == null)
                return NotFound();

            return Ok(matriculaDominio);
        }

        // GET: api/Dominio/ConSubdominio
        [Route("ConSubdominio")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDomConSubdom()
        {
            var listDominio = ServiceManager<DominioDAO>.Provider.GetDomConSubdom();
            if (listDominio == null)
            {
                return NotFound();
            }
            else
            {
                listDominio = listDominio.OrderBy(x => x.Nombre).ToList();
            }


            return Ok(listDominio);
        }

        // GET: api/Dominio/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<DominioDAO>.Provider.GetDominioById(Id);
            var retorno = ServiceManager<DominioDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // POST: api/Dominio
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DominioDTO))]
		[Authorize]
		public IHttpActionResult PostDominio(DominioDTO dominioDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            dominioDTO.UsuarioCreacion = user.Matricula;
            dominioDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdDominio = ServiceManager<DominioDAO>.Provider.AddOrEditDominio(dominioDTO);

            if (IdDominio == 0)
                return NotFound();

            return Ok(IdDominio);
        }

        // POST: api/Dominio/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListDominios(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<DominioDAO>.Provider.GetDominio(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.NombreEquipo = HttpUtility.HtmlEncode(x.NombreEquipo);
                x.MatriculaDueno = HttpUtility.HtmlEncode(x.MatriculaDueno);
            });

            dynamic reader = new BootstrapTable<DominioDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Subdominio")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostSubByDom(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<DominioDAO>.Provider.GetSubdominiosByDominio(int.Parse(pag.nombre), pag.pageNumber, pag.pageSize, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<SubdominioDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("CambiarDominio")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostCambiarDominio(CambioDominio cd)
        {
            var IdSubdom = ServiceManager<DominioDAO>.Provider.CambiarDominio(cd.SubdominioId, cd.DominioId);

            if (IdSubdom == 0)
                return NotFound();

            return Ok(IdSubdom);
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportDominios(string nombre, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarDominio(nombre, sortName, sortOrder);
            nomArchivo = string.Format("ListaDominio_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

    }
}
