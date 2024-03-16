using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ProductoManagerRolesDTO: BaseDTO
    {
        public int ProductoId { get; set; }
        public string ManagerMatricula { get; set; }
        public string ManagerNombre { get; set; }
        public string ManagerEmail { get; set; }
        public int ProductoManagerId { get; set; }
        public string ProductoManagerStr { get; set; }
        public int Registro { get; set; }
    }
}
