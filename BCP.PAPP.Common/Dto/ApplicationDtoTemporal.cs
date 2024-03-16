using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Dto
{
	public class ApplicationDtoTemporal
	{
        public int AppId { get; set; }
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public string description { get; set; }
        public int? implementationType { get; set; }
        public Nullable<int> managed { get; set; }
        public Nullable<int> deploymentType { get; set; }
        public string parentAPTCode { get; set; }
        public Nullable<int> status { get; set; }
        public string interfaceId { get; set; }
        public Nullable<System.DateTime> registerDate { get; set; }
        public string registerBy { get; set; }
        public Nullable<int> unit { get; set; }
        public string teamName { get; set; }
        public string userEntity { get; set; }
        public string developmentType { get; set; }
        public string developmentProvider { get; set; }
        public Nullable<int> infrastructure { get; set; }
        public string replacementApplication { get; set; }
        public Nullable<int> assetType { get; set; }
        public Nullable<int> BIANdomain { get; set; }
        public Nullable<int> mainOffice { get; set; }
        public Nullable<int> technologyCategory { get; set; }
        public Nullable<int> functionalLayer { get; set; }
        public string operatingHours { get; set; }
        public Nullable<bool> FlagComponenteCross { get; set; }
        public Nullable<int> technicalSubclassification { get; set; }
        public Nullable<int> technicalClassification { get; set; }
        public Nullable<int> area { get; set; }
        public Nullable<int> groupTicketRemedy { get; set; }
        public string webDomain { get; set; }
        public Nullable<System.DateTime> dateFirstRelease { get; set; }
        public string starProduct { get; set; }
        public string shorterApplicationResponseTime { get; set; }
        public string highestDegreeInterruption { get; set; }
        public Nullable<int> applicationCriticalityBIA { get; set; }
        public Nullable<int> classification { get; set; }
        public Nullable<int> finalCriticality { get; set; }
        public Nullable<int> tobe { get; set; }
        public string tierPreProduction { get; set; }
        public string tierProduction { get; set; }
        public Nullable<int> registrationSituation { get; set; }
        public Nullable<bool> isFormalApplication { get; set; }
        public Nullable<System.DateTime> regularizationDate { get; set; }
        public Nullable<int> authorizationMethod { get; set; }
        public Nullable<int> authenticationMethod { get; set; }
        public Nullable<bool> hasInterfaceId { get; set; }
        public Nullable<int> architectId { get; set; }
        public Nullable<int> architectSolutionId { get; set; }
        public Nullable<int> teamId { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public string approvedBy { get; set; }
        public Nullable<System.DateTime> dateApproved { get; set; }
        public Nullable<bool> isImported { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string desactivatedBy { get; set; }
        public Nullable<System.DateTime> dateDesactivated { get; set; }
        public Nullable<int> deploymentTypeOriginal { get; set; }
        public Nullable<bool> isObserved { get; set; }
        public string commentsObserved { get; set; }
        public string summaryStandard { get; set; }
        public Nullable<decimal> complianceLevel { get; set; }
        public string approvedByEmail { get; set; }
        public string desactivatedByEmail { get; set; }

        public Nullable<bool> aplicacionRevertida { get; set; }

    }
}
