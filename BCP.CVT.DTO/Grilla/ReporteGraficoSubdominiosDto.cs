using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class ReporteGraficoSubdominiosDto
    {
        public int Total { get; set; }
        public int TotalRojo { get; set; }
        public int TotalVerde { get; set; }
        public int TotalAmbar { get; set; }
    }

    public class ReporteDetalladoSubdominioDto
    {
        public int EquipoId { get; set; }
        public int TecnologiaId { get; set; }
        public int TotalFilas { get; set; }
        public string TipoEquipo { get; set; }
        public string Nombre { get; set; }
        public string Subdominio { get; set; }
        public string Dominio { get; set; }
        public string ClaveTecnologia { get; set; }
        public DateTime? FechaCalculoBase { get; set; }
        public string FechaCalculoBaseToString
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                        return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("dd/MM/yyyy") : "-";
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                {
                    if (TipoEquipo=="Appliance")
                    {
                        return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("dd/MM/yyyy") : "-";
                    }
                    else
                    {
                        return string.Empty;
                    }
                        
                }

                    
            }
        }
        public int Obsoleto { get; set; }
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }
        public int IndicadorObsolescencia
        {
            get
            {
                //if (ForzarObsolescencia)
                //    return -1;

                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1);
                            if (this.FechaCalculoBase.HasValue)
                                indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                            else
                                indicador = -1;
                        }

                        return indicador;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (TipoEquipo == "Appliance")
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1);
                            if (this.FechaCalculoBase.HasValue)
                                indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                            else
                                indicador = -1;
                        }

                        return indicador;
                    }
                    else
                    {
                        return -1;
                    }
                }
                    
            }
        }
        public int IndicadorObsolescencia_Proyeccion1
        {
            get
            {
                //if (ForzarObsolescencia)
                //    return -1;

                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1 + this.IndicadorMeses1);
                            if (this.FechaCalculoBase.HasValue)
                            {
                                if (IndicadorObsolescencia == 0 || IndicadorObsolescencia == -1)
                                    indicador = -1;
                                else
                                    indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                            }
                            else
                                indicador = -1;
                        }

                        return indicador;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (TipoEquipo == "Appliance")
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1 + this.IndicadorMeses1);
                            if (IndicadorObsolescencia == 0 || IndicadorObsolescencia == -1)
                                indicador = -1;
                            else
                                indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                        }

                        return indicador;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        public int IndicadorObsolescencia_Proyeccion2
        {
            get
            {
                //if (ForzarObsolescencia)
                //    return -1;

                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1 + this.IndicadorMeses2);
                            if (this.FechaCalculoBase.HasValue)
                            {
                                if (IndicadorObsolescencia_Proyeccion1 == 0 || IndicadorObsolescencia_Proyeccion1 == -1)
                                    indicador = -1;
                                else
                                    indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                            }
                            else
                                indicador = -1;
                        }

                        return indicador;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (TipoEquipo == "Appliance")
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.IndicadorMeses1 + this.IndicadorMeses2);
                            if (IndicadorObsolescencia_Proyeccion1 == 0 || IndicadorObsolescencia_Proyeccion1 == -1)
                                indicador = -1;
                            else
                                indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                        }

                        return indicador;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        public bool? FlagFechaFinSoporte { get; set; }
        public string TipoTecnologia { get; set; }
        public bool ForzarObsolescencia
        {
            get
            {
                return TipoTecnologiaId == (int)ETecnologiaTipo.NoEstandar;
            }
        }
        public int TipoTecnologiaId { get; set; }
        public string Appliance { get; set; }
        public int? EstadoId { get; set; }
        public string EstadoIdTecnologiaStr => EstadoId.HasValue ? EstadoId.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId.Value)) : string.Empty : string.Empty;
    }

    public class ReporteIndicadorObsolescenciaSoBd
    {
        public int Total { get; set; }
        public string ClaveTecnologia { get; set; }
    }
}
