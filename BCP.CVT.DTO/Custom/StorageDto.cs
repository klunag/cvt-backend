using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class StorageNivelDto
    {
        public int StorageId { get; set; }
        public string Nombre { get; set; }
        public string StorageVolumen { get; set; }
        public int EquipoId { get; set; }
        public string Equipo { get; set; }
        public bool TieneReplica { get; set; }
        public decimal CapacidadTB { get; set; }
        public decimal UsadoTB { get; set; }
        public decimal LibreTB { get; set; }
        public string TieneReplicaToString
        {
            get
            {
                return TieneReplica ? "Si" : "No";
            }
        }
    }

    public class StorageResumenDto
    {
        public int UbicacionId { get; set; }
        public string DetalleUbicacion { get; set; }
        public decimal CapacidadTB { get; set; }
        public decimal UsadoTB { get; set; }
        public decimal LibreTB { get; set; }
        public decimal ReplicadoTB { get; set; }
        public decimal NoReplicadoTB { get; set; }
        public int StorageTierId { get; set; }
        public string NombreTier { get; set; }
        public int StorageId { get; set; }
        public string NombreStorage { get; set; }
        public int Obsoleto { get; set; }
        public string NombreReporte { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public int TecnologiaId { get; set; }
        public string ClaveTecnologia { get; set; }
    }

    public class StorageAplicacionDto
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string EstadoAplicacion { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public string Criticidad { get; set; }
    }

    public class StorageEquipoDto
    {
        public decimal UsadoMB { get; set; }
        public decimal LibreMB { get; set; }
        public List<StorageParticionDto> Particiones { get; set; }
    }

    public class StorageParticionDto
    {
        public string Disco { get; set; }
        public decimal UsadoMB { get; set; }
        public decimal LibreMB { get; set; }
    }

    public class StorageIndicadorDto
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public decimal CapacidadTB { get; set; }
        public decimal UsadoTB { get; set; }
        public decimal LibreTB { get; set; }
        public decimal ReplicaTB { get; set; }
        public decimal NoReplicaTB { get; set; }
        public decimal CapacidadNoObsoleta { get; set; }
        public decimal CapacidadObsoleta { get; set; }
        public string CodigoAPT { get; set; }
        public string MesToString
        {
            get
            {
                if (Mes > 0)
                    return Utilitarios.GetEnumDescription2((Mes)this.Mes);
                else
                    return string.Empty;
            }
        }
        public string Valor { get; set; }
        public decimal Capacidad { get; set; }
    }        
}
