using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class AplicacionPortaflioDTO : BaseDTO
	{
		public int AplicacionDetalleId { get; set; }
		public int AplicacionId { get; set; }
		public string Codigo { get; set; }
		public string Valor { get; set; }
		public string Tabla { get; set; }
	}
}
