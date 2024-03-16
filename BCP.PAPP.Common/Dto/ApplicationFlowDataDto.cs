using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Dto
{
    public class ApplicationFlowDataDto
    {
        public int DataId { get; set; }
        public int FlowAppId { get; set; }
        public int fieldId { get; set; }
        public string currentValue { get; set; }
        public string newValue { get; set; }
        public DateTime? dateCreation { get; set; }
        public string createdBy { get; set; }
        public DateTime? dateModification { get; set; }
        public string modifiedBy { get; set; }

        public string currentValueDetail { get; set; }
        public string newValueDetail { get; set; }
    }
}
