using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Grilla;
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
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.Services.Interface.TechnologyConfiguration;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/GestionUmbrales")]
    public class GestionUmbralesController : BaseController
    {
        // POST: api/Tecnologia/ListadoNew
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostObtenerUmbrales(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            var totalRows = 0;
            int flagActivo = pag.Activos ? 1 : 0;
            var registros = ServiceManager<GestionUmbralesDAO>.Provider.ObtenerUmbrales(pag.equipoId, pag.codigoAPT, pag.umbralId, flagActivo, user.Matricula, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.ValorUmbral = HttpUtility.HtmlEncode(x.ValorUmbral); });

            dynamic reader = new BootstrapTable<GestionUmbralesDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);

        }

        [Route("ListadoDetalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostUmbralesDetalle(Paginacion pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<GestionUmbralesDAO>.Provider.ObtenerUmbralesDetalle(pag.id, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<GestionUmbralesDetalleDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);

        }

        [Route("AddOrEditUmbral")]
        [HttpPost]
        [ResponseType(typeof(GestionUmbralesDTO))]
        [Authorize]
        public HttpResponseMessage PostAddOrEditUmbral(GestionUmbralesDTO entidad)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<GestionUmbralesDAO>.Provider.addOrEditUmbral(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        //[Route("ProcesarComponenteCross")]
        //[HttpPost]
        //[ResponseType(typeof(GestionUmbralesDTO))]
        //[Authorize]
        //public HttpResponseMessage PostProcesarComponenteCross(GestionUmbralesDTO entidad)
        //{
        //    HttpResponseMessage response = null;

        //    if (!ModelState.IsValid)
        //        return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

        //    int resultado = ServiceManager<GestionUmbralesDAO>.Provider.editUmbralComponenteCross(entidad);

        //    if (resultado == 0)
        //        return response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al actualizar el Componentente Cross");

        //    response = Request.CreateResponse(HttpStatusCode.OK, resultado);

        //    return response;
        //}

        [Route("ProcesarComponenteCross")]
        [HttpPost]
        [ResponseType(typeof(GestionComponenteCrossDTO))]
        [Authorize]
        public HttpResponseMessage PostProcesarComponenteCross(GestionComponenteCrossDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            //int PerfilId = user.PerfilId;
            entidad.matricula = user.Matricula;
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int resultado = ServiceManager<GestionUmbralesDAO>.Provider.editUmbralComponenteCross2(entidad);

            if (resultado == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al actualizar el Componentente Cross");

            response = Request.CreateResponse(HttpStatusCode.OK, resultado);

            return response;
        }

        [Route("downloadEvidenciaUmbral")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetdownloadEvidenciaUmbral(int equipoId, string codigoApt, int idumb)
        {
            var user = TokenGenerator.GetCurrentUser();
            var totalRows = 0;
            var registros = ServiceManager<GestionUmbralesDAO>.Provider.ObtenerUmbrales(equipoId, codigoApt, idumb, 1, user.Matricula, 1, int.MaxValue, "", "", out totalRows);
            var data = new MemoryStream(registros[0].ArchivoEvidencia);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(data);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = registros[0].RefEvidencia;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        [Route("AddOrEditUmbralArchivo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAddOrEditUmbralArchivo()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty;
                byte[] _contenido = null;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile1 = null;
                    clientFile1 = request.Files["Evidencia"];
                    if (clientFile1 != null)
                    {
                        _nombre = new FileInfo(clientFile1.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                        {
                            _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                        }
                    }
                }

                int UmbralId = Convert.ToInt32(request.Params["UmbralId"]);
                if (UmbralId >= 0)
                {

                    GestionUmbralesDTO param = new GestionUmbralesDTO();
                    param.UmbralId = UmbralId;
                    param.ArchivoEvidencia = _contenido;

                    ServiceManager<GestionUmbralesDAO>.Provider.addOrEditUmbralArchivo(param);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //X
        [Route("ListadoTecnologiaCross")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListTecnologiaCross(PaginacionNewTec pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasGUmbrales(pag.prodId,
                                                                          pag.domIds,
                                                                          pag.subdomIds,
                                                                          pag.nombre,
                                                                          pag.aplica,
                                                                          pag.codigo,
                                                                          pag.dueno,
                                                                          //pag.equipo,
                                                                          pag.tipoTecIds,
                                                                          pag.estObsIds,
                                                                          false,
                                                                          pag.pageNumber,
                                                                          pag.pageSize,
                                                                          pag.sortName,
                                                                          pag.sortOrder,
                                                                          out totalRows,
                                                                          pag.tribuCoeStr,
                                                                          pag.squadStr);
            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<TecnologiaG>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListaAutoCompletarTecnologia")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetListaAutoCompletarTecnologia(string descripcion, int? id = null, string dominioIds = null, string subDominioIds = null)
        {
            var registros = ServiceManager<TecnologiaDAO>.Provider.GetListaAutoCompletarTecnologia(descripcion, id, dominioIds, subDominioIds);
            if (registros == null)
                return NotFound();
            return Ok(registros);
        }

        [Route("ListaAutoCompletarProducto")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetListaAutoCompletarProducto(string descripcion, string dominioIds = null, string subDominioIds = null)
        {
            var registros = ServiceManager<ProductoDAO>.Provider.GetListaAutoCompletarProducto(descripcion, dominioIds, subDominioIds);
            if (registros == null)
                return NotFound();
            return Ok(registros);
        }




        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var listTipos = ServiceManager<GestionUmbralesDAO>.Provider.GetTiposByFiltro("");
            var listTipoValor = ServiceManager<GestionUmbralesDAO>.Provider.GetTipoValorByFiltro("");
            var listDriver = ServiceManager<GestionUmbralesDAO>.Provider.GetDriverByFiltro("");
            var listDriverUM = ServiceManager<GestionUmbralesDAO>.Provider.GetDriverUMByFiltro("");


            var dataRpta = new
            {
                Tipos = listTipos,
                TipoValor = listTipoValor,
                Driver = listDriver,
                DriverUM = listDriverUM
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Reportes/Umbrales/Exportar")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ExportarUmbrales(ExportRequestGU request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string filename = string.Empty;
            var data = new ExportarData().ExportarReporteUmbrales(request.EquipoId, request.CodigoAPT);
            filename = string.Format("ReporteUmbrales_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            return Ok(new { excel = data, name = filename });
        }

        [Route("GetParametroArchivoAceptado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetParametroArchivoAceptado()
        {
            HttpResponseMessage response = null;
            var paramCD = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_UMBRALES_EXTENSIONES_ARCHIVOS_EVIDENCIA").Valor;

            var dataRpta = new
            {
                ArchivosAceptados = paramCD,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;

        }

        [Route("GetParametroOwnerActivaODesactiva")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetParametroOwnerActivaODesactiva()
        {
            HttpResponseMessage response = null;
            var paramCD = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_UMBRALES_OPCIONES_OWNER_TEC").Valor;

            var dataRpta = new
            {
                EstadoOwnerModificaCross = paramCD,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;

        }

        //NUEVO
        [Route("ListaAutoCompletarAppApis")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetListaAutoCompletarAppApis(string descripcion, int? id = null, string dominioIds = null, string subDominioIds = null)
        {
            var registros = ServiceManager<GestionUmbralesDAO>.Provider.GetAppsApisByFiltro(descripcion);
            if (registros == null)
                return NotFound();
            return Ok(registros);
        }


        [Route("ListadoAppApisCross")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListAppsApisCross(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            int PerfilId = user.PerfilId;
            string RolPerfil = user.Perfil;
            string Usuario = user.Matricula;


            var totalRows = 0;
            var registros = ServiceManager<GestionUmbralesDAO>.Provider.GetListAplicacionesApisCross(
                pag.codigoAPT, Usuario, PerfilId, RolPerfil, pag.tipoCodigo.ToString(), pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();


            dynamic reader = new BootstrapTable<AplicacionApisDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("AddOrEditPeak")]
        [HttpPost]
        [ResponseType(typeof(GestionPeakDTO))]
        [Authorize]
        public HttpResponseMessage PostAddOrEditPeak(GestionPeakDTO entidad)
        {
            HttpResponseMessage response = null;
            var user = TokenGenerator.GetCurrentUser();
            //int PerfilId = user.PerfilId;
            entidad.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            int entidadId = ServiceManager<GestionUmbralesDAO>.Provider.addOrEditPeak(entidad);

            if (entidadId == 0)
                return response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entidad no encontrada");

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);

            return response;
        }

        [Route("ListadoPeak")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostObtenerPeak(Paginacion pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<GestionUmbralesDAO>.Provider.ObtenerPeak(pag.codigoAPT, pag.equipoId, pag.Activos, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ValorPeak = HttpUtility.HtmlEncode(x.ValorPeak);
                x.DetallePeak = HttpUtility.HtmlEncode(x.DetallePeak);
            });

            dynamic reader = new BootstrapTable<GestionPeakDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);

        }



    }

}
