using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class TipoEquipoDAO : ServiceProvider
    {
        public abstract List<TipoEquipoDTO> GetTipoEquipo(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditTipoEquipo(TipoEquipoDTO objeto);
        public abstract TipoEquipoDTO GetTipoEquipoById(int id);
    }
}

