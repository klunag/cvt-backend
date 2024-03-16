using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class ClasificacionTecnicaDTO : TreeElementDTO
	{
		public int? ClasificacionTecnicaId { get; set; }
	
		public string Descripcion { get; set; }
		public string Nombre { get; set; }

	}
}
