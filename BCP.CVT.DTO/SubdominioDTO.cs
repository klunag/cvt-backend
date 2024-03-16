using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SubdominioDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Peso { get; set; }
        public string MatriculaDueno { get; set; }
        public bool CalculoObs { get; set; }
        public int NumTecAsociadas { get; set; }
        public DateTime? FechaAsociacion { get; set; }
        public string UsuarioAsociadoPor { get; set; }
        public int DomIdAsociado { get; set; }
        public string DomNomAsociado { get; set; }

        public bool? IsVisible { get; set; }

        public string FechaAsociacionToString
        {
            get
            {
                return FechaAsociacion.HasValue? FechaAsociacion.Value.ToString() : string.Empty;           
            }                          
        }
    }
}
