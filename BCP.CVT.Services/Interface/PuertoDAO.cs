using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class PuertoDAO : ServiceProvider
    {
        public abstract List<PuertoDTO> Puerto_List(Paginacion pag, out int totalFilas);
        public abstract PuertoDTO Puerto_Id(int id);
        public abstract string Puerto_Insert(PuertoDTO puerto);
        public abstract string Puerto_Update(PuertoDTO puerto);
        public abstract List<PuertoDTO> Puerto_List_Combo();
        public abstract List<CustomAutocomplete> GetByFiltro(string filtro);
    }
}
