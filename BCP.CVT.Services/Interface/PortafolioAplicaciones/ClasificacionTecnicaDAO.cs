using BCP.CVT.DTO;
using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Services.ModelDB;

namespace BCP.CVT.Services.Interface.PortafolioAplicaciones
{
    public abstract partial class ClasificacionTecnicaDAO : ServiceProvider
    {
        public abstract List<ClasificacionTecnicaDTO> GetClasificacion(Paginacion pag, out int totalRows);
        public abstract ClasificacionTecnicaDTO GetClasificacionById(int id);
        public abstract int AddOrEditClasificacion(ClasificacionTecnicaDTO objeto);

        public abstract int AddSubClasificacion(SubClasificacionTecnicaDTO objeto);

        public abstract bool CambiarEstadoClasificacion(int id, bool estado, string usuario);
    }
}
