using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ModuloAplicacionDTO: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool FlagRegistrado { get; set; }

        public string TipoDesarrollo { get; set; }
        public string InfraestructuraModulo { get; set; }
        public string MetodoAutenticacion { get; set; }
        public string MetodoAutorizacion { get; set; }
        public string CategoriaTecnologica { get; set; }

        public string ModeloEntrega { get; set; }
        public string Contingencia { get; set; }
        public bool? FlagOOR { get; set; }
        public bool? FlagRatificaOOR { get; set; }
        public string CodigoInterfaz { get; set; }
        public string CompatibilidadHV { get; set; }
        public string NombreServidor { get; set; }
        public string CompatibilidadWindows { get; set; }
        public string CompatibilidadNavegador { get; set; }
        public string InstaladaDesarrollo { get; set; }
        public string InstaladaCertificacion { get; set; }
        public string InstaladaProduccion { get; set; }
        public string NCET { get; set; }
        public string NCLS { get; set; }
        public string NCG { get; set; }
        public string ResumenSeguridadInformacion { get; set; }

        public int? CriticidadId { get; set; }
        public string EstadoModulo { get; set; }
    }
}
