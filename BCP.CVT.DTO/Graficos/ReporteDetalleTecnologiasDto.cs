using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Graficos
{
    public class ReporteDetalleTecnologiasDto
    {
        public int TotalFilas { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string ClaveTecnologia { get; set; }
        public string TipoTecnologia { get; set; }
        public int TotalTecnologias { get; set; }
        public int TotalObsoletos { get; set; }
        public int TotalPorVencer { get; set; }
        public int TotalVigente { get; set; }
        public int TotalIndefinida { get; set; }
        public int TotalObsoletosRoadmap { get; set; }
        public int TotalPorVencerRoadmap { get; set; }
        public int TotalVigenteRoadmap { get; set; }
        public int TotalIndefinidaRoadmap { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }


        public int TecnologiaId { get; set; }
        public string MesProyeccion { get; set; }
        public int FechaCalculoTec { get; set; }
    }

    public class ReporteDetalleTecnologiasAplicacionDto
    {
        public int TotalFilas { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public int TotalAplicacion { get; set; }
        public string SubdominioToString { get; set; }
        public string Owner { get; set; }
        public DateTime FechaFiltro { get; set; }
        public string CodigoAPT { get; set; }
        public string ClaveTecnologia { get; set; }
    }

    public class ReporteDetalleTecnologiasEquipoDto
    {
        public int TotalFilas { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public int TecnologiaId { get; set; }
        public string ClaveTecnologia { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public int TotalInstalaciones { get; set; }
        public int TotalServidores { get; set; }
        public int TotalServidoresAgencia { get; set; }
        public int TotalPCs { get; set; }
        public int TotalServicioNube { get; set; }
        public int TotalStorage { get; set; }
        public int TotalAppliance { get; set; }

        public DateTime FechaFiltro { get; set; }
        public string SubdominioToString { get; set; }
        public string Owner { get; set; }
        public string Equipo { get; set; }
        public string TipoEquipo { get; set; }

        public string TipoTecnologia { get; set; }

    }

    public class ReporteTecnologiaIntalacionDTO
    {
        public int TipoEquipoId { get; set; }
        public string TipoEquipoStr { get; set; }
        public int TipoTecnologiaId { get; set; }
        public string TipoTecnologiaStr { get; set; }

        public int Equipos { get; set; }
        public int Tecnologias { get; set; }
        public int Familias { get; set; }

        public int Estandar { get; set; }
        public int EstandarRestringido { get; set; }
        public int PorRegularizar { get; set; }
        public int NoEstandar { get; set; }
        public int EnEvaluacion { get; set; }
        public int Excepcion { get; set; }

        public int TecnologiaId { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Versiones { get; set; }
        public string SistemaOperativo => string.Format("{0} {1} {2}", Fabricante, Nombre, Versiones);

        public int Total { get; set; }
    }

    public class ReporteDetalleTecnologiasFechasDto
    {
        public int TotalFilas { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string ClaveTecnologia { get; set; }
        public string TipoTecnologia { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public int? FechaCalculoTec { get; set; }
        public int? FuenteId { get; set; }
        public bool? FlagSiteEstandar { get; set; }
        public bool FlagFechaFinSoporte { get; set; }
        public int TotalComponentes { get; set; }

        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public DateTime? FechaAcordada { get; set; }

        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";
        public string FechaExtendidaStr => FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";
        public string FechaAcordadaStr => FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";

        public string FlagSiteEstandarStr => FlagSiteEstandar.HasValue ? (FlagSiteEstandar.Value ? "Si" : "No") : "-";
        public string FechaCalculoTecStr => FechaCalculoTec.HasValue ? (FechaCalculoTec.Value == 0 ? "Indefinida" : Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)(FechaCalculoTec.Value))) : "-";
        public string FuenteIdStr => FuenteId.HasValue ? (FuenteId.Value == 0 ? "Indefinida" : Utilitarios.GetEnumDescription2((Fuente)(FuenteId.Value))) : "-";
        public string FlagFechaFinSoporteStr => FlagFechaFinSoporte ? "Con fecha fin soporte" : "-";
        public string TotalComponentesStr => TotalComponentes == 0 ? "-" : TotalComponentes.ToString();
    }

}
