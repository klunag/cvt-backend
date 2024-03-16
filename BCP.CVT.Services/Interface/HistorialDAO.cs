using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class HistorialDAO : ServiceProvider
    {
        public abstract List<HistorialDTO> GetHistorialModificacionByEntidadId(string entidad, string id);
    }
}
