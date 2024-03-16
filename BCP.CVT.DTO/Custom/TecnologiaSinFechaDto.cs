using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class TecnologiaSinFechaDto
    {
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string ClaveTecnologia { get; set; }
        public string TipoTecnologia { get; set; }
        public int? FechaCalculoTec { get; set; }
        public int? FuenteId { get; set; }
        public bool? FlagSiteEstandar { get; set; }
        public bool? FlagFechaFinSoporte { get; set; }
        public int TotalComponentes { get; set; }

        public string SiteEstandarToString { get { return FlagSiteEstandar.HasValue ? (FlagSiteEstandar.Value ? "Sí" : "No") : ""; } }

        public string FuenteToString { get { return FuenteId.HasValue ? Utilitarios.GetEnumDescription2((Fuente)(FuenteId.Value)) : ""; } }

        public string FechaCalculoTecToString { get { return FechaCalculoTec.HasValue ? Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)(FechaCalculoTec.Value)) : ""; } }

        public string FechaFinToString
        {
            get
            {
                if (FlagFechaFinSoporte.HasValue)
                {
                    if (FlagFechaFinSoporte.Value)
                        return "Sin fecha asignada";
                    else
                        return "Con fecha de fin de soporte indefinida";
                }
                else
                    return string.Empty;
            }
        }

    }
}
