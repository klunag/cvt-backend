using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class ArquetipoDAO : ServiceProvider
    {
        public abstract List<ArquetipoDTO> GetArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditArquetipo(ArquetipoDTO objeto);
        public abstract ArquetipoDTO GetArquetipoById(int id);       
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<RelacionTecnologiaDTO> GetListTecByArquetipo(int id);
        public abstract bool AsociarTecByArq(List<int> itemsTecId, List<int> itemsRemoveTecId, int arqId);
        public abstract List<CustomAutocomplete> GetArquetipoByFiltro(string filtro);
        public abstract bool ExisteArquetipoById(int Id);
        public abstract bool ExisteCodigoByFiltro(string filtro, int id);
    }
}
