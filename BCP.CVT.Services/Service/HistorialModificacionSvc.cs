using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class HistorialModificacionSvc : HistorialModificacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool RegistrarHistorialModificacion(HistorialModificacionDTO registro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool seRegistro = false;

                    HistorialModificacion entidad = new HistorialModificacion
                    {
                        Entidad = registro.Entidad,
                        Id = registro.IdRegistro,
                        EntidadAsociado = registro.EntidadAsociado,
                        AsociadoId = registro.IdRegistroAsociado,
                        Descripcion = registro.Descripcion,
                        Accion = registro.Accion,
                        CreadoPor = registro.UsuarioCreacion,
                        FechaCreacion = registro.FechaCreacion
                    };

                    ctx.HistorialModificacion.Add(entidad);

                    return seRegistro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool RegistrarHistorialModificacion(HistorialModificacionDTO registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool RegistrarHistorialModificacion(HistorialModificacionDTO registro)"
                    , new object[] { null });
            }
        }
    }
}
