using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TipoEquipoDTO : BaseDTO
    {
        public int TipoEquipoId { get; set; }
        public string Nombre { get; set; }
        public string ModificadoPor { get; set; }
        public int CriterioObsolescenciaId { get; set; }
        public string NombreCriterioObsolescencia { get; set; }
        public bool FlagExcluirKPI { get; set; }
        public int TipoCicloVidaId { get; set; }
        public string NombreTipoCicloVida { get; set; }
        public bool FlagIncluirHardwareKPI { get; set; }
        public int TotalFilas { get; set; }
        public bool FlagIncluirEnDiagramaInfra { get; set; }
    }
}
