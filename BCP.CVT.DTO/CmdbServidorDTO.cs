using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CmdbServidorDTO
    {
        public string IdServidor { get; set; }
        public string NombreServidor { get; set; }
        public string NombreServidorDominio { get; set; }
        public string TotalMemoriaFisica { get; set; }
        public string NombreHostname { get; set; }
        public string Ambiente { get; set; }
        public string AmbienteId { get; set; }
        public string Estado { get; set; }
        public string Funcion { get; set; }
        public string Ubicacion { get; set; }
        public string NombreSistemaOperativo { get; set; }
        public string NombreFabricanteSistemaOperativo { get; set; }
        public string TipoSistemaOperativo { get; set; }
        public string VersionSistemaOperativo { get; set; }
        public string ModeloSistemaOperativo { get; set; }
        public string TotalProcesadoresFisicos { get; set; }
        public string TotalFileServer { get; set; }
        public string TotalIP { get; set; }
        public string TotalProcesadoresLogicos { get; set; }
        public string TotalSoftwareInstalado { get; set; }
        public string TotalStorageConfigurados { get; set; }
        public string RolServidor { get; set; }
        public bool Activo { get; set; }
        public bool Temporal { get; set; }
        public string IP { get; set; }
        public string Dominio { get; set; }
        public int TotalRegistros { get; set; }
        public int Obsolescencia { get; set; }
        public string Criticidad { get; set; }
    }

    public class CmdbAplicacionDTO
    {
        public string IdAplicacion { get; set; }
        public string IdServidor { get; set; }
        public string DescripcionCorta { get; set; }
        public string AdministradorAplicacion { get; set; }
        public string Ambiente { get; set; }
        public string CodigoBase { get; set; }
        public string CodigoAplicacion { get; set; }
        public string CodigoAplicacionEspecializada { get; set; }
        public string DescripcionCortaServidor { get; set; }
        public string Hostname { get; set; }
        public string NombreServidor { get; set; }
        public string URLCostoAplicacion { get; set; }
        public decimal CostoServidor { get; set; }
        public string SOServidor { get; set; }
        public string Expertos { get; set; }
        public string JefesServicio { get; set; }
        public string AplicacionRelacionada { get; set; }
        public string ServidoresRelacionados { get; set; }
        public string CriticidadToString { get; set; }
        public int TotalServidores { get; set; }
        public string AreaToString { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Unidad { get; set; }

    }

    public class CmdbCostosDTO
    {
        public string Servidor { get; set; }
        public string Rubro { get; set; }
        public string Categoria { get; set; }
        public decimal Costo { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
    }

    public class CmdbDiscoBigFix
    {
        public string Servidor { get; set; }
        public string Disco { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string FechaActualizacionToString
        {
            get
            {
                return FechaActualizacion.ToString("dd/MM/yyyy hh:mm:ss");
            }
        }
    }

}
