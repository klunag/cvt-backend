using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TecnologiaInstanciaDTO: BaseDTO
    {
        public int TecnologiaId { get; set; }
        public string ClaveTecnologia { get; set; }
        public int TipoEquipoId { get; set; }
        public string TipoEquipo { get; set; }
        public int EquipoId { get; set; }
        public string Equipo { get; set; }
        public int AmbienteId { get; set; }
        public string Ambiente { get; set; }
    }

    public class TecnologiaRelacionDTO : BaseDTO
    {
        public int TecnologiaId { get; set; }
        public string ClaveTecnologia { get; set; }
        public string CodigoAPT { get; set; }
        public string NombreAplicacion { get; set; }
        public string GestionadoPor { get; set; }
    }
}
