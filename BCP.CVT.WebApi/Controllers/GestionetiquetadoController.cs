using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Gestionetiquetado")]
    public class GestionetiquetadoController : BaseController
    {
        // GET: api/Guardicore/GetAssets
        [Route("RegistrarServidor")]
        [HttpGet]
        public IHttpActionResult RegistrarServidorGeneral(string etiqueta,string clave, string comentario)
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();
                string matricula = user.Matricula;

                var respuesta = ServiceManager<GuardicoreDAO>.Provider.SetEtiquetado(clave,etiqueta,comentario, matricula);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ListadoEtiquetado")]
        [HttpGet]
        public HttpResponseMessage ListadoEtiquetado(string etiqueta, string clave)
        {
            try
            {
                var data = ServiceManager<GuardicoreDAO>.Provider.GetEtiquetado(etiqueta, clave);

                data.ForEach(x => { x.comentario = HttpUtility.HtmlEncode(x.comentario); });

                var reader = new BootstrapTable<GuardicoreEtiquetado>()
                {
                    Total = data.Count(),
                    Rows = data
                };

                return Request.CreateResponse(HttpStatusCode.OK, reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("BuscarRegistro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage BuscarRegistro(int id)
        {
            try
            {
                var data = ServiceManager<GuardicoreDAO>.Provider.GetEtiquetadoId(id);

                return Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EditarEtiqueta")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EditarEtiqueta(int id, string etiqueta,string clave, string comentario)
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();
                string matricula = user.Matricula;
                var data = ServiceManager<GuardicoreDAO>.Provider.ActualizarRegistro(id,clave,etiqueta,comentario, matricula);

                return Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EliminarEtiqueta")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EliminarEtiqueta(int id)
        {
            try
            {
                ServiceManager<GuardicoreDAO>.Provider.EliminarRegistro(id);

                return Request.CreateResponse(HttpStatusCode.OK, "");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ExportarTab1")]
        [HttpGet]
        public IHttpActionResult GetExportarTab1(string etiqueta, string clave)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarGestionEtiquetadoGeneral(etiqueta, clave);
            nomArchivo = "GestionEtiquetadoGeneral";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        //tab 2

        [Route("RegistrarServidorRelacion")]
        [HttpGet]
        public IHttpActionResult RegistrarServidorRelacion(string etiqueta, string comodin, int prioridad, string comentario, int tipo)
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();
                string matricula = user.Matricula;

                var data = ServiceManager<GuardicoreDAO>.Provider.SetServidorRelacion(etiqueta,comodin,prioridad,comentario,tipo,matricula);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ListadoServidorRelacion")]
        [HttpGet]
        public HttpResponseMessage ListadoServidorRelacion(string etiqueta, string comodin, int prioridad, int tipo)
        {
            try
            {
                var data = ServiceManager<GuardicoreDAO>.Provider.GetServidorRelacion(etiqueta, comodin, prioridad, tipo);

                data.ForEach(x => { x.comentario = HttpUtility.HtmlEncode(x.comentario); });


                var reader = new BootstrapTable<GuardicoreServidorRelacionDTO>()
                {
                    Total = data.Count(),
                    Rows = data
                };

                return Request.CreateResponse(HttpStatusCode.OK, reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("BuscarServidorRelacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage BuscarServidorRelacion(int id)
        {
            try
            {
                var data = ServiceManager<GuardicoreDAO>.Provider.GetServidorRelacionId(id);

                return Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EditarServidorRelacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EditarServidorRelacion(int id, string etiqueta, string comodin, int prioridad, string comentario, int tipo)
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();
                string matricula = user.Matricula;

                var data = ServiceManager<GuardicoreDAO>.Provider.ActualizarServidorRelacion(id, etiqueta, comodin, prioridad, comentario, tipo, matricula);

                return Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EliminarServidorRelacion")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage EliminarServidorRelacion(int id)
        {
            try
            {
                ServiceManager<GuardicoreDAO>.Provider.EliminarServidorRelacion(id);

                return Request.CreateResponse(HttpStatusCode.OK, "");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ExportarTab2")]
        [HttpGet]
        public IHttpActionResult GetExportarTab2(string etiqueta, string comodin, int prioridad, int tipo)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarGestionEtiquetadoTecnologia(etiqueta, comodin, prioridad, tipo);
            nomArchivo = "GestionEtiquetadoTecnologia";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
        private List<string> reglasEtiquetado(string servidor)
        {
            var etiqueta = "";
            var clave = "";

            if (servidor.Substring(1, 4).ToUpper() == "FILE")
            {
                clave = "FILE";
                etiqueta = "FILESERVER";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSIIS")
            {
                clave = "CONSIIS";
                etiqueta = "WS-IIS";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSWIN")
            {
                clave = "CONSWIN";
                etiqueta = "SRV-WINDOWS";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSSQL")
            {
                clave = "CONSSQL";
                etiqueta = "DBSQL";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSAPP")
            {
                clave = "CONSAPP";
                etiqueta = "APTSRVS";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSWAS")
            {
                clave = "CONSWAS";
                etiqueta = "APTWAS";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSDB2")
            {
                clave = "CONSDB2";
                etiqueta = "DB2";
            }
            else if (servidor.Substring(1, 6).ToUpper() == "CONSDB")
            {
                clave = "CONSDB";
                etiqueta = "DB";
            }
            else if (servidor.Substring(1, 7).ToUpper() == "CONSLNX")
            {
                clave = "CONSLNX";
                etiqueta = "SRV-LINUX";
            }
            else if (servidor.Substring(1, 6).ToUpper() == "CONSMQ")
            {
                clave = "CONSMQ";
                etiqueta = "MQ";
            }
            else if (servidor.Substring(1, 6).ToUpper() == "CONSWS")
            {
                clave = "CONSWS";
                etiqueta = "WS";
            }
            else
            {
                clave = "NO SE ENCONTRO";
                etiqueta = "NO SE ENCONTRO";
            }
            return new List<string>() { clave, etiqueta };
        }
    }
}