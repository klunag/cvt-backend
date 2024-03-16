using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class AmbienteDAO : ServiceProvider
    {
        public abstract List<AmbienteDTO> GetAmbiente(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<AmbienteDTO> GetVentana(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<AmbienteDTO> GetAmbiente();
        public abstract int AddOrEditAmbiente(AmbienteDTO objeto);
        public abstract AmbienteDTO GetAmbienteById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<CustomAutocomplete> GetAmbienteByFiltro(string filtro);
        public abstract bool ExisteCodigoByFiltro(int codigo, int id);
        public abstract int UpdateVentana(AmbienteDTO objeto);
    }
}
