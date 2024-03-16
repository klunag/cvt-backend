using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class ModeloDAO : ServiceProvider
    {
        public abstract List<ModeloDTO> LeerModelo(string nombre, int tipo, string codigo, string nroSerie, string hostName, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int Agregarodelo(ModeloDTO modDTO);
        public abstract void DeleteModelo(int id, string usuario);
        public abstract void ActualizarCodigo(int id, string codigo);
        public abstract ModeloDTO BuscarModelo(int id);
        public abstract void EditarModelo(ModeloDTO modDTO);
        public abstract List<ModeloDTO> ExportarModelo(string criterio, int tipo, string nroSerie, string hostName);
        public abstract List<EquipoDTO> ObtenerEquipos(int modelo, string fabricante, int tipoId, string nombre, string nroSerie, string hostName, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> ModeloHardware_Combo();
        public abstract List<CustomAutocomplete> FabricanteHardware_Combo();
    }
}
