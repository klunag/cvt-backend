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
    
    public partial class Proceso
    {
        public int ProcesoId { get; set; }
        public int TipoTareaId { get; set; }
        public Nullable<int> ResultadoEjecucionId { get; set; }
        public string LogResultados { get; set; }
        public string LogErrores { get; set; }
        public Nullable<System.DateTime> FechaInicioEjecucion { get; set; }
        public Nullable<System.DateTime> FechaFinEjecucion { get; set; }
        public bool FlagEjecutado { get; set; }
        public bool FlagActivo { get; set; }
        public string CreadoPor { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    }
}