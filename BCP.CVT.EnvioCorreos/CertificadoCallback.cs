using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.EnvioCorreos
{
    public static class CertificadoCallback
    {
        static CertificadoCallback()
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidarCertificadoCallBack;
        }

        public static void Initialize()
        {
        }

        private static bool ValidarCertificadoCallBack(object sender
            , X509Certificate certificado
            , X509Chain chain
            , SslPolicyErrors politicas)
        {
            if (politicas == SslPolicyErrors.None)
                return true;

            if ((politicas & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificado.Subject == certificado.Issuer) &&
                            (status.Status == X509ChainStatusFlags.UntrustedRoot))
                            continue;
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                                return false;
                        }
                    }
                }
                return true;
            }
            else
                return false;

        }
    }
}
