using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.EnvioCorreos
{
    public static class ServiceEWS
    {
        static ServiceEWS()
        {
            CertificadoCallback.Initialize();
        }

        private static bool RedireccionarURLValidacionCallback(string url)
        {
            bool resultado = false;
            Uri redireccionUri = new Uri(url);

            if (redireccionUri.Scheme == "https")
                resultado = true;

            return resultado;
        }

        public static ExchangeService ConectarServicio(string correo
            , string contrasenia)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);

            service.Credentials = new NetworkCredential(correo, contrasenia);
            service.AutodiscoverUrl(correo, RedireccionarURLValidacionCallback);

            return service;
        }

        public static ExchangeService ConectarServicioImpersonado(string correo
            , string contrasenia
            , string correoImpersonar)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);

            service.Credentials = new NetworkCredential(correo, contrasenia);

            ImpersonatedUserId userId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress,
                correoImpersonar);

            service.ImpersonatedUserId = userId;
            service.AutodiscoverUrl(correo, RedireccionarURLValidacionCallback);

            return service;
        }
    }
}
