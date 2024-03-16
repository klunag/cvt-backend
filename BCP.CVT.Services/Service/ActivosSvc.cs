using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BCP.PAPP.Common.Cross;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;

namespace BCP.CVT.Services.Service
{
    public class ActivosSvc : ActivosDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditActivos(ActivosDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var entidad = new TipoActivoInformacion()
                        {
                            FlujoRegistro = (int)EFlujoRegistro.FNA,
                            Descripcion = objeto.Descripcion,
                            Nombre = objeto.Nombre,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioModificacion,
                            FechaCreacion = FECHA_ACTUAL,
                            FechaModificacion = FECHA_ACTUAL,
                            FlagEliminado = false,
                            FlagUserIT = objeto.FlagUserIT.HasValue ? objeto.FlagUserIT.Value : false,
                            FlagExterna = objeto.FlagExterna.HasValue ? objeto.FlagExterna.Value : false
                        };
                        ctx.TipoActivoInformacion.Add(entidad);

                        //var detalleHtml = $"--Detalle de campos-- <br/>" +
                        //                $"* Nuevo item registrado";
                        //var auditoriaItem = new AuditoriaTipoActivo()
                        //{
                        //    Accion = "I",
                        //    Entidad = objeto.Nombre,
                        //    Usuario = objeto.UsuarioCreacion,
                        //    Campo = detalleHtml,
                        //    FlagActivo = true,
                        //    UsuarioCreacion = objeto.UsuarioCreacion,
                        //    FechaCreacion = FECHA_ACTUAL
                        //};
                        //ctx.AuditoriaTipoActivo.Add(auditoriaItem);
                        ctx.SaveChanges();

                        ID = entidad.TipoActivoInformacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.TipoActivoInformacion
                                       where u.TipoActivoInformacionId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {                            
                            //var detalleHtml = $"--Detalle de campos-- <br/>" +
                            //    $"* Nombre<br/>" +
                            //    $" Valor anterior: {entidad.Nombre} / Valor nuevo: {objeto.Nombre}<br/>" +
                            //    $"* Descripción<br/>" +
                            //    $" Valor anterior: {entidad.Descripcion} / Valor nuevo: {objeto.Descripcion}<br/>";
                            

                            entidad.FlujoRegistro = (int)EFlujoRegistro.FNA;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Nombre = objeto.Nombre;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.FlagUserIT = objeto.FlagUserIT;
                            entidad.FlagExterna = objeto.FlagExterna;

                            //var auditoriaItem = new AuditoriaTipoActivo()
                            //{
                            //    Accion = "U",
                            //    Entidad = objeto.Nombre,
                            //    Usuario = objeto.UsuarioCreacion,
                            //    Campo = detalleHtml,
                            //    FlagActivo = true,
                            //    UsuarioCreacion = objeto.UsuarioCreacion,
                            //    FechaCreacion = FECHA_ACTUAL
                            //};
                            //ctx.AuditoriaTipoActivo.Add(auditoriaItem);
                            ctx.SaveChanges();

                            ID = entidad.TipoActivoInformacionId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditArea(AreaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Area()
                        {
                            AreaId = objeto.Id,
                            DivisionId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            ResponsableMatricula = objeto.ResponsableMatricula,
                            ResponsableCorreo = objeto.ResponsableCorreo,
                            CodigoSIGA = objeto.CodigoSIGA,
                            FlagEditar = true,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Area.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.AreaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Area
                                       where u.AreaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.ResponsableMatricula = objeto.ResponsableMatricula;
                            entidad.ResponsableCorreo = objeto.ResponsableCorreo;
                            entidad.CodigoSIGA = objeto.CodigoSIGA;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.AreaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditAreaBian(AreaBianDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new AreaBian()
                        {
                            AreaBianId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            Correo = objeto.Correo,
                            NombreResponsable = objeto.NombreResponsable,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.AreaBian.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.AreaBianId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.AreaBian
                                       where u.AreaBianId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.Correo = objeto.Correo;
                            entidad.NombreResponsable = objeto.NombreResponsable;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.AreaBianId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditBandeja(BandejaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Bandeja()
                        {
                            BandejaId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Bandeja.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.BandejaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Bandeja
                                       where u.BandejaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.BandejaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditBandejaAprobacion(BandejaAprobacionDTO objeto)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new BandejaAprobacion()
                        {
                            BandejaAprobacionId = objeto.Id,
                            BandejaId = objeto.EntidadRelacionId,
                            MatriculaAprobador = objeto.MatriculaAprobador,
                            Correo = objeto.Correo,
                            Nombres = objeto.Nombres,
                            FlagValidarMatricula = objeto.FlagValidarMatricula,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.BandejaAprobacion.Add(entidad);

                        //Actualizamos app.SolicitudAprobadores x Bandeja que aun no estan aprobadas
                        ctx.SolicitudAprobadores.Where(x => x.FlagActivo && x.BandejaId == entidad.BandejaId && !x.FlagAprobado.Value).ToList()
                         .ForEach(x =>
                         {
                             x.Matricula = string.IsNullOrWhiteSpace(x.Matricula) ? objeto.MatriculaAprobador : $"{x.Matricula}|{objeto.MatriculaAprobador}";
                             x.UsuarioModificacion = objeto.UsuarioCreacion;
                             x.FechaModificacion = FECHA_ACTUAL;
                         });

                        ctx.SaveChanges();

                        ID = entidad.BandejaAprobacionId;
                    }
                    else
                    {
                        var entidad = ctx.BandejaAprobacion.FirstOrDefault(x => x.BandejaAprobacionId == objeto.Id);
                        if (entidad != null)
                        {
                            string oldMatricula = entidad.MatriculaAprobador;
                            entidad.MatriculaAprobador = objeto.MatriculaAprobador;
                            entidad.Correo = objeto.Correo;
                            entidad.Nombres = objeto.Nombres;
                            entidad.FlagValidarMatricula = objeto.FlagValidarMatricula;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            //Actualizamos app.SolicitudAprobadores x Bandeja que aun no estan aprobadas
                            ctx.SolicitudAprobadores.Where(x => x.FlagActivo && x.BandejaId == entidad.BandejaId && !x.FlagAprobado.Value).ToList()
                            .ForEach(x =>
                            {
                                x.Matricula = string.IsNullOrWhiteSpace(x.Matricula) ? objeto.MatriculaAprobador : x.Matricula.Replace(oldMatricula, objeto.MatriculaAprobador);
                                x.UsuarioModificacion = objeto.UsuarioCreacion;
                                x.FechaModificacion = FECHA_ACTUAL;
                            });

                            ctx.SaveChanges();

                            ID = entidad.BandejaAprobacionId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditBandejaAprobacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditBandejaAprobacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditCuestionarioAplicacion(CuestionarioAplicacionDTO objeto)
        {
            DbContextTransaction transaction = null;
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        ctx.Database.CommandTimeout = 0;

                        if (objeto.Id == 0)
                        {
                            var entidad = new CuestionarioAplicacion()
                            {
                                CuestionarioAplicacionId = objeto.Id,
                                CodigoAPT = objeto.CodigoAPT,
                                GestorAplicacion = objeto.GestorAplicacion,
                                Consultor = objeto.Consultor,
                                ConfidencialidadCalculo = objeto.ConfidencialidadCalculo,
                                ConfidencialidadDescripcion = objeto.ConfidencialidadDescripcion,
                                IntegridadCalculo = objeto.IntegridadCalculo,
                                IntegridadDescripcion = objeto.IntegridadDescripcion,
                                DisponibilidadCalculo = objeto.DisponibilidadCalculo,
                                DisponibilidadDescripcion = objeto.DisponibilidadDescripcion,
                                PrivacidadCalculo = objeto.PrivacidadCalculo,
                                PrivacidadDescripcion = objeto.PrivacidadDescripcion,
                                Clasificacion = objeto.Clasificacion,
                                NivelCriticidad = objeto.NivelCriticidad,
                                CriticidadFinal = objeto.CriticidadFinal,
                                FlagActivo = true,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                UsuarioModificacion = objeto.UsuarioCreacion,
                                FechaModificacion = FECHA_ACTUAL,
                                FechaCreacion = FECHA_ACTUAL
                            };
                            ctx.CuestionarioAplicacion.Add(entidad);
                            ctx.SaveChanges();

                            ID = entidad.CuestionarioAplicacionId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.CuestionarioAplicacion
                                           where u.CuestionarioAplicacionId == objeto.Id
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.GestorAplicacion = objeto.GestorAplicacion;
                                entidad.Consultor = objeto.Consultor;
                                entidad.ConfidencialidadCalculo = objeto.ConfidencialidadCalculo;
                                entidad.ConfidencialidadDescripcion = objeto.ConfidencialidadDescripcion;
                                entidad.IntegridadCalculo = objeto.IntegridadCalculo;
                                entidad.IntegridadDescripcion = objeto.IntegridadDescripcion;
                                entidad.DisponibilidadCalculo = objeto.DisponibilidadCalculo;
                                entidad.DisponibilidadDescripcion = objeto.DisponibilidadDescripcion;
                                entidad.PrivacidadCalculo = objeto.PrivacidadCalculo;
                                entidad.PrivacidadDescripcion = objeto.PrivacidadDescripcion;
                                entidad.Clasificacion = objeto.Clasificacion;
                                entidad.NivelCriticidad = objeto.NivelCriticidad;
                                entidad.CriticidadFinal = objeto.CriticidadFinal;
                                entidad.FechaModificacion = FECHA_ACTUAL;
                                entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                                ctx.SaveChanges();

                                ID = entidad.CuestionarioAplicacionId;
                            }
                        }

                        if (objeto.CuestionarioAplicacionDetalle != null && objeto.CuestionarioAplicacionDetalle.Count > 0)
                        {
                            foreach (var item in objeto.CuestionarioAplicacionDetalle)
                            {
                                item.CuestionarioAplicacionId = ID;
                                AddOrEditCuestionarioAplicacionDetalle(item, ctx);
                            }
                        }

                        transaction.Commit();
                        return ID;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditCuestionarioAplicacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditCuestionarioAplicacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditCuestionarioAplicacionDetalle(CuestionarioAplicacionDetalleDTO objeto, GestionCMDB_ProdEntities _ctx)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                var ctx = _ctx;
                var entidad = (from u in ctx.CuestionarioAplicacionDetalle
                               where u.CuestionarioAplicacionId == objeto.CuestionarioAplicacionId
                               && u.PreguntaId == objeto.PreguntaId
                               select u).FirstOrDefault();
                if (entidad != null)
                {
                    entidad.AlternativaSeleccionada = objeto.AlternativaSeleccionada;
                    entidad.FechaModificacion = FECHA_ACTUAL;
                    entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                    ctx.SaveChanges();

                    return entidad.CuestionarioAplicacionDetalleId;
                }
                else
                {
                    var nuevaEntidad = new CuestionarioAplicacionDetalle()
                    {
                        CuestionarioAplicacionDetalleId = objeto.Id,
                        CuestionarioAplicacionId = objeto.CuestionarioAplicacionId,
                        PreguntaId = objeto.PreguntaId,
                        AlternativaSeleccionada = objeto.AlternativaSeleccionada,
                        FlagActivo = true,
                        UsuarioCreacion = objeto.UsuarioCreacion,
                        UsuarioModificacion = objeto.UsuarioCreacion,
                        FechaModificacion = FECHA_ACTUAL,
                        FechaCreacion = FECHA_ACTUAL
                    };
                    ctx.CuestionarioAplicacionDetalle.Add(nuevaEntidad);
                    ctx.SaveChanges();

                    return nuevaEntidad.CuestionarioAplicacionDetalleId;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditCuestionarioAplicacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditCuestionarioAplicacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditCuestionarioPae(CuestionarioPaeDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new CuestionarioPae()
                        {
                            CuestionarioPaeId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.CuestionarioPae.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.CuestionarioPaeId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.CuestionarioPae
                                       where u.CuestionarioPaeId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.CuestionarioPaeId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditDivision(DivisionDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Division()
                        {
                            DivisionId = objeto.Id,
                            GerenciaId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            ResponsableMatricula = objeto.ResponsableMatricula,
                            ResponsableCorreo = objeto.ResponsableCorreo,
                            CodigoSIGA = objeto.CodigoSIGA,
                            FlagEditar = true,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Division.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.DivisionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Division
                                       where u.DivisionId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.ResponsableMatricula = objeto.ResponsableMatricula;
                            entidad.ResponsableCorreo = objeto.ResponsableCorreo;
                            entidad.CodigoSIGA = objeto.CodigoSIGA;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.DivisionId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditDivision(DivisionDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditDivision(DivisionDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditDominioBian(DominioBianDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new DominioBian()
                        {
                            DominioBianId = objeto.Id,
                            AreaBianId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            Correo = objeto.Correo,
                            NombreResponsable = objeto.NombreResponsable,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.DominioBian.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.DominioBianId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.DominioBian
                                       where u.DominioBianId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.Correo = objeto.Correo;
                            entidad.NombreResponsable = objeto.NombreResponsable;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.DominioBianId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditGerencia(GerenciaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Gerencia()
                        {
                            GerenciaId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            ResponsableMatricula = objeto.ResponsableMatricula,
                            ResponsableCorreo = objeto.ResponsableCorreo,
                            CodigoSIGA = objeto.CodigoSIGA,
                            FlagEditar= true,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Gerencia.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.GerenciaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Gerencia
                                       where u.GerenciaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.ResponsableMatricula = objeto.ResponsableMatricula;
                            entidad.ResponsableCorreo = objeto.ResponsableCorreo;
                            entidad.CodigoSIGA = objeto.CodigoSIGA;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.GerenciaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditJefaturaAti(JefaturaAtiDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new JefaturaAti()
                        {
                            JefaturaAtiId = objeto.Id,
                            DominioBianId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            Correo = objeto.Correo,
                            NombreResponsable = objeto.NombreResponsable,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.JefaturaAti.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.JefaturaAtiId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.JefaturaAti
                                       where u.JefaturaAtiId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.Correo = objeto.Correo;
                            entidad.NombreResponsable = objeto.NombreResponsable;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.JefaturaAtiId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditPreguntaPae(PreguntaPaeDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new PreguntaPae()
                        {
                            PreguntaPaeId = objeto.Id,
                            CuestionarioPaeId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.PreguntaPae.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.PreguntaPaeId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.PreguntaPae
                                       where u.PreguntaPaeId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.PreguntaPaeId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditProcesoVital(ProcesoVitalDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new ProcesoVital()
                        {
                            ProcesoVitalId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagProcesoVital = objeto.FlagProcesoVital,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.ProcesoVital.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.ProcesoVitalId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ProcesoVital
                                       where u.ProcesoVitalId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FlagProcesoVital = objeto.FlagProcesoVital;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.ProcesoVitalId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditUnidad(UnidadDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Unidad()
                        {
                            UnidadId = objeto.Id,
                            AreaId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Responsable = objeto.Responsable,
                            ResponsableMatricula = objeto.ResponsableMatricula,
                            ResponsableCorreo = objeto.ResponsableCorreo,
                            CodigoSIGA = objeto.CodigoSIGA,
                            FlagEditar = true,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Unidad.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.UnidadId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Unidad
                                       where u.UnidadId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Responsable = objeto.Responsable;
                            entidad.ResponsableMatricula = objeto.ResponsableMatricula;
                            entidad.ResponsableCorreo = objeto.ResponsableCorreo;
                            entidad.CodigoSIGA = objeto.CodigoSIGA;
                            entidad.FlagEditar = objeto.FlagEditar;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.UnidadId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditUnidad(UnidadDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditUnidad(UnidadDTO objeto)"
                    , new object[] { null });
            }
        }

        private EstandarPortafolioDTO GetEstandarPortafolioTecnologiaByTipoEstandar(GestionCMDB_ProdEntities ctx, int tipoEstandarId, string nombreEstandar)
        {
            var newEstandar = new EstandarPortafolioDTO();
            if (!string.IsNullOrEmpty(nombreEstandar) && nombreEstandar != "NO APLICA")
            {
                switch (tipoEstandarId)
                {
                    case (int)ETipoEstandarPortafolio.SistemaOperativo:
                        var itemSO = (from x in ctx.Tecnologia
                                      join y in ctx.Subdominio on x.SubdominioId equals y.SubdominioId
                                      join z in ctx.Tipo on x.TipoTecnologia equals z.TipoId
                                      where x.Activo && y.Activo && z.Activo
                                      && y.SubdominioId == 36
                                      && x.ClaveTecnologia.ToUpper().Equals(nombreEstandar.ToUpper())
                                      select new EstandarPortafolioDTO()
                                      {
                                          Id = x.TecnologiaId,
                                          SubdominioId = y.SubdominioId,
                                          TipoTecnologiaId = z.TipoId,
                                          EstadoId = x.EstadoId
                                      }).FirstOrDefault();

                        newEstandar = itemSO;
                        break;
                    case (int)ETipoEstandarPortafolio.HerramientaProgramacion:
                        var itemHP = (from x in ctx.Tecnologia
                                      join y in ctx.Subdominio on x.SubdominioId equals y.SubdominioId
                                      join z in ctx.Tipo on x.TipoTecnologia equals z.TipoId
                                      where x.Activo && y.Activo && z.Activo
                                      && (y.SubdominioId == 13 || y.SubdominioId == 8)
                                      && x.ClaveTecnologia.ToUpper().Equals(nombreEstandar.ToUpper())
                                      select new EstandarPortafolioDTO()
                                      {
                                          Id = x.TecnologiaId,
                                          SubdominioId = y.SubdominioId,
                                          TipoTecnologiaId = z.TipoId,
                                          EstadoId = x.EstadoId
                                      }).FirstOrDefault();

                        newEstandar = itemHP;
                        break;
                    case (int)ETipoEstandarPortafolio.GestorBaseDatos:
                        var itemBD = (from x in ctx.Tecnologia
                                      join y in ctx.Subdominio on x.SubdominioId equals y.SubdominioId
                                      join z in ctx.Tipo on x.TipoTecnologia equals z.TipoId
                                      where x.Activo && y.Activo && z.Activo
                                      && (y.SubdominioId == 68 || y.SubdominioId == 69)
                                      && x.ClaveTecnologia.ToUpper().Equals(nombreEstandar.ToUpper())
                                      select new EstandarPortafolioDTO()
                                      {
                                          Id = x.TecnologiaId,
                                          SubdominioId = y.SubdominioId,
                                          TipoTecnologiaId = z.TipoId,
                                          EstadoId = x.EstadoId
                                      }).FirstOrDefault();

                        newEstandar = itemBD;
                        break;
                    case (int)ETipoEstandarPortafolio.Framework:
                        var itemFW = (from x in ctx.Tecnologia
                                      join y in ctx.Subdominio on x.SubdominioId equals y.SubdominioId
                                      join z in ctx.Tipo on x.TipoTecnologia equals z.TipoId
                                      where x.Activo && y.Activo && z.Activo
                                      && y.SubdominioId == 14
                                      && x.ClaveTecnologia.ToUpper().Equals(nombreEstandar.ToUpper())
                                      select new EstandarPortafolioDTO()
                                      {
                                          Id = x.TecnologiaId,
                                          SubdominioId = y.SubdominioId,
                                          TipoTecnologiaId = z.TipoId,
                                          EstadoId = x.EstadoId
                                      }).FirstOrDefault();

                        newEstandar = itemFW;
                        break;
                }
            }
            else
                newEstandar = null;

            return newEstandar;
        }

        //Here
        public override GrupoNivelCumplimiento CalcularPorcentajeEstandar(int? id_aplicacion, int flagPc, double[] L, double[] M, double N,
            string _SO, string _HP, string _BD, string _FW, int ncls)
        {
            try
            {
                var GCN = new GrupoNivelCumplimiento();
                int puntuacion = 0;
                double porcentaje = 0;
                double calculoSO, calculoHP, calculoBD, calculoFW, calculoTotal = 0;
                string nombreEstandarSO, nombreEstandarHP, nombreEstandarBD, nombreEstandarFw = "";
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidadAplicacion = id_aplicacion.HasValue ? ctx.AplicacionDetalle.FirstOrDefault(x => x.AplicacionId == id_aplicacion) : null;

                    nombreEstandarSO = string.IsNullOrEmpty(_SO) ? entidadAplicacion != null ? entidadAplicacion.SWBase_SO : _SO : _SO;
                    nombreEstandarHP = string.IsNullOrEmpty(_HP) ? entidadAplicacion != null ? entidadAplicacion.SWBase_HP : _HP : _HP;
                    nombreEstandarBD = string.IsNullOrEmpty(_BD) ? entidadAplicacion != null ? entidadAplicacion.SWBase_BD : _BD : _BD;
                    nombreEstandarFw = string.IsNullOrEmpty(_FW) ? entidadAplicacion != null ? entidadAplicacion.SWBase_Framework : _FW : _FW;

                    //SO
                    var estandarSO = GetEstandarPortafolioTecnologiaByTipoEstandar(ctx, (int)ETipoEstandarPortafolio.SistemaOperativo, nombreEstandarSO);

                    puntuacion = estandarSO != null ? flagPc == 1 ? estandarSO.PuntuacionEstacion.Value : estandarSO.PuntuacionServidor.Value : 0;
                    porcentaje = CalculoPorcentaje((int)ETipoEstandarPortafolio.SistemaOperativo, nombreEstandarSO, nombreEstandarHP, nombreEstandarBD, nombreEstandarFw,
                    L, M, N);

                    calculoSO = puntuacion * porcentaje;

                    //HP
                    var estandarHP = GetEstandarPortafolioTecnologiaByTipoEstandar(ctx, (int)ETipoEstandarPortafolio.HerramientaProgramacion, nombreEstandarHP);

                    puntuacion = estandarHP != null ? flagPc == 1 ? estandarHP.PuntuacionEstacion.Value : estandarHP.PuntuacionServidor.Value : 0;
                    porcentaje = CalculoPorcentaje((int)ETipoEstandarPortafolio.HerramientaProgramacion, nombreEstandarSO, nombreEstandarHP, nombreEstandarBD, nombreEstandarFw,
                    L, M, N);

                    calculoHP = puntuacion * porcentaje;

                    //BD
                    var estandarBD = GetEstandarPortafolioTecnologiaByTipoEstandar(ctx, (int)ETipoEstandarPortafolio.GestorBaseDatos, nombreEstandarBD);

                    puntuacion = estandarBD != null ? flagPc == 1 ? estandarBD.PuntuacionEstacion.Value : estandarBD.PuntuacionServidor.Value : 0;
                    porcentaje = CalculoPorcentaje((int)ETipoEstandarPortafolio.GestorBaseDatos, nombreEstandarSO, nombreEstandarHP, nombreEstandarBD, nombreEstandarFw,
                    L, M, N);

                    calculoBD = puntuacion * porcentaje;

                    //FW
                    var estandarFW = GetEstandarPortafolioTecnologiaByTipoEstandar(ctx, (int)ETipoEstandarPortafolio.Framework, nombreEstandarFw);

                    puntuacion = estandarFW != null ? flagPc == 1 ? estandarFW.PuntuacionEstacion.Value : estandarFW.PuntuacionServidor.Value : 0;
                    porcentaje = CalculoPorcentaje((int)ETipoEstandarPortafolio.Framework, nombreEstandarSO, nombreEstandarHP, nombreEstandarBD, nombreEstandarFw,
                    L, M, N);

                    calculoFW = puntuacion * porcentaje;

                    calculoTotal = calculoSO + calculoHP + calculoBD + calculoFW;
                    calculoTotal = Utilitarios.TruncateDouble(calculoTotal, 2) * 100;

                    int temp1;
                        //, temp2 = 0;

                    var NCET = calculoTotal < 0 ? "0" : calculoTotal.ToString();
                    //var NCLS = ncls; //TODO tiene que venir de algun lado

                    var ncet_tmp = int.TryParse(NCET, out temp1) ? temp1 : 0;
                    var ncls_tmp = ncls;
                    var ncg_tmp = (ncet_tmp + ncls_tmp) / 2;

                    if (entidadAplicacion != null)
                    {
                        entidadAplicacion.NCET = calculoTotal < 0 ? "0" : calculoTotal.ToString();
                        entidadAplicacion.NCG = ncg_tmp.ToString();
                        ctx.SaveChanges();
                    }

                    GCN.NCG = ncg_tmp;
                    GCN.NCET = ncet_tmp;
                    GCN.NCLS = ncls_tmp;

                    return GCN;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> CalcularPorcentajeEstandar(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> CalcularPorcentajeEstandar(string filtro)"
                    , new object[] { null });
            }
        }

        private double CalculoPorcentaje(int tipoEstandarId, string nombreEstandarSO, string nombreEstandarHP, string nombreEstandarBD, string nombreEstandarFw,
            double[] L, double[] M, double N)
        {
            string NO_APLICA = "NO APLICA";
            double porcentaje = 0;

            switch (tipoEstandarId)
            {
                case (int)ETipoEstandarPortafolio.SistemaOperativo:
                    if (nombreEstandarSO.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = 0;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA) && nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = M[1];
                    }
                    else if (nombreEstandarHP.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = N;
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarHP.ToUpper().Equals(NO_APLICA) && !nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                       && !nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = L[1];
                    }
                    else
                    {
                        porcentaje = L[0];
                    }

                    break;
                case (int)ETipoEstandarPortafolio.HerramientaProgramacion:
                    if (nombreEstandarHP.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = 0;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA) && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[1];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = N;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarHP.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                        && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && !nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = L[1];
                    }
                    else
                    {
                        porcentaje = L[0];
                    }
                    break;
                case (int)ETipoEstandarPortafolio.GestorBaseDatos:
                    if (nombreEstandarBD.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = 0;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA) && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[1];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = N;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && !nombreEstandarBD.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (nombreEstandarHP.ToUpper().Equals(NO_APLICA) && !nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && !nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = L[1];
                    }
                    else
                    {
                        porcentaje = L[0];
                    }

                    break;
                case (int)ETipoEstandarPortafolio.Framework:
                    if (nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = 0;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA) && nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[1];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && nombreEstandarBD.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = N;
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && !nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (nombreEstandarHP.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && !nombreEstandarFw.ToUpper().Equals(NO_APLICA)) //OK
                    {
                        porcentaje = M[0];
                    }
                    else if (nombreEstandarSO.ToUpper().Equals(NO_APLICA) && nombreEstandarBD.ToUpper().Equals(NO_APLICA)
                        && !nombreEstandarFw.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = M[0];
                    }
                    else if (!nombreEstandarSO.ToUpper().Equals(NO_APLICA) && !nombreEstandarHP.ToUpper().Equals(NO_APLICA)
                       && !nombreEstandarBD.ToUpper().Equals(NO_APLICA))
                    {
                        porcentaje = L[1];
                    }
                    else
                    {
                        porcentaje = L[0];
                    }
                    break;
            }

            return porcentaje;
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.TipoActivoInformacion
                                  where u.TipoActivoInformacionId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = FECHA_ACTUAL;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;
                        ctx.SaveChanges();

                        /*var detalleHtml = $"--Detalle de campos-- <br/>" +
                                $"* Se cambió el estado del registro";

                        var auditoriaItem = new AuditoriaTipoActivo()
                        {
                            Accion = "U",
                            Entidad = itemBD.Nombre,
                            Usuario = usuario,
                            Campo = detalleHtml,
                            FlagActivo = true,
                            UsuarioCreacion = usuario,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.AuditoriaTipoActivo.Add(auditoriaItem);
                        ctx.SaveChanges();*/

                        retorno = !retorno;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoArea(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.Area.FirstOrDefault(x => x.AreaId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoAreaBian(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.AreaBian.FirstOrDefault(x => x.AreaBianId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoDivision(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.Division.FirstOrDefault(x => x.DivisionId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoDominioBian(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.DominioBian.FirstOrDefault(x => x.DominioBianId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoGerencia(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.Gerencia.FirstOrDefault(x => x.GerenciaId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoJefaturaAti(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.JefaturaAti.FirstOrDefault(x => x.JefaturaAtiId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoProcesoVital(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ProcesoVital.FirstOrDefault(x => x.ProcesoVitalId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoUnidad(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.Unidad.FirstOrDefault(x => x.UnidadId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteMatriculaEnBandeja(string filtro, int bandejaId, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.BandejaAprobacion
                                    where u.FlagActivo
                                    && u.MatriculaAprobador.ToUpper().Equals(filtro.ToUpper())
                                    && u.BandejaId == bandejaId
                                    && u.BandejaAprobacionId != id
                                    orderby u.MatriculaAprobador
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<ActivosDTO> GetActivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoActivoInformacion
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     && !u.FlagEliminado.Value
                                     orderby u.Nombre ascending
                                     select new ActivosDTO()
                                     {
                                         Id = u.TipoActivoInformacionId,
                                         FlujoRegistro = u.FlujoRegistro,
                                         Descripcion = u.Descripcion,
                                         Nombre = u.Nombre,
                                         FlujoRegistroNombre = u.FlujoRegistro == 1 ? "FNA" : "PAE",
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         FechaModificacion = u.FechaModificacion,
                                         FlagUserIT = u.FlagUserIT.HasValue ? u.FlagUserIT.Value : false
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetActivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetActivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ActivosDTO> GetActivos()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoActivoInformacion
                                     where u.FlagActivo
                                     select new ActivosDTO()
                                     {
                                         Id = u.TipoActivoInformacionId,
                                         FlujoRegistro = u.FlujoRegistro,
                                         Descripcion = u.Descripcion,
                                         Nombre = u.Nombre,
                                         FlagUserIT = u.FlagUserIT.HasValue ? u.FlagUserIT.Value : false
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivossDTO> GetActivos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetActivos()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetActivosByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoActivoInformacion
                                   where /*u.FlagActivo &&*/
                                   (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoActivoInformacionId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre,
                                       FlagActivo = u.FlagActivo                                       
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override ActivosDTO GetActivosById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoActivoInformacion

                                   where u.TipoActivoInformacionId == id
                                   select new ActivosDTO()
                                   {
                                       Id = u.TipoActivoInformacionId,
                                       Descripcion = u.Descripcion,
                                       Nombre = u.Nombre,
                                       FlujoRegistro = u.FlujoRegistro,
                                       Activo = u.FlagActivo,
                                       //FechaCreacion = u.FechaCreacion.HasValue? u.FechaCreacion.Value : DateTime.Now,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       FlagUserIT = u.FlagUserIT.HasValue ? u.FlagUserIT.Value : false,
                                       FlagExterna = u.FlagExterna.HasValue ? u.FlagExterna.Value : false
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ActivosDTO GetActivosById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ParametricasDTO GetActivosById(int id)"
                    , new object[] { null });
            }
        }

        public override ActivosDTO GetActivosByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoActivoInformacion
                                   where u.Nombre.ToUpper().Equals(nombre.ToUpper()) && u.FlagActivo
                                   select new ActivosDTO()
                                   {
                                       Id = u.TipoActivoInformacionId,
                                       Nombre = u.Nombre,
                                       FlujoRegistro = u.FlujoRegistro,
                                       FlagUserIT = u.FlagUserIT.HasValue ? u.FlagUserIT.Value : false
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ActivosDTO GetActivosById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ParametricasDTO GetActivosById(int id)"
                    , new object[] { null });
            }
        }

        public override List<AreaDTO> GetArea(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Area
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.DivisionId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new AreaDTO()
                                     {
                                         Id = u.AreaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.Area,
                                         Nivel = (int)ETreeLevel.Tres,
                                         EntidadRelacionId = u.DivisionId,
                                         FlagEditar = u.FlagEditar
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AreaBianDTO> GetAreaBian(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AreaBian
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado
                                     select new AreaBianDTO()
                                     {
                                         Id = u.AreaBianId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.AreaBian,
                                         Nivel = (int)ETreeLevel.Uno,
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAreaBianByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AreaBian
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.AreaBianId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override AreaBianDTO GetAreaBianById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AreaBian
                                   where u.AreaBianId == id
                                   select new AreaBianDTO()
                                   {
                                       Id = u.AreaBianId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       Correo = u.Correo,
                                       NombreResponsable = u.NombreResponsable,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.AreaBian
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAreaByFiltro(string filtro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        var item = ctx.Division.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                        id = item != null ? item.DivisionId : 1;
                    }

                    var entidad = (from u in ctx.Area
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.DivisionId == id)
                                   //&& (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.AreaId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override AreaDTO GetAreaById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Area
                                   where u.AreaId == id
                                   select new AreaDTO()
                                   {
                                       Id = u.AreaId,
                                       EntidadRelacionId = u.DivisionId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       FlagEditar = u.FlagEditar,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.Area,
                                       CodigoSIGA = u.CodigoSIGA,
                                       ResponsableCorreo = u.ResponsableCorreo,
                                       ResponsableMatricula = u.ResponsableMatricula
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<BandejaDTO> GetBandeja(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var IdsBandeja = new int[] {
                    (int)EBandejaAprobadorAplicacion.ArquitecturaTI,
                    (int)EBandejaAprobadorAplicacion.PO,
                    (int)EBandejaAprobadorAplicacion.TTL,
                    (int)EBandejaAprobadorAplicacion.GestorUserIT
                };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Bandeja
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !IdsBandeja.Contains(u.BandejaId)
                                     select new BandejaDTO()
                                     {
                                         Id = u.BandejaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.Bandeja,
                                         Nivel = (int)ETreeLevel.Uno
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
                    , "Error en el metodo: List<ActivosDTO> GetBandeja(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetBandeja(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<BandejaAprobacionDTO> GetBandejaAprobacion(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.BandejaAprobacion
                                     where (pag.id == 0 || u.BandejaId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new BandejaAprobacionDTO()
                                     {
                                         Id = u.BandejaAprobacionId,
                                         MatriculaAprobador = u.MatriculaAprobador,
                                         Nombre = u.MatriculaAprobador, //A eliminar
                                         Descripcion = u.MatriculaAprobador, //A eliminar
                                         FlagValidarMatricula = u.FlagValidarMatricula,
                                         Correo = u.Correo,
                                         Nombres = u.Nombres,
                                         EntidadRelacionId = u.BandejaId,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.BandejaAprobacion,
                                         Nivel = (int)ETreeLevel.Dos
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
                    , "Error en el metodo: List<ActivosDTO> GetPreguntaPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetPreguntaPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override BandejaAprobacionDTO GetBandejaAprobacionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.BandejaAprobacion
                                   where u.BandejaAprobacionId == id
                                   select new BandejaAprobacionDTO()
                                   {
                                       Id = u.BandejaAprobacionId,
                                       EntidadRelacionId = u.BandejaId,
                                       Correo = u.Correo,
                                       Nombres = u.Nombres,
                                       MatriculaAprobador = u.MatriculaAprobador,
                                       FlagValidarMatricula = u.FlagValidarMatricula,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.BandejaAprobacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: BandejaAprobacionDTO GetBandejaAprobacionById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: BandejaAprobacionDTO GetBandejaAprobacionById(int id)"
                    , new object[] { null });
            }
        }

        public override BandejaDTO GetBandejaById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Bandeja
                                   where u.BandejaId == id
                                   select new BandejaDTO()
                                   {
                                       Id = u.BandejaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.Bandeja
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: BandejaDTO GetBandejaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: BandejaDTO GetBandejaById(int id)"
                    , new object[] { null });
            }
        }

        public override CuestionarioAplicacionDTO GetCuestionarioAplicacionByCodigoAPT(string codigoAPT)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.CuestionarioAplicacion
                                   where u.CodigoAPT.Equals(codigoAPT)
                                   select new CuestionarioAplicacionDTO()
                                   {
                                       Id = u.CuestionarioAplicacionId,
                                       CodigoAPT = u.CodigoAPT,
                                       GestorAplicacion = u.GestorAplicacion,
                                       Consultor = u.Consultor,
                                       ConfidencialidadCalculo = u.ConfidencialidadCalculo,
                                       IntegridadCalculo = u.IntegridadCalculo,
                                       DisponibilidadCalculo = u.DisponibilidadCalculo,
                                       PrivacidadCalculo = u.PrivacidadCalculo,
                                       ConfidencialidadDescripcion = u.ConfidencialidadDescripcion,
                                       IntegridadDescripcion = u.IntegridadDescripcion,
                                       DisponibilidadDescripcion = u.DisponibilidadDescripcion,
                                       PrivacidadDescripcion = u.PrivacidadDescripcion,
                                       Clasificacion = u.Clasificacion,
                                       NivelCriticidad = u.NivelCriticidad,
                                       CriticidadFinal = u.CriticidadFinal,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();

                    if (entidad != null)
                    {
                        var listadoDetalle = (from x in ctx.CuestionarioAplicacionDetalle
                                              join b in ctx.PreguntaPae on x.PreguntaId equals b.PreguntaPaeId
                                              where x.CuestionarioAplicacionId == entidad.Id
                                              select new CuestionarioAplicacionDetalleDTO
                                              {
                                                  //Id = x.CuestionarioAplicacionDetalleId,
                                                  Id = x.PreguntaId,
                                                  PreguntaId = x.PreguntaId,
                                                  CuestionarioId = b.CuestionarioPaeId,
                                                  AlternativaSeleccionada = x.AlternativaSeleccionada,
                                                  Score = x.AlternativaSeleccionada
                                              }).ToList();

                        entidad.CuestionarioAplicacionDetalle = listadoDetalle;
                    }

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetCuestionarioAplicacionByCodigoAPT(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetCuestionarioAplicacionByCodigoAPT(int id)"
                    , new object[] { null });
            }
        }

        public override List<CuestionarioPaeDTO> GetCuestionarioPae(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.CuestionarioPae
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado.Value && (pag.Activos ? u.FlagActivo == true : u.FlagActivo == u.FlagActivo)
                                     select new CuestionarioPaeDTO()
                                     {
                                         Id = u.CuestionarioPaeId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.CuestionarioPae,
                                         Nivel = (int)ETreeLevel.Uno
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                    if (!string.IsNullOrEmpty(pag.codigoAPT))
                    {
                        var item = ctx.CuestionarioAplicacion.FirstOrDefault(x => x.FlagActivo && x.CodigoAPT.ToUpper().Equals(pag.codigoAPT.ToUpper()));
                        if (item != null)
                        {
                            foreach (var reg in registros)
                            {
                                switch (reg.Id)
                                {
                                    case (int)ECuestionarioPae.Confidencialidad:
                                        reg.SeccionCalculo = item.ConfidencialidadCalculo;
                                        break;
                                    case (int)ECuestionarioPae.Integridad:
                                        reg.SeccionCalculo = item.IntegridadCalculo;
                                        break;
                                    case (int)ECuestionarioPae.Disponibilidad:
                                        reg.SeccionCalculo = item.DisponibilidadCalculo;
                                        break;
                                    case (int)ECuestionarioPae.Privacidad:
                                        reg.SeccionCalculo = item.PrivacidadCalculo;
                                        break;
                                }
                            }
                        }
                    }

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetCuestionarioPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetCuestionarioPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override CuestionarioPaeDTO GetCuestionarioPaeById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.CuestionarioPae
                                   where u.CuestionarioPaeId == id
                                   select new CuestionarioPaeDTO()
                                   {
                                       Id = u.CuestionarioPaeId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.CuestionarioPae
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: CuestionarioPaeDTO GetCuestionarioPaeById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: CuestionarioPaeDTO GetCuestionarioPaeById(int id)"
                    , new object[] { null });
            }
        }

        public override List<DivisionDTO> GetDivision(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Division
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.GerenciaId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new DivisionDTO()
                                     {
                                         Id = u.DivisionId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.Division,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.GerenciaId,
                                         FlagEditar = u.FlagEditar
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
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetDivisionByFiltro(string filtro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        var item = ctx.Gerencia.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                        id = item != null ? item.GerenciaId : 1;
                    }

                    var entidad = (from u in ctx.Division
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.GerenciaId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.DivisionId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override DivisionDTO GetDivisionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Division
                                   where u.DivisionId == id
                                   select new DivisionDTO()
                                   {
                                       Id = u.DivisionId,
                                       EntidadRelacionId = u.GerenciaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       ResponsableMatricula = u.ResponsableMatricula,
                                       ResponsableCorreo = u.ResponsableCorreo,
                                       CodigoSIGA = u.CodigoSIGA,
                                       FlagEditar = u.FlagEditar,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.Division
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<DominioBianDTO> GetDominioBian(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.DominioBian
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.AreaBianId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new DominioBianDTO()
                                     {
                                         Id = u.DominioBianId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.DominioBian,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.AreaBianId
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetDominioBianByFiltro(string filtro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        var item = ctx.AreaBian.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                        id = item != null ? item.AreaBianId : 1;
                    }

                    var entidad = (from u in ctx.DominioBian
                                   where u.FlagActivo
                                   //&& (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   && (string.IsNullOrEmpty(filtro) || u.AreaBianId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.DominioBianId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override DominioBianDTO GetDominioBianById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.DominioBian
                                   where u.DominioBianId == id
                                   select new DominioBianDTO()
                                   {
                                       Id = u.DominioBianId,
                                       EntidadRelacionId = u.AreaBianId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       Correo = u.Correo,
                                       NombreResponsable = u.NombreResponsable,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.DominioBian,
                                       AreaBIANId = u.AreaBianId
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<EstandarDTO> GetEstandarPortafolio(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Estandar
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado.Value
                                     select new EstandarDTO()
                                     {
                                         Id = u.EstandarId,
                                         TipoEstandarId = u.TipoEstandarId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         PuntuacionServidor = u.PuntuacionServidor,
                                         PuntuacionEstacion = u.PuntuacionEstacion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
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
                    , "Error en el metodo: List<ActivosDTO> GetEstandarPortafolio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetEstandarPortafolio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<FlujosRegistroDTO> GetFlujos()
        {
            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    List<FlujosRegistroDTO> lista = new List<FlujosRegistroDTO>();
                    int[] itemValues = (int[])System.Enum.GetValues(typeof(EFlujoRegistro));
                    string[] itemNames = System.Enum.GetNames(typeof(EFlujoRegistro));

                    for (int i = 0; i <= itemNames.Length - 1; i++)
                    {
                        FlujosRegistroDTO obj = new FlujosRegistroDTO();
                        obj.Id = itemValues[i];
                        obj.Descripcion = itemNames[i];
                        lista.Add(obj);
                    }

                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<FlujosRegistroDTO> GetFlujos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<FlujosRegistroDTO> GetFlujos()"
                    , new object[] { null });
            }
        }

        public override List<GerenciaDTO> GetGerencia(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Gerencia
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado.Value
                                     select new GerenciaDTO()
                                     {
                                         Id = u.GerenciaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.Gerencia,
                                         Nivel = (int)ETreeLevel.Uno,
                                         FlagEditar = u.FlagEditar
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
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Gerencia
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.GerenciaId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override GerenciaDTO GetGerenciaById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Gerencia
                                   where u.GerenciaId == id
                                   select new GerenciaDTO()
                                   {
                                       Id = u.GerenciaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       ResponsableMatricula = u.ResponsableMatricula,
                                       ResponsableCorreo = u.ResponsableCorreo,
                                       CodigoSIGA = u.CodigoSIGA,
                                       FlagEditar = u.FlagEditar,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.Gerencia
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<JefaturaAtiDTO> GetJefaturaAti(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.JefaturaAti
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.DominioBianId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new JefaturaAtiDTO()
                                     {
                                         Id = u.JefaturaAtiId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.JefaturaAti,
                                         Nivel = (int)ETreeLevel.Uno,
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro1, string filtro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        //var item = ctx.DominioBian.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                        var item = (from x in ctx.DominioBian
                                    join y in ctx.AreaBian on x.AreaBianId equals y.AreaBianId
                                    where x.FlagActivo && y.FlagActivo
                                    && y.Nombre.ToUpper().Equals(filtro1.ToUpper())
                                    && x.Nombre.ToUpper().Equals(filtro.ToUpper())
                                    select x).FirstOrDefault();

                        id = item != null ? item.DominioBianId : 1;
                    }

                    var entidad = (from u in ctx.JefaturaAti
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.DominioBianId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.JefaturaAtiId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override JefaturaAtiDTO GetJefaturaAtiById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.JefaturaAti
                                   where u.JefaturaAtiId == id
                                   select new JefaturaAtiDTO()
                                   {
                                       Id = u.JefaturaAtiId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       Correo = u.Correo,
                                       NombreResponsable = u.NombreResponsable,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.JefaturaAti
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<PreguntaPaeDTO> GetPreguntaPae(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.PreguntaPae
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.CuestionarioPaeId == pag.id)
                                     && !u.FlagEliminado.Value && (pag.Activos ? u.FlagActivo == true : u.FlagActivo == u.FlagActivo)
                                     select new PreguntaPaeDTO()
                                     {
                                         Id = u.PreguntaPaeId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.PreguntaPae,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.CuestionarioPaeId
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                    if (!string.IsNullOrEmpty(pag.codigoAPT))
                    {
                        var itemsWithScore = (from x in ctx.CuestionarioAplicacionDetalle
                                              join y in ctx.CuestionarioAplicacion on x.CuestionarioAplicacionId equals y.CuestionarioAplicacionId
                                              where y.CodigoAPT == pag.codigoAPT
                                              && x.FlagActivo
                                              select x).ToList();

                        if (itemsWithScore != null && itemsWithScore.Count > 0)
                        {
                            foreach (var reg in registros)
                            {
                                var item = itemsWithScore.FirstOrDefault(x => x.PreguntaId == reg.Id);
                                if (item != null)
                                    reg.Score = item.AlternativaSeleccionada.Value;
                            }
                        }
                    }

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetPreguntaPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetPreguntaPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override PreguntaPaeDTO GetPreguntaPaeById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.PreguntaPae
                                   where u.PreguntaPaeId == id
                                   select new PreguntaPaeDTO()
                                   {
                                       Id = u.PreguntaPaeId,
                                       EntidadRelacionId = u.CuestionarioPaeId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.PreguntaPae
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<ProcesoVitalDTO> GetProcesoVital(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ProcesoVital
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado.Value
                                     select new ProcesoVitalDTO()
                                     {
                                         Id = u.ProcesoVitalId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         FlagProcesoVital = u.FlagProcesoVital,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
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

        public override List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ProcesoVital
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.ProcesoVitalId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override ProcesoVitalDTO GetProcesoVitalById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ProcesoVital
                                   where u.ProcesoVitalId == id
                                   select new ProcesoVitalDTO()
                                   {
                                       Id = u.ProcesoVitalId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       FlagProcesoVital = u.FlagProcesoVital,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
        }

        public override int GetPuntuacionByEstandar(int tipoEstandarId, string nombreEstandar, int flagPc)
        {
            try
            {
                //flag tiene que ser >= 0
                int puntuacion = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = ctx.Estandar.FirstOrDefault(x => x.FlagActivo
                                        && x.TipoEstandarId == tipoEstandarId
                                        && x.Nombre.ToUpper().Equals(nombreEstandar.ToUpper()));

                    puntuacion = entidad != null ? flagPc == 1 ? entidad.PuntuacionEstacion.Value : entidad.PuntuacionServidor.Value : 0;

                    return puntuacion;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: int GetPuntuacionByEstandar(int tipoEstandarId, string nombreEstandar, int flagPc)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: int GetPuntuacionByEstandar(int tipoEstandarId, string nombreEstandar, int flagPc)"
                    , new object[] { null });
            }
        }

        public override List<UnidadDTO> GetUnidad(Paginacion pag, out int totalRows)
        {
            if (pag.nombre == null)
            {

                pag.nombre = "";
            }
            pag.areaId = pag.id;
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
              
                    var registros = (from u in ctx.Unidad
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && ( u.AreaId == pag.areaId)
                                     && !u.FlagEliminado.Value
                                     select new UnidadDTO()
                                     {
                                         Id = u.UnidadId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.Unidad,
                                         Nivel = (int)ETreeLevel.Cuatro,
                                         EntidadRelacionId = u.AreaId,
                                         FlagEditar = u.FlagEditar
                                     }).ToList();

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetDivision(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetUnidadByFiltro(string filtroUnidad, string filtroPadre)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtroPadre))
                    {
                        var item = ctx.Area.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtroPadre.ToUpper()));
                        id = item != null ? item.AreaId : 1;
                    }

                    var entidad = (from u in ctx.Unidad
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtroUnidad) || u.Nombre.ToUpper().Contains(filtroUnidad.ToUpper()))
                                   && (string.IsNullOrEmpty(filtroPadre) || u.AreaId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.UnidadId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override UnidadDTO GetUnidadById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Unidad
                                   where u.UnidadId == id
                                   select new UnidadDTO()
                                   {
                                       Id = u.UnidadId,
                                       EntidadRelacionId = u.AreaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Responsable = u.Responsable,
                                       ResponsableCorreo = u.ResponsableCorreo,
                                       ResponsableMatricula = u.ResponsableMatricula,
                                       CodigoSIGA = u.CodigoSIGA,
                                       FlagEditar = u.FlagEditar,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.Unidad
                                   }).FirstOrDefault();

                    if(entidad!=null)
                    {
                        var result = ServiceManager<ApplicationDAO>.Provider.BuscarEnUnidad(entidad.ResponsableMatricula);
                        if (result != null)
                        {
                            entidad.Situacion = result.Resultado == 1 ? "RESPONSABLE" : (result.Resultado == 2 ? "RESPONSABLE" : "ASOCIADO");
                            entidad.Unidad = result.Resultado == 1 || result.Resultado == 2 ? entidad.Nombre : result.Nombre;
                        }
                    }
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetAreaById(int id)"
                    , new object[] { null });
            }
        }

        public override bool? ProcesoClaveEsVital(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = ctx.ProcesoVital.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                    return entidad?.FlagProcesoVital;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> ProcesoClaveEsVital(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> ProcesoClaveEsVital(string filtro)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditEstandarPortafolio(EstandarDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Estandar()
                        {
                            EstandarId = objeto.Id,
                            TipoEstandarId = objeto.TipoEstandarId,
                            PuntuacionEstacion = objeto.PuntuacionEstacion,
                            PuntuacionServidor = objeto.PuntuacionServidor,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.Estandar.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.EstandarId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Estandar
                                       where u.EstandarId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.TipoEstandarId = objeto.TipoEstandarId;
                            entidad.PuntuacionEstacion = objeto.PuntuacionEstacion;
                            entidad.PuntuacionServidor = objeto.PuntuacionServidor;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.EstandarId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override EstandarDTO GetEstandarPortafolioById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Estandar
                                   where u.EstandarId == id
                                   select new EstandarDTO()
                                   {
                                       Id = u.EstandarId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       TipoEstandarId = u.TipoEstandarId,
                                       PuntuacionEstacion = u.PuntuacionEstacion,
                                       PuntuacionServidor = u.PuntuacionServidor,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoEstandarPortafolio(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.Estandar.FirstOrDefault(x => x.EstandarId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ServidorAplicacionDTO> GetServidorAplicacion(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ServidorAplicacion
                                     where (string.IsNullOrEmpty(pag.codigoAPT) || u.CodigoAPT.ToUpper().Contains(pag.codigoAPT.ToUpper()))
                                     && u.FlagActivo
                                     select new ServidorAplicacionDTO()
                                     {
                                         Id = u.ServidorAplicacionId,
                                         NombreServidor = u.NombreServidor,
                                         CodigoAPT = u.CodigoAPT,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
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
                    , "Error en el metodo: List<ActivosDTO> GetCuestionarioPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetCuestionarioPae(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditServidorAplicacion(ServidorAplicacionDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new ServidorAplicacion()
                        {
                            ServidorAplicacionId = objeto.Id,
                            NombreServidor = objeto.NombreServidor,
                            CodigoAPT = objeto.CodigoAPT,
                            FlagActivo = true,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.ServidorAplicacion.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.ServidorAplicacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ServidorAplicacion
                                       where u.ServidorAplicacionId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.NombreServidor = objeto.NombreServidor;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.ServidorAplicacionId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoServidorAplicacion(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ServidorAplicacion.FirstOrDefault(x => x.ServidorAplicacionId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteServidorByCodigApt(string codigoApt, string nombreServidor)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.ServidorAplicacion
                                    where u.FlagActivo
                                    && u.CodigoAPT.ToUpper().Equals(codigoApt.ToUpper())
                                    && u.NombreServidor.ToUpper().Equals(nombreServidor.ToUpper())
                                    orderby u.CodigoAPT
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoCuestionarioPae(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.CuestionarioPae.FirstOrDefault(x => x.CuestionarioPaeId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoPreguntaPae(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.PreguntaPae.FirstOrDefault(x => x.PreguntaPaeId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ArquitectoTiDTO> GetArquitectoTi(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ArquitectoTI
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.JefaturaAtiId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new ArquitectoTiDTO()
                                     {
                                         Id = u.ArquitectoTIId,
                                         Responsable = u.Matricula,
                                         NombreResponsable = u.Nombre,
                                         Correo = u.Correo,
                                         FlagValidarMatricula = u.FlagValidarMatricula,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.ArquitectoTi,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.JefaturaAtiId
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditArquitectoTi(ArquitectoTiDTO objeto)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidadRegistro = ctx.ArquitectoTI.FirstOrDefault(x => x.Correo == objeto.Correo && x.JefaturaAtiId == objeto.EntidadRelacionId);
                        if (entidadRegistro == null)
                        {
                            var entidad = new ArquitectoTI()
                            {
                                ArquitectoTIId = objeto.Id,
                                JefaturaAtiId = objeto.EntidadRelacionId,
                                Matricula = objeto.Matricula,
                                Correo = objeto.Correo,
                                Nombre = objeto.NombreResponsable,
                                FlagValidarMatricula = true,
                                FlagActivo = true,
                                FlagEliminado = false,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                UsuarioModificacion = objeto.UsuarioCreacion,
                                FechaModificacion = FECHA_ACTUAL,
                                FechaCreacion = FECHA_ACTUAL,
                                FlagEncargadoJefatura = objeto.FlagEncargadoJefatura
                            };
                            ctx.ArquitectoTI.Add(entidad);
                            ctx.SaveChanges();

                            ID = entidad.ArquitectoTIId;
                        }
                        else 
                        {
                            var sql = string.Format("update app.ArquitectoTI set FlagActivo=1 , FlagEliminado=0 where ArquitectoTIId={0}", entidadRegistro.ArquitectoTIId);
                            ctx.Database.ExecuteSqlCommand(sql);
                            ID = entidadRegistro.ArquitectoTIId;
                        }
                    }
                    else
                    {
                        var entidad = (from u in ctx.ArquitectoTI
                                       where u.ArquitectoTIId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Matricula = objeto.Matricula;
                            entidad.Correo = objeto.Correo;
                            entidad.Nombre = objeto.NombreResponsable;
                            entidad.FlagValidarMatricula = true;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.FlagEncargadoJefatura = objeto.FlagEncargadoJefatura;
                            ctx.SaveChanges();

                            ID = entidad.ArquitectoTIId;
                        }
                    }                    

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override ArquitectoTiDTO GetArquitectoTiById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ArquitectoTI
                                   where u.ArquitectoTIId == id
                                   select new ArquitectoTiDTO()
                                   {
                                       Id = u.ArquitectoTIId,
                                       EntidadRelacionId = u.JefaturaAtiId,
                                       NombreResponsable = u.Nombre,
                                       Responsable = u.Matricula,
                                       Correo = u.Correo,
                                       FlagValidarMatricula = u.FlagValidarMatricula,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.ArquitectoTi,
                                       Matricula = u.Matricula,
                                       FlagEncargadoJefatura=u.FlagEncargadoJefatura,
                                       Nombre = u.Nombre
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetArquitectoTiByFiltro(string jefaturaFiltro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(jefaturaFiltro))
                    {
                        var item = (from x in ctx.JefaturaAti
                                        //join y in ctx.DominioBian on x.DominioBianId equals y.DominioBianId
                                    where x.FlagActivo
                                    && x.Nombre.ToUpper().Equals(jefaturaFiltro.ToUpper())
                                    //&& y.FlagActivo
                                    //&& y.Nombre.ToUpper().Equals(dominioFiltro.ToUpper())
                                    select x).FirstOrDefault();

                        id = item != null ? item.JefaturaAtiId : 1;
                    }

                    var entidad = (from u in ctx.ArquitectoTI
                                   where /*u.FlagActivo &&*/
                                   (string.IsNullOrEmpty(jefaturaFiltro) || u.JefaturaAtiId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.ArquitectoTIId.ToString(),
                                       Descripcion = u.Matricula + " - " + u.Nombre,
                                       value = u.Nombre,
                                       FlagActivo = u.FlagActivo
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoArquitectoTi(int id, bool estado, string usuario)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ArquitectoTI.FirstOrDefault(x => x.ArquitectoTIId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        var lSolicitudesNoAprobadas = (from x in ctx.Solicitud
                                                       join y in ctx.Aplicacion on x.AplicacionId equals y.AplicacionId
                                                       join z in ctx.JefaturaAti on y.JefaturaATI.ToUpper() equals z.Nombre.ToUpper()
                                                       where !x.FlagAprobacion.Value && y.FlagActivo
                                                       && x.TipoSolicitud == (int)ETipoSolicitudAplicacion.CreacionAplicacion
                                                       && x.EstadoSolicitud != (int)EEstadoSolicitudAplicacion.Registrado
                                                       && z.JefaturaAtiId == itemBD.JefaturaAtiId
                                                       select new DataArquitectoAplicacion()
                                                       {
                                                           SolicitudId = x.SolicitudAplicacionId,
                                                           ArquitectoTI = y.ArquitectoTI
                                                       }).ToList();

                        if (lSolicitudesNoAprobadas != null && lSolicitudesNoAprobadas.Count > 0)
                        {
                            foreach (var item in lSolicitudesNoAprobadas)
                            {
                                ctx.SolicitudAprobadores.Where(x => x.FlagActivo && x.SolicitudAplicacionId == item.SolicitudId
                                && !x.FlagAprobado.Value
                                && x.BandejaId == (int)EBandejaAprobadorAplicacion.ArquitecturaTI).ToList()
                                .ForEach(x =>
                                {
                                    x.Matricula = estado ? string.IsNullOrWhiteSpace(x.Matricula) ? itemBD.Matricula : $"{x.Matricula}|{itemBD.Matricula}" : x.Matricula.Replace(itemBD.Matricula, "");
                                    x.UsuarioModificacion = usuario;
                                    x.FechaModificacion = FECHA_ACTUAL;
                                });
                            }
                        }

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteMatriculaEnJefaturaAti(string filtro, int entidadRelacionId, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.ArquitectoTI
                                    where u.FlagEliminado == false
                                    && u.Matricula.ToUpper().Equals(filtro.ToUpper())
                                    && u.ArquitectoTIId != id
                                    orderby u.Matricula
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override string GetUltimoCodigoAptPAE()
        {
            try
            {
                string retorno = "A001";
                int nextNumber = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ULTIMO_CODIGOAPT_PAE_PORTAFOLIO);
                    string dataParametro = parametro != null ? parametro.Valor : "";

                    if (!string.IsNullOrEmpty(dataParametro))
                    {
                        string letterCode = dataParametro.Substring(0, 1);
                        string numberCode = dataParametro.Substring(1, 3);

                        int currentNumber = int.TryParse(numberCode, out currentNumber) ? currentNumber : 0;

                        if (currentNumber == 999)
                        {
                            //Number
                            nextNumber = 1;

                            //Letter
                            int currentLetterPos = Utilitarios.GetPosByLetter(letterCode);
                            int nextLetterPos = currentLetterPos == (int)ELetraColumnaExcel.Z ? 1 : currentLetterPos + 1;
                            string nextLetterPosStr = Utilitarios.GetEnumDescription2((ELetraColumnaExcel)nextLetterPos);
                            retorno = $"{nextLetterPosStr}00{nextNumber}";
                        }
                        else
                        {
                            //Letter
                            int currentLetterPos = Utilitarios.GetPosByLetter(letterCode);
                            int nextLetterPos = currentLetterPos == (int)ELetraColumnaExcel.Z ? 1 : currentLetterPos;
                            string nextLetterPosStr = Utilitarios.GetEnumDescription2((ELetraColumnaExcel)nextLetterPos);

                            //Number
                            nextNumber = currentNumber + 1;
                            int numberOfDigits = Utilitarios.GetIntegerDigitCountString(nextNumber);
                            string numberStr = "";

                            if (numberOfDigits == 1)
                                numberStr = $"00{nextNumber}";
                            else if (numberOfDigits == 2)
                                numberStr = $"0{nextNumber}";
                            else
                                numberStr = $"{nextNumber}";

                            retorno = $"{nextLetterPosStr}{numberStr}";
                        }
                    }
                }

                return retorno;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override List<GestionadoPorDTO> GetGestionadoPor(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.GestionadoPor
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     //&& u.Visualizar.Value
                                     && u.FlagActivo == (pag.Activos == true)
                                     //&& u.FlagEliminado == (pag.Activos == true)
                                     select new GestionadoPorDTO()
                                     {
                                         Id = u.GestionadoPorId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         FlagEquipoAgil = u.FlagEquipoAgil,
                                         FlagSubsidiarias = u.FlagSubsidiarias,
                                         FlagJefeEquipo = u.FlagJefeEquipo,
                                         FlagUserIT = u.FlagUserIT,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
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

        public override int AddOrEditGestionadoPor(GestionadoPorDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new GestionadoPor()
                        {
                            GestionadoPorId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            CodigoSIGA = objeto.CodigoSIGA,
                            FlagEquipoAgil = objeto.FlagEquipoAgil,
                            FlagUserIT=objeto.FlagUserIT,
                            FlagSubsidiarias = objeto.FlagSubsidiarias,
                            FlagJefeEquipo = objeto.FlagJefeEquipo,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL,
                            Visualizar = true
                        };
                        ctx.GestionadoPor.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.GestionadoPorId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.GestionadoPor
                                       where u.GestionadoPorId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.CodigoSIGA = objeto.CodigoSIGA;
                            entidad.FlagEquipoAgil = objeto.FlagEquipoAgil;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.FlagUserIT = objeto.FlagUserIT;
                            entidad.FlagSubsidiarias = objeto.FlagSubsidiarias;
                            entidad.FlagJefeEquipo = objeto.FlagJefeEquipo;

                            ctx.SaveChanges();

                            ID = entidad.GestionadoPorId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override GestionadoPorDTO GetGestionadoPorById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.GestionadoPor
                                   where u.GestionadoPorId == id
                                   select new GestionadoPorDTO()
                                   {
                                       Id = u.GestionadoPorId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       CodigoSIGA = u.CodigoSIGA,
                                       FlagEquipoAgil = u.FlagEquipoAgil,
                                       FlagUserIT=u.FlagUserIT,
                                       FlagSubsidiarias=u.FlagSubsidiarias,
                                       FlagJefeEquipo = u.FlagJefeEquipo,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoGestionadoPor(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.GestionadoPor.FirstOrDefault(x => x.GestionadoPorId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetGestionadoPorByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.GestionadoPor
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.GestionadoPorId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override GestionadoPorDTO GetGestionadoPorByNombre(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.GestionadoPor
                                   where u.Nombre.ToUpper().Equals(filtro.ToUpper()) && u.FlagActivo
                                   select new GestionadoPorDTO()
                                   {
                                       Id = u.GestionadoPorId,
                                       FlagEquipoAgil = u.FlagEquipoAgil
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ActivosDTO GetGestionadoPorByNombre(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ParametricasDTO GetGestionadoPorByNombre(int id)"
                    , new object[] { null });
            }
        }

        private string GetNewInputCode()
        {
            try
            {
                string inputDefault = "txtNewField0";
                //string CODIGO_PARAMETRO = "ULTIMO_CAMPO_NUEVOID_PORTAFOLIO";
                //Obtener el ultimo codigo de campo nuevo creado
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ULTIMO_CAMPO_NUEVOID_PORTAFOLIO);
                var param_valor = parametro != null ? !string.IsNullOrEmpty(parametro.Valor) ? parametro.Valor : inputDefault : inputDefault;
                //Obtener el tamano del string
                int dimStr = param_valor.Length;
                string numberCode = param_valor.Substring(11, dimStr - 11);
                int currentNumber = int.TryParse(numberCode, out currentNumber) ? currentNumber : 0;
                int nextNumberCode = currentNumber + 1;
                string finalCode = $"txtNewField{nextNumberCode}";
                return finalCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override int AddOrEditInfoCampoPortafolio(InfoCampoPortafolioDTO objeto, GestionCMDB_ProdEntities context)
        {
            try
            {
                var ctx = context;
                int ID = 0;
                //string CODIGO_PARAMETRO = "ULTIMO_CAMPO_NUEVOID_PORTAFOLIO";
                DateTime FECHA_ACTUAL = DateTime.Now;
                if (objeto.Id == 0)
                {
                    var newCode = GetNewInputCode();

                    var entidad = new InfoCampoPortafolio()
                    {
                        InfoCampoPortafolioId = objeto.Id,
                        ConfiguracionColumnaAplicacionId = objeto.ConfiguracionColumnaAplicacionId,
                        Nombre = objeto.Nombre,
                        Codigo = newCode,
                        Descripción = objeto.Descripción,
                        ToolTip = objeto.ToolTip,
                        SeccionTabId = objeto.SeccionTabId,
                        TipoFlujoId = objeto.TipoFlujoId,
                        TipoInputId = objeto.TipoInputId,
                        FlagActivo = true,
                        FlagEliminado = false,
                        UsuarioCreacion = objeto.UsuarioModificacion,
                        UsuarioModificacion = objeto.UsuarioModificacion,
                        FechaModificacion = FECHA_ACTUAL,
                        FechaCreacion = FECHA_ACTUAL,
                        MantenimientoPortafolioId = objeto.MantenimientoPortafolioId,
                        FlagParametrica = false,
                        ParametricaDescripcion = objeto.ParametricaDescripcion
                    };
                    ctx.InfoCampoPortafolio.Add(entidad);
                    ctx.SaveChanges();

                    var updateParametro = new ParametroDTO()
                    {
                        Codigo = Utilitarios.ULTIMO_CAMPO_NUEVOID_PORTAFOLIO,
                        Valor = entidad.Codigo,
                        UsuarioModificacion = objeto.UsuarioModificacion
                    };
                    ServiceManager<ParametroDAO>.Provider.ActualizarParametroByCodigo(updateParametro, ctx);

                    ID = entidad.InfoCampoPortafolioId;
                }
                else
                {
                    var entidad = (from u in ctx.InfoCampoPortafolio
                                   where u.InfoCampoPortafolioId == objeto.Id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.Nombre = objeto.Nombre;
                        entidad.TipoFlujoId = objeto.TipoFlujoId;
                        entidad.ToolTip = objeto.ToolTip;
                        entidad.FechaModificacion = FECHA_ACTUAL;
                        entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                        ctx.SaveChanges();

                        ID = entidad.InfoCampoPortafolioId;
                    }
                }

                //Tipo input list box
                if (objeto.TipoInputId == (int)ETipoInputHTML.ListBox)
                {
                    //item data list box
                    var lDataListBox = objeto.DataListBoxDetalle;
                    var lDataListBoxEliminar = objeto.DataListBoxEliminar;

                    if (objeto.MantenimientoPortafolioId.Value == (int)EConfiguracionPortafolio.Parametrica)
                    {
                        //List box Parametrica
                        //delete parametricas
                        if (lDataListBoxEliminar != null && lDataListBoxEliminar.Count() > 0)
                        {
                            foreach (var idParametricaDetalle in lDataListBoxEliminar)
                                ServiceManager<ActivosDAO>.Provider.EliminarRegistroConfiguracion(idParametricaDetalle, objeto.MantenimientoPortafolioId.Value, objeto.UsuarioModificacion, objeto.UsuarioModificacion);
                        }

                        //Add or edit parametricas
                        if (lDataListBox != null && lDataListBox.Count > 0)
                        {
                            string codigoDescripcion = !string.IsNullOrEmpty(objeto.ParametricaDescripcion) ? objeto.ParametricaDescripcion : GetCodigoParametrica(ctx, ID);
                            foreach (var item in lDataListBox)
                            {
                                item.Id = int.Parse(item.Id) < 0 ? "-1" : item.Id;

                                var objData = new ParametricasDTO()
                                {
                                    Id = int.Parse(item.Id),
                                    ParametricaId = (int)EEntidadParametrica.PORTAFOLIO,
                                    Descripcion = codigoDescripcion,
                                    Valor = item.Valor,
                                    UsuarioCreacion = objeto.UsuarioCreacion,
                                    UsuarioModificacion = objeto.UsuarioModificacion
                                };

                                ServiceManager<ParametricasDAO>.Provider.AddOrEditParametricas(objData);
                            }
                        }
                    }
                }

                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        private string GetCodigoParametrica(GestionCMDB_ProdEntities _ctx, int idInfoCampo)
        {
            try
            {
                string cod_retorno = "";
                var ctx = _ctx;
                //using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                //{
                var state = true;
                while (state)
                {
                    var codigo = Utilitarios.RandomString(6);
                    var itemParametricaDetalle = ctx.ParametricaDetalle.FirstOrDefault(x => x.FlagActivo
                    && x.Descripcion.ToUpper().Equals(codigo.ToUpper()));
                    if (itemParametricaDetalle != null)
                        state = true;
                    else
                        state = false; cod_retorno = codigo;
                }

                //Actualizamos InfoCampoPortafolio
                var regICP = ctx.InfoCampoPortafolio.FirstOrDefault(x => x.FlagActivo && x.InfoCampoPortafolioId == idInfoCampo);
                if (regICP != null)
                {
                    regICP.FlagParametrica = true;
                    regICP.MantenimientoPortafolioId = (int)EConfiguracionPortafolio.Parametrica;
                    regICP.ParametricaDescripcion = cod_retorno;
                    ctx.SaveChanges();
                }
                //}
                return cod_retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<CustomAutocomplete> GetAllToolTipPortafolio(int? tipoFlujoId = null, bool? IsNewField = null, string codigoAPT = null)
        {
            try
            {
                var listTablaProcedenciaIds = new int[]
                {
                    (int)ETablaProcedenciaAplicacion.Aplicacion,
                    (int)ETablaProcedenciaAplicacion.AplicacionDetalle,
                    (int)ETablaProcedenciaAplicacion.InfoCampoPortafolio
                };
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lEntidad = (from u in ctx.ConfiguracionColumnaAplicacion
                                    join x in ctx.InfoCampoPortafolio on u.ConfiguracionColumnaAplicacionId equals x.ConfiguracionColumnaAplicacionId
                                    where u.FlagActivo && x.FlagActivo
                                    && (IsNewField == null || u.FlagCampoNuevo.Value == IsNewField)
                                    && (tipoFlujoId == null || x.TipoFlujoId.Contains(tipoFlujoId.Value.ToString()))
                                    && listTablaProcedenciaIds.Contains(u.TablaProcedencia.Value)
                                    orderby u.NombreBD
                                    select new CustomAutocomplete()
                                    {
                                        Id = u.ConfiguracionColumnaAplicacionId.ToString(),
                                        Codigo = x.Codigo,
                                        Descripcion = u.FlagCampoNuevo.Value ? u.NombreExcel : u.NombreBD,
                                        value = x.ToolTip,
                                        FlagNuevo = u.FlagCampoNuevo,
                                        FlagObligatorio = u.FlagObligatorio,
                                        FlagMostrarCampo = u.FlagMostrarCampo,
                                        TipoFlujoId = x.TipoFlujoId,
                                        TipoInputId = x.TipoInputId,
                                        InfoCampoId = x.InfoCampoPortafolioId,
                                        ValorCampo = ""
                                    }).ToList();

                    if (!string.IsNullOrEmpty(codigoAPT))
                    {
                        var lDataCampo = ctx.NuevoCampoPortafolio.Where(x => x.FlagActivo
                        && x.CodigoAPT.ToUpper().Equals(codigoAPT.ToUpper())).ToList();

                        if (lDataCampo != null && lDataCampo.Count > 0)
                        {
                            foreach (var item in lEntidad)
                            {
                                var element = lDataCampo.FirstOrDefault(x => x.InfoCampoPortafolioId == item.InfoCampoId);
                                if (element != null)
                                    item.ValorCampo = element.Valor;
                            }
                        }
                    }

                    return lEntidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<AuditoriaTipoActivoDTO> GetHistoricoModificacionTAI(PaginacionHistoricoModificacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.AuditoriaTipoActivo
                                         where (string.IsNullOrEmpty(pag.Entidad) || u.Entidad.ToUpper() == pag.Entidad.ToUpper())
                                         && (string.IsNullOrEmpty(pag.Accion) || u.Accion.ToUpper() == pag.Accion.ToUpper())
                                         && (pag.FechaActualizacion == null
                                         || DbFunctions.TruncateTime(u.FechaCreacion) == DbFunctions.TruncateTime(pag.FechaActualizacion).Value)
                                         select new AuditoriaTipoActivoDTO()
                                         {
                                             Id = u.AuditoriaTipoActivoId,
                                             Accion = u.Accion,
                                             Entidad = u.Entidad,
                                             ValorAnterior = u.ValorAnterior,
                                             ValorNuevo = u.ValorNuevo,
                                             Campo = u.Campo,
                                             Usuario = u.Usuario,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<AuditoriaDataDTO> GetHistoricoModificacion(string Accion, string Entidad, DateTime FechaActualizacion, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<AuditoriaDataDTO> GetHistoricoModificacion(string Accion, string Entidad, DateTime FechaActualizacion, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<PlataformaBcpDTO> GetPlataformaBcp(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.PlataformaBcp
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.AreaBianId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new PlataformaBcpDTO()
                                     {
                                         Id = u.PlataformaBcpId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.PlataformaBcp,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.AreaBianId
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
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditPlataformaBcp(PlataformaBcpDTO objeto)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new PlataformaBcp()
                        {
                            PlataformaBcpId = objeto.Id,
                            AreaBianId = objeto.EntidadRelacionId,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.PlataformaBcp.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.PlataformaBcpId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.PlataformaBcp
                                       where u.PlataformaBcpId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            ctx.SaveChanges();

                            ID = entidad.PlataformaBcpId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAreaBian(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override PlataformaBcpDTO GetPlataformaBcpById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.PlataformaBcp
                                   where u.PlataformaBcpId == id
                                   select new PlataformaBcpDTO()
                                   {
                                       Id = u.PlataformaBcpId,
                                       EntidadRelacionId = u.AreaBianId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.PlataformaBcp
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetPlataformaBcpByFiltro(string filtro)
        {
            try
            {
                int id = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        var item = ctx.AreaBian.FirstOrDefault(x => x.FlagActivo && x.Nombre.ToUpper().Equals(filtro.ToUpper()));
                        id = item != null ? item.AreaBianId : 1;
                    }

                    var entidad = (from u in ctx.PlataformaBcp
                                   where u.FlagActivo
                                   //&& (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   && (string.IsNullOrEmpty(filtro) || u.AreaBianId == id)
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.PlataformaBcpId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoPlataformaBcp(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.PlataformaBcp.FirstOrDefault(x => x.PlataformaBcpId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.JefaturaAti
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.JefaturaAtiId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override DataValidacion ExisteRelacionByConfiguracion(int idEntidad, int idConfiguracion, int? idEntidadRelacion)
        {
            try
            {
                bool? retorno = null;
                int NIVEL_VALIDACION = (int)ENivelValidacionPortafolio.NivelCero;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    switch (idConfiguracion)
                    {
                        case (int)EConfiguracionPortafolio.TAI:
                            var itemTAI = ctx.TipoActivoInformacion.FirstOrDefault(x => x.TipoActivoInformacionId == idEntidad);
                            if (itemTAI != null)
                            {
                                var appUseTAI = (from u in ctx.Application
                                                 where u.assetType == itemTAI.TipoActivoInformacionId                                                 
                                                 select u.isApproved);

                                if (appUseTAI.Any())
                                    NIVEL_VALIDACION = appUseTAI.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.AreaBian:
                            var itemAB = ctx.AreaBian.FirstOrDefault(x => x.AreaBianId == idEntidad);
                            if (itemAB != null)
                            {
                                //Tercera validacion
                                var appUseDominioBian = (from u in ctx.Application join u2 in ctx.DominioBian on u.BIANdomain equals u2.DominioBianId
                                                      where u2.AreaBianId == itemAB.AreaBianId                                                      
                                                      select u.isApproved);

                                if (appUseDominioBian.Any())
                                    NIVEL_VALIDACION = appUseDominioBian.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;

                                if (!appUseDominioBian.Any())
                                {
                                    //Tercera validacion
                                    var appUsePlataforma = (from u in ctx.Application
                                                          join u2 in ctx.PlataformaBcp on u.tobe equals u2.PlataformaBcpId
                                                          where u2.AreaBianId == itemAB.AreaBianId
                                                          select u.isApproved);

                                    if (appUsePlataforma.Any())
                                        NIVEL_VALIDACION = appUsePlataforma.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;

                                    if (!appUsePlataforma.Any())
                                    {
                                        //Primera validacion
                                        retorno = (from u in ctx.DominioBian
                                                   where u.FlagActivo
                                                   && u.AreaBianId == itemAB.AreaBianId
                                                   select true).FirstOrDefault();

                                        if (retorno.Value) NIVEL_VALIDACION = (int)ENivelValidacionPortafolio.NivelUno;

                                        //Segunda validacion
                                        retorno = (from u in ctx.PlataformaBcp
                                                   where u.FlagActivo
                                                   && u.PlataformaBcpId == itemAB.AreaBianId
                                                   select true).FirstOrDefault();

                                        if (retorno.Value) NIVEL_VALIDACION = (int)ENivelValidacionPortafolio.NivelUno;
                                    }                                   
                                }
                            }
                            break;

                        case (int)EConfiguracionPortafolio.DominioBian:
                            var itemDB = ctx.DominioBian.FirstOrDefault(x => x.DominioBianId == idEntidad);                            
                            if (itemDB != null)
                            {
                                var appUseDominioBian = (from u in ctx.Application
                                                         where u.BIANdomain == itemDB.DominioBianId                                                                                                                  
                                                         select u.isApproved);

                                if (appUseDominioBian.Any())
                                    NIVEL_VALIDACION = appUseDominioBian.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.PlataformaBcp:
                            var itemPlataforma = ctx.PlataformaBcp.FirstOrDefault(x => x.PlataformaBcpId == idEntidad);                            
                            if (itemPlataforma != null)
                            {
                                var appUsePlataforma = (from u in ctx.Application                                                        
                                                        where u.tobe == itemPlataforma.PlataformaBcpId                                                        
                                                        select u.isApproved);

                                if (appUsePlataforma.Any())
                                    NIVEL_VALIDACION = appUsePlataforma.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.JefaturaAti:
                            var itemJA = ctx.JefaturaAti.FirstOrDefault(x => x.JefaturaAtiId == idEntidad);
                            if (itemJA != null)
                            {
                                //Segunda validacion
                                var appUseJefatura = (from u in ctx.ArquitectoTI
                                                      where u.JefaturaAtiId == itemJA.JefaturaAtiId
                                                      && u.FlagActivo == true
                                                      select u.ArquitectoTIId);

                                if (appUseJefatura.Any()) 
                                    NIVEL_VALIDACION = appUseJefatura == null ? (int)ENivelValidacionPortafolio.NivelCero : (int)ENivelValidacionPortafolio.NivelTres;
                                    //NIVEL_VALIDACION = appUseJefatura.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;

                        //case (int)EConfiguracionPortafolio.ArquitectoTi:
                        //    var itemAT = ctx.ArquitectoTI.FirstOrDefault(x => x.ArquitectoTIId == idEntidad);                            
                        //    if (itemAT != null)
                        //    {
                        //        var appUseArquitecto = (from u in ctx.ApplicationFlow
                        //                                where u.ownerId == itemAT.Matricula && u.isActive == true && u.isCompleted == false
                        //                                select u.isApproved);

                        //        if (appUseArquitecto.Any()) 
                        //            //NIVEL_VALIDACION = appUseArquitecto == null ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelCero;
                        //            NIVEL_VALIDACION = appUseArquitecto.Contains(false) ? (int)ENivelValidacionPortafolio.NivelDos: (int)ENivelValidacionPortafolio.NivelTres;
                        //    }
                        //    break;
                        
                        case (int)EConfiguracionPortafolio.GestionadoPor:
                            var itemGP = ctx.GestionadoPor.FirstOrDefault(x => x.GestionadoPorId == idEntidad);
                            if (itemGP != null)
                            {
                                var appUseGestionadoPor = (from u in ctx.Application
                                                           where u.managed == itemGP.GestionadoPorId
                                                           select u.isApproved);

                                if (appUseGestionadoPor.Any())
                                    NIVEL_VALIDACION = appUseGestionadoPor.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;
                        case (int)EConfiguracionPortafolio.ClasificacionTecnica:
                            var itemCT = ctx.ClasificacionTecnica.FirstOrDefault(x => x.ClasificacionTecnicaId == idEntidad);
                            if (itemCT != null)
                            {
                                //Tercera validacion
                                var appUseClasificacion = (from u in ctx.Application                                                         
                                                         where u.technicalClassification == itemCT.ClasificacionTecnicaId
                                                         select u.isApproved);

                                if (appUseClasificacion.Any())
                                    NIVEL_VALIDACION = appUseClasificacion.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;

                                if (!appUseClasificacion.Any())
                                {
                                    //Primera validacion
                                    retorno = (from u in ctx.SubClasificacionTecnica
                                               where u.ClasificacionTecnicaId == itemCT.ClasificacionTecnicaId
                                               select true).FirstOrDefault();

                                    if (retorno.Value) 
                                        NIVEL_VALIDACION = (int)ENivelValidacionPortafolio.NivelUno;                                   
                                }
                            }
                            break;

                        case (int)EConfiguracionPortafolio.SubclasificacionTecnica:
                            var itemST = ctx.SubClasificacionTecnica.FirstOrDefault(x => x.SubClasificacionTecnicaId == idEntidad);
                            if (itemST != null)
                            {
                                var appUseSubclasifcacion = (from u in ctx.Application
                                                         where u.technicalSubclassification == itemST.SubClasificacionTecnicaId
                                                         select u.isApproved);

                                if (appUseSubclasifcacion.Any())
                                    NIVEL_VALIDACION = appUseSubclasifcacion.Contains(false) ? (int)ENivelValidacionPortafolio.NivelTres : (int)ENivelValidacionPortafolio.NivelDos;
                            }
                            break;
                    }

                    var dataRetorno = new DataValidacion()
                    {
                        FlagExisteRelacion = NIVEL_VALIDACION != (int)ENivelValidacionPortafolio.NivelCero ? true : false,
                        FlagSeEjecuta = NIVEL_VALIDACION == (int)ENivelValidacionPortafolio.NivelTres ? false : true,
                        MensajeAPI = Utilitarios.GetEnumDescription2((ENivelValidacionPortafolio)NIVEL_VALIDACION)
                    };

                    return dataRetorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool EliminarRegistroConfiguracion(int idEntidad, int idConfiguracion, string usuario, string NombreUsuario)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var columna = ctx.ConfiguracionColumnaAplicacion.FirstOrDefault(x => x.ConfiguracionColumnaAplicacionId == idConfiguracion);
                    if (columna != null)
                    {
                        columna.FechaModificacion = DateTime.Now;
                        columna.NombreBDEntidadRelacion = NombreUsuario;
                        columna.UsuarioModificacion = usuario;
                    }

                    switch (idConfiguracion)
                    {
                        case (int)EConfiguracionPortafolio.TAI:
                            var itemTAI = ctx.TipoActivoInformacion.FirstOrDefault(x => x.TipoActivoInformacionId == idEntidad);
                            if (itemTAI != null)
                            {
                                itemTAI.FechaModificacion = FECHA_ACTUAL;
                                itemTAI.UsuarioModificacion = usuario;
                                itemTAI.FlagActivo = false;
                                itemTAI.FlagEliminado = true;
                                retorno = true;
                            }
                            break;
                        case (int)EConfiguracionPortafolio.CodigoReservado:
                            var itemCod = ctx.CodigoReservado.FirstOrDefault(x => x.CodigoReservadoId == idEntidad);
                            if (itemCod != null)
                            {
                           
                                itemCod.FlagActivo = false;
                                itemCod.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.AreaBian:
                            var itemAB = ctx.AreaBian.FirstOrDefault(x => x.AreaBianId == idEntidad);
                            if (itemAB != null)
                            {
                                itemAB.FechaModificacion = FECHA_ACTUAL;
                                itemAB.UsuarioModificacion = usuario;
                                itemAB.FlagActivo = false;
                                itemAB.FlagEliminado = true;
                                retorno = true;

                                //Dominio BIAN
                                ctx.DominioBian
                                       .Where(x => x.FlagActivo && x.AreaBianId == idEntidad).ToList()
                                       .ForEach(x =>
                                       {
                                           x.FlagActivo = false;
                                           x.FlagEliminado = true;
                                           x.UsuarioModificacion = usuario;
                                           x.FechaModificacion = FECHA_ACTUAL;
                                       });

                                //Plataforma BCP
                                ctx.PlataformaBcp
                                       .Where(x => x.FlagActivo && x.AreaBianId == idEntidad).ToList()
                                       .ForEach(x =>
                                       {
                                           x.FlagActivo = false;
                                           x.FlagEliminado = true;
                                           x.UsuarioModificacion = usuario;
                                           x.FechaModificacion = FECHA_ACTUAL;
                                       });
                            }
                            break;

                        case (int)EConfiguracionPortafolio.DominioBian:
                            var itemDB = ctx.DominioBian.FirstOrDefault(x => x.DominioBianId == idEntidad);
                            if (itemDB != null)
                            {
                                itemDB.FechaModificacion = FECHA_ACTUAL;
                                itemDB.UsuarioModificacion = usuario;
                                itemDB.FlagActivo = false;
                                itemDB.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.PlataformaBcp:
                            var itemPlataforma = ctx.PlataformaBcp.FirstOrDefault(x => x.PlataformaBcpId == idEntidad);
                            if (itemPlataforma != null)
                            {
                                itemPlataforma.FechaModificacion = FECHA_ACTUAL;
                                itemPlataforma.UsuarioModificacion = usuario;
                                itemPlataforma.FlagActivo = false;
                                itemPlataforma.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.JefaturaAti:
                            var itemJA = ctx.JefaturaAti.FirstOrDefault(x => x.JefaturaAtiId == idEntidad);
                            if (itemJA != null)
                            {
                                itemJA.FechaModificacion = FECHA_ACTUAL;
                                itemJA.UsuarioModificacion = usuario;
                                itemJA.FlagActivo = false;
                                itemJA.FlagEliminado = true;
                                retorno = true;

                                //Arquitecto TI
                                var listArquitectos = ctx.ArquitectoTI.Where(x => x.FlagActivo && x.JefaturaAtiId == itemJA.JefaturaAtiId);
                                if (listArquitectos.Any())
                                    listArquitectos.ToList()
                                        .ForEach(x =>
                                        {
                                            x.FlagActivo = false;
                                            x.FlagEliminado = true;
                                            x.UsuarioModificacion = usuario;
                                            x.FechaModificacion = FECHA_ACTUAL;
                                        });

                                UpdateEliminarArquitectoTI(listArquitectos.Select(x => x.Matricula).ToArray(), itemJA.JefaturaAtiId, ctx, usuario);
                            }
                            break;

                        case (int)EConfiguracionPortafolio.ArquitectoTi:
                            var itemAT = ctx.ArquitectoTI.FirstOrDefault(x => x.ArquitectoTIId == idEntidad);
                            if (itemAT != null)
                            {
                                itemAT.FechaModificacion = FECHA_ACTUAL;
                                itemAT.UsuarioModificacion = usuario;
                                itemAT.FlagActivo = false;
                                itemAT.FlagEliminado = true;
                                retorno = true;

                                var arrMatriculas = new string[]
                                {
                                    itemAT.Matricula
                                };

                                UpdateEliminarArquitectoTI(arrMatriculas, itemAT.JefaturaAtiId, ctx, usuario);
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Gerencia:
                            var itemGerencia = ctx.Gerencia.FirstOrDefault(x => x.GerenciaId == idEntidad);
                            if (itemGerencia != null)
                            {
                                itemGerencia.FechaModificacion = FECHA_ACTUAL;
                                itemGerencia.UsuarioModificacion = usuario;
                                itemGerencia.FlagActivo = false;
                                itemGerencia.FlagEliminado = true;
                                retorno = true;

                                //Division
                                var lDivision = ctx.Division.Where(x => x.FlagActivo && x.GerenciaId == itemGerencia.GerenciaId);
                                if (lDivision.Any())
                                {
                                    lDivision.ToList()
                                           .ForEach(x =>
                                           {
                                               x.FlagActivo = false;
                                               x.FlagEliminado = true;
                                               x.UsuarioModificacion = usuario;
                                               x.FechaModificacion = FECHA_ACTUAL;
                                           });

                                    //Area
                                    var IdsDivision = lDivision.Select(x => x.DivisionId);
                                    var lArea = ctx.Area.Where(x => x.FlagActivo && IdsDivision.Contains(x.DivisionId));
                                    if (lArea.Any())
                                    {
                                        var IdsArea = lArea.Select(x => x.AreaId).ToArray();
                                        lArea.ToList()
                                               .ForEach(x =>
                                               {
                                                   x.FlagActivo = false;
                                                   x.FlagEliminado = true;
                                                   x.UsuarioModificacion = usuario;
                                                   x.FechaModificacion = FECHA_ACTUAL;
                                               });

                                        //Unidad
                                        ctx.Unidad.Where(x => x.FlagActivo && IdsArea.Contains(x.AreaId)).ToList()
                                              .ForEach(x =>
                                              {
                                                  x.FlagActivo = false;
                                                  x.FlagEliminado = true;
                                                  x.UsuarioModificacion = usuario;
                                                  x.FechaModificacion = FECHA_ACTUAL;
                                              });
                                    }
                                }
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Division:
                            var itemDiv = ctx.Division.FirstOrDefault(x => x.DivisionId == idEntidad);
                            if (itemDiv != null)
                            {
                                itemDiv.FechaModificacion = FECHA_ACTUAL;
                                itemDiv.UsuarioModificacion = usuario;
                                itemDiv.FlagActivo = false;
                                itemDiv.FlagEliminado = true;
                                retorno = true;

                                //Area
                                var listArea = ctx.Area.Where(x => x.FlagActivo && x.DivisionId == idEntidad);
                                if (listArea.Any())
                                {
                                    var IdsArea = listArea.Select(x => x.AreaId).ToArray();
                                    listArea.ToList()
                                           .ForEach(x =>
                                           {
                                               x.FlagActivo = false;
                                               x.FlagEliminado = true;
                                               x.UsuarioModificacion = usuario;
                                               x.FechaModificacion = FECHA_ACTUAL;
                                           });

                                    //Unidad
                                    ctx.Unidad.Where(x => x.FlagActivo && IdsArea.Contains(x.AreaId)).ToList()
                                          .ForEach(x =>
                                          {
                                              x.FlagActivo = false;
                                              x.FlagEliminado = true;
                                              x.UsuarioModificacion = usuario;
                                              x.FechaModificacion = FECHA_ACTUAL;
                                          });
                                }
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Area:
                            var itemArea = ctx.Area.FirstOrDefault(x => x.AreaId == idEntidad);
                            if (itemArea != null)
                            {
                                itemArea.FechaModificacion = FECHA_ACTUAL;
                                itemArea.UsuarioModificacion = usuario;
                                itemArea.FlagActivo = false;
                                itemArea.FlagEliminado = true;
                                retorno = true;

                                //Unidad
                                ctx.Unidad.Where(x => x.FlagActivo && x.AreaId == idEntidad).ToList()
                                       .ForEach(x =>
                                       {
                                           x.FlagActivo = false;
                                           x.FlagEliminado = true;
                                           x.UsuarioModificacion = usuario;
                                           x.FechaModificacion = FECHA_ACTUAL;
                                       });
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Unidad:
                            var itemUnidad = ctx.Unidad.FirstOrDefault(x => x.UnidadId == idEntidad);
                            if (itemUnidad != null)
                            {
                                itemUnidad.FechaModificacion = FECHA_ACTUAL;
                                itemUnidad.UsuarioModificacion = usuario;
                                itemUnidad.FlagActivo = false;
                                itemUnidad.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.GestionadoPor:
                            var itemGP = ctx.GestionadoPor.FirstOrDefault(x => x.GestionadoPorId == idEntidad);
                            if (itemGP != null)
                            {
                                itemGP.FechaModificacion = FECHA_ACTUAL;
                                itemGP.UsuarioModificacion = usuario;
                                itemGP.FlagActivo = false;
                                itemGP.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.ProcesoVital:
                            var itemPV = ctx.ProcesoVital.FirstOrDefault(x => x.ProcesoVitalId == idEntidad);
                            if (itemPV != null)
                            {
                                itemPV.FechaModificacion = FECHA_ACTUAL;
                                itemPV.UsuarioModificacion = usuario;
                                itemPV.FlagActivo = false;
                                itemPV.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Estandar:
                            var itemEstandar = ctx.Estandar.FirstOrDefault(x => x.EstandarId == idEntidad);
                            if (itemEstandar != null)
                            {
                                itemEstandar.FechaModificacion = FECHA_ACTUAL;
                                itemEstandar.UsuarioModificacion = usuario;
                                itemEstandar.FlagActivo = false;
                                itemEstandar.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.CuestionarioPae:
                            var itemCP = ctx.CuestionarioPae.FirstOrDefault(x => x.CuestionarioPaeId == idEntidad);
                            if (itemCP != null)
                            {
                                itemCP.FechaModificacion = FECHA_ACTUAL;
                                itemCP.UsuarioModificacion = usuario;
                                itemCP.FlagActivo = false;
                                itemCP.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.PreguntaPae:
                            var itemPP = ctx.PreguntaPae.FirstOrDefault(x => x.PreguntaPaeId == idEntidad);
                            if (itemPP != null)
                            {
                                itemPP.FechaModificacion = FECHA_ACTUAL;
                                itemPP.UsuarioModificacion = usuario;
                                itemPP.FlagActivo = false;
                                itemPP.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.BandejaAprobacion:
                            var itemBAP = ctx.BandejaAprobacion.FirstOrDefault(x => x.BandejaAprobacionId == idEntidad);
                            if (itemBAP != null)
                            {
                                itemBAP.FechaModificacion = FECHA_ACTUAL;
                                itemBAP.UsuarioModificacion = usuario;
                                itemBAP.FlagActivo = false;
                                itemBAP.FlagEliminado = true;
                                retorno = true;

                                ctx.SolicitudAprobadores
                                        .Where(x => x.FlagActivo && !x.FlagAprobado.Value && x.BandejaId == itemBAP.BandejaId).ToList()
                                        .ForEach(x =>
                                        {
                                            x.Matricula = x.Matricula.Replace(itemBAP.MatriculaAprobador, "");
                                            x.UsuarioModificacion = usuario;
                                            x.FechaModificacion = FECHA_ACTUAL;
                                        });
                            }
                            break;

                        case (int)EConfiguracionPortafolio.Parametrica:
                            var itemPD = ctx.ParametricaDetalle.FirstOrDefault(x => x.ParametricaDetalleId == idEntidad);
                            if (itemPD != null)
                            {
                                itemPD.FechaModificacion = FECHA_ACTUAL;
                                itemPD.ModificadoPor = usuario;
                                itemPD.FlagActivo = false;
                                itemPD.FlagEliminado = true;
                                retorno = true;
                            }
                            break;

                        case (int)EConfiguracionPortafolio.GestionCampo:
                            var itemCCA = ctx.ConfiguracionColumnaAplicacion.FirstOrDefault(x => x.ConfiguracionColumnaAplicacionId == idEntidad);
                            if (itemCCA != null)
                            {
                                itemCCA.FechaModificacion = FECHA_ACTUAL;
                                itemCCA.UsuarioModificacion = usuario;
                                itemCCA.FlagActivo = false;
                                itemCCA.FlagEliminado = true;
                                retorno = true;

                                var itemInfoCampo = ctx.InfoCampoPortafolio.FirstOrDefault(y => y.ConfiguracionColumnaAplicacionId == idEntidad);
                                if (itemInfoCampo != null)
                                {
                                    itemInfoCampo.FechaModificacion = FECHA_ACTUAL;
                                    itemInfoCampo.UsuarioModificacion = usuario;
                                    itemInfoCampo.FlagActivo = false;
                                    itemInfoCampo.FlagEliminado = true;

                                    ctx.NuevoCampoPortafolio
                                        .Where(x => x.FlagActivo && x.InfoCampoPortafolioId == itemInfoCampo.InfoCampoPortafolioId).ToList()
                                        .ForEach(x =>
                                        {
                                            x.FlagActivo = false;
                                            x.FlagEliminado = true;
                                        });
                                }
                            }
                            break;
                        case (int)EConfiguracionPortafolio.RolGestion:
                            var itemRG = ctx.RolOndemand.FirstOrDefault(x => x.RolOndDemandId == idEntidad);
                            if (itemRG != null)
                            {
                                itemRG.FechaModificacion = FECHA_ACTUAL;
                                itemRG.ModificadoPor = usuario;
                                itemRG.IsActive = false;
                                itemRG.IsDeleted = true;
                                retorno = true;

                                //Desactivar todas las asignaciones que tenga el usuario y que no hayan sido completadas
                                var query = "update app.ApplicationFlow set isActive='false', isCompleted='true' where ownerId='{0}' and typeRegister={1}";
                                switch (itemRG.RoleId)
                                {
                                    case (int)ERoles.AIO:
                                        ctx.Database.ExecuteSqlCommand(string.Format(query, itemRG.Username, (int)ApplicationManagerRole.AIO));
                                        break;
                                    case (int)ERoles.ArquitectoTecnologia:
                                        ctx.Database.ExecuteSqlCommand(string.Format(query, itemRG.Username, (int)ApplicationManagerRole.ArquitectoTI));
                                        break;
                                    case (int)ERoles.DevSecOps:
                                        ctx.Database.ExecuteSqlCommand(string.Format(query, itemRG.Username, (int)ApplicationManagerRole.DevSecOps));
                                        break;
                                    case (int)ERoles.GobiernoUserIT:
                                        ctx.Database.ExecuteSqlCommand(string.Format(query, itemRG.Username, (int)ApplicationManagerRole.GobiernoUserIT));
                                        break;
                                }
                            }
                            break;
                        case (int)EConfiguracionPortafolio.ClasificacionTecnica:
                            var itemCT = ctx.ClasificacionTecnica.FirstOrDefault(x => x.ClasificacionTecnicaId == idEntidad);
                            if (itemCT != null)
                            {
                                itemCT.FechaModificacion = FECHA_ACTUAL;
                                itemCT.UsuarioModificacion = usuario;
                                itemCT.FlagActivo = false;
                                itemCT.FlagEliminado = true;
                                retorno = true;

                            }
                            break;
                        case (int)EConfiguracionPortafolio.SubclasificacionTecnica:
                            var itemSCT = ctx.SubClasificacionTecnica.FirstOrDefault(x => x.SubClasificacionTecnicaId == idEntidad);
                            if (itemSCT != null)
                            {
                                itemSCT.FechaModificacion = FECHA_ACTUAL;
                                itemSCT.UsuarioModificacion = usuario;
                                itemSCT.FlagActivo = false;
                                itemSCT.FlagEliminado = true;
                                retorno = true;

                            }
                            break;
                    }

                    ctx.SaveChanges();

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        private void UpdateEliminarArquitectoTI(string[] arrMatriculas, int JefaturaId, GestionCMDB_ProdEntities ctx, string Usuario)
        {
            DateTime FECHA_ACTUAL = DateTime.Now;

            var IdsSolicitudesNoAprobadas = (from x in ctx.Solicitud
                                             join y in ctx.Aplicacion on x.AplicacionId equals y.AplicacionId
                                             join z in ctx.JefaturaAti on y.JefaturaATI.ToUpper() equals z.Nombre.ToUpper()
                                             where !x.FlagAprobacion.Value && y.FlagActivo
                                             && x.TipoSolicitud == (int)ETipoSolicitudAplicacion.CreacionAplicacion
                                             && x.EstadoSolicitud != (int)EEstadoSolicitudAplicacion.Registrado
                                             && z.JefaturaAtiId == JefaturaId
                                             select x.SolicitudAplicacionId);

            if (IdsSolicitudesNoAprobadas.Any())
            {
                foreach (var item_Mat in arrMatriculas)
                {
                    ctx.SolicitudAprobadores.Where(x => x.FlagActivo
                    && IdsSolicitudesNoAprobadas.Contains(x.SolicitudAplicacionId)
                    && !x.FlagAprobado.Value
                    && x.BandejaId == (int)EBandejaAprobadorAplicacion.ArquitecturaTI).ToList()
                    .ForEach(x =>
                    {
                        x.Matricula = x.Matricula.Replace(item_Mat, "");
                        x.UsuarioModificacion = Usuario;
                        x.FechaModificacion = FECHA_ACTUAL;
                    });
                }
            }
        }

        public override int AddOrEditNuevoCampoPortafolio(NuevoCampoPortafolioDTO objeto, GestionCMDB_ProdEntities _ctx)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                var ctx = _ctx;

                var entidad = (from x in ctx.NuevoCampoPortafolio
                               join y in ctx.InfoCampoPortafolio on x.InfoCampoPortafolioId equals y.InfoCampoPortafolioId
                               where x.FlagActivo && x.CodigoAPT.ToUpper().Equals(objeto.CodigoAPT.ToUpper())
                               && y.Codigo.ToUpper().Equals(objeto.Codigo)
                               select x).FirstOrDefault();

                if (entidad != null)
                {
                    entidad.Valor = objeto.Valor;
                    entidad.FechaModificacion = FECHA_ACTUAL;
                    entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                    ctx.SaveChanges();

                    ID = entidad.NuevoCampoPortafolioId;
                }
                else
                {
                    var infoCampo = ctx.InfoCampoPortafolio.FirstOrDefault(x => x.FlagActivo
                    && x.Codigo.ToUpper().Equals(objeto.Codigo));

                    if (infoCampo != null)
                    {
                        var newItem = new NuevoCampoPortafolio()
                        {
                            NuevoCampoPortafolioId = objeto.Id,
                            InfoCampoPortafolioId = infoCampo.InfoCampoPortafolioId,
                            CodigoAPT = objeto.CodigoAPT,
                            Valor = objeto.Valor,
                            FlagActivo = true,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.NuevoCampoPortafolio.Add(newItem);
                        ctx.SaveChanges();

                        ID = newItem.NuevoCampoPortafolioId;
                    }
                }

                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditNuevoCampoPortafolio(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditNuevoCampoPortafolio(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<EstandarPortafolioDTO> GetEstandarPortafolioTecnologia(PaginacionEstandar pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var IdsEstandares = new int[] { 36, 13, 68, 69, 14 };
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Tecnologia
                                     join x in ctx.Subdominio on u.SubdominioId equals x.SubdominioId
                                     join y in ctx.Tipo on u.TipoTecnologia equals y.TipoId
                                     where u.Activo && x.Activo && y.Activo
                                     && (pag.TipoTecnologiaId == null || u.TipoTecnologia == pag.TipoTecnologiaId)
                                     && (pag.EstadoId == null || u.EstadoId == pag.EstadoId)
                                     && IdsEstandares.Contains(x.SubdominioId)
                                     select new EstandarPortafolioDTO()
                                     {
                                         Id = u.TecnologiaId,
                                         SubdominioId = x.SubdominioId,
                                         TipoEstandar = x.Nombre,
                                         Nombre = u.ClaveTecnologia,
                                         EstadoId = u.EstadoId,
                                         TipoTecnologiaId = y.TipoId,
                                         TipoTecnologiaStr = y.Nombre,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
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
                    , "Error en el metodo: List<ActivosDTO> GetEstandarPortafolio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetEstandarPortafolio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoBandejaAprobacion(int id, bool estado, string usuario)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.BandejaAprobacion.FirstOrDefault(x => x.BandejaAprobacionId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = FECHA_ACTUAL;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SolicitudAprobadores
                                      .Where(x => x.FlagActivo && !x.FlagAprobado.Value && x.BandejaId == itemBD.BandejaId).ToList()
                                      .ForEach(x =>
                                      {
                                          x.Matricula = estado ? string.IsNullOrWhiteSpace(x.Matricula) ? itemBD.MatriculaAprobador : x.Matricula + "|" + itemBD.MatriculaAprobador : x.Matricula.Replace(itemBD.MatriculaAprobador, "");
                                          x.UsuarioModificacion = usuario;
                                          x.FechaModificacion = FECHA_ACTUAL;
                                      });

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override string[] GetDataInfoCampoPortafolioByConfiguracion(int idEntidad)
        {
            try
            {
                var listRetorno = new string[] { };
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemInfoCampo = ctx.InfoCampoPortafolio.FirstOrDefault(x => x.FlagActivo
                    && x.ConfiguracionColumnaAplicacionId == idEntidad);

                    if (itemInfoCampo != null)
                    {
                        switch (itemInfoCampo.MantenimientoPortafolioId.Value)
                        {
                            case (int)EConfiguracionPortafolio.TAI:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetActivosByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.AreaBian:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetAreaBianByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.DominioBian:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.PlataformaBcp:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.JefaturaAti:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.ArquitectoTi:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Gerencia:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetGerenciaByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.Division:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Area:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Unidad:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.GestionadoPor:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.ProcesoVital:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetProcesoVitalByFiltro(null).Select(x => x.Descripcion).ToArray();
                                break;

                            case (int)EConfiguracionPortafolio.Parametrica:
                                string codigoParametrica = itemInfoCampo.ParametricaDescripcion;
                                if (!string.IsNullOrEmpty(codigoParametrica))
                                {
                                    listRetorno = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(codigoParametrica,
                                    (int)EEntidadParametrica.PORTAFOLIO
                                    ).Select(x => x.Descripcion).ToArray();
                                }

                                break;
                        }
                    }

                    return listRetorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<DataListBox> GetDataListBoxByConfiguracion(int idEntidad)
        {
            try
            {
                var listRetorno = new List<DataListBox>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemInfoCampo = ctx.InfoCampoPortafolio.FirstOrDefault(x => x.FlagActivo
                    && x.ConfiguracionColumnaAplicacionId == idEntidad);

                    if (itemInfoCampo != null)
                    {
                        switch (itemInfoCampo.MantenimientoPortafolioId.Value)
                        {
                            case (int)EConfiguracionPortafolio.TAI:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetActivosByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.AreaBian:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetAreaBianByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.DominioBian:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.PlataformaBcp:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.JefaturaAti:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.ArquitectoTi:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Gerencia:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetGerenciaByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.Division:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Area:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.Unidad:
                                //TODO
                                break;

                            case (int)EConfiguracionPortafolio.GestionadoPor:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.ProcesoVital:
                                listRetorno = ServiceManager<ActivosDAO>.Provider.GetProcesoVitalByFiltro(null)
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                break;

                            case (int)EConfiguracionPortafolio.Parametrica:
                                string codigoParametrica = itemInfoCampo.ParametricaDescripcion;
                                if (!string.IsNullOrEmpty(codigoParametrica))
                                {
                                    listRetorno = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(codigoParametrica,
                                    (int)EEntidadParametrica.PORTAFOLIO
                                    )
                                    .Select(x => new DataListBox()
                                    {
                                        Id = x.Id,
                                        Valor = x.Descripcion
                                    }).ToList();
                                }

                                break;
                        }
                    }

                    return listRetorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteNombreEntidadByConfiguracion(int idEntidad, string nombreFiltro, int idConfiguracion, int? idEntidadRelacion)
        {
            try
            {
                bool? retorno = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    switch (idConfiguracion)
                    {
                        case (int)EConfiguracionPortafolio.TAI:
                            retorno = (from u in ctx.TipoActivoInformacion
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.TipoActivoInformacionId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();
                            break;

                        case (int)EConfiguracionPortafolio.AreaBian:
                            retorno = (from u in ctx.AreaBian
                                       where !u.FlagEliminado
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.AreaBianId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.DominioBian:
                            retorno = (from u in ctx.DominioBian
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.DominioBianId != idEntidad
                                       && u.AreaBianId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.PlataformaBcp:
                            retorno = (from u in ctx.PlataformaBcp
                                       where !u.FlagEliminado.Value
                                       //&& u.Nombre.ToUpper().Equals(nombreFiltro.ToUpper())
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.PlataformaBcpId != idEntidad
                                       && u.AreaBianId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();
                            break;

                        case (int)EConfiguracionPortafolio.JefaturaAti:
                            retorno = (from u in ctx.JefaturaAti
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.JefaturaAtiId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.ArquitectoTi:

                            break;

                        case (int)EConfiguracionPortafolio.Gerencia:
                            retorno = (from u in ctx.Gerencia
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.GerenciaId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();
                            break;

                        case (int)EConfiguracionPortafolio.Division:
                            retorno = (from u in ctx.Division
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.DivisionId != idEntidad
                                       && u.GerenciaId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.Area:
                            retorno = (from u in ctx.Area
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.AreaId != idEntidad
                                       && u.DivisionId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.Unidad:
                            retorno = (from u in ctx.Unidad
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.UnidadId != idEntidad
                                       && u.AreaId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();
                            break;

                        case (int)EConfiguracionPortafolio.GestionadoPor:
                            retorno = (from u in ctx.GestionadoPor
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.GestionadoPorId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.Estandar:
                            break;

                        case (int)EConfiguracionPortafolio.ProcesoVital:
                            retorno = (from u in ctx.ProcesoVital
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.ProcesoVitalId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.ClasificacionTecnica:
                            retorno = (from u in ctx.ClasificacionTecnica
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.ClasificacionTecnicaId != idEntidad
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;

                        case (int)EConfiguracionPortafolio.SubclasificacionTecnica:
                            retorno = (from u in ctx.SubClasificacionTecnica
                                       where !u.FlagEliminado.Value
                                       && u.Nombre.ToUpper().Replace(" ", "").Equals(nombreFiltro.ToUpper())
                                       && u.SubClasificacionTecnicaId != idEntidad
                                       && u.ClasificacionTecnicaId == idEntidadRelacion.Value
                                       orderby u.Nombre
                                       select true).FirstOrDefault();

                            break;
                    }

                    return retorno == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<CustomDataListBox> GetDataListBoxNewByFiltro()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var dataConfig = (from x in ctx.ConfiguracionColumnaAplicacion
                                      join y in ctx.InfoCampoPortafolio on x.ConfiguracionColumnaAplicacionId equals y.ConfiguracionColumnaAplicacionId
                                      where x.FlagActivo && y.FlagActivo && x.FlagCampoNuevo.Value
                                      && y.TipoInputId == (int)ETipoInputHTML.ListBox
                                      && y.MantenimientoPortafolioId == (int)EConfiguracionPortafolio.Parametrica
                                      orderby x.ConfiguracionColumnaAplicacionId
                                      select new CustomDataListBox()
                                      {
                                          Id = x.ConfiguracionColumnaAplicacionId.ToString(),
                                          ParametricaDescripcion = y.ParametricaDescripcion,
                                          CodigoHtml = y.Codigo
                                      }).ToList();

                    if (dataConfig != null && dataConfig.Count > 0)
                    {
                        foreach (var item in dataConfig)
                        {
                            var arrDataLB = (from x in ctx.ParametricaDetalle
                                             where x.FlagActivo && x.ParametricaId == (int)EEntidadParametrica.PORTAFOLIO
                                             && x.Descripcion.ToUpper().Equals(item.ParametricaDescripcion.ToUpper())
                                             select x.Valor).ToArray();

                            if (arrDataLB != null && arrDataLB.Count() > 0) item.DataListBox = arrDataLB;
                        }
                    }

                    return dataConfig;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarFlagMostrarCampo(int id, bool estado, string usuario, string NombreUsuario)
        {
            try
            {
                bool retorno = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ConfiguracionColumnaAplicacion.FirstOrDefault(x => x.ConfiguracionColumnaAplicacionId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagMostrarCampo = estado;
                        itemBD.NombreBDEntidadRelacion = NombreUsuario;
                        ctx.SaveChanges();

                        retorno = true;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool AddOrEditListServidorAplicacion(DataServidorAplicacion objeto)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemApp = ctx.Aplicacion.FirstOrDefault(x => x.AplicacionId == objeto.AplicacionId);
                    if (itemApp != null)
                    {
                        //Remove
                        var listIds = objeto.IdsEliminarServidor;
                        if (listIds != null && listIds.Count() > 0)
                        {
                            ctx.ServidorAplicacion.Where(x => x.FlagActivo && listIds.Contains(x.ServidorAplicacionId)).ToList()
                                .ForEach(x =>
                                {
                                    x.FlagActivo = false;
                                    x.UsuarioModificacion = objeto.Usuario;
                                    x.FechaModificacion = FECHA_ACTUAL;
                                });

                            ctx.SaveChanges();
                        }

                        //Add or edit
                        var listSA = objeto.ServidorDetalle;
                        if (listSA != null && listSA.Count > 0)
                        {
                            foreach (var item in listSA)
                            {
                                var itemSA = ctx.ServidorAplicacion.FirstOrDefault(x => x.ServidorAplicacionId == item.Id);
                                if (itemSA != null)
                                {
                                    itemSA.NombreServidor = item.NombreServidor;
                                    itemSA.FechaModificacion = FECHA_ACTUAL;
                                    itemSA.UsuarioModificacion = objeto.Usuario;
                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    var newItem = new ServidorAplicacion()
                                    {
                                        CodigoAPT = itemApp.CodigoAPT,
                                        NombreServidor = item.NombreServidor,
                                        FlagActivo = true,
                                        FechaCreacion = FECHA_ACTUAL,
                                        UsuarioCreacion = objeto.Usuario
                                    };
                                    ctx.ServidorAplicacion.Add(newItem);
                                    ctx.SaveChanges();
                                }
                            }
                        }

                        retorno = true;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditGerencia(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<TeamSquadDTO> GetTeamSquad(int gestionado)
        {
            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TeamSquad
                                     where u.GestionadoPorId == gestionado &&!u.FlagEliminado && u.FlagActivo
                                     orderby u.Nombre ascending
                                     select new TeamSquadDTO()
                                     {
                                         Id = u.EquipoId,                                                                                 
                                         Nombre = u.Nombre,
                                         Responsable = u.Responsable,
                                         ResponsableCorreo = u.ResponsableCorreo,
                                         ResponsableMatricula = u.ResponsableMatricula,                                         
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         FechaModificacion = u.FechaModificacion
                                     }).ToList();                                       

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquad()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquad()"
                    , new object[] { null });
            }
        }

        public override TeamSquadDTO GetTeamSquadId(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.TeamSquad
                                     where u.EquipoId == id
                                     orderby u.Nombre ascending
                                     select new TeamSquadDTO()
                                     {
                                         Id = u.EquipoId,
                                         Nombre = u.Nombre,
                                         Responsable = u.Responsable,
                                         ResponsableCorreo = u.ResponsableCorreo,
                                         ResponsableMatricula = u.ResponsableMatricula,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         FechaModificacion = u.FechaModificacion
                                     }).FirstOrDefault();

                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquad()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquad()"
                    , new object[] { null });
            }
        }

        /*Roles Gestion*/
        
        public override List<RolesGestionDTO> GetRolesGestion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.RolOndemand
                                     where (u.Name.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     && !u.IsDeleted
                                     orderby u.Name ascending
                                     select new RolesGestionDTO()
                                     {
                                         Id = u.RolOndDemandId,
                                         Username = u.Username,
                                         Name = u.Name,
                                         Email = u.Email,
                                         RoleId = u.RoleId,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         UsuarioCreacion = u.CreadoPor,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioModificacion = u.ModificadoPor,
                                         FechaModificacion = u.FechaModificacion
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<RolesGestionDTO> GetRolesGestion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<RolesGestionDTO> GetRolesGestion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<RolesGestionDTO> GetRolesGestion()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.RolOndemand
                                     where u.IsActive == true && u.IsDeleted == false
                                     select new RolesGestionDTO()
                                     {
                                         Id = u.RolOndDemandId,
                                         Username = u.Username,
                                         Name = u.Name,
                                         Email = u.Email,
                                         RoleId = u.RoleId
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<RolesGestionDTO> GetRolesGestion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<RolesGestionDTO> GetRolesGestion()"
                    , new object[] { null });
            }
        }

        public override int AddOrEditRolesGestion(RolesGestionDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var registroActual = ctx.RolOndemand.FirstOrDefault(x => x.Username == objeto.Username && x.RoleId == objeto.RoleId);
                        if(registroActual == null)
                        {
                            var entidad = new RolOndemand()
                            {
                                Username = objeto.Username,
                                Name = objeto.Name,
                                Email = objeto.Email,
                                RoleId = objeto.RoleId,
                                CreadoPor = objeto.UsuarioCreacion,
                                ModificadoPor = objeto.UsuarioModificacion,
                                FechaCreacion = FECHA_ACTUAL,
                                FechaModificacion = FECHA_ACTUAL,
                                IsDeleted = false,
                                IsActive = true
                            };
                            ctx.RolOndemand.Add(entidad);
                            ctx.SaveChanges();

                            ID = entidad.RolOndDemandId;
                        }
                        else
                        {
                            if (registroActual.IsDeleted)
                            {
                                registroActual.IsDeleted = false;
                                registroActual.IsActive = true;
                                ctx.SaveChanges();
                            }

                            ID = -2; //Asociado a mensaje de control para informar al usuario sobre el registro de un rol ya registrado
                        }
                    }
                    else
                    {
                        var entidad = (from u in ctx.RolOndemand
                                       where u.RolOndDemandId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {

                            entidad.Name = objeto.Name;
                            entidad.Username = objeto.Username;
                            entidad.Email = objeto.Email;
                            entidad.RoleId = objeto.RoleId;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            ID = entidad.RoleId;
                        }
                    }

                    //Validar los flujos pendientes y asignarlos al usuario
                    var typeRegister = 0;
                    switch (objeto.RoleId)
                    {
                        case (int)ERoles.AIO:
                            typeRegister = (int)ApplicationManagerRole.AIO;
                            break;
                        //case (int)ERoles.ArquitectoTecnologia:
                        //    typeRegister = (int)ApplicationManagerRole.ArquitectoTI;
                        //    break;
                        case (int)ERoles.DevSecOps:
                            typeRegister = (int)ApplicationManagerRole.DevSecOps;                            
                            break;
                        case (int)ERoles.GobiernoUserIT:
                            typeRegister = (int)ApplicationManagerRole.GobiernoUserIT;                            
                            break;
                    }

                    if (typeRegister != 0)
                    {
                        var flujosActivos = (from u in ctx.ApplicationFlow
                                             where u.typeRegister == typeRegister && u.isActive.Value && !u.isCompleted.Value
                                             select u.AppId).ToList();
                        foreach (var item in flujosActivos)
                        {
                            try
                            {
                                var flujoPorUsuario = ctx.ApplicationFlow.FirstOrDefault(x => x.AppId == item && x.isActive.Value && !x.isCompleted.Value && x.ownerId == objeto.Username && x.typeRegister == typeRegister);
                                if (flujoPorUsuario == null)
                                {
                                    var flujoRol = new ApplicationFlow()
                                    {
                                        AppId = item,
                                        createdBy = objeto.UsuarioCreacion,
                                        dateCreation = DateTime.Now,
                                        isCompleted = false,
                                        isNotified = false,
                                        ownerId = objeto.Username,
                                        ownerEmail = objeto.Email,
                                        typeFlow = (int)Flow.Registro,
                                        typeRegister = typeRegister,
                                        isActive = true,
                                        RegistradoPor = objeto.UsuarioModificacion
                                    };
                                    ctx.ApplicationFlow.Add(flujoRol);
                                    ctx.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message, ex);
                            }
                        }
                    }
                    
                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditRolesGestion(RolesGestionDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditRolesGestion(RolesGestionDTO objeto)"
                    , new object[] { null });
            }
        }

        public override RolesGestionDTO GetRolesGestionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.RolOndemand

                                   where u.RolOndDemandId == id
                                   select new RolesGestionDTO()
                                   {
                                       Id = u.RolOndDemandId,
                                       Username = u.Username,
                                       Name = u.Name,
                                       Email = u.Email,
                                       RoleId = u.RoleId,
                                       IsActive=u.IsActive,
                                       UsuarioModificacion=u.CreadoPor

                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRolesGestionDTO
                    , "Error en el metodo: RolesGestionDTO GetRolesGestionById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRolesGestionDTO
                    , "Error en el metodo: RolesGestionDTO GetRolesGestionById(int id)"
                    , new object[] { null });
            }
        }

        
        public override bool CambiarEstadoRolesGestion(int id, bool estado, string usuario)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.RolOndemand
                                  where u.RolOndDemandId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = FECHA_ACTUAL;
                        itemBD.ModificadoPor = usuario;
                        itemBD.IsActive = estado;
                        ctx.SaveChanges();

                        if (!estado)
                        {
                            //Desactivar todas las asignaciones que tenga el usuario y que no hayan sido completadas
                            var query = "update app.ApplicationFlow set isActive='false', isCompleted='true' where ownerId='{0}' and typeRegister={1}";
                            switch (itemBD.RoleId)
                            {
                                case (int)ERoles.AIO:
                                    ctx.Database.ExecuteSqlCommand(string.Format(query, itemBD.Username, (int)ApplicationManagerRole.AIO));
                                    break;
                                //case (int)ERoles.ArquitectoTecnologia:
                                //    ctx.Database.ExecuteSqlCommand(string.Format(query, itemBD.Username, (int)ApplicationManagerRole.ArquitectoTI));
                                //    break;
                                case (int)ERoles.DevSecOps:
                                    ctx.Database.ExecuteSqlCommand(string.Format(query, itemBD.Username, (int)ApplicationManagerRole.DevSecOps));
                                    break;
                                case (int)ERoles.GobiernoUserIT:
                                    ctx.Database.ExecuteSqlCommand(string.Format(query, itemBD.Username, (int)ApplicationManagerRole.GobiernoUserIT));
                                    break;
                            }
                        }
                        retorno = !retorno;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstadoRolesGestion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstadoRolesGestion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditClasificacion(ClasificacionTecnicaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new ClasificacionTecnica()
                        {
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.ClasificacionTecnica.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.ClasificacionTecnicaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ClasificacionTecnica
                                       where u.ClasificacionTecnicaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.ClasificacionTecnicaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditClasificacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditClasificacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditSubclasificacion(SubClasificacionTecnicaDTO objeto)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new SubClasificacionTecnica()
                        {
                            SubClasificacionTecnicaId = objeto.Id,
                            ClasificacionTecnicaId = objeto.EntidadRelacionId,                            
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL                            
                        };
                        ctx.SubClasificacionTecnica.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.SubClasificacionTecnicaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.SubClasificacionTecnica
                                       where u.SubClasificacionTecnicaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {                            
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            
                            ctx.SaveChanges();

                            ID = entidad.SubClasificacionTecnicaId;
                        }
                    }                    

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditSubclasificacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditSubclasificacion(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override ClasificacionTecnicaDTO GetClasificacionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ClasificacionTecnica
                                   where u.ClasificacionTecnicaId == id
                                   select new ClasificacionTecnicaDTO()
                                   {
                                       Id = u.ClasificacionTecnicaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,                                       
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.ClasificacionTecnica
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetClasificacionById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetClasificacionById(int id)"
                    , new object[] { null });
            }
        }

        public override SubClasificacionTecnicaDTO GetSubclasificacionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.SubClasificacionTecnica
                                   where u.SubClasificacionTecnicaId == id
                                   select new SubClasificacionTecnicaDTO()
                                   {
                                       Id = u.SubClasificacionTecnicaId,
                                       ClasificacionTecnicaId = u.ClasificacionTecnicaId.Value,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       MantenimientoId = (int)EConfiguracionPortafolio.ClasificacionTecnica
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetClasificacionById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetClasificacionById(int id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoClasificacion(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ClasificacionTecnica.FirstOrDefault(x => x.ClasificacionTecnicaId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstadoClasificacion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstadoClasificacion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoSubclasificacion(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.SubClasificacionTecnica.FirstOrDefault(x => x.SubClasificacionTecnicaId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstadoClasificacion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstadoClasificacion(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ClasificacionTecnicaDTO> GetClasificacion(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ClasificacionTecnica
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))                                     
                                     && !u.FlagEliminado.Value
                                     select new ClasificacionTecnicaDTO()
                                     {
                                         Id = u.ClasificacionTecnicaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.ClasificacionTecnica,
                                         Nivel = (int)ETreeLevel.Uno,
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
                    , "Error en el metodo: List<ActivosDTO> GetClasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetClasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<SubClasificacionTecnicaDTO> GetSubclasificacion(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.SubClasificacionTecnica
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && (pag.id == 0 || u.ClasificacionTecnicaId == pag.id)
                                     && !u.FlagEliminado.Value
                                     select new SubClasificacionTecnicaDTO()
                                     {
                                         Id = u.SubClasificacionTecnicaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         MantenimientoId = (int)EConfiguracionPortafolio.SubclasificacionTecnica,
                                         Nivel = (int)ETreeLevel.Dos,
                                         EntidadRelacionId = u.ClasificacionTecnicaId.Value
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
                    , "Error en el metodo: List<ActivosDTO> GetSubclasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetSubclasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<GerenciaAllDTO> GetTAllGDAU()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Unidad
                                   join a in ctx.Area
                                   on u.AreaId equals a.AreaId
                                   join d in ctx.Division
                                   on a.DivisionId equals d.DivisionId
                                   join g in ctx.Gerencia
                                   on d.GerenciaId equals g.GerenciaId
                                   where g.FlagActivo == true 
                                   && d.FlagActivo == true
                                   && a.FlagActivo == true
                                   && u.FlagActivo == true
                                   select new GerenciaAllDTO()
                                   {
                                       NombreG = g.Nombre,
                                       DescripcionG = g.Descripcion,
                                       ResponsableG = g.Responsable,
                                       MatriculaG = g.ResponsableMatricula,
                                       CorreoG = g.ResponsableCorreo,
                                       CodigoSIGAG = g.CodigoSIGA,
                                       NombreD = d.Nombre,
                                       DescripcionD = d.Descripcion,
                                       ResponsableD = d.Responsable,
                                       MatriculaD = d.ResponsableMatricula,
                                       CorreoD = d.ResponsableCorreo,
                                       CodigoSIGAD = d.CodigoSIGA,
                                       NombreA = a.Nombre,
                                       DescripcionA = a.Descripcion,
                                       ResponsableA = a.Responsable,
                                       MatriculaA = a.ResponsableMatricula,
                                       CorreoA = a.ResponsableCorreo,
                                       CodigoSIGAA = a.CodigoSIGA,
                                       NombreU = u.Nombre,
                                       DescripcionU = u.Descripcion,
                                       ResponsableU = u.Responsable,
                                       MatriculaU = u.ResponsableMatricula,
                                       CorreoU = u.ResponsableCorreo,
                                       CodigoSIGAU = u.CodigoSIGA,
                                   }).ToList();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: GerenciaAllDTO GetTAllGDAU(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: GerenciaAllDTO GetTAllGDAU(int id)"
                    , new object[] { null });
            }
        }

        public override int GetArquitectoByCorreo(string correo)
        {
            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ArquitectoTI
                                     where u.Correo.ToUpper() == correo.ToUpper()
                                     && !u.FlagEliminado.Value
                                     select u).Count();
                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetAreaBian(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ActivosDTO GetActivosByUserIT()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoActivoInformacion
                                   where u.FlagUserIT == true
                                   select new ActivosDTO()
                                   {
                                       Id = u.TipoActivoInformacionId,
                                       Descripcion = u.Descripcion,
                                       Nombre = u.Nombre,
                                       FlujoRegistro = u.FlujoRegistro,
                                       Activo = u.FlagActivo,                                       
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       FlagUserIT = u.FlagUserIT.HasValue ? u.FlagUserIT.Value : false
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ActivosDTO GetActivosByUserIT(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ParametricasDTO GetActivosByUserIT(int id)"
                    , new object[] { null });
            }
        }

        public override List<TeamSquadDTO> GetTeamSquadByGestionadoPor(PaginationTeamSquad pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TeamSquad                                     
                                     where u.FlagActivo && u.GestionadoPorId == pag.GestionadoPorId
                                     select new TeamSquadDTO()
                                     {
                                         Id = u.EquipoId,
                                         Nombre = u.Nombre,
                                         Responsable = u.Responsable,
                                         ResponsableMatricula = u.ResponsableMatricula,
                                         ResponsableCorreo = u.ResponsableCorreo,
                                         CodigoSIGA = u.CodigoSIGA,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         GestionadoPorId = u.GestionadoPorId
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
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquadByGestionadoPor(PaginationTeamSquad pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TeamSquadDTO> GetTeamSquadByGestionadoPor(PaginationTeamSquad pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool UpdateResponsibleTeamSquad(TeamSquadDTO obj)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.TeamSquad.FirstOrDefault(x => x.EquipoId == obj.Id);
                    if (itemBD == null) return false;

                    itemBD.FechaModificacion = DateTime.Now;
                    itemBD.UsuarioModificacion = obj.UsuarioModificacion;
                    itemBD.Responsable = obj.Responsable;
                    itemBD.ResponsableCorreo = obj.ResponsableCorreo;
                    itemBD.ResponsableMatricula = obj.ResponsableMatricula;
                    var rowsAffected = ctx.SaveChanges();

                    return rowsAffected > 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<TribeLeaderDTO> GetTribeLeaderByGestionadoPor(PaginationTeamSquad pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TeamSquad
                                     join x in ctx.TribeLeader on u.EquipoId equals x.EquipoId
                                     where u.FlagActivo && u.GestionadoPorId == pag.GestionadoPorId
                                     && x.Activo
                                     select new TribeLeaderDTO()
                                     {
                                         Id = x.TribeLeaderId,
                                         Responsable = x.Responsable,
                                         ResponsableMatricula = x.ResponsableMatricula,
                                         ResponsableCorreo = x.ResponsableCorreo,
                                         Activo = x.Activo,
                                         FechaCreacion = x.FechaCreacion,
                                         EquipoId = u.EquipoId,
                                         GestionadoPorId = u.GestionadoPorId,
                                         NombreEquipo = u.Nombre
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder).FirstOrDefault();

                    var resultado = new List<TribeLeaderDTO>();
                    //var item = new TribeLeaderDTO()
                    //{
                    //    Id = 1,
                    //    Responsable = "",
                    //    ResponsableMatricula = "",
                    //    ResponsableCorreo = "",
                    //    //FechaCreacion = x.FechaCreacion,
                    //    //EquipoId = u.EquipoId,
                    //    GestionadoPorId = pag.GestionadoPorId,
                    //    //NombreEquipo = u.Nombre
                    //};
                    if(registros!=null)
                        resultado.Add(registros);

                    totalRows = resultado.Count();
                    var retorno = resultado.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TribeLeaderDTO> GetTribeLeaderByGestionadoPor(PaginationTeamSquad pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<TribeLeaderDTO> GetTribeLeaderByGestionadoPor(PaginationTeamSquad pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool UpdateResponsibleTribeLeader(TribeLeaderDTO obj, out string message)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    message = string.Empty;

                    var equiposIds = ctx.TeamSquad.Where(x => x.GestionadoPorId == obj.GestionadoPorId && x.FlagActivo)
                        .Select(x => x.EquipoId).ToArray();

                    if (equiposIds.Length == 0)
                    {
                        message = "No existen equipos asociados a la Tribu";
                        return false;
                    }

                    //Update
                    ctx.TribeLeader.Where(x => x.Activo && equiposIds.Contains(x.EquipoId)).ToList()
                         .ForEach(x =>
                         {
                             x.Responsable = obj.Responsable;
                             x.ResponsableCorreo = obj.ResponsableCorreo;
                             x.ResponsableMatricula = obj.ResponsableMatricula;
                         });

                    //Insert
                    foreach (var idEquipo in equiposIds)
                    {
                        var itemTL = ctx.TribeLeader.FirstOrDefault(x => x.EquipoId == idEquipo);
                        if (itemTL == null)
                        {
                            var newItem = new TribeLeader()
                            {
                                EquipoId = idEquipo,
                                Responsable = obj.Responsable,
                                ResponsableCorreo = obj.ResponsableCorreo,
                                ResponsableMatricula = obj.ResponsableMatricula,
                                FechaCreacion = DateTime.Now,
                                Activo = true
                            };

                            ctx.TribeLeader.Add(newItem);
                        }
                    }

                    var rowsAffected = ctx.SaveChanges();
                    var response = rowsAffected > 0;
                    message = response ? "Guardado correctamente" : "Actualizado correctamente";

                    return response;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetCamposByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ConfiguracionColumnaAplicacion
                                   where u.FlagActivo
                                   && u.NombreExcel.ToUpper().Contains(filtro.ToUpper())
                                   orderby u.NombreExcel
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.NombreExcel,
                                       Descripcion = u.NombreExcel,
                                       value = u.NombreExcel
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
    }
}
