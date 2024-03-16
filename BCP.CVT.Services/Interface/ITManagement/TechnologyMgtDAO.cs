using BCP.CVT.DTO.ITManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface.ITManagement
{
    public abstract class TechnologyMgtDAO : ServiceProvider
    {
        public abstract List<TechnologyDTO> GetTechnologies(TechnologyPag pag, out int totalRows);
        public abstract List<TypeTechnologyDTO> GetTypeTechnologies();
        public abstract List<ReasonDTO> GetReasons();
        public abstract List<UndefinedReasonDTO> GetUndefinedReasons();
        public abstract List<DomainTechnologyDTO> GetDomainTechnologies();
        public abstract List<ProductDTO> GetProducts(ProductPag pag, out int totalRows);
        public abstract ProductDTO GetProductById(string productCode);
        public abstract List<SubdomainTechnologyDTO> GetSubdomainsByDomainTechnologyId(int domainId);
        public abstract ResponseTechnologyPost AddTechnology(TechnologyPostDTO objDTO);
        
    }
}
