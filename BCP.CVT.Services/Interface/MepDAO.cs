using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class MepDAO : ServiceProvider
    {
        public abstract List<NotaAplicacionDTO> GetListado(PaginacionMep pag, out int totalRows);
        public abstract int AddOrEditNotaAplicacion(NotaAplicacionDTO objeto);
        public abstract NotaAplicacionDTO GetNotaAplicacionById(int id);
        public abstract bool ExisteCodigoAPT(string codigoAPT, int? id);
    }
}
