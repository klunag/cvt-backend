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
    
    public partial class Parametrica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parametrica()
        {
            this.ParametricaDetalle = new HashSet<ParametricaDetalle>();
        }
    
        public int ParametricaId { get; set; }
        public int EntidadId { get; set; }
        public string Valor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParametricaDetalle> ParametricaDetalle { get; set; }
    }
}
