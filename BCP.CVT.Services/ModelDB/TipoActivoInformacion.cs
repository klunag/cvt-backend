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
    
    public partial class TipoActivoInformacion
    {
        public int TipoActivoInformacionId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> FlujoRegistro { get; set; }
        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<bool> FlagEliminado { get; set; }
        public Nullable<bool> FlagUserIT { get; set; }
        public Nullable<bool> FlagExterna { get; set; }
    }
}
