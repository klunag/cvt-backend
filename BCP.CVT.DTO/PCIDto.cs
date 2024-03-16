using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class PCIDto : BaseDTO
	{
		public int TipoPCIId { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }

		public bool? FlagActivo { get; set; }
		public DateTime? FechaCeacionPCI { get; set; }

	}
}
