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
    
    public partial class ConfiguracionColumnaAplicacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConfiguracionColumnaAplicacion()
        {
            this.InfoCampoPortafolio = new HashSet<InfoCampoPortafolio>();
        }
    
        public int ConfiguracionColumnaAplicacionId { get; set; }
        public string NombreBD { get; set; }
        public string NombreExcel { get; set; }
        public Nullable<int> TablaProcedencia { get; set; }
        public Nullable<bool> FlagEdicion { get; set; }
        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<bool> FlagVerExportar { get; set; }
        public Nullable<int> OrdenColumna { get; set; }
        public string NombreBDEntidadRelacion { get; set; }
        public Nullable<bool> FlagCampoNuevo { get; set; }
        public Nullable<bool> FlagEliminado { get; set; }
        public Nullable<bool> FlagModificable { get; set; }
        public Nullable<bool> FlagObligatorio { get; set; }
        public Nullable<bool> FlagMostrarCampo { get; set; }
        public Nullable<int> ActivoAplica { get; set; }
        public Nullable<int> ModoLlenado { get; set; }
        public string RolRegistra { get; set; }
        public string RolAprueba { get; set; }
        public string DescripcionCampo { get; set; }
        public Nullable<int> NivelConfiabilidad { get; set; }
        public string RolResponsableActualizacion { get; set; }
        public Nullable<int> TipoRegistro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InfoCampoPortafolio> InfoCampoPortafolio { get; set; }
    }
}