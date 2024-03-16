using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class TecnologiaCargaDto: BaseDTO
    {
        public int SubdominioId { get; set; }
        public int DominioId { get; set; }
        public int FamiliaId { get; set; }
        public int TipoId { get; set; }

        public string TecnologiaEquivalenteStr { get; set; }
        public string SubdominioStr { get; set; }
        public string DominioStr { get; set; }
        public string FamiliaStr { get; set; }
        public string TipoStr { get; set; }

        public DateTime? FechaLanzamiento { get; set; }
        public string FechaLanzamientoStr { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteStr { get; set; }
        public DateTime? FechaFinSoporteExtendida { get; set; }
        public string FechaFinSoporteExtendidaStr { get; set; }
        public DateTime? FechaFinInterna { get; set; }
        public string FechaFinSoporteInterna { get; set; }

        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }
        public string ClaveTecnologia => string.Format("{0} {1} {2}", Fabricante, Nombre, Version);

        public string ClaveTecnologiaValidar { get; set; }
        public string Estado { get; set; }
        public string CodigoTecnologia { get; set; }
        public string MostrarSite { get; set; }
        public string CasoUso { get; set; }
        public string Descripcion { get; set; }
        public string TieneFechaFinSoporte { get; set; }
        public string Fuente { get; set; }
        public string EquipoAdministracion { get; set; }
        public string GrupoRemedy { get; set; }
        public string ConformidadSeguridad { get; set; }
        public string ConformidadTecnologia { get; set; }
        public string Confluence { get; set; } 
    }
}
