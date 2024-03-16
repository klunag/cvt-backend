using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Graficos;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ReporteDAO : ServiceProvider
    {
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleResponsable(string codigoApt);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleResponsableAplicaciones(string matricula, int tipo);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalle(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleConsultor(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetAplicacionTecnologia(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetExportar(string aplicacion, string equipo, int tipo, string estado, string tecnologia, string subdominioIds, string ambiente, DateTime FechaConsulta);
        public abstract List<ReporteGerenciaDivisionDto> GetExportarConsultor(string aplicacion, string equipo, int tipo, string estado, string matricula, string tecnologia, string subdominioIds, string ambiente, DateTime FechaConsulta);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivision(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionExportar(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetAppsPorSegundNivel(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionResponsable(string matricula, int tipo);
        public abstract List<ReporteGerenciaDivisionDto> GetGerenciaDivisionConsultor(PaginaReporteGerencia pag, out int totalRows);
        public abstract List<ReporteServidoresHuerfanosDto> GetServidoresHuerfanos(PaginaReporteHuerfanos pag, DateTime fechaConsulta, int subdominioSO, out int totalRows);
        public abstract List<ReporteServidoresHuerfanosDto> GetServiciosNubeHuerfanos(PaginaReporteHuerfanos pag, DateTime fechaConsulta, out int totalRows);
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologias(PaginaReporteTecnologiasCustom pag, out int totalRows, bool detalle);

        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelaciones(PaginaReporteTecnologiasCustom pag, out int totalRows);
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesTodo();
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesAplicaciones();
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesEquipos();

        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaTodo();
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaServidores();
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaPCs();
        public abstract List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaAplicaciones();

        public abstract ReporteGraficoSubdominiosDto GetGrafico(string tipos, string subsidiaria, string subdominios, DateTime fechaConsulta, string tipoTecnologia);
        public abstract List<ReporteDetalladoSubdominioDto> GetDetalleGrafico(PaginacionDetalleGraficoSubdominio pag, out int totalRows);        
        public abstract List<ReporteDetalladoSubdominioDto> GetDetalleTecnologiasGrafico(PaginacionDetalleGraficoSubdominio pag, out int totalRows);
        public abstract List<ReporteEvolucionTecnologiaDto> GetEvolucionTecnologia(int tecnologia, out int totalRows);
        public abstract List<ReporteAcumuladoDto> GetEvolucionSubdominios(string tipoEquipos, string subdominios, string subsidiaria, DateTime fecha);
        public abstract List<ReporteAcumuladoDto> GetEvolucionSubdominiosEquipo(string tipoEquipos, string subdominios, string subsidiaria, DateTime fecha);
        public abstract List<ReporteDetalladoSubdominioDto> GetDetalleEvolucionSubdominios(PaginacionDetalleGraficoSubdominio pag, out int totalRows);
        public abstract List<ReporteAgrupacionDto> GetAgrupacion(string tipoEquipoIds, string subdominios, string agrupacion, string subsidiaria, string estados, DateTime fecha);
        public abstract List<ReporteAgrupacionDetalleDto> GetAgrupacionDetalle(string tipoEquipo, string subdominios, string agrupacion, string subsidiaria, string estados, DateTime fecha);

        public abstract List<ReporteDetalleTecnologiasDto> GetReporteDetalleTecnologia(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasAplicacionDto> GetReporteDetalleTecnologia_Aplicacion(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologia_Equipo(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologiaInstalaciones(FiltrosDashboardTecnologia pag, out int totalRows);

        public abstract List<ReporteDetalleTecnologiasFechasDto> GetReporteTecnologiaSinFechaFin(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasFechasDto> GetReporteTecnologiaFechaIndefinida(FiltrosDashboardTecnologia pag, out int totalRows);

        public abstract List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologiaInstalacionesByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologia_EquipoByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows);
        public abstract List<ReporteDetalleTecnologiasAplicacionDto> GetReporteDetalleTecnologia_AplicacionByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows);

        public abstract List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones(FiltrosDashboardTecnologia pag); //Reporte 1
        public abstract List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Tipo(FiltrosDashboardTecnologia pag); //Reporte 2
        public abstract List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Tipo_Equipo(FiltrosDashboardTecnologia pag); //Reporte 3
        public abstract List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_SO(FiltrosDashboardTecnologia pag); //Reporte 4
        public abstract List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Equipo_Agrupacion(FiltrosDashboardTecnologia pag); //Reporte 5

        //Consolidados
        public abstract List<ConsolidadoDto> GetConsolidadoBatch();
        public abstract List<VentanaDto> GetConsolidadoVentana();
        public abstract bool ExisteArchivoConsolidado(int Id);
        public abstract byte[] ObtenerArrConsolidado(int Id);
        public abstract string GetRutaConsolidadoByTipo(int Id);
        public abstract List<ConsolidadoDto> GetRelacionesAprobadas();
        public abstract List<ConsolidadoDto> GetRelacionesPendientes();
        public abstract List<ConsolidadoDto> GetRelacionesEliminadas();
        public abstract List<ConsolidadoDto> GetRelacionesDelta();

        //Indicadores gerenciales
        public abstract List<PieChart> GetIndicadorObsolescenciaSoBd(PaginacionDetalleGraficoSubdominio pag);

        //Nuevos reportes
        public abstract List<TecnologiaOwnerDto> GetReporteOwner(PaginacionOwner pag, out int totalRows);
        public abstract List<TecnologiaOwnerDto> GetReporteOwnerByTecnologia(PaginacionOwner pag, out int totalRows);
        public abstract List<TecnologiaOwnerDto> GetReporteOwnerConsolidado(PaginacionOwner pag, out int totalRows);

        public abstract List<TecnologiaOwnerDto> GetReporteOwnerByTecnologiav2(PaginacionOwner pag, out int totalRows);
        public abstract List<TecnologiaOwnerDto> GetReporteOwnerByAplicaciones(PaginacionOwner pag, out int totalRows);

        public abstract List<ReporteEvolucionInstalacionTecnologiasDto> ReporteEvolucionInstalacionTecnologias(string tipoEquipoIds, string subsidiariaIds, DateTime fechaBase, int nroMeses, bool flagAgrupacionFamilia, int? idTecnologia, string fabricante, string nombreTecnologia);
    }
}
