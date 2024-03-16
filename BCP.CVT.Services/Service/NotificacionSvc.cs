using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    partial class NotificacionSvc : NotificacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override bool ActualizarNotificacionEstado(int idNotificacion, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Notificacion
                                   where u.NotificacionId == idNotificacion
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.Activo = estado;
                        entidad.FlagEnviado = estado;
                        entidad.FechaEnvio = DateTime.Now;
                        entidad.UsuarioModificacion = usuario;
                        entidad.FechaModificacion = DateTime.Now;
                        ctx.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
        }

        public override List<NotificacionDTO> ListarNotificaciones(bool? flagEstado = null)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Notificacion
                                     where u.FlagEnviado == null && u.FechaEnvio == null && (u.Activo == flagEstado || flagEstado == null)
                                     select new NotificacionDTO()
                                     {
                                         Id = u.NotificacionId,
                                         TipoNotificacionId = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         De = u.De,
                                         Para = u.Para,
                                         CC = u.CC,
                                         BCC = u.BCC,
                                         Cuerpo = u.Cuerpo,
                                         Asunto = u.Asunto
                                     });
                    return registros.ToList();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditNotificacion(NotificacionDTO objRegistro)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new Notificacion()
                        {
                            TipoNotificacionId = objRegistro.TipoNotificacionId,
                            Nombre = objRegistro.Nombre,
                            De = objRegistro.De,
                            Para = objRegistro.Para,
                            CC = objRegistro.CC,
                            BCC = objRegistro.BCC,
                            Cuerpo = objRegistro.Cuerpo,
                            Asunto = objRegistro.Asunto,
                            MatriculaResponsable = objRegistro.Matricula,
                            FlagEnviado = objRegistro.FlagEnviado,
                            FechaEnvio = objRegistro.FechaEnvio,
                            UsuarioCreacion = objRegistro.UsuarioCreacion,
                            FechaCreacion = FECHA_ACTUAL,
                            Activo = true
                        };
                        ctx.Notificacion.Add(entidad);
                        ctx.SaveChanges();
                        ID = entidad.NotificacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Notificacion
                                       where u.NotificacionId == objRegistro.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.TipoNotificacionId = objRegistro.TipoNotificacionId;
                            entidad.Nombre = objRegistro.Nombre;
                            entidad.De = objRegistro.De;
                            entidad.Para = objRegistro.Para;
                            entidad.CC = objRegistro.CC;
                            entidad.BCC = objRegistro.BCC;
                            entidad.Cuerpo = objRegistro.Cuerpo;
                            entidad.Asunto = objRegistro.Asunto;
                            entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.Activo = false;
                            ctx.SaveChanges();
                            ID = entidad.NotificacionId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditNotificacionApp(NotificacionAplicacionDTO objRegistro)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //if (objRegistro.Id == 0)
                    //{
                    //    var entidad = new NotificacionApp()
                    //    {
                    //        TipoNotificacionId = objRegistro.TipoNotificacionId,
                    //        Nombre = objRegistro.Nombre,
                    //        De = objRegistro.De,
                    //        Para = objRegistro.Para,
                    //        CC = objRegistro.CC,
                    //        BCC = objRegistro.BCC,
                    //        Cuerpo = objRegistro.Cuerpo,
                    //        Asunto = objRegistro.Asunto,
                    //        //MatriculaResponsable = objRegistro.Matricula,
                    //        FlagEnviado = objRegistro.FlagEnviado,
                    //        FechaEnvio = objRegistro.FechaEnvio,
                    //        UsuarioCreacion = objRegistro.UsuarioCreacion,
                    //        FechaCreacion = FECHA_ACTUAL,
                    //        Activo = true,
                    //        //SolicitudAplicacionId = objRegistro.SolicitudId
                    //    };
                    //    ctx.NotificacionApp.Add(entidad);
                    //    ctx.SaveChanges();
                    //    ID = entidad.NotificacionId;
                    //}
                    //else
                    //{
                    //    var entidad = (from u in ctx.NotificacionApp
                    //                   where u.NotificacionId == objRegistro.Id
                    //                   select u).FirstOrDefault();
                    //    if (entidad != null)
                    //    {
                    //        entidad.TipoNotificacionId = objRegistro.TipoNotificacionId;
                    //        entidad.Nombre = objRegistro.Nombre;
                    //        entidad.De = objRegistro.De;
                    //        entidad.Para = objRegistro.Para;
                    //        entidad.CC = objRegistro.CC;
                    //        entidad.BCC = objRegistro.BCC;
                    //        entidad.Cuerpo = objRegistro.Cuerpo;
                    //        entidad.Asunto = objRegistro.Asunto;
                    //        entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                    //        entidad.FechaModificacion = FECHA_ACTUAL;
                    //        entidad.Activo = false;
                    //        ctx.SaveChanges();
                    //        ID = entidad.NotificacionId;
                    //    }
                    //}
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int AddOrEditNotificacionApp(NotificacionAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int AddOrEditNotificacionApp(NotificacionAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacion
                                     where (u.Activo == flagEstado || flagEstado == null)
                                     select new TipoNotificacionDto()
                                     {
                                         Id = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         BuzonSalida = u.BuzonSalida,
                                         Para = u.Para,
                                         Frecuencia = u.Frecuencia                                         
                                     });
                    return registros.ToList();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditTipoNotificacion(TipoNotificacionDto objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var entidad = (from u in ctx.TipoNotificacion
                                   where u.TipoNotificacionId == objRegistro.Id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        //entidad.Nombre = objRegistro.Nombre;
                        //entidad.Descripcion = objRegistro.Descripcion;
                        entidad.BuzonSalida = objRegistro.BuzonSalida;
                        entidad.Para = objRegistro.Para;
                        entidad.ConCopia = objRegistro.ConCopia;
                        entidad.Asunto = objRegistro.Asunto;
                        entidad.Cuerpo = objRegistro.Cuerpo;
                        entidad.Frecuencia = objRegistro.Frecuencia;
                        entidad.FechaInicio = objRegistro.FechaInicio;
                        entidad.HoraEnvio = objRegistro.HoraEnvio;
                        entidad.FechaModificacion = DateTime.Now;
                        entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                        entidad.CuerpoAlternativo = objRegistro.CuerpoAlternativo;

                        ctx.SaveChanges();
                        ID = entidad.TipoNotificacionId;
                    }

                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoNotificacion
                                   where u.TipoNotificacionId == idTipoNotificacion
                                   select new TipoNotificacionDto
                                   {
                                       Id = u.TipoNotificacionId,
                                       Descripcion = u.Descripcion,
                                       Nombre = u.Nombre,
                                       Para = u.Para,
                                       Frecuencia = u.Frecuencia,
                                       Asunto = u.Asunto,
                                       ConCopia = u.ConCopia,
                                       Cuerpo = u.Cuerpo,
                                       FechaInicio = u.FechaInicio,
                                       HoraEnvio = u.HoraEnvio,
                                       BuzonSalida = u.BuzonSalida,
                                       Activo = u.Activo,
                                       CuerpoAlternativo = u.CuerpoAlternativo
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion)"
                    , new object[] { null });
            }
        }
         
        public override List<AplicacionResponsableDto> ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AplicacionPortafolioResponsables
                                     where u.PortafolioResponsableId == idPortafolioResponsable
                                     select new AplicacionResponsableDto()
                                     {
                                         Matricula = u.Matricula,
                                         Colaborador = u.Colaborador,
                                         PortafolioResponsableId = u.PortafolioResponsableId,
                                     }).Distinct().ToList();

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder).ToList();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> ListarPortafolioResponsable()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.PortafolioResponsables 
                                     select new CustomAutocomplete()
                                     {
                                         Id = u.PortafolioResponsableId.ToString(),
                                         Descripcion = u.Nombre
                                     }).Distinct();
                    return registros.ToList();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> ListarPortafolioResponsable"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> ListarPortafolioResponsable"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> ListarNotificacionesResponsablesAplicacionesDetalle(int idPortafolioResponsable, string matricula, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AplicacionPortafolioResponsables
                                     join b in ctx.Aplicacion on u.CodigoAPT equals b.CodigoAPT
                                     where u.PortafolioResponsableId == idPortafolioResponsable
                                     && u.Matricula.ToUpper() == matricula.ToUpper()
                                     select new AplicacionDTO()
                                     {
                                        CodigoAPT = u.CodigoAPT,
                                        Nombre = b.Nombre,
                                        EstadoAplicacion = b.EstadoAplicacion,

                                     }).ToList();

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder).ToList();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override NotificacionResponsableAplicacionDto ObtenerNotificacionResponsableAplicacion(int idPortafolioResponsable, bool flagTTLJdE)
        {
            try
            {
                var entidad = new NotificacionResponsableAplicacionDto();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    entidad = (from u in ctx.NotificacionResponsableAplicacion
                               where u.Activo
                               && u.PortafolioResponsableId == idPortafolioResponsable
                               //&& !u.FlagEnvio
                               && (u.FlagNotificacionTTLJdE.Value == flagTTLJdE)
                               orderby u.FechaInicio descending
                               select new NotificacionResponsableAplicacionDto()
                               {
                                   Id = u.NotificacionResponsableAplicacionId,
                                   BuzonSalida = u.BuzonSalida,
                                   ConCopia = u.ConCopia,
                                   Asunto = u.Asunto,
                                   Cuerpo = u.Cuerpo,
                                   Activo = u.Activo,
                                   FechaInicio = u.FechaInicio,
                                   HoraEnvio = u.HoraEnvio,
                                   FlagEnvio = u.FlagEnvio,
                                   PortafolioResponsableId = u.PortafolioResponsableId,
                                   FlagNotificacionTTLJdE = u.FlagNotificacionTTLJdE
                               }).FirstOrDefault();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> ListarPortafolioResponsable"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> ListarPortafolioResponsable"
                    , new object[] { null });
            }
        }

        public override int AddOrEditNotificacionResponsableAplicacion(NotificacionResponsableAplicacionDto objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new NotificacionResponsableAplicacion()
                        {
                            PortafolioResponsableId = objRegistro.PortafolioResponsableId,
                            BuzonSalida = objRegistro.BuzonSalida,
                            ConCopia = objRegistro.ConCopia,
                            Cuerpo = objRegistro.Cuerpo,
                            Asunto = objRegistro.Asunto,
                            FlagEnvio = false,
                            FechaInicio = objRegistro.FechaInicio,
                            HoraEnvio = objRegistro.HoraEnvio,
                            UsuarioCreacion = objRegistro.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Activo = true,
                            FlagNotificacionTTLJdE = objRegistro.FlagNotificacionTTLJdE
                        };
                        ctx.NotificacionResponsableAplicacion.Add(entidad);
                        ctx.SaveChanges();
                        ID = entidad.NotificacionResponsableAplicacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.NotificacionResponsableAplicacion
                                       where u.NotificacionResponsableAplicacionId == objRegistro.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            
                            entidad.BuzonSalida = objRegistro.BuzonSalida;
                            entidad.ConCopia = objRegistro.ConCopia;
                            entidad.FechaInicio = objRegistro.FechaInicio;
                            entidad.HoraEnvio = objRegistro.HoraEnvio;
                   
                            entidad.Cuerpo = objRegistro.Cuerpo;
                            entidad.Asunto = objRegistro.Asunto;
                            entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                            entidad.FechaModificacion = DateTime.Now;
                            
                            ctx.SaveChanges();
                            ID = entidad.NotificacionResponsableAplicacionId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<TipoNotificacionDto> ListarNotificacionesFrecuentes()
        {
            var lista = new List<int>()
            {
                (int)ETipoNotificacion.EquiposSinTecnologias,
                (int)ETipoNotificacion.ServiciosEnNubeHuerfanos,
                (int)ETipoNotificacion.ServidoresHuerfanos,
                (int)ETipoNotificacion.SincronizacionResponsables,
                (int)ETipoNotificacion.TecnologiaNoRegistrada,
                (int)ETipoNotificacion.TecnologiaPendiente,
                (int)ETipoNotificacion.TecnologiasHuerfanas,
                (int)ETipoNotificacion.TecnologiaSinFecha,
                (int)ETipoNotificacion.AlertaNoCritica,
                (int)ETipoNotificacion.AlertaEstandares, //TODO
                (int)ETipoNotificacion.AlertaTecnologia,
                (int)ETipoNotificacion.ValidacionTecnicaExpertos,
                (int)ETipoNotificacion.RiesgoObsolescenciaAplicaciones
            };
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacion
                                     where lista.Contains(u.TipoNotificacionId) && u.Activo
                                     select new TipoNotificacionDto()
                                     {
                                         Id = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         BuzonSalida = u.BuzonSalida,
                                         Para = u.Para,
                                         Frecuencia = u.Frecuencia,
                                         ConCopia = u.ConCopia,
                                         Cuerpo = u.Cuerpo,
                                         FechaInicio = u.FechaInicio,
                                         HoraEnvio = u.HoraEnvio
                                     }).ToList();
                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
        }

        public override void ActualizarFechaUltimoEnvio(int tipoNotificacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacion
                                     where u.TipoNotificacionId == tipoNotificacion
                                     select u).First();
                    if (registros != null)
                    {
                        registros.FechaUltimoEnvio = DateTime.Now;
                        registros.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: ActualizarFechaUltimoEnvio(int tipoNotificacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: ActualizarFechaUltimoEnvio(int tipoNotificacion)"
                    , new object[] { null });
            }
        }

        public override List<NotificacionDTO> ListarNotificacionesPendientesEnvio()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Notificacion
                                     where u.FlagEnviado == false
                                     select new NotificacionDTO()
                                     {
                                         Id = u.NotificacionId,
                                         TipoNotificacionId = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         De = u.De,
                                         Para = u.Para,
                                         CC = u.CC,
                                         BCC = u.BCC,
                                         Cuerpo = u.Cuerpo,
                                         Asunto = u.Asunto,
                                         Matricula = u.MatriculaResponsable
                                     }).Take(250).ToList();

                    //var registroResponsableAplicacion = (from u in ctx.NotificacionResponsableAplicacion
                    //                                     where !u.FlagEnvio
                    //                                     select new NotificacionDTO()
                    //                                     {
                    //                                         Id = 0,
                    //                                         NotificacionResponsableAplicacionId = u.NotificacionResponsableAplicacionId,
                    //                                         TipoNotificacionId = (int)ETipoNotificacion.RiesgoObsolescenciaAplicaciones,
                    //                                     }).ToList();

                    //if(registroResponsableAplicacion != null && registroResponsableAplicacion.Count() > 0)
                    //    registros.AddRange(registroResponsableAplicacion);

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
        }

        public override List<TipoNotificacionDto> ListarNotificacionesDiarias()
        {
            var lista = new List<int>()
            {
                (int)ETipoNotificacion.AlertaCritcaSNOWADDM,
                (int)ETipoNotificacion.AlertaCriticaPortafolio,
                (int)ETipoNotificacion.AlertaIP,
                (int)ETipoNotificacion.AlertaRecursosAzure
            };
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacion
                                     where lista.Contains(u.TipoNotificacionId) && u.Activo
                                     select new TipoNotificacionDto()
                                     {
                                         Id = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         BuzonSalida = u.BuzonSalida,
                                         Para = u.Para,
                                         Frecuencia = u.Frecuencia,
                                         ConCopia = u.ConCopia,
                                         Cuerpo = u.Cuerpo,
                                         FechaInicio = u.FechaInicio,
                                         HoraEnvio = u.HoraEnvio
                                     }).ToList();
                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarNotificacionesDiarias()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarNotificacionesDiarias()"
                    , new object[] { null });
            }
        }

        public override List<TipoNotificacionDto> ListarNotificacionesAppDiarias()
        {
            var lista = new List<int>()
            {
                (int)ETipoNotificacionAplicacion.AprobacionSolicitudCreacion,
                (int)ETipoNotificacionAplicacion.AprobacionSolicitudModificacion,
                (int)ETipoNotificacionAplicacion.AprobacionSolicitudEliminacion,
                (int)ETipoNotificacionAplicacion.CambioEstadoArquitecturaTI,
                (int)ETipoNotificacionAplicacion.CambioEstadoClasificacion,
                (int)ETipoNotificacionAplicacion.CambioEstadoDevOps,
                (int)ETipoNotificacionAplicacion.CambioEstadoLiderUsuario,
                (int)ETipoNotificacionAplicacion.CambioEstadoTTL,
                (int)ETipoNotificacionAplicacion.CambioEstadoUserIT,
                (int)ETipoNotificacionAplicacion.NotificacionBuzonesCreacion,
                (int)ETipoNotificacionAplicacion.NotificacionBuzonesEliminacion,
                (int)ETipoNotificacionAplicacion.RegistroSolicitudCreacion,
                (int)ETipoNotificacionAplicacion.RegistroSolicitudModificacion,
                (int)ETipoNotificacionAplicacion.RegistroSolicitudEliminacion
            };
            try
            {
                //using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                //{
                //    var registros = (from u in ctx.TipoNotificacionApp
                //                     where lista.Contains(u.TipoNotificacionId) && u.Activo
                //                     select new TipoNotificacionDto()
                //                     {
                //                         Id = u.TipoNotificacionId,
                //                         Nombre = u.Nombre,
                //                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                //                         Asunto = u.Asunto,
                //                         Descripcion = u.Descripcion,
                //                         BuzonSalida = u.BuzonSalida,
                //                         Para = u.Para,
                //                         Frecuencia = u.Frecuencia,
                //                         ConCopia = u.ConCopia,
                //                         Cuerpo = u.Cuerpo,
                //                         FechaInicio = u.FechaInicio,
                //                         HoraEnvio = u.HoraEnvio
                //                     }).ToList();
                //    return registros;
                //}
                return null;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarNotificacionesAppDiarias()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarNotificacionesAppDiarias()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionPortafolioResponsablesDTO> ListarNotificacionesResponsablesTTLJdE(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var registros = new List<AplicacionPortafolioResponsablesDTO>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (idPortafolioResponsable == (int)EPortafolioResponsable.TTL)
                    {
                        registros = (from u in ctx.AplicacionPortafolioResponsables
                                     join b in ctx.Aplicacion on u.CodigoAPT equals b.CodigoAPT
                                     where u.PortafolioResponsableId == idPortafolioResponsable
                                     && b.FlagActivo && b.TribeTechnicalLead != "NO APLICA" && b.TribeTechnicalLead != ""
                                     && b.EstadoAplicacion != "Eliminada"
                                     group u by new { u.Matricula, u.Colaborador } into grp
                                     select new AplicacionPortafolioResponsablesDTO
                                     {
                                         Matricula = grp.Key.Matricula,
                                         Colaborador = grp.Key.Colaborador,
                                         NroAplicaciones = grp.Count()
                                     }).ToList();
                    }
                    else if(idPortafolioResponsable == (int)EPortafolioResponsable.JdE)
                    {
                        registros = (from u in ctx.AplicacionPortafolioResponsables
                                     join b in ctx.Aplicacion on u.CodigoAPT equals b.CodigoAPT
                                     where u.PortafolioResponsableId == idPortafolioResponsable
                                     && b.FlagActivo && b.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner != "NO APLICA" && b.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner != ""
                                     && b.EstadoAplicacion != "Eliminada"
                                     group u by new { u.Matricula, u.Colaborador } into grp
                                     select new AplicacionPortafolioResponsablesDTO
                                     {
                                         Matricula = grp.Key.Matricula,
                                         Colaborador = grp.Key.Colaborador,
                                         NroAplicaciones = grp.Count()
                                     }).ToList();
                    }

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder).ToList();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<NotificacionResponsableAplicacionDto> ListarNotificacionesResponsableAplicacion()
        {
            try
            {
                var data = new List<NotificacionResponsableAplicacionDto>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.NotificacionResponsableAplicacion
                                     join b in ctx.AplicacionPortafolioResponsables on u.PortafolioResponsableId equals b.PortafolioResponsableId
                                     where u.FlagEnvio == false
                                     group new { u, b } by new
                                     {
                                         u.NotificacionResponsableAplicacionId,
                                         u.PortafolioResponsableId,
                                         u.BuzonSalida,
                                         u.ConCopia,
                                         u.FechaInicio,
                                         u.HoraEnvio,
                                         u.Cuerpo,
                                         u.Asunto,
                                         u.FlagNotificacionTTLJdE,
                                         b.Matricula,
                                         b.CorreoElectronico
                                     } into grp
                                     select new NotificacionResponsableAplicacionDto()
                                     {
                                         Id = grp.Key.NotificacionResponsableAplicacionId,
                                         PortafolioResponsableId = grp.Key.PortafolioResponsableId,
                                         BuzonSalida = grp.Key.BuzonSalida,
                                         ConCopia = grp.Key.ConCopia,
                                         FechaInicio = grp.Key.FechaInicio,
                                         HoraEnvio = grp.Key.HoraEnvio,
                                         Cuerpo = grp.Key.Cuerpo,
                                         Asunto = grp.Key.Asunto,
                                         FlagNotificacionTTLJdE = grp.Key.FlagNotificacionTTLJdE,
                                         Matricula = grp.Key.Matricula,
                                         Correo = grp.Key.CorreoElectronico
                                     }).ToList();

                    if (registros != null && registros.Count() > 0)
                    {
                        //foreach (var item in registros)
                        //{
                        //    if (!string.IsNullOrEmpty(item.Matricula))
                        //        item.Correo = new ADUsuario().ObtenerADUsuario(item.Matricula).Correo;
                        //}

                        data = (from x in registros
                                group x by new { x.PortafolioResponsableId, x.FlagNotificacionTTLJdE, x.BuzonSalida, x.ConCopia, x.Cuerpo, x.Asunto } into gp
                                select new NotificacionResponsableAplicacionDto
                                {
                                    PortafolioResponsableId = gp.Key.PortafolioResponsableId,
                                    FlagNotificacionTTLJdE = gp.Key.FlagNotificacionTTLJdE,
                                    BuzonSalida = gp.Key.BuzonSalida,
                                    ConCopia = gp.Key.ConCopia,
                                    Asunto = gp.Key.Asunto,
                                    Para = string.Join(";", gp.Select(i => i.Correo))
                                }).ToList();
                    }

                    return data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
        }

        public override bool ActualizarNotificacionResponsableAplicacionEstado(int idNotificacion, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (idNotificacion != 0)
                    {
                        var entidad = (from u in ctx.NotificacionResponsableAplicacion
                                       where u.NotificacionResponsableAplicacionId == idNotificacion
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Activo = estado;
                            entidad.FlagEnvio = estado;
                            //entidad.FechaEnvio = DateTime.Now;
                            entidad.UsuarioModificacion = usuario;
                            entidad.FechaModificacion = DateTime.Now;
                            ctx.SaveChanges();
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
        }

        public override List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionExpertos()
        {
            try
            {
                var data = new List<NotificacionResponsableAplicacionDto>();
                var cadenaConexion = Constantes.CadenaConexion;                
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Notificaciones_Expertos_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new NotificacionResponsableAplicacionDto()
                            {
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Colaborador")) ? string.Empty : reader.GetString(reader.GetOrdinal("Colaborador")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("CorreoElectronico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                                TotalAplicaciones = reader.IsDBNull(reader.GetOrdinal("TotalApps")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalApps"))
                            };
                            data.Add(objeto);
                        }
                        reader.Close();
                    }

                    return data;
                }
            }            
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
        }

        public override List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionUsuariosLideres(TipoNotificacionDto tipo)
        {
            var matricula = string.Empty;
            try
            {
                var data = new List<NotificacionResponsableAplicacionDto>();
                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosLideres]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", (int)EUsuarioNotificacion.TTL)); //TTL

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new NotificacionResponsableAplicacionDto()
                            {
                                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuario")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Activo = true,
                                Asunto = tipo.Asunto,
                                BuzonSalida = tipo.BuzonSalida,
                                ConCopia = tipo.ConCopia,
                                Correo = tipo.BuzonSalida,
                                Cuerpo = tipo.Cuerpo,
                                TipoUsuario = (int)EUsuarioNotificacion.TTL
                            };
                            objeto.UsuariosConCopia = this.ObtenerMatriculas(objeto.Matricula, (int)EUsuarioNotificacion.TTL);
                            data.Add(objeto);
                        }
                        reader.Close();
                    }                    
                }

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosLideres]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", (int)EUsuarioNotificacion.LiderUsuario)); //Líder Usuario

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new NotificacionResponsableAplicacionDto()
                            {
                                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuario")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Activo = true,
                                Asunto = tipo.Asunto,
                                BuzonSalida = tipo.BuzonSalida,
                                ConCopia = tipo.ConCopia,
                                Correo = tipo.BuzonSalida,
                                Cuerpo = tipo.Cuerpo,
                                TipoUsuario = (int)EUsuarioNotificacion.LiderUsuario
                            };
                            matricula = objeto.Matricula;
                            objeto.UsuariosConCopia = this.ObtenerMatriculas(objeto.Matricula, (int)EUsuarioNotificacion.LiderUsuario);
                            data.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var dataLista = (from x in data
                                     join b in ctx.AplicacionPortafolioResponsables on x.Matricula equals b.Matricula
                                     select new
                                     {
                                         x.NombreUsuario,
                                         x.Matricula,
                                         x.Activo,
                                         x.Asunto,
                                         x.BuzonSalida,
                                         x.ConCopia,
                                         x.Correo,
                                         x.Cuerpo,
                                         x.TipoUsuario,
                                         Para = b.CorreoElectronico
                                     }).ToList().Select(a => new NotificacionResponsableAplicacionDto
                                     {
                                         NombreUsuario = a.NombreUsuario,
                                         Matricula = a.Matricula,
                                         Activo = a.Activo,
                                         Asunto = a.Asunto,
                                         BuzonSalida = a.BuzonSalida,
                                         ConCopia = a.ConCopia,
                                         Correo = a.Correo,
                                         Cuerpo = a.Cuerpo,
                                         TipoUsuario = a.TipoUsuario,
                                         Para = a.Para
                                     });

                }

                return data;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
        }

        public override List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionUsuariosLideres()
        {
            var matricula = string.Empty;
            try
            {
                var data = new List<NotificacionResponsableAplicacionDto>();
                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosLideres]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", (int)EUsuarioNotificacion.TTL)); //TTL

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new NotificacionResponsableAplicacionDto()
                            {
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuario")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("CorreoElectronico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Activo = true,                                
                                TipoUsuario = (int)EUsuarioNotificacion.TTL
                            };
                            objeto.UsuariosConCopia = this.ObtenerMatriculas_v2(objeto.Matricula, (int)EUsuarioNotificacion.TTL);
                            data.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosLideres]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", (int)EUsuarioNotificacion.LiderUsuario)); //Líder Usuario

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new NotificacionResponsableAplicacionDto()
                            {
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuario")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("CorreoElectronico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Activo = true,                                
                                TipoUsuario = (int)EUsuarioNotificacion.LiderUsuario
                            };
                            matricula = objeto.Matricula;
                            objeto.UsuariosConCopia = this.ObtenerMatriculas_v2(objeto.Matricula, (int)EUsuarioNotificacion.LiderUsuario);
                            data.Add(objeto);
                        }
                        reader.Close();
                    }
                }                

                return data;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesAplicacionUsuariosLideres()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesAplicacionUsuariosLideres()"
                    , new object[] { null });
            }
        }

        private List<string> ObtenerMatriculas(string matricula, int tipo)
        {
            var lista = new List<string>();
            var cadenaConexion = Constantes.CadenaConexion;
            if (tipo == (int)EUsuarioNotificacion.TTL)
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosAdicionales_TTL]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaOwner"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }

                            elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaGestor")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaGestor"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }

                            elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaRiesgo")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaRiesgo"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            else
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosAdicionales_LiderUsuario]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaGestor")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaGestor"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }

                            elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaBroker")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaBroker"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }

                            elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaJDE")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaJDE"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }
                            elemento = reader.IsDBNull(reader.GetOrdinal("MatriculaRiesgo")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaRiesgo"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return lista.Distinct().ToList();
        }

        private List<string> ObtenerMatriculas_v2(string matricula, int tipo)
        {
            var lista = new List<string>();
            var cadenaConexion = Constantes.CadenaConexion;
            if (tipo == (int)EUsuarioNotificacion.TTL)
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosAdicionales_TTL_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var elemento = reader.IsDBNull(reader.GetOrdinal("CorreoElectronico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoElectronico"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }                           
                        }
                        reader.Close();
                    }
                }
            }
            else
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Notificaciones_ListarUsuariosAdicionales_LiderUsuario_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var elemento = reader.IsDBNull(reader.GetOrdinal("CorreoElectronico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoElectronico"));
                            if (!string.IsNullOrWhiteSpace(elemento))
                            {
                                if (!elemento.ToUpper().Contains("APLICA"))
                                    lista.Add(elemento);
                            }                           
                        }
                        reader.Close();
                    }
                }
            }

            return lista.Distinct().ToList();
        }

        public override List<TipoNotificacionDto> ListarTipoNotificacionesApp(bool? flagEstado = null)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacionApp
                                     where (u.Activo == flagEstado || flagEstado == null)
                                     select new TipoNotificacionDto()
                                     {
                                         Id = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         BuzonSalida = u.BuzonSalida,
                                         Para = u.Para,
                                         Frecuencia = u.Frecuencia
                                     });
                    return registros.ToList();                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditTipoNotificacionApp(TipoNotificacionDto objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var entidad = (from u in ctx.TipoNotificacionApp
                                   where u.TipoNotificacionId == objRegistro.Id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        //entidad.Nombre = objRegistro.Nombre;
                        //entidad.Descripcion = objRegistro.Descripcion;
                        entidad.BuzonSalida = objRegistro.BuzonSalida;
                        entidad.Para = objRegistro.Para;
                        entidad.ConCopia = objRegistro.ConCopia;
                        entidad.Asunto = objRegistro.Asunto;
                        entidad.Cuerpo = objRegistro.Cuerpo;
                        entidad.Frecuencia = objRegistro.Frecuencia;
                        //entidad.FechaInicio = objRegistro.FechaInicio;
                        //entidad.HoraEnvio = objRegistro.HoraEnvio;
                        entidad.FechaModificacion = DateTime.Now;
                        entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                        entidad.AIO = objRegistro.AIO;
                        entidad.ArquitectoNegocio = objRegistro.ArquitectoNegocio;
                        entidad.ATI = objRegistro.ATI;
                        entidad.Brokerr = objRegistro.Brokerr;
                        entidad.DevSec = objRegistro.DevSec;
                        entidad.GobiernoIT = objRegistro.GobiernoIT;
                        entidad.JefeEquipo = objRegistro.JefeEquipo;
                        entidad.LiderUsuario = objRegistro.LiderUsuario;
                        entidad.Solicitante = objRegistro.Solicitante;
                        entidad.TL_TTL = objRegistro.TL_TTL;
                        entidad.UsuarioAutorizador = objRegistro.UsuarioAutorizador;
                        
                        //entidad.CuerpoAlternativo = objRegistro.CuerpoAlternativo;

                        ctx.SaveChanges();
                        ID = entidad.TipoNotificacionId;
                    }

                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override TipoNotificacionDto ObtenerTipoNotificacionApp(int idTipoNotificacion)
        {
            var cadenaConexion = Constantes.CadenaConexion;
            var entidad = new TipoNotificacionDto();
            try
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_ObtenerTipoNotificacionApp]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idTipoNotificacion", idTipoNotificacion));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TipoNotificacionDto()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("TipoNotificacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoNotificacionId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Para = reader.IsDBNull(reader.GetOrdinal("Para")) ? string.Empty : reader.GetString(reader.GetOrdinal("Para")),
                                Frecuencia = reader.IsDBNull(reader.GetOrdinal("Frecuencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("Frecuencia")),
                                Asunto = reader.IsDBNull(reader.GetOrdinal("Asunto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asunto")),
                                ConCopia = reader.IsDBNull(reader.GetOrdinal("ConCopia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ConCopia")),
                                Cuerpo = reader.IsDBNull(reader.GetOrdinal("Cuerpo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Cuerpo")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                HoraEnvio = reader.IsDBNull(reader.GetOrdinal("HoraEnvio")) ? 0 : reader.GetInt32(reader.GetOrdinal("HoraEnvio")),
                                BuzonSalida = reader.IsDBNull(reader.GetOrdinal("BuzonSalida")) ? string.Empty : reader.GetString(reader.GetOrdinal("BuzonSalida")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo")),
                                CuerpoAlternativo = reader.IsDBNull(reader.GetOrdinal("CuerpoAlternativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CuerpoAlternativo")),
                                AIO = reader.IsDBNull(reader.GetOrdinal("AIO")) ? false : reader.GetBoolean(reader.GetOrdinal("AIO")),
                                ArquitectoNegocio = reader.IsDBNull(reader.GetOrdinal("ArquitectoNegocio")) ? false : reader.GetBoolean(reader.GetOrdinal("ArquitectoNegocio")),
                                ATI = reader.IsDBNull(reader.GetOrdinal("ATI")) ? false : reader.GetBoolean(reader.GetOrdinal("ATI")),
                                Brokerr = reader.IsDBNull(reader.GetOrdinal("Brokerr")) ? false : reader.GetBoolean(reader.GetOrdinal("Brokerr")),
                                DevSec = reader.IsDBNull(reader.GetOrdinal("DevSec")) ? false : reader.GetBoolean(reader.GetOrdinal("DevSec")),
                                GobiernoIT = reader.IsDBNull(reader.GetOrdinal("GobiernoIT")) ? false : reader.GetBoolean(reader.GetOrdinal("GobiernoIT")),
                                JefeEquipo = reader.IsDBNull(reader.GetOrdinal("JefeEquipo")) ? false : reader.GetBoolean(reader.GetOrdinal("JefeEquipo")),
                                LiderUsuario = reader.IsDBNull(reader.GetOrdinal("LiderUsuario")) ? false : reader.GetBoolean(reader.GetOrdinal("LiderUsuario")),
                                Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? false : reader.GetBoolean(reader.GetOrdinal("Solicitante")),
                                TL_TTL = reader.IsDBNull(reader.GetOrdinal("TL_TTL")) ? false : reader.GetBoolean(reader.GetOrdinal("TL_TTL")),
                                UsuarioAutorizador = reader.IsDBNull(reader.GetOrdinal("UsuarioAutorizador")) ? false : reader.GetBoolean(reader.GetOrdinal("UsuarioAutorizador")),
                                ArquitectoSolucion = reader.IsDBNull(reader.GetOrdinal("ArquitectoSolucion")) ? false : reader.GetBoolean(reader.GetOrdinal("ArquitectoSolucion"))
                            };
                            entidad = objeto;
                        }
                        reader.Close();
                    }
                    return entidad;
                }
                //using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                //{
                //    var entidad = (from u in ctx.TipoNotificacionApp
                //                   where u.TipoNotificacionId == idTipoNotificacion
                //                   select new TipoNotificacionDto
                //                   {
                //                       Id = u.TipoNotificacionId,
                //                       Descripcion = u.Descripcion,
                //                       Nombre = u.Nombre,
                //                       Para = u.Para,
                //                       Frecuencia = u.Frecuencia,
                //                       Asunto = u.Asunto,
                //                       ConCopia = u.ConCopia,
                //                       Cuerpo = u.Cuerpo,
                //                       FechaInicio = u.FechaInicio,
                //                       HoraEnvio = u.HoraEnvio,
                //                       BuzonSalida = u.BuzonSalida,
                //                       Activo = u.Activo,
                //                       CuerpoAlternativo = u.CuerpoAlternativo,
                //                       AIO = u.AIO,
                //                       ArquitectoNegocio = u.ArquitectoNegocio,
                //                       ATI = u.ATI,
                //                       Brokerr = u.Brokerr,
                //                       DevSec = u.DevSec,
                //                       GobiernoIT = u.GobiernoIT,
                //                       JefeEquipo = u.JefeEquipo,
                //                       LiderUsuario = u.LiderUsuario,
                //                       Solicitante = u.Solicitante,
                //                       TL_TTL = u.TL_TTL,
                //                       UsuarioAutorizador=u.UsuarioAutorizador
                //                       //ArquitectoSolucion=u.ArquitectoSolucion
                //                   }).FirstOrDefault();
                //    return entidad;                    
                //}
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion)"
                    , new object[] { null });
            }
        }
        public override List<int> ObtenerIDsTipoNotificacionApp_NoAplicationFlow(string valor)
        {
            var cadenaConexion = Constantes.CadenaConexion;
            var listIDs = new List<int>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_IDsTipoNotificacionApp_NoNotificarAplicationFlow]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@valor", valor));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var Id = reader.IsDBNull(reader.GetOrdinal("IDTipoNotificacionApp")) ? 0 : reader.GetInt32(reader.GetOrdinal("IDTipoNotificacionApp"));
                            listIDs.Add(Id);
                        }
                        reader.Close();
                    }
                    return listIDs;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<int> ObtenerIDsTipoNotificacionApp_NoAplicationFlow(string valor)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<int> ObtenerIDsTipoNotificacionApp_NoAplicationFlow(string valor)"
                    , new object[] { null });
            }
        }

        private string DevolverParaByTipoNotificacion(int solicitudId, int tipoNotificacionId, string paraCorreoTipoNotificacion, int bandejaId, string Usuario, GestionCMDB_ProdEntities _ctx)
        {
            var ctx = _ctx;
            string retornoPara = string.Empty;
            string _Para = string.Empty;
            var lMatriculasAdmin = new List<string>();
            var lCorreosAdmin = new List<string>();
            var lCorreosSubAprobadores = new List<string>();

            try
            {
                //Recuperando todos los administradores:
                string lMatriculaAprobadores = Settings.Get<string>("Responsables.Portafolio");
                if (!string.IsNullOrEmpty(lMatriculaAprobadores))
                    lMatriculasAdmin = lMatriculaAprobadores.Split('|').ToList();

                //Agregando al usuario aprobador:
                lMatriculasAdmin.Add(Usuario);

                if (lMatriculasAdmin != null && lMatriculasAdmin.Count > 0)
                {
                    foreach (var iMatricula in lMatriculasAdmin)
                    {
                        if (!string.IsNullOrEmpty(iMatricula))
                        {
                            var usuario = new ADUsuario().ObtenerADUsuario(iMatricula);
                            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Correo))
                            {
                                lCorreosAdmin.Add(usuario.Correo);
                            }                                                        
                        }
                    }
                }

                if (lCorreosAdmin != null && lCorreosAdmin.Count > 0)
                    _Para = string.Join(";", lCorreosAdmin);

                //if (!string.IsNullOrEmpty(paraCorreoTipoNotificacion))
                //    _Para = string.Concat(_Para, string.Format(";{0}", paraCorreoTipoNotificacion));

                switch (tipoNotificacionId)
                {
                    case (int)ETipoNotificacionAplicacion.RegistroSolicitudCreacion:
                        var lMatriculaSubaprobadores = (from x in ctx.SolicitudAprobadores
                                                        where x.SolicitudAplicacionId == solicitudId
                                                        && x.FlagActivo
                                                        && !x.FlagAprobado.Value
                                                        select x.Matricula).ToList();

                        if (lMatriculaSubaprobadores != null && lMatriculaSubaprobadores.Count > 0)
                        {
                            string subMatriculas = string.Join("|", lMatriculaSubaprobadores);
                            var arrSubMatriculas = subMatriculas.Split('|').ToList();
                            if (arrSubMatriculas != null && arrSubMatriculas.Count > 0)
                            {
                                foreach (var iMatSub in arrSubMatriculas)
                                {
                                    if (!string.IsNullOrEmpty(iMatSub))
                                    {                                        
                                        var usuario = new ADUsuario().ObtenerADUsuario(iMatSub);
                                        if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Correo))
                                        {
                                            lCorreosSubAprobadores.Add(usuario.Correo);
                                        }                                        
                                    }
                                }
                            }

                            if (lCorreosSubAprobadores != null && lCorreosSubAprobadores.Count > 0)
                            {
                                string Para_Sub = string.Join(";", lCorreosSubAprobadores);
                                _Para = string.Concat(_Para, string.Format(";{0}", Para_Sub));
                            }
                        }

                        retornoPara = _Para;

                        break;
                    case (int)ETipoNotificacionAplicacion.RegistroSolicitudEliminacion:
                    case (int)ETipoNotificacionAplicacion.RegistroSolicitudModificacion:
                        retornoPara = _Para;

                        break;
                    case (int)ETipoNotificacionAplicacion.AprobacionSolicitudCreacion:
                    case (int)ETipoNotificacionAplicacion.AprobacionSolicitudEliminacion:
                    case (int)ETipoNotificacionAplicacion.AprobacionSolicitudModificacion:
                    case (int)ETipoNotificacionAplicacion.NotificacionBuzonesCreacion:
                    case (int)ETipoNotificacionAplicacion.NotificacionBuzonesEliminacion:
                        //Notificar a todos los aprobadores admin(esta incluido el que hizo la aprobacion) y al creador de la solicitud
                        var iSolicitud = ctx.Solicitud.FirstOrDefault(x => x.SolicitudAplicacionId == solicitudId);
                        if(iSolicitud != null)
                        {
                            var usuario = new ADUsuario().ObtenerADUsuario(iSolicitud.UsuarioCreacion);
                            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Correo))
                            {
                                _Para = string.Concat(_Para, string.Format(";{0}", usuario.Correo));
                            }
                            
                        }
                        retornoPara = _Para;

                        break;
                    case (int)ETipoNotificacionAplicacion.CambioEstadoArquitecturaTI:
                    case (int)ETipoNotificacionAplicacion.CambioEstadoClasificacion:
                    case (int)ETipoNotificacionAplicacion.CambioEstadoDevOps:
                    case (int)ETipoNotificacionAplicacion.CambioEstadoLiderUsuario:
                    case (int)ETipoNotificacionAplicacion.CambioEstadoTTL:
                    case (int)ETipoNotificacionAplicacion.CambioEstadoUserIT:
                        //Notificar a todos los subaprobadores de dichas bandejas, a todos los aprobadores admin y al creador de la solicitud
                        //1. Creador
                        var itemSol = ctx.Solicitud.FirstOrDefault(x => x.SolicitudAplicacionId == solicitudId);
                        if (itemSol != null)
                        {                            
                            var usuario = new ADUsuario().ObtenerADUsuario(itemSol.UsuarioCreacion);
                            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Correo))
                            {
                                _Para = string.Concat(_Para, string.Format(";{0}", usuario.Correo));
                            }
                        }

                        //2. Subaprobadores por bandeja
                        var iSubaprobBandeja = (from x in ctx.SolicitudAprobadores
                                                where x.SolicitudAplicacionId == solicitudId
                                                && x.BandejaId == bandejaId
                                                && x.FlagActivo
                                                select x.Matricula).FirstOrDefault();

                        if(iSubaprobBandeja != null)
                        {
                            var arrSubMatriculas = iSubaprobBandeja.Split('|').ToList();
                            if (arrSubMatriculas != null && arrSubMatriculas.Count > 0)
                            {
                                foreach (var iMatSub in arrSubMatriculas)
                                {
                                    if (!string.IsNullOrEmpty(iMatSub))
                                    {                                        
                                        var usuario = new ADUsuario().ObtenerADUsuario(iMatSub);
                                        if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Correo))
                                        {
                                            lCorreosSubAprobadores.Add(usuario.Correo);
                                        }
                                    }
                                }

                                if (lCorreosSubAprobadores != null && lCorreosSubAprobadores.Count > 0)
                                {
                                    string Para_Sub = string.Join(";", lCorreosSubAprobadores);
                                    _Para = string.Concat(_Para, string.Format(";{0}", Para_Sub));
                                }
                            }
                        }
                        retornoPara = _Para;

                        break;

                }
                var data = retornoPara.Split(';').ToList();
                data = data.Distinct().ToList();
                retornoPara = string.Join(";", data);

                return retornoPara;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override int RegistrarNotificacionApp(NotificacionAplicacionDTO objRegistro, GestionCMDB_ProdEntities _ctx)
        {
            try
            {
                int ID = 0;
                //DateTime FECHA_ACTUAL = DateTime.Now;
                //var entidad = new NotificacionApp();
                //var ctx = _ctx;

                //if (objRegistro.Id == 0)
                //{
                //    var tipoNotificacionApp = ctx.TipoNotificacionApp.FirstOrDefault(x => x.TipoNotificacionId == objRegistro.TipoNotificacionId);
                //    if (tipoNotificacionApp != null)
                //    {
                //        string PARA_ALL = DevolverParaByTipoNotificacion(objRegistro.SolicitudId,
                //            objRegistro.TipoNotificacionId, 
                //            tipoNotificacionApp.Para,
                //            objRegistro.BandejaId,
                //            objRegistro.UsuarioCreacion,
                //            _ctx);

                //        if (!string.IsNullOrEmpty(PARA_ALL))
                //        {
                //            entidad = new NotificacionApp()
                //            {
                //                TipoNotificacionId = objRegistro.TipoNotificacionId,
                //                //SolicitudAplicacionId = objRegistro.SolicitudId,
                //                Nombre = tipoNotificacionApp.Nombre,
                //                De = "", //objRegistro.De,
                //                Para = PARA_ALL,
                //                CC = tipoNotificacionApp.ConCopia,
                //                BCC = objRegistro.BCC,
                //                Cuerpo = tipoNotificacionApp.Cuerpo,
                //                Asunto = tipoNotificacionApp.Asunto,
                //                FlagEnviado = false,
                //                FechaEnvio = null,
                //                UsuarioCreacion = objRegistro.UsuarioCreacion,
                //                FechaCreacion = FECHA_ACTUAL,
                //                Activo = true
                //            };
                //            ctx.NotificacionApp.Add(entidad);
                //            ctx.SaveChanges();
                //        }
                //    }

                    //ID = entidad.NotificacionId;
                return ID;
            }                            
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: int RegistrarNotificacion(NotificacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<NotificacionAplicacionDTO> ListarNotificacionesAppPendientesEnvio()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //var registros = (from u in ctx.NotificacionApp
                    //                 where !u.FlagEnviado.Value
                    //                 select new NotificacionAplicacionDTO()
                    //                 {
                    //                     Id = u.NotificacionId,
                    //                     TipoNotificacionId = u.TipoNotificacionId,
                    //                     Nombre = u.Nombre,
                    //                     De = u.De,
                    //                     Para = u.Para,
                    //                     CC = u.CC,
                    //                     BCC = u.BCC,
                    //                     Cuerpo = u.Cuerpo,
                    //                     Asunto = u.Asunto,
                    //                     UsuarioCreacion = u.UsuarioCreacion,
                    //                     //SolicitudId = u.SolicitudAplicacionId,
                    //                 }).Take(250).ToList();

                    //return registros;
                    return null;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: List<NotificacionDTO> ListarNotificacionesPendientesEnvio()"
                    , new object[] { null });
            }
        }

        public override bool ActualizarNotificacionAppEstado(int idNotificacion, bool estado, string usuario)
        {
            try
            {
                bool retorno = false;
                //DateTime FECHA_ACTUAL = DateTime.Now;
                //using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                //{
                //    var entidad = ctx.NotificacionApp.FirstOrDefault(x => x.NotificacionId == idNotificacion);
                //    if (entidad != null)
                //    {
                //        entidad.Activo = estado;
                //        entidad.FlagEnviado = estado;
                //        entidad.FechaEnvio = FECHA_ACTUAL;
                //        entidad.UsuarioModificacion = usuario;
                //        entidad.FechaModificacion = FECHA_ACTUAL;
                //        ctx.SaveChanges();

                //        retorno = true;
                //    }
                //}

                return retorno;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: bool ActualizarNotificacionEstado(int idNotificacion, bool estado)"
                    , new object[] { null });
            }
        }

        public override List<TipoNotificacionDto> GetTipoNotificacionesApp(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoNotificacionApp
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && u.Activo == true
                                     select new TipoNotificacionDto()
                                     {
                                         Id = u.TipoNotificacionId,
                                         Nombre = u.Nombre,
                                         FechaUltimoEnvio = u.FechaUltimoEnvio,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         BuzonSalida = u.BuzonSalida,
                                         Para = u.Para,
                                         Frecuencia = u.Frecuencia
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetProcesoVital(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetProcesoVital(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<string> ListarRolOnDemandAdmins()
        {
            try
            {
                
                var registros = new List<string>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                        registros = (from u in ctx.RolOndemand
                                    
                                     where u.RoleId == (int)ERoles.Administrador
                                   
                                     select u.Email).ToList();
          

                    
         

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TipoNotificacionDto ObtenerTipoNotificacionPorNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoNotificacion
                                   where u.Nombre == nombre && u.Activo == true
                                   select new TipoNotificacionDto
                                   {
                                       Id = u.TipoNotificacionId,
                                       Descripcion = u.Descripcion,
                                       Nombre = u.Nombre,
                                       Para = u.Para,
                                       Frecuencia = u.Frecuencia,
                                       Asunto = u.Asunto,
                                       ConCopia = u.ConCopia,
                                       Cuerpo = u.Cuerpo,
                                       FechaInicio = u.FechaInicio,
                                       HoraEnvio = u.HoraEnvio,
                                       BuzonSalida = u.BuzonSalida,
                                       Activo = u.Activo,
                                       CuerpoAlternativo = u.CuerpoAlternativo,
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacionPorNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorNotificacionDTO
                    , "Error en el metodo: TipoNotificacionDto ObtenerTipoNotificacionPorNombre(string nombre)"
                    , new object[] { null });
            }
        }
    }
}
