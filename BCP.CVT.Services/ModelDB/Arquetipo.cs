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
    
    public partial class Arquetipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Arquetipo()
        {
            this.Tecnologia = new HashSet<Tecnologia>();
            this.ArquetipoTecnologia = new HashSet<ArquetipoTecnologia>();
        }
    
        public int ArquetipoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreDiag { get; set; }
        public Nullable<bool> DiagIs { get; set; }
        public string DirFisicaDiag { get; set; }
        public Nullable<int> EstadoAprob { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string Codigo { get; set; }
        public Nullable<bool> Automatizado { get; set; }
        public int TipoArquetipoId { get; set; }
        public int EntornoId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tecnologia> Tecnologia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArquetipoTecnologia> ArquetipoTecnologia { get; set; }
        public virtual Entorno Entorno { get; set; }
        public virtual TipoArquetipo TipoArquetipo { get; set; }
    }
}
