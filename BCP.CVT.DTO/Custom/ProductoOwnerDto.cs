using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class ProductoOwnerDto
    {
        public int ProductoId { get; set; }
        public string ProductoStr { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public int TotalTecnologias { get; set; }
        public int TotalInstanciasServidores { get; set; }
        public int TotalInstanciasServicioNube { get; set; }
        public int TotalInstanciasPcs { get; set; }
        public int TotalAplicaciones { get; set; }
        public int TribuCoeId { get; set; }
        public string TribuCoeDisplayName { get; set; }
        public string TribuCoeDisplayNameResponsable { get; set; }
        public int SquadId { get; set; }
        public string SquadDisplayName { get; set; }
        public string SquadDisplayNameResponsable { get; set; }
    }
}
