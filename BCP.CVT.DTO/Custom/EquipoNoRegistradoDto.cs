using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class EquipoNoRegistradoDto
    {
        public int EquipoNoRegistradoId { get; set; }
        public string NombreEquipo { get; set; }
        public string IPEquipo { get; set; }
        public int Origen { get; set; }
        public string OrigenToString
        {
            get
            {                
                return Utilitarios.GetEnumDescription2((OrigenEquipoNoRegistrado)Origen);
            }
        }
        public int Motivo { get; set; }
        public string MotivoToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((MotivoEquipoNoRegistrado)Motivo);
            }
        }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroToString
        {
            get
            {
                return FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public int Estado { get; set; }
        public string EstadoToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EstadoEquipoNoRegistrado)Estado);
            }
        }
        public bool? FlagAprobado { get; set; }
        public string FlagAprobadoStr { get { return (new int[] { (int)OrigenEquipoNoRegistrado.Qualys }).Contains(Origen) ? !FlagAprobado.HasValue ? "Pendiente" : FlagAprobado.Value ? "Aprobado" : "Rechazado" : "Pendiente"; } }
        public DateTime? FechaDescubrimiento { get; set; }
        public int? Fuente { get; set; }
        public int? EquipoId { get; set; }
        public string SistemaOperativo { get; set; }
        public int TotalFilas { get; set; }
        public string creadoPor { get; set; }
    }
}
