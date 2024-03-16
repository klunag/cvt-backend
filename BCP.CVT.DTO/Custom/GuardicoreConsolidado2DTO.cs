using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class GuardicoreConsolidado2DTO
    {
        public string codigoapp { get; set; }
        public int idestado { get; set; }
        public string nombreestado { get; set; }
        public string nombreApps { get; set; }
        public int TotalApps { get; set; }
        public string equipo { get; set; }
        public int idambiente { get; set; }
        public DateTime? fecha { get; set; }
        public DateTime? fechaaumentada { get; set; }
        public int idequipo { get; set; }
        public string ips { get; set; }
        public string EquipoOrigen { get; set; }
        public string EquipoDestino { get; set; }
        public string IpOrigen { get; set; }
        public string IpDestino { get; set; }
        public int cantSiEsta { get; set; }
        public int cantNoEsta { get; set; }
        public string EstadoCVT { get; set; }
    }
    
    public class GuardicoreFase2DTO
    {
        public string dominio { get; set; }
        public string subdominio { get; set; }
        public string tecnologia { get; set; }
        public DateTime? fechaEscane { get; set; }
    }

    public class GuardicoreEtiquetado
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string etiqueta { get; set; }
        public string comentario { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
    public class GuardicoreServidorRelacionDTO
    {
        public int id { get; set; }
        public string etiqueta { get; set; }
        public string comodin { get; set; }
        public int prioridad { get; set; }
        public string comentario { get; set; }
        public int tipoaplicacionrelacion { get; set; }
        public string nombreTipoAplicacionRelacion
        {
            get
            {
                return Utilitarios.GetEnumDescription2((TipoAplicacionRelacionEtiquetado)tipoaplicacionrelacion);
            }
        }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
