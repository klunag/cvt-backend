using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class DominioDAO : ServiceProvider
    {
        public abstract List<DominioDTO> GetDominio(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditDominio(DominioDTO objeto);
        public abstract DominioDTO GetDominioById(int id);
        public abstract List<SubdominioDTO> GetSubdominiosByDominio(int id, int pageNumber, int pageSize, out int totalRows);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<DominioDTO> GetAllDominio();
        public abstract List<DominioDTO> GetDomConSubdom();
        public abstract List<CustomAutocomplete> GetAllDominioByFiltro(string filtro);
        public abstract int CambiarDominio(int SubdomId, int DomId);
        public abstract string GetMatriculaDominio(int id);

        public abstract List<DtoDominio> GetDominios();
        public abstract List<DtoSubdominio> GetSubdominios(int dominio);
    }
}
