using System; 
namespace BCP.CVT.DTO.Auditoria
{
    public class AuditoriaAPIDTO
    {
        public int AuditoriaAPIId { get; set; }
        public string APINombre { get; set; }
        public string APIUsuario { get; set; }
        public string APIParametros { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public string APIMetodo { get; set; }

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
