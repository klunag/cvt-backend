using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TeamSquadDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }
        public string ResponsableMatricula { get; set; }
        public string ResponsableCorreo { get; set; }
        public string CodigoSIGA { get; set; }
        public int GestionadoPorId { get; set; }

        //NotMapped
        public int MantenimientoId { get; set; }
        public int EntidadRelacionId { get; set; }
        public int Nivel { get; set; }
        public bool IsSelected => false;
    }

    public class TribeLeaderDTO : BaseDTO
    {    
        public string Responsable { get; set; }
        public string ResponsableMatricula { get; set; }
        public string ResponsableCorreo { get; set; }

        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public int GestionadoPorId { get; set; }
    }

}
