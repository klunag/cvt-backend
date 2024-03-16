using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Storage
{
    public class StorageDto
    {
        public string Equipo { get; set; }
        public string SoftwareBase { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public decimal EspacioTotalGB { get; set; }
        public int EspacioProvisionadoMB { get; set; }
        public decimal EspacioUsadoGB { get; set; }        

        public string Storage { get; set; }
        public int EspacionTotalStorageMB { get; set; }
        public int EspacioProvisionadoStorageMB { get; set; }
        public int EspacioUsadoStorageMB { get; set; }
        public string ClaveStore { get; set; }
        public int NroDisco { get; set; }
        public int EspacioNoCompartidoMB { get; set; }
        public string Disco { get; set; }
        public decimal CapacidadMB { get; set; }
        public decimal CapacidadGB
        {
            get
            {
                return Math.Round(CapacidadMB / 1024, 2);
            }
        }
        public decimal UsadoMB { get; set; }
        public decimal UsadoGB
        {
            get
            {
                return Math.Round(UsadoMB / 1024, 2);
            }
        }
        public decimal LibreMB { get; set; }
        public decimal LibreGB
        {
            get
            {
                return Math.Round(LibreMB / 1024, 2);
            }
        }

        public string CodigoAPT { get; set; }
        public bool FlagTemporal { get; set; }
        public string FlagTemporalToString {
            get
            {
                return FlagTemporal ? "Carga manual" : "Descubierto automáticamente";
            }
        }
        public bool TieneReplica { get; set; }
        public string TieneReplicaToString
        {
            get
            {
                return TieneReplica ? "Si" : "No";
            }
        }
        public int EstadoId { get; set; }
        public string DetalleAmbiente { get; set; }
        public int Total { get; set; }
        public string EstadoIdToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)EstadoId);
            }
        }
        public string Volumen { get; set; }
        public string Etiqueta { get; set; }
        public int StorageTierId { get; set; }
        public string NombreTier { get; set; }
        public int IndicadorObsolescencia { get; set; }
        public int Fuente { get; set; }
        public string FuenteToString
        {
            get
            {
                if (Fuente == 0)
                    return string.Empty;
                else
                    return Fuente == 1 ? "x86" : "AIX";
            }
        }
        public bool FlagServidorServicio { get; set; }
        public string FlagServidorServicioToString
        {
            get
            {
                return FlagServidorServicio ? "Servidor de servicio" : string.Empty;
            }
        }
        public decimal PorcentajeLibre { get; set; }
        public string PoolName { get; set; }
        public int UbicacionId { get; set; }
        public string UbicacionToString
        {
            get
            {
                switch (UbicacionId)
                {
                    case (int)EUbicacionStorage.Chorrillos:
                        return "Chorrillos";
                    case (int)EUbicacionStorage.LaMolina:
                        return "La Molina";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
