using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class NotaAplicacionDTO: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string NombreAplicacion { get; set; }

        public decimal? Distribuida { get; set; }
        public decimal? MainFrame { get; set; }
        public decimal? PaqueteSaas { get; set; }
        public decimal? UserITMacro { get; set; }
        public decimal? UserITWeb { get; set; }
        public decimal? UserITClientSever { get; set; }

        public string TTL { get; set; }
        public string JDE { get; set; }
        public string Experto { get; set; }

        public string DistribuidaStr => Distribuida.HasValue ? string.Format("{0:0.00}%", Distribuida.Value) : "-";
        public string MainFrameStr => MainFrame.HasValue ? string.Format("{0:0.00}%", MainFrame.Value) : "-";
        public string PaqueteSaasStr => PaqueteSaas.HasValue ? string.Format("{0:0.00}%", PaqueteSaas.Value) : "-";
        public string UserITMacroStr => UserITMacro.HasValue ? string.Format("{0:0.00}%", UserITMacro.Value) : "-";
        public string UserITWebStr => UserITWeb.HasValue ? string.Format("{0:0.00}%", UserITWeb.Value) : "-";
        public string UserITClientSeverStr => UserITClientSever.HasValue ? string.Format("{0:0.00}%", UserITClientSever.Value) : "-";
    }
}
