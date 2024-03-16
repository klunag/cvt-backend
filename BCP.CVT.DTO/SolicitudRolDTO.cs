using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class SolicitudRolDTO : BaseDTO
    {
        public long? SolicitudId { get; set; }
        public string Producto { get; set; }
        public int? ProductoId { get; set; }
        public string Dominio { get; set; }
        public int? DominioId { get; set; }
        public string SubDominio { get; set; }
        public int? SubDominioId { get; set; }
        public int? CantRoles { get; set; }
        public int? EstadoSolicitud { get; set; }
        public string EstadoSolicitudStr
        {
            get
            {

                return Utilitarios.GetEnumDescription2((EstadoSolicitudRolFuncion)EstadoSolicitud);
            }
        }

        public DateTime? FechaRevision { get; set; }
        public string FechaRevisionStr => FechaRevision.HasValue ? FechaRevision.Value.ToString("dd/MM/yyyy") : string.Empty;

        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionStr => FechaCreacion.HasValue ? FechaCreacion.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string RevisadoPor { get; set; }
        //------
        public string OwnerNombre { get; set; }
        public string Owner { get; set; }
        public string SeguridadNombre { get; set; }
        public string Seguridad { get; set; }
        public string BuzonOwnerProducto { get; set; }
        public string BuzonOwnerSeguridad { get; set; }
        public string BuzonOwnerEspecialista { get; set; }
        public int? IdTipoSolicitud { get; set; }
        public string TipoSolicitudStr
        {
            get
            {
                return Utilitarios.GetEnumDescription2((TipoSolicitudRolFunciones)IdTipoSolicitud);
            }
        }
        public string Codigo { get; set; }
        public string Fabricante { get; set; }
        public int? FuncionId { get; set; }
        public bool EsOwner { get; set; }
        public bool EsSeguridad { get; set; }
        public bool EsRegistroMio { get; set; }
        public int EsPendienteMio { get; set; }
        public string CreadoPor { get; set; }
        public List<RolesProductoAmbienteDTO> Ambiente { get; set; }
    }
    public class SolicitudRolResponsablesDTO 
    {
        public int SolicitudCabeceraId { get; set; }
        public string Responsable { get; set; }
        public string BuzonResponsable { get; set; }
        public int ResponsableId { get; set; }
        public int TipoSolicitudId { get; set; }
        public string ResponsableStr
        {
            get
            {
                return Utilitarios.GetEnumDescription2((TipoResponsableSolicitud)ResponsableId).ToUpper();
            }
        }
        public bool EsAprobado { get; set; }
        public int EstadoSolicitudId { get; set; }
    }
    public class SolicitudRolCorreosDTO
    {
        public string Matricula { get; set; }
        public string Mail { get; set; }
    }

}