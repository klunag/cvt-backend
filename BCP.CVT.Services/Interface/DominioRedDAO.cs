using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class DominioRedDAO : ServiceProvider
    {
        public abstract List<DominioServidorDTO> GetDominioRed(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);        
        public abstract List<CustomAutocomplete> GetDominioRedByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDominioRedActivos();
        public abstract int AddOrEditDominioRed(DominioServidorDTO objeto);
        public abstract DominioServidorDTO GetDominioRedById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
    }
}
