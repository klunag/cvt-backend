using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class GestionadoPorDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string CodigoSIGA { get; set; }
        public bool? FlagEquipoAgil { get; set; }
        public bool? FlagUserIT { get; set; }
        public bool? FlagSubsidiarias { get; set; }
        public bool? FlagJefeEquipo { get; set; }
        public string FlagEquipoAgilDetail => FlagEquipoAgil.HasValue ? FlagEquipoAgil.Value ? "Si" : "No" : "-";
        public string FlagUserITDetail => FlagUserIT.HasValue ? FlagUserIT.Value ? "Si" : "No" : "-";
        public string FlagSubsidiariasDetail => FlagSubsidiarias.HasValue ? FlagSubsidiarias.Value ? "Si" : "No" : "-";
        public string FlagJefeEquipoDetail => FlagJefeEquipo.HasValue ? FlagJefeEquipo.Value ? "Si" : "No" : "-";
    }
}
