using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RolesGestionDTO: BaseDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string RoleDetail
        {
            get
            {
                return Utilitarios.GetEnumDescription2((ERoles)RoleId);
            }
        }
    }
}
