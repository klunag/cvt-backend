using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionResponsableDto : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string EstadoAplicacion { get; set; }
        public string TTL { get; set; }
        public string JdE { get; set; }
        public string Broker { get; set; }
        public string Owner { get; set; }
        public string Gestor { get; set; }
        public string Experto { get; set; }

        public string Matricula { get; set; }
        public string Colaborador { get; set; }
        public int PortafolioResponsableId { get; set; }
        public string Rol { get; set; }        
    }

    public class IndicadorResponsableDto : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string EstadoAplicacion { get; set; }
        public int TotalTtl { get; set; }
        public int TotalJde { get; set; }
        public int TotalBroker { get; set; }
        public int TotalOwner { get; set; }
        public int TotalExperto { get; set; }
        public int TotalGestor { get; set; }
        public int TotalFilas { get; set; }

        public int TotalTtlNoAplica { get; set; }
        public int TotalJdeNoAplica { get; set; }
        public int TotalBrokerNoAplica { get; set; }
        public int TotalOwnerNoAplica { get; set; }
        public int TotalExpertoNoAplica { get; set; }
        public int TotalGestorNoAplica { get; set; }        
    }

    public class IndicadorResponsableDetalleDto : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string EstadoAplicacion { get; set; }
        public int Indicador { get; set; }
        public string Comentario { get; set; }
        public int TotalFilas { get; set; }
    }

}
