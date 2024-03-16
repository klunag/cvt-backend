using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{ 
    public abstract class HardwareDAO : ServiceProvider
    {
        
        public abstract FiltrosHardware GetFiltros_Detallado();

        public abstract List<CustomAutocomplete> GetEquipoByFiltro(string filtro);
        public abstract List<ReporteEquipoHardwareDTO> GetReporteDetallado(PaginaHardwareDetallado pag, out int totalRows);


        public abstract FiltrosHardware GetFiltros_KPI();
        public abstract List<ReporteHardwareKpiDTO> GetReporteKPI(string matricula, int perfilId, string UnidadFondeoIds, string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds, int? nivel, string filtrosPadre);
        public abstract List<ReporteHardwareKpiDTO> GetReporteKPI_Exportar(string matricula, int perfilId, string UnidadFondeoIds, string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds);

    }
}
