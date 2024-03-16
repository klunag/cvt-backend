using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Dto
{
    public class ApplicationManagerCatalogDto
    {
        public int applicationManagerCatalogId { get; set; }
        public string applicationId { get; set; }
        public string username { get; set; }
        public string managerName { get; set; }
        public bool isActive { get; set; }
        public int applicationManagerId { get; set; }
        public DateTime? dateCreation { get; set; }
        public string createdBy { get; set; }
        public DateTime? dateModification { get; set; }
        public string modifiedBy { get; set; }
        public string email { get; set; }
        public string applicationManagerIdDetail { get; set; }
    }
}
