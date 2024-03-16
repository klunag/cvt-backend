using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class DtoDominio
    {
        public int domainId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DtoSubdominio
    {
        public int subdomainId { get; set; }
        public int domainId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DtoTipoTecnologia
    {
        public int technologyTypeId { get; set; }
        public string name { get; set; }
    }

    public class DtoTecnologia
    {
        public int technologyId { get; set; }
        public string vendor { get; set; }
        public string technologyName { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string family { get; set; }
        public string technologyType { get; set; }
        public bool hasEndDateSupport { get; set; }
        public int endDateSupportSource { get; set; }
        public int typeEndDateSupport { get; set; }
        public DateTime? endDateSupport { get; set; }
        public string useCases { get; set; }
        public int domainId { get; set; }
        public int subdomainId { get; set; }
        public string confluenceUrl { get; set; }
        public string complianceSecurityArchitect { get; set; }
        public string complianceTechnologyArchitect { get; set; }
        public string remedySupportGroup { get; set; }
        public string managementTeam { get; set; }
        public bool? shownStandarsSite { get; set; }
        public int technologyState { get; set; }
        public string finalTechnologyType { get; set; }

        public int valueParameterExistencia { get; set; }
        public int valueParameterFacilidad { get; set; }
        public int valueParameterRiesgo { get; set; }
        public int valueParameterVulnerabilidad { get; set; }
    }

    public class DtoResultCode
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
