using BCP.CVT.Cross;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BCP.CVT.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            HandlerConfig.RegisterHandlers(GlobalConfiguration.Configuration.MessageHandlers);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            log4net.Config.XmlConfigurator.Configure();
            //MvcHandler.DisableMvcResponseHeader = true;


            var cache = new MemoryCacheManager<string>();

            bool resultServer = true;
            Constantes.ServidorSQL = cache.GetCache("ServidorSQL");
            if (Constantes.ServidorSQL == null)
            {
                if(!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.ServidorSQL = cache.GetOrCreate("ServidorSQL", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.ServerSQL"), out resultServer));
                else
                    Constantes.ServidorSQL = Settings.Get<string>("Secretos.E195-ServidorAzureSQL");
            }

            bool resultBd = true;
            Constantes.BaseDatosSQL = cache.GetCache("BaseDatosSQL");
            if (Constantes.BaseDatosSQL == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.BaseDatosSQL = cache.GetOrCreate("BaseDatosSQL", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.BDSQL"), out resultBd));
                else
                    Constantes.BaseDatosSQL = Settings.Get<string>("Secretos.E195-BDAzureSQL");
            }

            bool resultUsuario = true;
            Constantes.UsuarioSQL = cache.GetCache("UsuarioSQL");
            if (Constantes.UsuarioSQL == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.UsuarioSQL = cache.GetOrCreate("UsuarioSQL", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.UserSQL"), out resultUsuario));
                else
                    Constantes.UsuarioSQL = Settings.Get<string>("Secretos.E195-UsuarioBD");
            }

            Constantes.PasswordSQL = cache.GetCache("PasswordSQL");
            bool resultPassword = true;
            if (Constantes.PasswordSQL == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.PasswordSQL = cache.GetOrCreate("PasswordSQL", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.PwdSQL"), out resultPassword));
                else
                    Constantes.PasswordSQL = Settings.Get<string>("Secretos.E195-UsuarioContrasenia");
            }

            Constantes.AzureAppSecret = cache.GetCache("SecretoApp");
            bool resultSecreto = true;
            if (Constantes.AzureAppSecret == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.AzureAppSecret = cache.GetOrCreate("SecretoApp", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.SecretApp"), out resultSecreto));
                else
                    Constantes.AzureAppSecret = Settings.Get<string>("Secretos.E195-SecretAPP");
            }

            try
            {
                Constantes.StorageName = cache.GetCache("StorageNombre");
                bool resultStorageName = true;
                if (Constantes.StorageName == null)
                {
                    if (!Settings.Get<bool>("Secretos.Local.Activar"))
                        Constantes.StorageName = cache.GetOrCreate("StorageNombre", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.StorageName"), out resultStorageName));
                    else
                        Constantes.StorageName = Settings.Get<string>("Secretos.E195-StorageNombre");
                }

                Constantes.StorageKey = cache.GetCache("StorageKey");
                bool resultStorageClave = true;
                if (Constantes.StorageKey == null)
                {
                    if (!Settings.Get<bool>("Secretos.Local.Activar"))
                        Constantes.StorageKey = cache.GetOrCreate("StorageKey", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.StorageKey"), out resultStorageClave));
                    else
                        Constantes.StorageKey = Settings.Get<string>("Secretos.E195-StorageClave");
                }
            }
            catch (Exception ex)
            {
                
            }

            Constantes.TokenJenkins = cache.GetCache("TokenJenkins");
            bool resultTokenJenkins = true;
            if (Constantes.TokenJenkins == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.TokenJenkins = cache.GetOrCreate("TokenJenkins", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.TokenJenkins"), out resultTokenJenkins));
                else
                    Constantes.TokenJenkins = Settings.Get<string>("Secretos.E195-TokenJenkins");
            }

            Constantes.UsuarioJenkins = cache.GetCache("UsuarioJenkins");
            bool resultUsuarioJenkins = true;
            if (Constantes.UsuarioJenkins == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.UsuarioJenkins = cache.GetOrCreate("UsuarioJenkins", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.UsuarioJenkins"), out resultUsuarioJenkins));
                else
                    Constantes.UsuarioJenkins = Settings.Get<string>("Secretos.E195-UsuarioJenkins");
            }

            Constantes.JwtSecretKey = cache.GetCache("JwtSecretKey");
            bool resultJwtSecretKey = true;
            if (Constantes.JwtSecretKey == null)
            {
                if (!Settings.Get<bool>("Secretos.Local.Activar"))
                    Constantes.JwtSecretKey = cache.GetOrCreate("JwtSecretKey", AzureUtilitarios.GetSecret(Settings.Get<string>("Azure.KeyVault.Secret.JwtSecretKey"), out resultJwtSecretKey));
                else
                    Constantes.JwtSecretKey = Settings.Get<string>("Secretos.E195-JwtSecretKey");
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("Error ", exc);

            if (exc.GetType() == typeof(HttpException))
            {
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                Server.ClearError();
                Server.Transfer("~/errores/HttpErrorPage.html");
            }
            else
            {
                Server.Transfer("~/errores/ErrorPage.html");
            }
        }
    }
}
