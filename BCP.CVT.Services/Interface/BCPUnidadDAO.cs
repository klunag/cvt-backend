using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class BCPUnidadDAO : ServiceProvider
    {
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoePorFiltro(string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro);
    }
}
