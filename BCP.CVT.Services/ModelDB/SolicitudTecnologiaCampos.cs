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
    
    public partial class SolicitudTecnologiaCampos
    {
        public long SolicitudTecnologiaCamposId { get; set; }
        public Nullable<int> SolicitudTecnologiaId { get; set; }
        public Nullable<int> ConfiguracionTecnologiaCamposId { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public Nullable<int> EstadoCampo { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    }
}
