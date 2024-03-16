using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class GestionUmbralesDTO : BaseDTO
    {
        public int UmbralId { get; set; }
        public int TipoUmbralId { get; set; }
        public string TipoDescripcion { get; set; }
        public int TipoValorId { get; set; }
        public string TipoValorDescripcion { get; set; }
        public string ValorUmbral { get; set; }
        public int indicador { get; set; }
        public string RefEvidencia { get; set; }
        public int TotalRows { get; set; }
        public bool FlagActivo { get; set; }
        public string FlagActivoStr
        {
            get
            {
                return (FlagActivo ? "Vigente" : "Historico");
            }
        }
        public int? UmbralTecnologiaId { get; set; }
        public int? UmbralEquipoId { get; set; }
        public string UmbralDetalle { get; set; }
        public string UmbralClaveTecnologica { get; set; }
        public byte[] ArchivoEvidencia { get; set; }
        public string UmbralAppId { get; set; }
        public string ValorAspiracional { get; set; }
        public int isOwnerRecord { get; set; }
    }
    public class GestionUmbralesDetalleDTO : BaseDTO
    {
        public int UmbralDetalleId { get; set; }
        public string UmbralClaveTecnologica { get; set; }
        public int DriverId { get; set; }
        public string DriverNombre { get; set; }
        public int UnidadMedidaId { get; set; }
        public string UnidadMedidaNombre { get; set; }
        public string Valor { get; set; }
        public int TotalRows { get; set; }
        public bool FlagActivo { get; set; }
        public string FlagActivoStr
        {
            get
            {
                return (FlagActivo ? "Activo" : "Inactivo");
            }
        }

        public int UmbralId { get; set; }
        public string UmbralTipoDescripcion { get; set; }
        public string UmbralTipoValorDescripcion { get; set; }
        public string UmbralValorUmbral { get; set; }
    }


    public class GestionComponenteCrossDTO : BaseDTO
    {
        public string codigoAppApi { get; set; }
        public string matricula { get; set; }
        public string tipoCross { get; set; }
    }


    public class GestionPeakDTO : BaseDTO
    {
        public int PeakId { get; set; }

        [StringLength(4)]
        public string CodigoAPT { get; set; }

        public int? EquipoId { get; set; }
        public int TipoValorId { get; set; }
        public string ValorPeak { get; set; }
        public string DetallePeak { get; set; }
        public bool FlagActivo { get; set; }
        public int Estado { get; set; }
        public string FlagActivoDesc { get; set; }
        public string TipoValorDesc { get; set; }
        //public string Estado { get; set; }
        public string Fecha { get; set; }
        public int TotalRows { get; set; }
        public string NombreCross { get; set; }
        public string ValorAspiracional { get; set; }
    }

    public class ExportRequestGU
    {
        /* [Range(0,999.9999)]*/
        public int EquipoId { get; set; }
        [StringLength(4)]
        public string CodigoAPT { get; set; }
    }
}
