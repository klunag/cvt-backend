using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ModeloDTO
    {
        public int idmodelo { get; set; }
        public string nombre { get; set; }
        public string fabricante { get; set; }
        public bool FlagFechaFinSoporte { get; set; }
        public string FlagFinSoporte { get; set; }
        public bool FlagTemporal { get; set; }
        public string FlagTemporalToString { get; set; }
        public DateTime? FechaFinSoporteExtendida { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaInterna { get; set; }
        public string TipoFechaInterna { get; set; }
        public string ComentarioFechaFinSoporte { get; set; }
        public string MotivoFechaIndefinida { get; set; }
        public string urlFechaIndefinida { get; set; }
        public string Usuario { get; set; }
        public string UsuarioModificacion { get; set; }
        public int tipoEquipoId { get; set; }
        public string tipoEquipoNombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public string UrlSharepoint { get; set; }
        public string UrlLineamiento { get; set; }
        public int Remedy { get; set; }
        public string RemedyNombre { get; set; }
        public string FlagActivo { get; set; }
        public string capacidades { get; set; }
        public int tipofechacalculo { get; set; }
        public string TipoFechaCalculoToString
        {
            get
            {
                if (tipofechacalculo <= 0) return null;
                return Utilitarios.GetEnumDescription2((FechaCalculoTecnologia)tipofechacalculo);
            }
        }

        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string Item { get; set; }

        public string FechaCreacionToString { get; set; }
        public string FechaFinToString { get; set; }
        public int totalRows { get; set; }

        public int indicador { get; set; }
        public int indicadorproyectado { get; set; }
        public int indicadorproyectado2 { get; set; }

        public string Equipo { get; set; }
        public string FechaUltimoEscaneoCorrecto { get; set; }
        public string EstadoEquipo { get; set; }
        public string IP { get; set; }

        public string CodigoModelo
        {
            get
            {
                return idmodelo.ToString("000000");
            }
        }
        public string NumeroSerie { get; set; }
        public string CodigoEquipo { get; set; }
        public string Owner { get; set; }
        public string OwnerContacto { get; set; }

        public DateTime? FechaDespliegue { get; set; }
        public DateTime? FechaDisponible { get; set; }
        public EquipoDTO Equipos { get; set; }
    }
}
