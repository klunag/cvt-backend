using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class ReporteDetalladoTecnologiaDto
    {
        public int TotalFilas { get; set; }
        public int TecnologiaId { get; set; }
        public int? EstadoTecnologia { get; set; }
        public string EstadoTecnologiaToString => EstadoTecnologia.HasValue ? (EstadoTecnologia.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((EstadoTecnologia)EstadoTecnologia)) : "-";
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteToString
        {
            get
            {
                return FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "-";
            }
        }
        public DateTime? FechaExtendida { get; set; }
        public string FechaExtendidaToString
        {
            get
            {
                return FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : "-";
            }
        }
        public DateTime? FechaAcordada { get; set; }
        public string FechaAcordadaToString
        {
            get
            {
                return FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : "-";
            }
        }
        public bool? FlagFechaFinSoporte { get; set; }
        public string FlagFechaFinSoporteToString
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        return FlagFechaFinSoporte.HasValue ? (FlagFechaFinSoporte.Value ? "Sí" : "No") : "No";
                    }
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                    return string.Empty;
                
            }
        }
        public int? FuenteId { get; set; }
        public string FuenteIdToString => FuenteId.HasValue ? (FuenteId.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((Fuente)FuenteId)) : "-";
        public int? FechaCalculoTec { get; set; }
        public string FechaCalculoTecToString => FechaCalculoTec.HasValue ? (FechaCalculoTec.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)FechaCalculoTec)) : "-";
        public int? EstadoId { get; set; }
        public string EstadoIdToString => EstadoId.HasValue ? (EstadoId.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoId)) : "-";
        public string ClaveTecnologia { get; set; }
        public string TipoTecnologia { get; set; }
        public string Subdominio { get; set; }
        public string Dominio { get; set; }
        public string Familia { get; set; }
        public string FechaFinConfigurada
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        if (FechaCalculoTec.HasValue)
                        {
                            switch (FechaCalculoTec.Value)
                            {
                                case (int)FechaCalculoTecnologia.FechaExtendida:
                                    return FechaExtendidaToString;
                                case (int)FechaCalculoTecnologia.FechaFinSoporte:
                                    return FechaFinSoporteToString;
                                case (int)FechaCalculoTecnologia.FechaInterna:
                                    return FechaAcordadaToString;
                                default:
                                    return "-";
                            }
                        }
                        else
                        {
                            return "-";
                        }
                    }
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                    return "-";
            }
        }
        public int MesesIndicador1 { get; set; }
        public int MesesIndicador2 { get; set; }
        public int EliminacionTecObsoleta { get; set; }
        public string Roadmap { get; set; }

        public int EstadoActual { get; set; }
        public int EstadoIndicador1 { get; set; }
        public int EstadoIndicador2 { get; set; }

        public string Dueno { get; set; }
        public string DuenoStr => !string.IsNullOrEmpty(Dueno) ? Dueno : "-";
        public int TipoTecnologiaId { get; set; }
    }
}
