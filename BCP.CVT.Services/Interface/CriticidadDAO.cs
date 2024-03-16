using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class CriticidadDAO : ServiceProvider
    {
        public abstract List<CriticidadDTO> GetAllCriticidad();
        public abstract List<CriticidadDTO> GetCriticidad(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditCriticidad(CriticidadDTO objeto);
        public abstract CriticidadDTO GetCriticidadById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<CustomAutocomplete> GetCriticidadByFiltro(string filtro);
    }
}
