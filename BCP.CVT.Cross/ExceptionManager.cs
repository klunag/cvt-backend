using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross
{
    public sealed class ExceptionManager
    {
        private static readonly Lazy<ExceptionManager> lazy =
            new Lazy<ExceptionManager>(() => new ExceptionManager());

        /// <summary>
        /// Asociado a la entrada Errores.RutaArchivo en la sección appSettings
        /// </summary>
        private static string _RutaFichero = Settings.Get<string>("Errores.RutaArchivo");
        /// <summary>
        /// Asociado a la entrada Errores.RegistroEventos en la sección appSettings
        /// </summary>
        private static bool _RegistroHabilitado = Settings.Get<bool>("Errores.RegistroEventos");

        private static string _NombreFichero = Settings.Get<string>("Errores.NombreArchivo");

        public static ExceptionManager Instance { get { return lazy.Value; } }

        private ExceptionManager()
        {

        }

        public void ManageException(Exception ex)
        {
            if (_RegistroHabilitado)
            {
                ObtenerInformacionExcepcion(ex);
            }
        }

        private void ObtenerInformacionExcepcion(Exception ex)
        {
            if (ex.Message != "Thread was being aborted.")
            {
                FormatoMensaje mensaje = new FormatoMensaje();
                mensaje.Titulo = "Se ha producido un error en la aplicación";
                mensaje.Detalle = "Se ha producido un error en un componente de la aplicación.";
                mensaje.Acciones = "Revise el log de errores para un mayor detalle del mismo, y para que pueda iniciar acciones para su pronta corrección.";

                this.AgregarInformacionException(mensaje, ex);

                this.RegistrarArchivo(mensaje);
            }
        }

        private void AgregarInformacionException(FormatoMensaje mensaje
                , Exception excepcion)
        {
            int contExcepcionInterna = 0;
            Exception excepcionInterna;
            string tituloExcepcionInterna;

            mensaje.AgregarDatoAdicional("Exception.Type", excepcion.GetType().Name.ToString());
            mensaje.AgregarDatoAdicional("Exception.Message", excepcion.Message);
            mensaje.AgregarDatoAdicional("Exception.Source", excepcion.Source);

            excepcionInterna = excepcion.InnerException;
            while (excepcionInterna != null)
            {
                contExcepcionInterna++;

                tituloExcepcionInterna = string.Format("Exception.InnerException{0}", contExcepcionInterna);

                mensaje.AgregarDatoAdicional(tituloExcepcionInterna + ".Type", excepcion.InnerException.GetType().Name);
                mensaje.AgregarDatoAdicional(tituloExcepcionInterna + ".Message", excepcion.InnerException.Message.ToString());
                mensaje.AgregarDatoAdicional(tituloExcepcionInterna + ".Source", excepcion.InnerException.Source != null ? excepcion.InnerException.Source.ToString() : "");

                excepcionInterna = excepcionInterna.InnerException;
            }

            mensaje.AgregarDatoAdicional("Exception.StackTrace", excepcion.StackTrace.ToString());
        }

        private void RegistrarArchivo(FormatoMensaje mensaje)
        {
            string archivoLog = string.Format(_NombreFichero, System.DateTime.Now.Year.ToString(), System.DateTime.Now.Month.ToString("00"), System.DateTime.Now.Day.ToString("00"));
            string rutaArchivo = string.Concat(_RutaFichero, archivoLog);

            using (StreamWriter sw = new StreamWriter(rutaArchivo, true))
            {
                sw.WriteLine(mensaje.ToString());
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }
        }

        public void RegistrarLinea(string mensaje)
        {
            string archivoLog = string.Format(_NombreFichero, System.DateTime.Now.Year.ToString(), System.DateTime.Now.Month.ToString("00"), System.DateTime.Now.Day.ToString("00"));
            string rutaArchivo = string.Concat(_RutaFichero, archivoLog);

            using (StreamWriter sw = new StreamWriter(rutaArchivo, true))
            {
                sw.WriteLine(mensaje);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
