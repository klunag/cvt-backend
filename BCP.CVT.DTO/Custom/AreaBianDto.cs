using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class AreaBianDto
    {
        public string Categoria { get; set; }

        public int BiceMayoristaTesoreria { get; set; }
        public int BiceMinorista { get; set; }
        public int BiceMayorista { get; set; }
        public int Bice { get; set; }

        public int BusinessIntegration { get; set; }

        public int BusinessSupport { get; set; }

        public int BankingProducts { get; set; }

        public int BusinessDirection { get; set; }
        public int BusinessDirectionTesoreria { get; set; }

        //Autocalculado
        public int BiceFinal
        {
            get
            {
                return Bice + BiceMayoristaTesoreria + BiceMinorista + BiceMayorista;
            }
        }
        public int BusinessDirectionFinal
        {
            get
            {
                return BusinessDirection + BusinessDirectionTesoreria;
            }
        }
    }

    public class AplicacionReporteDto
    {
        public string Categoria { get; set; }
        public int TotalVigentes { get; set; }
        public int TotalEnDesarrollo { get; set; }
        public int AppsOnPremise { get; set; }
        public int AppsOnCloud { get; set; }
        public int AppsObsolescenciaOnPremise { get; set; }
        public int AppsObsolescenciaOnCloud { get; set; }
        public int AppsDesarrolladasOnPremise { get; set; }
        public int AppsDesarrolladasOnCloud { get; set; }
        public int AppsPaquetesOnPremise { get; set; }
        public int AppsPaquetesOnCloud { get; set; }
        public int SumaTotalVigentes { get; set; }
        public int SumaTotalEnDesarrollo { get; set; }
        public int SumaAppsOnPremise { get; set; }
        public int SumaAppsOnCloud { get; set; }
        public int SumaAppsDesarrolladasOnPremise { get; set; }
        public int SumaAppsDesarrolladasOnCloud { get; set; }
        public int SumaAppsPaquetesOnPremise { get; set; }
        public int SumaAppsPaquetesOnCloud { get; set; }
        public int SumaAppsObsolescenciaOnPremise { get; set; }
        public int SumaAppsObsolescenciaOnCloud { get; set; }
        public string CapaFuncional { get; set; }
    }

    public class ReporteAplicacionDto
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string EstadoAplicacion { get; set; }
        public string GestionadoPor { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string GerenciaCentral { get; set; }
        public string AreaBIAN { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string CapaFuncional { get; set; }
        public int Total { get; set; }
    }

    public class ReporteAlertasDto
    {
        public int RelacionId { get; set; }
        public string CodigoAPT { get; set; }
        public string Software { get; set; }
        public string Servidor { get; set; }
        public DateTime? FechaEjecucion { get; set; }

        public string Componente
        {
            get
            {
                return !string.IsNullOrEmpty(Servidor) ? Servidor : Software;
            }
        }

        public string DetalleComponente
        {
            get
            {
                if (!string.IsNullOrEmpty(Servidor))
                    return "El equipo no existe o está desactivado";
                else
                    return "La tecnología en el portafolio no está registrada como una equivalencia en CVT";
            }
        }

        public int Total { get; set; }

        public string FechaEjecucionToString
        {
            get
            {
                return FechaEjecucion.HasValue ? FechaEjecucion.Value.ToString("dd/MM/yy hh:mm:ss tt") : string.Empty;
            }
        }
    }

    public class ReporteEstadoPortafolioDTO : BootstrapTable<ReporteEstadoPortafolio>
    {
        public List<ReportePortafolioGrafico> AplicacionesBancarias { get; set; }
        public List<ReportePortafolioGrafico> AplicacionesXJerarquia { get; set; }
        public List<ReportePortafolioGrafico> AplicacionesXEstado { get; set; }
        public List<ReportePortafolioGrafico> AplicacionesXCriticidad { get; set; }
        public List<ReportePortafolioGrafico> AplicacionesXCategoriaTecnologica { get; set; }
        public List<ReportePortafolioGrafico> SaludAplicacionesXUnidad { get; set; }
        public BootstrapTable<ReporteEstadoPortafolioRoles> ListadoRolesAplicaciones { get; set; }
    }

    public class ReportePortafolioGrafico
    {
        public int Cantidad { get; set; }
        public int GrupoId { get; set; }
        public string Grupo { get; set; }
        public string FechaStr { get; set; }
        public DateTime? Fecha { get; set; }

        public string FechaDescripcion { get; set; }

        //TODO: todos estos campos "valores" hay ue quitarlos
        public decimal Valor { get; set; }
        public decimal Valor2 { get; set; }
        public decimal Valor3 { get; set; }
        public decimal Valor4 { get; set; }
        public decimal Valor5 { get; set; }
    }
    public class ReporteCambioBajaResponsablesGDHDTO : BootstrapTable<ReporteCambioBajaResponsablesGDH>
    {
    }
    public class ReporteCambioBajaResponsablesGDH
    {
        public string ApplicationId { get; set; }
        public string Unidad { get; set; }
        public string UnidadAnterior { get; set; }
        public string Puesto { get; set; }
        public string PuestoAnterior { get; set; }
        public string Matricula { get; set; }
        public string NombreResponsable { get; set; }
        public string NombreRol { get; set; }
        public string TipoCambio { get; set; }
        public string FechaReporte { get; set; }

    }

    public class ReporteEstadoPortafolio
    {
        public string ApplicationId { get; set; }
        public string NombreAplicacion { get; set; }
        public string TipoActivo { get; set; }
        public int Estado { get; set; }
        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public int CriticidadBIA { get; set; }
        public string Categoria { get; set; }
        public string NivelCumplimiento { get; set; }

        public int GerenciaId { get; set; }
        public int DivisionId { get; set; }
        public int AreaId { get; set; }
        public int UnidadId { get; set; }

        public string EstadoToStr
        {
            get
            {
                switch (this.Estado)
                {
                    case (int)PAPP.Common.Cross.ApplicationState.Eliminada: return "Eliminada";
                    case (int)PAPP.Common.Cross.ApplicationState.EnDesarrollo: return "En Desarrollo";
                    case (int)PAPP.Common.Cross.ApplicationState.NoVigente: return "No Vigente";
                    case (int)PAPP.Common.Cross.ApplicationState.Vigente: return "Vigente";
                    default: return "";
                }
            }
        }

        public string CriticidadBIAToStr
        {
            get
            {
                switch (this.CriticidadBIA)
                {
                    case (int)PAPP.Common.Cross.CriticidadFinal.Baja: return "Baja";
                    case (int)PAPP.Common.Cross.CriticidadFinal.Alta: return "Alta";
                    case (int)PAPP.Common.Cross.CriticidadFinal.Media: return "Media";
                    case (int)PAPP.Common.Cross.CriticidadFinal.MuyAlta: return "Muy Alta";
                    default: return "";
                }
            }
        }
        public int cantRegistros { get; set; }
    }
    public class ReporteEstadoPortafolioRoles
    {
        public string Matricula { get; set; }
        public string ManagerName { get; set; }
        public int Lider { get; set; }
        public int Gestor { get; set; }
        public int Experto { get; set; }
        public int TL { get; set; }
        public int TTL { get; set; }
        public int JdE { get; set; }
        public int Broker { get; set; }
        public int Total { get; set; }

    }

    public class ReportePortafolioVariacionDTO
    {
        public List<ReportePortafolioGrafico> Solicitudes { get; set; }
        public List<ReporteVariacionPortafolioSolicitudes> SolicitudesData { get; set; }
    }
    public class ReporteVariacionPortafolioSolicitudes
    {
        public string Tipo { get; set; }
        public string FrecuenciaNombre { get; set; }
        public int FrecuenciaDias { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string ApplicationId { get; set; }
        public int EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public string CriticidadNombre { get; set; }
        public string Gerencia { get; set; }

        public string FechaDesdeStr => FechaDesde.HasValue ? FechaDesde.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaHastaStr => FechaHasta.HasValue ? FechaHasta.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string FechaDescripcion { get; set; }

        public string AplicacionNombre { get; set; }
        public string TipoEquipo { get; set; }
        public string GestionadoPor { get; set; }
        public DateTime? FechaMostrar { get; set; }
        public string FechaMostrarStr
        {
            get
            {
                return this.FechaMostrar.HasValue ? this.FechaMostrar.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public DateTime? FreezeDate { get; set; }

    }

    public class ReportePortafolioDTO<T>
    {
        public List<T> ChartReportData { get; set; }
        public List<ReportePortafolioGrafico> ChartReport { get; set; }

        public DataTable Data { get; set; }
    }

    public class ReportePedidosPortafolio
    {
        public string Usuario { get; set; }
        public int ColumnaId { get; set; }
        public string ColumnaNombre { get; set; }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string UrlSite { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Rol { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaRechazada { get; set; }
        public DateTime? FechaTransferencia { get; set; }
        public DateTime? FechaRegistroSituacion { get; set; }
        public int Dias { get; set; }
        public int DiasAprobado { get; set; }
        public string RolDesc { get; set; }
        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public string TipoActivo { get; set; }
        public string GestionadoPor { get; set; }
        public string Solicitante { get; set; }
        public string ValorAnterior { get; set; }
        public string NuevoValor { get; set; }
    }

    public class ReporteCamposPortafolio
    {
        public string Tipo { get; set; }
        public string FechaDescripcion { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int ColumnaAgruparId { get; set; }
        public string ColumnaAgruparValor { get; set; }
        public int ColumnaFiltroId { get; set; }
        public string ColumnaFiltroValor { get; set; }
        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }

        public DateTime?  FechaFreeze { get; set; }
    }
}
