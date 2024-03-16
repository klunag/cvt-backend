using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.Services.Service
{
    class ArchivosCvtSvc : ArchivosCvtDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int AddOrEdit(ArchivosCvtDTO objRegistro)
        {
            try
            {
                int ID = 0;
                var CURRENT_DATE = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                               where t.CodigoInterno == objRegistro.TablaProcedenciaId
                                               && t.FlagActivo
                                               select t.TablaProcedenciaId).FirstOrDefault();

                    if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + objRegistro.TablaProcedenciaId);

                    if (objRegistro.Id == 0)
                    {
                        var entidad = new ArchivosCVT()
                        {
                            TablaProcedenciaId = TablaProcedenciaId.Value,
                            EntidadId = objRegistro.EntidadId,
                            Nombre = objRegistro.Nombre,
                            Contenido = objRegistro.Contenido,
                            Activo = objRegistro.Activo,
                            FechaCreacion = CURRENT_DATE,
                            UsuarioCreacion = objRegistro.UsuarioCreacion,
                            NombreRef = objRegistro.NombreRef,
                            DescripcionRef = objRegistro.DescripcionRef
                        };
                        ctx.ArchivosCVT.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.ArchivoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ArchivosCVT
                                       where u.ArchivoId == objRegistro.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objRegistro.Nombre;
                            entidad.Contenido = objRegistro.Contenido;
                            entidad.Activo = objRegistro.Activo;
                            entidad.FechaModificacion = CURRENT_DATE;
                            entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                            ctx.SaveChanges();
                            ID = entidad.ArchivoId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);                
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
        }
        public override int AddOrEdit2(ArchivosCvtDTO objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad3 = ctx.ApplicationFile.FirstOrDefault(x => x.ApplicationId == objRegistro.AppId &&
                    x.FileType == (int)FileType.ArchivoDesestimacion);
                    if (entidad3 != null)
                    {
                        ctx.ApplicationFile.Remove(entidad3);
                    }



                    var entidad = new ApplicationFile()
					{
						ApplicationId = objRegistro.AppId,
						Comentario = objRegistro.DescripcionRef,
                        ArchivoAsociado = objRegistro.Contenido,
                        Nombre = objRegistro.Nombre,
                        FileType = (int)FileType.ArchivoDesestimacion
                    };
                    var entidad2 = ctx.Application.FirstOrDefault(x => x.AppId == objRegistro.AppId);
                    if (entidad2 != null)
                    {
                    
                        entidad2.commentsObserved = objRegistro.DescripcionRef;

                    
                    }
                    ctx.ApplicationFile.Add(entidad);
					ctx.SaveChanges();
                    ID = entidad.IdApplicationFile;

               
                    
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
        }
        public override int AddOrEdit3(ArchivosCvtDTO objRegistro)
        {
            int ID = 0;

            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad2 = ctx.ApplicationFile.FirstOrDefault(x => x.ApplicationId == objRegistro.AppId &&
                    x.FileType== (int)FileType.ArchivoSeguridad);
                    if (entidad2 != null)
                    {
                        ctx.ApplicationFile.Remove(entidad2);
                    }

                    var entidad = new ApplicationFile()
                    {
                        ApplicationId = objRegistro.AppId,
                        Comentario = "Archivo Adjunto de Seguridad",
                        ArchivoAsociado = objRegistro.Contenido,
                        Nombre = objRegistro.Nombre,
                        FileType= (int)FileType.ArchivoSeguridad
                    };

                    ctx.ApplicationFile.Add(entidad);
                    
                    ctx.SaveChanges();
                    ID = entidad.IdApplicationFile;
                }                
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }

            //Registro del archivo en la bitácora
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var aplicacion = ctx.Application.FirstOrDefault(x => x.AppId == objRegistro.AppId);
                    if (aplicacion != null)
                    {
                        var BitacoraMensaje = string.Empty;
                        var dateAndTime = DateTime.Now;
                        string date = dateAndTime.ToString("dd'/'MM'/'yyyy");
                        var hour = dateAndTime.ToString("HH:mm:ss");

                        BitacoraMensaje = "" + objRegistro.NombresCompletos + "" + "(" + objRegistro.Matricula + ")" + " " +
                            " modificó el archivo de seguridad de la aplicación " + "" + aplicacion.applicationId + "" + " - " + "" + aplicacion.applicationName + ", el archivo tiene el siguiente nombre:" 
                            + objRegistro.Nombre;

                        BitacoraMensaje = BitacoraMensaje + ", el día " + "" + date + "" + " a las " + "" + hour + "";

                        var registroBitacora = new BitacoraAcciones()
                        {
                            CodigoAPT = aplicacion.applicationId,
                            DetalleBitacora = BitacoraMensaje,
                            CreadoPor = objRegistro.Matricula,
                            FechaCreacion = DateTime.Now,
                            NombreUsuarioCreacion = objRegistro.NombresCompletos,
                            Archivo = objRegistro.Contenido,
                            NombreArchivo = objRegistro.Nombre
                        };

                        ctx.BitacoraAcciones.Add(registroBitacora);
                        ctx.SaveChanges();
                    }                    
                    ctx.SaveChanges();                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }

            return ID;
        }


        public override int AddOrEditTempFile(ArchivosCvtDTO objRegistro)
        {
            int ID = 0;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var application = ctx.Application.FirstOrDefault(x => x.AppId == objRegistro.AppId);
                    var archivoAnterior = ctx.ApplicationFile.FirstOrDefault(x => x.ApplicationId == objRegistro.AppId && x.FileType == (int)FileType.ArchivoSeguridad);
                    var entidad2 = ctx.ApplicationFile.FirstOrDefault(x => x.ApplicationId == objRegistro.AppId &&
                    x.FileType == (int)FileType.ArchivoSeguridadTemporal);
                    if (entidad2 != null)
                    {
                        ctx.ApplicationFile.Remove(entidad2);
                    }

                    var entidad = new ApplicationFile()
                    {
                     
                        Comentario = "Archivo Adjunto de Seguridad Temporal",
                        ArchivoAsociado = objRegistro.Contenido,
                        Nombre = objRegistro.Nombre,
                        FileType = (int)FileType.ArchivoSeguridadTemporal,
                        SolicitudId = objRegistro.SolId,
                    };

                    ctx.ApplicationFile.Add(entidad);

                    ctx.SaveChanges();
                    ID = entidad.IdApplicationFile;


                    SolicitudCampos campos = new SolicitudCampos();
                    campos.ApplicationId = application.applicationId;
                    campos.ColumnaId = (int)Campos.ArchivoAdjuntoSeguridad;
                    campos.ValorAnterior =archivoAnterior==null?"" : Convert.ToString(archivoAnterior.IdApplicationFile);
                    campos.NuevoValor = Convert.ToString(ID);
                    campos.EstadoPendiente = true;
                    campos.SolicitudId = objRegistro.SolId;
                    ctx.SolicitudCampos.Add(campos);

                    ctx.SaveChanges();
  
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }

            //Registro del archivo en la bitácora
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var aplicacion = ctx.Application.FirstOrDefault(x => x.AppId == objRegistro.AppId);
                    if (aplicacion != null)
                    {
                        var BitacoraMensaje = string.Empty;
                        var dateAndTime = DateTime.Now;
                        string date = dateAndTime.ToString("dd'/'MM'/'yyyy");
                        var hour = dateAndTime.ToString("HH:mm:ss");

                        BitacoraMensaje = "" + objRegistro.NombresCompletos + "" + "(" + objRegistro.Matricula + ")" + " " +
                            " generó una solicitud de modificación del archivo de seguridad de la aplicación " + "" + aplicacion.applicationId + "" + " - " + "" + aplicacion.applicationName + ", el archivo tiene el siguiente nombre:"
                            + objRegistro.Nombre;

                        BitacoraMensaje = BitacoraMensaje + ", el día " + "" + date + "" + " a las " + "" + hour + "";

                        var registroBitacora = new BitacoraAcciones()
                        {
                            CodigoAPT = aplicacion.applicationId,
                            DetalleBitacora = BitacoraMensaje,
                            CreadoPor = objRegistro.Matricula,
                            FechaCreacion = DateTime.Now,
                            NombreUsuarioCreacion = objRegistro.NombresCompletos,
                            Archivo = objRegistro.Contenido,
                            NombreArchivo = objRegistro.Nombre
                        };

                        ctx.BitacoraAcciones.Add(registroBitacora);
                        ctx.SaveChanges();
                    }
                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }

            return ID;
        }



        public override List<ArchivosCvtDTO> GetArchivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.ArchivosCVT
                                         join t in ctx.TablaProcedencia on u.TablaProcedenciaId equals t.TablaProcedenciaId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         where ((u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.NombreRef.ToUpper().Contains(filtro.ToUpper())
                                         || u.DescripcionRef.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro)) && t.TablaProcedenciaId.Equals((int)ETablaProcedencia.CVT_Otros))
                                         select new ArchivosCvtDTO()
                                         {
                                             Id = u.ArchivoId,
                                             Nombre = u.Nombre,
                                             TablaProcedenciaId = u.TablaProcedenciaId,
                                             TablaProcedenciaStr = (t != null) ? t.Nombre : "",
                                             NombreRef = u.NombreRef != null ? u.NombreRef : "",
                                             DescripcionRef = u.DescripcionRef != null ? u.DescripcionRef : "",
                                             EntidadId = u.EntidadId,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ArchivosCvtDTO GetByEntidadIdByProcedenciaId(string entidadId, int procedenciaId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                                   where t.CodigoInterno == procedenciaId
                                                   && t.FlagActivo
                                                   select t.TablaProcedenciaId).FirstOrDefault();

                        if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + procedenciaId);

                        var entidad = (from u in ctx.ArchivosCVT
                                       where u.Activo && u.EntidadId == entidadId && u.TablaProcedenciaId == TablaProcedenciaId
                                       select new ArchivosCvtDTO()
                                       {
                                           Id = u.ArchivoId,
                                           Nombre = u.Nombre
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetByEntidadIdByProcedenciaId(string entidadId, int procedenciaId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetByEntidadIdByProcedenciaId(string entidadId, int procedenciaId)"
                    , new object[] { null });
            }
        }

        public override ArchivosCvtDTO GetById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.ArchivosCVT
                                       where u.Activo
                                       && u.ArchivoId == id
                                       select new ArchivosCvtDTO()
                                       {
                                           Id = u.ArchivoId,
                                           Nombre = u.Nombre,
                                           Contenido = u.Contenido
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override ApplicationFileDTO GetById2(int appId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.ApplicationFile
                                       where u.ApplicationId == appId
                                       && u.FileType == (int)FileType.ArchivoDesestimacion
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdApplicationFile,
                                           Nombre = u.Nombre,
                                           Comentario = u.Comentario,
                                           ApplicationId=u.ApplicationId,
                                           ArchivoAsociado=u.ArchivoAsociado
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override ApplicationFileDTO GetById3(int appId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.ApplicationFile
                                       where u.ApplicationId == appId
                                       && u.FileType==(int)FileType.ArchivoSeguridad
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdApplicationFile,
                                           Nombre = u.Nombre,
                                           Comentario = u.Comentario,
                                           ApplicationId = u.ApplicationId,
                                           ArchivoAsociado = u.ArchivoAsociado
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override ApplicationFileDTO GetById4(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.ApplicationFile
                                       where u.IdApplicationFile == Id
                                     
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdApplicationFile,
                                           Nombre = u.Nombre,
                                           Comentario = u.Comentario,
                                           ApplicationId = u.ApplicationId,
                                           ArchivoAsociado = u.ArchivoAsociado
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }


        public override ApplicationFileDTO GetByIdGST(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.SolicitudArchivos
                                       where u.IdSolicitud == Id
        
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdSolicitudArchivos,
                                           Nombre = u.NombreConformidadGST,
                             
                                  
                                           ArchivoAsociado = u.ConformidadGST
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override ApplicationFileDTO GetByIdTicket(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.SolicitudArchivos
                                       where u.IdSolicitud == Id
                                  
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdSolicitudArchivos,
                                           Nombre = u.NombreTicketEliminacion,


                                           ArchivoAsociado = u.TicketEliminacion
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override ApplicationFileDTO GetByIdRatificacion(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 180;
                        var entidad = (from u in ctx.SolicitudArchivos
                                       where u.IdSolicitud == Id
                                 
                                       select new ApplicationFileDTO()
                                       {
                                           IdApplicationFile = u.IdSolicitudArchivos,
                                           Nombre = u.NombreRatificacion,


                                           ArchivoAsociado = u.Ratificacion
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: ArchivosCvtDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override void DeleteFile(int id, string usuario)
        {
            try
            {
                int ID = 0;
                var CURRENT_DATE = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var entidad = (from u in ctx.ArchivosCVT
                                   where u.ArchivoId == id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.Activo = false;
                        entidad.FechaModificacion = CURRENT_DATE;
                        entidad.UsuarioModificacion = usuario;
                        ctx.SaveChanges();
                        //ID = entidad.ArchivoId;
                    }
                }
                //return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorArchivoCVTDTO
                    , "Error en el metodo: int AddOrEdit(ArchivosCvtDTO objRegistro)"
                    , new object[] { null });
            }
        }
    }
}
