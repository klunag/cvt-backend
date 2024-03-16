using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class EntornoDAO : ServiceProvider
    {
        public abstract List<EntornoDTO> GetEntorno(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);     
        public abstract List<CustomAutocomplete> GetEntornoByFiltro(string filtro);
        public abstract int AddOrEditEntorno(EntornoDTO objeto);
        public abstract EntornoDTO GetEntornoById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
    }
}
