using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Descubrimiento
{
    public class AddmSoftwareServidorDto
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ProductVersion { get; set; }
        public string Host { get; set; }
        public string ExtendedEosDate { get; set; }
        public string EosDate { get; set; }
        public string RetirementDate { get; set; }        
    }
}
