using BCP.CVT.Cross;
using BCP.CVT.DTO;
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
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using System.Data.SqlClient;
using System.Data;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/File")]
    public class FileController : BaseController
    {
        [Route("upload")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty, _usuarioCreacion = string.Empty;
                string _nombreRef, _descripcionRef = string.Empty;
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

                int _archivoId = Convert.ToInt16(request.Params["ArchivoId"]);
                if (_archivoId >= 0)
                {
                    int _codIntTablaProcedencia = 0;
                    _codIntTablaProcedencia = Convert.ToInt16(request.Params["TablaProcedencia"]);
                    _entidadId = request.Params["EntidadId"];
                    var user = TokenGenerator.GetCurrentUser();
                    _usuarioCreacion = user.Matricula;

                    _nombreRef = request.Params["NombreRef"];
                    _descripcionRef = request.Params["DescripcionRef"];

                    ArchivosCvtDTO objRegistro = new ArchivosCvtDTO();
                    objRegistro.Id = _archivoId;
                    objRegistro.TablaProcedenciaId = _codIntTablaProcedencia;
                    objRegistro.EntidadId = _entidadId;
                    objRegistro.Activo = _contenido != null;
                    objRegistro.Contenido = _contenido;
                    objRegistro.UsuarioCreacion = _usuarioCreacion;
                    objRegistro.UsuarioModificacion = _usuarioCreacion;
                    objRegistro.Nombre = _nombre;
                    objRegistro.NombreRef = _nombreRef;
                    objRegistro.DescripcionRef = _descripcionRef;

                    int Id = ServiceManager<ArchivosCvtDAO>.Provider.AddOrEdit(objRegistro);
                    bool estado = Id > 0;
                    response = Request.CreateResponse(HttpStatusCode.OK, estado);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("upload2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload2()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty, _nombre2 = string.Empty, _nombre3 = string.Empty;
                byte[] _contenido = null;
                byte[] _contenido2 = null;
                byte[] _contenido3 = null;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile1 = null;
                    clientFile1 = request.Files["File1"];
                    if (clientFile1!=null) {
                        _nombre = new FileInfo(clientFile1.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                        {
                            _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                        }
                    }
                    HttpPostedFile clientFile2 = null;
                    clientFile2 = request.Files["File2"];
                    if (clientFile2 != null)
                    {
                        _nombre2 = new FileInfo(clientFile2.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile2.InputStream))
                        {
                            _contenido2 = binaryReader.ReadBytes(clientFile2.ContentLength);
                        }
                    }
                    HttpPostedFile clientFile3 = null;
                    clientFile3 = request.Files["File3"];
                    if (clientFile3 != null)
                    {
                        _nombre3 = new FileInfo(clientFile3.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile3.InputStream))
                        {
                            _contenido3 = binaryReader.ReadBytes(clientFile3.ContentLength);
                        }
                    }

                }

                int _solicitudId = Convert.ToInt16(request.Params["SolicitudAplicacionId"]);
                if (_solicitudId >= 0)
                {


                    SolicitudArchivosDTO objRegistro = new SolicitudArchivosDTO();
                    objRegistro.IdSolicitud = _solicitudId;

                    objRegistro.ConformidadGST = _contenido;
                    objRegistro.TicketEliminacion = _contenido2;
                    objRegistro.Ratificacion = _contenido3;
                    objRegistro.NombreConformidadGST = _nombre;
                    objRegistro.NombreTicketEliminacion = _nombre2;
                    objRegistro.NombreRatificacion = _nombre3;

                  ServiceManager<ApplicationDAO>.Provider.SubirArchivosRemove(objRegistro);
                   
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("uploadConstanciaActivoTSI")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUploadConstanciaActivoTSI()
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
                    clientFile1 = request.Files["Constancia"];
                    if (clientFile1 != null)
                    {
                        _nombre = new FileInfo(clientFile1.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                        {
                            _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                        }
                    }                                       
                }

                int _solicitudId = Convert.ToInt32(request.Params["EquipoSolicitudId"]);
                if (_solicitudId >= 0)
                {
                    ServiceManager<ApplianceDAO>.Provider.ActualizarArchivo(_solicitudId, _contenido, _nombre);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("uploadNuevoActivoTSI")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUploadNuevoActivoTSI()
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
                    clientFile1 = request.Files["Constancia"];
                    if (clientFile1 != null)
                    {
                        _nombre = new FileInfo(clientFile1.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                        {
                            _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                        }
                    }
                }

                int equipoId = Convert.ToInt32(request.Params["EquipoId"]);
                if (equipoId >= 0)
                {
                    ServiceManager<ApplianceDAO>.Provider.ActualizarArchivoSoftwareBase(equipoId, _contenido, _nombre);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("uploadNuevoActivoTSI2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUploadNuevoActivoTSI2()
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
                    clientFile1 = request.Files["Constancia"];
                    if (clientFile1 != null)
                    {
                        _nombre = new FileInfo(clientFile1.FileName).Name;
                        using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                        {
                            _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                        }
                    }
                }

                int equipoId = Convert.ToInt32(request.Params["EquipoId"]);
                if (equipoId >= 0)
                {
                    ServiceManager<ApplianceDAO>.Provider.ActualizarArchivoSoftwareBase2(equipoId, _contenido, _nombre);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("downloadConstanciaActivoTSI")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadConstanciaTSI(int id)
        {
            var objRegistro = ServiceManager<ApplianceDAO>.Provider.GetSolicitudById(id);
            var data = new MemoryStream(objRegistro.ArchivoConstancia);

            return Ok(new { data = data.ToArray(), name = objRegistro.NombreArchivo });
        }

        [Route("downloadNuevoActivoTSI")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadNuevoActivoTSI(int id)
        {
            var objRegistro = ServiceManager<ApplianceDAO>.Provider.GetArchivoById(id);
            var data = new MemoryStream(objRegistro.ArchivoConstancia);

            return Ok(new { data = data.ToArray(), name = objRegistro.NombreArchivo });
        }

        [Route("upload5")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload5()
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
                    clientFile1 = request.Files["File1"];
                    _nombre = new FileInfo(clientFile1.FileName).Name;
                    using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                    {
                        _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                    }
                  

                }

                int _solicitudId = Convert.ToInt16(request.Params["SolicitudAplicacionId"]);
                if (_solicitudId >= 0)
                {


                    SolicitudArchivosDTO objRegistro = new SolicitudArchivosDTO();
                    objRegistro.IdSolicitud = _solicitudId;

                    objRegistro.ConformidadGST = _contenido;

                    objRegistro.NombreConformidadGST = _nombre;
               

                    ServiceManager<ApplicationDAO>.Provider.SubirArchivosRemove(objRegistro);

                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("upload3")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload3()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty, _usuarioCreacion = string.Empty;
                string _descripcionRef = string.Empty;
                int? appId = null;
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

                int _archivoId = Convert.ToInt16(request.Params["ArchivoId"]);
                if (_archivoId >= 0)
                {
                    int _codIntTablaProcedencia = 0;
                    _codIntTablaProcedencia = Convert.ToInt16(request.Params["TablaProcedencia"]);
                    _entidadId = request.Params["EntidadId"];
                    var user = TokenGenerator.GetCurrentUser();
                    _usuarioCreacion = user.Matricula;

           
                    _descripcionRef = request.Params["DescripcionRef"];
                    appId = Convert.ToInt32(request.Params["AppId"]);

                    ArchivosCvtDTO objRegistro = new ArchivosCvtDTO();
                    objRegistro.Id = _archivoId;
                    objRegistro.EntidadId = _entidadId;
                    objRegistro.Activo = _contenido != null;
                    objRegistro.Contenido = _contenido;
                    objRegistro.UsuarioCreacion = _usuarioCreacion;
                    objRegistro.UsuarioModificacion = _usuarioCreacion;
                    objRegistro.DescripcionRef = _descripcionRef;
                    objRegistro.AppId = appId;
                    objRegistro.Nombre = _nombre;

                    int Id = ServiceManager<ArchivosCvtDAO>.Provider.AddOrEdit2(objRegistro);
                    bool estado = Id > 0;
                    response = Request.CreateResponse(HttpStatusCode.OK, estado);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("upload4")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload4()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty, _usuarioCreacion = string.Empty, _matricula = string.Empty, _nombresCompletos = string.Empty;
      
                int? appId = null;
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

                int _archivoId = Convert.ToInt16(request.Params["ArchivoId"]);
                if (_archivoId >= 0)
                {
                    var user = TokenGenerator.GetCurrentUser();
                    _usuarioCreacion = user.Matricula;
                    _matricula =user.Matricula;
                    _nombresCompletos = user.Nombres;

                    appId = Convert.ToInt32(request.Params["AppId"]);

                    ArchivosCvtDTO objRegistro = new ArchivosCvtDTO();
                    objRegistro.Id = _archivoId;
                    objRegistro.EntidadId = _entidadId;
                    objRegistro.Activo = _contenido != null;
                    objRegistro.Contenido = _contenido;
                    objRegistro.UsuarioCreacion = _usuarioCreacion;
                    objRegistro.UsuarioModificacion = _usuarioCreacion;
                    objRegistro.AppId = appId;
                    objRegistro.Nombre = _nombre;
                    objRegistro.Matricula = _matricula;
                    objRegistro.NombresCompletos = _nombresCompletos;

                    int Id = ServiceManager<ArchivosCvtDAO>.Provider.AddOrEdit3(objRegistro);
                    bool estado = Id > 0;
                    response = Request.CreateResponse(HttpStatusCode.OK, estado);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        [Route("upload6")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpload6()
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
                    clientFile1 = request.Files["File1"];
                    _nombre = new FileInfo(clientFile1.FileName).Name;
                    using (var binaryReader = new BinaryReader(clientFile1.InputStream))
                    {
                        _contenido = binaryReader.ReadBytes(clientFile1.ContentLength);
                    }
                }

                int _BitacoraId = Convert.ToInt16(request.Params["BitacoraId"]);
                if (_BitacoraId >= 0)
                {
                    var cadenaConexion = Constantes.CadenaConexion;
                    using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                    {
                        cnx.Open();
                        string sql = "[app].[USP_Actualizar_Archivo_BitacoraAcciones]";

                        using (var comando = new SqlCommand(sql, cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@BitacoraId", _BitacoraId));
                            comando.Parameters.Add(new SqlParameter("@Archivo", _contenido));
                            comando.Parameters.Add(new SqlParameter("@NombreArchivo", _nombre));

                            var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                            while (reader.Read())
                            {
                                int result = reader.GetData<int>("ID");
                                bool estado = result > 0;
                                response = Request.CreateResponse(HttpStatusCode.OK, estado);
                            }
                            reader.Close();
                        }
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("uploadTempFile")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUploadTempFile()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                string _entidadId = string.Empty, _nombre = string.Empty, _usuarioCreacion = string.Empty, _matricula = string.Empty, _nombresCompletos = string.Empty;

                int? appId = null;
                int? solId = null;
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

                int _archivoId = Convert.ToInt16(request.Params["ArchivoId"]);
                if (_archivoId >= 0)
                {
                    var user = TokenGenerator.GetCurrentUser();
                    _usuarioCreacion = user.Matricula;
                    _matricula = user.Matricula;
                    _nombresCompletos = user.Nombres;

                    solId = Convert.ToInt32(request.Params["SolId"]);
                    appId = Convert.ToInt32(request.Params["AppId"]);

                    ArchivosCvtDTO objRegistro = new ArchivosCvtDTO();
                    objRegistro.Id = _archivoId;
                    objRegistro.EntidadId = _entidadId;
                    objRegistro.Activo = _contenido != null;
                    objRegistro.Contenido = _contenido;
                    objRegistro.UsuarioCreacion = _usuarioCreacion;
                    objRegistro.UsuarioModificacion = _usuarioCreacion;
                    objRegistro.SolId = solId;
                    objRegistro.AppId = appId;
                    objRegistro.Nombre = _nombre;
                    objRegistro.Matricula = _matricula;
                    objRegistro.NombresCompletos = _nombresCompletos;

                    int Id = ServiceManager<ArchivosCvtDAO>.Provider.AddOrEditTempFile(objRegistro);
                    bool estado = Id > 0;
                    response = Request.CreateResponse(HttpStatusCode.OK, estado);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("download")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownload(int id)
        {
            ArchivosCvtDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetById(id);
            var data = new MemoryStream(objRegistro.Contenido);

            return Ok(new { data= data.ToArray(), name= objRegistro.Nombre });
        }

        [Route("download2")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownload2(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetById2(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data= data.ToArray(), name= objRegistro.Nombre });
        }

        [Route("download3")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownload3(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetById3(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data = data.ToArray(), name = objRegistro.Nombre });
        }

        [Route("downloadById")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadById(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetById4(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data = data.ToArray(), name = objRegistro.Nombre });
        }


        [Route("downloadGST")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadGST(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetByIdGST(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data = data.ToArray(), name = objRegistro.Nombre});
        }

        [Route("downloadTicket")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadTicket(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetByIdTicket(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data = data.ToArray(), name = objRegistro.Nombre });
        }


        [Route("downloadRatificacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostDownloadRatificacion(int id)
        {
            ApplicationFileDTO objRegistro = ServiceManager<ArchivosCvtDAO>.Provider.GetByIdRatificacion(id);
            var data = new MemoryStream(objRegistro.ArchivoAsociado);

            return Ok(new { data = data.ToArray(), name = objRegistro.Nombre });
        }


        [Route("GetByEntidadIdByProcedenciaId")]
        [HttpGet]
		[Authorize]
		public HttpResponseMessage GetByIdByProcedenciaId(string entidadId, int procedenciaId)
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<ArchivosCvtDAO>.Provider.GetByEntidadIdByProcedenciaId(entidadId,procedenciaId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListarArchivos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ArchivosCvtDAO>.Provider.GetArchivos(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<ArchivosCvtDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ObtenerCodigoInterno")]
        [HttpPost]
		[Authorize]
		public HttpResponseMessage PostObtenerCodigoInterno()
        {
            HttpResponseMessage response = null;
            //var listTipoArquetipo = ServiceManager<TipoArquetipoDAO>.Provider.GetTipoArquetipoByFiltro(null);
            //var listEntorno = ServiceManager<EntornoDAO>.Provider.GetEntornoByFiltro(null);
            //var listEntorno = Utilitarios.EnumToList<ETipoRiesgo>();

            var dataRpta = new
            {
                //TipoRiesgo = listTipoRiesgo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                CodigoInterno = (int)ETablaProcedencia.CVT_Otros,
                //TipoArquetipo = listTipoArquetipo,
                //Entorno = listEntorno
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }
    }
}
