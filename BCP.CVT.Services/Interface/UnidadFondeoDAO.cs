using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class UnidadFondeoDAO : ServiceProvider
    {
        public abstract List<UnidadFondeoDTO> Unidad_Fondeo_List(Paginacion pag, out int totalFilas);
        public abstract UnidadFondeoDTO Unidad_Fondeo_Id(int id);
        public abstract string Unidad_Fondeo_Insert(UnidadFondeoDTO unidadFondeo);
        public abstract string Unidad_Fondeo_Update(UnidadFondeoDTO unidadFondeo);
        public abstract string Unidad_Fondeo_Asignar(SegundoNivelDTO segundoNivel);
    }
}
