using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AmbienteDTO: BaseDTO
    {
        public int Codigo { get; set; }
        public string DetalleAmbiente { get; set; }
        public string PrefijoBase { get; set; }
        public string PrefijoBase2 { get; set; }
        public int? DiaInicio { get; set; }
        public int? DiaFin { get; set; }
        public string DiaInicio_HoraInicio { get; set; }
        public string DiaInicio_HoraFin { get; set; }
        public string DiaFin_HoraInicio { get; set; }
        public string DiaFin_HoraFin { get; set; }
        public int TotalEquipos { get; set; }
        public string Ventana
        {
            get
            {
                if (DiaInicio.HasValue && DiaFin.HasValue)
                {
                    var diaUno = Utilitarios.GetEnumDescription2((EDias)DiaInicio.Value);
                    var diaDos = Utilitarios.GetEnumDescription2((EDias)DiaFin.Value);

                    return string.Format("{0} de {1} a {2} y {3} de {4} y {5}", diaUno, DiaInicio_HoraInicio, DiaInicio_HoraFin, diaDos, DiaFin_HoraInicio, DiaFin_HoraFin);
                }
                else
                    return "No definido";
            }
        }
    }
}
