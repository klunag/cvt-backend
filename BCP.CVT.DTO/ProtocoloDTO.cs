using System;

namespace BCP.CVT.DTO
{
    public class ProtocoloDTO
    {
        public int ProtocoloId { get; set; }
        public string Nombre { get; set; }
        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TotalFilas { get; set; }
        public string FechaCreacionStr
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
    }

}
