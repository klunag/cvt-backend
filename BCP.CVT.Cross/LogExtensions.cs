using log4net;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace BCP.CVT.Cross
{
    public static class LogExtensions
    {
        public static void ErrorEntity(this ILog log, DbEntityValidationException excepcionEntity)
        {
            string tituloEntityError;
            FormatoMensaje mensajeError = new FormatoMensaje();
            mensajeError.AgregarDatoAdicional("SqlException.Source",
                excepcionEntity.Source);
            mensajeError.AgregarDatoAdicional("SqlException.HelpLink",
                excepcionEntity.HelpLink);
            mensajeError.AgregarDatoAdicional("SqlException.StackTrace",
                excepcionEntity.StackTrace.ToString());
            mensajeError.AgregarDatoAdicional("SqlException.Message",
                excepcionEntity.Message);
            mensajeError.AgregarDatoAdicional("SqlException.HResult",
                excepcionEntity.HResult.ToString());
            mensajeError.AgregarDatoAdicional("SqlException.Data",
                excepcionEntity.Data.ToString());

            foreach (DbEntityValidationResult errors in excepcionEntity.EntityValidationErrors)
            {
                tituloEntityError = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    errors.Entry.Entity.GetType().Name, errors.Entry.State);
                foreach (var validationError in errors.ValidationErrors)
                {
                    mensajeError.AgregarDatoAdicional(tituloEntityError + string.Format("- Property: \"{0}\", Error: \"{1}\"",
                        validationError.PropertyName, validationError.ErrorMessage), "DbEntityValidationResult");
                }
            }

            if (mensajeError != null)
                log.ErrorFormat("Error en el Acceso a Datos: {0}", mensajeError == null ? "" : mensajeError.ToString());
        }

    }
}
