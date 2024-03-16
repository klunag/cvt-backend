using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class PlotlyDataDTO : CustomAutocomplete
    {

        public decimal X { get; set; }
        public decimal Y { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string FiltroEstado { get; set; }
    }
}
