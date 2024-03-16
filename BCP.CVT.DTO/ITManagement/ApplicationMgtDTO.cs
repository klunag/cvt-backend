using System;

namespace BCP.CVT.DTO.ITManagement
{
    public class ApplicationMgtDTO
    {
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public string description { get; set; }
        public string implementationType { get; set; }
        public string managed { get; set; }
        public string deploymentType { get; set; }
        public string parentAPTCode { get; set; }
        public string status { get; set; }
        public string interfaceId { get; set; }
        public DateTime? registerDate { get; set; }
        public string unit { get; set; }
        public string teamName { get; set; }
        public string expert { get; set; }
        public string userEntity { get; set; }
        public string developmentType { get; set; }
        public string developmentProvider { get; set; }
        public string infrastructure { get; set; }
        public string replacementApplication { get; set; }
        public string assetType { get; set; }
        public string BIANarea { get; set; }
        public string BIANdomain { get; set; }
        public string mainOffice { get; set; }
        public string technologyCategory { get; set; }
        public string technicalClassification { get; set; }
        public string technicalSubclassification { get; set; }
        public string authorizingUser { get; set; }
        public string centralGerence { get; set; }
        public string division { get; set; }
        public string area { get; set; }
        public string systemBroker { get; set; }
        public string tribeLead { get; set; }
        public string tribeTechnicalLead { get; set; }
        public string teamLeader { get; set; }
        public string owner { get; set; }
        public string groupTicketRemedy { get; set; }
        public string webDomain { get; set; }
        public DateTime? dateFirstRelease { get; set; }
        public string starProduct { get; set; }
        public string shorterApplicationResponseTime { get; set; }
        public string highestDegreeInterruption { get; set; }
        public string applicationCriticalityBIA { get; set; }
        public string classification { get; set; }
        public string finalCriticality { get; set; }
        public string tobe { get; set; }
        public string tierPreProduction { get; set; }
        public string tierProduction { get; set; }
        public string registrationSituation { get; set; }
        public bool? isFormalApplication { get; set; }
        public DateTime? regularizationDate { get; set; }
        public string authorizationMethod { get; set; }
        public string authenticationMethod { get; set; }
    }
}
