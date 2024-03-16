using System;
using BCP.CVT.Cross;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoInstanciaBdDTO 
    {
        public int Id { get; set; }
        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public string InstanciaBD { get; set; }
        public int Inventario { get; set; }
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
                if(UsuarioModificacion != "")
                    return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
                else
                    return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string InventarioStr => Utilitarios.GetEnumDescription2((EquipoInstanciaBD_Inventario)Inventario);
        public string Estado
        {
            get
            {
                if (FlagActivo)
                    return "Activo";
                else
                    return "Desactivado";
            }
        }
        public string CodigoApt { get; set; }
        public string NombreApt { get; set; }
        public string Usado { get; set; }
            

    }
}
