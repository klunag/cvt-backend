using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class MotorProcesoDAO : ServiceProvider
    {
        public abstract List<MotorProcesoDTO> Proceso_List(Paginacion pag, out int totalFilas);
        public abstract MotorProcesoDTO Proceso_Id(int id);
        public abstract string Proceso_Insert(MotorProcesoDTO proceso);
        public abstract string Proceso_Update(MotorProcesoDTO proceso);
        public abstract List<MotorProcesoDTO> Proceso_List_Combo();
        public abstract List<CustomAutocomplete> GetByFiltro(string filtro);
    }
}
