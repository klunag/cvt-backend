using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Threading.Tasks;
using System.Web;
using System.Xml.XPath;
using System.Web.Security.AntiXss;

namespace BCP.CVT.Services.Service
{
    class BigFixSvc: BigFixDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddPeticion(BigFixDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = new PeticionBigFix()
                    {
                        Email = objeto.Email,
                        FechaPeticion = DateTime.Now,
                        FlagProcesado = objeto.FlagProcesado,
                        GeneradoPor = objeto.GeneradoPor,
                        Ruta = objeto.Ruta,
                        Servidor = objeto.Servidor,
                        IdBigFix = objeto.IdBigFix
                    };

                    ctx.PeticionBigFix.Add(entidad);
                    ctx.SaveChanges();
                    return entidad.PeticionId;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorBigFix
                    , "Error al registrar un agrupamiento."
                    , new object[] { null });
            }
        }

        public override string ConsultarEstadoBigFix(string id)
        {
            var bigFixDTO = ServiceManager<BigFixDAO>.Provider.GetBigFixById(id);
            if(bigFixDTO != null)
            {
                var url = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("BIGFIX_URL_TRANSACCION").Valor; //Agregado
                var urlFinal = string.Format(url, bigFixDTO.IdBigFix);

                RestSharp.RestClient client = new RestSharp.RestClient(new Uri(urlFinal));

                RestSharp.IRestRequest request = new RestSharp.RestRequest(RestSharp.Method.GET);
                request.AddHeader("Authorization", "Basic QVBFMTk1REVTOjZMS3ZweTk1");

                var validarSSL = Settings.Get<bool>("BigFix.SSLValidar");
                if (validarSSL)
                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPoliciyErrors) => true;

                var response = client.Execute(request);
                if(response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var xmlRetorno = response.Content;

                        var status = GetValorXML(xmlRetorno, "/BESAPI/ActionResults/Status");
                        status = string.IsNullOrWhiteSpace(status) ? "-" : status;

                        var date = GetValorXML(xmlRetorno, "/BESAPI/ActionResults/DateIssued");
                        date = string.IsNullOrWhiteSpace(date) ? "-" : date;

                        var statusTask = GetValorXML(xmlRetorno, "/BESAPI/ActionResults/Computer/Status");
                        statusTask = string.IsNullOrWhiteSpace(statusTask) ? "-" : statusTask;

                        var HTML = "";

                        HTML = "<p>" +
                                "Estos son los datos obtenidos desde BigFix:" +
                                "<br/>- <strong>ID de la transacción:</strong> " + id +
                                "<br/>- <strong>Estado de la transacción:</strong> " + status +
                                "<br/>- <strong>Fecha de envío de la tarea al servidor:</strong> " + date +
                                "<br/>- <strong>Estado de la tarea en el servidor:</strong> " + statusTask +
                            "</p>";

                        HTML = HTML + "<p>" +
                            "Estos son los datos de la tarea enviada a BigFix" +
                            "<br/>- <strong>Servidor:</strong> " + bigFixDTO.Servidor +
                            "<br/>- <strong>Ruta requerida:</strong> " + bigFixDTO.Ruta +
                            "<br/>- <strong>Correos electrónicos:</strong> " + bigFixDTO.Email +
                            "</p>";

                        HTML = HTML + "<strong>XML de la respuesta</strong>" +
                            "<pre class=\"prettyprint lang-xml linenums prettyprinted\">" +
                                "<code>" +
                                    HttpUtility.HtmlEncode(xmlRetorno) +
                                "</code>" +
                            "</pre>";

                        return HTML;
                    }
                    else
                    {
                        var HTML = "";
                        HTML = "<p>" +
                            "<span class=\"warning\">Tu solicitud no fue enviada a IBM BigFix, revisa los datos registrados en el formulario o la disponibilidad del servicio. La respuesta de la petición fue " + string.Format("ERROR {0} ", (int)response.StatusCode) + "</span>" +
                        "</p>";
                        return HTML;
                    }
                }
                else
                {
                    var HTML = "";
                    HTML = "<p>" +
                            "<span class=\"warning\">Tu solicitud no fue enviada a IBM BigFix, revisa los datos registrados en el formulario o la disponibilidad del servicio. La respuesta de la petición fue nula.</span>" +
                        "</p>";
                    return HTML;
                }
            }
            else
            {
                var HTML = "";
                HTML = "<p>" +
                            "<span class=\"warning\">El código ingresado no se encuentra registrado en el catálogo de transacciones de BigFix.</span>" +
                        "</p>";
                return HTML;
            }          
        }

        private string GetValorXML(string xml, string ruta)
        {
            try
            {
                var retorno = string.Empty;
                using (var sr = new StringReader(xml))
                {
                    XPathDocument xd = new XPathDocument(sr);
                    var nav = xd.CreateNavigator();
                    retorno = nav.SelectSingleNode(ruta).Value;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        public override BigFixDTO GetBigFixById(string id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidades = (from u in ctx.PeticionBigFix
                                     where u.IdBigFix == id
                                     select new BigFixDTO()
                                     {
                                         FechaCreacion = u.FechaPeticion,
                                         FechaPeticion = u.FechaPeticion,
                                         Email = u.Email,
                                         FlagProcesado = u.FlagProcesado,
                                         GeneradoPor = u.GeneradoPor,
                                         Id = u.PeticionId,
                                         Ruta = u.Ruta,
                                         Servidor = u.Servidor,
                                         IdBigFix = u.IdBigFix
                                     }).FirstOrDefault();

                    return entidades;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorLogAuditoriaDTO
                    , "Error al listar el log."
                    , new object[] { null });
            }
        }

        public override List<BigFixDTO> GetLog(int page, int rows, out int nroTotal)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    nroTotal = 0;
                    var entidades = (from u in ctx.PeticionBigFix
                                     select new BigFixDTO()
                                     {
                                         FechaCreacion = u.FechaPeticion,
                                         FechaPeticion = u.FechaPeticion,
                                         Email = u.Email,
                                         FlagProcesado = u.FlagProcesado,
                                         GeneradoPor = u.GeneradoPor,
                                         Id = u.PeticionId,
                                         Ruta = u.Ruta,
                                         Servidor = u.Servidor,
                                         IdBigFix = u.IdBigFix
                                     });
                    nroTotal = entidades.Count();
                    var registrosFinal = entidades.OrderByDescending(x => x.FechaCreacion).Skip(rows * (page - 1)).Take(rows).ToList();

                    return registrosFinal;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorLogAuditoriaDTO
                    , "Error al listar el log."
                    , new object[] { null });
            }
        }

        public override int UpdatePeticion(int id, bool estado, string idBigFix)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = ctx.PeticionBigFix.FirstOrDefault(x => x.PeticionId == id);
                    if (registro != null)
                    {
                        registro.FlagProcesado = estado;
                        registro.IdBigFix = idBigFix;
                        ctx.SaveChanges();
                    }

                    return registro.PeticionId;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorBigFix
                    , "Error al registrar una petición."
                    , new object[] { null });
            }
        }

        public override bool ExisteID(string Id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.PeticionBigFix
                                    where u.IdBigFix == Id
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteID(string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteID(string Id)"
                    , new object[] { null });
            }
        }

        private string ProcesarEnvio(int id, BigFixDTO objeto)
        {
            var HTML = string.Empty;
            var rutaArchivos = Settings.Get<string>("BigFix.RutaXML");            
            var rutaBase = Path.Combine(rutaArchivos, "XMLBase.xml");
            var contenidoXML = File.ReadAllText(rutaBase);
            contenidoXML = contenidoXML.Replace("[SERVIDOR]", objeto.Servidor)
                                        .Replace("[RUTA]", objeto.Ruta)
                                        .Replace("[EMAIL]", objeto.Email);
            File.WriteAllText(Path.Combine(rutaArchivos, string.Format("{0}.xml", id.ToString())), contenidoXML);

            var url = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("BIGFIX_URL_SERVICIO").Valor;            
            RestSharp.RestClient client = new RestSharp.RestClient(new Uri(url));            
            RestSharp.IRestRequest request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("Authorization", "Basic QVBFMTk1REVTOjZMS3ZweTk1");

            request.AddParameter("application/xml"
                , File.ReadAllBytes(Path.Combine(rutaArchivos, string.Format("{0}.xml", id.ToString())))
                , RestSharp.ParameterType.RequestBody);

            var validarSSL = Settings.Get<bool>("BigFix.SSLValidar");
            if (validarSSL)
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPoliciyErrors) => true;

            var response = client.Execute(request);
            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var xmlRetorno = response.Content;
                    var idBigFix = GetIdBigFix(xmlRetorno);

                    ServiceManager<BigFixDAO>.Provider.UpdatePeticion(id, true, idBigFix);
                    
                    HTML = string.Format("Tu solicitud fue enviada a IBM BigFix, en breve la plataforma enviará el correo de respuesta. El ID de la solicitud es <strong>{0}</strong>", idBigFix);
                    return HTML;
                }
                else
                {
                    ServiceManager<BigFixDAO>.Provider.UpdatePeticion(id, false, string.Format("ERROR {0} ", (int)response.StatusCode));
                    if (response.ErrorException != null)
                        log.Error(response.ErrorMessage, response.ErrorException);                    
                                        
                    HTML = "Tu solicitud no fue enviada a IBM BigFix, revisa los datos registrados en el formulario o la disponibilidad del servicio.";
                    return HTML;
                }
            }
            else
            {
                ServiceManager<BigFixDAO>.Provider.UpdatePeticion(id, false, "ERROR -1");
                
                HTML = "Tu solicitud no fue enviada a IBM BigFix, revisa los datos registrados en el formulario o la disponibilidad del servicio. La respuesta de la petición fue nula.";
                return HTML;
            }
        }

        private string GetIdBigFix(string xml)
        {
            try
            {
                var retorno = string.Empty;
                using (var sr = new StringReader(xml))
                {
                    XPathDocument xd = new XPathDocument(sr);
                    var nav = xd.CreateNavigator();
                    retorno = nav.SelectSingleNode("/BESAPI/Action/ID").Value;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        public override List<CmdbDiscoBigFix> GetDiscos(string servidor)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.USP_Cmdb_ListarDiscosServidores(servidor)
                                     select new CmdbDiscoBigFix()
                                     {
                                         Disco = u.InformacionDisco,
                                         FechaActualizacion = u.FechaSincronizacion,
                                         Servidor = u.Hostname
                                     }).ToList();
                    return registros;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorCmdbDTO
                    , "Error al listar todos los discos del servidor."
                    , new object[] { null });
            }
        }

        public override string SolicitarArchivosBigFix(BigFixDTO entidad)
        {
            try
            {
                var objeto = new BigFixDTO()
                {
                    Email = entidad.Email,
                    FechaPeticion = DateTime.Now,
                    FlagProcesado = false,
                    GeneradoPor = entidad.UsuarioCreacion,//Session["Usuario.Matricula"].ToString(),
                    Ruta = AntiXssEncoder.XmlAttributeEncode(entidad.Ruta),
                    Servidor = AntiXssEncoder.XmlAttributeEncode(entidad.Servidor)
                };

                var idPeticion = ServiceManager<BigFixDAO>.Provider.AddPeticion(objeto);

                var HTML = "";
                HTML = "Tu solicitud fue enviada a IBM BigFix, en breve la plataforma enviará el correo de respuesta.";
                return HTML;
                //var validar = Settings.Get<bool>("BigFix.Validar");
                //if (validar)
                //{
                //    var HTML = ProcesarEnvio(idPeticion, objeto);
                //    return HTML;
                //}
                //else
                //{
                //    var HTML = "";
                //    HTML = "<p>" +
                //                "<span class=\"warning\"> No se logro validar la llave \"BigFix.Validar\"</span>" +
                //            "</p>";
                //    return HTML;
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                var HTML = "";
                HTML = "<p>" +
                            "<span class=\"warning\">El servicio BigFix no responde, por favor inténtalo más tarde</span>" +
                        "</p>";
                return HTML;
            }                            
        }

        public override List<BigFixDTO> ListarBixFixPorFlagProcesado(bool flagProcesado)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.PeticionBigFix
                                     where u.FlagProcesado.Equals(flagProcesado)
                                     select new BigFixDTO
                                     {
                                         GeneradoPor = u.GeneradoPor,
                                         FechaPeticion = u.FechaPeticion,
                                         Servidor = u.Servidor,
                                         Ruta = u.Ruta,
                                         Email = u.Email,
                                         FlagProcesado = u.FlagProcesado,
                                         IdBigFix = u.IdBigFix,
                                         Id = u.PeticionId
                                     }).ToList();
                    return registros;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorCmdbDTO
                    , "Error al listar todos los discos del servidor."
                    , new object[] { null });
            }
        }
    }
}
