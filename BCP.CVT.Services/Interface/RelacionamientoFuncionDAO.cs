using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class RelacionamientoFuncionDAO : ServiceProvider
    {
        public abstract List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List(PaginacionRelacionamientoFuncion pag, out int totalFilas);
        public abstract RelacionamientoFuncionDTO RelacionamientoFuncion_Id(int id);
        public abstract string RelacionamientoFuncion_Insert(RelacionamientoFuncionDTO relacionamientoFuncion);
        public abstract string RelacionamientoFuncion_Update(RelacionamientoFuncionDTO relacionamientoFuncion);
        public abstract List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List_Combo();
        public abstract List<CustomAutocomplete> GetByFiltro(string filtro);
    }
}
