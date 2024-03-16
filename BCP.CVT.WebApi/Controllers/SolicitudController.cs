using BCP.CVT.Cross;
using BCP.CVT.DTO;
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
using BCP.PAPP.Common.Dto;
using BCP.PAPP.Common.Cross;
using Newtonsoft.Json;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Solicitud")]
    public class SolicitudController : BaseController
    {
        // POST: api/Solicitud
        [Route("")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostSolicitud(SolicitudDto solicitudDto)
        {
            var user = TokenGenerator.GetCurrentUser();
            solicitudDto.UsuarioCreacion = user.Matricula;
            solicitudDto.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int idSolicitud = ServiceManager<SolicitudAplicacionDAO>.Provider.AddOrEdit(solicitudDto);

            if (idSolicitud == 0)
                return Ok(idSolicitud);

            return Ok(idSolicitud);
        }

        // POST: api/Solicitud/Listado
        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitud(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudes(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Observaciones = HttpUtility.HtmlEncode(x.Observaciones); });

            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        [Route("Listado2")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitud2(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudes2(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Observaciones = HttpUtility.HtmlEncode(x.Observaciones);
                x.NombreAplicacion = HttpUtility.HtmlEncode(x.NombreAplicacion);
                x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
            });

            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Listado3")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitud3(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudes3(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.NombreAplicacion = HttpUtility.HtmlEncode(x.NombreAplicacion);
                x.Observaciones = HttpUtility.HtmlEncode(x.Observaciones);
                x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
            });

            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Listado/Detalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitudDetalle(PaginacionSolicitud pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ValorAnterior = HttpUtility.HtmlEncode(x.ValorAnterior);
                x.NuevoValor = HttpUtility.HtmlEncode(x.NuevoValor);
                x.DetalleActual = HttpUtility.HtmlEncode(x.DetalleActual);
                x.DetalleNuevo = HttpUtility.HtmlEncode(x.DetalleNuevo);
            });

            var reader = new BootstrapTable<SolicitudDetalleDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Listado/DetalleEliminacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitudDetalleEliminacion(PaginacionSolicitud pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesDetalleEliminacion(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<SolicitudDetalleDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Rechazar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRechazarSolicitud(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.registerByEmail = user.CorreoElectronico;
            obj.NombreUsuarioAprobacion = user.Nombres;

            HttpResponseMessage response = null;
            ServiceManager<SolicitudAplicacionDAO>.Provider.RechazarSolicitud(obj.AppId, obj.comments, obj.registerBy, obj.NombreUsuarioAprobacion);

            response = Request.CreateResponse(HttpStatusCode.OK, "200");

            return response;
        }

        [Route("Desestimar")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostDesestimarSolicitud(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.registerByEmail = user.CorreoElectronico;
            obj.NombreUsuarioModificacion = user.Nombres;

            HttpResponseMessage response = null;
            ServiceManager<SolicitudAplicacionDAO>.Provider.DesestimarSolicitud(obj.AppId, obj.comments, obj.registerBy, obj.NombreUsuarioModificacion);

            response = Request.CreateResponse(HttpStatusCode.OK, "200");

            return response;
        }

		//[Route("Aprobar")]
		//[HttpPost]
		//[Authorize]
		//public HttpResponseMessage PostAprobarSolicitud(ApplicationDto obj)
		//{
		//	HttpResponseMessage response = null;
		//	ServiceManager<SolicitudAplicacionDAO>.Provider.AprobarSolicitud(obj.AppId, obj.registerBy, obj.NombreUsuarioAprobacion);

		//	response = Request.CreateResponse(HttpStatusCode.OK, "200");

		//	return response;
		//}

		[Route("Aprobar")]
		[HttpPost]
		[Authorize]
		public HttpResponseMessage PostAprobarSolicitud()
		{
			try
			{
				HttpResponseMessage response = null;
				HttpRequest request = HttpContext.Current.Request;

                var user = TokenGenerator.GetCurrentUser();

				var obj = JsonConvert.DeserializeObject<ApplicationDto>(request.Form["data"]);
                obj.registerBy = user.Matricula;
                obj.registerByEmail = user.CorreoElectronico;
                obj.NombreUsuarioAprobacion = user.Nombres;

				if (obj != null)
				{

					ServiceManager<SolicitudAplicacionDAO>.Provider.AprobarSolicitud(obj.AppId, obj.registerBy, obj.NombreUsuarioAprobacion);

					int _solicitudId = obj.AppId;
					if (_solicitudId >= 0)
					{
						if (request.Files.Count > 0)
						{
							string _nombre = string.Empty;
							byte[] _contenido = null;


							HttpPostedFile clientFile1 = null;
							clientFile1 = request.Files["File"];
							_nombre = new FileInfo(clientFile1.FileName).Name;
							using (var binaryReader = new BinaryReader(clientFile1.InputStream))
							{
								_contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
							}

							SolicitudArchivosDTO objRegistro = new SolicitudArchivosDTO();
							objRegistro.IdSolicitud = _solicitudId;

							objRegistro.ConformidadGST = _contenido;
							objRegistro.NombreConformidadGST = _nombre;

							ServiceManager<ApplicationDAO>.Provider.SubirArchivosRemove(objRegistro);

						}

					}
					response = Request.CreateResponse(HttpStatusCode.OK, true);
				}

				return response;
			}
			catch (Exception e)
			{
				throw e;
			}
		}




		// POST: api/Solicitud/Upload
		[Route("Upload")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _nombre = string.Empty;
                byte[] _contenido = null;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    _nombre = clientFile.FileName;
                    using (var binaryReader = new BinaryReader(clientFile.InputStream))
                    {
                        _contenido = binaryReader.ReadBytes(clientFile.ContentLength);
                    }

                }

                int _solicitudId = Convert.ToInt16(request.Params["SolicitudId"]);
                if (_solicitudId >= 0)
                {
                    ServiceManager<SolicitudAplicacionDAO>.Provider.AddArchivo(_solicitudId, _contenido, _nombre);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("Download")]
        [HttpGet]
        public IHttpActionResult PostDownload(int id)
        {
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetContenidoFileSolicitudById(id);
            var data = new MemoryStream(entidad.ArchivosAsociados);

            return Ok(new { data = data.ToArray(), name = entidad.NombreArchivos });
        }

        [Route("DownloadArchivoEliminacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadArchivoEliminacion(int id, int tipoArchivo)
        {
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetContenidoFileSolicitudEliminacionById(id);

            var nombreArchivo = string.Empty;

            MemoryStream data = null;
            if (tipoArchivo == 1 && entidad!= null && entidad.NombreConformidadGST!=null)
            {
                data = new MemoryStream(entidad.ConformidadGSTFile);
                nombreArchivo = entidad.NombreConformidadGST;
            }
            else if (tipoArchivo == 2 && entidad != null && entidad.NombreTicketEliminacion != null)
            {
                data = new MemoryStream(entidad.TicketEliminacionFile);
                nombreArchivo = entidad.NombreTicketEliminacion;
            }
            else if (tipoArchivo == 3 && entidad != null && entidad.NombreRatificacion != null)
            {
                data = new MemoryStream(entidad.RatificacionFile);
                nombreArchivo = entidad.NombreRatificacion;
            }

            return Ok(new { data = data.ToArray(), name = nombreArchivo});
        }

        [Route("DownloadArchivoBitacora")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadArchivoBitacora(int id)
        {
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetContenidoFileBitacoraById(id);

            var nombreArchivo = string.Empty;

            MemoryStream data = null;
            data = new MemoryStream(entidad.ConformidadGSTFile);
            nombreArchivo = entidad.NombreConformidadGST;

            return Ok(new { data = data.ToArray(), name = nombreArchivo });

        }

        [Route("GetSolicitudArchivoAprobadoById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSolicitudArchivoAprobadoById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetContenidoFileSolicitudEliminacionById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad == null ? false : true);

            return response;
        }
        [Route("GetSolicitudById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSolicitudById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;
            var lTipoSolicitudApp = Utilitarios.EnumToList<ETipoSolicitudAplicacion>();
            var lEstadoSolicitudApp = Utilitarios.EnumToList<EstadoSolicitud>();
            var _lModeloEntrega = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.ModeloEntrega),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();
            var _ClasificacionTecnica = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.ClasificacionTecnica),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

            //var _lPlataformaBCP = Utilitarios.EnumToList<EPlataformaBCP>();
            var _lAreaBian = ServiceManager<ActivosDAO>.Provider.GetAreaBianByFiltro(null).Select(x => x.Descripcion).ToArray();
            var _lJefaturaAti = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null).Select(x => x.Descripcion).ToArray();

            var dataRpta = new
            {
                ModeloEntrega = _lModeloEntrega,
                ClasificacionTecnica = _ClasificacionTecnica,
                Tipo = lTipoSolicitudApp.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                Estado = lEstadoSolicitudApp.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                //Plataforma = _lPlataformaBCP.Select(x => Utilitarios.GetEnumDescription2(x)).ToArray(),
                AreaBian = _lAreaBian,
                JefaturaAti = _lJefaturaAti
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("CambiarEstado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(ParametroEstadoSolicitud entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.CambiarEstadoSolicitud(entidad.Id, entidad.TipoSolicitudId, entidad.EstadoSolicitudId, entidad.MotivoComentario, entidad.Observacion, entidad.UsuarioModificacion, entidad.BandejaId);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GetColumnaAplicacionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaAplicacionByFiltro(string filtro, string IdsTipoFlujo = null)
        {
            HttpResponseMessage response = null;
            var lTablaProcedencia = string.Format("{0};{1};{2}",
                (int)ETablaProcedenciaAplicacion.Aplicacion,
                (int)ETablaProcedenciaAplicacion.AplicacionDetalle,
                (int)ETablaProcedenciaAplicacion.InfoCampoPortafolio);

            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.GetColumnaAplicacionByFiltro(filtro, lTablaProcedencia, IdsTipoFlujo);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GetColumnaModuloByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetColumnaModuloByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.GetColumnaModuloByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GetProveedorDesarrolloByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetProveedorByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProveedorDesarrolloByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("AddOrEditSolicitudComentarios")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostSolicitudComentarios(SolicitudComentariosDTO entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;

            int entidadId = ServiceManager<SolicitudAplicacionDAO>.Provider.AddOrEditSolicitudComentarios(entidad);

            response = Request.CreateResponse(HttpStatusCode.OK, entidadId);
            return response;
        }

        [Route("ListadoSolicitudComentarios")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListSolicitudComentarios(PaginacionSolicitud pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudComentarios(pag, out totalRows);

            var retorno = new BootstrapTable<SolicitudComentariosDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GetModuloByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetModuloByFiltro(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.GetModuloByCodigoAPT(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ListadoAprobadores")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListadoAprobadores(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesAprobador(pag, out totalRows);
            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("CambiarEstadoSolicitudAprobador")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage CambiarEstadoSolicitudAprobador(ParametroEstadoSolicitud entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.UsuarioCreacion = user.Matricula;
            entidad.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.CambiarEstadoSolicitudAprobadores(entidad.Id, entidad.EstadoSolicitudId, entidad.BandejaId, entidad.Observacion, entidad.UsuarioModificacion);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ExisteCambioEstadoSolicitudAprobadores")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteCambioEstadoSolicitudAprobadores(int id_solicitud, int id_bandeja)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.ExisteCambioEstadoSolicitudAprobadores(id_solicitud, id_bandeja);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("UpdateAplicacionByBandejaAprobador")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpdateAplicacionByBandejaAprobador(UpdateAplicacionBandeja entidad)
        {
            var user = TokenGenerator.GetCurrentUser();
            entidad.Usuario = user.Matricula;

            HttpResponseMessage response = null;
            bool retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.UpdateAplicacionByBandeja(entidad);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("GetAplicacionBandejaById")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionBandejaById(int Id)
        {
            HttpResponseMessage response = null;
            var entidad = ServiceManager<SolicitudAplicacionDAO>.Provider.GetAplicacionBandejaById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, entidad);

            return response;
        }

        [Route("GetTipoFlujoByAplicacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTipoFlujoByAplicacion(string codigoAPT)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.GetTipoFlujoByAplicacion(codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ExisteSolicitudByCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteSolicitudByCodigoAPT(string filtro, int id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<SolicitudAplicacionDAO>.Provider.ExisteSolicitudByCodigoAPT(filtro, id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ExportarBandejaAprobadores")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostExportar(int BandejaId, int TipoSolicitud, string EstadoSolicitud, DateTime? FechaDesde, DateTime? FechaHasta, string ModeloEntrega, int PerfilId, string Matricula, string sortName, string sortOrder)
        {
            string filename = "";
            var lEstadoSolicitud = new List<int>();
            if (!string.IsNullOrEmpty(EstadoSolicitud)) lEstadoSolicitud = EstadoSolicitud.Split('|').Select(int.Parse).ToList();
            string bandejaStr = Utilitarios.GetEnumDescription2((EBandejaAprobadorAplicacion)BandejaId).Replace(" ", string.Empty);
            var data = new ExportarData().ExportarBandejaAprobadores(BandejaId, TipoSolicitud, lEstadoSolicitud, FechaDesde, FechaHasta, ModeloEntrega, PerfilId, Matricula, sortName, sortOrder);
            filename = string.Format("ListadoBandeja{1}_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"), bandejaStr);

            return Ok(new { excel = data, name = filename });
        }

        [Route("ExportarBandejaAdministrador")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostExportarBandejaAdministrador(int TipoSolicitud, string EstadoSolicitud, DateTime? FechaDesde, DateTime? FechaHasta, int PerfilId, string Matricula, string sortName, string sortOrder)
        {
            string filename = "";
            var lEstadoSolicitud = new List<int>();
            if (!string.IsNullOrEmpty(EstadoSolicitud)) lEstadoSolicitud = EstadoSolicitud.Split('|').Select(int.Parse).ToList();
            var data = new ExportarData().ExportarBandejaAdministrador(TipoSolicitud, lEstadoSolicitud, FechaDesde, FechaHasta, PerfilId, Matricula, sortName, sortOrder);
            filename = string.Format("ListadoBandejaAdministrador_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ExportarSolicitudesModificacion")]
        [HttpGet]
        public IHttpActionResult ExportarSolicitudesCreacion(string codigoApt, int estadoSolicitudUnico, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            var matricula = user.Matricula;
            string nomArchivo = "";
            if (string.IsNullOrEmpty(codigoApt)) codigoApt = null;
            var data = new ExportarData().ExportarSolicitudesModificacion(matricula, codigoApt, estadoSolicitudUnico, sortName, sortOrder);
            nomArchivo = string.Format("Solicitudes_Modificacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
        [Route("ExportarSolicitudesEliminacion")]
        [HttpGet]
        public IHttpActionResult ExportarSolicitudesEliminacion(string codigoApt, int estadoSolicitudUnico, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            var matricula = user.Matricula;
            string nomArchivo = "";
            if (string.IsNullOrEmpty(codigoApt)) codigoApt = null;
            var data = new ExportarData().ExportarSolicitudesEliminacion(matricula, codigoApt, estadoSolicitudUnico, sortName, sortOrder);
            nomArchivo = string.Format("Solicitudes_Eliminacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
    }
}
