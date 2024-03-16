using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class MensajeDTO: BaseDTO
    {
        public int TipoMensajeId { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaUltimaVisita { get; set; }
        public string UsuarioUltimaVisita { get; set; }
        public string NombreUsuarioCreacion { get; set; }

        public string FechaUltimaVisitaStr => FechaUltimaVisita.HasValue ? FechaUltimaVisita.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string TipoMensajeStr => Utilitarios.GetEnumDescription2((ETipoMensaje)(TipoMensajeId)) ;         
    }
}
