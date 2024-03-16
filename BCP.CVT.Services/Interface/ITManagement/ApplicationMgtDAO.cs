using BCP.CVT.DTO.ITManagement;
using System;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface.ITManagement
{
    public abstract class ApplicationMgtDAO : ServiceProvider
    {
        public abstract List<ApplicationMgtDTO> GetApplications(ApplicationPag pag, out int totalRows);
        public abstract ApplicationIdMgtDTO GetApplicationById(string applicationId);
        public abstract List<PortfolioManagerDTO> GetPortfolioManagerByApplicationId(string applicationId);
        public abstract ApplicationObsMgtDTO GetApplicationObsolescenceById(string applicationId, DateTime date);
        public abstract List<ApplicationRelationsMgtDTO> GetApplicationRelationsById(ApplicationRelationsPag pag, out int totalRows);
    }
}
