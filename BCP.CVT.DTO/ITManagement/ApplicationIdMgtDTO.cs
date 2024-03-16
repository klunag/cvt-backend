using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class ApplicationIdMgtDTO
    {
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public string description { get; set; }
        public string interfaceId { get; set; }
        public string status { get; set; }
        public string assetType { get; set; }
        public string centralGerence { get; set; }
        public string division { get; set; }
        public string area { get; set; }
        public string unit { get; set; }
        public string BIANarea { get; set; }
        public string BIANdomain { get; set; }
        public string mainOffice { get; set; }
        public string technologyCategory { get; set; }
        public string technicalClassification { get; set; }
        public string technicalSubclassification { get; set; }
        public string managed { get; set; }
        public List<PortfolioManagerDTO> portfolioManager { get; set; }
    }

    public class PortfolioManagerDTO
    {
        public string employeeId { get; set; }
        public string fullName { get; set; }
        public int roleId { get; set; }
        public string role { get; set; }
        public string email { get; set; }
    }
}
