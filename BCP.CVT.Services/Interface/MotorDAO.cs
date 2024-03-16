using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class MotorDAO : ServiceProvider
    {
        public abstract List<MotorExcluirDTO> ProcesoExcluir_List(Paginacion pag, out int totalFilas);
        public abstract MotorExcluirDTO ProcesoExcluir_Id(int id);
        public abstract string ProcesoExcluir_Insert(MotorExcluirDTO proceso);
        public abstract string ProcesoExcluir_Update(MotorExcluirDTO proceso);
        public abstract List<MotorExcluirDTO> ServidorOrigenExcluir_List(Paginacion pag, out int totalFilas);
        public abstract MotorExcluirDTO ServidorOrigenExcluir_Id(int id);
        public abstract string ServidorOrigenExcluir_Insert(MotorExcluirDTO proceso);
        public abstract string ServidorOrigenExcluir_Update(MotorExcluirDTO proceso);
    }
}
