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
    class ProcesoSvc : ProcesoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void AddOrEditProceso(ProcesoDTO registro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (registro.ProcesoId == 0)
                    {
                        var entidad = new Proceso()
                        {
                            TipoTareaId = registro.TipoTareaId,
                            ResultadoEjecucionId = registro.ResultadoEjecucionId,
                            FlagEjecutado = registro.FlagEjecutado,
                            FlagActivo = registro.Activo,
                            CreadoPor = registro.UsuarioCreacion,
                            FechaCreacion = registro.FechaCreacion
                        };
                        ctx.Proceso.Add(entidad);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        var entidad = (from u in ctx.Proceso
                                       where u.ProcesoId == registro.ProcesoId
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.ResultadoEjecucionId = registro.ResultadoEjecucionId;
                            entidad.LogResultados = registro.LogResultados;
                            entidad.LogErrores = registro.LogErrores;
                            entidad.FechaInicioEjecucion = registro.FechaInicioEjecucion;
                            entidad.FechaFinEjecucion = registro.FechaFinEjecucion;
                            entidad.FlagEjecutado = registro.FlagEjecutado;
                            entidad.ModificadoPor = registro.UsuarioModificacion;
                            entidad.FechaModificacion = registro.FechaModificacion;
                            ctx.SaveChanges();
                        }
                        else
                            throw new Exception(string.Format("Error en el metodo: registro.ProcesoId<{0}> no se encuentra registrado en la entidad ctx.Proceso.", registro.ProcesoId));
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: AddOrEditProceso(ProcesoDTO registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: AddOrEditProceso(ProcesoDTO registro)"
                    , new object[] { null });
            }
        }

        public override List<ProcesoDTO> GetAllProceso()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Proceso
                                   where u.FlagActivo
                                   && u.FlagEjecutado == false
                                   && u.ResultadoEjecucionId == (int)EResultadoEjecucion.Pendiente
                                   select new ProcesoDTO()
                                   {
                                       ProcesoId = u.ProcesoId,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.CreadoPor,
                                       TipoTareaId = u.TipoTareaId
                                   }).ToList();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: List<ProcesoDTO> GetAllProceso()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: List<ProcesoDTO> GetAllProceso()"
                    , new object[] { null });
            }
        }

        public override ProcesoDTO GetProceso(int? anio, int? mes, int? dia, int tipoTarea)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Proceso
                                  where
                                  (u.FechaCreacion.Year == anio.Value && u.FechaCreacion.Month == mes.Value && u.FechaCreacion.Day == dia.Value)
                                  || (null == anio && null == mes && null == dia)
                                  && u.FlagActivo
                                  && u.TipoTareaId == tipoTarea
                                  orderby u.FechaCreacion descending
                                  select new ProcesoDTO
                                  {
                                      ProcesoId = u.ProcesoId,
                                      ResultadoEjecucionId = u.ResultadoEjecucionId,
                                      LogResultados = u.LogResultados,
                                      LogErrores = u.LogErrores,
                                      FechaCreacion = u.FechaCreacion,
                                      FechaInicioEjecucion = u.FechaInicioEjecucion,
                                      FechaFinEjecucion = u.FechaFinEjecucion,
                                      FlagEjecutado = u.FlagEjecutado,
                                      UsuarioModificacion = u.ModificadoPor
                                  }).FirstOrDefault();

                    return itemBD;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: ProcesoDTO GetProceso(int? anio, int? mes, int? dia, int tipoTarea)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: ProcesoDTO GetProceso(int? anio, int? mes, int? dia, int tipoTarea)"
                    , new object[] { null });
            }
        }

        public override int ValidarProcesoPorDia(int anio, int mes, int dia, int tipoTarea)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int itemBD = (from u in ctx.Proceso
                                  where u.FechaCreacion.Year == anio
                                  && u.FechaCreacion.Month == mes
                                  && u.FechaCreacion.Day == dia
                                  && u.FlagActivo
                                  && u.TipoTareaId == tipoTarea
                                  select u).Count();

                    return itemBD;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: int ValidarProcesoPorDia(int anio, int mes, int dia, int tipoTarea)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProcesoDTO
                    , "Error en el metodo: int ValidarProcesoPorDia(int anio, int mes, int dia, int tipoTarea)"
                    , new object[] { null });
            }
        }
    }
}
