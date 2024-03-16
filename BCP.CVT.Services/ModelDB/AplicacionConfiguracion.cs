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
    
    public partial class AplicacionConfiguracion
    {
        public long AplicacionConfiguracion1 { get; set; }
        public int AplicacionId { get; set; }
        public Nullable<decimal> IndiceObsolescencia { get; set; }
        public Nullable<decimal> Exposicion { get; set; }
        public Nullable<decimal> RiesgoAplicacion { get; set; }
        public Nullable<decimal> Priorizacion { get; set; }
        public Nullable<decimal> UbicacionGrafico { get; set; }
        public int Obsoleto { get; set; }
        public bool FlagActivo { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public int DiaRegistro { get; set; }
        public int MesRegistro { get; set; }
        public int AnioRegistro { get; set; }
        public Nullable<decimal> IndiceObsolescencia_Proyeccion1 { get; set; }
        public Nullable<decimal> IndiceObsolescencia_Proyeccion2 { get; set; }
        public Nullable<decimal> RiesgoAplicacion_Proyeccion1 { get; set; }
        public Nullable<decimal> RiesgoAplicacion_Proyeccion2 { get; set; }
        public Nullable<decimal> Priorizacion_Proyeccion1 { get; set; }
        public Nullable<decimal> Priorizacion_Proyeccion2 { get; set; }
        public Nullable<decimal> IndiceObsolescencia_ForwardLooking { get; set; }
        public Nullable<decimal> IndiceObsolescencia_ForwardLooking_Proyeccion1 { get; set; }
        public Nullable<decimal> IndiceObsolescencia_ForwardLooking_Proyeccion2 { get; set; }
    
        public virtual Aplicacion Aplicacion { get; set; }
    }
}
