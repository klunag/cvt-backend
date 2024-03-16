using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class QualyDto : BaseDTO
    {
        public string TipoVulnerabilidad { get; set; }
        public int NivelSeveridad { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string FechaPublicacionStr
        {
            get
            {
                return FechaPublicacion == null ? null : FechaPublicacion.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
        public string ListaSoftware { get; set; }
        public string Diagnostico { get; set; }
        public int? ProductoId { get; set; }
        public string Codigo { get; set; }
        public ProductoDTO Producto { get; set; }
        public string Solucion { get; set; }
        public string ReferenciaVendedor { get; set; }
        public string Amenaza { get; set; }
        public string Impacto { get; set; }
        public string Explotabilidad { get; set; }
        public string MalwareAsociado { get; set; }
        public string PCIVuln { get; set; }
        public string TecnologiaStr { get; set; }
        public int TotalTecnologias { get; set; }

        public int? EquipoId { get; set; }
        public string EquipoNombre { get; set; }
        public string TipoEquipoNombre { get; set; }        
    }

    public class VulnerabilidadEquipoDto
    {
        public int QID { get; set; }
        public int EquipoId { get; set; }
        public string Vuln_Status { 
            get
            {
                return Utilitarios.GetEnumDescription2((EstadoVulnerabilidades)VulnStatusId);
            }
        }
        public int Times_Detected { get; set; }
        public int Times_Reopened { get; set; }
        public string First_Detected { get; set; }
        public string Last_Detected { get; set; }
        public string Date_Last_Fixed { get; set; }
        public string Protocol { get; set; }
        public string Port { get; set; }
        public string ClaveTecnologia { get; set; }
        public string Nombre { get; set; }
        public string Producto { get; set; }
        public string Codigo { get; set; }
        public string Subdominio { get; set; }
        public string Dominio { get; set; }
        public string Titulo { get; set; }
        public int NivelSeveridad { get; set; }
        public string Solucion { get; set; }
        public string Diagnostico { get; set; }
        public string ListaSoftware { get; set; }
        public string CodigoAPT { get; set; }        
        public string EstadoAplicacion 
        {
            get
            {
                return Utilitarios.GetEnumDescription2((ApplicationState)status);
            }
        }
        public string Aplicacion { get; set; }
        public int status { get; set; }
        public int VulnStatusId { get; set; }
        public string GestionadoPor { get; set; }
        public string TeamSquad { get; set; }
        public string Categoria { get; set; }
    }
}
