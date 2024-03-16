using System;

namespace BCP.CVT.DTO
{
    public class RelacionamientoFuncionDTO
    {
        public int RelacionReglasGeneralesId { get; set; }
        public int AplicaEn { get; set; }
        public string AplicaEnStr { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public bool FlagActivo { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
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
