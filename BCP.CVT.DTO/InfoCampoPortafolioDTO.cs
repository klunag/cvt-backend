using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class InfoCampoPortafolioDTO: BaseDTO
    {
        public int ConfiguracionColumnaAplicacionId { get; set; }
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public string Codigo { get; set; }
        public string ToolTip { get; set; }
        public int? SeccionTabId { get; set; }
        public string TipoFlujoId { get; set; }
        public int? TipoInputId { get; set; }
        public bool? FlagMultiselect { get; set; }

        public string ParametricaDescripcion { get; set; }
        public bool? FlagParametrica { get; set; }
        public int? MantenimientoPortafolioId { get; set; }

        public List<DataListBox> DataListBoxDetalle { get; set; }
        public int[] DataListBoxEliminar { get; set; }
    }
}
