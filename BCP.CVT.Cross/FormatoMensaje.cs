using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross
{
    internal class FormatoMensaje
    {
        /* ATRIBUTOS */
        private StringBuilder datosAdicionales = new StringBuilder();

        /* METODOS */

        /// <summary>
        /// Obtener o establecer el título del Error
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Obtener o establecer el detalle
        /// </summary>
        public string Detalle { get; set; }

        /// <summary>
        /// Obtener o establecer las acciones a seguir.
        /// </summary>
        public string Acciones { get; set; }

        /// <summary>
        /// Forma el mensaje siguiendo un formato establecido
        /// </summary>
        /// <remarks>
        /// Para evitar problemas de interpretación con las comillas de valor 
        /// se reemplaza el caracter comillas (") por barra invertida con comillas(\").
        /// </remarks>
        /// <returns>Cadena con el contenido del mensaje en el formato definido</returns>
        public override string ToString()
        {
            Titulo = string.IsNullOrWhiteSpace(Titulo) ? "" : Titulo;
            string resultado = string.Format(
                    "[Datos Generales]\r\n" +
                    "Título=\"{0}\"\r\n" +
                    "Detalle=\"{1}\"\r\n" +
                    "Acciones=\"{2}\"\r\n" +
                    "[Datos Adicionales]\r\n" +
                    "{3}",
                    Titulo.Replace("\"", "\\\""), Detalle, Acciones, datosAdicionales.ToString());

            return resultado;
        }

        /// <summary>
        /// Agrega informacion en la sección de datos adicionales
        /// </summary>
        /// <param name="nombre">Nombre del dato adicional</param>
        /// <param name="valor">Valor del dato adicional</param>
        public void AgregarDatoAdicional(string nombre, string valor)
        {
            datosAdicionales.Append(string.Format("{0}=\"{1}\"\r\n", nombre, valor));
        }

        /// <summary>
        /// Agrega un título en la sección de datos adicionales
        /// </summary>
        /// <param name="Titulo">Título a agregar</param>
        public void AgregarTituloDatoAdicional(string Titulo)
        {
            datosAdicionales.Append(string.Format("\r\n[{0}]\r\n", Titulo));
        }
    }
}
