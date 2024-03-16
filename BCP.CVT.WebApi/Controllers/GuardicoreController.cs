using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.Cross;
using BCP.CVT.Services.Exportar;
using System.Web;
using System.Threading.Tasks;
using BCP.CVT.WebApi.Auth;
using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.PAPP.Common.Custom;
using BCP.CVT.DTO.Custom;
using System.IO;
using FileHelpers;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Guardicore")]
    public class GuardicoreController : BaseController
    {        
        // GET: api/Guardicore/GetAssets
        [Route("GetAssets")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAssets(int pageNumber, int pageSize, string equipoId)
        {

            try
            {
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                //var urlGetAsset = String.Format("{0}/assets?status=on&sort=-last_seen", URL_API);

                var filterRows = "0";

                if (pageNumber > 1)
                {
                    filterRows = ((pageSize * pageNumber) - pageSize).ToString();
                }

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                equipoId = String.IsNullOrEmpty(equipoId) ? "" : "asset=vm:"+ equipoId + "&";

                var urlGetAsset = String.Format("{0}/assets?{3}status=on&limit={1}&offset={2}&sort=-last_seen", API.Valor, pageSize, filterRows,equipoId);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    var datos = JsonConvert.DeserializeObject<List<GuardicoreApiDTO>>(result.SelectToken("objects").ToString());

                    foreach (var item in datos)
                    {
                        item.name = item.name.IndexOf(".") == -1 ? item.name : String.Concat(item.name.Take(item.name.IndexOf(".")));
                        var so = item.guest_agent_details.os_details;
                        so.os_display_name = so.os_display_name != String.Empty && so.os_version_name != String.Empty ? so.os_display_name + " " + so.os_version_name :
                                             so.os_display_name != String.Empty && so.os_version_name == String.Empty ? so.os_display_name :
                                             so.os_display_name == String.Empty && so.os_version_name != String.Empty ? so.os_version_name : "-";
                    }

                    var listGuar = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConCvtEquipos(datos);

                    var TotalRows = (int)result.SelectToken("total_count");
                    var arreglo = new { Total = TotalRows, Rows = listGuar };
                    return Ok(arreglo);

                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetAssetsByName")]
        [HttpGet]
        //[Authorize]
        [AllowAnonymous]
        public IHttpActionResult GetAssetsByName(string name)
        {

            try
            {
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/assets?status=on&search={1}&limit=500&offset=0&sort=-last_seen", API.Valor, name);
            //var urlGetAsset = String.Format("{0}/visibility/policy/options?active=true&add_wild_card=false&filter_key=vm&filter_name=vm&filter_value=bcpe&ignore_scope=true&include_process_display_name=true&limit=10&offset=0&use_published_group_names=true",API.Valor);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);


                    var datos = JsonConvert.DeserializeObject<List<GuardicoreApiDTO>>(result.SelectToken("objects").ToString());

                    return Ok(datos);
                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetDestinationByName")]
        [HttpGet]
        [AllowAnonymous]
        //[Authorize]
        public IHttpActionResult GetDestinationByName(string name)
        {

            try
            {
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                //var urlGetAsset = String.Format("{0}/assets?status=on&search={1}&limit=500&offset=0&sort=-last_seen", API.Valor, name);
                var urlGetAsset = String.Format("{0}/visibility/policy/options?active=true&add_wild_card=false&filter_key=vm&filter_name=vm&filter_value={1}&ignore_scope=true&include_process_display_name=true&limit=10&offset=0&use_published_group_names=true", API.Valor, name);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    var datos = JsonConvert.DeserializeObject<List<GuardicoreTipoDTO>>(result.SelectToken("available_options").ToString());

                    var arreglo = (from t in datos
                                   select new CustomAutocompleteApplication
                                   {
                                       Id = t.value,
                                       Descripcion = t.text,
                                       Value = t.value
                                   }).ToList();

                    return Ok(arreglo);
                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetConnectionsByAsset")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetConnectionsByAsset(GuardicoreParametrosApiDto param)
        {

            try
            {
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var filterRows = "0";

                if (param.pageNumber > 1)
                {
                    filterRows = ((param.pageSize * param.pageNumber) - param.pageSize).ToString();
                }

                param.tipo = String.IsNullOrEmpty(param.tipo) ? "" : "&connection_type=" + param.tipo;
                param.accion = String.IsNullOrEmpty(param.accion) ? "" : "&policy_verdict=" + param.accion;
                param.appOrigen = String.IsNullOrEmpty(param.appOrigen) ? "" : param.appOrigen;// + ",";
                param.appDestino = String.IsNullOrEmpty(param.appDestino) ? "" : param.appDestino;// + ",";
                param.ambOrigen = String.IsNullOrEmpty(param.ambOrigen) ? "" : param.ambOrigen;
                param.ambDestino = String.IsNullOrEmpty(param.ambDestino) ? "" : param.ambDestino;

                var origen = param.appOrigen != "" || param.ambOrigen != "" ? "&source=labels:" + param.appOrigen + "" + param.ambOrigen : "";
                var destino = param.appDestino != "" || param.ambDestino != "" ? "&destination=labels:" + param.appDestino + "" + param.ambDestino : "";





                origen = origen.Replace("LO-", "").Replace("AO-", "");
                destino = destino.Replace("LD-", "").Replace("AD-", "");



                param.puertos = String.IsNullOrEmpty(param.puertos) ? "" : "&port=" + param.puertos + "&protocols=TCP,UDP";

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);


                var urlGetConnections = String.Format("{0}/connections?any_side=address_classification:Private&from_time={1}&limit={3}&offset={4}&sort=-slot_start_time&to_time={2}{5}{6}{7}{8}{9}", API.Valor, param.fromTime, param.toTime, param.pageSize, filterRows, param.tipo, param.accion, origen, destino, param.puertos);
                client = new RestClient(urlGetConnections);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    var datos = JsonConvert.DeserializeObject<List<ConnectionDto>>(result.SelectToken("objects").ToString());

                    foreach (var item in datos)
                    {
                        item.validateSource = item.source_desc == "" ? "" :
                                              item.source.vm.name.IndexOf(".") == -1 ? item.source.vm.name :
                                              String.Concat(item.source.vm.name.Take(item.source.vm.name.IndexOf(".")));
                        item.validateDestination = item.destination_desc == "" ? "" :
                                                   item.destination.vm.name.IndexOf(".") == -1 ? item.destination.vm.name :
                                                   String.Concat(item.destination.vm.name.Take(item.destination.vm.name.IndexOf(".")));

                    }

                    var listGuar = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConnectionsConCvtEquipos(datos);

                    var TotalRows = (int)result.SelectToken("total_count");

                    if (listGuar == null)
                        return NotFound();

                    dynamic arreglo = new BootstrapTable<ConnectionDto>()
                    {
                        Total = TotalRows,
                        Rows = listGuar
                    };

                    return Ok(arreglo);

                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("ConnectionFilter")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConnectionFilter()
        {
            var arreglo = new FilterGuardicore();
            var datosTipo = tiposConnections();
            var datosAccion = accionesConnections();
            var datosLabels = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreLabels();
            var datosAmbiente = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreLabelsAmbiente();

            //var contarLabels = 1;
            //List<GuardicoreTipoDTO> listLabels = new List<GuardicoreTipoDTO>();

            arreglo.types = (from t in datosTipo
                             select new CustomAutocompleteApplication
                             {
                                 Id = t.value,
                                 Descripcion = t.text,
                                 Value = t.value
                             }).ToList();
            arreglo.actions = (from t in datosAccion
                               select new CustomAutocompleteApplication
                               {
                                   Id = t.value,
                                   Descripcion = t.text,
                                   Value = t.value
                               }).ToList();

            arreglo.labelsOrigen = (from t in datosLabels
                                    select new CustomAutocompleteApplication
                                    {
                                        Id = "LO-" + t.id,
                                        Descripcion = t.value,
                                        Value = "LO-" + t.id
                                    }).ToList();

            arreglo.labelsDestino = (from t in datosLabels
                                     select new CustomAutocompleteApplication
                                     {
                                         Id = "LD-" + t.id,
                                         Descripcion = t.value,
                                         Value = "LD-" + t.id
                                     }).ToList();

            arreglo.ambienteOrigen = (from t in datosAmbiente
                                      select new CustomAutocompleteApplication
                                      {
                                          Id = "AO-" + t.id,
                                          Descripcion = t.value,
                                          Value = "AO-" + t.id
                                      }).ToList();

            arreglo.ambienteDestino = (from t in datosAmbiente
                                       select new CustomAutocompleteApplication
                                       {
                                           Id = "AD-" + t.id,
                                           Descripcion = t.value,
                                           Value = "AD-" + t.id
                                       }).ToList();

            return Ok(arreglo);
        }


        [Route("ConsolidadoFilter")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConsolidadoFilter()
        {
            var arreglo = new Consolidado2();
            var datosTipo = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreComboEstado();

            arreglo.estado = (from t in datosTipo
                              select new CustomAutocompleteApplication
                              {
                                  Id = t.idestado.ToString(),
                                  Descripcion = t.nombreestado,
                                  Value = t.idestado.ToString()
                              }).ToList();

            return Ok(arreglo);
        }

        [Route("Reporte/Consolidado2/tab1")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ReporteConsolidado2(string estado, string apps, string gest)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidado2tab1(estado, apps, gest);
            var totalRows = registros.Count;

            var reader = new BootstrapTable<GuardicoreConsolidado2DTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Reporte/Consolidado2/tab2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ReporteConsolidado2Nivel2(string estado, string apps, string gest)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidado2tab2(estado, apps, gest);
            totalRows = registros.Count;

            var reader = new BootstrapTable<GuardicoreConsolidado2DTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Reporte/Consolidado2/tab1/nivel2")]
        [HttpPost]
        [AllowAnonymous]
        //[Authorize]
        public HttpResponseMessage ReporteConsolidado2Tab1Nivel2(string estado, string apps, string gest)
        {
            try
            {
                HttpResponseMessage response = null;

                var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidado2tab1Nivel2(estado, apps, gest);

                var reader = new BootstrapTable<GuardicoreConsolidado2DTO>()
                {
                    Total = registros.Count(),
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, reader);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("Reporte/Consolidado2/tab2/nivel2")]
        [HttpPost]
        //[Authorize]
        public HttpResponseMessage ReporteConsolidado2Tab2Nivel2(string apps)
        {
            try
            {
                HttpResponseMessage response = null;

                var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidado2tab2Nivel2(apps);

                var reader = new BootstrapTable<GuardicoreConsolidado2DTO>()
                {
                    Total = registros.Count(),
                    Rows = registros
                };

                response = Request.CreateResponse(HttpStatusCode.OK, reader);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("Reporte")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Reporte()
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidado();
            totalRows = registros.Count;

            var reader = new BootstrapTable<GuardicoreConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Reporte/SO")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ReporteGrupoSO(int idestado)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreGrupoSO(idestado);
            totalRows = registros.Count;

            var reader = new BootstrapTable<GuardicoreConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Reporte/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ReporteDetalle(int idestado, string so)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreConsolidadoDetalle(idestado, so);
            totalRows = registros.Count;

            foreach (var item in registros)
            {
                var fecha = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(item.fechaescaneo));
                item.fechaescaneo = fecha.ToShortDateString();
                item.ip = item.ip.Replace(",", ",\n");
            }

            var reader = new BootstrapTable<GuardicoreConsolidadoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Reporte/Exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportarGestionEquipos()
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarReporteGuardicore();
            nomArchivo = "ReporteGuardicoreVsCvt";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Reporte/Exportar2")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetExportarConsolidad2(string estado, string apps, string gest)
        {
            string nomArchivo = "";

            var data = new ExportarData().ExportarReporteConsolido2(estado, apps, gest);
            nomArchivo = "ReporteGuardicoreVsCvt";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        private List<GuardicoreTipoDTO> tiposConnections()
        {
            try
            {
                List<GuardicoreTipoDTO> datos = new List<GuardicoreTipoDTO>();
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/connections/filter-options?filter_name=connection_type&sort=-slot_start_time", API.Valor);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    datos = JsonConvert.DeserializeObject<List<GuardicoreTipoDTO>>(result.SelectToken("available_options").ToString());
                }
                return datos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<GuardicoreTipoDTO> accionesConnections()
        {
            try
            {
                List<GuardicoreTipoDTO> datos = new List<GuardicoreTipoDTO>();
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/connections/filter-options?filter_name=policy_verdict&sort=-slot_start_time", API.Valor);
                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    datos = JsonConvert.DeserializeObject<List<GuardicoreTipoDTO>>(result.SelectToken("available_options").ToString());
                }
                return datos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<GuardicoreTipoDTO> LabelOrigenDestinationConnections(int pageNumber)
        {
            try
            {
                List<GuardicoreTipoDTO> datos = new List<GuardicoreTipoDTO>();
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var filterRows = "0";

                var limit = 100;

                if (pageNumber > 1)
                {
                    filterRows = ((limit * pageNumber) - limit).ToString();
                }

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/visibility/policy/options?active=true&add_wild_card=false&filter_key=user_label&filter_name=user_label&filter_value=&ignore_scope=true&include_process_display_name=true&limit={1}&offset={2}&use_published_group_names=true", API.Valor, limit, filterRows);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    datos = JsonConvert.DeserializeObject<List<GuardicoreTipoDTO>>(result.SelectToken("available_options").ToString());
                }
                return datos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GetAuthKey()
        {
            RestClient client = null;
            RestRequest request = null;
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);
            var usuarioAPI = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GUARDICORE_API_USERNAME");
            var passwordAPI = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GUARDICORE_API_PWD");

            client = new RestClient(string.Format("{0}/authenticate", API.Valor));
            request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");

            dynamic jsonObj = new JObject();
            jsonObj = JObject.FromObject(new
            {
                username = usuarioAPI.Valor,
                password = passwordAPI.Valor

            });

            string jsonObject = JsonConvert.SerializeObject(jsonObj, Formatting.Indented, jsonSerializerSettings);

            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);

            var response = client.Execute(request);

            var resultadoMje = string.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonRetorno = response.Content;
                JObject result = JObject.Parse(jsonRetorno);

                var access_token = result.SelectToken("access_token").ToString();

                return access_token;
            }
            return "";
        }

        [Route("Fase2/Tab2/Listado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ListadoFase2Tab2()
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<GuardicoreDAO>.Provider.GetGuardicoreFase2Tab2Listado();
            totalRows = registros.Count;

            var reader = new BootstrapTable<GuardicoreFase2DTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("GetAplicacionesByMatricula")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionesByMatricula(string filtro)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            HttpResponseMessage response = null;
            var listApp = ServiceManager<GuardicoreDAO>.Provider.GetAplicacionMatricula(matricula, filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("Reporte/TotalFilasFiltroReporte")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage TotalFilasFiltroReporte(GuardicoreParametrosApiDto param)
        {
            try
            {
                int total = 0;
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                param.tipo = String.IsNullOrEmpty(param.tipo) ? "" : "&connection_type=" + param.tipo;
                param.accion = String.IsNullOrEmpty(param.accion) ? "" : "&policy_verdict=" + param.accion;
                param.appOrigen = String.IsNullOrEmpty(param.appOrigen) ? "" : param.appOrigen + ",";
                param.appDestino = String.IsNullOrEmpty(param.appDestino) ? "" : param.appDestino + ",";
                param.ambOrigen = String.IsNullOrEmpty(param.ambOrigen) ? "" : param.ambOrigen;
                param.ambDestino = String.IsNullOrEmpty(param.ambDestino) ? "" : param.ambDestino;

                var origen = param.appOrigen != "" || param.ambOrigen != "" ? "&source=labels:" + param.appOrigen + "" + param.ambOrigen : "";
                var destino = param.appDestino != "" || param.ambDestino != "" ? "&destination=labels:" + param.appDestino + "" + param.ambDestino : "";

                origen = origen.Replace("LO-", "").Replace("AO-", "");
                destino = destino.Replace("LD-", "").Replace("AD-", "");





                param.puertos = String.IsNullOrEmpty(param.puertos) ? "" : "&port=" + param.puertos + "&protocols=TCP,UDP";










                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/connections?any_side=address_classification:Private&from_time={1}&limit={3}&offset={4}&sort=-slot_start_time&to_time={2}{5}{6}{7}{8}{9}", API.Valor, param.fromTime, param.toTime, 10, 0, param.tipo, param.accion, origen, destino, param.puertos);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    total = (int)result.SelectToken("total_count");
                }

                return Request.CreateResponse<int>(total);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("Reporte/ExportarGuardicoreCSV")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ExportarCsvAPIGuardicore(GuardicoreParametrosApiDto param)
        {
            try
            {
                String datos = "";
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                param.tipo = String.IsNullOrEmpty(param.tipo) ? "" : "&connection_type=" + param.tipo;
                param.accion = String.IsNullOrEmpty(param.accion) ? "" : "&policy_verdict=" + param.accion;
                param.appOrigen = String.IsNullOrEmpty(param.appOrigen) ? "" : param.appOrigen + ",";
                param.appDestino = String.IsNullOrEmpty(param.appDestino) ? "" : param.appDestino + ",";
                param.ambOrigen = String.IsNullOrEmpty(param.ambOrigen) ? "" : param.ambOrigen;
                param.ambDestino = String.IsNullOrEmpty(param.ambDestino) ? "" : param.ambDestino;
                param.total = param.total < 100000 ? param.total : 100000;

                var origen = param.appOrigen != "" || param.ambOrigen != "" ? "&source=labels:" + param.appOrigen + "" + param.ambOrigen : "";
                var destino = param.appDestino != "" || param.ambDestino != "" ? "&destination=labels:" + param.appDestino + "" + param.ambDestino : "";

                origen = origen.Replace("LO-", "").Replace("AO-", "");
                destino = destino.Replace("LD-", "").Replace("AD-", "");

                param.puertos = String.IsNullOrEmpty(param.puertos) ? "" : "&port=" + param.puertos + "&protocols=TCP,UDP";


                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/connections/export?from_time={1}&limit={8}&offset=0&sort=-slot_start_time&to_time={2}&compress_result=false{3}{4}{5}{6}{7}", API.Valor, param.fromTime, param.toTime, param.tipo, param.accion, origen, destino, param.puertos, param.total);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);

                    datos = result.SelectToken("export_task_status_id").ToString();
                }

                return Request.CreateResponse<string>(datos);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("Reporte/ExportarGuardicoreCSV/Estado")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult EstadoDelExportarCsv(string id)
        {
            try
            {
                var datos = "";
                var cargado = "";
                var total = "";
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}/export_csv_task_status?task_id={1}", API.Valor, id);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonRetorno = response.Content;
                    JObject result = JObject.Parse(jsonRetorno);
                    cargado = result.SelectToken("records_written").ToString();
                    total = result.SelectToken("total_records").ToString();

                    if (total == cargado)
                    {
                        datos = result.SelectToken("exported_csv_file_id").ToString();
                    }
                }

                var array = new { cargado, total, datos };

                return Json(array);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("Reporte/ExportarGuardicoreCSV/Prepare")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult DescargarArchivoExportado(string estado)
        {

            string cadena = "";

            try
            {
                RestClient client = null;
                RestRequest request = null;
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var authKey = GetAuthKey();

                var API = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.GUARDICORE_API_URL);

                var urlGetAsset = String.Format("{0}exported_csv_files/{1}", API.Valor, estado);

                client = new RestClient(urlGetAsset);
                client.AddDefaultHeader("Authorization", "Bearer " + authKey);

                request = new RestRequest(Method.GET);

                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "text/csv");

                var response = client.Execute(request);

                var urlfin = response.Headers[3].Value.ToString();

                using (WebClient web1 = new WebClient())
                {
                    var url = String.Format("https://cus-2580.cloud.guardicore.com{1}", API.Valor, urlfin);
                    var data = web1.DownloadData(url);

                    StreamReader stream = new StreamReader(new MemoryStream(data));

                    var engine = new FileHelperEngine<GuardicoreCsvDto>();
                    engine.HeaderText = engine.GetFileHeader();
                    var datGuardicore = engine.ReadString(stream.ReadToEnd());

                    List<ConnectionDto> items = new List<ConnectionDto>();

                    for (var i = 1; i < datGuardicore.Count(); i++)
                    {
                        ConnectionDto item = new ConnectionDto();
                        item.id = string.IsNullOrEmpty(datGuardicore[i].id) ? "-" : datGuardicore[i].id.ToString();
                        item.source_ip = string.IsNullOrEmpty(datGuardicore[i].source_ip) ? "-" : datGuardicore[i].source_ip;
                        item.validateSource = string.IsNullOrEmpty(datGuardicore[i].source_asset_name) ? "-" :
                                                                   datGuardicore[i].source_asset_name.IndexOf(".") == -1 ? datGuardicore[i].source_asset_name :
                                                                   String.Concat(datGuardicore[i].source_asset_name.Take(datGuardicore[i].source_asset_name.IndexOf(".")));
                        item.destination_ip = string.IsNullOrEmpty(datGuardicore[i].destination_ip) ? "-" : datGuardicore[i].destination_ip;
                        item.validateDestination = string.IsNullOrEmpty(datGuardicore[i].destination_asset_name) ? "-" :
                                                                        datGuardicore[i].destination_asset_name.IndexOf(".") == -1 ? datGuardicore[i].destination_asset_name :
                                                                        String.Concat(datGuardicore[i].destination_asset_name.Take(datGuardicore[i].destination_asset_name.IndexOf(".")));
                        item.policy_ruleset = string.IsNullOrEmpty(datGuardicore[i].policy_ruleset) ? "-" : datGuardicore[i].policy_ruleset.ToString();
                        item.source_process = string.IsNullOrEmpty(datGuardicore[i].source_process) ? "-" : datGuardicore[i].source_process.Replace(",", ";").ToString();
                        item.destination_process = string.IsNullOrEmpty(datGuardicore[i].destination_process) ? "-" : datGuardicore[i].destination_process.Replace(",", ";").ToString();
                        item.destination_port = string.IsNullOrEmpty(datGuardicore[i].destination_port) ? "-" : datGuardicore[i].destination_port.ToString();
                        item.ip_protocol = string.IsNullOrEmpty(datGuardicore[i].ip_protocol) ? "-" : datGuardicore[i].ip_protocol.ToString();
                        item.connection_type = string.IsNullOrEmpty(datGuardicore[i].connection_type) ? "-" : datGuardicore[i].connection_type.ToString();
                        item.labelsOrigen = string.IsNullOrEmpty(datGuardicore[i].source_asset_labels) ? "-" : datGuardicore[i].source_asset_labels.Replace(",", ";").ToString();
                        item.labelDestino = string.IsNullOrEmpty(datGuardicore[i].destination_asset_labels) ? "-" : datGuardicore[i].destination_asset_labels.Replace(",", ";").ToString();
                        item.slot_start_time = 0;

                        items.Add(item);
                    }

                    if (items.Count > 0)
                    {
                        cadena = Convert.ToBase64String(new ExportarData().ExportarGuardicoreInteraccionEntreServidores(items));
                    }
                }

                return Json(cadena);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}