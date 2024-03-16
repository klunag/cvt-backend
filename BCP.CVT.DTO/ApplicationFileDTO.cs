using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class ApplicationFileDTO
	{
		public int? IdApplicationFile { get; set; }
		public int? ApplicationId { get; set; }

		public byte[] ArchivoAsociado{ get; set; }

		public string Comentario { get; set; }

		public string Nombre { get; set; }
	}
}
