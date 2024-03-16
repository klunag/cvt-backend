using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class ConsultaDTO : BaseDTO
	{
		public int ConsultaId { get; set; }
		public DateTime? FechaConsulta { get; set; }
		public int? TipoConsulta { get; set; }
		public string Consulta { get; set; }

		public string ConsultaUsuario { get; set; }
		public DateTime? FechaRespuesta { get; set; }
		public string RespuestaPortafolio { get; set; }
		public string NombreUsuarioConsultor { get; set; }
		public string MatriculaUsuarioConsultor { get; set; }
		public string applicationId { get; set; }

		public int? Respondido { get; set; }

		public string RespondidoName { get; set; }
		public string EmailUsuarioConsultor
		{
			get; set;

		}
		public string TipoConsultaName
		{
			get; set;

		}
		public string FechaConsultaF { get; set; }
		public string FechaRespuestaF { get; set; }
	}
}
