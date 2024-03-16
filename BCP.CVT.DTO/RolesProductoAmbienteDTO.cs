using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;


namespace BCP.CVT.DTO
{
	public class RolesProductoAmbienteDTO : BaseDTO
	{
        public int? RolesProductoAmbienteId { get; set; }
        public int? RolesProductoId { get; set; }
        public int Ambiente { get; set; }
        public string AmbienteStr
        {
            get
            {
                var resultado = (Ambiente == null) ? string.Empty : Utilitarios.GetEnumDescription2((AmbienteRolProducto)Ambiente);
                return resultado;
            }
        }
        public string GrupoRed { get; set; }
        public bool? FlagActivo { get; set; }
    }
}
