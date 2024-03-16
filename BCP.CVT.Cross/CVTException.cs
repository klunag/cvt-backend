using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross
{
    // RemedyException ---> CVTException
    /// <summary>
    /// Enumeración que contiene los ids de los tipos de RemedyException que puede lanzar
    /// </summary>
    public enum CVTExceptionIds : int
    {
        ErrorAgrupamientoDTO = 1,
        ErrorAmbienteDTO = 2,
        ErrorBaseMetadataDTO = 3,
        ErrorCodigoCISDTO = 4,
        ErrorConfiguracionDTO = 5,
        ErrorEstadoDTO = 6,
        ErrorLogAuditoriaDTO = 7,
        ErrorRelacionDTO = 8,
        ErrorUbicacionDTO = 9,
        ErrorAplicacionDTO = 10,
        ErrorCmdbDTO = 11,
        ErrorAdmin = 12,
        ErrorBigFix = 13,
        ErrorFisico = 14,
        ErrorDominioDTO = 15,
        ErrorProcesoDTO = 16,
        ErrorParametroDTO = 17,
        //ErrorRelacionDTO = 18,
        ErrorRoadMapDTO = 18,
        ErrorCriticidadDTO = 19,
        ErrorEstadoAplicacionDTO = 20,
        ErrorTecnologiaDTO = 21,
        ErrorEquipoDTO = 22,
        ErrorArquetipoDTO = 23,
        ErrorExcepcionDTO = 24,
        ErrorArchivoCVTDTO = 25,
        ErrorAlertaCVTDTO = 26,
        ErrorRelevanciaDTO = 27,
        ErrorGraficoDTO = 28,
        ErrorEntornoDTO = 29,
        ErrorFamiliaDTO = 30,
        ErrorSubdominioDTO = 31,
        ErrorTecnologiaNoRegistradaDTO = 32,
        ErrorTipoArquetipoDTO = 33,
        ErrorTipoDTO = 34,
        ErrorVistaDTO = 35,
        ErrorNotificacionDTO = 36,
        ErrorAuthDTO = 37,
        ErrorParametricasDTO = 38,
        ErrorUsuarioDTO = 39,
        ErrorActivosDTO = 40,
        ErrorAzureDTO = 41,
        ErrorRolesGestionDTO = 42,
        ErrorProductoDTO = 43,
        ErrorMotivoDTO = 44,
        ErrorTipoCicloVidaDTO = 45,
        ErrorBCPUnidades = 46,
        ErrorAssets = 47,
        ErrorDependencias = 48,
        ErrorRelacionesApps = 100
    }
    [Serializable]
    public class CVTException : Exception
    {
        /// <summary>
        /// Obtiene o establece el id del CredicorpExceptionIds lanzado
        /// </summary>
        public CVTExceptionIds Id { get; set; }

        /// <summary>
        /// Obtiene o establece el arreglo de datos adicionales que se pueden agregar al detalle de la excepción
        /// </summary>
        public object[] Datos { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public CVTException() { }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="id">Tipo de excepción</param>
        /// <param name="message">Mensaje informativo de la excepción</param>
        /// <param name="datos">Datos adicionales de la excepción</param>
        public CVTException(CVTExceptionIds id, string message, object[] datos) : this(id, message, datos, null) { }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="id">Tipo de excepción</param>
        /// <param name="message">Mensaje informativo de la excepción</param>
        /// <param name="datos">Datos adicionales de la excepción</param>
        /// <param name="inner">Excepción base generada</param>
        public CVTException(CVTExceptionIds id, string message, object[] datos, Exception inner)
            : base(message, inner)
        {
            Id = id;
            Datos = datos;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CVTException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
