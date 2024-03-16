using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class IPDAO : ServiceProvider
    {
        public abstract List<IPDetalleDto> GetVistaConsolidado(Paginacion pag, out int totalRows);
        public abstract List<IPResumenDto> GetVistaEstadoResumen();
        public abstract List<IPDetalleDto> GetVistaEstadoDetalle(PaginacionIP pag, out int totalRows);

        public abstract List<IPConsolidadoDto> GetVistaConsolidadoNivel1(DateTime fecha);
        public abstract List<IPConsolidadoDto> GetVistaConsolidadoNivel2(int identificacion, DateTime fecha);
        public abstract List<IPConsolidadoDto> GetVistaConsolidadoNivel3(int identificacion, string cmdb, DateTime fecha);
        public abstract List<IPConsolidadoDto> GetVistaConsolidadoNivel4(int identificacion, string cmdb, string zona, DateTime fecha);
        public abstract List<IPDetalleDto> GetVistaConsolidadoNivel5(int identificacion, string cmdb, string zona, string ips, DateTime fecha);

        public abstract List<IPEstadoDto> GetVistaEstadoNivel1(DateTime fecha);
        public abstract List<IPEstadoDto> GetVistaEstadoNivel2(string zona, DateTime fecha);
        public abstract List<IPEstadoDto> GetVistaEstadoNivel3(string zona, string addm, DateTime fecha);
        public abstract List<IPEstadoDto> GetVistaEstadoNivel4(string zona, string addm, string fuente, DateTime fecha);
        public abstract List<IPDetalleDto> GetVistaEstadoNivel5(string zona, string addm, string fuente, string ips, DateTime fecha);

        public abstract List<IPSeguimientoDto> GetSeguimiento(PaginacionIP pag);
    }
}
