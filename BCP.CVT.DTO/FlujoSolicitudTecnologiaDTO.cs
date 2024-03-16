using System;

namespace BCP.CVT.DTO
{
    public class FlujoSolicitudTecnologiaDTO
    {
        public int? ConfiguracionTecnologiaCamposId { get; set; }
        public string NombreCampo { get; set; }
        public string TablaProcedencia { get; set; }
        public string DescripcionCampo { get; set; }
        public int? FlagActivo { get; set; } 
        public int? CorrelativoCampo { get; set; }
        public string ValorNuevo { get; set; }
        public string ValorAnterior { get; set; }
        public int? SolicitudTecnologiaId { get; set; }
        public int? FlagConfiguracion { get; set; }
    }
}
