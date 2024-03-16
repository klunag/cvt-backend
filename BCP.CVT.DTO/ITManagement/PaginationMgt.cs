using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{   
    public class APIResponse<T>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public APIResponse(string _Code, string _Description, List<string> _Errors = null)
        {
            Code = _Code;
            Description = _Description;
            Errors = _Errors;
        }

        public APIResponse(string _Code, string _Description, T _Data, string _Detail = null)
        {
            Code = _Code;
            Description = _Description;
            Detail = _Detail;
            Data = _Data;
        }
    }

    public class BasePaging
    {
        [Required]
        [Range(1,100)]
        public int? pageNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int? pageSize { get; set; }
    }

    public class ApplicationPag: BasePaging
    {
        public string gerence { get; set; }
        public string division { get; set; }
        public string unit { get; set; }
        public string area { get; set; }
        public string status { get; set; }
        public string technicalClassification { get; set; }
        public string technicalSubClassification { get; set; }
        public string applicationId { get; set; }
        public string assetType { get; set; }
        public string owner { get; set; }
    }

    public class ApplicationRelationsPag : BasePaging
    {
        public string applicationId { get; set; }
        [Required]
        public DateTime? date { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }

    public class ApplicationObsolescencePag
    {
        [Required]
        [StringLength(4)]
        public string applicationId { get; set; }
        [Required]
        public DateTime? date { get; set; }
    }

    public class ItResourcePag: BasePaging
    {
        public string types { get; set; }
        public string status { get; set; }
        public string discovery { get; set; }
        public string operatingSystem { get; set; }
        public string domain { get; set; }
        public string itResourceId { get; set; }
    }

    public class ItResourceRelationsPag : BasePaging
    {
        [Required]
        public DateTime? date { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string applicationId { get; set; }
        public string itResourceId { get; set; }
        public DateTime? dateCreatedBy { get; set; }
        public DateTime? dateModifiedBy { get; set; }
    }

    public class TechnologyPag: BasePaging
    {
        public string sortName { get; set; }
        public string sortOrder { get; set; }
        public string domain { get; set; }
        public string subDomain { get; set; }
        public string status { get; set; }
        public string family { get; set; }
        public string type { get; set; }
        public string technologyObsolescence { get; set; }
        public string name { get; set; }
    }

    public class ProductPag : BasePaging
    {
        public string productCode { get; set; }
    }

    //Results
    /***** Technology *****/
    public class TechnologyResponse
    {
        public int total { get; set; }
        public List<TechnologyDTO> technologies { get; set; }
    }

    public class ProductResponse
    {
        public int total { get; set; }
        public List<ProductDTO> products { get; set; }
    }
}
