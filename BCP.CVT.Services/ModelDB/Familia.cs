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
    
    public partial class Familia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Familia()
        {
            this.Tecnologia = new HashSet<Tecnologia>();
        }
    
        public int FamiliaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaFinSoporte { get; set; }
        public Nullable<System.DateTime> FechaExtendida { get; set; }
        public Nullable<System.DateTime> FechaInterna { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> Existencia { get; set; }
        public Nullable<int> Facilidad { get; set; }
        public Nullable<int> Riesgo { get; set; }
        public Nullable<decimal> Vulnerabilidad { get; set; }
        public string Fabricante { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tecnologia> Tecnologia { get; set; }
    }
}
