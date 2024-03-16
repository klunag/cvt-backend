using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ReporteHardwareKpiDTO : BaseDTO
    {

        public int Nivel { get; set; }
        public string UnidadFondeoParent { get; set; }
        public string UnidadFondeoId { get; set; }
        public string UnidadFondeo { get; set; }
        public string SegundoNivel { get; set; }
        public string Unidad { get; set; }
        public string CodigoAPT { get; set; }
        public string AplicacionStr { get; set; }
        public int ObsoletoKPI { get; set; }
        public decimal PorcentajeObsoletoKPI { get; set; }
        public int VigenteKPI { get; set; }
        public decimal PorcentajeVigenteKPI { get; set; }
        public int TotalKPI { get; set; }
        public decimal PorcentajeTotalKPI { get; set; }
        public int Vence12KPI { get; set; }
        public decimal PorcentajeVence12KPI { get; set; }
        public int Vence24KPI { get; set; }
        public decimal PorcentajeVence24KPI { get; set; }
        public int Vence24KPICorto { get; set; }
        public decimal PorcentajeVence24KPICorto { get; set; }
        public decimal PorcentajeKPIFlooking { get; set; }
        public decimal PorcentajeKPI { get; set; }
        public string GestionadoPor { get; set; }
        public string TeamSquad { get; set; }
        public string Modelo { get; set; }
    }

}
