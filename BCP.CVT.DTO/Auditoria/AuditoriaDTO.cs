using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Auditoria
{
    public class AuditoriaDTO 
    {
        public int AuditoriaDataId { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string IdAsociado { get; set; }
        public string Campo { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int TotalFilas { get; set; }

        public string FechaCreacionFormato
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
                //return FechaCreacion != null ? FechaCreacion.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
    }
}
