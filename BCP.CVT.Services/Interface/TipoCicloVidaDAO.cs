using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class TipoCicloVidaDAO : ServiceProvider
    {
        public abstract List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditTipoCicloVida(TipoCicloVidaDTO objeto);
        public abstract TipoCicloVidaDTO GetTipoCicloVidaById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract void CambiarDefault(int id, string usuario);
        public abstract List<CustomAutocomplete> GetTipoCicloVidaByFiltro(bool FlagTecnologia);
    }
}

