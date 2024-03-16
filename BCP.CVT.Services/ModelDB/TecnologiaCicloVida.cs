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
    
    public partial class TecnologiaCicloVida
    {
        public long TecnologiaCicloVidaId { get; set; }
        public int TecnologiaId { get; set; }
        public Nullable<System.DateTime> FechaObsolescencia { get; set; }
        public Nullable<System.DateTime> FechaCalculoBase { get; set; }
        public bool EsIndefinida { get; set; }
        public Nullable<decimal> IndiceObsolescencia { get; set; }
        public Nullable<decimal> Criterio2 { get; set; }
        public Nullable<decimal> Criterio3 { get; set; }
        public Nullable<decimal> Criterio4 { get; set; }
        public Nullable<decimal> Vulnerabilidad { get; set; }
        public Nullable<decimal> Riesgo { get; set; }
        public bool FlagActivo { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public int DiaRegistro { get; set; }
        public int MesRegistro { get; set; }
        public int AnioRegistro { get; set; }
        public int Obsoleto { get; set; }
        public Nullable<decimal> IndiceObsolescenciaProyectado1 { get; set; }
        public Nullable<decimal> IndiceObsolescenciaProyectado2 { get; set; }
        public Nullable<decimal> RiesgoProyectado1 { get; set; }
        public Nullable<decimal> RiesgoProyectado2 { get; set; }
    
        public virtual Tecnologia Tecnologia { get; set; }
    }
}
