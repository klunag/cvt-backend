using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class ProtocoloDAO : ServiceProvider
    {
        public abstract List<ProtocoloDTO> Protocolo_List(Paginacion pag, out int totalFilas);
        public abstract ProtocoloDTO Protocolo_Id(int id);
        public abstract string Protocolo_Insert(ProtocoloDTO protocolo);
        public abstract string Protocolo_Update(ProtocoloDTO protocolo);
        public abstract List<ProtocoloDTO> Protocolo_List_Combo();
        public abstract List<CustomAutocomplete> GetByFiltro(string filtro);
    }
}
