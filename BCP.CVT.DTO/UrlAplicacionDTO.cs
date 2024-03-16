using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionCandidataDTO : BaseDTO
    {
        public int UrlAplicacionId { get; set; }
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
    }

    public class UrlFuenteDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class UrlAplicacionDTO: BaseDTO
    {
        public string Url { get; set; }
        public string CodigoAPT { get; set; }
        public string NombreAplicacion { get; set; }
        public int? EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public int UrlFuenteId { get; set; }
        public DateTime? FechaUltimaActualizacion { get; set; }
        public bool? FlagEditarAplicacion { get; set; }
        public string Comentario { get; set; }

        public string FechaUltimaActualizacionStr => FechaUltimaActualizacion.HasValue? FechaUltimaActualizacion.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string UrlFuenteIdStr => Utilitarios.GetEnumDescription2((EUrlFuente)UrlFuenteId);
    }

    public class UrlAplicacionEquipoDTO : BaseDTO
    {
        public int UrlAplicacionId { get; set; }
        public int? EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public string IP { get; set; }

        public bool? FlagTemporal { get; set; }
        public string Ubicacion { get; set; }

        public string TemporalToString
        {
            get
            {
                var strDescubrimiento = string.Empty;
                if (FlagTemporal.HasValue)
                {
                    if (FlagTemporal.Value)
                    {
                        strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Manual);
                    }
                    else
                    {
                        strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Automaticamente);
                    }
                }
                else
                {
                    strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Automaticamente);
                }

                return strDescubrimiento;
            }
        }
    }
}
