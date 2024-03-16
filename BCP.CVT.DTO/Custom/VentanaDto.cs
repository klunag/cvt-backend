using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class VentanaDto
    {
        public string Equipo { get; set; }
        public string TipoEquipo { get; set; }                
        public string Dominio { get; set; }
        public string SO { get; set; }
        public bool? FlagTemporal { get; set; }
        public int? CaracteristicaEquipo { get; set; }
        public AmbienteDTO Ambiente { get; set; }
        public string TemporalToString => FlagTemporal.HasValue ? (FlagTemporal.Value ? "Carga manual" : "Descubrimiento automático") : "Descubrimiento automático";
        public string CaracteristicaEquipoToString
        {
            get
            {
                if (CaracteristicaEquipo == 0)
                    return string.Empty;
                else
                    return Utilitarios.GetEnumDescription2((ECaracteristicaEquipo)CaracteristicaEquipo);
            }
        }
    }
}
