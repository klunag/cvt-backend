using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class TechnologyDTO
    {
        public string tecnologyId { get; set; }
        public string endDateSupport { get; set; }
        public string extendedDate { get; set; }
        public string agreementDate { get; set; }
        public bool? hasEndDateSupport { get; set; }
        public string sourceId { get; set; }
        public string technologyObsolescence { get; set; }
        public StatusDTO status { get; set; }
        public string name { get; set; }
        public TypeTechnologyDTO type { get; set; }
        public string subDomain { get; set; }
        public string domain { get; set; }
        public string family { get; set; }
        public string endDateSet { get; set; }
        public string roadmap { get; set; }
        public string tribuCoeId { get; set; }
        public string tribuCoeName { get; set; }
        public string squadId { get; set; }
        public string squadName { get; set; }
        public string ownerId { get; set; }
        public string ownerName { get; set; }
        public string technologyStatusId { get; set; }
    }

    public class TypeTechnologyDTO
    {
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only positive numbers")]
        public string technologyTypeId { get; set; }
        public string name { get; set; }
    }

    public class ReasonDTO
    {        
        public int reasonId { get; set; }
        public string name { get; set; }
    }

    public class UndefinedReasonDTO
    {
        public string undefinedReasonId { get; set; }
        public string name { get; set; }
    }

    public class StatusDTO
    {
        public int code { get; set; }
        public string description { get; set; }
    }

    public class DomainTechnologyDTO
    {
        public int domainId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class SubdomainTechnologyDTO
    {
        public int subDomainId { get; set; }
        public int domainId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ProductDTO
    {
        public int productId { get; set; }
        public string developmentCompany { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string domain { get; set; }
        public string subdomain { get; set; }
        //public string productType { get; set; }
        public string unitOrganizational { get; set; }
        public string subunitOrganizational { get; set; }
        public string ownerName { get; set; }
        public string groupTicketRemedy { get; set; }
        public string productCode { get; set; }
        public string lifeCycle { get; set; }
    }

    public class TechnologyPostDTO
    {
        public int technologyId { get; set; }
        [Required]
        public string vendor { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string version { get; set; }
        public string description { get; set; }        
        [Required]
        public TypeTechnologyDTO type { get; set; }
        public bool? hasEndDateSupport { get; set; }
        public int? endDateSupportSource { get; set; }
        
        public int? typeEndDateSupport { get; set; }
        public string endDateSupport { get; set; }
        public string useCase { get; set; }
        
        public string confluenceUrl { get; set; }
        public string complianceSecurityArchitect { get; set; }
        public string complianceTechnologyArchitect { get; set; }
        public string remedySupportGroup { get; set; }
        public string managementTeam { get; set; }
        public bool? shownStandarsSite { get; set; }        
        public int? existenceValueParam { get; set; }
        public int? usabilityValueParam { get; set; }
        public int? riskValueParam { get; set; }
        public int? vulnerabilityValueParam { get; set; }

        public string reference { get; set; }       
        public int? sourceId { get; set; }
        public bool? hasEquivalence { get; set; }
        public string productCode { get; set; }
        public int? implementationScript { get; set; }
        [Required]
        public int? productId { get; set; }

        public string internalDateType { get; set; }
        public string undefinedReasonId { get; set; }
        public string urlUndefinedDate { get; set; }
        public string applyTo { get; set; }
        public int? technologyReview { get; set; }
        public string compatibilitySO { get; set; }
        public string compatibilityCloud { get; set; }
        public int? licenseScheme { get; set; }
        public string endDateComments { get; set; }
        public string monitoringScheme { get; set; }

        public string userModification { get; set; }
    }

    public class ResponseTechnologyPost
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ResponseTechnologyPost(string _code, string _message)
        {
            Code = _code;
            Message = _message;
        }
    }

}
