using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class SiemDAO : ServiceProvider
    {
        public abstract List<SiemDTO> GetListado(int pageNumber, int pageSize, out int totalRows);
        public abstract List<SiemDTO> GetListado2(int pageNumber, int pageSize, out int totalRows);
    }
}
