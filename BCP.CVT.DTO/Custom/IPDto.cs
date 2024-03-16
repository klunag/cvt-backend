using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class IPEstadoDto
    {
        public string Zona { get; set; }
        public string CMDB { get; set; }
        public string Fuente { get; set; }
        public string IPPorServer { get; set; }

        public int IPTotal { get; set; }
        public int VLAN { get; set; }

        public int AddmVerde { get; set; }
        public int AddmAmarillo { get; set; }
        public int AddmRojo { get; set; }
        public int SegmentacionVerde { get; set; }
        public int SegmentacionAmarillo { get; set; }
        public int SegmentacionRojo { get; set; }
        public int SeguridadVerde { get; set; }
        public int SeguridadRojo { get; set; }
        public int TelecomVerde { get; set; }
        public int TelecomRojo { get; set; }
    }

    public class IPConsolidadoDto
    {
        public string Prefix { get; set; }
        public string NamePrefix { get; set; }
        public string AmbienteRed { get; set; }

        public string Zona { get; set; }
        public string CMDB { get; set; }
        public string Fuente { get; set; }        
        public string IPPorServer { get; set; }

        public int IPTotal { get; set; }
        public int CMDBEscaneado { get; set; }
        public int SegRedOK { get; set; }
        public int SegRedNOK { get; set; }
        public int VLAN { get; set; }
        public int TotalEquipoCVT { get; set; }
        public int TotalEquipoIP { get; set; }

        public int ConsoleInventory { get; set; }
        public int Security { get; set; }
        public int Other { get; set; }
        public decimal SegredProgress { get; set; }        
        public int TotalPending { get; set; }        
        public decimal TotalProgress { get; set; }        
        public int TotalFilas { get; set; }

        public decimal PorcentajeControl
        {
            get
            {
                return IPTotal == 0 ? 0 : Math.Round(((decimal)SegRedOK / (decimal)IPTotal) * 100, 2);
            }
        }

        public int TotalEquipoInactivoCVT { get; set; }

        public int Identificacion { get; set; }
        public int Servidores { get; set; }
        public int ServidoresUnicos { get; set; }
        public string IdentificacionToString
        {
            get
            {
                return (Identificacion == 1 ? "IP identificadas" : "IP no identificadas");
            }
        }

    }

    public class IPResumenDto
    {
        public int TotalIPs { get; set; }
        public int TotalExisteIPs { get; set; }
        public int TotalCoincideCVT { get; set; }
        public int TotalServidores { get; set; }
        public int TotalServidoresAgencia { get; set; }
        public int TotalIPsAcumuladas { get; set; }
    }

    public class IPDetalleDto
    {
        public int TotalFilas { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public string VLAN { get; set; }
        public string Prefix { get; set; }
        public string NamePrefix { get; set; }
        public string Zona { get; set; }
        public string AmbienteRed { get; set; }
        public string Fuente { get; set; }
        public string CMDB { get; set; }
        public string EquipoReporte { get; set; }
        public string Virtual { get; set; }
        public string IPAddress { get; set; }
        public string IPPorSever { get; set; }
        public string AmbienteServidor { get; set; }
        public string CruceAmbientes { get; set; }
        public string Gateways { get; set; }
        public string StaticRoutes { get; set; }
        public string SegredStatus { get; set; }
        public string SecurityStatus { get; set; }
        public int ExisteCVT { get; set; }
        public int CoincideCVT { get; set; }
        public string Nombre { get; set; }
        public string TipoEquipo { get; set; }

        public int EstadoAddm { get; set; }
		public int EstadoSegmentacion { get; set; }
		public int EstadoSeguridad { get; set; }
		public int EstadoTelecom { get; set; }
        public string Plataforma { get; set; }
        public string EquipoInactivo { get; set; }
        public string Responsable { get; set; }
    }

    public class IPSeguimientoDto
    {
        public string Responsable { get; set; }
        public int StaticOK { get; set; }
        public int StaticNOK { get; set; }
        public int GatewaysOK { get; set; }
        public int GatewaysNOK { get; set; }
        public int TotalIP { get; set; }
        public int Total { get; set; }
        public int TotalExisteCVT { get; set; }
        public decimal PorcAvanceCVT { get; set; }

        public int NroEquipos { get; set; }
    }
}
