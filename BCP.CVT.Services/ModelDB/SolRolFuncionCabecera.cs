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
    
    public partial class SolRolFuncionCabecera
    {
        public long IdSolRolFuncionCabecera { get; set; }
        public Nullable<long> IdSolicitud { get; set; }
        public string OwnerProducto { get; set; }
        public string AnalistaSeguridad { get; set; }
        public Nullable<int> EstadoSolicitudCabecera { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaRevision { get; set; }
        public string RevisadoPor { get; set; }
        public string NombreOwner { get; set; }
        public string NombreAnalista { get; set; }
        public Nullable<int> IdProducto { get; set; }
        public string BuzonOwnerProducto { get; set; }
        public string BuzonSeguridad { get; set; }
        public string BuzonEspecialista { get; set; }
        public Nullable<int> IdTipoSolicitud { get; set; }
    }
}
