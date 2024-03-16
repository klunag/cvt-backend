using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Arquetipo")]
    public class ArquetipoController : BaseController
    {
        // POST: api/Arquetipo
        [Route("")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostArquetipo(ArquetipoDTO arqDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            arqDTO.UsuarioCreacion = user.Matricula;
            arqDTO.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var request = HttpContext.Current.Request;
            //HttpPostedFile diagArqFile = null;
            //string path = string.Empty;
            //string nombreDiag = string.Empty;

            //if (request.Files.Count > 0)
            //{
            //    string ruta = HttpContext.Current.Server.MapPath("~/App_Data/FileServer");
            //    diagArqFile = request.Files["DiagArq"];
            //    path = Path.Combine(ruta, diagArqFile.FileName);
            //    nombreDiag = diagArqFile.FileName;
            //}
            //else if (request.Params["FileName"] != "")
            //{
            //    var objArquetipo = ServiceManager<ArquetipoDAO>.Provider.GetArquetipoById(int.Parse(request.Params["Id"]));
            //    path = objArquetipo.DirFisicaDiag;
            //    nombreDiag = objArquetipo.NombreDiag;
            //}

            //if(request.Params["Nombre"] != "")
            //{
            //    var objArquetipo = ServiceManager<ArquetipoDAO>.Provider.GetArquetipoById(int.Parse(request.Params["Id"]));
            //    path = objArquetipo.DirFisicaDiag;
            //    nombreDiag = objArquetipo.NombreDiag;
            //}

            //var arqDTO = new ArquetipoDTO()
            //{
            //    Id = int.Parse(request.Params["Id"]),
            //    Nombre = request.Params["Nombre"],
            //    Descripcion = request.Params["Descripcion"],
            //    Activo = bool.Parse(request.Params["Activo"]),
            //    DiagArqFile = diagArqFile,
            //    DirFisicaDiag = path,
            //    NombreDiag = nombreDiag
            //};

            int IdArq = ServiceManager<ArquetipoDAO>.Provider.AddOrEditArquetipo(arqDTO);

            if (IdArq == 0)
                return NotFound();

            return Ok(IdArq);
        }

        // POST: api/Arquetipo/Listado
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListArquetipo(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ArquetipoDAO>.Provider.GetArquetipo(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Codigo = HttpUtility.HtmlEncode(x.Codigo);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.TipoArquetipoStr = HttpUtility.HtmlEncode(x.TipoArquetipoStr);
                x.EntornoStr = HttpUtility.HtmlEncode(x.EntornoStr);
            });

            dynamic reader = new BootstrapTable<ArquetipoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        // POST: api/Arquetipo
        [Route("AsociarTecnologias")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostAsocTecnologias(ParametroTec request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var retorno = ServiceManager<ArquetipoDAO>.Provider.AsociarTecByArq(request.itemsTecId, request.itemsRemoveTecId, request.arqId);

            return Ok(retorno);
        }

        // GET: api/Arquetipo/5
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(ArquetipoDTO))]
		[Authorize]
		public IHttpActionResult GetArquetipoById(int id)
        {
            var objArq = ServiceManager<ArquetipoDAO>.Provider.GetArquetipoById(id);
            if (objArq == null)
                return NotFound();

            return Ok(objArq);
        }

        // GET: api/Arquetipo/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<ArquetipoDAO>.Provider.GetArquetipoById(Id);
            var retorno = ServiceManager<ArquetipoDAO>.Provider.CambiarEstado(Id, !entidad.Activo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        // GET: api/Arquetipo/ObtenerTecnologias/5
        [Route("ObtenerTecnologias/{id:int}")]
        [HttpGet]
		[Authorize]
		public IHttpActionResult GetTecByArquetipos(int id)
        {
            var listTec = ServiceManager<ArquetipoDAO>.Provider.GetListTecByArquetipo(id);
            if (listTec == null)
                return NotFound();

            return Ok(listTec);
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportArquetipos(string nombre, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarArquetipo(nombre, sortName, sortOrder);
            nomArchivo = string.Format("ListaArquetipo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("download")]
        [HttpGet]
        public HttpResponseMessage PostDescargar(string nombre)
        {

            if (string.IsNullOrEmpty(nombre))
            {
                throw new Exception("El nombre del archivo no debe ser vacio");
            }
            string ruta = HttpContext.Current.Server.MapPath("~/App_Data/FileServer");
            string path = Path.Combine(ruta, nombre);
            var data = new MemoryStream(File.ReadAllBytes(path));

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(data);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = nombre;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return httpResponseMessage;
        }

        [Route("GetByFiltro")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetArquetipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ArquetipoDAO>.Provider.GetArquetipoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ExisteArquetipo")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ExisteArquetipo(int Id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ArquetipoDAO>.Provider.ExisteArquetipoById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ObtenerCodigoInterno")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostObtenerCodigoInterno()
        {
            HttpResponseMessage response = null;
            var listTipoArquetipo = ServiceManager<TipoArquetipoDAO>.Provider.GetTipoArquetipoByFiltro(null);
            var listEntorno = ServiceManager<EntornoDAO>.Provider.GetEntornoByFiltro(null);
            //var listEntorno = Utilitarios.EnumToList<ETipoRiesgo>();

            var dataRpta = new
            {
                //TipoRiesgo = listTipoRiesgo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                CodigoInterno = (int)ETablaProcedencia.CVT_Arquetipo,
                TipoArquetipo = listTipoArquetipo,
                Entorno = listEntorno
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ExisteCodigoByFiltro")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage ExisteCodigoByFiltro(string codigo, int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ArquetipoDAO>.Provider.ExisteCodigoByFiltro(codigo, id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }
    }
}
