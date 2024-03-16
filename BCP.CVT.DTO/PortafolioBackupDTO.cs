using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class PortafolioBackupDTO
    {
        public int PortafolioBackupId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string Comentario { get; set; }

        public byte[] BackupBytes { get; set; }


        public string FechaCreacionFormato
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
                //return FechaCreacion != null ? FechaCreacion.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaCreacionStr
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

    }
}
