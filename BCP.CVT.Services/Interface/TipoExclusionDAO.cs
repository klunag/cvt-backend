using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class TipoExclusionDAO : ServiceProvider
    {
        public abstract List<TipoExclusionDTO> GetTipoExclusion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetTipoExclusionByFiltro(string filtro);
        public abstract int AddOrEditTipoExclusion(TipoExclusionDTO objeto);
        public abstract TipoExclusionDTO GetTipoExclusionById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
    }
}
