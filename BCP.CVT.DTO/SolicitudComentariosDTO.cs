using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SolicitudComentariosDTO: BaseDTO
    {
        public int SolicitudAplicacionId { get; set; }
        public string Comentarios { get; set; }
        public int TipoComentarioId { get; set; }
        public int? BandejaId { get; set; }
        public string MatriculaResponsable { get; set; }

        public string TipoComentarioIdStr => Utilitarios.GetEnumDescription2((ETipoComentarioSolicitud)TipoComentarioId);
        public string BandejaAprobadorStr => BandejaId.HasValue ? $"B. {Utilitarios.GetEnumDescription2((EBandejaAprobadorAplicacion)BandejaId)} : {UsuarioCreacion}" : $"B. Administrador: {UsuarioCreacion}";
    }
}
