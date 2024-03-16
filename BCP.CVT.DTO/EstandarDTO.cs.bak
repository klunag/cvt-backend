using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EstandarDTO : BaseDTO
    {
        public int TipoEstandarId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? PuntuacionServidor { get; set; }
        public int? PuntuacionEstacion { get; set; }

        public string TipoEstandarIdStr => Utilitarios.GetEnumDescription2((ETipoEstandarPortafolio)TipoEstandarId);
    }


    public class EstandarPortafolioDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public int SubdominioId { get; set; }
        public string TipoEstandar { get; set; }
        public int TipoTecnologiaId { get; set; }
        public string TipoTecnologiaStr { get; set; }
        public int? EstadoId { get; set; }

        //public int? PuntuacionServidor => TipoTecnologiaId == 1 && ((EstadoId == (int)ETecnologiaEstado.Vigente) || EstadoId == (int)ETecnologiaEstado.VigentePorVencer) ? 1 : 0;
        public int? PuntuacionServidor => TipoTecnologiaId == 1 && ((EstadoId == (int)ETecnologiaEstado.Vigente) || EstadoId == (int)ETecnologiaEstado.Deprecado) ? 1 : 0;
        //public int? PuntuacionEstacion => TipoTecnologiaId == 1 && ((EstadoId == (int)ETecnologiaEstado.Vigente) || EstadoId == (int)ETecnologiaEstado.VigentePorVencer) ? 1 : 0;
        public int? PuntuacionEstacion => TipoTecnologiaId == 1 && ((EstadoId == (int)ETecnologiaEstado.Vigente) || EstadoId == (int)ETecnologiaEstado.Deprecado) ? 1 : 0;

        public string EstadoStr => EstadoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId)) : "";

    }
}
