using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class ItResourceIdMgtDTO
    {
        public string itResourceId { get; set; }
        public string type { get; set; }
        public string discovery { get; set; }
        public string status { get; set; }
        public string enviroment { get; set; }
        public string domain { get; set; }
        public string operatingSystem { get; set; }
        public string registerDate { get; set; }
        public List<TechnologyResourceDTO> technologies { get; set; }
        public List<RelatedApplicationDTO> relatedApplications { get; set; }
    }

    public class TechnologyResourceDTO
    {
        public string technologyDomain { get; set; }
        public string technologySubdomain { get; set; }
        public string technologyFullName { get; set; }
        public string technologyType { get; set; }
        public string endOfSupportTechnology { get; set; }
        public string currentObsolescence { get; set; }
    }

    public class RelatedApplicationDTO
    {
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public string enviroment { get; set; }
        public string status { get; set; }
    }
}
