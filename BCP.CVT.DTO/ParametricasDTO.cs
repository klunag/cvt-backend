using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class ParametricasDTO : BaseDTO
	{
		public int ParametricaDetalleId { get; set; }
		public int ParametricaId { get; set; }
		public string Descripcion { get; set; }
		public string Valor { get; set; }
		public string Tabla { get; set; }
	}
}
