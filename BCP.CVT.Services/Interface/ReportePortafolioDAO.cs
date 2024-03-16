using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Graficos;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BCP.CVT.Services.Interface
{
    public abstract class ReportePortafolioDAO : ServiceProvider
    {
        public abstract List<AreaBianDto> GetReporteAreaBian(string gerencia, string estado);
        public abstract List<AreaBianDto> GetReporteAreaBianBT(PaginacionAplicacion pag);
        public abstract List<ReporteAplicacionDto> GetReporteAreaBianDetalle(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionReporteDto> GetReporteAplicaciones(string gerencia);
        public abstract List<AplicacionReporteDto> GetReporteAplicacionesBT(PaginacionAplicacion pag);
        public abstract List<ReporteAplicacionDto> GetReporteAplicacionesTotalDetalle(PaginacionAplicacion pag, out int totalRows);
        public abstract List<ReporteAplicacionDto> GetReporteAplicacionesInfraestructuraDetalle(PaginacionAplicacion pag, out int totalRows);
        public abstract List<ReporteAplicacionDto> GetReporteAplicacionesInfraestructuraDesarrolloDetalle(PaginacionAplicacion pag, out int totalRows);
        public abstract List<ReporteAplicacionDto> GetReporteAplicacionesObsolescenciaDetalle(PaginacionAplicacion pag, out int totalRows);

        public abstract List<ReporteAlertasDto> GetReporteAlertas(PaginacionAplicacion pag, out int totalRows);

        public abstract List<InstanciasDto> GetInstancias(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasEquipos(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasEquiposAplicaciones(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasProductos(PaginacionEquipo pag, out int totalRows);

        public abstract List<InstanciasDto> GetInstanciasFamilias(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasFamiliasProductos(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasFamiliasTecnologias(PaginacionEquipo pag, out int totalRows);
        public abstract List<InstanciasDto> GetInstanciasFamiliasAplicaciones(PaginacionEquipo pag, out int totalRows);

        public abstract List<InstanciasDto> GetResumenAplicaciones(PaginacionAplicacion pag, out int totalRows);
        public abstract List<InstanciasDto> GetResumenAplicacionesDetalle(PaginacionAplicacion pag, out int totalRows);



        public abstract List<InstanciasDto> GetInstanciasGrafico(PaginacionEquipo pag);
        public abstract List<InstanciasDto> GetInstanciasProductosGrafico(PaginacionEquipo pag);
        public abstract List<InstanciasDto> GetInstanciasEquiposGrafico(PaginacionEquipo pag);
        public abstract List<InstanciasDto> GetInstanciasEquiposAplicacionesGrafico(PaginacionEquipo pag);

        public abstract ReporteEstadoPortafolioDTO GetReporteEstadoPortafolio(FiltrosReporteEstadoPortafolio filtros);
        public abstract ReporteCambioBajaResponsablesGDHDTO GetReporteCambioBajaResponsableGDH(FiltrosReporteCambioBajaResponsableGDH reporte);
        public abstract BootstrapTable<ReporteEstadoPortafolioRoles> PortafolioListadoRolesAplicaciones(FiltrosReporteEstadoPortafolio filtros);
        public abstract List<ReporteEstadoPortafolio> GetDataSaludAplicationsExport(FiltrosReporteEstadoPortafolio filtros);

        public abstract int GetNroPeriodos(string fechaBase, int frecuencia);

        public abstract List<AplicacionReporteDto> GetExportarAplicacionesBT(string management,string functionalLayer, string sortName, string sortOrder);

        #region Reprte Campos
        public abstract List<CustomAutocomplete> GetListAgrupamiento();
        public abstract List<CustomAutocomplete> GetListFiltros();
        public abstract List<CustomAutocomplete> GetListAgrupadoPor();
        public abstract List<CustomAutocomplete> GetListValoresFiltros(int idTablaFiltrar);

        public abstract DataTable ReportePortafolioCampos(FiltrosReporteCamposPortafolio filtros);
        public abstract List<ReporteCamposPortafolio> ReportePortafolioCampos_Data(FiltrosReporteCamposPortafolio filtros);

        #endregion

        #region Variacion
        public abstract ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioVariacionDTO GetReportePortafolioVariacion_Estados(FiltrosReporteVariacionPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioVariacionDTO GetReportePortafolioVariacion_DistrXGerencia(FiltrosReporteVariacionPortafolio filtros, bool getData = false, int idGerencia = 0);

        public abstract DateTime? ObtenerFechaBaseReporte(DateTime fechaConsulta, int nroPeriodos, int frecuencia);
        #endregion

        #region Reporte Pedidos
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionAcumulada(FiltrosReportePedidosPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionPeriodo(FiltrosReportePedidosPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_ConsultasPortafolio(FiltrosReportePedidosPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_CamposMasRequeridos(FiltrosReportePedidosPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_SLA(FiltrosReportePedidosPortafolio filtros, bool getData = false);
        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_RegistroAPP(FiltrosReportePedidosPortafolio filtros, bool getData = false);

        public abstract ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_Consultas(FiltrosReportePedidosPortafolio filtros, bool getData = false);


        #endregion
    }
}
