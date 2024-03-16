using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.CargaMasiva;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using BCP.PAPP.Common.Dto;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.PAPP.Common.Custom;
using BCP.PAPP.Common.Cross;
using Newtonsoft.Json;

using log4net;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/applicationportfolio")]
    public class ApplicationPortfolioController : BaseController
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [Route("")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostListPublicacionAplicacion(int origin, string originTable, int pageSize, int pageNumber)
        {
            var pag = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = originTable,
                Procedencia = origin,
                Gerencia = string.Empty,
                Division = string.Empty,
                Unidad = string.Empty,
                Area = string.Empty,
                Estado = string.Empty,
                ClasificacionTecnica = string.Empty,
                SubclasificacionTecnica = string.Empty,
                Aplicacion = string.Empty,
                pageSize = pageSize,
                pageNumber = pageNumber
            };
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<AplicacionDAO>.Provider.GetPublicacionAplicacion(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }

        [Route("application/stepone")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationStepOne(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.registerByEmail = usr.CorreoElectronico;
            obj.registerByName = usr.Nombres;
            obj.Matricula = usr.Matricula;
            obj.NombreUsuarioModificacion = usr.Nombres;

            HttpResponseMessage response = null;

            var usuarios = new List<ApplicationManagerCatalogDto>();

            try
            {
                var azureManger = new AzureGroupsManager();
                var parametroDevSecOps = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_DEVSECOPS");
                var usuariosDevSecOps = new AzureGroupsManager().GetGroupMembersByName(parametroDevSecOps.Valor);
                if (usuariosDevSecOps != null)
                {
                    foreach (var item in usuariosDevSecOps)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.DevSecOps,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroArquitecturaTI = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ARQUITECTURA_TI");
                var usuariosArquitecturaTI = new AzureGroupsManager().GetGroupMembersByName(parametroArquitecturaTI.Valor);
                if (usuariosArquitecturaTI != null)
                {
                    foreach (var item in usuariosArquitecturaTI)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.ArquitectoTI,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroUsuariosAIO = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_AIO");
                var usuariosAIO = new AzureGroupsManager().GetGroupMembersByName(parametroUsuariosAIO.Valor);
                if (usuariosAIO != null)
                {
                    foreach (var item in usuariosAIO)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.AIO,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroGobierno = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_GOBIERNO_USERIT");
                var usuariosGobierno = new AzureGroupsManager().GetGroupMembersByName(parametroGobierno.Valor);
                if (usuariosGobierno != null)
                {
                    foreach (var item in usuariosGobierno)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.GobiernoUserIT,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

            }
            catch (Exception)
            {

            }

            var rolesGestion = ServiceManager<ActivosDAO>.Provider.GetRolesGestion();
            var managerid = 0;
            foreach (var user in rolesGestion)
            {
                managerid = 0;
                switch (user.RoleId)
                {
                    case (int)ERoles.AIO:
                        managerid = (int)ApplicationManagerRole.AIO;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.ArquitectoTecnologia:
                        managerid = (int)ApplicationManagerRole.ArquitectoTI;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.DevSecOps:
                        managerid = (int)ApplicationManagerRole.DevSecOps;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.GobiernoUserIT:
                        managerid = (int)ApplicationManagerRole.GobiernoUserIT;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    default: break;
                }
            }

            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

            var dataResult = ServiceManager<ApplicationDAO>.Provider.AddApplicationStepOne(obj, usuariosFinales);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);


            return response;
        }

        [Route("application/steptwo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationStepTwo(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.Matricula = usr.Matricula;
            obj.NombreUsuarioModificacion = usr.Nombres;
            obj.EmailSolicitante = usr.CorreoElectronico;

            HttpResponseMessage response = null;

            var usuarios = new List<ApplicationManagerCatalogDto>();
            try
            {
                var azureManger = new AzureGroupsManager();

                var parametroArquitecturaTI = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ARQUITECTURA_TI");
                var usuariosArquitecturaTI = new AzureGroupsManager().GetGroupMembersByName(parametroArquitecturaTI.Valor);
                if (usuariosArquitecturaTI != null)
                {
                    foreach (var item in usuariosArquitecturaTI)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.ArquitectoTI,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroGobierno = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_GOBIERNO_USERIT");
                var usuariosGobierno = new AzureGroupsManager().GetGroupMembersByName(parametroGobierno.Valor);
                if (usuariosGobierno != null)
                {
                    foreach (var item in usuariosGobierno)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.GobiernoUserIT,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var rolesGestion = ServiceManager<ActivosDAO>.Provider.GetRolesGestion();
                var managerid = 0;
                foreach (var user in rolesGestion)
                {
                    managerid = 0;
                    switch (user.RoleId)
                    {
                        case (int)ERoles.GobiernoUserIT:
                            managerid = (int)ApplicationManagerRole.GobiernoUserIT;
                            usuarios.Add(new ApplicationManagerCatalogDto()
                            {
                                applicationManagerId = managerid,
                                email = user.Email,
                                username = user.Username,
                                managerName = string.Empty
                            });
                            break;
                        default: break;
                    }
                }

            }
            catch (Exception)
            {

            }


            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationStepTwo(obj, usuariosFinales);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/steptwo/Update")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationStepTwoUpdate(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.NombreUsuarioAprobacion = usr.Nombres;
            obj.NombreUsuarioModificacion = usr.Nombres;
            obj.Matricula = usr.Matricula;
            obj.EmailSolicitante = usr.CorreoElectronico;

            HttpResponseMessage response = null;

            var usuarios = new List<ApplicationManagerCatalogDto>();
            try
            {
                var azureManger = new AzureGroupsManager();

                var parametroDevSecOps = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_DEVSECOPS");
                var usuariosDevSecOps = new AzureGroupsManager().GetGroupMembersByName(parametroDevSecOps.Valor);
                if (usuariosDevSecOps != null)
                {
                    foreach (var item in usuariosDevSecOps)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.DevSecOps,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroGobierno = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_GOBIERNO_USERIT");
                var usuariosGobierno = new AzureGroupsManager().GetGroupMembersByName(parametroGobierno.Valor);
                if (usuariosGobierno != null)
                {
                    foreach (var item in usuariosGobierno)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.GobiernoUserIT,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

            }
            catch (Exception)
            {

            }

            var rolesGestion = ServiceManager<ActivosDAO>.Provider.GetRolesGestion();
            var managerid = 0;
            foreach (var user in rolesGestion)
            {
                managerid = 0;
                switch (user.RoleId)
                {
                    case (int)ERoles.DevSecOps:
                        managerid = (int)ApplicationManagerRole.DevSecOps;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.GobiernoUserIT:
                        managerid = (int)ApplicationManagerRole.GobiernoUserIT;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    default: break;
                }
            }

            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

            try
            {
                var users = new List<ApplicationManagerCatalogDto>();

                var dataResult1 = ServiceManager<ApplicationDAO>.Provider.EditApplicationStepTwo2(obj, usuariosFinales);
                var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationStepTwo3(obj, users);

                ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);

                response = Request.CreateResponse(HttpStatusCode.OK, dataResult1);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("application/steptwo/UpdateUserIT")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationStepTwoUpdateUserIT(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.Matricula = user.Matricula;

            HttpResponseMessage response = null;

            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationStepTwo2UserIT(obj, null);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/steptwo/Update2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationStepTwoUpdate2(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.Matricula = usr.Matricula;
            obj.NombreUsuarioModificacion = usr.Nombres;

            HttpResponseMessage response = null;

            var usuarios = new List<ApplicationManagerCatalogDto>();
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationStepTwo3(obj, usuarios);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/ValidarModificacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ValidarModificacionApp(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.NombreUsuarioAprobacion = usr.Nombres;
            obj.NombreUsuarioModificacion = usr.Nombres;
            obj.Matricula = usr.Matricula;
            obj.EmailSolicitante = usr.CorreoElectronico;

            HttpResponseMessage response = null;

            var dataResult = ServiceManager<ApplicationDAO>.Provider.ValidarModificacion(obj);

            if (dataResult)
                response = Request.CreateResponse(HttpStatusCode.OK, dataResult);
            else
            {
                dataResult = ServiceManager<ApplicationDAO>.Provider.ValidarModificacion2(obj);
                response = Request.CreateResponse(HttpStatusCode.OK, dataResult);
            }

            return response;
        }

        [Route("application/ValidarModificacion2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ValidarModificacionApp2(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.NombreUsuarioAprobacion = usr.Nombres;
            obj.NombreUsuarioModificacion = usr.Nombres;
            obj.Matricula = usr.Matricula;
            obj.EmailSolicitante = usr.CorreoElectronico;

            HttpResponseMessage response = null;

            var dataResult = ServiceManager<ApplicationDAO>.Provider.ValidarModificacion2(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/evalarchitect")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationEvalArchitect(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.Matricula = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationEvalArchitect(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/evaluserit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationEvalUserIT(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationEvalUserIT(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/architectit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationArchitectIT(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.Matricula = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationArchitectIT(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/architectSolution")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationArchitectSolution(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationArchitectSolution(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/owner")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationOwner(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.Matricula = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationOwner(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/devsecops")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationDevSecOps(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationDevSecOps(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/teamleader")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationTeamLeader(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationTeamLeader(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/ttl")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationTTL(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.emailCustodio = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationTTL(obj);
            ServiceManager<ApplicationDAO>.Provider.ValidateRegister(obj.AppId);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/aio")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationAIO(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.Matricula = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.registerBy = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationEvalAIO(obj);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/exists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsApplication(string id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ApplicationDAO>.Provider.ExistsApplicationById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }


        [Route("application/existsInterface")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsApplicationInterface(string id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ApplicationDAO>.Provider.ExistsApplicationInterfaceById(id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("application/existsByName")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetExistsApplicationByName(string id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ApplicationDAO>.Provider.ExistsApplicationByName(id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("application/ExisteCodigoAPT")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteCodigoAPT(string codigo)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ApplicationDAO>.Provider.ExisteCodigoAPT(codigo);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        [Route("application/ExisteCodigoInterfaz")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteCodigoInterfaz(string codigo)
        {
            HttpResponseMessage response = null;
            string codigoValido = ServiceManager<ApplicationDAO>.Provider.ExisteCodigoInterfaz(codigo);
            response = Request.CreateResponse(HttpStatusCode.OK, codigoValido);
            return response;
        }

        [Route("application/changeStatus")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostChangeStatus(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.ChangeStatusApplication(pag.Id, pag.Status, pag.Username, pag.Comments, pag.PreviousState);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("application/changeStatusEliminado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostChangeStatusEliminado(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.ChangeStatusApplicationEliminada(pag.Id, pag.Username);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("application/reverseStatus")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReverseStatus(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.ReverseStatusApplication(pag.Id, pag.Status, pag.Username, pag.Comments);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("application/remove")]
        [HttpPost]
        [Authorize]
        //public HttpResponseMessage PostRemove(PaginationApplication pag)
        public HttpResponseMessage PostRemove()
        {
            var user = TokenGenerator.GetCurrentUser();

            HttpResponseMessage response = null;
            HttpRequest request = HttpContext.Current.Request;
            var pag = JsonConvert.DeserializeObject<PaginationApplication>(request.Form["data"]);
            pag.Username = user.Matricula;
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            if (request.Files.Count > 0)
            {
                string _nombre = string.Empty, _nombre2 = string.Empty, _nombre3 = string.Empty;
                byte[] _contenido = null;
                byte[] _contenido2 = null;
                byte[] _contenido3 = null;

                HttpPostedFile clientFile1 = null;
                clientFile1 = request.Files["File1"];
                if (clientFile1 != null)
                {
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

                pag.ConformidadGST = _contenido;
                pag.archivoConformidadNombre = _nombre;
                pag.TicketEliminacion = _contenido2;
                pag.archivoTicketEliminacionNombre = _nombre2;
                pag.Ratificacion = _contenido3;
                pag.archivoRatificacionNombre = _nombre3;
            }

            int id = ServiceManager<ApplicationDAO>.Provider.RemoveApplication(pag.Id, pag.Status, pag.Username, pag.Comments, pag.Matricula, pag.PreviousState, pag.NombreUsuarioModificacion, pag.Email,
            pag.flagRequiereConformidad, pag.ticketEliminacion, pag.expertoNombre, pag.expertoMatricula, pag.expertoCorreo, pag.tipoEliminacion
            ,pag.ConformidadGST, pag.archivoConformidadNombre,pag.TicketEliminacion,pag.archivoTicketEliminacionNombre, pag.Ratificacion, pag.archivoRatificacionNombre);


            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("application/removeAdmin")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRemoveAdmin(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int id = ServiceManager<ApplicationDAO>.Provider.RemoveApplicationAdmin(pag.Id, pag.Status, pag.Username, pag.Comments, pag.Matricula, pag.PreviousState, pag.NombreUsuarioModificacion, pag.Email,
                pag.flagRequiereConformidad, pag.ticketEliminacion, pag.expertoNombre, pag.expertoMatricula, pag.expertoCorreo, pag.tipoEliminacion);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
        [Route("application/updateSolicitud")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostUpdateSolicitud(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int id = ServiceManager<ApplicationDAO>.Provider.UpdateSolicitud(pag.Id, pag.Status, pag.Username, pag.Comments, pag.Matricula, pag.PreviousState, pag.NombreUsuarioModificacion, pag.Email,
                pag.flagRequiereConformidad, pag.ticketEliminacion, pag.expertoNombre, pag.expertoMatricula, pag.expertoCorreo, pag.tipoEliminacion);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("application/approveRemove")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApproveRemove(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int id = ServiceManager<ApplicationDAO>.Provider.ApproveRemoveApplication(pag.solId, pag.flowId, pag.Matricula, pag.NombreUsuarioModificacion, pag.Email);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("application/refuseRemove")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRefuseRemove(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int id = ServiceManager<ApplicationDAO>.Provider.RefuseRemoveApplication(pag.solId, pag.flowId, pag.Matricula, pag.NombreUsuarioModificacion, pag.Email, pag.Comments);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("application/observarRemove")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObservarRemove(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;
            pag.Email = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int id = ServiceManager<ApplicationDAO>.Provider.ObservarRemoveApplication(pag.solId, pag.flowId, pag.Matricula, pag.NombreUsuarioModificacion, pag.Email, pag.Comments);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }

        [Route("application/listByUser")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationsByUser(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationByUser(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.name = HttpUtility.HtmlEncode(x.name); });

            var reader = new BootstrapTable<ApplicationList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/listUserITApps")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationsUserIT(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationUserIT(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.assetTypeName = HttpUtility.HtmlEncode(x.assetTypeName);
                x.description = HttpUtility.HtmlEncode(x.description);
                x.name = HttpUtility.HtmlEncode(x.name);
            });

            var reader = new BootstrapTable<ApplicationList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/listApplicationFlow")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationsFlow(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlow(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ApplicationFlowDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationDetail(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }
        [Route("application/Solicitud/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSolicitudDetail(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetSolicitudById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/Solicitud2/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetSolicitudDetail2(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetSolicitudById2(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/Detail/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationDetail2(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationById2(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/CambioUnidad/{id:int},{UnidadId:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationUnidad(int id, int UnidadId)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationUnidadById(id, UnidadId);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/CambioOwner")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationOwner(PaginationApplication pag)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationOwnerById(pag.Id, pag.Username);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }


        [Route("application/UnidadesOwner")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetUnidadesOwner(PaginationApplication pag)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetUnidadesOwnerById(pag.Username);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/CambioArquitecto/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationJefaturaATI(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationJefaturaATIById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/GetFullDetail/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationFullDetail(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetFullApplicationById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/stepone/lists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsStepOne()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListStepOne();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/stepone/toolbox")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAppToolbox()
        {
            HttpResponseMessage response = null;
            var objToolbox = ServiceManager<ApplicationDAO>.Provider.GetAppToolbox();

            response = Request.CreateResponse(HttpStatusCode.OK, objToolbox);
            return response;
        }

        [Route("application/ValidateActivationJenkins")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ValidateActivationJenkins()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.ValidateActivationJenkins();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/steptwo/lists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsStepTwo()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListStepTwo();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/steptwo/infraestructure/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetInfraestructureByApp(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetLInfraestuctureByApp(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/steptwo/listsAdmin")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsStepTwoAdmin()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListStepTwo_Admin();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/requestsByUser")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetRequestsByUser(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.role = user.Perfil;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByUser(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (!string.IsNullOrWhiteSpace(item.assignedTo))
                {
                    userAD = azureManger.GetUserDataByMail(item.assignedTo);
                    if (userAD != null)
                        item.assignedToDetail = userAD.Nombres;
                }
            }

            registros.ForEach(x =>
            {
                x.applicationName = HttpUtility.HtmlEncode(x.applicationName);
                x.comments = HttpUtility.HtmlEncode(x.comments);
            });

            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("application/requestsByUserIT")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetRequestsByUserIT(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.role = user.Perfil;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByUserIT(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.comments = HttpUtility.HtmlEncode(x.comments); });

            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/lists/combosExterna")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsExterna()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListExterna();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/lists/architecteval")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoEvaluador()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoEvaluador();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/lists/architectevalExterna")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoEvaluadorExterna()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoEvaluadorExterna();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/listsAdmin/architecteval")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoEvaluadorAdmin()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoEvaluador_Admin();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/lists/architecteval/{area:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoEvaluador(int area)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoEvaluador(area);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/listsAdmin/architecteval/{area:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoEvaluadorAdmin(int area)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoEvaluador(area);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("flow/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetApplicationFlow(int id)
        {
            var objAmb = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/lists/devsecops")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsDevSecOps()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListDevSecOps();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/lists/architectit")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoTI()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoTI();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/lists/architectSolution")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoSolution()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoSolution();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }
        [Route("application/lists/owner")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsOwner()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListGetListsOwner();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/listsadmin/architectit")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectoTIAdmin()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoTI_Admin();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/managedteams/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTeamsSquad(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetEquipos(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/managedteamsAdmin/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetTeamsSquadAdmin(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetEquiposAdmin(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/technicalClassification/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubclasificacionTecnica(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListSubclasificacion(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/technicalClassificationAdmin/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetSubclasificacionTecnicaAdmin(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListSubclasificacionAdmin(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/requests/creation")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationRequestCreation(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetRequestAppCreation(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.name = HttpUtility.HtmlEncode(x.name);
                x.managedBy = HttpUtility.HtmlEncode(x.managedBy);
            });

            var reader = new BootstrapTable<ApplicationList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/requests/Asignadas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationRequestAsignadas(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.Username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetRequestAppAsignada(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.name = HttpUtility.HtmlEncode(x.name);
                x.assetTypeName = HttpUtility.HtmlEncode(x.assetTypeName);
            });

            var reader = new BootstrapTable<ApplicationList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("application/requests/AsignadasNoEliminadas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationRequestAsignadasNoEliminadas(PaginationApplication pag)
        {
            var horaInicio = Settings.Get<TimeSpan>("Restriccion.HoraInicio");
            var horaFin = Settings.Get<TimeSpan>("Restriccion.HoraFin");

            var horaActual = DateTime.Now.TimeOfDay;

            if (horaActual > horaInicio && horaActual < horaFin)
            {
                var reader = new BootstrapTable<ApplicationList>()
                {
                    Total = -1
                };

                return Ok(reader);
            }
            else
            {
                var user = TokenGenerator.GetCurrentUser();
                pag.Username = user.Matricula;
                pag.Matricula = user.Matricula;

                var totalRows = 0;

                var registros = ServiceManager<ApplicationDAO>.Provider.GetRequestAppAsignadaNoEliminadas(pag, out totalRows);

                if (registros == null)
                    return NotFound();

                registros.ForEach(x => { x.name = HttpUtility.HtmlEncode(x.name); });

                var reader = new BootstrapTable<ApplicationList>()
                {
                    Total = totalRows,
                    Rows = registros
                };

                return Ok(reader);
            }
        }
        [Route("ListarRelacionesAplicacionComponentes")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarRelacionesAplicacionComponentes(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.username = user.Matricula;
            objDTO.PerfilId = user.PerfilId;
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRelacionesAplicacionComponentes(objDTO, out totalRows);

            var reader = new BootstrapTable<RelacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("ListarRelacionesAplicacionAplicacion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarRelacionesAplicacionAplicacion(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.username = user.Matricula;
            objDTO.PerfilId = user.PerfilId;
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRelacionesAplicacionAplicacion(objDTO, out totalRows);

            var reader = new BootstrapTable<RelacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }
        [Route("ListarRelacionesOwnersAPIs")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListarRelacionesOwnersAPIs(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.username = user.Matricula;
            objDTO.PerfilId = user.PerfilId;
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetListarRelacionesOwnersAPIs(objDTO, out totalRows);

            var reader = new BootstrapTable<RelacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }
        [Route("ListarRelacionesAzureResources")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ListarRelacionesAzureResources(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.username = user.Matricula;
            objDTO.PerfilId = user.PerfilId;
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetListarRelacionesAzureResources(objDTO, out totalRows);

            var reader = new BootstrapTable<RelacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("application/eliminadas/status")]
        [Authorize]
        public IHttpActionResult GetApplicationEliminadaStatus(string codApplication)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.GetApplicationEliminadaStatus(codApplication, DateTime.Now);


            return Ok(colorStatus);
        }


        [Route("application/eliminadas/getDescription")]
        [Authorize]
        public IHttpActionResult GetApplicationEliminadaGetDescription(string codApplication)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.GetApplicationEliminadaGetDescription(codApplication);


            return Ok(colorStatus);
        }


        [Route("application/Gerencia/getUsuarioUnidad")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetUsuarioUnidad(PaginationApplication pag)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.GetUsuarioUnidad(pag.Username);


            return Ok(colorStatus);
        }

        [Route("application/Gerencia/getOwner")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetOwner(PaginationApplication pag)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.GetOwner(pag.Username);

            return Ok(colorStatus);
        }

        [Route("application/Gerencia/getPersonaSIGA")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetPersonaSIGA(PaginationApplication pag)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.GetPersonaSIGA(pag.Matricula);


            return Ok(colorStatus);
        }


        [Route("application/Gerencia/BuscarEnUnidad")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult BuscarEnUnidad(PaginationApplication pag)
        {

            var colorStatus = ServiceManager<ApplicationDAO>.Provider.BuscarEnUnidad(pag.Username);


            return Ok(colorStatus);
        }


        [Route("application/eliminadas/listadoAprobadas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationEliminadasAprobadas(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationEliminadasAprobadas(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.name = HttpUtility.HtmlEncode(x.name); });

            var reader = new BootstrapTable<ApplicationList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/eliminadas/solicitudesReversion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationSolicitudesReversion(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesReversionEliminacion(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion); });

            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/eliminadas/solicitudesReversionGestion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetApplicationSolicitudesReversionGestion(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            //pag.EstadoSolicitudUnico = (int)EstadoSolicitud.Pendiente; //TODO: o es el enum EEstadoSolicitudAplicacion ?? 
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesReversionEliminacion(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.TipoActivoInformacion = HttpUtility.HtmlEncode(x.TipoActivoInformacion);
                x.NombreAplicacion = HttpUtility.HtmlEncode(x.NombreAplicacion);
                x.Comentarios = HttpUtility.HtmlEncode(x.Comentarios);
                x.Observaciones = HttpUtility.HtmlEncode(x.Observaciones);
                x.ObservacionesRechazo = HttpUtility.HtmlEncode(x.ObservacionesRechazo);
            });

            var reader = new BootstrapTable<SolicitudDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("application/eliminadas/solicitarReversion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ApplicationEliminadasSolicitarReversion()
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();

                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;

                var obj = JsonConvert.DeserializeObject<SolicitudDto>(request.Form["data"]);
                obj.Matricula = user.Matricula;
                obj.UserName = user.Matricula;
                obj.Usuario = user.Nombres;
                obj.Email = user.CorreoElectronico;

                if (obj != null)
                {
                    obj.TipoSolicitud = (int)TipoSolicitud.RevertirEliminacion;
                    obj.FechaCreacion = DateTime.Now;

                    int _solicitudId = ServiceManager<SolicitudAplicacionDAO>.Provider.RegitrarSolicitudReversionEliminacionApplication(obj);

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
                    response = Request.CreateResponse(HttpStatusCode.OK, _solicitudId);
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("application/eliminadas/aprobarRechazarSolicitud")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ApplicationEliminadasAprobarRechazarSolicitud()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;
                var user = TokenGenerator.GetCurrentUser();
                var obj = JsonConvert.DeserializeObject<SolicitudDto>(request.Form["data"]);
                obj.UserName = user.Matricula;
                obj.Matricula = user.Matricula;
                obj.Email = user.CorreoElectronico;
                obj.Usuario = user.Nombres;

                var tipoAccion = int.Parse(request.Form["accion"]);

                if (obj != null)
                {
                    if (tipoAccion == 1)
                    {
                        ServiceManager<ApplicationDAO>.Provider.AprobarSolicitudReversionEliminacion(obj);

                    }
                    else if (tipoAccion == 2)
                    {
                        ServiceManager<ApplicationDAO>.Provider.RechazarSolicitudReversionEliminacion(obj);
                    }

                    if (obj.Id >= 0)
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
                            objRegistro.IdSolicitud = obj.Id;

                            if (tipoAccion == 1)
                            {
                                objRegistro.TicketEliminacion = _contenido;
                                objRegistro.NombreTicketEliminacion = _nombre;
                            }
                            else if (tipoAccion == 2)
                            {
                                objRegistro.Ratificacion = _contenido;
                                objRegistro.NombreRatificacion = _nombre;
                            }

                            ServiceManager<ApplicationDAO>.Provider.ActualizarSolicitudArchivos(objRegistro, tipoAccion);

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


        [Route("GestionAplicacion/ListarPublicacionAplicacionAsignada")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListPublicacionAplicacionAsignada(PaginacionReporteAplicacion pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ApplicationDAO>.Provider.GetPublicacionAplicacionAsignada(pag, out totalRows);
            if (registros != null)
            {
                var dataRpta = new { Total = totalRows, Rows = registros };
                response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            }
            return response;
        }
        [Route("application/ListarSolicitud")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetSolicitud(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetSolicitud(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.ApplicationName = HttpUtility.HtmlEncode(x.ApplicationName);
                x.Observaciones = HttpUtility.HtmlEncode(x.Observaciones);
                x.ObservacionesRechazo = HttpUtility.HtmlEncode(x.ObservacionesRechazo);
            });

            var reader = new BootstrapTable<SolicitudList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/approved")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApprovedApplication(PaginationApplication pag)
        {
            var usr = TokenGenerator.GetCurrentUser();
            pag.Username = usr.Matricula;
            pag.Matricula = usr.Matricula;
            pag.NombreUsuarioModificacion = usr.Nombres;
            pag.Email = usr.CorreoElectronico;

            var usuarios = new List<ApplicationManagerCatalogDto>();
            try
            {
                var parametroArquitecturaTI = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ARQUITECTURA_TI");
                var usuariosArquitecturaTI = new AzureGroupsManager().GetGroupMembersByName(parametroArquitecturaTI.Valor);
                if (usuariosArquitecturaTI != null)
                {
                    foreach (var item in usuariosArquitecturaTI)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.ArquitectoTI,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroUsuariosAIO = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_AIO");
                var usuariosAIO = new AzureGroupsManager().GetGroupMembersByName(parametroUsuariosAIO.Valor);
                if (usuariosAIO != null)
                {
                    foreach (var item in usuariosAIO)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.AIO,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }
            }
            catch (Exception)
            {

            }

            var rolesGestion = ServiceManager<ActivosDAO>.Provider.GetRolesGestion();
            var managerid = 0;
            foreach (var user in rolesGestion)
            {
                managerid = 0;
                switch (user.RoleId)
                {
                    case (int)ERoles.AIO:
                        managerid = (int)ApplicationManagerRole.AIO;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.ArquitectoTecnologia:
                        managerid = (int)ApplicationManagerRole.ArquitectoTI;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    default: break;
                }
            }

            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.ApprovedApplication(pag.Id, pag.Username, pag.Email, usuariosFinales, pag.Matricula, pag.NombreUsuarioModificacion);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/flows/refused")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRefusedFlowpplication(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.registerByEmail = user.CorreoElectronico;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.RefuseFlow(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/user/refused")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRefusedUserApplication(ApplicationDto obj)
        {
            var user = TokenGenerator.GetCurrentUser();
            obj.registerBy = user.Matricula;
            obj.registerByEmail = user.CorreoElectronico;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.RefuseUser(obj);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/SolicitudApproved")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApprovedSolicitud(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.ApprovedSolicitud(pag.Id, pag.Username);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/SolicitudEliminacionApproved")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApprovedSolicitudEliminacion(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.ApprovedSolicitudEliminacion(pag.Id, pag.Username, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/SolicitudEliminacionObserved")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObservedSolicitudEliminacion(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.ObservedSolicitudEliminacion(pag.Id, pag.Username, pag.Matricula, pag.NombreUsuario, pag.Comments);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/SolicitudRefused")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRefusedSolicitud(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.Matricula = user.Matricula;
            pag.NombreUsuario = user.Nombres;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.RefusedSolicitud(pag.Id, pag.Username, pag.Comments, pag.Matricula, pag.NombreUsuario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/SolicitudEliminacionRefused")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRefusedSolicitudEliminacion(PaginationSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<ApplicationDAO>.Provider.RefusedSolicitudEliminacion(pag.Id, pag.Username, pag.Comments);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("application/flows")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetFlowsByApp(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByApp(pag, out totalRows);

            if (registros == null)
                return NotFound();
            else
            {
                var azureManger = new AzureGroupsManager();
                var userAD = new AzureUserDto();
                foreach (var item in registros)
                {
                    if (!string.IsNullOrEmpty(item.mail))
                    {
                        userAD = azureManger.GetUserDataByMail(item.mail);
                        if (userAD != null)
                            item.nameDetail = userAD.Nombres;
                    }
                }

                registros.ForEach(x =>
                {
                    x.applicationName = HttpUtility.HtmlEncode(x.applicationName);
                    x.comments = HttpUtility.HtmlEncode(x.comments);
                });
            }

            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/flowsEliminacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetFlowsByAppEiminacion(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByAppEliminacion(pag, out totalRows);

            if (registros == null)
                return NotFound();
            else
            {
                var azureManger = new AzureGroupsManager();
                var userAD = new AzureUserDto();
                foreach (var item in registros)
                {
                    if (!string.IsNullOrEmpty(item.mail))
                    {
                        userAD = azureManger.GetUserDataByMail(item.mail);
                        if (userAD != null)
                            item.nameDetail = userAD.Nombres;
                    }
                }
            }

            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/flowsModificacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetFlowsByAppModificacion(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByAppModificacion(pag, out totalRows);

            if (registros == null)
                return NotFound();
            else
            {
                var azureManger = new AzureGroupsManager();
                var userAD = new AzureUserDto();
                foreach (var item in registros)
                {
                    if (!string.IsNullOrEmpty(item.mail))
                    {
                        userAD = azureManger.GetUserDataByMail(item.mail);
                        if (userAD != null)
                            item.nameDetail = userAD.Nombres;
                    }
                }
            }

            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/flowsPendiente")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetFlowsByAppPendiente(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetApplicationFlowByAppPendiente(pag, out totalRows);
            var reader = new BootstrapTable<ApplicationFlowList>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/lists/jefaturaati/{jefatura:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectos(int jefatura)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoJefatura(jefatura);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/listsAdmin/jefaturaati/{jefatura:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsArquitectosAdmin(int jefatura)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListArquitectoJefaturaAdmin(jefatura);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/admin/lists")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetListsAdmin()
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<CommonDAO>.Provider.GetListsAdmin();

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("application/roles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetRolesByApp(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetRolesByApplication(pag.Id);

            if (registros == null)
                return NotFound();

            totalRows = (registros != null) ? registros.Count : 0;

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (string.IsNullOrWhiteSpace(item.managerName))
                {
                    userAD = azureManger.GetUserDataByMail(item.email);
                    if (userAD != null)
                        item.managerName = userAD.Nombres;
                }
            }

            var reader = new BootstrapTable<ApplicationManagerCatalogDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/roles/experts")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetExpertsByApp(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetExpertsByApplication(pag.Id);

            if (registros == null)
                return NotFound();

            totalRows = (registros != null) ? registros.Count : 0;

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (string.IsNullOrWhiteSpace(item.managerName))
                {
                    userAD = azureManger.GetUserDataByMail(item.email);
                    if (userAD != null)
                        item.managerName = userAD.Nombres;
                }
            }

            var reader = new BootstrapTable<ApplicationManagerCatalogDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarAppReversion")]
        [HttpGet]
        public IHttpActionResult ExportarAppReversion(string applicationId
 , string estado
 , string sortName
 , string sortOrder
 )
        {
            var user = TokenGenerator.GetCurrentUser();
            string username = user.Matricula;

            string filename = "";

            applicationId = string.IsNullOrEmpty(applicationId) ? string.Empty : applicationId;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            username = string.IsNullOrEmpty(username) ? string.Empty : username;



            var paginacion = new PaginacionReporteAplicacion()
            {
                //TablaProcedencia = TablaProcedencia,
                //Procedencia = Procedencia,
                applicationId = applicationId,
                Estado = estado,
                Username = username,
                pageNumber = 1,
                pageSize = 200,
                sortName = sortName,
                sortOrder = sortOrder
            };

            var data = new ExportarData().ExportarAppReversionEliminacion(paginacion);
            filename = string.Format("PortafolioDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("application/rolesInitial")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetRolesByApp2(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetRolesByInitialApplication(pag.Id);

            if (registros == null)
                return NotFound();

            totalRows = (registros != null) ? registros.Count : 0;

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (string.IsNullOrWhiteSpace(item.managerName))
                {
                    userAD = azureManger.GetUserDataByMail(item.email);
                    if (userAD != null)
                        item.managerName = userAD.Nombres;
                }
            }

            var reader = new BootstrapTable<ApplicationManagerCatalogDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("application/owners")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetOwnersByApp(PaginationApplication pag)
        {

            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetOwnersByApplication(pag.Id);

            if (registros == null)
                return NotFound();

            totalRows = (registros != null) ? registros.Count : 0;

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (string.IsNullOrWhiteSpace(item.managerName))
                {
                    userAD = azureManger.GetUserDataByMail(item.email);
                    if (userAD != null)
                        item.managerName = userAD.Nombres;
                }
            }

            var reader = new BootstrapTable<ApplicationManagerCatalogDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/expertos")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetExpertosByApp(PaginationApplication pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<ApplicationDAO>.Provider.GetExpertosByApplication(pag.Id);

            if (registros == null)
                return NotFound();

            totalRows = (registros != null) ? registros.Count : 0;

            var azureManger = new AzureGroupsManager();
            var userAD = new AzureUserDto();
            foreach (var item in registros)
            {
                if (string.IsNullOrWhiteSpace(item.managerName))
                {
                    userAD = azureManger.GetUserDataByMail(item.email);
                    if (userAD != null)
                        item.managerName = userAD.Nombres;
                }
            }

            var reader = new BootstrapTable<ApplicationManagerCatalogDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/stepadmin")]
        [HttpPost]
        [Authorize]
        //public HttpResponseMessage PostApplicationStepAdmin(ApplicationDto obj)
        public HttpResponseMessage PostApplicationStepAdmin()
        {
            var user = TokenGenerator.GetCurrentUser();

            HttpResponseMessage response = null;
            HttpRequest request = HttpContext.Current.Request;

            var obj = JsonConvert.DeserializeObject<ApplicationDto>(request.Form["data"]);
            obj.registerBy = user.Matricula;
            obj.NombreUsuarioModificacion = user.Nombres;
            obj.EmailSolicitante = user.CorreoElectronico;
            obj.Matricula = user.Matricula;

            var usuarios = new List<ApplicationManagerCatalogDto>();
            var azureManger = new AzureGroupsManager();
            try
            {
                var parametroArquitecturaTI = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ARQUITECTURA_TI");
                var usuariosArquitecturaTI = new AzureGroupsManager().GetGroupMembersByName(parametroArquitecturaTI.Valor);
                if (usuariosArquitecturaTI != null)
                {
                    foreach (var item in usuariosArquitecturaTI)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.ArquitectoTI,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroGobierno = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_GOBIERNO_USERIT");
                var usuariosGobierno = new AzureGroupsManager().GetGroupMembersByName(parametroGobierno.Valor);
                if (usuariosGobierno != null)
                {
                    foreach (var item in usuariosGobierno)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.GobiernoUserIT,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

            }
            catch (Exception)
            {

            }


            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

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

                obj.archivoMotivo = _contenido;
                obj.archivoMotivoNombre = _nombre;
            }

            var dataResult = ServiceManager<ApplicationDAO>.Provider.EditApplicationAdmin(obj, usuariosFinales);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }


        [Route("GestionAplicacion/Exportar/Actualizar")]
        [HttpGet]
        public IHttpActionResult GetExportarAplicacionPortafolioActualizar()
        {
            var data = new ExportarData().ExportarAplicacionPortafolioUpdate2();
            string filename = "CargaMasivaAplicacionesBCP";
            string nomArchivo = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }
        [Route("GestionAplicacion/Exportar/ActualizarHistorico")]
        [HttpGet]
        public IHttpActionResult GetExportarAplicacionPortafolioActualizarHistorico()
        {


            var data = Utilitarios.ObtenerPlantillaExportar("PlantillaAplicacionHistorico.xlsx");
            string nomArchivo = "PlantillaAplicacionHistorico.xlsx";

            return Ok(new { excel = data, name = nomArchivo });
        }
        [Route("GestionAplicacion/Exportar/ValidacionCargaMasiva")]
        [HttpGet]
        public IHttpActionResult GetExportarAplicacionPortafolioValidacionCargaMasiva()
        {
            var data = new ExportarData().ExportarAplicacionPortafolioValidacionesCargaMasiva();
            string filename = "ValidacionesCargaMasiva";
            string nomArchivo = string.Format("{0}_{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("GestionAplicacion/ActualizarAplicacionesHistorico")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarAplicacionesHistorico()
        {
            try
            {
                HttpResponseMessage response = null;
                HttpRequest request = HttpContext.Current.Request;
                var user = TokenGenerator.GetCurrentUser();
                string username = user.Matricula;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);
                    CargaDataAplicacion a = new CargaDataAplicacion();
                    var retorno = a.CargaMasivaAplicacionUpdate2(extension, inputStream, username);
                    response = Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("GestionAplicacion/ActualizarAplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostActualizarAplicaciones()
        {
            try
            {
                var user = TokenGenerator.GetCurrentUser();
                HttpResponseMessage response = null;
                HttpContext.Current.Server.ScriptTimeout = 7200;
                HttpRequest request = HttpContext.Current.Request;
                string username = user.Matricula;
                string NombreUsuario = user.Nombres;
                string Matricula = user.Matricula;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile clientFile = null;
                    clientFile = request.Files["File"];
                    var inputStream = clientFile.InputStream;
                    var nombre = clientFile.FileName;
                    var extension = Path.GetExtension(nombre);
                    CargaDataAplicacion a = new CargaDataAplicacion();
                    var retorno = a.CargaMasivaAplicacionUpdate(extension, inputStream, username, NombreUsuario, Matricula);
                    response = Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("ConnectJenkins")]
        [Authorize]
        public void ConnectJenkins(string codApp)
        {


            var objURL = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("URL_CLIENTE_JENKINS");
            var objUsername = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("USERNAME_CLIENTE_JENKINS");
            var objPassword = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("PASSWORD_CLIENTE_JENKINS");

            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            log.Debug("Iniciando conexión");
            RestClient client = null;
            RestRequest request = null;
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            log.DebugFormat("Intentando conexión al host: {0}", "https://jenkinsdesa.lima.bcp.com.pe/view/IAAC/job/INFR-DEV/job/clone_automation-group-poaz/buildWithParameters");
            client = new RestClient(objURL.Valor + codApp);
            client.Authenticator = new HttpBasicAuthenticator(objUsername.Valor, objPassword.Valor);
            log.DebugFormat("Generando token de autorización: {0}", "apncdes:1197b3ed862b72176e78fb89e95f703594");

            request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json;charset=utf-8");
            log.DebugFormat("Código de aplicación enviada: {0}", "AP25");

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonRetorno = response.Content;
                log.Debug("HTTP Status code response: 200");
                log.DebugFormat("Respuesta: {0}", jsonRetorno);
            }
            else
            {
                log.DebugFormat("HTTP Status code response: {0}", response.StatusCode.ToString());
                log.DebugFormat("HTTP Status Message: {0}", response.StatusDescription);
                log.DebugFormat("HTTP Error Message: {0}", response.ErrorMessage);
            }
        }

        [Route("GestionAplicacion/EliminarPortafolio")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetEliminarPortafolio()
        {
            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.EliminarPortafolio();

            response = Request.CreateResponse(HttpStatusCode.OK, "ok");
            return response;
        }

        [Route("GestionAplicacion/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivision(string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string clasificacionTecnica
            , string subclasificacionTecnica
            , string aplicacion
            , string sortName
            , string sortOrder
            , string TablaProcedencia
            , int Procedencia
            , string TipoActivo
            , int Exportar
            )
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue,
                Exportar = Exportar.ToString()
            };

            var data = new ExportarData().ExportarReportePublicacionAplicacionCatalogoAplicaciones2(paginacion);
            filename = string.Format("CatalogoDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("GestionAplicacion/Exportar2")]
        [HttpGet]
        public IHttpActionResult ExportarGerenciaDivision2(string gerencia
          , string division
          , string unidad
          , string area
          , string estado
          , string clasificacionTecnica
          , string subclasificacionTecnica
          , string aplicacion
          , string sortName
          , string sortOrder
          , string TablaProcedencia
          , int Procedencia
          , string TipoActivo
          , string TipoPCI
          , int Exportar
          , string GestionadoPor
          , string LiderUsuario
          )
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            TipoPCI = string.IsNullOrEmpty(TipoPCI) ? string.Empty : TipoPCI;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;
            GestionadoPor = string.IsNullOrEmpty(GestionadoPor) ? string.Empty : GestionadoPor;
            LiderUsuario = string.IsNullOrEmpty(LiderUsuario) ? string.Empty : LiderUsuario;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                TipoPCI = TipoPCI,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue,
                Exportar = Exportar.ToString(),
                GestionadoPor = GestionadoPor,
                LiderUsuario = LiderUsuario
            };

            var data = new ExportarData().ExportarReportePublicacionAplicacionCatalogoAplicaciones3(paginacion);
            filename = string.Format("CatalogoDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("application/filter")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApplicationVigenteByFilter(string filtro, string codigoAPT)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<ApplicationDAO>.Provider.GetApplicationVigenteByFilter(filtro, true, codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("application/filter/replace")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetApplicationReplaceByFilter(string filtro, string codigoAPT)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<ApplicationDAO>.Provider.GetApplicationVigenteByFilter(filtro, true, codigoAPT);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("application/managedBy/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetManagedBy(int id)
        {
            var objAmb = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorById(id);
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/assetType/userIT")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GeAssetTypeUserIT()
        {
            var objAmb = ServiceManager<ActivosDAO>.Provider.GetActivosByUserIT();
            if (objAmb == null)
                return NotFound();

            return Ok(objAmb);
        }

        [Route("application/ListGroupRemedy")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetGroupRemedy(string filtro)
        {
            HttpResponseMessage response = null;
            var listApp = ServiceManager<ApplicationDAO>.Provider.GetGroupRemedyByFilter(filtro, true);
            response = Request.CreateResponse(HttpStatusCode.OK, listApp);
            return response;
        }

        [Route("application/deleteApplication")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostDeletedApplication(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.DeleteApplication(pag.Id, pag.Username);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("application/deleteApplication2")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostDeletedApplication2(PaginationApplication pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Username = user.Matricula;
            pag.Email = user.CorreoElectronico;
            pag.Matricula = user.Matricula;
            pag.NombreUsuarioModificacion = user.Nombres;

            HttpResponseMessage response = null;
            ServiceManager<ApplicationDAO>.Provider.DeleteApplication2(pag.Id, pag.Username, pag.Email, pag.Comments, pag.Matricula, pag.NombreUsuarioModificacion);
            //return Ok();
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }

        [Route("ExportarAplicacionesDesestimadas")]
        [HttpGet]
        public IHttpActionResult ExportarAplicacionesDesestimadas(string gerencia
            , string division
            , string unidad
            , string area
            , string estado
            , string clasificacionTecnica
            , string subclasificacionTecnica
            , string aplicacion
            , string sortName
            , string sortOrder
            , string TablaProcedencia
            , int Procedencia
            , string TipoActivo
            )
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue
            };

            var data = new ExportarData().ExportarReportePublicacionAplicacionDesestimadas(paginacion);
            filename = string.Format("ReporteAplicacionesDesestimadas_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ExportarFormatosRegistro")]
        [HttpGet]
        public IHttpActionResult ExportarFormatosRegistro(string gerencia
     , string division
     , string unidad
     , string area
     , string estado
     , string clasificacionTecnica
     , string subclasificacionTecnica
     , string aplicacion
     , string sortName
     , string sortOrder
     , string TablaProcedencia
     , int Procedencia
     , string TipoActivo
     )
        {
            string filename = "";

            gerencia = string.IsNullOrEmpty(gerencia) ? string.Empty : gerencia;
            division = string.IsNullOrEmpty(division) ? string.Empty : division;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            area = string.IsNullOrEmpty(area) ? string.Empty : area;
            unidad = string.IsNullOrEmpty(unidad) ? string.Empty : unidad;
            clasificacionTecnica = string.IsNullOrEmpty(clasificacionTecnica) ? string.Empty : clasificacionTecnica;
            subclasificacionTecnica = string.IsNullOrEmpty(subclasificacionTecnica) ? string.Empty : subclasificacionTecnica;
            TipoActivo = string.IsNullOrEmpty(TipoActivo) ? string.Empty : TipoActivo;
            aplicacion = string.IsNullOrEmpty(aplicacion) ? string.Empty : aplicacion;

            var paginacion = new PaginacionReporteAplicacion()
            {
                TablaProcedencia = TablaProcedencia,
                Procedencia = Procedencia,
                Gerencia = gerencia,
                Division = division,
                Estado = estado,
                Area = area,
                Unidad = unidad,
                ClasificacionTecnica = clasificacionTecnica,
                SubclasificacionTecnica = subclasificacionTecnica,
                TipoActivo = TipoActivo,
                Aplicacion = aplicacion,
                pageNumber = 1,
                pageSize = int.MaxValue
            };

            var data = new ExportarData().ExportarReportePublicacionFormatosRegistro(paginacion);
            filename = string.Format("ReporteFormatosDeRegistro_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }

        [Route("ExportarSolicitudes")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostExportAplicacionesConfiguracion(
            int flow
            , string role
            , string nombreAplicacion
            , string sortName
            , string sortOrder
            , int estado
            , int gestionadoPor
            , int estadoSolicitud
            , string nombreUsuario)
        {
            string nomArchivo = "";
            if (nombreAplicacion == "") nombreAplicacion = null;

            var user = TokenGenerator.GetCurrentUser();
            nombreUsuario = user.Matricula;
            role = user.Perfil;

            var data = new ExportarData().ExportarSolicitudes(flow, role, nombreAplicacion, sortName, sortOrder, estado, gestionadoPor, estadoSolicitud, nombreUsuario);
            nomArchivo = string.Format("ListaAplicacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        //[Route("GestionAplicacion/Portafolio/Backup/Obtener")]
        //[HttpGet]
        //[Authorize]
        //public HttpResponseMessage PostListPortafolioBackup(int idBackup)
        //{

        //    var data = ServiceManager<AplicacionDAO>.Provider.GetAplicacionPortafolioBackupById(idBackup);
        //    string filename = "ListadoPortafolioAplicaciones";
        //    string nomArchivo = string.Format("{0}_{1}.xlsx", filename, data.FechaCreacion.ToString("yyyyMMdd_HHmmss"));

        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new ByteArrayContent(data.BackupBytes);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = nomArchivo;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return httpResponseMessage;
        //}

        [Route("flows/data")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitudDetalle(PaginacionSolicitud pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SolicitudAplicacionDAO>.Provider.GetSolicitudesDetalleData(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<SolicitudDetalleDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }



        [Route("Consultas/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage getListarCombosConsultas()
        {
            HttpResponseMessage response = null;
            var listTiposConsulta = Utilitarios.EnumToList<TipoConsulta>();

            var dataRpta = new
            {
                tiposConsulta = listTiposConsulta.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Consultas/{id:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage getConsultaById(int id)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<ApplicationDAO>.Provider.GetConsultaById(id);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("Consultas/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostConsultas(ConsultaDTO rgDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            rgDTO.NombreUsuarioConsultor = user.Nombres;
            rgDTO.MatriculaUsuarioConsultor = user.Matricula;
            rgDTO.EmailUsuarioConsultor = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int ConsultaId = ServiceManager<ApplicationDAO>.Provider.AddOrEditConsulta(rgDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ConsultaId);
            return response;
        }

        [Route("Consultas/EliminarConsulta")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EliminarConsulta(ConsultaDTO rgDTO)
        {
            HttpResponseMessage response = null;
            int ConsultaId = ServiceManager<ApplicationDAO>.Provider.EliminarConsulta(rgDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ConsultaId);
            return response;
        }

        [Route("Consultas/EditConsulta")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage EditConsulta(ConsultaDTO rgDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            rgDTO.NombreUsuarioConsultor = user.Nombres;
            rgDTO.MatriculaUsuarioConsultor = user.Matricula;
            rgDTO.EmailUsuarioConsultor = user.CorreoElectronico;

            HttpResponseMessage response = null;
            int ConsultaId = ServiceManager<ApplicationDAO>.Provider.EditConsulta(rgDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ConsultaId);
            return response;
        }

        [Route("Consultas/ListadoConsultas")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListConsultasByUser(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;

            var totalRows = 0;
            var registros = ServiceManager<ApplicationDAO>.Provider.GetConsultasByUser(pag.tipoId, pag.Respondido, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.Matricula, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Consulta = HttpUtility.HtmlEncode(x.Consulta);
                x.RespuestaPortafolio = HttpUtility.HtmlEncode(x.RespuestaPortafolio);
            });

            dynamic reader = new BootstrapTable<ConsultaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }


        [Route("Consultas/ListadoConsultas2")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListConsultas(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ApplicationDAO>.Provider.GetConsultas(pag.tipoId, pag.Respondido, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.Matricula, pag.Desde, pag.Hasta, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.RespuestaPortafolio = HttpUtility.HtmlEncode(x.RespuestaPortafolio);
                x.Consulta = HttpUtility.HtmlEncode(x.Consulta);
                x.ConsultaUsuario = HttpUtility.HtmlEncode(x.ConsultaUsuario);
            });

            dynamic reader = new BootstrapTable<ConsultaDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Consultas/Responder")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRespuesta(ConsultaDTO rgDTO)
        {
            HttpResponseMessage response = null;
            int ConsultaId = ServiceManager<ApplicationDAO>.Provider.AddRespuesta(rgDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ConsultaId);
            return response;
        }


        [Route("Consultas/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarConsultas(int tipoid, int respondido, string matricula
            , string desde
            , string hasta
            , string sortName
            , string sortOrder
            )
        {
            string filename = "";

            matricula = string.IsNullOrEmpty(matricula) ? string.Empty : matricula;
            desde = string.IsNullOrEmpty(desde) ? string.Empty : desde;
            hasta = string.IsNullOrEmpty(hasta) ? string.Empty : hasta;


            var paginacion = new PaginacionReporteAplicacion()
            {
                //TablaProcedencia = TablaProcedencia,
                //Procedencia = Procedencia,
                Matricula = matricula,
                Desde = desde,
                Hasta = hasta,
                pageNumber = 1,
                pageSize = int.MaxValue,
                sortName = sortName,
                sortOrder = sortOrder,
                tipoId = tipoid,
                Respondido = respondido
            };

            var data = new ExportarData().ExportarReporteConsultas(paginacion);
            filename = string.Format("ConsolidadoConsultas_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }
        [Route("ExportarSolicitudes")]
        [HttpGet]
        public IHttpActionResult ExportarSolicitudes(string codigoAPT, int estado
           , string sortName
           , string sortOrder
           )
        {
            string filename = "";

            codigoAPT = string.IsNullOrEmpty(codigoAPT) ? string.Empty : codigoAPT;



            var paginacion = new PaginationSolicitud()
            {

                Status = estado,
                CodigoAPT = codigoAPT,

                pageNumber = 1,
                pageSize = int.MaxValue,
                sortName = sortName,
                sortOrder = sortOrder,

            };

            var data = new ExportarData().ExportarReporteSolicitudes(paginacion);
            filename = string.Format("ReporteSolicitudes_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }
        [Route("ExportarAppUserIt")]
        [HttpGet]
        public IHttpActionResult ExportarAppUserIt(string applicationId
           , string estado
           , string sortName
           , string sortOrder
           )
        {
            var user = TokenGenerator.GetCurrentUser();
            string username = user.Matricula;

            string filename = "";

            applicationId = string.IsNullOrEmpty(applicationId) ? string.Empty : applicationId;
            estado = string.IsNullOrEmpty(estado) ? string.Empty : estado;
            username = string.IsNullOrEmpty(username) ? string.Empty : username;



            var paginacion = new PaginacionReporteAplicacion()
            {
                //TablaProcedencia = TablaProcedencia,
                //Procedencia = Procedencia,
                applicationId = applicationId,
                Estado = estado,
                Username = username,
                pageNumber = 1,
                pageSize = 200,
                sortName = sortName,
                sortOrder = sortOrder
            };

            var data = new ExportarData().ExportarAppUserIT(paginacion);
            filename = string.Format("PortafolioDeAplicacionesBCP_{0}.xlsx", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(new { excel = data, name = filename });
        }
        [Route("ExportarSolicitudesCreacion")]
        [HttpGet]
        public IHttpActionResult ExportarSolicitudesCreacion(string nombre, int status, string user, string sortName, string sortOrder)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarSolicitudesCreacion(nombre, status, user, sortName, sortOrder);
            nomArchivo = string.Format("Solicitudes_Creacion_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }


        [Route("application/existePersona")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExistePersonas(PaginationApplication pag)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<ApplicationDAO>.Provider.GetPersona(pag.Username);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("PCI/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetActivosById(int id)
        {
            var objPar = ServiceManager<ApplicationDAO>.Provider.GetPCIById(id);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }


        [Route("PCI/CambiarEstadoPCI")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstadoPCI(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var entidad = ServiceManager<ApplicationDAO>.Provider.GetPCIById(Id);
            var retorno = ServiceManager<ApplicationDAO>.Provider.CambiarEstadoPCI(Id, !entidad.FlagActivo, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("PCI/AddOrEdit")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAPCI(PCIDto pciDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            pciDTO.UsuarioCreacion = user.Matricula;
            pciDTO.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            int ActivosId = ServiceManager<ApplicationDAO>.Provider.AddOrEditPCI(pciDTO);
            response = Request.CreateResponse(HttpStatusCode.OK, ActivosId);
            return response;
        }

        [Route("PCI/ListadoPCI")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListPCI(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ApplicationDAO>.Provider.GetPCI(pag.nombre, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<PCIDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        [Route("ListarCheckListCross")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getListarCheckListCross()
        {
            var totalRows = 0;
            var registros = ServiceManager<ApplicationDAO>.Provider.GetCheckListCross();

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
            });

            dynamic reader = new BootstrapTable<CheckListCross>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("application/reversion")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostApplicationReversion(ApplicationDto obj)
        {
            var usr = TokenGenerator.GetCurrentUser();
            obj.registerBy = usr.Matricula;
            obj.registerByEmail = usr.CorreoElectronico;
            obj.registerByName = usr.Nombres;
            obj.Matricula = usr.Matricula;
            obj.NombreUsuarioModificacion = usr.Nombres;

            HttpResponseMessage response = null;

            var usuarios = new List<ApplicationManagerCatalogDto>();

            try
            {
                var azureManger = new AzureGroupsManager();
                var parametroDevSecOps = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_DEVSECOPS");
                var usuariosDevSecOps = new AzureGroupsManager().GetGroupMembersByName(parametroDevSecOps.Valor);
                if (usuariosDevSecOps != null)
                {
                    foreach (var item in usuariosDevSecOps)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.DevSecOps,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroArquitecturaTI = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ARQUITECTURA_TI");
                var usuariosArquitecturaTI = new AzureGroupsManager().GetGroupMembersByName(parametroArquitecturaTI.Valor);
                if (usuariosArquitecturaTI != null)
                {
                    foreach (var item in usuariosArquitecturaTI)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.ArquitectoTI,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroUsuariosAIO = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_AIO");
                var usuariosAIO = new AzureGroupsManager().GetGroupMembersByName(parametroUsuariosAIO.Valor);
                if (usuariosAIO != null)
                {
                    foreach (var item in usuariosAIO)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.AIO,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

                var parametroGobierno = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_GOBIERNO_USERIT");
                var usuariosGobierno = new AzureGroupsManager().GetGroupMembersByName(parametroGobierno.Valor);
                if (usuariosGobierno != null)
                {
                    foreach (var item in usuariosGobierno)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.GobiernoUserIT,
                            email = item.mail,
                            username = item.matricula,
                            managerName = string.Empty
                        });
                    }
                }

            }
            catch (Exception)
            {

            }

            var rolesGestion = ServiceManager<ActivosDAO>.Provider.GetRolesGestion();
            var managerid = 0;
            foreach (var user in rolesGestion)
            {
                switch (user.RoleId)
                {
                    case (int)ERoles.AIO:
                        managerid = (int)ApplicationManagerRole.AIO;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.ArquitectoTecnologia:
                        managerid = (int)ApplicationManagerRole.ArquitectoTI;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.DevSecOps:
                        managerid = (int)ApplicationManagerRole.DevSecOps;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                    case (int)ERoles.GobiernoUserIT:
                        managerid = (int)ApplicationManagerRole.GobiernoUserIT;
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = managerid,
                            email = user.Email,
                            username = user.Username,
                            managerName = string.Empty
                        });
                        break;
                }
            }

            var tmpUsuarios = (from u in usuarios
                               select new
                               {
                                   u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select new ApplicationManagerCatalogDto()
                                   {
                                       applicationManagerId = u.applicationManagerId,
                                       email = u.email,
                                       username = u.username
                                   }).ToList();

            var dataResult = ServiceManager<ApplicationDAO>.Provider.AddApplicationReversion(obj, usuariosFinales);
            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);


            return response;
        }

        [Route("userSIGA/{nombre}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUserSIGA(string nombre)
        {
            var objPar = ServiceManager<ApplicationDAO>.Provider.GetUsuarios(nombre);
            if (objPar == null)
                return NotFound();

            return Ok(objPar);
        }
    }
}


