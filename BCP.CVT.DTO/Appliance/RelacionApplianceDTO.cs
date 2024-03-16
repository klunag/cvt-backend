using System;

namespace BCP.CVT.DTO.Appliance
{
    public class RelacionApplianceDTO
    {
        public int EquipoId { get; set; }
        public string CodigoAPT { get; set; }
        public int? AmbienteId { get; set; }
        public int DiaRegistro { get; set; }
        public int MesRegistro { get; set; }
        public int AnioRegistro { get; set; }
    }
}
