using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TecnologiaEquivalenciaDTO: BaseDTO
    {
        public int? TecnologiaId { get; set; }
        //public int? EquivalenciaId { get; set; }
        //public int? TablaProcedenciaId { get; set; }
        //public string Categoria { get; set; }
        public string Nombre { get; set; }
        //public string Version { get; set; }

        public string DominioTecnologia { get; set; }
        public string SubdominioTecnologia { get; set; }
        public string NombreTecnologia { get; set; }
        public string TipoTecnologia { get; set; }
        public int? EstadoId { get; set; }
        public string EstadoStr { get { return EstadoId.HasValue ? Utilitarios.GetEnumDescription2((ETecnologiaEstado)(EstadoId)) : ""; } }
        public int? TecnologiaEquivalenciaId { get; set; }
        public bool FlagActivo { get; set; }
    }
}
