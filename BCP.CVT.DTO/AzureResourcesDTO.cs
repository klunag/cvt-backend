using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AzureResourcesDTO: BaseDTO
    {
        public string SuscriptionId { get; set; }
        public string SuscriptionDisplayName { get; set; }
        public string SuscriptionState { get; set; }
        public string ResourcesName { get; set; }
        public string ResourcesType { get; set; }
        public string ResourcesLocation { get; set; }
        public string ResourcesState { get; set; }
        public string ResourceGroup { get; set; }
        public string ResourcesSO { get; set; }
        public string VMId { get; set; }
        public string AKSVersion { get; set; }

        public string CodigoAPT { get; set; }
        public string Equivalence { get; set; }
        public string Hostnames { get; set; }

        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string ClaveTecnologia { get; set; }
        public string Analizar { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string TipoEquipo { get; set; }
        public string Observaciones { get; set; }

        public AzureResourcesDTO()
        {

        }
    }
}
