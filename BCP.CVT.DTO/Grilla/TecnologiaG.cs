﻿using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class TecnologiaG : BaseDTO
    {
        public int TipoTecnologiaId { get; set; }
        public int SolicitudTecnologiaId { get; set; }
        public string Tipo { get; set; }
        public string Familia { get; set; }
        public string Subdominio { get; set; }
        public string Dominio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public string Versiones { get; set; }
        public string UsuarioAprobacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        //public int? TipoId { get; set; }
        public int? EstadoId { get; set; }
        public DateTime? FechaDeprecada { get; set; }
        public string FechaDeprecadaStr => FechaDeprecada.HasValue ? FechaDeprecada.Value.ToString("dd/MM/yyyy") : string.Empty;
        public int? TecReemplazoDepId { get; set; }
        public string TecReemplazoDepNombre { get; set; }
        public int? AplicacionId { get; set; }
        public AplicacionDTO Aplicacion { get; set; }
        public int? EstadoIdCalculado
        {
            get
            {
                int estadoId = (int)ETecnologiaEstado.Vigente;
                DateTime fechaActual = DateTime.Now.Date;
                bool flagFechaFinSoporte = FlagFechaFinSoporte ?? false;

                if (flagFechaFinSoporte)
                {
                    if (!FechaCalculoTec.HasValue) estadoId = (int)ETecnologiaEstado.Obsoleto;
                    else
                    {
                        FechaCalculoTecnologia fechaCalculoTecEnum = (FechaCalculoTecnologia)FechaCalculoTec.Value;
                        DateTime? fechaCalculada = fechaCalculoTecEnum == FechaCalculoTecnologia.FechaExtendida ? FechaExtendida : fechaCalculoTecEnum == FechaCalculoTecnologia.FechaInterna ? FechaAcordada : fechaCalculoTecEnum == FechaCalculoTecnologia.FechaFinSoporte ? FechaFinSoporte : null;

                        if (!fechaCalculada.HasValue) estadoId = (int)ETecnologiaEstado.Obsoleto;
                        else if (fechaCalculada.Value < fechaActual) estadoId = (int)ETecnologiaEstado.Obsoleto;
                    }
                }

                return estadoId;
                //if (!(FlagFechaFinSoporte ?? false)) return (int)ETecnologiaEstadoEstandar.Vigente;
                //if ((FlagFechaFinSoporte ?? false) && !FechaCalculoTec.HasValue) return (int)ETecnologiaEstado.Obsoleto;
                //DateTime fechaComparacion = DateTime.Now;
                //if (FechaCalculada < fechaComparacion)
                //    return (int)ETecnologiaEstadoEstandar.Obsoleto;
                //else
                //{
                //    fechaComparacion = fechaComparacion.AddMonths(CantidadMeses1);
                //    if (FechaCalculada < fechaComparacion)
                //        return (int)ETecnologiaEstadoEstandar.VigentePorVencer;
                //    else
                //        return (int)ETecnologiaEstadoEstandar.Vigente;
                //}
            }
        }
        public int CantidadMeses1 { get; set; }

        public string ClaveTecnologia { get; set; }
        public int? UrlConfluenceId { get; set; }
        public UrlConfluence UrlConfluenceIdEnum
        {
            get
            {
                if (UrlConfluenceId == null) return Cross.UrlConfluence.NoAplica;

                if (UrlConfluenceId == 0) return Cross.UrlConfluence.NoAplica;

                UrlConfluence urlConfluenceIdEnum = (UrlConfluence)UrlConfluenceId.Value;

                return urlConfluenceIdEnum;
            }
        }
        public string UrlConfluenceIdStr
        {
            get { return Utilitarios.GetEnumDescription2(UrlConfluenceIdEnum); }
        }
        public string UrlConfluence { get; set; }
        public string UrlConfluenceStr
        {
            get
            {
                if (UrlConfluenceIdEnum == Cross.UrlConfluence.SiAplica && !string.IsNullOrEmpty(UrlConfluence)) return UrlConfluence;
                else return Utilitarios.GetEnumDescription2(UrlConfluenceIdEnum);
            }
        }

        //public string TipoStr { get { return TipoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaTipo)(TipoId)) : ""; } }

        public string EstadoTecnologiaToString => EstadoIdCalculado.HasValue ? EstadoIdCalculado.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoIdCalculado.Value)) : string.Empty : string.Empty;

        public string EstadoStr
        {
            get
            {
                if (this.TipoTecnologiaId == 4)
                    return "Obsoleto";
                else
                {
                    if (this.FlagFechaFinSoporte.HasValue)
                    {
                        if (!this.FlagFechaFinSoporte.Value)
                            return "Vigente";
                        else
                        {
                            DateTime? fechaFinSoporte = null;
                            if (!FechaCalculoTec.HasValue)
                            {
                                return "Obsoleto";
                            }
                            else
                            {
                                switch (FechaCalculoTec.Value)
                                {
                                    case (int)FechaCalculoTecnologia.FechaInterna:
                                        fechaFinSoporte = this.FechaFinSoporte;
                                        break;
                                    case (int)FechaCalculoTecnologia.FechaExtendida:
                                        fechaFinSoporte = this.FechaExtendida;
                                        break;
                                    case (int)FechaCalculoTecnologia.FechaFinSoporte:
                                        fechaFinSoporte = this.FechaFinSoporte;
                                        break;
                                }
                                if (fechaFinSoporte != null)
                                {
                                    if (fechaFinSoporte.HasValue)
                                    {
                                        if (fechaFinSoporte < DateTime.Now)
                                            return "Obsoleto";
                                        else
                                            return "Vigente";
                                    }
                                    else
                                        return "Obsoleto";
                                }
                                else
                                    return "Obsoleto";
                            }
                        }
                    }
                    else
                        return "Obsoleto";

                }
            }
        }

        //public string EstadoTecnologiaCalculadoToString => EstadoId.HasValue ? EstadoId.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId.Value)) : string.Empty : string.Empty;

        public string EstadoTecnologiaStr
        {
            get
            {
                string es = string.Empty;
                switch (Estado)
                {
                    case 1: es = "Registrado"; break;
                    case 2: es = "En proceso de revisión"; break;
                    case 3: es = "Aprobado"; break;
                    case 4: es = "Observado"; break;
                }
                return es;
            }
        }
        public string TipoTecnologiaStr => string.IsNullOrEmpty(Tipo) ? "Pendiente" : Tipo;
        public string FechaAprobacionStr => FechaAprobacion.HasValue ? FechaAprobacion.Value.ToString("dd/MM/yyyy") : " Pendiente ";
        public string UsuarioAprobacionStr => string.IsNullOrEmpty(UsuarioAprobacion) ? string.Empty : UsuarioAprobacion;
        public string FechaHoraCreacionFormato => FechaCreacion.ToString("dd/MM/yyyy hh:mm:dd tt");

        public int? FechaCalculoTec { get; set; }
        public string FechaCalculoTecStr => FechaCalculoTec.HasValue ? FechaCalculoTec.Value > 0 ? Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)(FechaCalculoTec.Value)) : string.Empty : string.Empty;
        public string FechaCalculoValorTecStr
        {
            get
            {
                if (FlagFechaFinSoporte != true) return "Fecha indefinida";
                else return (FlagFechaFinSoporte ?? (bool?)false).Value == true ? FechaCalculoTec.HasValue ? FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaExtendida ? FechaExtendidaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaInterna ? FechaAcordadaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaFinSoporte ? FechaFinSoporteToString : "Sin fecha fin configurada" : "Sin fecha fin configurada" : "Sin fecha fin configurada";
            }
        }

        public int MotivoId { get; set; }
        public string MotivoStr { get; set; }

        public int? AutomatizacionImplementadaId { get; set; }
        public string AutomatizacionImplementadaStr
        {
            get
            {
                return AutomatizacionImplementadaId.HasValue ? AutomatizacionImplementadaId.Value > 0 ? Utilitarios.GetEnumDescription2((EAutomatizacionImplementada)AutomatizacionImplementadaId) : null : null;
            }
        }

        public int? RevisionSeguridadId { get; set; }
        public RevisionSeguridad RevisionSeguridadIdEnum
        {
            get
            {
                if (RevisionSeguridadId == null) return RevisionSeguridad.NoAplica;

                if (RevisionSeguridadId == 0) return RevisionSeguridad.NoAplica;

                RevisionSeguridad seguridadIdEnum = (RevisionSeguridad)RevisionSeguridadId.Value;

                return seguridadIdEnum;
            }
        }
        public string RevisionSeguridadIdStr
        {
            get { return Utilitarios.GetEnumDescription2(RevisionSeguridadIdEnum); }
        }
        public string RevisionSeguridadStr
        {
            get
            {
                return RevisionSeguridadId.HasValue ? RevisionSeguridadId.Value > 0 ? Utilitarios.GetEnumDescription2((RevisionSeguridad)RevisionSeguridadId) : null : null;
            }
        }
        public string RevisionSeguridadDescripcion { get; set; }

        public string CompatibilidadSO { get; set; }
        public string CompatibilidadSOStr { get { return string.IsNullOrEmpty(CompatibilidadSO) ? "No Aplica" : string.Join(", ", CompatibilidadSO.Split(',')); } }
        public string CompatibilidadCloud { get; set; }
        public string CompatibilidadCloudStr { get { return string.IsNullOrEmpty(CompatibilidadCloud) ? "No Aplica" : string.Join(", ", CompatibilidadCloud.Split(',')); } }

        public string OwnerId { get; set; }
        public string OwnerDisplay { get; set; }

        public string EquipoAprovisionamiento { get; set; }

        public int? NroInstancias { get; set; }

        public bool? FlagConfirmarFamilia { get; set; }
        public string FlagConfirmarFamiliaStr => (FlagConfirmarFamilia.HasValue ? (FlagConfirmarFamilia.Value ? "Si" : "No") : string.Empty);

        public bool FlagTieneEquivalencias { get; set; }
        public string FlagTieneEquivalenciasStr { get { return FlagTieneEquivalencias ? "SÍ" : "NO"; } }

        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public DateTime? FechaAcordada { get; set; }
        public DateTime? FechaExtendida { get; set; }

        public DateTime? FechaCalculada
        {
            get
            {
                DateTime? fechaCalculo = null;

                if (!FechaCalculoTec.HasValue) return fechaCalculo;

                switch ((FechaCalculoTecnologia)FechaCalculoTec.Value)
                {
                    case FechaCalculoTecnologia.FechaExtendida:
                        fechaCalculo = FechaExtendida;
                        break;
                    case FechaCalculoTecnologia.FechaFinSoporte:
                        fechaCalculo = FechaFinSoporte;
                        break;
                    case FechaCalculoTecnologia.FechaInterna:
                        fechaCalculo = FechaAcordada;
                        break;
                }

                return fechaCalculo;
            }
        }

        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaLanzamientoStr => FechaLanzamiento.HasValue ? FechaLanzamiento.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaAcordadaStr => FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaExtendidaStr => FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaDesactivaStr => FechaDesactiva.HasValue ? FechaDesactiva.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
        public string FechaFinSoporteToString => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";
        public string FechaLanzamientoToString => FechaLanzamiento.HasValue ? FechaLanzamiento.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";
        public string FechaAcordadaToString => FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";
        public string FechaExtendidaToString => FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : "Sin fecha fin configurada";

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

        
        public string TipoFechaInterna { get; set; }

        public string ComentariosFechaFin { get; set; }
        public string SustentoMotivo { get; set; }
        public string SustentoUrl { get; set; }
        public int? Fuente { get; set; }
        public string FuenteStr => Fuente.HasValue ? Fuente.Value > 0 ? Utilitarios.GetEnumDescription2((Fuente)(Fuente.Value)) : string.Empty : string.Empty;

        public int? Existencia { get; set; }
        public int? Facilidad { get; set; }
        public int? Riesgo { get; set; }
        public decimal? Vulnerabilidad { get; set; }
        public string CasoUso { get; set; }
        public string Requisitos { get; set; }
        public string Compatibilidad { get; set; }
        public string Aplica { get; set; }

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
        public string SoporteEmpresarial { get; set; }

        public bool? FlagAplicacion { get; set; }
        public string FlagAplicacionStr => (FlagAplicacion.HasValue ? (FlagAplicacion.Value ? "Si" : "No") : string.Empty);
        public string CodigoAPT { get; set; }

        public bool? FlagFechaFinSoporte { get; set; }
        public string FlagFechaFinSoporteStr => (FlagFechaFinSoporte.HasValue ? (FlagFechaFinSoporte.Value ? "Tiene fecha fin soporte" : "No tiene fecha fin soporte") : string.Empty);
        public string FlagVigenteIndefinido => (FlagFechaFinSoporte.HasValue ? (FlagFechaFinSoporte.Value ? "No" : "Sí") : string.Empty);

        public string Observacion { get; set; }

        public bool? FlagSiteEstandar { get; set; }
        public string FlagSiteEstandarStr => (FlagSiteEstandar.HasValue ? (FlagSiteEstandar.Value ? "Si" : "No") : string.Empty);

        public string Fabricante { get; set; }
        public string CodigoTecnologiaAsignado { get; set; }

        public string FlagVigenteFamiliaStr => EstadoId.HasValue ? (EstadoId == (int)ETecnologiaEstado.Vigente ? "Si" : "No") : string.Empty;
        public string EstadTecnologia => EstadoId.HasValue ? EstadoId.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId.Value)) : string.Empty : string.Empty;

        public string TipoProductoStr { get; set; }
        public string CodigoProducto { get; set; }
        public string TribuCoeStr { get; set; }
        public string ResponsableTribuCoeStr { get; set; }
        public string SquadStr { get; set; }
        public string ResponsableSquadStr { get; set; }
        public string TribuCoeStrAnterior { get; set; }
        public string ResponsableTribuCoeStrAnterior { get; set; }
        public string SquadStrAnterior { get; set; }
        public string ResponsableSquadStrAnterior { get; set; }
        public string TipoCambioSquad { get; set; }
        public string ProductoNombre { get; set; }
        public string EstadoResolucion { get; set; }
        
        public string ProductoStr { get { return (Fabricante ?? "") + " " + (ProductoNombre ?? ""); } }

        public string ListaAplicacionStr { get; set; }
        public string TribuCoeId { get; set; }
        public string TribuCoeName { get; set; }
        public string SquadId { get; set; }
        public string SquadName { get; set; }
        public int? CantRoles { get; set; }
        public int? CantFunciones { get; set; }
        public int ProductoId { get; set; }
        public string MotivoDesactiva { get; set; }
        public DateTime? FechaDesactiva { get; set; }
        public string UsuarioDesactiva { get; set; }
        public string ListaExpertosStr { get; set; }
        public bool? FlagComponenteCross { get; set; }
        public string FlagComponenteCrossStr => (FlagComponenteCross.HasValue ? (FlagComponenteCross.Value ? "Si" : "No") : string.Empty);
        public int OwnerTecnologia { get; set; }
        public int? LineamientoBaseSeguridadId { get; set; }
        public RevisionSeguridad LineamientoBaseSeguridadIdEnum
        {
            get
            {
                if (LineamientoBaseSeguridadId == null) return RevisionSeguridad.NoAplica;

                if (LineamientoBaseSeguridadId == 0) return RevisionSeguridad.NoAplica;

                RevisionSeguridad lineamientoBaseSeguridadIdEnum = (RevisionSeguridad)LineamientoBaseSeguridadId.Value;
                return lineamientoBaseSeguridadIdEnum;
            }
        }
        public string LineamientoBaseSeguridadIdStr
        {
            get { return Utilitarios.GetEnumDescription2(LineamientoBaseSeguridadIdEnum); }
        }
        public string LineamientoBaseSeguridad { get; set; }
        public string LineamientoBaseSeguridadStr
        {
            get
            {
                if (LineamientoBaseSeguridadIdEnum == Cross.RevisionSeguridad.SiAplica && !string.IsNullOrEmpty(LineamientoBaseSeguridad)) return LineamientoBaseSeguridad;
                else return Utilitarios.GetEnumDescription2(LineamientoBaseSeguridadIdEnum);
            }
        }

    }

    

    public class TecnologiaBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? SubdominioId { get; set; }
        public int EstadoId { get; set; }
        public int? EstadoIdCalculado
        {
            get
            {
                //int estadoId = (int)ETecnologiaEstado.Vigente;
                //DateTime fechaActual = DateTime.Now.Date;
                //bool flagFechaFinSoporte = FlagFechaFinSoporte ?? false;

                //if (TipoTecnologiaToString.ToLower().Contains("deprecado")) estadoId = (int)ETecnologiaEstado.Deprecado;
                //else
                //{
                //    if (flagFechaFinSoporte)
                //    {
                //        if (!FechaCalculoTec.HasValue) estadoId = (int)ETecnologiaEstado.Obsoleto;
                //        else
                //        {
                //            FechaCalculoTecnologia fechaCalculoTecEnum = (FechaCalculoTecnologia)FechaCalculoTec.Value;
                //            DateTime? fechaCalculada = fechaCalculoTecEnum == FechaCalculoTecnologia.FechaExtendida ? FechaExtendida : fechaCalculoTecEnum == FechaCalculoTecnologia.FechaInterna ? FechaAcordada : fechaCalculoTecEnum == FechaCalculoTecnologia.FechaFinSoporte ? FechaFinSoporte : null;

                //            if (!fechaCalculada.HasValue) estadoId = (int)ETecnologiaEstado.Obsoleto;
                //            else if (fechaCalculada.Value < fechaActual) estadoId = (int)ETecnologiaEstado.Obsoleto;
                //        }
                //    }
                //}


                //return estadoId;

                if (!(FlagFechaFinSoporte ?? false))
                {
                    if (TipoTecnologia == 13 || TipoTecnologia == 15 || TipoTecnologia == 16)
                        return (int)ETecnologiaEstadoEstandar.VigentePorVencer;
                    else
                        return (int)ETecnologiaEstadoEstandar.Vigente;
                }
                
                if ((FlagFechaFinSoporte ?? false) && !FechaCalculoTec.HasValue) return (int)ETecnologiaEstadoEstandar.Obsoleto;
                DateTime fechaComparacion = DateTime.Now;
                if(!FechaCalculada.HasValue)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;

                if (FechaCalculada < fechaComparacion)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;
                else
                {
                    fechaComparacion = fechaComparacion.AddMonths(CantidadMeses1);
                    if (FechaCalculada < fechaComparacion)
                        return (int)ETecnologiaEstadoEstandar.VigentePorVencer;
                    else
                    {
                        if(TipoTecnologia == 13 || TipoTecnologia == 15 || TipoTecnologia == 16)
                            return (int)ETecnologiaEstadoEstandar.VigentePorVencer;
                        else
                            return (int)ETecnologiaEstadoEstandar.Vigente;
                    }                       
                }
            }
        }
        public int CantidadMeses1 { get; set; }
        public int EstadoIdCalculadoEstandar
        {
            get
            {
                DateTime fechaActual = DateTime.Now;
                if (!FechaCalculada.HasValue)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;

                if (FechaCalculada < fechaActual)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;
                else
                {
                    fechaActual = fechaActual.AddMonths(Indicador1);
                    if (FechaCalculada < fechaActual)
                        return (int)ETecnologiaEstadoEstandar.VigentePorVencer;
                    else
                        return (int)ETecnologiaEstadoEstandar.Vigente;
                }
            }
        }
        public string CodigoTecnologia { get; set; }
        public string CodigoProducto { get; set; }
        public string Url { get; set; }
        public bool? FlagFechaFinSoporte { get; set; }
        public int? FechaCalculoTec { get; set; }
        public int? TipoTecnologia { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaAcordada { get; set; }
        public DateTime? FechaCalculada
        {
            get
            {
                DateTime? fechaCalculo = null;

                if (!FechaCalculoTec.HasValue) return fechaCalculo;

                switch ((FechaCalculoTecnologia)FechaCalculoTec.Value)
                {
                    case FechaCalculoTecnologia.FechaExtendida:
                        fechaCalculo = FechaExtendida;
                        break;
                    case FechaCalculoTecnologia.FechaFinSoporte:
                        fechaCalculo = FechaFinSoporte;
                        break;
                    case FechaCalculoTecnologia.FechaInterna:
                        fechaCalculo = FechaAcordada;
                        break;
                }

                return fechaCalculo;
            }
        }
        public string TipoTecnologiaToString { get; set; }
        public string EquipoAprovisionamiento { get; set; }
        public string UnidadStr { get; set; }
        public string OwnerStr { get; set; }
        public string EsquemaLicenciamientoId { get; set; }
        public string EsquemaLicenciamientoStr
        {
            get
            {
                return string.IsNullOrEmpty(EsquemaLicenciamientoId) ? null : !int.TryParse(EsquemaLicenciamientoId, out int esquemaLicenciamientoId) ? null : Utilitarios.GetEnumDescription2((EEsquemaLicenciamientoSuscripcion)esquemaLicenciamientoId);
            }
        }
        public int Indicador1 { get; set; }
    }

    public class TecnologiaEstandarDTO : BaseDTO
    {
        public string Tipo { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoTecnologia { get; set; }
        public string ClaveTecnologia { get; set; }
        public string UrlConfluence { get; set; }
        public string EqAdmContacto { get; set; }
        public string GrupoSoporteRemedy { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string PublicacionLBS { get; set; }
        public int EstadoTecnologiaId { get; set; }
        public int? EstadoId { get; set; }
        public int CantidadMeses1 { get; set; }
        public int? TipoTecnologia { get; set; }
        public int? EstadoIdCalculado
        {
            get
            {
                if (!(FlagFechaFinSoporte ?? false))
                {
                        return EstadoId;
                }
                if ((FlagFechaFinSoporte ?? false) && !FechaCalculoTec.HasValue) return (int)ETecnologiaEstadoEstandar.Obsoleto;
                DateTime fechaComparacion = DateTime.Now;
                if (!FechaCalculada.HasValue)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;

                if (FechaCalculada < fechaComparacion)
                    return (int)ETecnologiaEstadoEstandar.Obsoleto;
                else
                {
                    fechaComparacion = fechaComparacion.AddMonths(CantidadMeses1);
                    if (FechaCalculada < fechaComparacion)
                        return EstadoId == (int)ETecnologiaEstadoEstandar.Vigente ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : EstadoId == (int)ETecnologiaEstadoEstandar.Deprecado ? (int)ETecnologiaEstadoEstandar.DeprecadoPorVencer : EstadoId == (int)ETecnologiaEstadoEstandar.Restringido ? (int)ETecnologiaEstadoEstandar.RestringidoPorVencer : 0;
                    else
                    {
                        return EstadoId;
                    }
                }
            }
        }
        public string EstadoTecnologiaToString => EstadoIdCalculado.HasValue ? EstadoIdCalculado.Value > 0 ? Utilitarios.GetEnumDescription2((ETecnologiaEstadoEstandar)(EstadoIdCalculado.Value)) : string.Empty : string.Empty;

        public int? FuenteId { get; set; }
        public int? FechaCalculoTec { get; set; }
        public string FechaCalculoTecStr { get { return !FechaCalculoTec.HasValue ? null : FechaCalculoTec.Value == 0 ? null : Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)FechaCalculoTec.Value); } }
        public string FechaCalculoTecEstandarStr { get { return !FechaCalculoTec.HasValue ? "Fecha indefinida" : FechaCalculoTec.Value == 0 ? "Fecha indefinida" : Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)FechaCalculoTec.Value); } }
        public DateTime? FechaLanzamiento { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaAcordada { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public DateTime? FechaCalculada
        {
            get
            {
                DateTime? fechaCalculo = null;

                if (!FechaCalculoTec.HasValue) return fechaCalculo;

                switch ((FechaCalculoTecnologia)FechaCalculoTec.Value)
                {
                    case FechaCalculoTecnologia.FechaExtendida:
                        fechaCalculo = FechaExtendida;
                        break;
                    case FechaCalculoTecnologia.FechaFinSoporte:
                        fechaCalculo = FechaFinSoporte;
                        break;
                    case FechaCalculoTecnologia.FechaInterna:
                        fechaCalculo = FechaAcordada;
                        break;
                }

                return fechaCalculo;
            }
        }
        public bool? FlagFechaFinSoporte { get; set; }

        public string FuenteIdStr => FlagFechaFinSoporte.HasValue && FlagFechaFinSoporte.Value ? FuenteId.HasValue ? Utilitarios.GetEnumDescription2((Fuente)FuenteId.Value) : string.Empty : string.Empty;
        public string EstadoIdStr => EstadoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoId) : string.Empty;
        public string NombreTecnologia => !string.IsNullOrEmpty(CodigoTecnologia) ?
            $"[{CodigoTecnologia}] {ClaveTecnologia}" : $"{ClaveTecnologia}";

        public string FechaFinSoporteToString => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "-";
        public string FechaExtendidaToString => FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : "-";
        public string FechaAcordadaToString => FechaAcordada.HasValue ? FechaAcordada.Value.ToString("dd/MM/yyyy") : "-";

        public string FechaCalculoValorTecStr
        {
            get
            {
                if (FlagFechaFinSoporte != true) return "Fecha indefinida";
                else return (FlagFechaFinSoporte ?? (bool?)false).Value == true ? FechaCalculoTec.HasValue ? FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaExtendida ? FechaExtendidaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaInterna ? FechaAcordadaToString : FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaFinSoporte ? FechaFinSoporteToString : "Sin fecha fin configurada" : "Sin fecha fin configurada" : "Sin fecha fin configurada";
            }
        }

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

        public string EquipoAprovisionamiento { get; set; }
        public string UnidadStr { get; set; }
        public string OwnerStr { get; set; }
        public string EsquemaLicenciamientoId { get; set; }
        public string EsquemaLicenciamientoStr
        {
            get
            {
                return string.IsNullOrEmpty(EsquemaLicenciamientoId) ? null : !int.TryParse(EsquemaLicenciamientoId, out int esquemaLicenciamientoId) ? null : Utilitarios.GetEnumDescription2((EEsquemaLicenciamientoSuscripcion)esquemaLicenciamientoId);
            }
        }

        public string TribuCoeDisplayName { get; set; }
        public string SquadDisplayName { get; set; }
        public int? LineamientoTecnologiaId { get; set; }
        public UrlConfluence LineamientoTecnologiaIdEnum
        {
            get
            {
                if (LineamientoTecnologiaId == null) return Cross.UrlConfluence.NoAplica;

                if (LineamientoTecnologiaId == 0) return Cross.UrlConfluence.NoAplica;

                UrlConfluence lineamientoTecnologiaIdEnum = (UrlConfluence)LineamientoTecnologiaId.Value;

                return lineamientoTecnologiaIdEnum;
            }
        }
        public string LineamientoTecnologiaIdStr
        {
            get { return Utilitarios.GetEnumDescription2(LineamientoTecnologiaIdEnum); }
        }
        public string LineamientoTecnologia { get; set; }
        public string LineamientoTecnologiaStr
        {
            get
            {
                if (LineamientoTecnologiaIdEnum == Cross.UrlConfluence.SiAplica && !string.IsNullOrEmpty(LineamientoTecnologia)) return LineamientoTecnologia;
                else return Utilitarios.GetEnumDescription2(LineamientoTecnologiaIdEnum);
            }
        }
        public int? LineamientoBaseSeguridadId { get; set; }
        public RevisionSeguridad LineamientoBaseSeguridadIdEnum
        {
            get
            {
                if (LineamientoBaseSeguridadId == null) return RevisionSeguridad.NoAplica;

                if (LineamientoBaseSeguridadId == 0) return RevisionSeguridad.NoAplica;

                RevisionSeguridad lineamientoBaseSeguridadIdEnum = (RevisionSeguridad)LineamientoBaseSeguridadId.Value;
                return lineamientoBaseSeguridadIdEnum;
            }
        }
        public string LineamientoBaseSeguridadIdStr
        {
            get { return Utilitarios.GetEnumDescription2(LineamientoBaseSeguridadIdEnum); }
        }
        public string LineamientoBaseSeguridad { get; set; }
        public string LineamientoBaseSeguridadStr
        {
            get
            {
                if (LineamientoBaseSeguridadIdEnum == Cross.RevisionSeguridad.SiAplica && !string.IsNullOrEmpty(LineamientoBaseSeguridad)) return LineamientoBaseSeguridad;
                else return Utilitarios.GetEnumDescription2(LineamientoBaseSeguridadIdEnum);
            }
        }
        public string Aplica { get; set; }
        public string CompatibilidadSOIds { get; set; }
        public string CompatibilidadSOIdsStr { get { return string.IsNullOrEmpty(CompatibilidadSOIds) ? "No Aplica" : string.Join(",", CompatibilidadSOIds.Split(',')); } }
        public string CompatibilidadCloudIds { get; set; }
        public string CompatibilidadCloudIdsStr { get { return string.IsNullOrEmpty(CompatibilidadCloudIds) ? "No Aplica" : string.Join(",", CompatibilidadCloudIds.Split(',')); } }
        public bool FlagVigentePorVencer { get; set; }
        //public string EstadoIdCalculadoStr { get { return FlagVigentePorVencer ? "En salida/Por vencer" : EstadoIdStr; } }
        public int? TecReemplazoDepId { get; set; }
        public string TecnologiaReemplazo { get; set; }
    }

    public class TecnologiaOwnerConsolidadoObsolescenciaDTO
    {
        public int Nivel { get; set; }
        public string OwnerParentId { get; set; }
        public int? OwnerId { get; set; }
        public string Owner { get; set; }
        public string ProductoStr { get; set; }
        public int ObsoletoKPI { get; set; }
        public decimal PorcentajeObsoletoKPI { get; set; }
        public int VigenteKPI { get; set; }
        public int Vigente12MesesKPI { get; set; }
        public int Vigente12MesesAMasKPI { get; set; }
        public decimal PorcentajeVigenteKPI { get; set; }
        public int DeprecadoKPI { get; set; }
        public decimal PorcentajeDeprecadoKPI { get; set; }
        public int TotalKPI { get; set; }
        public decimal PorcentajeTotalKPI { get; set; }
        public decimal PorcentajeTotalVigente12MesesKPI { get; set; }
        public decimal PorcentajeTotalVigente12MesesAMasKPI { get; set; }

        public List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListaDetalle { get; set; }
    }

    public class TecnologiaGestionadoPorConsolidadoObsolescenciaDTO
    {
        public int Nivel { get; set; }
        public string SoportadoPorParent { get; set; }
        public string SoportadoPorId { get; set; }
        public string SoportadoPor { get; set; }
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

        public List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListaDetalle { get; set; }
    }

    public class TecnologiaUdFConsolidadoObsolescenciaDTO
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

        public List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListaDetalle { get; set; }
    }

    public class ProductoList
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public  bool Activo { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public int Estado { get; set; }
        public int? EstadoId { get; set; }
        public string ProductoNombre { get; set; }
        public string TribuCoeStr { get; set; }
        public string SquadStr { get; set; }
        public string CodigoProducto { get; set; }
    }

}


