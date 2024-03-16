using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class SubClasificacionTecnicaDTO : TreeElementDTO
	{

		public int? SubClasificacionTecnicaId { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }

		public int ClasificacionTecnicaId { get; set; }
	}
}
