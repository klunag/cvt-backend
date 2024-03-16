using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class TipoDAO: ServiceProvider
    {
        public abstract List<TipoDTO> GetTipo(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TipoDTO> GetAllTipo();
        public abstract List<DtoTipoTecnologia> GetAllTipoActivos();
        public abstract List<CustomAutocomplete> GetAllTipoByFiltro(string filtro);
        public abstract int AddOrEditTipo(TipoDTO objeto);
        public abstract TipoDTO GetTipoById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract bool TieneFlagEstandar();
        public abstract bool GetFlagEstandar(int id);
        public abstract bool ValidarFlagEstandar(int id);
    }
}
