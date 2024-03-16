using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class SolicitudTecnologiaCamposDTO
    {
        public int SolicitudTecnologiaId { get; set; }
        public int ConfiguracionTecnologiaCamposId { get; set; }
        public string NombreCampo { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public int EstadoCampo { get; set; }
        public string EstadoCampoStr
        {
            get
            {
                return Utilitarios.GetEnumDescription2((FlujoEstadoSolicitud)EstadoCampo).ToUpper();
            }
        }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionStr => FechaCreacion.HasValue ? FechaCreacion.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string RolAprueba { get; set; }
        public string ValorCampoPropuesto { get; set; }
        public int CorrelativoCampo { get; set; }
    }
    public class ConfiguracionTecnologiaCamposDTO
    {
        public int Id { get; set; }
        public string NombreCampo { get; set; }
        public string TablaProcedencia { get; set; }
        public string DescripcionCampo { get; set; }
        public int FlagActivo { get; set; }
        public string RolAprueba { get; set; }
        public int ConfiguracionTecnologiaCamposId { get; set; }
        public int CorrelativoCampo { get; set; }
    }
    public class FlujoActualizacionTecnologiaCamposDTO
    {
        public int SubdominioId { get; set; }
        public int DominioId { get; set; }
        public int? TipoTecnologiaId { get; set; }
        public string TipoTecnologiaStr { get; set; }
        public int? FamiliaId { get; set; }
        public int TecnologiaPorValidarId { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int EstadoTecnologia { get; set; }
        public string Versiones { get; set; }
        public bool? FlagFechaFinSoporte { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaAcordada { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public string ComentariosFechaFin { get; set; }
        public string SustentoMotivo { get; set; }
        public string SustentoUrl { get; set; }
        public int? Fuente { get; set; }
        public string FuenteStr
        {
            get
            {
                if (!Fuente.HasValue) return null;
                if (Fuente.Value <= 0) return null;
                return Utilitarios.GetEnumDescription2((Cross.Fuente)Fuente.Value);
            }
        }
        public int? Existencia { get; set; }
        public int? Facilidad { get; set; }
        public int? Riesgo { get; set; }
        public decimal? Vulnerabilidad { get; set; }
        public string CasoUso { get; set; }
        public System.Web.HttpPostedFileBase CasoUsoArchivoFile { get; set; }
        public string Requisitos { get; set; }
        public string Compatibilidad { get; set; }
        public string Aplica { get; set; }
        public bool? FlagAplicacion { get; set; }
        public string CodigoAPT { get; set; }
        public int? EliminacionTecObsoleta { get; set; }
        public string RoadmapOpcional { get; set; }
        public string Referencias { get; set; }
        public string PlanTransConocimiento { get; set; }
        public string EsqMonitoreo { get; set; }
        public string LineaBaseSeg { get; set; }
        public string EsqPatchManagement { get; set; }
        public string Dueno { get; set; }
        public string EqAdmContacto { get; set; }
        public string GrupoSoporteRemedy { get; set; }
        public string ConfArqSeg { get; set; }
        public string ConfArqTec { get; set; }
        public string EncargRenContractual { get; set; }
        public string EsqLicenciamiento { get; set; }
        public int? EsqLicenciamientoId { get; set; }
        public string SoporteEmpresarial { get; set; }
        public List<TecnologiaAplicacionDTO> ListAplicaciones { get; set; }
        public List<AutorizadorDTO> ListAutorizadores { get; set; }
        public List<ArquetipoDTO> ListArquetipo { get; set; }
        public List<TecnologiaDTO> ListTecnologiaVinculadas { get; set; }
        public List<TecnologiaEquivalenciaDTO> ListEquivalencias { get; set; }
        public string Observacion { get; set; }
        public bool? FlagSiteEstandar { get; set; }
        public int? FechaCalculoTec { get; set; }
        public string FechaCalculoTecStr
        {
            get
            {
                if (!FechaCalculoTec.HasValue) return null;
                if (FechaCalculoTec.Value <= 0) return null;
                return Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)FechaCalculoTec.Value);
            }
        }

        public string SubdominioNomb { get; set; }
        public string DominioNomb { get; set; }
        public string EliminacionTecNomb { get; set; }
        public string AplicacionNomb { get; set; }
        public string TipoTecNomb { get; set; }
        public string FamiliaNomb { get; set; }

        public string Fabricante { get; set; }
        public int? EstadoId { get; set; }
        public DateTime? FechaDeprecada { get; set; }
        public int? TecReemplazoDepId { get; set; }

        public bool? FlagUnicaVigente { get; set; }
        public int? ArquetipoId { get; set; }
        public string ArquetipoNombre { get; set; }
        public string ClaveTecnologia { get; set; }
        public string CodigoTecnologiaAsignado { get; set; }
        public int? UrlConfluenceId { get; set; }
        public string UrlConfluence { get; set; }

        public List<int> ItemsRemoveAutId { get; set; }
        public List<string> ItemsAddAutorizador { get; set; }
        public List<int> ItemsRemoveTecEqId { get; set; }
        public List<int> ItemsAddTecEqId { get; set; }
        public List<int> ItemsRemoveTecVinculadaId { get; set; }
        public List<int> ItemsAddTecVinculadaId { get; set; }
        public List<int> ItemsRemoveAppId { get; set; }
        public List<int> ItemsRemoveArqId { get; set; }
        public List<int> ItemsRemoveEqTecId { get; set; }

        //Listas para el SP
        public string ItemsRemoveAutIdSTR { get; set; }
        public string ItemsAddAutorizadorSTR { get; set; }
        public string ItemsRemoveTecEqIdSTR { get; set; }
        public string ItemsAddTecEqIdSTR { get; set; }
        public string ItemsRemoveTecVinculadaIdSTR { get; set; }
        public string ItemsAddTecVinculadaIdSTR { get; set; }

        //public string TipoStr { get { return TipoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaTipo)(TipoId)) : ""; } }
        public string EstadoStr { get { return EstadoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId)) : ""; } }
        public string EstadoTecnologiaStr { get { return EstadoTecnologia > 0 ? Utilitarios.GetEnumDescription2((EstadoTecnologia)(EstadoTecnologia)) : ""; } }

        public int? ArchivoId { get; set; }
        public string ArchivoStr { get; set; }

        public DateTime? FechaCalculoBase { get; set; }
        public string FechaCalculoBaseStr
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                        return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("dd/MM/yyyy") : string.Empty;
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                    return string.Empty;
            }
        }

        public bool Obsoleto { get; set; }
        public string ObsoletoStr
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                        return Obsoleto ? "Si" : "No";
                    else
                        return "No";
                }
                else
                    return Obsoleto ? "Si" : "No";
            }
        }

        public bool FlagTieneEquivalencias { get; set; }

        // TecnologiaEstandar
        public string TecnologiaVigenteStr { get; set; }
        public string TecnologiaDeprecadoStr { get; set; }
        public string TecnologiaObsoletoStr { get; set; }

        public string FlagFechaFinSoporteStr
        {
            get
            {
                return (FlagFechaFinSoporte.HasValue ? (FlagFechaFinSoporte.Value ? "Tiene fecha fin soporte" : "No tiene fecha fin soporte") : string.Empty);
            }
        }

        public string FechaLanzamientoToString
        {
            get
            {
                return FechaLanzamiento.HasValue ? FechaLanzamiento.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaFinSoporteToString
        {
            get
            {
                return FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaAcordadaToString
        {
            get
            {
                return FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaExtendidaToString
        {
            get
            {
                return FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public long RelacionId { get; set; }


        public decimal? RiesgoTecnologia { get; set; }

        public decimal? ObsolescenciaTecnologia { get; set; }
        public decimal? ObsolescenciaTecnologiaProyectado1 { get; set; }
        public decimal? ObsolescenciaTecnologiaProyectado2 { get; set; }

        public bool FlagConfirmarFamilia { get; set; }
        public bool FlagCambioEstado { get; set; }

        //Para estado de las tecnologías   
        public int Meses { get; set; }
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }
        public int MesesObsolescencia { get; set; }

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
                        if (this.Obsoleto)
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
            }
        }
        //public int IndicadorMeses1 { get; set; }
        //public int IndicadorMeses2 { get; set; }

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
                        if (this.Obsoleto)
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
                        if (this.Obsoleto)
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

        public bool ForzarObsolescencia
        {
            get
            {
                if (TipoTecnologiaId.HasValue)
                    return TipoTecnologiaId.Value == (int)ETecnologiaTipo.NoEstandar;
                else
                    return false;
            }
        }

        public int? AutomatizacionImplementadaId { get; set; }
        public string AutomatizacionImplementadaStr
        {
            get
            {
                if (!AutomatizacionImplementadaId.HasValue) return null;
                if (AutomatizacionImplementadaId.Value <= 0) return null;
                return Utilitarios.GetEnumDescription2((EAutomatizacionImplementada)AutomatizacionImplementadaId.Value);
            }
        }

        public int? TipoCicloVidaId { get; set; }
        public string TipoCicloVidasTR { get; set; }
        public int? TipoProductoId { get; set; }
        public string TipoProductoStr { get; set; }

        public int? AplicacionId { get; set; }
        public AplicacionDTO Aplicacion { get; set; }

        public int? ProductoId { get; set; }
        public ProductoDTO Producto { get; set; }
        public bool? MostrarSiteEstandaresId { get; set; }
        public int? MotivoId { get; set; }
        public string CodigoProducto { get; set; }
        public string Codigo { get; set; }
        public int? RevisionSeguridadId { get; set; }
        public string RevisionSeguridadDescripcion { get; set; }
        public byte[] CasoUsoArchivo { get; set; }
        public string CompatibilidadSOId { get; set; }
        public string CompatibilidadCloudId { get; set; }
        public string EquipoAprovisionamiento { get; set; }
        public string TipoFechaInterna { get; set; }
        public string FechaFinSoporteSite
        {
            get
            {
                if (FechaCalculoTec != null)
                {
                    if (FechaCalculoTec.HasValue)
                    {
                        switch (FechaCalculoTec.Value)
                        {
                            case (int)FechaCalculoTecnologia.FechaInterna:
                                return FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : string.Empty;
                            case (int)FechaCalculoTecnologia.FechaExtendida:
                                return FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : string.Empty;
                            case (int)FechaCalculoTecnologia.FechaFinSoporte:
                                return FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
                            default: return string.Empty;
                        }
                    }
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
        }

        public int CantidadVulnerabilidades { get; set; }
        public bool EsAplicacion { get; set; }
        public string TribuCoeId { get; set; }
        public string TribuCoeDisplayName { get; set; }
        public string TribuCoeMatricula { get; set; }
        public string TribuCoeResponsable { get; set; }
        public string SquadId { get; set; }
        public string SquadDisplayName { get; set; }
        public string ConfArqSegId { get; set; }
        public string ConfArqSegDisplayName { get; set; }
        public string ConfArqTecId { get; set; }
        public string ConfArqTecDisplayName { get; set; }
        public string UsuarioMatricula { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioMail { get; set; }
        public int? MetodoAutenticacion { get; set; }
        public int? MetodoAutorizacion { get; set; }
        public string MenorRTO { get; set; }
        public string MayorRTO { get; set; }
        public string TierPreProduccion { get; set; }
        public string TierProduccion { get; set; }
        public string ModeloEntrega { get; set; }
        public int? TipoActivoId { get; set; }

        public string TipoActivoStr
        {
            get
            {
                string texto = (TipoActivoId.HasValue) ? Utilitarios.GetEnumDescription2((TipoActivoProducto)TipoActivoId) : string.Empty;
                return texto;
            }
        }
        public string OwnerId { get; set; }
        public string OwnerMatricula { get; set; }
        public string OwnerNombre { get; set; }
        public string LiderUsuarioAutorizador { get; set; }
        public string CodigoInterfaz { get; set; }
        public int? CriticidadBIA { get; set; }
        public int? CriticidadFinal { get; set; }
        public int? ClasificacionActivo { get; set; }

        // Datos Solicitud
        public int SolicitudId { get; set; }
        public int TipoSolicitudId { get; set; }
        public string NombreCreador { get; set; }
        public string MatriculaCreador { get; set; }
        public int TecnologiaId { get; set; }
        public string UsuarioModificacion { get; set; }

        public bool UsoProducto { get; set; }
        public bool ActivoTecnologia { get; set; }

    }
}
