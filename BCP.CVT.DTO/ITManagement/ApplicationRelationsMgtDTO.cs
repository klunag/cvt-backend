using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class ApplicationRelationsMgtDTO
    {
        public string applicationId { get; set; }
        public string relationType { get; set; }
        public string status { get; set; }
        public string itResourceId { get; set; }
        public string technology { get; set; }
        public string relavance { get; set; }
        public bool? isResourceOwner { get; set; }
        public string component { get; set; }
        public DateTime? dateCreation { get; set; }
        public string createdBy { get; set; }
        public DateTime? dateModification { get; set; }
        public string modifiedBy { get; set; }
    }
}
