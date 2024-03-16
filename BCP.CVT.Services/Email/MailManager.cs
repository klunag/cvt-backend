using BCP.CVT.Cross;
using BCP.CVT.EnvioCorreos;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.PAPP.Common.Cross;
using BCP.PAPP.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Email
{
    public class MailingManager
    {
        private static string LABEL_CODIGO_APLICACION = "[CodigoAPT]";
        private static string LABEL_NOMBRE_APLICACION = "[NombreAplicacion]";
        private static string LABEL_MOTIVO_ELIMINACION = "[MotivoEliminacion]";

        private static string LABEL_TIPO_CONSULTA = "[TipoConsulta]";
        private static string LABEL_CONSULTA = "[Consulta]";
        private static string LABEL_RESPUESTA = "[Respuesta]";

        private static string LABEL_CODIGO_PRODUCTO = "[CodigoProducto]";
        private static string LABEL_NOMBRE_PRODUCTO = "[NombreProducto]";
        private static string LABEL_SOLICITUD = "[SolicitudId]";
        private static string LABEL_TECNICO_ESPECIALISTA = "[TecnicoEsp]";
        private static string LABEL_NOMBRE_TECNOLOGIA = "[NombreTecnologia]";

        public void ProcesarEnvioNotificacionesAdministradores(int idTipoNotificacionApp
            , string codAPT, Dictionary<string, string> dict_notificacion
            , bool paraAdministradores
            , bool para
            , List<string> adicionales = null
            , bool expertos = false)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
            var objAplicacion = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(codAPT);

            if (objTipoNotificacion != null && objAplicacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();

                ValidarRolEnvioCorreo(ApplicationManagerRole.Solicitante, objTipoNotificacion.Solicitante, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoEvaluador, objTipoNotificacion.ArquitectoNegocio, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoTI, objTipoNotificacion.ATI, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.TL, objTipoNotificacion.TL_TTL, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.TTL, objTipoNotificacion.TL_TTL, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.DevSecOps, objTipoNotificacion.DevSec, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.JefeDeEquipo, objTipoNotificacion.JefeEquipo, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.UsuarioAutorizador, objTipoNotificacion.UsuarioAutorizador, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.Broker, objTipoNotificacion.Brokerr, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.GobiernoUserIT, objTipoNotificacion.GobiernoIT, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.AIO, objTipoNotificacion.AIO, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.Owner, objTipoNotificacion.LiderUsuario, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoSoluciones, objTipoNotificacion.ArquitectoSolucion, listaIdsRoles);
                if (expertos)
                    ValidarRolEnvioCorreo(ApplicationManagerRole.Experto, true, listaIdsRoles);


                if (listaIdsRoles.Count != 0 || listaPara != null || para)
                {
                    listaCorreos = ServiceManager<ApplicationDAO>.Provider.GetApplicationManagerCatalogByRolesIds(listaIdsRoles, objAplicacion.id);

                    var mailService = new EmailApiService();

                    //preparar correo
                    var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_CODIGO_APLICACION, codAPT);
                    if (objAplicacion.isReactivated == true)
                    {
                        asuntoCorreo = asuntoCorreo.Replace("[Registro de aplicación]", "[Reactivación de aplicación]");
                    }
                    var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_APLICACION, codAPT)
                                                                 .Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName);                    

                    if (dict_notificacion != null)
                    {
                        foreach (KeyValuePair<string, string> entry in dict_notificacion)
                        {
                            cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                        }
                    }

                    var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                    var toCorreo = new List<string>();
                    if (listaCorreos.Count != 0)
                    {
                        toCorreo = listaCorreos.Select(x => x.email).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                    }

                    if (listaPara != null)
                    {
                        if (listaPara.Count > 0)
                            toCorreo.AddRange(listaPara);
                    }                    

                    if (adicionales != null)
                    {
                        if (adicionales.Count > 0)
                            toCorreo.AddRange(adicionales);
                    }

                    var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                    var administradores = this.ListaCorreosAdministradores();
                    if (para)
                    {
                        if (administradores != null)
                        {
                            if (administradores.Count > 0)
                                toCorreo.AddRange(administradores);
                        }
                    }
                    else
                    {
                        if (administradores != null)
                        {
                            if (administradores.Count > 0)
                                ccCorreo.AddRange(administradores);
                        }
                    }

                    cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");
                    toCorreo = toCorreo.Distinct().ToList();
                    ccCorreo = ccCorreo.Distinct().ToList();

                    var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
                }
            }
        }

        public void ProcesarEnvioNotificacionesAdministradoresConsultas(int idTipoNotificacionApp
           , string codAPT, Dictionary<string, string> dict_notificacion
           , bool paraAdministradores
           , bool para
           , List<string> adicionales = null
           )
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);


            if (objTipoNotificacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();                                   
                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto;

                var cuerpoCorreo = objTipoNotificacion.Cuerpo;

                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                var toCorreo = new List<string>();
               

                if (listaPara != null)
                {
                    if (listaPara.Count > 0)
                        toCorreo.AddRange(listaPara);
                }

                if (adicionales != null)
                {
                    if (adicionales.Count > 0)
                        toCorreo.AddRange(adicionales);
                }

                var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                var administradores = this.ListaCorreosAdministradores();
                if (para)
                {
                    if (administradores != null)
                    {
                        if (administradores.Count > 0)
                            toCorreo.AddRange(administradores);
                    }
                }
                else
                {
                    if (administradores != null)
                    {
                        if (administradores.Count > 0)
                            ccCorreo.AddRange(administradores);
                    }
                }

                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");
                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
               
            }
        }



        public List<string> ObtenerEmailAdministradores() 
        {
            List<string> correosAdmins = new List<string>();
            List<string> correosAzure = new List<string>();

            var correosRolOnDemand = ServiceManager<NotificacionDAO>.Provider.ListarRolOnDemandAdmins();

            var azureManger = new AzureGroupsManager();
            var parametroAdmin = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ADMINISTRADORES");
            var usuariosAdmin = new AzureGroupsManager().GetGroupMembersByName(parametroAdmin.Valor);
            if (usuariosAdmin != null)
            {
                foreach (var item in usuariosAdmin)
                {
                    correosAzure.Add(item.mail);
                }
            }

            foreach (string item in correosAzure) {
                correosAdmins.Add(item);
            }

            foreach (string item in correosRolOnDemand)
            {
                correosAdmins.Add(item);
            }

            return correosAdmins;

        }

        public void ProcesarEnvioNotificaciones(int idTipoNotificacionApp, string codAPT, Dictionary<string, string> dict_notificacion, List<string> adicionales = null, bool expertos = false)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
            var objAplicacion = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(codAPT);

            if (objTipoNotificacion != null && objAplicacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();

                ValidarRolEnvioCorreo(ApplicationManagerRole.Solicitante, objTipoNotificacion.Solicitante, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoEvaluador, objTipoNotificacion.ArquitectoNegocio, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoTI, objTipoNotificacion.ATI, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.TL, objTipoNotificacion.TL_TTL, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.TTL, objTipoNotificacion.TL_TTL, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.DevSecOps, objTipoNotificacion.DevSec, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.JefeDeEquipo, objTipoNotificacion.JefeEquipo, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.UsuarioAutorizador, objTipoNotificacion.UsuarioAutorizador, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.Broker, objTipoNotificacion.Brokerr, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.GobiernoUserIT, objTipoNotificacion.GobiernoIT, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.AIO, objTipoNotificacion.AIO, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.Owner, objTipoNotificacion.LiderUsuario, listaIdsRoles);
                ValidarRolEnvioCorreo(ApplicationManagerRole.ArquitectoSoluciones, objTipoNotificacion.ArquitectoSolucion, listaIdsRoles); 
                if (expertos)
                    ValidarRolEnvioCorreo(ApplicationManagerRole.Experto, true, listaIdsRoles);


                if (listaIdsRoles.Count != 0 || listaPara != null)
                {
                    var parametroIdsNotifacion = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("NOTIFICACION_NO_USA_TABLA_APLICATIONFLOW");
                    var idsTipoNotificacionApp_NoAplicationFlow = ServiceManager<NotificacionDAO>.Provider.ObtenerIDsTipoNotificacionApp_NoAplicationFlow(parametroIdsNotifacion.Valor);

                    List<int> listNotifacionNoUseAplicationFlow = new List<int>();
                    listNotifacionNoUseAplicationFlow.Add((int)NotificationFlow.ActualizacionActualizacionDatosNoRequeridosAprobacion);
                    listNotifacionNoUseAplicationFlow.Add((int)NotificationFlow.ActualizacionCreaciónSolicitudModificación);
                    //var IdnotificacionModificacionNoRequiereAprobacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp((int)NotificationFlow.ActualizacionActualizacionDatosNoRequeridosAprobacion);

                    if (idsTipoNotificacionApp_NoAplicationFlow.Contains(idTipoNotificacionApp) || listNotifacionNoUseAplicationFlow.Contains(idTipoNotificacionApp))
                        listaCorreos = ServiceManager<ApplicationDAO>.Provider.GetOnlyApplicationManagerCatalogByRolesIds(listaIdsRoles, objAplicacion.id);
                    else
                        listaCorreos = ServiceManager<ApplicationDAO>.Provider.GetApplicationManagerCatalogByRolesIds(listaIdsRoles, objAplicacion.id);

                    var mailService = new EmailApiService();

                    //preparar correo
                    var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_CODIGO_APLICACION, codAPT);
                    asuntoCorreo = asuntoCorreo.Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName);
                    if(objAplicacion.isReactivated == true)
                    {
                        asuntoCorreo = asuntoCorreo.Replace("[Registro de aplicación]", "[Reactivación de aplicación]");
                        asuntoCorreo = asuntoCorreo.Replace("Nueva aplicación registrada", "Aplicación reactivada");
                    }
                    var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_APLICACION, codAPT)
                                                                 .Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName);

                    if (objAplicacion.isReactivated == true)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace("el registro de la nueva", "la reactivación de la");
                        cuerpoCorreo = cuerpoCorreo.Replace("por lo que se requiere su confirmación sobre la creación de la misma"
                            , "por lo que se requiere su confirmación sobre los datos de la misma");
                    }

                    if (dict_notificacion != null)
                    {
                        foreach (KeyValuePair<string, string> entry in dict_notificacion)
                        {
                            cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                        }
                    }

                    var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                    var toCorreo = new List<string>();
                    if (listaCorreos.Count != 0)
                    {                                                
                        toCorreo = listaCorreos.Select(x => x.email).Where(x=>!string.IsNullOrWhiteSpace(x)).Distinct().ToList();                                                                       
                    }

                    if (listaPara != null)
                    {
                        if (listaPara.Count > 0)
                            toCorreo.AddRange(listaPara);
                    }

                    if (adicionales != null)
                    {
                        if (adicionales.Count > 0)
                            toCorreo.AddRange(adicionales);
                    }

                    var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                    cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                    toCorreo = toCorreo.Distinct().ToList();
                    ccCorreo = ccCorreo.Distinct().ToList();

                    var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
                }
            }
        }

        public void ProcesarEnvioNotificacionesEspecifico(int idTipoNotificacionApp, string codAPT, Dictionary<string, string> dict_notificacion, List<string> correos, List<string> correosCC = null)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
            var objAplicacion = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(codAPT);

            if (objTipoNotificacion != null && objAplicacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();
                
                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_CODIGO_APLICACION, codAPT);
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_APLICACION, codAPT)
                                                                .Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName);

                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);                
                var toCorreo = correos.Select(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (listaPara != null)
                    toCorreo.AddRange(listaPara);

                var correosConCopia = new List<string>();
                if (correosCC != null)
                {
                    correosConCopia.AddRange(correosCC);
                }
                if (!string.IsNullOrWhiteSpace(objTipoNotificacion.ConCopia))
                {
                    var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                    correosConCopia.AddRange(ccCorreo);
                }               

                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                correosConCopia = correosConCopia.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, correosConCopia, asuntoCorreo, cuerpoCorreo, null, null);                
            }
        }

        public void ProcesarEnvioNotificacionesEspecificoSolicitud(int idTipoNotificacionApp, string codAPT, Dictionary<string, string> dict_notificacion, List<string> correos,string motivo)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
            var objAplicacion = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(codAPT);

            if (objTipoNotificacion != null && objAplicacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();


                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_CODIGO_APLICACION, codAPT);
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_APLICACION, codAPT)
                                                             .Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName);

                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                var toCorreo = new List<string>();
                if (listaPara != null)
                    toCorreo.AddRange(listaPara);
           
                var ccCorreo = correos.Select(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);             
            }
        }

        public void ProcesarEnvioNotificacionesEspecificoRechazo(int idTipoNotificacionApp, string codAPT, Dictionary<string, string> dict_notificacion, List<string> correos, string motivo)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
            var objAplicacion = ServiceManager<ApplicationDAO>.Provider.GetApplicationByCodigo(codAPT);

            if (objTipoNotificacion != null && objAplicacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();


                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_CODIGO_APLICACION, codAPT);
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_APLICACION, codAPT)
                                                             .Replace(LABEL_NOMBRE_APLICACION, objAplicacion.applicationName)
                                                               .Replace(LABEL_MOTIVO_ELIMINACION, motivo);

                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                //var fromCorreo = "julio.mendoza@itsight.pe";
                var toCorreo = correos.Select(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (listaPara != null)
                    toCorreo.AddRange(listaPara);
                var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
        }

        public void ProcesarEnvioNotificacionesConsulta(int idTipoNotificacionApp, string tipo, Dictionary<string, string> dict_notificacion, List<string> correos, string consulta)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);
           

            if (objTipoNotificacion != null )
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();
                var listaCC = string.IsNullOrWhiteSpace(objTipoNotificacion.ConCopia) ? null : objTipoNotificacion.ConCopia.Split(';').ToList();


                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto;
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_TIPO_CONSULTA, tipo)
                                                             .Replace(LABEL_CONSULTA, consulta);
                                                          

                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);

                var administradores = this.ListaCorreosAdministradores();
                var toCorreo = new List<string>();
                var ccCorreo = new List<string>();
                if (administradores != null)
                    toCorreo.AddRange(administradores);

                //var ccCorreo = correos.Select(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

                if (listaCC != null)
                    ccCorreo.AddRange(listaCC);
                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
        }

        public void ProcesarEnvioNotificacionesRespuesta(int idTipoNotificacionApp, string respuesta, Dictionary<string, string> dict_notificacion, List<string> correos, string consulta)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionApp(idTipoNotificacionApp);


            if (objTipoNotificacion != null)
            {
                //OBTENER LISTA DE ENVIOS DE CORREO
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaIdsRoles = new List<int>();
                var listaPara = string.IsNullOrWhiteSpace(objTipoNotificacion.Para) ? null : objTipoNotificacion.Para.Split(';').ToList();


                var mailService = new EmailApiService();

                //preparar correo
                var asuntoCorreo = objTipoNotificacion.Asunto;
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_RESPUESTA, respuesta)
                                                             .Replace(LABEL_CONSULTA, consulta);


                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                var toCorreo = correos.Select(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (listaPara != null)
                    toCorreo.AddRange(listaPara);

                var ccCorreo = objTipoNotificacion.ConCopia.Split(';').ToList();
                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
        }
        
        private void ValidarRolEnvioCorreo(ApplicationManagerRole rol, bool? flag, List<int> listaIdsRoles)
        {
            if (flag.HasValue && flag.Value)
            {
                listaIdsRoles.Add((int)rol);
            }
        }

        private List<string> ListaCorreosAdministradores()
        {
            var usuarios = new List<ApplicationManagerCatalogDto>();
            try
            {
                var azureManger = new AzureGroupsManager();
                var parametroAdministradores = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("GRUPO_ADMINISTRADORES");
                var usuariosAdmnistradores = new AzureGroupsManager().GetGroupMembersByName(parametroAdministradores.Valor);
                if (usuariosAdmnistradores != null)
                {
                    foreach (var item in usuariosAdmnistradores)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            applicationManagerId = (int)ApplicationManagerRole.AdministradorPortafolio,
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
                    case (int)ERoles.Administrador:
                        managerid = (int)ApplicationManagerRole.AdministradorPortafolio;

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
                                   select u.email).Distinct().ToList();
            return usuariosFinales;
        }
        private List<string> ListaCorreosParaEnvioSolicitud(int idSolicitud, int idTipo)
        {
            var usuarios = new List<ApplicationManagerCatalogDto>();
            try
            {
                var azureManger = new AzureGroupsManager();
                var parametroAdministradores = ServiceManager<SolicitudRolDAO>.Provider.ObtenerCorreoSolicitud(idSolicitud, idTipo);
                
                if (parametroAdministradores != null)
                {
                    foreach (var item in parametroAdministradores)
                    {
                        usuarios.Add(new ApplicationManagerCatalogDto()
                        {
                            //applicationManagerId = (int)ApplicationManagerRole.AdministradorPortafolio,
                            email = item.Mail,
                            username = item.Matricula,
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
                                   //u.applicationManagerId,
                                   u.username,
                                   u.email
                               }).Distinct().ToList();
            var usuariosFinales = (from u in tmpUsuarios
                                   select u.email).Distinct().ToList();
            return usuariosFinales;
        }
        public void ProcesarEnvioNotificacionesSolicitudRol(string idTipoNotificacionApp, Dictionary<string, string> dict_notificacion, string codProducto, string nombreProducto, int idSolicitud, string usuario, int idTipoSolicitud)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionPorNombre(idTipoNotificacionApp);

            if (objTipoNotificacion != null)
            {
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaCC = string.IsNullOrWhiteSpace(objTipoNotificacion.ConCopia) ? null : objTipoNotificacion.ConCopia.Split(';').ToList();
                var mailService = new EmailApiService();

                var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_NOMBRE_PRODUCTO, nombreProducto);
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_PRODUCTO, codProducto)
                                                             .Replace(LABEL_NOMBRE_PRODUCTO, nombreProducto)
                                                             .Replace(LABEL_TECNICO_ESPECIALISTA, usuario)
                                                             .Replace(LABEL_SOLICITUD, idSolicitud.ToString()
                                                             );


                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);

                int idTipoSol = 2;
                if (idTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                {
                    idTipoSol = 3;
                }
                

                var destinatarios = this.ListaCorreosParaEnvioSolicitud(idSolicitud, idTipoSol);
                var toCorreo = new List<string>();
                var ccCorreo = new List<string>();

                if (destinatarios != null)
                    toCorreo.AddRange(destinatarios);

                //===============================COMENTAR===============================
                //toCorreo = new List<string>();
                //var usuarios = new List<ApplicationManagerCatalogDto>();
                //usuarios.Add(new ApplicationManagerCatalogDto()
                //{
                //    email = "juanrodriguez@bcp.com.pe",
                //    username = "T12635",
                //    managerName = string.Empty
                //});

                //var tmpUsuarios = (from u in usuarios
                //                   select new
                //                   {
                //                       u.username,
                //                       u.email
                //                   }).Distinct().ToList();
                //var usuariosFinales = (from u in tmpUsuarios
                //                       select u.email).Distinct().ToList();

                //toCorreo.AddRange(usuariosFinales);
                //==============================================================

                if (listaCC != null)
                    ccCorreo.AddRange(listaCC);

                var conCopia = this.ListaCorreosParaEnvioSolicitud(idSolicitud, 1);
                ccCorreo.AddRange(conCopia);

                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
        }
        public void ProcesarEnvioNotificacionesObservacion(string idTipoNotificacionApp, Dictionary<string, string> dict_notificacion, string codProducto, string nombreProducto, int idSolicitud, int idTipoSolicitud)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionPorNombre(idTipoNotificacionApp);

            if (objTipoNotificacion != null)
            {
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                
                var listaCC = string.IsNullOrWhiteSpace(objTipoNotificacion.ConCopia) ? null : objTipoNotificacion.ConCopia.Split(';').ToList();
                var mailService = new EmailApiService();
                var asuntoCorreo = objTipoNotificacion.Asunto.Replace(LABEL_NOMBRE_PRODUCTO, nombreProducto);
                var cuerpoCorreo = objTipoNotificacion.Cuerpo.Replace(LABEL_CODIGO_PRODUCTO, codProducto)
                                                             .Replace(LABEL_NOMBRE_PRODUCTO, nombreProducto)
                                                             .Replace(LABEL_SOLICITUD, idSolicitud.ToString()
                                                             );


                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoCorreo = cuerpoCorreo.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);

                var destinatarios = this.ListaCorreosParaEnvioSolicitud(idSolicitud, 1);
                var toCorreo = new List<string>();
                var ccCorreo = new List<string>();

                if (destinatarios != null)
                    toCorreo.AddRange(destinatarios);

                // Copia de correo
                if (listaCC != null)
                    ccCorreo.AddRange(listaCC);

                int idTipoSol = 2;
                if (idTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                {
                    idTipoSol = 3;
                }

                var concopia = this.ListaCorreosParaEnvioSolicitud(idSolicitud, idTipoSol);

                if (concopia != null)
                    ccCorreo.AddRange(concopia);

                //===============================COMENTAR===============================
                //ccCorreo = new List<string>();
                //var usuarios = new List<ApplicationManagerCatalogDto>();
                //usuarios.Add(new ApplicationManagerCatalogDto()
                //{
                //    email = "juanrodriguez@bcp.com.pe",
                //    username = "T12635",
                //    managerName = string.Empty
                //});

                //var tmpUsuarios = (from u in usuarios
                //                   select new
                //                   {
                //                       u.username,
                //                       u.email
                //                   }).Distinct().ToList();
                //var usuariosFinales = (from u in tmpUsuarios
                //                       select u.email).Distinct().ToList();

                //ccCorreo.AddRange(usuariosFinales);
                //==============================================================

                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
        }

        public void ProcesarEnvioNotificacionTSI(string buzonSalida, List<string> toCorreo, List<string> ccCorreo, string cuerpoCorreo, string asuntoCorreo)
        {
            try
            {
                var mailService = new EmailApiService();
                var fromCorreo = mailService.GetUserByMail(buzonSalida);
 
                cuerpoCorreo = cuerpoCorreo.Replace("\n", "<br/>");
                cuerpoCorreo = cuerpoCorreo.Replace("\\n", "<br/>");
                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoCorreo, cuerpoCorreo, null, null);
            }
            catch (Exception ex)
            {

            }            
        }
        public void ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(string idTipoNotificacionApp, Dictionary<string, string> dict_notificacion, 
            string nombreTecnologia, List<string> listPara, List<string> listConCopia)
        {
            var objTipoNotificacion = ServiceManager<NotificacionDAO>.Provider.ObtenerTipoNotificacionPorNombre(idTipoNotificacionApp);

            if (objTipoNotificacion != null)
            {
                List<ApplicationManagerCatalogDto> listaCorreos = new List<ApplicationManagerCatalogDto>();
                var listaCC = string.IsNullOrWhiteSpace(objTipoNotificacion.ConCopia) ? null : objTipoNotificacion.ConCopia.Split(';').ToList();
                var mailService = new EmailApiService();

                var asuntoMail = objTipoNotificacion.Asunto.Replace(LABEL_NOMBRE_TECNOLOGIA, nombreTecnologia);
                var cuerpoMail = objTipoNotificacion.Cuerpo;
                
                if (dict_notificacion != null)
                {
                    foreach (KeyValuePair<string, string> entry in dict_notificacion)
                    {
                        cuerpoMail = cuerpoMail.Replace(entry.Key, entry.Value);
                    }
                }

                var fromCorreo = mailService.GetUserByMail(objTipoNotificacion.BuzonSalida);
                var toCorreo = new List<string>();
                var ccCorreo = new List<string>();

                if (listPara != null)
                    toCorreo.AddRange(listPara); 

                //===============================COMENTAR===============================
                //toCorreo = new List<string>();
                //var usuarios = new List<ApplicationManagerCatalogDto>();
                //usuarios.Add(new ApplicationManagerCatalogDto()
                //{
                //    email = "pierrgrimaldo@bcp.com.pe",
                //    username = "T14258",
                //    managerName = string.Empty
                //});

                //var tmpUsuarios = (from u in usuarios
                //                   select new
                //                   {
                //                       u.username,
                //                       u.email
                //                   }).Distinct().ToList();
                //var usuariosFinales = (from u in tmpUsuarios
                //                       select u.email).Distinct().ToList();

                //toCorreo.AddRange(usuariosFinales);
                //==============================================================

                if (listaCC != null)
                    ccCorreo.AddRange(listaCC);

                if (listConCopia != null)
                    ccCorreo.AddRange(listConCopia);

                cuerpoMail = cuerpoMail.Replace("\n", "<br/>");

                toCorreo = toCorreo.Distinct().ToList();
                ccCorreo = ccCorreo.Distinct().ToList();

                var rpta = mailService.SendMail(fromCorreo, toCorreo, ccCorreo, asuntoMail, cuerpoMail, null, null);
            }
        }

    }
}
