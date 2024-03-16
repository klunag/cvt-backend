using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class FamiliaDAO : ServiceProvider
    {
        public abstract List<FamiliaDTO> GetFamilia(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<FamiliaDTO> GetAllFamilia();
        public abstract List<CustomAutocomplete> GetAllFamiliaByFiltro(string filtro);
        public abstract int AddOrEditFamilia(FamiliaDTO objeto);    
        public abstract FamiliaDTO GetFamiliaById(int id);
        public abstract List<TecnologiaDTO> GetTecByFamilia(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaDTO> GetTecVinculadas(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract bool ExisteFamiliaById(int? Id, string nombre);
        public abstract bool AsociarTecnologiasByFamilia(int tecnologiaId, int familiaId, string usuario);
        public abstract bool ExisteFamiliaByNombre(int? Id, string nombre);
    }
}
