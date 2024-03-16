using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class InstanciasBDDAO : ServiceProvider
    {        
        public abstract List<EquipoInstanciaBdDTO> GetInstanciasBd(PaginacionEquipoInstanciaBD pag, out int totalRows);
        public abstract void AddEditInstanciaBd(int EquipoId, string instanciaBd);
        public abstract string ValidarCambiarEstado(int Id);
        public abstract void CambiarEstado(EquipoInstanciaBdDTO obj);


    }
}
