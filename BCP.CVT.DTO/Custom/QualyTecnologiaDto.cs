using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class QualyTecnologiaDto : BaseDTO
    {
        public int QualyId { get; set; }
        public int TecnologiaId { get; set; }
        public TecnologiaDTO Tecnologia { get; set; }
    }
}
