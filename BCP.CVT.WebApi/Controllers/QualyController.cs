using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Qualy")]
    public class QualyController : ApiController
    {        
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListQualys(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualys(pag.qualyId, pag.titulo, pag.nivelSeveridad, pag.productoStr, pag.tecnologiaStr, pag.asignadas, true, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<QualyDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportListQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, bool asignadas, string sortName, string sortOrder)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarQualy(qualyId, titulo, nivelSeveridad, productoStr, tecnologiaStr, asignadas, sortName, sortOrder);
            nomArchivo = string.Format("ListaVulnerabilidades-Qualys_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListadoVulnerabilidadesPorEquipo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListQualysVulnerabilidadesPorEquipo(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysVulnerabilidadesPorEquipo(pag.qualyId, pag.equipo, pag.tipoVulnerabilidad, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<QualyDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(QualyDto))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetQualyById(int id)
        {
            var objQualy = ServiceManager<QualyDAO>.Provider.GetQualyById(id);
            if (objQualy == null)
                return NotFound();

            return Ok(objQualy);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(QualyDto))]
        [Authorize]
        public IHttpActionResult PostQualy(QualyDto registro)
        {
            var user = TokenGenerator.GetCurrentUser();
            registro.UsuarioCreacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool seGuardo = ServiceManager<QualyDAO>.Provider.SaveQualy(registro);
            return Ok(seGuardo);
        }

        [Route("ReporteConsolidadoN1")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ReporteConsolidadoN1(string qid, string equipo, string producto)
        {
            qid = qid ?? "";
            equipo = equipo ?? "";
            producto = producto ?? "";
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoNivel1(qid, equipo, producto);
            totalRows = registros.Count;

            var reader = new BootstrapTable<QualyConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ReporteConsolidadoN2")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ReporteConsolidadoN2(string qid, string equipo, string producto, bool withEquipo)
        {
            qid = qid ?? "";
            equipo = equipo ?? "";
            producto = producto ?? "";
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoNivel2(qid, equipo, producto, withEquipo);
            totalRows = registros.Count;

            var reader = new BootstrapTable<QualyConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ReporteConsolidadoN3")]
        [HttpGet]
        [Authorize]
        //public HttpResponseMessage ReporteConsolidadoN3(string qid, string equipo, string producto, bool withEquipo, string severidad)
        public HttpResponseMessage ReporteConsolidadoN3(string qid, string equipo, string producto, bool withEquipo, bool withProductoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            qid = qid ?? "";
            equipo = equipo ?? "";
            producto = producto ?? "";
            HttpResponseMessage response = null;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoNivel3(qid, equipo, producto, withEquipo, withProductoTecnologia, pageNumber, pageSize, sortName, sortOrder, out int totalRows);
            //totalRows = registros.Count;

            var reader = new BootstrapTable<QualyConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ReporteConsolidadoN4")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ReporteConsolidadoN4(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoNivel4(qid, equipo, producto, withEquipo, severidad, withProductoTecnologia);
            totalRows = registros.Count;

            var reader = new BootstrapTable<QualyConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ReporteConsolidadoN5")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ReporteConsolidadoN5(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia, string tipoVulnerabilidad, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            HttpResponseMessage response = null;
            //var totalRows = 0;
            var registros = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoNivel5(qid, equipo, producto, withEquipo, severidad, withProductoTecnologia, tipoVulnerabilidad, pageNumber, pageSize, sortName, sortOrder, out int totalRows);
            //totalRows = registros.Count;

            var reader = new BootstrapTable<QualyConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ReporteConsolidadoExportar")]
        [HttpGet]
        public IHttpActionResult ReporteConsolidadoExportar(int tipo)
        {           
            string cadenaConexionAzure = Constantes.CadenaConexionStorage;
            string nombreContenedor = ConfigurationManager.AppSettings["CONTENEDOR.QUALYS"];

            string nombreArchivo = string.Empty;
            if(tipo == (int)TipoReporteVulnerabilidades.Administradores)
                nombreArchivo = ConfigurationManager.AppSettings["PARAMETRO.QUALYS_FILENAME_REPORTE_VULN_CONSOLIDADO_ALL"];
            else if (tipo == (int)TipoReporteVulnerabilidades.Seguridad)
                nombreArchivo = "Seguridad_" + ConfigurationManager.AppSettings["PARAMETRO.QUALYS_FILENAME_REPORTE_VULN_CONSOLIDADO_ALL"];

            CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(cadenaConexionAzure);
            //CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(Constantes.CadenaConexionStorage);
            CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(nombreContenedor);

            if (container.CreateIfNotExists())
            {
                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            string extensionArchivo = Path.GetExtension(nombreArchivo);
            string nombreArchivoConExtension = Path.GetFileName(nombreArchivo);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(nombreArchivoConExtension);
            cloudBlockBlob.Properties.ContentType = extensionArchivo;
            bool existeArchivo = cloudBlockBlob.Exists();
            if (!existeArchivo)  return Ok("");
           

            //cloudBlockBlob.Properties.ContentDisposition = "Attachment;filename=Qualys-Vulnerabilidades.csv";
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
        
        [Route("ReporteConsolidadoActiveExportar")]
        [HttpGet]
        public IHttpActionResult ReporteConsolidadoActiveExportar(int tipo)
        {
            string cadenaConexionAzure = Constantes.CadenaConexionStorage;
            string nombreContenedor = ConfigurationManager.AppSettings["CONTENEDOR.QUALYS"];

            string nombreArchivo = string.Empty;
            if (tipo == (int)TipoReporteVulnerabilidades.Administradores)
                nombreArchivo = ConfigurationManager.AppSettings["PARAMETRO.QUALYS_FILENAME_REPORTE_VULN_CONSOLIDADO_ACTIVE"];
            else if (tipo == (int)TipoReporteVulnerabilidades.Seguridad)
                nombreArchivo = "Seguridad_" + ConfigurationManager.AppSettings["PARAMETRO.QUALYS_FILENAME_REPORTE_VULN_CONSOLIDADO_ACTIVE"];

            CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(cadenaConexionAzure);
            //CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(Constantes.CadenaConexionStorage);
            CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(nombreContenedor);


            if (container.CreateIfNotExists())
            {
                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            string extensionArchivo = Path.GetExtension(nombreArchivo);
            string nombreArchivoConExtension = Path.GetFileName(nombreArchivo);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(nombreArchivoConExtension);
            cloudBlockBlob.Properties.ContentType = extensionArchivo;
            bool existeArchivo = cloudBlockBlob.Exists();
            if (!existeArchivo) return Ok("");

            //cloudBlockBlob.Properties.ContentDisposition = "Attachment;filename=Qualys-Vulnerabilidades-Active.csv";
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

        [Route("ConsolidadoKPI/ListarCombos")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetQualysConsolidadoKPIListarCombos()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listVulnStatus = Utilitarios.EnumToList<VulnStatus>().Select(x => new { Id = Utilitarios.GetEnumDescription2(x), Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            //var listVulnStatus = ServiceManager<QualyDAO>.Provider.GetQualysVulnStatusList();

            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
                EstadoVulnerabilidad = listVulnStatus
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ConsolidadoKPI")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetQualysConsolidadoKPI(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, int? unidadOrganizativaId, int? squadId, string equipoStr, string estadosVulnerabilidad, bool? tieneEquipoAsignado, bool? tieneProductoAsignado, bool? tieneTecnologiaAsignado)
        {
            HttpResponseMessage response = null;
            //var totalRows = 0;
            var registro = ServiceManager<QualyDAO>.Provider.GetQualysConsolidadoKPI(dominioIds, subDominioIds, productoStr, tecnologiaStr, unidadOrganizativaId, squadId, equipoStr, estadosVulnerabilidad, tieneEquipoAsignado, tieneProductoAsignado, tieneTecnologiaAsignado);
            //totalRows = registros.Count;

            //var reader = new BootstrapTable<QualyConsolidadoDto>()
            //{
            //    Total = totalRows,
            //    Rows = registros
            //};

            response = Request.CreateResponse(HttpStatusCode.OK, registro);
            return response;
        }

        [Route("VulnerabilidadesAplicacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostVulnerabilidadesAplicacion(PaginacionVulnerabilidades pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.PerfilId = user.PerfilId;
            pag.Matricula = user.Matricula;

            var totalRows = 0;
            if (pag == null)
            {
                pag = new PaginacionVulnerabilidades()
                {
                    codigoApt = string.Empty,
                    dominios = string.Empty,
                    estado = "Active",
                    pageNumber = 1,
                    pageSize = 30,
                    QID = 0,
                    subdominios = string.Empty,
                    Tecnologia = 0
                };
            }
                
            var registros = ServiceManager<QualyDAO>.Provider.GetVulnerabilidadesAplicacion(pag.Tecnologia, pag.QID, pag.subdominios, pag.dominios, pag.gestionado, pag.squads, pag.estado, pag.codigoApt, pag.pageNumber, pag.pageSize, pag.PerfilId, pag.Matricula, out totalRows);
           
            dynamic reader = new BootstrapTable<VulnerabilidadEquipoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarConsolidadoNew")]
        [HttpGet]
        public IHttpActionResult PostExportReportePublicacionAplicaciones(int Tecnologia, int QID, string subdominios, string dominios, string gestionado, string squads, string estado, string codigoApt)
        {
            var user = TokenGenerator.GetCurrentUser();
            int PerfilId = user.PerfilId;
            string Matricula = user.Matricula;

            string filename = string.Empty;
            var paginacion = new PaginacionVulnerabilidades()
            {
                Tecnologia = Tecnologia,
                QID = QID,
                subdominios= subdominios,
                dominios= dominios,
                estado= estado,
                codigoApt= codigoApt,
                PerfilId= PerfilId,
                pageNumber = 1,
                pageSize = int.MaxValue,
                gestionado = gestionado,
                Matricula = Matricula,
                squads = squads
            };
            var data = new ExportarData().ExportarReporteVulnerabilidades(paginacion);
            filename = string.Format("ReporteVulnerabilidades_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }
    }
}
