using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ProcesoDAO : ServiceProvider
    {
        public abstract void AddOrEditProceso(ProcesoDTO registro);
        public abstract int ValidarProcesoPorDia(int anio, int mes, int dia, int tipoTarea);
        public abstract List<ProcesoDTO> GetAllProceso();
        public abstract ProcesoDTO GetProceso(int? anio, int? mes, int? dia, int tipoTarea);
    }
}
