using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.CargaMasiva;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.DTO.Custom;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/TecnologiaNoRegistrada")]
    public class TecnologiaNoRegistradaController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListTecNoReg(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecNoRegVista(pag.nombre, pag.id, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.Tecnologia = HttpUtility.HtmlEncode(x.Tecnologia);
            });

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

        [Route("ListadoEquiposNoRegistrados")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListEquipoNoReg(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetEquipoNoRegSP(pag.nombre, pag.motivo, pag.origen, pag.flagAprobado, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

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

        [Route("AsociarTecnologias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAsocTecNoReg(Parametro request)
        {
            HttpResponseMessage response = null;

            var retorno = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.AsociarTecNoReg(request.id, request.items, request.UsuarioCreacion, request.UsuarioModificacion);
            if (retorno)
                response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("AsociarTecnologias2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAsociarTecnologias(ObjParametro request)
        {
            var user = TokenGenerator.GetCurrentUser();
            request.UsuarioCreacion = user.Matricula;
            request.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            var retorno = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.AsociarTecnologiasNoRegistradas(request.id, request.itemsTec, request.UsuarioCreacion, request.UsuarioModificacion);
            if (retorno)
                response = Request.CreateResponse(HttpStatusCode.OK, retorno);

            return response;
        }

        [Route("ListarSubdominiosSugeridos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubdominiosSugeridos(string nombre)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetSubdominiosSugeridos(nombre);
            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("ObtenerDominioSubdominioSugerido")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetDominioSubdominioSugerido(string nombre)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetSubdominiosSugeridos(nombre);
            var SubdominioId = retorno[0];
            var DominioId = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetDominioIdBySubdominioId(SubdominioId);

            var dataRpta = new
            {
                DominioId,
                SubdominioId,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListarTecnologiasSugeridas")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListTecSugeridas(PaginacionTecSug pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecSugeridas(pag.nombre, pag.subIds, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

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

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var listTipoEquipo = ServiceManager<EquipoDAO>.Provider.GetTipoEquipoByFiltro(null);
            var listTipoTec = ServiceManager<TipoDAO>.Provider.GetAllTipoByFiltro(null);
            var listaEquivalencia = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecnologiasUnicas();
            var listSubdominio = ServiceManager<SubdominioDAO>.Provider.GetSubdominiosMultiSelect();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetDomConSubdom();

            var dataRpta = new
            {
                TipoEquipo = listTipoEquipo,
                TipoTec = listTipoTec,
                Subdominio = listSubdominio,
                Dominio = listDominio,
                Equivalencias = listaEquivalencia
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult PostExportar(string tecnologia, int tipoEquipoId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(tecnologia)) tecnologia = "";
            var data = new ExportarData().ExportarTecnologiasNoRegistradas(tecnologia, tipoEquipoId, sortName, sortOrder);
            nomArchivo = string.Format("ListaTecnologiasNoRegistradas_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarEquipos")]
        [HttpGet]
        public IHttpActionResult PostExportarEquipos(string nombre, int motivo, int origen, int? flagAprobado, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarEquiposNoRegistrados(nombre ?? "", motivo, origen, flagAprobado, sortName, sortOrder);
            nomArchivo = string.Format("ListaEquiposNoRegistrados_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GetByEquipoId")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTecnologiaNoRegistradaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            int totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecnologiaNoRegistradaByEquipoId(equipoId, pageNumber, pageSize, sortName, sortOrder, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("ObtenerPlantillaTecnologiaNoRegistrada")]
        [HttpGet]
        public IHttpActionResult PostPlantillaEquipos()
        {
            string filename = "PlantillaCargaTecnologiasNoRegistradas.xlsx";
            var data = new ExportarData().ObtenerPlantillaExcelByName(filename);

            return Ok(new { excel = data, name = filename });
        }

        [Route("CargarTecnologiasNoRegistradas")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCargarEquipos()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    EstadoCargaMasiva estadoCM = null;
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);

                    estadoCM = new CargaData().CargaMasivaTecnologiasNoRegistradas(extension, inputStream);
                    response = Request.CreateResponse(HttpStatusCode.OK, estadoCM);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //[Route("GetUpdateEquipoNoRegistradoFlagAprobado")]
        //[HttpGet]
        //[Authorize]
        //public HttpResponseMessage GetUpdateEquipoNoRegistradoFlagAprobado(int equipoNoRegistradoId, string equipoStr, string usuario, bool flagAprobado)
        //{
        //    bool seActualizo = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.UpdateFlagAprobadoEquipoNoRegSP(equipoNoRegistradoId, flagAprobado);

        //    if (seActualizo && flagAprobado)
        //    {
        //        var registro = new EquipoNoRegistradoDto();
        //        registro.NombreEquipo = equipoStr;
        //        registro.creadoPor = usuario;
        //        seActualizo = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.AddEquipoFromEquipoNoRegistrado(registro);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, seActualizo);
        //}

        //[Route("ListadoEquipoTecnologiasNoRegistradasQualys")]
        //[HttpPost]
        //[Authorize]
        //public HttpResponseMessage PostListEquipoTecnologiaNoRegQualys(Paginacion pag)
        //{
        //    HttpResponseMessage response = null;
        //    var totalRows = 0;
        //    var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetEquipoTecnologiaNoRegQualysSP(pag.equipoStr, pag.tecnologiaStr, pag.flagAprobadoEquipo, pag.flagAprobado, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

        //    if (registros != null)
        //    {
        //        var dataRpta = new
        //        {
        //            Total = totalRows,
        //            Rows = registros
        //        };
        //        response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
        //    }
        //    return response;
        //}

        //[Route("ExportarEquipoTecnologiasNoRegistradasQualys")]
        //[HttpGet]
        //public HttpResponseMessage PostExportarEquipoTecnologiasNoRegistradasQualys(string equipoStr, string tecnologiaStr, int? flagAprobadoEquipo, int? flagAprobado, string sortName, string sortOrder)
        //{
        //    string nomArchivo = "";
        //    var data = new ExportarData().ExportarEquipoTecnologiasNoRegistradasQualys(equipoStr, tecnologiaStr, flagAprobadoEquipo, flagAprobado, sortName, sortOrder);
        //    nomArchivo = string.Format("ListaEquipoTecnologiasNoRegistradasQualys_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}

        //[Route("ListadoTecnologiasNoRegistradasQualys")]
        //[HttpPost]
        //[Authorize]
        //public HttpResponseMessage PostListTecnologiaNoRegQualys(Paginacion pag)
        //{
        //    HttpResponseMessage response = null;
        //    var totalRows = 0;
        //    var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecnologiaNoRegQualysSP(pag.tecnologiaStr, pag.flagAprobadoEquipo, pag.flagAprobado, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows, equipoId: pag.equipoId);

        //    if (registros != null)
        //    {
        //        var dataRpta = new
        //        {
        //            Total = totalRows,
        //            Rows = registros
        //        };
        //        response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
        //    }
        //    return response;
        //}

        //[Route("GetUpdateTecnologiaNoRegistradaQualysFlagAprobado")]
        //[HttpGet]
        //[Authorize]
        //public HttpResponseMessage GetUpdateTecnologiaNoRegistradaQualysFlagAprobado(int equipoId, string qualyIds, int tecnologiaId, string usuario, bool flagAprobado)
        //{
        //    bool seActualizo = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.UpdateFlagAprobadoTecnologiaNoRegQualysSP(equipoId, qualyIds, tecnologiaId, flagAprobado);

        //    if(seActualizo && flagAprobado)
        //    {
        //        var registro = new TecnologiaNoRegistradaQualysDto();
        //        registro.EquipoId = equipoId;
        //        registro.TecnologiaId = tecnologiaId;
        //        registro.creadoPor = usuario;
        //        seActualizo = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.AddTecnologiaToEquipo(registro);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, seActualizo);
        //}

        //[Route("ExportarTecnologiasNoRegistradasQualys")]
        //[HttpGet]
        //public HttpResponseMessage PostExportarTecnologiasNoRegistradasQualys(int equipoId, string tecnologiaStr, int? flagAprobadoEquipo, int? flagAprobado, string sortName, string sortOrder)
        //{
        //    string nomArchivo = "";
        //    var data = new ExportarData().ExportarTecnologiasNoRegistradasQualys(equipoId, tecnologiaStr, flagAprobadoEquipo, flagAprobado, sortName, sortOrder);
        //    nomArchivo = string.Format("ListaTecnologiasNoRegistradasQualys_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}

        //[Route("ExportarTecnologiasNoRegistradasDetalladoQualys")]
        //[HttpGet]
        //public HttpResponseMessage PostExportarTecnologiasNoRegistradasDetalladoQualys(string equipoStr, string tecnologiaStr, int? flagAprobadoEquipo, int? flagAprobado, string sortName, string sortOrder)
        //{
        //    string nomArchivo = "";
        //    var data = new ExportarData().ExportarTecnologiasNoRegistradasDetalladoQualys(equipoStr, tecnologiaStr, flagAprobadoEquipo, flagAprobado, sortName, sortOrder);
        //    nomArchivo = string.Format("ListaTecnologiasNoRegistradasQualys_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}

        [Route("ListadoTecnologiasNoRegistradasQualysXEquipo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListTecnologiaNoRegQualysXEquipo(Paginacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaNoRegistradaDAO>.Provider.GetTecnologiaNoRegQualysXEquipoSP(pag.equipoId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

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

        [Route("ExportarTecnologiasNoRegistradasQualysXEquipo")]
        [HttpGet]
        public IHttpActionResult PostExportarTecnologiasNoRegistradasQualysXEquipo(int equipoId, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarTecnologiasNoRegistradasQualysXEquipo(equipoId, sortName, sortOrder);
            nomArchivo = string.Format("VulnerabilidadesEquipo_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ReporteDetalladoExportar")]
        [HttpGet]
        public IHttpActionResult ReporteDetalladoExportar()
        {
            try
            {
                string cadenaConexionAzure = Constantes.CadenaConexionStorage;
                string nombreContenedor = ConfigurationManager.AppSettings["CONTENEDOR.TECNOLOGIANOREGISTRADA"];
                string nombreArchivo = ConfigurationManager.AppSettings["PARAMETRO.NOMBRE_ARCHIVO_TECNOREGISTRADA"];

                CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(cadenaConexionAzure);
                //CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(Constantes.CadenaConexionStorage);
                CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(nombreContenedor);

                //if (container.CreateIfNotExists())
                //{
                //    container.SetPermissionsAsync(new BlobContainerPermissions
                //    {
                //        PublicAccess = BlobContainerPublicAccessType.Blob
                //    });
                //}

                string extensionArchivo = Path.GetExtension(nombreArchivo);
                string nombreArchivoConExtension = Path.GetFileName(String.Format(nombreArchivo, DateTime.Now.ToString("yyyyMMdd")));

                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(nombreArchivoConExtension);
                cloudBlockBlob.Properties.ContentType = extensionArchivo;
                bool existeArchivo = cloudBlockBlob.Exists();

                if (!existeArchivo)
                {
                    var cantidad = container.ListBlobs().OfType<CloudBlockBlob>().Count();
                    if (cantidad == 0)
                    {
                        return BadRequest("No existe archivo generado");
                    }
                    cloudBlockBlob = container.ListBlobs().OfType<CloudBlockBlob>().OrderByDescending(x => x.Properties.LastModified).ToList().FirstOrDefault();
                    nombreArchivoConExtension = cloudBlockBlob.Name;
                }

                var sasConstraints = new SharedAccessBlobPolicy();
                sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
                sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1);
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

                var sasBlobToken = cloudBlockBlob.GetSharedAccessSignature(sasConstraints);

                byte[] data;

                using (var ms = new MemoryStream())
                {
                    cloudBlockBlob.DownloadToStream(ms);
                    data = ms.ToArray();
                }
                return Ok(new { data = data, name = nombreArchivoConExtension });
            }
            catch (Exception e)
            {
                log.DebugFormat("error : {0}", e.Message.ToString());
                throw e;
            }
        }
    }

}
