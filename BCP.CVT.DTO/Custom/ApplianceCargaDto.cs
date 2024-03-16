using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class ApplianceCargaDto: BaseDTO
    {
        public string Nombre { get; set; }
        public int TipoEquipo { get; set; }
        public int Ambiente { get; set; }
        public int Dominio { get; set; }

        public int? TipoActivo { get; set; }
        public string Serial { get; set; }
        public string Modelo { get; set; }
        public string Vendor { get; set; }
        public string Tecnologia { get; set; }
        public string Version { get; set; }
        public string Hostname { get; set; }
        public string IP { get; set; }
        public int? Dimension { get; set; }
        public string ArquitectoSeguridad { get; set; }
        public string SoportePrimerNivel { get; set; }
        public string Proveedor { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaFinExtendida { get; set; }
        public DateTime? FechaFinInterna { get; set; }
        public string ComentariosFechaFin { get; set; }
        public int? FechaCalculoId { get; set; }
        public DateTime? FechaAdquisicion { get; set; }
        public DateTime? FechaImplementacion { get; set; }
        public DateTime? FechaUltimaRenovacion { get; set; }
        public DateTime? VencimientoLicencia { get; set; }
        public int? CantidadLicencia { get; set; }
        public string FormaLicenciamiento { get; set; }
        public string CodigoInventario { get; set; }
        public string CyberSOC { get; set; }
        public int? Sede { get; set; }
        public string Sala { get; set; }
        public string RACK { get; set; }
        public string Ubicacion { get; set; }
        public int? Backup { get; set; }
        public int? BackupFrecuencia { get; set; }
        public string BackupDescripcion { get; set; }
        public int? IntegracionGestorInteligencia { get; set; }
        public string ConectorSIEM { get; set; }
        public int? CONA { get; set; }
        public string UmbralCPU { get; set; }
        public string UmbralMemoria { get; set; }
        public string UmbralDisco { get; set; }

        public string SoftwareBase { get; set; }
        public int? Criticidad { get; set; }
        public string UrlNube { get; set; }

        public bool ValidarExcel { get; set; }
    }
}
