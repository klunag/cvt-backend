using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class ReporteGerenciaDivisionDto : BaseDTO
    {

        public ReporteGerenciaDivisionDto()
        {
            this.EquipoSinTecnologiasYServidores = false;
        }

        public int TotalFilas { get; set; }
        public long RelacionId { get; set; }
        public string CodigoApt { get; set; }
        public string Aplicacion { get; set; }
        public string GestionadoPor { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string TipoEquipo { get; set; }
        public int TipoId { get; set; }
        public int EstadoId { get; set; }
        public string DetalleAmbiente { get; set; }
        public int? TecnologiaId { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string ClaveTecnologia { get; set; }
        public string DetalleCriticidad { get; set; }
        public string RoadMap { get; set; }
        public string ExpertoEspecialista { get; set; }
        public string Owner_LiderUsuario_ProductOwner { get; set; }
        public string JefeEquipo_ExpertoAplicacion { get; set; }
        public string BrokerSistemas { get; set; }
        public string TribeTechnicalLead { get; set; }

        public string ListaPCI { get; set; }
        public string EstadoAplicacion { get; set; }
        public DateTime? FechaCalculoBase { get; set; }
        public int? Obsoleto { get; set; }
        public decimal? IndiceObsolescencia { get; set; }
        public decimal? RiesgoTecnologia { get; set; }
        public decimal? RiesgoAplicacion { get; set; }
        public decimal? Priorizacion { get; set; }
        public int Meses { get; set; }
        public int? TotalTecnologiasObsoletas { get; set; }
        public int? TotalServidores { get; set; }
        public int? TotalTecnologias { get; set; }
        public string TipoTecnologia { get; set; }
        public string FechaFinToString
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                        return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("yyyy-MM-dd") : string.Empty;
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                    return string.Empty;
            }
        }
        public string EstadoToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)(this.EstadoId));
            }
        }
        public string TipoToString
        {
            get
            {
                String resultadoTipo = string.Empty;
                if (TipoId != 0)
                {
                    if (TipoEquipo == TipoEquipoValidarCD)
                    {
                        resultadoTipo = TipoEquipoValidarCD;
                    }
                    else
                    {
                        resultadoTipo = Utilitarios.GetEnumDescription2((ETipoRelacion)(this.TipoId));
                    }
                }
                //return TipoId == 0 ? string.Empty : Utilitarios.GetEnumDescription2((ETipoRelacion)(this.TipoId));
                return resultadoTipo;
            }
        }
        public string Relevancia { get; set; }
        public bool FlagActivo { get; set; }
        public string FlagActivoToString
        {
            get
            {
                return FlagActivo ? "Activo" : "Inactivo";
            }
        }
        //Asociado a los indicadores
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }

        public int IndicadorObsolescencia
        {
            get
            {
                //if (ForzarObsolescencia)
                //    return -1;

                var indicador = 1;
                //if (this.TipoEquipo == "Api")
                //{
                //    if (this.Obsoleto == 1)
                //    {
                //        indicador = -1;
                //    }
                //    else
                //    {
                //        indicador = 1;
                //    }
                //    return indicador;
                //}
                //else
                //{
                    if (FlagFechaFinSoporte.HasValue)
                    {
                        if (FlagFechaFinSoporte.Value)
                        {
                            //var indicador = 1;
                            if (this.Obsoleto == 1)
                                indicador = -1;
                            else
                            {
                                var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia);
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
                        return -1;
                //}
                
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
                            var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia + this.IndicadorMeses1);
                            if (this.FechaCalculoBase.HasValue)
                            {
                                var indObs = IndicadorObsolescencia;
                                if (indObs == 0 || indObs == -1)
                                    indicador = -1;
                                else
                                {
                                    if (this.FechaCalculoBase.Value > fechaValidacion)
                                        indicador = 1;
                                    else
                                    {
                                        var fechaValidacionDos = DateTime.Now.AddMonths(this.IndicadorMeses1);
                                        indicador = this.FechaCalculoBase.Value > fechaValidacionDos ? 0 : -1;
                                    }
                                }
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
                    return -1;
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
                            var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia + this.IndicadorMeses2);
                            if (this.FechaCalculoBase.HasValue)
                            {
                                var indObs = IndicadorObsolescencia_Proyeccion1;
                                if (indObs == 0 || indObs == -1)
                                    indicador = -1;
                                else
                                {
                                    if (this.FechaCalculoBase.Value > fechaValidacion)
                                        indicador = 1;
                                    else
                                    {
                                        var fechaValidacionDos = DateTime.Now.AddMonths(this.IndicadorMeses2);
                                        indicador = this.FechaCalculoBase.Value > fechaValidacionDos ? 0 : -1;
                                    }
                                }
                            }
                            else
                                indicador = -1;
                        }

                        return indicador;
                    }
                    return 1;
                }
                else
                    return -1;
            }
        }

        public int ReporteIndicadorActual
        {
            get
            {
                if (EstadoAplicacion == "Eliminada" && IndiceObsolescencia == 0 && TotalTecnologias == 0 && TotalServidores == 0)
                {
                    return 1;
                }
                else
                {
                    if (IndiceObsolescencia > 0)
                        return -1;
                    else
                    {
                        if (this.DetalleActual != null)
                        {
                            if (DetalleActual.TotalTecnologias == DetalleActual.TotalVerdesActual)
                                return 1;
                            else if (DetalleActual.TotalTecnologias == DetalleActual.TotalAmarillosActual + DetalleActual.TotalVerdesActual)
                                return 0;
                            else
                                return -1;
                        }
                        else
                            return -1;
                    }
                }
            }
        }

        public int ReporteIndicadorProyeccion1
        {
            get
            {
                if (IndiceObsolescencia > 0)
                    return -1;
                else
                {
                    if (this.DetalleActual != null)
                    {
                        if (DetalleActual.TotalTecnologias == DetalleActual.TotalVerdesProyeccion1)
                            return 1;
                        else if (DetalleActual.TotalTecnologias == DetalleActual.TotalAmarillosProyeccion1 + DetalleActual.TotalVerdesProyeccion1)
                            return 0;
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
            }
        }

        public int ReporteIndicadorProyeccion2
        {
            get
            {
                if (IndiceObsolescencia > 0)
                    return -1;
                else
                {
                    if (this.DetalleActual != null)
                    {
                        if (DetalleActual.TotalTecnologias == DetalleActual.TotalVerdesProyeccion2)
                            return 1;
                        else if (DetalleActual.TotalTecnologias == DetalleActual.TotalAmarillosProyeccion2 + DetalleActual.TotalVerdesProyeccion2)
                            return 0;
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
            }
        }

        public string TipoActivoInformacion { get; set; }
        public string Gestor { get; set; }
        public bool? FlagFechaFinSoporte { get; set; }
        public AplicacionIndicador DetalleActual { get; set; }
        public int TipoTecnologiaId { get; set; }
        public bool ForzarObsolescencia
        {
            get
            {
                return TipoTecnologiaId == (int)ETecnologiaTipo.NoEstandar;
            }
        }
        public int FlagExterno { get; set; }
        public string Funcion { get; set; }
        public string Componente { get; set; }

        public int MesesObsolescencia { get; set; }
        public bool EquipoSinTecnologiasYServidores { get; set; }
        public int TotalVulnerabilidadEquipo { get; set; }
        public int TotalVulnerabilidadTecnologia { get; set; }
        public int TotalVulnerabilidadAplicacion { get; set; }
        public DateTime? FechaInicioCuarentena { get; set; }
        public string FechaInicioCuarentenaToString
        {
            get
            {
                return FechaInicioCuarentena.HasValue ? FechaInicioCuarentena.Value.ToString("dd/MM/yyyy mm:dd:ss tt") : string.Empty;
            }
        }
        public DateTime? FechaFinCuarentena { get; set; }
        public string FechaFinCuarentenaToString
        {
            get
            {
                return FechaFinCuarentena.HasValue ? FechaFinCuarentena.Value.ToString("dd/MM/yyyy mm:dd:ss tt") : string.Empty;
            }
        }
        public decimal? IndiceObsolescencia_FLooking { get; set; }
        public decimal? IndiceObsolescencia_FL_Proyeccion { get; set; }
        public string FactoresKPI_FLooking { get; set; }
        public string FactoresKPI_FL_Proyeccion { get; set; }
        public string TipoEquipoValidarCD { get; set; }
        public int TipoEquipoId { get; set; }
        public string TipoCicloVidaNombre { get; set; }
        public string UnidadFondeo { get; set; }
        public int? EstadoIdTecnologia { get; set; }
        public string EstadoIdTecnologiaStr => EstadoIdTecnologia.HasValue ? EstadoIdTecnologia.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoIdTecnologia.Value)) : string.Empty : string.Empty;
    }

    public class AplicacionIndicador
    {
        public string CodigoApt { get; set; }
        public int TotalTecnologias { get; set; }
        public int TotalVerdesActual { get; set; }
        public int TotalVerdesProyeccion1 { get; set; }
        public int TotalVerdesProyeccion2 { get; set; }
        public int TotalAmarillosActual { get; set; }
        public int TotalAmarillosProyeccion1 { get; set; }
        public int TotalAmarillosProyeccion2 { get; set; }
    }
}
