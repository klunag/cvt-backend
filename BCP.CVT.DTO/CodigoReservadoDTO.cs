using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.PAPP.Common.Cross;
using BCP.CVT.Cross;

namespace BCP.CVT.DTO
{
    public class CodigoReservadoDTO : BaseDTO
    {
        public int CodigoReservadoId { get; set; }
        public int CodigoReutilizadoId { get; set; }
        
        public int? TipoCodigo { get; set; }
        public string Codigo { get; set; }

        public string Comentarios { get; set; }
        public bool? FlagActivo { get; set; }

        public bool? FlagEliminado { get; set; }

        public string TipoCodigoStr => Utilitarios.GetEnumDescription2((TipoCodigoReservado)TipoCodigo);


    }
}
