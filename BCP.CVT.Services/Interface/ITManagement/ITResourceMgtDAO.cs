using BCP.CVT.DTO.ITManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface.ITManagement
{
    public abstract class ITResourceMgtDAO : ServiceProvider
    {
        public abstract List<ItResourceMgtDTO> GetItResources(ItResourcePag pag, out int totalRows);
        public abstract ItResourceIdMgtDTO GetItResourceById(string itResourceId);
        public abstract List<TechnologyResourceDTO> GetTechnologiesByItResourceById(string itResourceId);
        public abstract List<RelatedApplicationDTO> GetRelatedApplicationsByItResourceById(string itResourceId);
        public abstract List<ItResourceRelationsMgtDTO> GetItResourceRelations(ItResourceRelationsPag pag, out int totalRows);
    }
}
