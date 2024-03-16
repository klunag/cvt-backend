using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dependencias")]
    public class DependenciasController : BaseController
    {
        [Route("Lista")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependencies(PaginacionDependencias pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasDAO>.Provider.GetListDependencias(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DependenciasDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Detalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependenciesDetalle(PaginacionDependencias pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasDAO>.Provider.GetListDependenciasDetalle(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DependenciasDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("VerificarAcceso")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage VerificarAcceso(string duena, string impacto)
        {
            var user = TokenGenerator.GetCurrentUser();
            string matricula = user.Matricula;
            HttpResponseMessage response = null;
            var dataRpta = new List<DependenciasDTO>();
            var dataRpta1 = ServiceManager<DependenciasDAO>.Provider.GetListAccesoApps(matricula);
            foreach (var item in dataRpta1)
            {
                if (item.CodigoAptOrigen == duena)
                {
                    dataRpta = dataRpta1;
                    break;
                }
                if (item.CodigoAptOrigen == impacto)
                {
                    dataRpta = dataRpta1;
                    break;
                }
            }
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ExportarDetalle")]
        [HttpGet]
        public IHttpActionResult GetDependenciesDetalle(string codigoAPT, string tipoConexionId, int equipoId, int tipoRelacionamientoId, int tipoEtiquetaId, string tipoCompId, int tipoConsultaId)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarDependenciaDetalle(codigoAPT, tipoConexionId, equipoId, tipoRelacionamientoId, tipoEtiquetaId, tipoCompId, tipoConsultaId);
            nomArchivo = string.Format("ReporteDependenciasApps_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ListaComponentes")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependenciesComponentes(PaginacionDependenciasComponentes pag)
        {
            HttpResponseMessage response = null;
            var registros = ServiceManager<DependenciasDAO>.Provider.GetListDependenciasComponentes(pag, out int totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<DependenciasComponentesDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("CombosData")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage CombosData()
        {
            HttpResponseMessage response = null;
            var dataRpta = ServiceManager<DependenciasDAO>.Provider.GetCombosData();
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("GetEquipoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEquipoByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<DependenciasDAO>.Provider.GetEquipoByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("GetAplicacionByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAplicacionByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<AplicacionDAO>.Provider.GetAplicacionByFiltro(filtro, true);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        #region Vista Grafica Dependencias
        [Route("VGD-Equipo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependenciesComponentesGrafo(PaginacionVGD pag)
        {
            var componente = ServiceManager<DependenciasDAO>.Provider.GetDataComponente(pag.TipoComponente, pag.equipoId);
            var relaciones = ServiceManager<DependenciasDAO>.Provider.GetAppsRelacionComponente(pag.TipoComponente, pag.equipoId, pag.TipoRelacionId);
            var tieneOwner = false;

            if (componente.Count <= 0 || relaciones.Count <= 0)
                return BadRequest();

            if (!String.IsNullOrEmpty(componente[0].CodigoAptOwner))
            {
                tieneOwner = true;
                relaciones = relaciones.FindAll(r => r.CodigoApt != componente[0].CodigoAptOwner);
            }

            // Parametros
            var colorEdgeRelacion = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_RELACION").Valor;
            var colorEdgeDependencia = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_DEPENDENCIA").Valor;
            var colorNodeAppRelacionada = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_CONSULTADA").Valor;
            var colorNodeAppOwner = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_OWNER").Valor;

            GrafoDTO grafo = new GrafoDTO();
            List<GrafoNodeDTO> grafoNodes = new List<GrafoNodeDTO>();
            List<GrafoEdgeDTO> grafoEdges = new List<GrafoEdgeDTO>();

            var tipoNodoPrincipal = "";
            switch (componente[0].TipoEquipoId)
            {
                case 0:
                    tipoNodoPrincipal = "Tec";
                    break;
                case 1:
                    tipoNodoPrincipal = "Servidor";
                    break;
                case 4:
                    tipoNodoPrincipal = "Nube";
                    break;
                case 9:
                    tipoNodoPrincipal = "Api";
                    break;
                default:
                    tipoNodoPrincipal = "Servidor";
                    break;
            }

            // Nodo principal
            var nodePrincipal = new GrafoNodeDTO()
            {
                id = componente[0].ComponenteId.ToString(),
                label = componente[0].Nombre,
                title = componente[0].Descripcion.Length == 0 ? "" : componente[0].Descripcion,
                tipoNodo = tipoNodoPrincipal,
                level = (pag.Nivel == 1 ? 1 : 2)//,
                //color = "blue"
            };
            grafoNodes.Add(nodePrincipal);

            // Nodo del owner
            if (tieneOwner)
            {
                var nodeOwner = new GrafoNodeDTO()
                {
                    id = componente[0].CodigoAptOwner,
                    label = componente[0].CodigoAptOwner,
                    title = componente[0].NombreAptOwner,
                    tipoNodo = "App",
                    color = colorNodeAppOwner,
                    level = ((pag.Nivel == 1 ? 1 : 2) + 1)
                };
                grafoNodes.Add(nodeOwner);

                // Edge equipo - owner
                var edge = new GrafoEdgeDTO()
                {
                    from = nodePrincipal.id,
                    to = nodeOwner.id,
                    label = "Dependencia",
                    arrows = (componente[0].TipoEquipoId == 1 ? "from" : "to"),
                    color = colorEdgeDependencia,
                    length = "200"
                };
                grafoEdges.Add(edge);

            }

            // Relaciones del Nodo Principal (Servidor, API)
            foreach (var r in relaciones)
            {
                var existeNodo = grafoNodes.Count(x => x.id == r.CodigoApt);
                if (existeNodo == 0)
                {
                    var node = new GrafoNodeDTO()
                    {
                        id = r.CodigoApt,
                        label = r.CodigoApt,
                        title = r.NombreApt,
                        tipoNodo = "App",
                        color = colorNodeAppRelacionada,
                        level = ((pag.Nivel == 1 ? 1 : 2) - 1)
                    };
                    grafoNodes.Add(node);
                }

                var existeEdge = grafoEdges.Count(e => e.from == componente[0].ComponenteId.ToString() && e.to == r.CodigoApt);
                if (existeEdge == 0)
                {
                    var colorEdge = r.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = r.CodigoApt,
                        to = nodePrincipal.id,
                        label = r.TipoRelacionDesc,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }
            }

            if (pag.Nivel == 2)
            {
                var appsDependencias = new List<AppsDependenciaImpacto>();
                var appsImpactos = new List<AppsDependenciaImpacto>();

                if (tieneOwner)
                {
                    appsImpactos = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(componente[0].CodigoAptOwner, "I", pag.TipoEtiquetaId, pag.TipoEtiquetaId);

                    foreach (var i in appsImpactos)
                    {
                        var existeNodo = grafoNodes.Count(x => x.id == i.CodigoAptDestino);
                        if (existeNodo == 0)
                        {
                            var node = new GrafoNodeDTO()
                            {
                                id = i.CodigoAptDestino,
                                label = i.CodigoAptDestino,
                                title = i.NombreAptDestino,
                                tipoNodo = "App",
                                color = colorNodeAppRelacionada,
                                level = (pag.Nivel + 2)
                            };
                            grafoNodes.Add(node);
                        }

                        var existeEdge = grafoEdges.Count(e => e.from == componente[0].CodigoAptOwner && e.to == i.CodigoAptDestino);
                        if (existeEdge == 0)
                        {
                            var colorEdge = i.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                            var edge = new GrafoEdgeDTO()
                            {
                                from = componente[0].CodigoAptOwner,
                                to = i.CodigoAptDestino,
                                label = i.TipoRelacionDesc,
                                color = colorEdge,
                                length = "200"
                            };
                            grafoEdges.Add(edge);
                        }
                    }

                }

                foreach (var r in relaciones)
                {
                    appsDependencias = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(r.CodigoApt, "D", pag.TipoRelacionId, pag.TipoEtiquetaId);

                    if (appsDependencias.Count > 0)
                    {
                        foreach (var d in appsDependencias)
                        {
                            var existeNodo = grafoNodes.Count(x => x.id == d.CodigoAptOrigen);
                            if (existeNodo == 0)
                            {
                                var node = new GrafoNodeDTO()
                                {
                                    id = d.CodigoAptOrigen,
                                    label = d.CodigoAptOrigen,
                                    title = d.NombreAptOrigen,
                                    tipoNodo = "App",
                                    color = colorNodeAppRelacionada,
                                    level = (pag.Nivel - 2)
                                };
                                grafoNodes.Add(node);
                            }

                            var existeEdge = grafoEdges.Count(e => e.from == d.CodigoAptOrigen && e.to == r.CodigoApt);
                            if (existeEdge == 0)
                            {
                                var colorEdge = d.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                                var edge = new GrafoEdgeDTO()
                                {
                                    from = d.CodigoAptOrigen,
                                    to = r.CodigoApt,
                                    label = d.TipoRelacionDesc,
                                    color = colorEdge,
                                    length = "200"
                                };
                                grafoEdges.Add(edge);
                            }
                        }
                    }
                }
            }

            // Evidar redundancias
            grafoEdges = grafoEdges.Where(e => e.from != e.to).ToList();
            grafoNodes = grafoNodes.GroupBy(n => n.id).Select(n => n.First()).ToList();

            grafo.nodes = grafoNodes;
            grafo.edges = grafoEdges;

            if (grafo == null)
                return NotFound();

            return Ok(grafo);
        }

        [Route("VGD-App")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependenciesAppsGrafo(PaginacionVGD pag)
        {
            if (String.IsNullOrEmpty(pag.codigoAPT))
            {
                return BadRequest();
            }

            var n1Dependencias = new List<AppsDependenciaImpacto>();
            var n1Impactos = new List<AppsDependenciaImpacto>();

            n1Dependencias = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(pag.codigoAPT, "D", pag.TipoRelacionId, pag.TipoEtiquetaId);
            n1Impactos = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(pag.codigoAPT, "I", pag.TipoRelacionId, pag.TipoEtiquetaId);

            if (n1Dependencias.Count() <= 0 && n1Impactos.Count() <= 0)
            {
                return NotFound();
            }

            // Parametros
            var colorEdgeRelacion = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_RELACION").Valor;
            var colorEdgeDependencia = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_DEPENDENCIA").Valor;
            var colorNodeAppRelacionada = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_CONSULTADA").Valor;
            var colorNodeAppOwner = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_OWNER").Valor;
            var colorAppConsultada = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_PRINCIPAL").Valor;

            GrafoDTO grafo = new GrafoDTO();
            List<GrafoNodeDTO> grafoNodes = new List<GrafoNodeDTO>();
            List<GrafoEdgeDTO> grafoEdges = new List<GrafoEdgeDTO>();

            // Nodo principal
            var nodePrincipal = new GrafoNodeDTO()
            {
                id = pag.codigoAPT,
                label = pag.codigoAPT,
                title = n1Dependencias.Count() > 0 ? n1Dependencias[0].NombreAptDestino : n1Impactos[0].NombreAptOrigen,
                tipoNodo = "App",
                color = colorAppConsultada,
                level = (pag.Nivel == 1 ? 1 : 2)
            };
            grafoNodes.Add(nodePrincipal);

            // Dependencias del Nodo Principal 
            foreach (var r in n1Dependencias)
            {
                var existeNodo = grafoNodes.Count(x => x.id == r.CodigoAptOrigen);
                if (existeNodo == 0)
                {
                    var node = new GrafoNodeDTO()
                    {
                        id = r.CodigoAptOrigen,
                        label = r.CodigoAptOrigen,
                        title = r.NombreAptOrigen,
                        tipoNodo = "App",
                        color = colorNodeAppRelacionada,
                        level = ((pag.Nivel == 1 ? 1 : 2) - 1)
                    };
                    grafoNodes.Add(node);
                }

                var existeEdge = grafoEdges.Count(e => e.from == r.CodigoAptOrigen && e.to == nodePrincipal.id);
                if (existeEdge == 0)
                {
                    var colorEdge = r.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = r.CodigoAptOrigen,
                        to = nodePrincipal.id,
                        label = r.TipoRelacionDesc,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }
            }

            // Impactos del Nodo Principal 
            foreach (var r in n1Impactos)
            {
                var existeNodo = grafoNodes.Count(x => x.id == r.CodigoAptDestino);
                if (existeNodo == 0)
                {
                    var node = new GrafoNodeDTO()
                    {
                        id = r.CodigoAptDestino,
                        label = r.CodigoAptDestino,
                        title = r.NombreAptDestino,
                        tipoNodo = "App",
                        color = colorNodeAppRelacionada,
                        level = ((pag.Nivel == 1 ? 1 : 2) + 1)
                    };
                    grafoNodes.Add(node);
                }

                var existeEdge = grafoEdges.Count(e => e.from == nodePrincipal.id && e.to == r.CodigoAptDestino);
                if (existeEdge == 0)
                {
                    var colorEdge = r.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = nodePrincipal.id,
                        to = r.CodigoAptDestino,
                        label = r.TipoRelacionDesc,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }
            }

            if (pag.Nivel == 2)
            {
                var n2Dependencias = new List<AppsDependenciaImpacto>();
                var n2Impactos = new List<AppsDependenciaImpacto>();

                List<GrafoNodeDTO> grafoNodes2 = new List<GrafoNodeDTO>();

                foreach (var dep in n1Dependencias)
                {
                    n2Dependencias = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(dep.CodigoAptOrigen, "D", pag.TipoRelacionId, pag.TipoEtiquetaId);

                    if (n2Dependencias.Count > 0)
                    {
                        foreach (var d in n2Dependencias)
                        {
                            var existeNodo = grafoNodes.Count(x => x.id == d.CodigoAptOrigen);
                            if (existeNodo == 0)
                            {
                                var node = new GrafoNodeDTO()
                                {
                                    id = d.CodigoAptOrigen,
                                    label = d.CodigoAptOrigen,
                                    title = d.NombreAptOrigen,
                                    tipoNodo = "App",
                                    color = colorNodeAppRelacionada,
                                    level = (pag.Nivel - pag.Nivel)
                                };
                                grafoNodes2.Add(node);
                            }

                            var existeEdge = grafoEdges.Count(e => e.from == d.CodigoAptOrigen && e.to == dep.CodigoAptOrigen);
                            if (existeEdge == 0)
                            {
                                var colorEdge = d.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                                var edge = new GrafoEdgeDTO()
                                {
                                    from = d.CodigoAptOrigen,
                                    to = dep.CodigoAptOrigen,
                                    label = d.TipoRelacionDesc,
                                    color = colorEdge,
                                    length = "200"
                                };
                                grafoEdges.Add(edge);
                            }
                        }
                    }
                }

                foreach (var imp in n1Impactos)
                {
                    n2Impactos = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(imp.CodigoAptDestino, "I", pag.TipoRelacionId, pag.TipoEtiquetaId);

                    if (n2Impactos.Count > 0)
                    {
                        foreach (var i in n2Impactos)
                        {
                            var existeNodo = grafoNodes.Count(x => x.id == i.CodigoAptDestino);
                            if (existeNodo == 0)
                            {
                                var node = new GrafoNodeDTO()
                                {
                                    id = i.CodigoAptDestino,
                                    label = i.CodigoAptDestino,
                                    title = i.NombreAptDestino,
                                    tipoNodo = "App",
                                    color = colorNodeAppRelacionada,
                                    level = (pag.Nivel + 2)
                                };
                                grafoNodes2.Add(node);
                            }

                            var existeEdge = grafoEdges.Count(e => e.from == imp.CodigoAptDestino && e.to == i.CodigoAptDestino);
                            if (existeEdge == 0)
                            {
                                var colorEdge = i.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                                var edge = new GrafoEdgeDTO()
                                {
                                    from = imp.CodigoAptDestino,
                                    to = i.CodigoAptDestino,
                                    label = i.TipoRelacionDesc,
                                    color = colorEdge,
                                    length = "200"
                                };
                                grafoEdges.Add(edge);
                            }
                        }
                    }
                }


                if (grafoNodes2.Count > 0)
                {
                    grafoNodes = grafoNodes.Union(grafoNodes2).ToList();
                }
            }

            // Evidar redundancias
            grafoEdges = grafoEdges.Where(e => e.from != e.to).ToList();
            grafoNodes = grafoNodes.GroupBy(n => n.id).Select(n => n.First()).ToList();

            grafo.nodes = grafoNodes;
            grafo.edges = grafoEdges;

            if (grafo == null)
                return NotFound();

            return Ok(grafo);
        }

        [Route("VGD-AppToApp")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetDependenciesAppToAppGrafo(PaginacionVGD pag)
        {
            if (pag.RelacionId.Equals(null) || pag.RelacionId == 0)
            {
                return BadRequest();
            }

            var relaciones = new List<RelacionAppToApp>();

            relaciones = ServiceManager<DependenciasDAO>.Provider.GetRelacionApps(pag.RelacionId);

            // Parametros
            var colorEdgeRelacion = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_RELACION").Valor;
            var colorEdgeDependencia = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_EDGE_DEPENDENCIA").Valor;
            var colorNodeAppRelacionada = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_CONSULTADA").Valor;
            var colorNodeAppOwner = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DEPENDENCIAS_APPS_GRAFOS_COLOR_APP_OWNER").Valor;

            GrafoDTO grafo = new GrafoDTO();
            List<GrafoNodeDTO> grafoNodes = new List<GrafoNodeDTO>();
            List<GrafoEdgeDTO> grafoEdges = new List<GrafoEdgeDTO>();

            foreach (var r in relaciones)
            {
                // CodigoAptOrigen
                var appOrigen = new GrafoNodeDTO()
                {
                    id = r.CodigoAptOrigen,
                    label = r.CodigoAptOrigen,
                    title = r.NombreAptOrigen,
                    tipoNodo = "App",
                    color = colorNodeAppRelacionada,
                    level = (pag.Nivel == 1 ? 0 : 1)
                };
                grafoNodes.Add(appOrigen);

                // EquipoOrigen
                var equipoOrigen = new GrafoNodeDTO()
                {
                    id = r.NomEquipoOrigen,
                    label = r.NomEquipoOrigen,
                    title = r.EqOrigen_SO,
                    tipoNodo = (r.TipoEquipoIdOrigen == 1 || r.TipoEquipoIdOrigen == 2) ? "Servidor" : r.TipoEquipoIdOrigen == 9 ? "Api" : "",
                    color = colorNodeAppRelacionada,
                    level = (pag.Nivel == 1 ? 1 : 2)
                };
                grafoNodes.Add(equipoOrigen);

                // Origen App-Equipo
                var existeEdgeOrigen = grafoEdges.Count(e => e.from == r.CodigoAptOrigen && e.to == r.NomEquipoOrigen);
                if (existeEdgeOrigen == 0)
                {
                    var colorEdge = r.TipoRelAppEqOrigen_Desc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = r.CodigoAptOrigen,
                        to = r.NomEquipoOrigen,
                        label = r.TipoRelAppEqOrigen_Desc,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }

                // CodigoAptDestino
                var appDestino = new GrafoNodeDTO()
                {
                    id = r.CodigoAptDestino,
                    label = r.CodigoAptDestino,
                    title = r.NombreAptDestino,
                    tipoNodo = "App",
                    color = colorNodeAppRelacionada,
                    level = (pag.Nivel == 1 ? 3 : 4)
                };
                grafoNodes.Add(appDestino);

                // EquipoDestino
                var equipoDestino = new GrafoNodeDTO()
                {
                    id = r.NomEquipoDestino,
                    label = r.NomEquipoDestino,
                    title = r.EqDestino_SO,
                    tipoNodo = (r.TipoEquipoIdOrigen == 1 || r.TipoEquipoIdOrigen == 2) ? "Servidor" : r.TipoEquipoIdOrigen == 9 ? "Api" : "",
                    color = colorNodeAppRelacionada,
                    level = (pag.Nivel == 1 ? 2 : 3)
                };
                grafoNodes.Add(equipoDestino);

                // Destino App-Equipo
                var existeEdgeDestino = grafoEdges.Count(e => e.from == r.CodigoAptDestino && e.to == r.NomEquipoDestino);
                if (existeEdgeDestino == 0)
                {
                    var colorEdge = r.TipoRelAppEqDestino_Desc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = r.CodigoAptDestino,
                        to = r.NomEquipoDestino,
                        label = r.TipoRelAppEqDestino_Desc,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }

                // Origen<>Destino
                var existeEdge = grafoEdges.Count(e => e.from == r.NomEquipoOrigen && e.to == r.NomEquipoDestino);
                if (existeEdge == 0)
                {
                    var colorEdge = r.DescTipoRel_App == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = r.NomEquipoOrigen,
                        to = r.NomEquipoDestino,
                        label = r.DescTipoRel_App,
                        color = colorEdge,
                        length = "200"
                    };
                    grafoEdges.Add(edge);
                }
            }

            if (relaciones.Count() == 1
                && relaciones[0].EquipoOrigen == 0
                && String.IsNullOrEmpty(relaciones[0].NomEquipoOrigen.ToString())
                && relaciones[0].EquipoDestino == 0
                && String.IsNullOrEmpty(relaciones[0].NomEquipoDestino.ToString()))
            {
                grafoEdges.Clear();

                var existeEdge = grafoEdges.Count(e => e.from == relaciones[0].CodigoAptOrigen && e.to == relaciones[0].CodigoAptDestino);
                if (existeEdge == 0)
                {
                    var colorEdge = relaciones[0].DescTipoRel_App == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                    var edge = new GrafoEdgeDTO()
                    {
                        from = relaciones[0].CodigoAptOrigen,
                        to = relaciones[0].CodigoAptDestino,
                        label = relaciones[0].DescTipoRel_App,
                        color = colorEdge,
                        length = "100"
                    };
                    grafoEdges.Add(edge);
                }
            }

            if (pag.Nivel == 2)
            {
                var n2Dependencias = new List<AppsDependenciaImpacto>();
                var n2Impactos = new List<AppsDependenciaImpacto>();

                List<GrafoNodeDTO> grafoNodes2 = new List<GrafoNodeDTO>();

                n2Dependencias = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(relaciones[0].CodigoAptOrigen, "D", pag.TipoRelacionId, pag.TipoEtiquetaId);

                if (n2Dependencias.Count > 0)
                {
                    foreach (var d in n2Dependencias)
                    {
                        var existeNodo = grafoNodes.Count(x => x.id == d.CodigoAptOrigen);
                        if (existeNodo == 0)
                        {
                            var node = new GrafoNodeDTO()
                            {
                                id = d.CodigoAptOrigen,
                                label = d.CodigoAptOrigen,
                                title = d.NombreAptOrigen,
                                tipoNodo = "App",
                                color = colorNodeAppRelacionada,
                                level = (pag.Nivel - pag.Nivel)
                            };
                            grafoNodes2.Add(node);
                        }

                        var existeEdge = grafoEdges.Count(e => e.from == d.CodigoAptOrigen && e.to == relaciones[0].CodigoAptOrigen);
                        if (existeEdge == 0)
                        {
                            var colorEdge = d.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                            var edge = new GrafoEdgeDTO()
                            {
                                from = d.CodigoAptOrigen,
                                to = relaciones[0].CodigoAptOrigen,
                                label = d.TipoRelacionDesc,
                                color = colorEdge,
                                length = "200"
                            };
                            grafoEdges.Add(edge);
                        }
                    }
                }

                n2Impactos = ServiceManager<DependenciasDAO>.Provider.GetAppsDependenciaImpacto(relaciones[0].CodigoAptDestino, "I", pag.TipoRelacionId, pag.TipoEtiquetaId);

                if (n2Impactos.Count > 0)
                {
                    foreach (var i in n2Impactos)
                    {
                        var existeNodo = grafoNodes.Count(x => x.id == i.CodigoAptDestino);
                        if (existeNodo == 0)
                        {
                            var node = new GrafoNodeDTO()
                            {
                                id = i.CodigoAptDestino,
                                label = i.CodigoAptDestino,
                                title = i.NombreAptDestino,
                                tipoNodo = "App",
                                color = colorNodeAppRelacionada,
                                level = (pag.Nivel + 3)
                            };
                            grafoNodes2.Add(node);
                        }

                        var existeEdge = grafoEdges.Count(e => e.from == relaciones[0].CodigoAptDestino && e.to == i.CodigoAptDestino);
                        if (existeEdge == 0)
                        {
                            var colorEdge = i.TipoRelacionDesc == "Dependencia" ? colorEdgeDependencia : colorEdgeRelacion;

                            var edge = new GrafoEdgeDTO()
                            {
                                from = relaciones[0].CodigoAptDestino,
                                to = i.CodigoAptDestino,
                                label = i.TipoRelacionDesc,
                                color = colorEdge,
                                length = "200"
                            };
                            grafoEdges.Add(edge);
                        }
                    }
                }


                if (grafoNodes2.Count > 0)
                {
                    grafoNodes = grafoNodes.Union(grafoNodes2).ToList();
                }
            }

            // Evidar redundancias
            grafoEdges = grafoEdges.Where(e => e.from != e.to).ToList();
            grafoNodes = grafoNodes.GroupBy(n => n.id).Select(n => n.First()).ToList();

            grafo.nodes = grafoNodes;
            grafo.edges = grafoEdges;

            if (grafo == null)
                return NotFound();

            return Ok(grafo);
        }
        #endregion

        #region Diagrama de Infraestructura
        [Route("BuscarDataDiagInfra")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult BuscarDataDiagInfra(Paginacion pag)
        {
            //var dataRpta = "";
            //dataRpta = ServiceManager<DependenciasDAO>.Provider.ActualizarEstadoServidores(pag);
            var lstReglasGenerales = ServiceManager<DependenciasDAO>.Provider.GetReglasGenerales();
            var lstReglasPorApp_Activos = ServiceManager<DependenciasDAO>.Provider.ListarReglasPorApp(pag);
            var lstReglasPorApp_Activos_ServidoresNoRel = lstReglasPorApp_Activos.Where(item => item.TipoEquipoId == 1 && item.Relacionado == false && item.FlagIncluir_Componente == true).ToList();
            var lstReglasPorApp_Activos_RecursosNubeNoRel = lstReglasPorApp_Activos.Where(item => item.TipoEquipoId == 4 && item.Relacionado == false && item.FlagIncluir_Componente == true).ToList();

            var registros_Apis = ServiceManager<DependenciasDAO>.Provider.GetListApis(pag, out int totalRows_Apis);

            if (lstReglasPorApp_Activos.Count == 0 && totalRows_Apis == 0)
                return NotFound();

            // Parametros
            var incluirNoRelacionados = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DIAGRAMA_INCLUIR_NO_RELACIONADOS").Valor;
            var incluirMainframe = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DIAGRAMA_INCLUIR_MAINFRAME").Valor;
            var strGrafoConfig = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("DIAGRAMA_CONFIGURACION_JSON").Valor;

            GrafoDTO grafo = new GrafoDTO();
            List<GrafoNodeDTO> grafoNodes = new List<GrafoNodeDTO>();
            List<GrafoEdgeDTO> grafoEdges = new List<GrafoEdgeDTO>();

            // Procesar Servidores
            if (lstReglasPorApp_Activos.Count > 0)
            {
                foreach (var r in lstReglasPorApp_Activos)
                {
                    var existeNodo = grafoNodes.Count(x => x.id == r.EquipoId.ToString());
                    if (existeNodo == 0 && r.FlagIncluir_Componente == true)
                    {
                        var node = new GrafoNodeDTO()
                        {
                            id = r.EquipoId.ToString(),
                            label = r.Nombre,
                            title = IdentificarToolTip(r),
                            tipoNodo = IdentificarIcono(r),
                            group = "1"
                        };
                        grafoNodes.Add(node);
                    }
                    if (r.Relacionado == false)
                    {
                        continue;
                    }
                    var existeNodo_Relacionado = grafoNodes.Count(y => y.id == r.EquipoId_Relacion.ToString());
                    if (existeNodo_Relacionado == 0 && r.FlagIncluir_ConectaCon == true)
                    {
                        var node_Relacionado = new GrafoNodeDTO()
                        {
                            id = r.EquipoId_Relacion.ToString(),
                            label = r.Nombre_Relacion,
                            title = IdentificarToolTip_Relacion(r),
                            tipoNodo = IdentificarIcono_Relacion(r),
                            group = "1"
                        };
                        grafoNodes.Add(node_Relacionado);
                    }
                    var existeEdge = grafoEdges.Count(e => (e.from == r.EquipoId.ToString() && e.to == r.EquipoId_Relacion.ToString()) || (e.from == r.EquipoId_Relacion.ToString() && e.to == r.EquipoId.ToString()));
                    if (existeEdge == 0 && r.FlagIncluir_Componente == true && r.FlagIncluir_ConectaCon == true)
                    {
                        var colorEdge = "#4472c4";

                        var edge = new GrafoEdgeDTO()
                        {
                            from = r.EquipoId.ToString(),
                            to = r.EquipoId_Relacion.ToString(),
                            color = colorEdge,
                            length = "100",
                            arrows = ""
                        };
                        grafoEdges.Add(edge);
                    }
                }
                // > Procesar Reglas Generales
                if (lstReglasGenerales.Count > 0)
                {
                    foreach (var i in lstReglasGenerales)
                    {
                        foreach (var j in lstReglasPorApp_Activos_ServidoresNoRel)
                        {
                            if (j.Funcion == null)
                            {
                                continue;
                            }
                            if (i.AplicaEn == j.TipoEquipoId && j.Funcion.ToUpper().Equals(i.Origen.ToUpper()))
                            {
                                var relacionadosFuncion = lstReglasPorApp_Activos_ServidoresNoRel.FindAll(x => x.Funcion.ToUpper().Equals(i.Destino.ToUpper()));
                                foreach (var r in relacionadosFuncion)
                                {
                                    var existeEdge = grafoEdges.Count(e => (e.from == j.EquipoId.ToString() && e.to == r.EquipoId.ToString()) || (e.from == r.EquipoId.ToString() && e.to == j.EquipoId.ToString()));
                                    if (existeEdge == 0)
                                    {
                                        var colorEdge = "#4472c4";

                                        var edge = new GrafoEdgeDTO()
                                        {
                                            from = j.EquipoId.ToString(),
                                            to = r.EquipoId.ToString(),
                                            color = colorEdge,
                                            length = "100",
                                            arrows = ""
                                        };
                                        grafoEdges.Add(edge);
                                    }
                                }
                            }
                        }
                        foreach (var j in lstReglasPorApp_Activos_RecursosNubeNoRel)
                        {
                            if (j.TecPrincipal == null)
                            {
                                continue;
                            }
                            if (i.AplicaEn == j.TipoEquipoId && j.TecPrincipal.ToUpper().Equals(i.Origen.ToUpper()))
                            {
                                var relacionadosFuncion = lstReglasPorApp_Activos_RecursosNubeNoRel.FindAll(x => x.TecPrincipal.ToUpper().Equals(i.Destino.ToUpper()));
                                foreach (var r in relacionadosFuncion)
                                {
                                    var existeEdge = grafoEdges.Count(e => (e.from == j.EquipoId.ToString() && e.to == r.EquipoId.ToString()) || (e.from == r.EquipoId.ToString() && e.to == j.EquipoId.ToString()));
                                    if (existeEdge == 0)
                                    {
                                        var colorEdge = "#4472c4";

                                        var edge = new GrafoEdgeDTO()
                                        {
                                            from = j.EquipoId.ToString(),
                                            to = r.EquipoId.ToString(),
                                            color = colorEdge,
                                            length = "100",
                                            arrows = ""
                                        };
                                        grafoEdges.Add(edge);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Procesar Apis
            if (registros_Apis.Count > 0)
            {
                foreach (var r in registros_Apis)
                {
                    var existeNodo = grafoNodes.Count(x => x.id == r.EquipoId.ToString());
                    if (existeNodo == 0)
                    {
                        var node = new GrafoNodeDTO()
                        {
                            id = r.EquipoId.ToString(),
                            label = r.NombreApi,
                            title = r.Owner + " : " + r.NombreOwner,
                            tipoNodo = "Api",
                            group = "2"
                        };
                        grafoNodes.Add(node);
                    }

                    // to-do : Relaciones entre Apis
                }
            }

            // Evidar redundancias
            grafoEdges = grafoEdges.Where(e => e.from != e.to).ToList();
            grafoNodes = grafoNodes.GroupBy(n => n.id).Select(n => n.First()).ToList();

            if (incluirNoRelacionados == "0")
            {
                if (grafoEdges.Count > 0)
                {
                    List<GrafoNodeDTO> grafoNodesRelacionados = new List<GrafoNodeDTO>();

                    foreach (var item in grafoEdges)
                    {
                        grafoNodesRelacionados.Add(grafoNodes.Where(g => g.id == item.from).First());
                        grafoNodesRelacionados.Add(grafoNodes.Where(g => g.id == item.to).First());
                    }
                    grafoNodes = grafoNodesRelacionados.GroupBy(n => n.id).Select(n => n.First()).ToList();

                    if (incluirMainframe == "1")
                    {
                        // Agregar CICS
                        List<GrafoNodeDTO> NodesCICS = new List<GrafoNodeDTO>();
                        NodesCICS = grafoNodes.Where(e => e.tipoNodo == "cics").ToList();
                        if (NodesCICS.Count > 0)
                        {
                            var node = new GrafoNodeDTO()
                            {
                                id = "node_CICS",
                                label = "Mainframe",
                                title = "Mainframe",
                                tipoNodo = "mainframe",
                                group = "1"
                            };
                            grafoNodes.Add(node);
                            foreach (var item in NodesCICS)
                            {
                                var colorEdge = "#4472c4";

                                var edge = new GrafoEdgeDTO()
                                {
                                    from = item.id,
                                    to = "node_CICS",
                                    color = colorEdge,
                                    length = "100",
                                    arrows = ""
                                };
                                grafoEdges.Add(edge);
                            }

                        }

                        // Grafo a mostrar
                        grafo.nodes = grafoNodes;
                        grafo.edges = grafoEdges;
                    }
                    else if (incluirMainframe == "0")
                    {
                        // Grafo a mostrar
                        grafo.nodes = grafoNodes;
                        grafo.edges = grafoEdges;
                    }
                }
            }
            else if (incluirNoRelacionados == "1")
            {
                if (incluirMainframe == "1")
                {
                    // Agregar CICS
                    List<GrafoNodeDTO> NodesCICS = new List<GrafoNodeDTO>();
                    NodesCICS = grafoNodes.Where(e => e.tipoNodo == "cics").ToList();
                    if (NodesCICS.Count > 0)
                    {
                        var node = new GrafoNodeDTO()
                        {
                            id = "node_CICS",
                            label = "Mainframe",
                            title = "Mainframe",
                            tipoNodo = "mainframe",
                            group = "1"
                        };
                        grafoNodes.Add(node);
                        foreach (var item in NodesCICS)
                        {
                            var colorEdge = "#4472c4";

                            var edge = new GrafoEdgeDTO()
                            {
                                from = item.id,
                                to = "node_CICS",
                                color = colorEdge,
                                length = "100",
                                arrows = ""
                            };
                            grafoEdges.Add(edge);
                        }

                    }
                    // Grafo a mostrar
                    grafo.nodes = grafoNodes;
                    grafo.edges = grafoEdges;

                }
                else if (incluirMainframe == "0")
                {
                    // Grafo a mostrar
                    grafo.nodes = grafoNodes;
                    grafo.edges = grafoEdges;
                }
            }

            if (grafo.nodes == null && grafo.edges == null)
            {
                return NotFound();
            }

            dynamic jsonGrafoConfig = JsonConvert.DeserializeObject(strGrafoConfig);
            grafo.options = jsonGrafoConfig;
            return Ok(grafo);
        }

        private string IdentificarToolTip(RelacionReglasPorAppDiagramaDTO r)
        {
            string resultado = "";
            if (r.TipoEquipoId == 8 || r.TipoEquipoId == 11 || r.TipoEquipoId == 12)
            {
                resultado = r.Nombre;
            }
            else if (r.TipoEquipoId == 4)
            {
                resultado = r.TecPrincipal;
            }
            else
            {
                if (r.ListaIpEquipo == "")
                {
                    resultado = r.TecPrincipal;
                }
                else
                {
                    resultado = r.TecPrincipal + " - " + r.ListaIpEquipo;
                }
            }
            return resultado;
        }
        private string IdentificarToolTip_Relacion(RelacionReglasPorAppDiagramaDTO r)
        {
            string resultado = "";
            if (r.TipoEquipoId_Relacion == 8 || r.TipoEquipoId_Relacion == 11 || r.TipoEquipoId_Relacion == 12)
            {
                resultado = r.Nombre_Relacion;
            }
            else if (r.TipoEquipoId_Relacion == 4)
            {
                resultado = r.TecPrincipal_Relacion;
            } 
            else
            {
                if (r.ListaIpEquipo_Relacion == "")
                {
                    resultado = r.TecPrincipal_Relacion;
                }
                else
                {
                    resultado = r.TecPrincipal_Relacion + " - " + r.ListaIpEquipo_Relacion;
                }
            }
            return resultado;
        }
        private string IdentificarIcono(RelacionReglasPorAppDiagramaDTO r)
        {
            string resultado = "";

            switch (r.TipoEquipoId)
            {
                case 1:
                    if (r.Nombre.ToUpper().Contains("CICS") && r.TecPrincipal.ToUpper().Contains("IBM"))
                    {
                        resultado = "cics";
                    }
                    else
                    {
                        resultado = "Servidor";
                    }
                    break;
                case 2:
                    resultado = "servidor-agencia";
                    break;
                case 3:
                    resultado = "pc";
                    break;
                case 4:
                    //resultado = "cloud";
                    resultado = r.TecPrincipal;
                    break;
                case 5:
                    resultado = "storage";
                    break;
                case 6:
                    resultado = "appliance";
                    break;
                case 7:
                    resultado = "redes-comunicaciones";
                    break;
                case 8:
                    resultado = "certificado-digital";
                    break;
                case 9:
                    resultado = "Api";
                    break;
                case 10:
                    resultado = "client-secret";
                    break;
                case 11:
                    resultado = "Tec";
                    break;
                case 12:
                    resultado = "Aplicacion";
                    break;
                default:
                    resultado = "Aplicacion";
                    break;
            }
            return resultado;
        }
        private string IdentificarIcono_Relacion(RelacionReglasPorAppDiagramaDTO r)
        {
            string resultado = "";

            switch (r.TipoEquipoId_Relacion)
            {
                case 1:
                    if (r.Nombre_Relacion.ToUpper().Contains("CICS") && r.TecPrincipal_Relacion.ToUpper().Contains("IBM"))
                    {
                        resultado = "cics";
                    }
                    else
                    {
                        resultado = "Servidor";
                    }
                    break;
                case 2:
                    resultado = "servidor-agencia";
                    break;
                case 3:
                    resultado = "pc";
                    break;
                case 4:
                    //resultado = "cloud";
                    resultado = r.TecPrincipal_Relacion;
                    break;
                case 5:
                    resultado = "storage";
                    break;
                case 6:
                    resultado = "appliance";
                    break;
                case 7:
                    resultado = "redes-comunicaciones";
                    break;
                case 8:
                    resultado = "certificado-digital";
                    break;
                case 9:
                    resultado = "Api";
                    break;
                case 10:
                    resultado = "client-secret";
                    break;
                case 11:
                    resultado = "Tec";
                    break;
                case 12:
                    resultado = "Aplicacion";
                    break;
                default:
                    resultado = "Aplicacion";
                    break;
            }
            return resultado;
        }
        #endregion
    }
}