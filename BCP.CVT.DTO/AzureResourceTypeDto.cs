using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AzureResourceTypeDto
    {
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }        
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsVirtualMachine { get; set; }

        public string StatusProcess
        {
            get
            {
                return IsActive ? "Analizar" : "Se ignora el análisis";
            }
        }
        public string CreationDateISO
        {
            get
            {
                return CreationDate.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string ModificationDateISO
        {
            get
            {
                return ModificationDate.HasValue ? ModificationDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
            }
        }

        public string IsVirtualMachineStr => IsVirtualMachine.HasValue ? IsVirtualMachine.Value ? "Si" : "No" : "No";
    }
}
