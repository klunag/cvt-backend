using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoSoftwareBaseDTO : BaseDTO
    {
        public int EquipoId { get; set; }
        public int DominioId { get; set; }
        public int? SubdominioId { get; set; }
        
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
        public string CantidadLicencia { get; set; }
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

        public string EquipoDetalle { get; set; }
        public string EstadoIntegracionSIEM { get; set; }
        public string CONAAvanzado { get; set; }
        public string EstadoAppliance { get; set; }
        public string Ventana { get; set; }

        public string SubdominioStr { get; set; }
        public string ClaveEquipo => $"{Vendor} {Tecnologia} {Version}";
        public string TipoActivoStr { get; set; } //=> TipoActivo.HasValue ? Utilitarios.GetEnumDescription2((ETipoActivoEquipo)TipoActivo.Value) : "";
        public string DimensionStr { get; set; } //=> Dimension.HasValue ? Utilitarios.GetEnumDescription2((EDimensionEquipo)Dimension.Value) : "";
        public string SedeStr { get; set; } //=> Sede.HasValue ? Utilitarios.GetEnumDescription2((ESedeEquipo)Sede.Value) : "";
        public string BackupStr { get; set; } //=> Backup.HasValue ? Utilitarios.GetEnumDescription2((EBackup)Backup.Value) : "";
        public string BackupFrecuenciaStr { get; set; } //=> BackupFrecuencia.HasValue ? Utilitarios.GetEnumDescription2((EBackupFrecuencia)BackupFrecuencia.Value) : "";
        public string IntegracionGestorInteligenciaStr { get; set; } //=> IntegracionGestorInteligencia.HasValue ? Utilitarios.GetEnumDescription2((EIntegracionGestorInteligencia)IntegracionGestorInteligencia.Value) : "";
        public string ConaStr { get; set; } //=> CONA.HasValue ? Utilitarios.GetEnumDescription2((ECONA)CONA.Value) : "";
        public string CriticidadStr { get; set; } //=> Criticidad.HasValue ? Utilitarios.GetEnumDescription2((ECriticidadEquipo)Criticidad.Value) : "";
        public string CyberSOCStr { get; set; } 
        public string FechaCalculoStr => FechaCalculoId.HasValue ? Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)FechaCalculoId.Value) : ""; //Enum de tecnologia reutilizado

        public string CantidadLicenciaStr => !string.IsNullOrEmpty(CantidadLicencia) ? CantidadLicencia : "";
        public string FechaLanzamientoStr => FechaLanzamiento.HasValue ? FechaLanzamiento.Value.ToString("dd/MM/yyyy") : "";
        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "";
        public string FechaFinInternaStr => FechaFinInterna.HasValue ? FechaFinInterna.Value.ToString("dd/MM/yyyy") : "";
        public string FechaFinExtendidaStr => FechaFinExtendida.HasValue ? FechaFinExtendida.Value.ToString("dd/MM/yyyy") : "";

        public string FechaAdquisicionStr => FechaAdquisicion.HasValue ? FechaAdquisicion.Value.ToString("dd/MM/yyyy") : "";
        public string FechaImplementacionStr => FechaImplementacion.HasValue ? FechaImplementacion.Value.ToString("dd/MM/yyyy") : "";
        public string FechaUltimaRenovacionStr => FechaUltimaRenovacion.HasValue ? FechaUltimaRenovacion.Value.ToString("dd/MM/yyyy") : "";
        public string VencimientoLicenciaStr => VencimientoLicencia.HasValue ? VencimientoLicencia.Value.ToString("dd/MM/yyyy") : "";
        public bool? FlagSeguridad { get; set; }
        
        public string CodigoAPT { get; set; }
        public long? RelacionId { get; set; }
        public bool? FlagAprobado { get; set; }
        public string CorreoSolicitante { get; set; }
        public string NombreCreadoPor { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] ArchivoConstancia { get; set; }
        public bool TieneArchivo
        {
            get
            {
                return (ArchivoConstancia != null);
            }
        }

        public string NombreEquipo { get; set; }
    }
}
