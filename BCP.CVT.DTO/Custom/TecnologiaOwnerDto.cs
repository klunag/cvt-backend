using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class TecnologiaOwnerDto
    {
        public string Codigo { get; set; }
        public int EstadoTecnologia { get; set; }
        public string EstadoTecnologiaStr
        {
            get
            {
                return EstadoTecnologia == 0 ? null : Utilitarios.GetEnumDescription2((BCP.CVT.Cross.EstadoTecnologia)EstadoTecnologia);
            }
        }
        public string CodigoAPT { get; set; }
        public string EstadoAplicacion { get; set; }
        public string GestionadoPor { get; set; }
        public string NombreEquipo_Squad { get; set; }
        public string Criticidad { get; set; }

        public string NombresDominio { get; set; }
        public string MatriculaDominio { get; set; }
        public string MatriculaSubdominio { get; set; }
        public string NombresSubdominio { get; set; }
        public string MatriculaTecnologia { get; set; }
        public string NombresTecnologia { get; set; }
        public int TecnologiaId { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string ClaveTecnologia { get; set; }
        public string Roadmap { get; set; }
        public int? FechaCalculoTec { get; set; }
        public int? FuenteId { get; set; }
        public bool? FlagFechaFinSoporte { get; set; }
        public DateTime? FechaCalculoBase { get; set; }
        public int Obsoleto { get; set; }
        public decimal? IndiceObsolescencia { get; set; }
        public decimal? Riesgo { get; set; }
        public int TotalEquipos { get; set; }
        public int TotalAplicaciones { get; set; }
        public int TotalInstanciasServidores { get; set; }
        public int TotalInstanciasServicioNube { get; set; }
        public int TotalInstanciasPcs { get; set; }
        public int TotalFilas { get; set; }
        public string Componente { get; set; }
        public string NombreComponente { get; set; }
        public string TipoTecnologia { get; set; }
        public int Meses { get; set; }
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }

        public DateTime? FechaMaxFamilia { get; set; }
        public string Tecnologia { get; set; }
        public int TotalIndefinidas { get; set; }

        public int IndicadorProyOwnerHoy { get; set; }
        public int IndicadorProyOwner1 { get; set; }
        public int IndicadorProyOwner2 { get; set; }
        public int IndicadorProyOwner3 { get; set; }

        public bool? EsIndefinida { get; set; }

        public int ProductoId { get; set; }
        public string ProductoStr { get; set; }
        public int? TribuCoeId { get; set; }
        public string TribuCoeDisplayName { get; set; }
        public string TribuCoeDisplayNameResponsable { get; set; }
        public bool FlagTribuCoe { get { return TribuCoeId.HasValue; } }
        public string FlagTribuCoeStr { get { return FlagTribuCoe ? "Sí" : "No"; } }
        public int SquadId { get; set; }
        public string SquadDisplayName { get; set; }
        public string SquadDisplayNameResponsable { get; set; }
        public int EstadoId { get; set; }
        public string EstadoStr
        {
            get
            {
                return EstadoId == 0 ? null : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoId);
            }
        }
        //public int EstadoIdActual { get; set; }
        //public string EstadoStrActual
        //{
        //    get
        //    {
        //        return EstadoIdActual == 0 ? null : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoIdActual);
        //    }
        //}
        //public int EstadoIdDoceMeses { get; set; }
        //public string EstadoStrDoceMeses
        //{
        //    get
        //    {
        //        return EstadoIdDoceMeses == 0 ? null : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoIdDoceMeses);
        //    }
        //}
        //public int EstadoIdVeintiCuatroMeses { get; set; }
        //public string EstadoStrVeintiCuatroMeses
        //{
        //    get
        //    {
        //        return EstadoIdVeintiCuatroMeses == 0 ? null : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoIdVeintiCuatroMeses);
        //    }
        //}

        public string EsIndefinidaStr => EsIndefinida.HasValue ? (EsIndefinida.Value ? "Si" : "No") : "-"; 

        public string FechaMaxFamiliaStr => FechaMaxFamilia.HasValue ? FechaMaxFamilia.Value.ToString("yyyy-MM-dd") : "-";
        public string Familia => string.Format("{0} {1}", Fabricante, Nombre);

        public string FechaCalculoTecStr => FechaCalculoTec.HasValue ? Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)(FechaCalculoTec.Value)) : "-";
        public string FechaCalculoBaseStr => FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("yyyy-MM-dd") : "-";



        //Calculados
        public string FuenteToString { get { return FuenteId.HasValue ? Utilitarios.GetEnumDescription2((Fuente)(FuenteId.Value)) : ""; } }

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


        public DateTime? FechaAcordada { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public DateTime? FechaFinSoporte { get; set; }


        public string FechaFinSoporteToString => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";
        public string FechaAcordadaToString => FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";
        public string FechaExtendidaToString => FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";


        public string FechaCalculoValorTecStr
        {
            get
            {
                if (FlagFechaFinSoporte != true) return "Fecha indefinida";
                else return (FlagFechaFinSoporte ?? (bool?)false).Value == true ? FechaCalculoTec.HasValue ? FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaExtendida ? FechaExtendidaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaInterna ? FechaAcordadaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaFinSoporte ? FechaFinSoporteToString : "Sin fecha fin configurada" : "Sin fecha fin configurada" : "Sin fecha fin configurada";
            }
        }

        public int IndicadorObsolescencia
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.Meses);
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
            }
        }

        public int IndicadorObsolescencia_Proyeccion1
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.Meses + this.IndicadorMeses1);
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
                    return -1;
            }
        }

        public int IndicadorObsolescencia_Proyeccion2
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                    {
                        var indicador = 1;
                        if (this.Obsoleto == 1)
                            indicador = -1;
                        else
                        {
                            var fechaValidacion = DateTime.Now.AddMonths(this.Meses + this.IndicadorMeses2);
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
                    return 1;
                }
                else
                    return -1;
            }
        }
    }
}
