using System;
using BCP.CVT.Cross;
namespace BCP.CVT.DTO
{
    public class MotorReglasDTO
    {
        public int MotorReglaId { get; set; }
        public int ProcesoId { get; set; }
        public int PuertoId { get; set; }
        public int ProtocoloId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public int ProcesoTipo { get; set; }
        public string ProcesoTipoStr => Utilitarios.GetEnumDescription2((MotorReglaProcesoTipo)ProcesoTipo);
        public string Funcion { get; set; }
        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TotalFilas { get; set; }
        public string FechaCreacionStr
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string Proceso { get; set; }
        public string Puerto { get; set; }
        public string Protocolo { get; set; }
        public string Producto { get; set; }
        public string Estado
        {
            get
            {
                if (FlagActivo)
                {
                    return "Activo";
                } else
                {
                    return "Desactivado";
                }
            }
        }
    }

}
