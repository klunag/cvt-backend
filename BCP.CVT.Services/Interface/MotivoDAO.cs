using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class MotivoDAO : ServiceProvider
    {
        public abstract List<MotivoDTO> GetMotivo(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<MotivoDTO> GetAllMotivo();
        public abstract List<CustomAutocomplete> GetAllMotivoByFiltro(string filtro);
        public abstract int AddOrEditMotivo(MotivoDTO objeto);
        public abstract MotivoDTO GetMotivoById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract bool ExisteMotivoById(int? Id, string nombre);
        public abstract bool ExisteMotivoByNombre(int? Id, string nombre);
    }
}
