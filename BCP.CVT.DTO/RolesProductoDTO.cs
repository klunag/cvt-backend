using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
	public class RolesProductoDTO : BaseDTO
	{
        public int? ProductoId { get; set; }
        public string Rol { get; set; }
        public string GrupoRed { get; set; }
        public string ProductoNombre { get; set; }
        public int? RolesProductoId { get; set; }
        public int? FuncionesRelacionadas { get; set; }
        public string Descripcion { get; set; }
        public string Producto { get; set; }
        public int IdRolesProducto { get; set; }
        public string EstadoRol { get; set; }
        public int SolicitudId { get; set; }
        public bool? FlagActivo { get; set; }
        public bool? FlagEliminado { get; set; }
        public int? TipoCuenta { get; set; }
        public string TipoCuentaStr
        {
            get
            {
                var resultado = (TipoCuenta == null) ? string.Empty : Utilitarios.GetEnumDescription2((TipoCuenta)TipoCuenta);
                return resultado;
            }
        }
        public List<RolesProductoAmbienteDTO> Ambiente { get; set; }
    }
}
