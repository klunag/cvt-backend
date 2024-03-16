using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
	public class SolicitudArchivosDTO : BaseDTO
	{
		public int IdSolicitudArchivos { get; set; }
		public int? IdSolicitud { get; set; }
		public byte[] ConformidadGST { get; set; }
		public byte[] TicketEliminacion { get; set; }
		public byte[] Ratificacion { get; set; }
		public string NombreConformidadGST { get; set; }
		public string NombreTicketEliminacion { get; set; }
		public string NombreRatificacion { get; set; }
	}

}