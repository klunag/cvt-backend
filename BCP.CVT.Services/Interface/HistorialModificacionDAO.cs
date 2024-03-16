using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class HistorialModificacionDAO : ServiceProvider
    {
        public abstract bool RegistrarHistorialModificacion(HistorialModificacionDTO registro);
    }
}
