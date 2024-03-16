using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class SolicitudFlujoDTO : BaseDTO
    {
        public long? SolicitudTecnologiaId { get; set; }
        public string Tecnologia { get; set; }
        public string CodigoProducto { get; set; }
        public int? TecnologiaId { get; set; }
        public string Dominio { get; set; }
        public int? DominioId { get; set; }
        public string SubDominio { get; set; }
        public int? SubDominioId { get; set; }
        public int? EstadoSolicitud { get; set; }
        public string EstadoSolicitudStr
        {
            get
            {

                return Utilitarios.GetEnumDescription2((FlujoEstadoSolicitud)EstadoSolicitud);
            }
        }
        public DateTime? FechaModificacion { get; set; }
        public string FechaModificacionStr => FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy") : string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionStr => FechaCreacion.HasValue ? FechaCreacion.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string ModificadoPor { get; set; }
        public string CreadoPor { get; set; }
        public int? IdTipoSolicitud { get; set; }
        public string TipoSolicitudStr
        {
            get
            {
                return Utilitarios.GetEnumDescription2((FlujoTipoSolicitud)IdTipoSolicitud);
            }
        }
        public string Codigo { get; set; }
        public int? TipoFlujo { get; set; }
        public string UsuarioSolicitante { get; set; }
        public string ResponsableBuzon { get; set; }
        public string ResponsableMatricula { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public string OwnerDestino { get; set; }
    }
    public class SolicitudFlujoDetalleDTO : BaseDTO
    {
        public string NombreCampo { get; set; }
        public string TablaProcedencia { get; set; }
        public string RolAprueba { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public int? SolicitudTecnologiaCamposId { get; set; }
        public int? ConfiguracionTecnologiaCamposId { get; set; }
        public int? EstadoCampo { get; set; }
        public int? CorrelativoCampo { get; set; }
        public int? FlagConfiguration { get; set; }
    }

    public class TipoFlujoConfiguracion
    {
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
    }
}
