using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class ActivosDTO : BaseDTO
	{
		public int TipoActivoInformacionId { get; set; }
		public int? FlujoRegistro { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }
		public string FlujoRegistroNombre { get; set; }
		public bool? FlagUserIT { get; set; }

		public bool? FlagExterna { get; set; }
	}
}
