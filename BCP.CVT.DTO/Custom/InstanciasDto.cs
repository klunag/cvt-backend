using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class InstanciasDto
    {
        public int Orden { get; set; }
        public int Subdominio { get; set; }
        public string NombreReporte { get; set; }
        public int Ahora { get; set; }
        public int Meses12 { get; set; }
        public int Meses24 { get; set; }
        public int Meses36 { get; set; }
        public int Mas36 { get; set; }
        public int Deprecado { get; set; }
        public string DeprecadoToString
        {
            get
            {
                return Deprecado > 0 ? "X" : string.Empty;
            }
        }
        public int TotalSubdominio
        {
            get
            {
                return Ahora + Meses12 + Meses24 + Meses36 + Mas36;
            }
        }
        public int Total { get; set; }
        public int TecnologiaId { get; set; }

        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string EstadoAplicacion { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string AreaBIAN { get; set; }
        public string Fabricante { get; set; }
        public string NombreProducto { get; set; }
        public string Familia
        {
            get
            {
                return string.Format("{0} {1}", Fabricante, NombreProducto);
            }
        }
        public string ClaveTecnologia { get; set; }
        public string TipoComponente { get; set; }
        public string DetalleAmbiente { get; set; }

        public int SemanaNro { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
