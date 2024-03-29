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
    
    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.ProductoArquetipo = new HashSet<ProductoArquetipo>();
            this.Tecnologia = new HashSet<Tecnologia>();
            this.ProductoManagerRoles = new HashSet<ProductoManagerRoles>();
        }
    
        public int ProductoId { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DominioId { get; set; }
        public int SubDominioId { get; set; }
        public int TipoProductoId { get; set; }
        public int EstadoObsolescenciaId { get; set; }
        public string TribuCoeId { get; set; }
        public string TribuCoeDisplayName { get; set; }
        public string SquadId { get; set; }
        public string SquadDisplayName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerMatricula { get; set; }
        public Nullable<int> GrupoTicketRemedyId { get; set; }
        public string GrupoTicketRemedyNombre { get; set; }
        public bool EsAplicacion { get; set; }
        public Nullable<int> AplicacionId { get; set; }
        public string Codigo { get; set; }
        public Nullable<int> TipoCicloVidaId { get; set; }
        public Nullable<int> EsquemaLicenciamientoSuscripcionId { get; set; }
        public string EquipoAdmContacto { get; set; }
        public string EquipoAprovisionamiento { get; set; }
        public bool FlagActivo { get; set; }
        public string CreadoPor { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoArquetipo> ProductoArquetipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tecnologia> Tecnologia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoManagerRoles> ProductoManagerRoles { get; set; }
    }
}
