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
    
    public partial class Subdominio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subdominio()
        {
            this.SubdominioEquivalencia = new HashSet<SubdominioEquivalencia>();
            this.Tecnologia = new HashSet<Tecnologia>();
            this.EquipoSoftwareBase = new HashSet<EquipoSoftwareBase>();
        }
    
        public int SubdominioId { get; set; }
        public int DominioId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Peso { get; set; }
        public string Dueno { get; set; }
        public bool CalObs { get; set; }
        public bool Activo { get; set; }
        public string UsuarioAsociadoPor { get; set; }
        public Nullable<System.DateTime> FechaAsociacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<bool> FlagIsVisible { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubdominioEquivalencia> SubdominioEquivalencia { get; set; }
        public virtual Dominio Dominio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tecnologia> Tecnologia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipoSoftwareBase> EquipoSoftwareBase { get; set; }
    }
}
