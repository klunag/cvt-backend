using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class VistaRelacionDto
    {
        public string CodigoApt { get; set; }
        public string Aplicacion { get; set; }
        public string GestionadoPor { get; set; }
        public string DetalleCriticidad { get; set; }
        public string JefeEquipo { get; set; }
        public string Equipo { get; set; }
        public string DetalleAmbiente { get; set; }
        public string SistemaOperativo { get; set; }
        public int EstadoId { get; set; }

        public string ListaPCI { get; set; }

        public int ApplicationId { get; set; }
        public string EstadoRelacion
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)EstadoId);
            }
        }
        public string TipoActivoTI { get; set; }
        public int EquipoId { get; set; }
        public int? TipoEquipoId { get; set; }

        public bool? RelacionAplicacion { get; set; }
        public string RelacionAplicacionDesc
        {
            get
            {
                return RelacionAplicacion == null ? null : RelacionAplicacion == true ? Utilitarios.GetEnumDescription2((ERelacionAplicacion)1) : Utilitarios.GetEnumDescription2((ERelacionAplicacion)2);
            }
        }

        public int? TipoRelacionId { get; set; }
        public string TipoRelacion { get; set; }
        public string TipoRelacionDesc
        {
            get
            {
                return TipoRelacionId == null ? null : Utilitarios.GetEnumDescription2((ETipoRelacionComponente)TipoRelacionId);
            }
        }
        public int TotalRows { get; set; }


    }
}
