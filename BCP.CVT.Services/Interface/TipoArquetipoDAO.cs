using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class TipoArquetipoDAO : ServiceProvider
    {
        public abstract List<TipoArquetipoDTO> GetTipoArquetipo(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetTipoArquetipoByFiltro(string filtro);
        public abstract int AddOrEditTipoArquetipo(TipoArquetipoDTO objeto);
        public abstract TipoArquetipoDTO GetTipoArquetipoById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
    }
}
