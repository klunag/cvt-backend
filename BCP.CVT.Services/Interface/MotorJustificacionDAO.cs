using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class MotorJustificacionDAO : ServiceProvider
    {
        public abstract List<MotorJustificacionDTO> GetRegistros(PaginacionMotorJustificacion pag, out int totalRows);
    }
}
