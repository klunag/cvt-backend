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
    
    public partial class Relacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Relacion()
        {
            this.RelacionDetalle = new HashSet<RelacionDetalle>();
            this.ComentarioEliminacionRelacion = new HashSet<ComentarioEliminacionRelacion>();
        }
    
        public long RelacionId { get; set; }
        public string CodigoAPT { get; set; }
        public int TipoId { get; set; }
        public Nullable<int> AmbienteId { get; set; }
        public Nullable<int> EquipoId { get; set; }
        public int EstadoId { get; set; }
        public bool FlagActivo { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> DiaRegistro { get; set; }
        public Nullable<int> MesRegistro { get; set; }
        public Nullable<int> AnioRegistro { get; set; }
        public Nullable<System.DateTime> FechaRegistroCuarentena { get; set; }
        public Nullable<System.DateTime> FechaFinCuarentena { get; set; }
        public Nullable<bool> FlagRelacionAplicacion { get; set; }
        public string Funcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelacionDetalle> RelacionDetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComentarioEliminacionRelacion> ComentarioEliminacionRelacion { get; set; }
        public virtual Equipo Equipo { get; set; }

        // Relacion App-Componente
        public Nullable<int> TipoRelacionComponente { get; set; }
    }
}
