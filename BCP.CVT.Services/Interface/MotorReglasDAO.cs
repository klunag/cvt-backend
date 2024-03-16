using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class MotorReglasDAO : ServiceProvider
    {
        public abstract List<MotorReglasDTO> GetMotorReglas(PaginacionMotorReglas pag, out int totalRows);
        public abstract string CambiarEstado(int id, bool estado, string usuario);

        public abstract string AddOrEditMotorReglas(MotorReglasDTO objeto);
    }
}
