//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BCP.CVT.Services.ModelDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SolicitudAplicacionDetalle
    {
        public int AplicacionId { get; set; }
        public string MotivoCreacion { get; set; }
        public string PersonaSolicitud { get; set; }
        public string ModeloEntrega { get; set; }
        public string PlataformaBCP { get; set; }
        public string EntidadUso { get; set; }
        public string Proveedor { get; set; }
        public string Ubicacion { get; set; }
        public string Infraestructura { get; set; }
        public string RutaRepositorio { get; set; }
        public string Contingencia { get; set; }
        public string MetodoAutenticacion { get; set; }
        public string MetodoAutorizacion { get; set; }
        public string AmbienteInstalacion { get; set; }
        public string GrupoServiceDesk { get; set; }
        public Nullable<bool> FlagOOR { get; set; }
        public Nullable<bool> FlagRatificaOOR { get; set; }
        public string AplicacionReemplazo { get; set; }
        public string TipoDesarrollo { get; set; }
        public Nullable<System.DateTime> FechaSolicitud { get; set; }
        public Nullable<int> EstadoSolicitudId { get; set; }
        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string AprobadoPor { get; set; }
        public string ObservadoPor { get; set; }
        public string Observacion { get; set; }
        public string CodigoInterfaz { get; set; }
        public string InterfazApp { get; set; }
        public string NombreServidor { get; set; }
        public string CompatibilidadWindows { get; set; }
        public string CompatibilidadNavegador { get; set; }
        public string CompatibilidadHV { get; set; }
        public string InstaladaDesarrollo { get; set; }
        public string InstaladaCertificacion { get; set; }
        public string InstaladaProduccion { get; set; }
        public string GrupoTicketRemedy { get; set; }
        public string NCET { get; set; }
        public string NCLS { get; set; }
        public string NCG { get; set; }
        public string ResumenSeguridadInformacion { get; set; }
        public string ProcesoClave { get; set; }
        public string Confidencialidad { get; set; }
        public string Integridad { get; set; }
        public string Disponibilidad { get; set; }
        public string Privacidad { get; set; }
        public string Clasificacion { get; set; }
        public string RoadmapPlanificado { get; set; }
        public string DetalleEstrategia { get; set; }
        public string EstadoRoadmap { get; set; }
        public string EtapaAtencion { get; set; }
        public string RoadmapEjecutado { get; set; }
        public string FechaInicioRoadmap { get; set; }
        public string FechaFinRoadmap { get; set; }
        public string CodigoAppReemplazo { get; set; }
        public string SWBase_SO { get; set; }
        public string SWBase_HP { get; set; }
        public string SWBase_LP { get; set; }
        public string SWBase_BD { get; set; }
        public string SWBase_Framework { get; set; }
        public string RET { get; set; }
        public string CriticidadAplicacionBIA { get; set; }
        public string ProductoMasRepresentativo { get; set; }
        public string MenorRTO { get; set; }
        public string MayorGradoInterrupcion { get; set; }
        public Nullable<bool> FlagFileCheckList { get; set; }
        public Nullable<bool> FlagFileMatriz { get; set; }
        public string GestorAplicacionCTR { get; set; }
        public string ConsultorCTR { get; set; }
        public string ValorL_NC { get; set; }
        public string ValorM_NC { get; set; }
        public string ValorN_NC { get; set; }
        public string ValorPC_NC { get; set; }
        public string UnidadUsuario { get; set; }
        public int AplicacionDetalleId { get; set; }
    }
}
