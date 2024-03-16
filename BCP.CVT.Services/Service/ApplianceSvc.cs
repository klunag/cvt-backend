using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.Data.Entity.SqlServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using BCP.CVT.Services.Interface.Appliance;
using BCP.CVT.DTO.Appliance;
using BCP.CVT.Services.Log;

namespace BCP.CVT.Services.Service
{
    class ApplianceSvc : ApplianceDAO
    {

        public override void ActualizarArchivo(int id, byte[] archivo, string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    var registro = (from u in ctx.EquipoSolicitud
                                    where u.EquipoSolicitudId == id
                                    select u).First();
                    if (registro != null)
                    {
                        registro.ArchivoConstancia = archivo;
                        registro.NombreArchivo = nombre;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);

                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override void ActualizarArchivoSoftwareBase(int id, byte[] archivo, string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    var equipoid = (from e in ctx.EquipoSolicitud
                                    where e.EquipoSolicitudId == id
                                    select e.EquipoId).FirstOrDefault();

                    var registro = (from u in ctx.EquipoSolicitud
                                    where u.EquipoId == equipoid && u.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.PendienteAtencionOwner
                                    select u).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.ArchivoConstancia = archivo == null ? registro.ArchivoConstancia : archivo;
                        registro.NombreArchivo = string.IsNullOrWhiteSpace(nombre) ? registro.NombreArchivo : nombre;
                    }

                    var softwareBase = (from u in ctx.EquipoSoftwareBase
                                        where u.EquipoId == equipoid && u.FlagActivo == true
                                        select u).FirstOrDefault();
                    if (softwareBase != null)
                    {
                        softwareBase.ArchivoConstancia = archivo == null ? softwareBase.ArchivoConstancia : archivo;
                        softwareBase.NombreArchivo = string.IsNullOrWhiteSpace(nombre) ? softwareBase.NombreArchivo : nombre;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override void ActualizarArchivoSoftwareBase2(int id, byte[] archivo, string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    var registro = (from u in ctx.EquipoSolicitud
                                    where u.EquipoId == id
                                    select u).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.ArchivoConstancia = archivo == null ? registro.ArchivoConstancia : archivo;
                        registro.NombreArchivo = string.IsNullOrWhiteSpace(nombre) ? registro.NombreArchivo : nombre;
                    }

                    var softwareBase = (from u in ctx.EquipoSoftwareBase
                                        where u.EquipoId == id
                                        select u).FirstOrDefault();
                    if (softwareBase != null)
                    {
                        softwareBase.ArchivoConstancia = archivo == null ? softwareBase.ArchivoConstancia : archivo;
                        softwareBase.NombreArchivo = string.IsNullOrWhiteSpace(nombre) ? softwareBase.NombreArchivo : nombre;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override void ActualizarTipoEquipo(int equipoId, string usuario, int solicitud)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    var registro = (from u in ctx.Equipo
                                    where u.EquipoId == equipoId
                                    select u).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.TipoEquipoId = (int)ETipoEquipo.Appliance;
                        registro.ModificadoPor = usuario;
                        registro.FechaModificacion = DateTime.Now;

                        var solicitudObj = ctx.EquipoSolicitud.FirstOrDefault(x => x.EquipoSolicitudId == solicitud);

                        //Validar si ya existe el software base
                        var softwareBaseActual = ctx.EquipoSoftwareBase.FirstOrDefault(x => x.EquipoId == equipoId);
                        if (softwareBaseActual == null)
                        {
                            var softwareBase = new EquipoSoftwareBase()
                            {
                                EquipoId = equipoId,
                                FechaCreacion = DateTime.Now,
                                FechaFinSoporte = solicitudObj.FechaFinSoporte,
                                UsuarioCreacion = usuario,
                                VencimientoLicencia = solicitudObj.FechaFinSoporte,
                                FlagActivo = true,
                                FlagSeguridad = true,
                                FlagAprobado = true
                            };
                            ctx.EquipoSoftwareBase.Add(softwareBase);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditSolicitud(EquipoSolicitudDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                int ID = 0;
                int tipoEquipoActual = 0;


                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        var equipo = ctx.Equipo.FirstOrDefault(x => x.EquipoId == objRegistro.EquipoId);
                        if (equipo != null)
                            tipoEquipoActual = equipo.TipoEquipoId.Value;

                        if (objRegistro.Id == 0)
                        {
                            //Obtener información del Owner de la aplicación
                            var ownerAplicacion = ctx.ApplicationManagerCatalog.FirstOrDefault(x => x.applicationId == objRegistro.CodigoAPT
                                                                                                && x.applicationManagerId == (int)ApplicationManagerRole.Owner
                                                                                                && x.isActive);

                            var entidad = new EquipoSolicitud()
                            {
                                FechaCreacion = DateTime.Now,
                                CreadoPor = objRegistro.UsuarioCreacion,
                                Comentarios = objRegistro.Comentarios,
                                EquipoId = objRegistro.EquipoId,
                                EquipoSolicitudId = 0,
                                EstadoSolicitud = objRegistro.EstadoSolicitud,
                                FechaFinSoporte = objRegistro.FechaFinSoporte,
                                NombreUsuarioCreacion = objRegistro.NombreUsuarioCreacion,
                                TipoEquipoActual = tipoEquipoActual,
                                TipoEquipoSolicitado = (int)ETipoEquipo.Appliance,
                                CorreoSolicitante = objRegistro.CorreoSolicitante,
                                CodigoAPT = objRegistro.CodigoAPT,
                                FlagSeguridad = objRegistro.FlagSeguridad,
                                UsuarioAsignado = ownerAplicacion != null ? ownerAplicacion.username : string.Empty,
                                NombreEquipo = equipo.Nombre
                            };
                            ctx.EquipoSolicitud.Add(entidad);
                            ctx.SaveChanges();
                            ID = entidad.EquipoSolicitudId;

                            var equipoSoftwareBase = new EquipoSoftwareBase()
                            {
                                FlagSeguridad = objRegistro.FlagSeguridad,
                                CodigoAPT = objRegistro.CodigoAPT,
                                EquipoId = objRegistro.EquipoId.Value,
                                FechaCalculoId = 3,
                                FechaCreacion = DateTime.Now,
                                FlagActivo = true,
                                FlagAprobado = false,
                                UsuarioCreacion = objRegistro.UsuarioCreacion,
                                VencimientoLicencia = objRegistro.FechaFinSoporte,
                                SubdominioId = 31,
                                Hostname = equipo.Nombre
                            };
                            ctx.EquipoSoftwareBase.Add(equipoSoftwareBase);
                            ctx.SaveChanges();

                            transaction.Commit();

                            //Proceso de envío de correos
                            this.EnviarCorreoAppliance(Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI, ID, ownerAplicacion != null ? ownerAplicacion.email : string.Empty);
                        }
                        else
                        {
                            var entidad = (from u in ctx.EquipoSolicitud
                                           where u.EquipoSolicitudId == objRegistro.Id
                                           select u).First();
                            if (entidad != null)
                            {
                                entidad.Comentarios = objRegistro.Comentarios;
                                entidad.EquipoId = objRegistro.EquipoId;
                                entidad.EstadoSolicitud = objRegistro.EstadoSolicitud;
                                entidad.FechaFinSoporte = objRegistro.FechaFinSoporte;
                                entidad.NombreUsuarioModificacion = objRegistro.NombreUsuarioModificacion;
                                entidad.TipoEquipoActual = tipoEquipoActual;
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.ModificadoPor = objRegistro.UsuarioModificacion;
                                ctx.SaveChanges();
                                ID = entidad.EquipoSolicitudId;
                            }
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEditSolicitud(EquipoSolicitudDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEditSolicitud(EquipoSolicitudDTO objRegistro)"
                    , new object[] { null });
            }
        }

        /// <summary>
        /// CambiarEstadoSolicitud
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estado"></param>
        /// <param name="usuario"></param>
        /// <param name="nombres"></param>
        /// <param name="comentarios"></param>
        public override void CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres, string comentarios, string matricula_aprobador, string codigoAPT)
        {
            DbContextTransaction dbContextTransaction;
            using (var cnx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                using (dbContextTransaction = cnx.Database.BeginTransaction())
                {
                    var oEquipo_solicitud = new EquipoSolicitud();

                    if (!string.IsNullOrEmpty(id.ToString()))
                        oEquipo_solicitud = ServiceManager<ApplianceEquipoDAO>.Provider.GetEquipoSolicitud(id);

                    if (!ReferenceEquals(null, oEquipo_solicitud))
                    {
                        switch (estado)
                        {
                            case ((int)EstadoSolicitudActivosTSI.PendienteAtencionCVT):
                                AprobarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                                break;
                            //case ((int)EstadoSolicitudActivosTSI.PendienteAtencionOwner):
                            //    AprobarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                            //    break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoOwner):
                                AprobarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoOwner):
                                ActualizarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoCVT):
                                ActualizarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoCVT):
                                ActualizarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.Desestimado):
                                DesestimarSolicitudAppliance(id, usuario, nombres, comentarios, estado, matricula_aprobador, codigoAPT);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ActualizarSolicitudAppliance
        /// </summary>
        /// <param name="oEquipo_solicitud"></param>
        private void ActualizarSolicitudAppliance(int idEquipo, string user, string names, string comments, int status, string matricula_aprobador)
        {
            DbContextTransaction transaction = null;
            try
            {
                using (var cnx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var oEquipoServidor = new Equipo();
                    var oEquipoSoftwareBase = new EquipoSoftwareBase();
                    var oEquipo_solicitud = cnx.EquipoSolicitud.Where(u => u.EquipoSolicitudId == idEquipo).FirstOrDefault();
                    var itipoEquipoId = 0;
                    var iEquipoId = 0;

                    //Obtener el correo del Owner según el código APT
                    var ownerAplicacion = cnx.ApplicationManagerCatalog.FirstOrDefault(x => x.applicationId == oEquipo_solicitud.CodigoAPT && x.applicationManagerId == (int)ApplicationManagerRole.Owner && x.isActive);
                    var ownerEmail = (string.IsNullOrEmpty(ownerAplicacion.email) ? string.Empty : ownerAplicacion.email.ToString());

                    if (!string.IsNullOrEmpty(oEquipo_solicitud.EquipoId.ToString()))
                    {
                        //Obtener registro de Equipo
                        oEquipoServidor = cnx.Equipo.Where(i => i.EquipoId == (int)oEquipo_solicitud.EquipoId).FirstOrDefault();
                        //Crear software base para el equipo si en caso no existiese
                        oEquipoSoftwareBase = cnx.EquipoSoftwareBase.Where(x => x.EquipoId == (int)oEquipo_solicitud.EquipoId).FirstOrDefault();
                    }

                    if (status.Equals((int)EstadoSolicitudActivosTSI.RechazadoOwner))
                    {
                        //Actualización de campos en EquipoSolicitud
                        oEquipo_solicitud.ComentariosAprobacionRechazo = comments;
                        oEquipo_solicitud.FechaModificacion = DateTime.Now;
                        oEquipo_solicitud.ModificadoPor = matricula_aprobador;
                        oEquipo_solicitud.EstadoSolicitud = status;
                        oEquipo_solicitud.NombreUsuarioModificacion = names;
                        oEquipo_solicitud.EquipoId = null;
                        oEquipo_solicitud.AprobadoRechazadoPor = matricula_aprobador;
                        oEquipo_solicitud.NombreUsuarioAprobadoRechazo = names;
                        oEquipo_solicitud.FechaAprobacionRechazo = DateTime.Now;

                        //Eliminar registro en EquipoSoftwareBase
                        cnx.EquipoSoftwareBase.RemoveRange(cnx.EquipoSoftwareBase.Where(x => x.EquipoId == oEquipoServidor.EquipoId));

                        var equipo = cnx.Equipo.FirstOrDefault(i => i.EquipoId == oEquipoServidor.EquipoId);
                        if (equipo.FlagTemporal == true && string.IsNullOrEmpty(equipo.Ubicacion) && equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                            cnx.Equipo.Remove(equipo);

                        cnx.SaveChanges();

                        EnviarCorreoAppliance(Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_OWNER, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);
                    }
                    else if (status.Equals((int)EstadoSolicitudActivosTSI.RechazadoCVT))
                    {
                        //Actualización de campos en EquipoSolicitud
                        oEquipo_solicitud.ComentariosAprobacionRechazo = comments;
                        oEquipo_solicitud.FechaModificacion = DateTime.Now;
                        oEquipo_solicitud.ModificadoPor = matricula_aprobador;
                        oEquipo_solicitud.EstadoSolicitud = status;
                        oEquipo_solicitud.NombreUsuarioModificacion = names;
                        oEquipo_solicitud.EquipoId = null;
                        oEquipo_solicitud.AprobadoRechazoCVT = names;
                        oEquipo_solicitud.FechaAprobacionRechazoCVT = DateTime.Now;

                        //Eliminar registro en EquipoSoftwareBase
                        cnx.EquipoSoftwareBase.RemoveRange(cnx.EquipoSoftwareBase.Where(x => x.EquipoId == oEquipoServidor.EquipoId));

                        var equipo = cnx.Equipo.FirstOrDefault(i => i.EquipoId == oEquipoServidor.EquipoId);
                        if (equipo.FlagTemporal == true && string.IsNullOrEmpty(equipo.Ubicacion) && equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                            cnx.Equipo.Remove(equipo);

                        cnx.SaveChanges();

                        EnviarCorreoAppliance(Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_CVT, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);
                    }
                    else if (status.Equals((int)EstadoSolicitudActivosTSI.AprobadoCVT))
                    {
                        using (transaction = cnx.Database.BeginTransaction())
                        {
                            //Cambio a appliance
                            if (oEquipoServidor.TipoEquipoId == (int)ETipoEquipo.Servidor)
                            {
                                oEquipoServidor.TipoEquipoId = (int)ETipoEquipo.Appliance;
                                cnx.SaveChanges();
                            }

                            oEquipo_solicitud.AprobadoRechazoCVT = names;
                            oEquipo_solicitud.FechaAprobacionRechazoCVT = DateTime.Now;
                            oEquipo_solicitud.FechaModificacion = DateTime.Now;
                            oEquipo_solicitud.ModificadoPor = matricula_aprobador;
                            oEquipo_solicitud.EstadoSolicitud = status;
                            oEquipo_solicitud.NombreUsuarioModificacion = names;

                            var dia = DateTime.Now.Day;
                            var mes = DateTime.Now.Month;
                            var anio = DateTime.Now.Year;

                            //Validar si la relación previamente existe, si ya la está, solo aprobarla
                            var relacionApplianceDto = new RelacionApplianceDTO
                            {
                                EquipoId = (int)oEquipo_solicitud.EquipoId,
                                CodigoAPT = oEquipo_solicitud.CodigoAPT,
                                AmbienteId = oEquipoServidor.AmbienteId,
                                DiaRegistro = dia,
                                MesRegistro = mes,
                                AnioRegistro = anio
                            };

                            var oRelacionExistente = cnx.Relacion.FirstOrDefault(x => x.EquipoId == relacionApplianceDto.EquipoId
                                                       && x.CodigoAPT == relacionApplianceDto.CodigoAPT
                                                       && x.DiaRegistro == relacionApplianceDto.DiaRegistro
                                                       && x.MesRegistro == relacionApplianceDto.MesRegistro
                                                       && x.AnioRegistro == relacionApplianceDto.AnioRegistro);

                            if (ReferenceEquals(null, oRelacionExistente))
                            {

                                var relationObject = new Relacion()
                                {
                                    AmbienteId = 1,
                                    AnioRegistro = anio,
                                    CodigoAPT = oEquipo_solicitud.CodigoAPT,
                                    CreadoPor = matricula_aprobador,
                                    DiaRegistro = dia,
                                    EquipoId = oEquipo_solicitud.EquipoId,
                                    EstadoId = (int)EEstadoRelacion.Aprobado,
                                    FechaCreacion = DateTime.Now,
                                    FlagActivo = true,
                                    MesRegistro = mes,
                                    //RelacionId = 0,
                                    TipoId = 1
                                };
                                cnx.Relacion.Add(relationObject);
                                cnx.SaveChanges();

                                var ID = relationObject.RelacionId;

                                var tecnologiaSO = (from et in cnx.EquipoTecnologia
                                                    join t in cnx.Tecnologia on et.TecnologiaId equals t.TecnologiaId
                                                    where (et.DiaRegistro == dia) && (et.MesRegistro == mes) && (et.AnioRegistro == anio)
                                                    && (et.FlagActivo == true) && (et.EquipoId == relationObject.EquipoId) && (t.SubdominioId == 36)
                                                    select new { tecnologiaid = et.TecnologiaId }).FirstOrDefault();
                                if (tecnologiaSO != null)
                                {
                                    var objRelacionDetalle = new RelacionDetalle()
                                    {
                                        RelacionId = ID,
                                        TecnologiaId = Convert.ToInt32(tecnologiaSO.tecnologiaid), //TODO
                                        RelevanciaId = 1,
                                        FlagActivo = true,
                                        FechaCreacion = DateTime.Now,
                                        CreadoPor = matricula_aprobador,
                                    };
                                    cnx.RelacionDetalle.Add(objRelacionDetalle);
                                }
                            }
                            else
                            {
                                if (oRelacionExistente.EstadoId != (int)EEstadoRelacion.Aprobado)
                                {
                                    oRelacionExistente.EstadoId = (int)EEstadoRelacion.Aprobado;
                                    oRelacionExistente.FechaFinCuarentena = null;
                                    oRelacionExistente.FechaRegistroCuarentena = null;
                                    oRelacionExistente.FechaModificacion = DateTime.Now;
                                    oRelacionExistente.ModificadoPor = matricula_aprobador;
                                }
                            }

                            cnx.SaveChanges();
                            transaction.Commit();
                            #region USP_AprobarRelacionAppliance
                            //Activar appliance y completar relación
                            try
                            {

                                using (SqlConnection connection = new SqlConnection(Constantes.CadenaConexion))
                                {
                                    connection.Open();
                                    using (var comando = new SqlCommand("[cvt].[USP_AprobarRelacionAppliance]", connection))
                                    {
                                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                                        comando.Parameters.AddWithValue("@equipo", oEquipo_solicitud.EquipoId);
                                        int filasAfectadas = comando.ExecuteNonQuery();
                                    }
                                    connection.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                HelperLog.Error(ex.Message );
                            }
                            #endregion
                            EnviarCorreoAppliance(Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_CVT, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);
                            ProcesarCicloVidaSoftwareBase(oEquipo_solicitud.EquipoId.Value);
                        }
                    }



                    //----------------------------------------------------------------//




                    //    if (!(bool)oEquipo_solicitud.FlagSeguridad)
                    //{
                    //    if (ReferenceEquals(null, oEquipoSoftwareBase))
                    //    {
                    //        var equipoSoftwareObject = new EquipoSoftwareBase()
                    //        {
                    //            FlagSeguridad = false,
                    //            ArchivoConstancia = oEquipo_solicitud.ArchivoConstancia,
                    //            CodigoAPT = oEquipo_solicitud.CodigoAPT,
                    //            EquipoId = oEquipo_solicitud.EquipoId.Value,
                    //            EquipoSoftwareBaseId = 0,
                    //            FechaCalculoId = 3,
                    //            FechaCreacion = DateTime.Now,
                    //            FlagActivo = true,
                    //            FlagAprobado = true,
                    //            NombreArchivo = oEquipo_solicitud.NombreArchivo,
                    //            UsuarioCreacion = matricula_aprobador,
                    //            VencimientoLicencia = oEquipo_solicitud.FechaFinSoporte, 
                    //            SubdominioId = 31
                    //        };
                    //        cnx.EquipoSoftwareBase.Add(equipoSoftwareObject);
                    //    }
                    //}
                    //else
                    //{
                    //    if (status.Equals((int)EstadoSolicitudActivosTSI.AprobadoOwner))
                    //    {
                    //        if (ReferenceEquals(null, oEquipoSoftwareBase))
                    //        {
                    //            var equipoSoftwareBaseObject = new EquipoSoftwareBase()
                    //            {
                    //                FlagSeguridad = true,
                    //                ArchivoConstancia = oEquipo_solicitud.ArchivoConstancia,
                    //                CodigoAPT = oEquipo_solicitud.CodigoAPT,
                    //                EquipoId = oEquipo_solicitud.EquipoId.Value,
                    //                EquipoSoftwareBaseId = 0,
                    //                FechaCalculoId = 3,
                    //                FechaCreacion = DateTime.Now,
                    //                FlagActivo = true,
                    //                FlagAprobado = true,
                    //                NombreArchivo = oEquipo_solicitud.NombreArchivo,
                    //                UsuarioCreacion = matricula_aprobador,
                    //                VencimientoLicencia = oEquipo_solicitud.FechaFinSoporte,
                    //                Hostname = oEquipoServidor.Nombre,
                    //                SubdominioId = 31,
                    //            };
                    //            cnx.EquipoSoftwareBase.Add(equipoSoftwareBaseObject);
                    //        }
                    //        else
                    //        {
                    //            oEquipoSoftwareBase.FlagActivo = true;
                    //            oEquipoSoftwareBase.VencimientoLicencia = oEquipo_solicitud.FechaFinSoporte;
                    //            oEquipoSoftwareBase.UsuarioModificacion = matricula_aprobador;
                    //            oEquipoSoftwareBase.FechaModificacion = DateTime.Now;
                    //        }
                    //    }

                    //}

                    //cnx.SaveChanges(); 

                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "ActualizarSolicitudAppliance(int idEquipo, string user, string names, string comments, int status, string matricula_aprobador)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "ActualizarSolicitudAppliance(int idEquipo, string user, string names, string comments, int status, string matricula_aprobador)"
                    , new object[] { null });
            }
        }

        private void AprobarSolicitudAppliance(int idEquipo, string user, string names, string comments, int status, string matricula_aprobador)
        {
            using (var cnx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                var oEquipo_solicitud = cnx.EquipoSolicitud.Where(u => u.EquipoSolicitudId == idEquipo).FirstOrDefault();
                var ownerAplicacion = cnx.ApplicationManagerCatalog.FirstOrDefault(x => x.applicationId == oEquipo_solicitud.CodigoAPT && x.applicationManagerId == (int)ApplicationManagerRole.Owner && x.isActive);
                var ownerEmail = (string.IsNullOrEmpty(ownerAplicacion.email) ? string.Empty : ownerAplicacion.email.ToString());
                oEquipo_solicitud.EstadoSolicitud = (int)EstadoSolicitudActivosTSI.PendienteAtencionCVT;
                oEquipo_solicitud.FechaAprobacionRechazo = DateTime.Now;
                oEquipo_solicitud.AprobadoRechazadoPor = matricula_aprobador;
                oEquipo_solicitud.NombreUsuarioAprobadoRechazo = names;
                oEquipo_solicitud.ComentariosAprobacionRechazo = comments;
                oEquipo_solicitud.FechaModificacion = DateTime.Now;
                oEquipo_solicitud.ModificadoPor = matricula_aprobador;
                oEquipo_solicitud.NombreUsuarioModificacion = names;

                cnx.SaveChanges();
                //Enviar email a CVT y solicitador
                EnviarCorreoAppliance(Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_PENDIENTE_CVT, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);
                //Enviar email al solicitador
                EnviarCorreoAppliance(Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_OWNER, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);
            }
        }

        private void DesestimarSolicitudAppliance(int idEquipo, string user, string names, string comments, int status, string matricula_aprobador, string codigoAPT)
        {
            DbContextTransaction transaction = null;

            try
            {
                using (var cnx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = cnx.Database.BeginTransaction())
                    {
                        var oEquipo_solicitud = cnx.EquipoSolicitud.Where(u => u.EquipoSolicitudId == idEquipo).FirstOrDefault();
                        var ownerAplicacion = cnx.ApplicationManagerCatalog.FirstOrDefault(x => x.applicationId == oEquipo_solicitud.CodigoAPT && x.applicationManagerId == (int)ApplicationManagerRole.Owner && x.isActive);
                        var ownerEmail = (string.IsNullOrEmpty(ownerAplicacion.email) ? string.Empty : ownerAplicacion.email.ToString());
                        var iEquipoId = oEquipo_solicitud.EquipoId;
                        //Actualización de campos en EquipoSolicitud
                        oEquipo_solicitud.ComentariosDesestimacion = comments;
                        oEquipo_solicitud.FechaModificacion = DateTime.Now;
                        oEquipo_solicitud.ModificadoPor = user;
                        oEquipo_solicitud.EstadoSolicitud = status;
                        oEquipo_solicitud.NombreUsuarioModificacion = names;
                        oEquipo_solicitud.EquipoId = null;
                        cnx.SaveChanges();

                        //Eliminar registro en EquipoSoftwareBase
                        cnx.EquipoSoftwareBase.RemoveRange(cnx.EquipoSoftwareBase.Where(x => x.EquipoId == iEquipoId));

                        //Eliminar registro en Equipo
                        var equipo = cnx.Equipo.FirstOrDefault(i => i.EquipoId == iEquipoId);
                        if (equipo.FlagTemporal == true && string.IsNullOrEmpty(equipo.Ubicacion) && equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                            cnx.Equipo.Remove(equipo);

                        //Eliminar registro por iEquipoId
                        cnx.SaveChanges();

                        transaction.Commit();
                        //Envío de correo de desestimación
                        EnviarCorreoAppliance(Utilitarios.NOTIFICACION_DESESTIMACION_SOLICITUD_TRANSFERENCIA_OWNER, oEquipo_solicitud.EquipoSolicitudId, ownerEmail);

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }

        }

        public override void DesestimarSolicitud(int equipoId, string usuario, string nombres)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.EquipoSolicitud
                                   where u.EquipoId == equipoId && u.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoCVT
                                   select u).First();
                    if (entidad != null)
                    {
                        entidad.EstadoSolicitud = (int)EstadoSolicitudActivosTSI.Desestimado;
                        entidad.ModificadoPor = usuario;
                        entidad.NombreUsuarioModificacion = nombres;
                        entidad.FechaModificacion = DateTime.Now;
                        entidad.ComentariosDesestimacion = "Se desestima y revierte el tipo de equipo asociado al servidor";

                        //Si el equipo original era un appliance y se ha desestimado, se tiene que eliminar, caso contrario todo se mantiene
                        var equipo = (from u in ctx.Equipo
                                      where u.EquipoId == equipoId
                                      select u).First();
                        if (equipo != null)
                        {
                            if (equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                            {
                                var softwareBaseAppliance = ctx.EquipoSoftwareBase.First(x => x.EquipoId == equipoId);

                                ctx.EquipoSoftwareBase.Remove(softwareBaseAppliance);
                                ctx.Equipo.Remove(equipo);
                            }
                            else
                            {
                                equipo.TipoEquipoId = entidad.TipoEquipoActual;
                                equipo.FechaModificacion = DateTime.Now;
                                equipo.ModificadoPor = usuario;
                            }
                        }
                        ctx.SaveChanges();

                        this.EnviarCorreo((int)ETipoNotificacion.DesestimacionSolicitudTSI, entidad.EquipoSolicitudId, string.Empty);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }
        }

        public override EquipoSolicitudDTO GetSolicitudById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.EquipoSolicitud
                                         join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId into lj1
                                         from u2 in lj1.DefaultIfEmpty()
                                         where u.EquipoSolicitudId == id
                                         orderby u.FechaCreacion descending
                                         select new EquipoSolicitudDTO()
                                         {
                                             Id = u.EquipoSolicitudId,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion.Value,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             AprobadoRechazadoPor = u.AprobadoRechazadoPor,
                                             Comentarios = u.Comentarios,
                                             ArchivoConstancia = u.ArchivoConstancia,
                                             EquipoId = u.EquipoId,
                                             EstadoSolicitud = u.EstadoSolicitud,
                                             FechaAprobacionRechazo = u.FechaAprobacionRechazo,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
                                             NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                             NombreUsuarioModificacion = u.NombreUsuarioModificacion,
                                             TipoEquipoActual = u.TipoEquipoActual,
                                             TipoEquipoSolicitado = u.TipoEquipoSolicitado,
                                             NombreEquipo = (string.IsNullOrEmpty(u2.Nombre) ? u.NombreEquipo : u2.Nombre),
                                             ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
                                             ComentariosDesestimacion = u.ComentariosDesestimacion,
                                             NombreArchivo = u.NombreArchivo,
                                             CorreoSolicitante = u.CorreoSolicitante,
                                             CodigoAPT = u.CodigoAPT,
                                             FechaAprobacionRechazoCVT = u.FechaAprobacionRechazoCVT,
                                             AprobadoRechazadoPorCVT = u.AprobadoRechazoCVT
                                         }).FirstOrDefault();


                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override EquipoSolicitudDTO GetArchivoById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.EquipoSoftwareBase
                                         where u.EquipoId == id
                                         orderby u.FechaCreacion descending
                                         select new EquipoSolicitudDTO()
                                         {
                                             ArchivoConstancia = u.ArchivoConstancia,
                                             EquipoId = u.EquipoId,
                                             NombreArchivo = u.NombreArchivo
                                         }).FirstOrDefault();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override List<EquipoSolicitudDTO> GetSolicitudPendientesXAprobarCvt(string nombre
            , int estado
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string perfil
            , string usuario
            , out int totalRows)
        {
            totalRows = 0;
            var resultado = new List<EquipoSolicitudDTO>();
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    if (perfil.Contains("E195_Administrador"))
                    {
                        var registros = (from u in ctx.EquipoSolicitud
                                         join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId into lj1
                                         from u2 in lj1.DefaultIfEmpty()
                                         where (u2.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre))
                                         && (estado == 0 || u.EstadoSolicitud == estado)
                                         orderby u.FechaCreacion descending
                                         select new EquipoSolicitudDTO()
                                         {
                                             Id = u.EquipoSolicitudId,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion.Value,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             AprobadoRechazadoPor = u.AprobadoRechazadoPor,
                                             Comentarios = u.Comentarios,
                                             ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
                                             ComentariosDesestimacion = u.ComentariosDesestimacion,
                                             CorreoSolicitante = u.CorreoSolicitante,
                                             EquipoId = u.EquipoId,
                                             EstadoSolicitud = u.EstadoSolicitud,
                                             FechaAprobacionRechazo = u.FechaAprobacionRechazo,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
                                             NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                             NombreUsuarioModificacion = u.NombreUsuarioModificacion,
                                             TipoEquipoActual = u.TipoEquipoActual,
                                             TipoEquipoSolicitado = u.TipoEquipoSolicitado,
                                             NombreEquipo = (string.IsNullOrEmpty(u2.Nombre) ? u.NombreEquipo : u2.Nombre),
                                             UsuarioAsignado = u.UsuarioAsignado,
                                             UsuarioConsulta = usuario,
                                             Perfil = perfil,
                                             FlagRegistroEquipo = u.FlagRegistroEquipo,
                                             FechaAprobacionRechazoCVT = u.FechaAprobacionRechazoCVT,
                                             AprobadoRechazadoPorCVT = u.AprobadoRechazoCVT
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }

            return resultado;

        }

        public override List<EquipoSolicitudDTO> GetSolicitudes(string nombre
            , int estado
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string perfil
            , string usuario
            , out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        if (perfil.Contains("E195_Administrador") || perfil.Contains("E195_Seguridad"))
                        {
                            var registros = (from u in ctx.EquipoSolicitud
                                             join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId into lj1
                                             from u2 in lj1.DefaultIfEmpty()
                                             where (u2.Nombre.ToUpper().Contains(nombre.ToUpper())
                                             || string.IsNullOrEmpty(nombre))
                                             && (estado == 0 || u.EstadoSolicitud == estado)
                                             orderby u.FechaCreacion descending
                                             select new EquipoSolicitudDTO()
                                             {
                                                 Id = u.EquipoSolicitudId,
                                                 UsuarioCreacion = u.CreadoPor,
                                                 FechaCreacion = u.FechaCreacion.Value,
                                                 FechaModificacion = u.FechaModificacion,
                                                 UsuarioModificacion = u.ModificadoPor,
                                                 AprobadoRechazadoPor = u.AprobadoRechazadoPor,
                                                 Comentarios = u.Comentarios,
                                                 ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
                                                 ComentariosDesestimacion = u.ComentariosDesestimacion,
                                                 CorreoSolicitante = u.CorreoSolicitante,
                                                 EquipoId = u.EquipoId,
                                                 EstadoSolicitud = u.EstadoSolicitud,
                                                 FechaAprobacionRechazo = u.FechaAprobacionRechazo,
                                                 FechaFinSoporte = u.FechaFinSoporte,
                                                 NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
                                                 NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                                 NombreUsuarioModificacion = u.NombreUsuarioModificacion,
                                                 TipoEquipoActual = u.TipoEquipoActual,
                                                 TipoEquipoSolicitado = u.TipoEquipoSolicitado,
                                                 NombreEquipo = (string.IsNullOrEmpty(u2.Nombre) ? u.NombreEquipo : u2.Nombre),
                                                 UsuarioAsignado = u.UsuarioAsignado,
                                                 UsuarioConsulta = usuario,
                                                 Perfil = perfil,
                                                 FlagRegistroEquipo = u.FlagRegistroEquipo,
                                                 FechaAprobacionRechazoCVT = u.FechaAprobacionRechazoCVT,
                                                 AprobadoRechazadoPorCVT = u.AprobadoRechazoCVT
                                             }).OrderBy(sortName + " " + sortOrder);

                            totalRows = registros.Count();
                            var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            return resultado;
                        }
                        else
                        {
                            var registros = (from u in ctx.EquipoSolicitud
                                             join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId into lj1
                                             from u2 in lj1.DefaultIfEmpty()
                                             where (u2.Nombre.ToUpper().Contains(nombre.ToUpper())
                                             || string.IsNullOrEmpty(nombre))
                                             && (estado == 0 || u.EstadoSolicitud == estado)
                                             && (u.UsuarioAsignado == usuario || u.CreadoPor == usuario)
                                             orderby u.FechaCreacion descending
                                             select new EquipoSolicitudDTO()
                                             {
                                                 Id = u.EquipoSolicitudId,
                                                 UsuarioCreacion = u.CreadoPor,
                                                 FechaCreacion = u.FechaCreacion.Value,
                                                 FechaModificacion = u.FechaModificacion,
                                                 UsuarioModificacion = u.ModificadoPor,
                                                 AprobadoRechazadoPor = u.AprobadoRechazadoPor,
                                                 Comentarios = u.Comentarios,
                                                 ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
                                                 ComentariosDesestimacion = u.ComentariosDesestimacion,
                                                 CorreoSolicitante = u.CorreoSolicitante,
                                                 EquipoId = u.EquipoId,
                                                 EstadoSolicitud = u.EstadoSolicitud,
                                                 FechaAprobacionRechazo = u.FechaAprobacionRechazo,
                                                 FechaFinSoporte = u.FechaFinSoporte,
                                                 NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
                                                 NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                                 NombreUsuarioModificacion = u.NombreUsuarioModificacion,
                                                 TipoEquipoActual = u.TipoEquipoActual,
                                                 TipoEquipoSolicitado = u.TipoEquipoSolicitado,
                                                 NombreEquipo = (string.IsNullOrEmpty(u2.Nombre) ? u.NombreEquipo : u2.Nombre),
                                                 UsuarioAsignado = u.UsuarioAsignado,
                                                 UsuarioConsulta = usuario,
                                                 Perfil = perfil,
                                                 FlagRegistroEquipo = u.FlagRegistroEquipo,
                                                 FechaAprobacionRechazoCVT = u.FechaAprobacionRechazoCVT,
                                                 AprobadoRechazadoPorCVT = u.AprobadoRechazoCVT
                                             }).OrderBy(sortName + " " + sortOrder);

                            totalRows = registros.Count();
                            var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            return resultado;
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ValidacionSolicitudDTO GetValidacion(int id)
        {
            var retorno = new ValidacionSolicitudDTO();
            retorno.Procesar = true;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    var registro = (from u in ctx.Equipo
                                    where u.EquipoId == id
                                    select u).FirstOrDefault();
                    if (registro != null)
                    {
                        if (registro.TipoEquipoId == (int)ETipoEquipo.Appliance)
                        {
                            retorno.Procesar = false;
                            retorno.Mensaje = "El equipo seleccionado ya está catalogado como un Appliance, seleccione otro equipo.";
                        }
                    }

                    if (retorno.Procesar)
                    {
                        var estadoSolicitudes = new List<int>() {
                            (int)EstadoSolicitudActivosTSI.AprobadoOwner,
                            (int)EstadoSolicitudActivosTSI.PendienteAtencionOwner,
                            (int)EstadoSolicitudActivosTSI.Registrado
                        };
                        var solicitudes = (from u in ctx.EquipoSolicitud
                                           where u.EquipoId == id && estadoSolicitudes.Contains(u.EstadoSolicitud)
                                           select true).FirstOrDefault();
                        if (solicitudes)
                        {
                            retorno.Procesar = false;
                            retorno.Mensaje = "El equipo seleccionado ya tiene solicitudes en curso, desestimela o seleccione otro equipo.";
                        }

                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        private void EnviarCorreo(int tipoNotificacion, int solicitud, string owner)
        {
            var emailService = new Email.MailingManager();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.EquipoSolicitud
                                         join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId
                                         where u.EquipoSolicitudId == solicitud
                                         orderby u.FechaCreacion descending
                                         select new EquipoSolicitudDTO()
                                         {
                                             Id = u.EquipoSolicitudId,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion.Value,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             AprobadoRechazadoPor = u.AprobadoRechazadoPor,
                                             Comentarios = u.Comentarios,
                                             ArchivoConstancia = u.ArchivoConstancia,
                                             EquipoId = u.EquipoId,
                                             EstadoSolicitud = u.EstadoSolicitud,
                                             FechaAprobacionRechazo = u.FechaAprobacionRechazo,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
                                             NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                             NombreUsuarioModificacion = u.NombreUsuarioModificacion,
                                             TipoEquipoActual = u.TipoEquipoActual,
                                             TipoEquipoSolicitado = u.TipoEquipoSolicitado,
                                             NombreEquipo = u2.Nombre,
                                             ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
                                             ComentariosDesestimacion = u.ComentariosDesestimacion,
                                             NombreArchivo = u.NombreArchivo,
                                             CorreoSolicitante = u.CorreoSolicitante,
                                             FlagRegistroEquipo = u.FlagRegistroEquipo
                                         }).FirstOrDefault();

                        var notificacion = ctx.TipoNotificacion.FirstOrDefault(x => x.TipoNotificacionId == tipoNotificacion);
                        if (notificacion != null && registros != null)
                        {
                            var cuerpo = notificacion.Cuerpo;
                            var asunto = notificacion.Asunto;

                            cuerpo = cuerpo.Replace("FECHA_APROBACION", registros.FechaAprobacionRechazoToString);
                            cuerpo = cuerpo.Replace("TIPO_EQUIPO_FINAL", registros.TipoEquipoSolicitadoToString);
                            cuerpo = cuerpo.Replace("TIPO_EQUIPO_INICIAL", registros.TipoEquipoActualToString);

                            if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.PendienteAtencionOwner)
                                cuerpo = cuerpo.Replace("COMENTARIOS", registros.Comentarios);
                            else if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.Desestimado)
                                cuerpo = cuerpo.Replace("COMENTARIOS", registros.ComentariosDesestimacion);
                            else if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoOwner
                                || registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.RechazadoOwner)
                                cuerpo = cuerpo.Replace("COMENTARIOS", registros.ComentariosAprobacionRechazo);

                            cuerpo = cuerpo.Replace("SOLICITANTE", registros.NombreUsuarioCreacion);
                            cuerpo = cuerpo.Replace("TIPO_EQUIPO", registros.TipoEquipoActualToString);
                            cuerpo = cuerpo.Replace("EQUIPO", registros.NombreEquipo);

                            if (!registros.FlagRegistroEquipo.HasValue)
                            {
                                cuerpo = cuerpo.Replace("[ACCION]", "cambio de equipo a");
                                asunto = asunto.Replace("[ACCION]", "cambio de equipo a");
                            }
                            else
                            {
                                if (registros.FlagRegistroEquipo.Value)
                                {
                                    cuerpo = cuerpo.Replace("[ACCION]", "registro de");
                                    asunto = asunto.Replace("[ACCION]", "registro de");
                                }
                                else
                                {
                                    cuerpo = cuerpo.Replace("[ACCION]", "cambio de equipo a");
                                    asunto = asunto.Replace("[ACCION]", "cambio de equipo a");
                                }
                            }

                            if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.RegistroSolicitudTSI)
                            {
                                var para = notificacion.Para.Split(';').ToList();
                                var cc = notificacion.ConCopia.Split(';').ToList();
                                cc.Add(registros.CorreoSolicitante);

                                emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
                            }
                            else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.DesestimacionSolicitudTSI)
                            {
                                var para = notificacion.Para.Split(';').ToList();
                                var cc = notificacion.ConCopia.Split(';').ToList();
                                cc.Add(registros.CorreoSolicitante);

                                emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
                            }
                            else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.AprobacionSolicitudTSI)
                            {
                                var para = new List<string>() { registros.CorreoSolicitante };
                                var cc = notificacion.ConCopia.Split(';').ToList();
                                cc.Add(registros.CorreoSolicitante);

                                emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
                            }
                            else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.RechazoSolicitudTSI)
                            {
                                var para = new List<string>() { registros.CorreoSolicitante };
                                var cc = notificacion.ConCopia.Split(';').ToList();
                                cc.Add(registros.CorreoSolicitante);

                                emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
                            }
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public void EnviarCorreoAppliance(string notification_type, int id_solicitud, string owner)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;
                    var emailService = new Email.MailingManager();

                    var dataEquipoSolicitud = ServiceManager<ApplianceEquipoDAO>.Provider.GetEquipoSolicitudXId(id_solicitud);
                    var oTipoNotificacion = ServiceManager<ApplianceEquipoDAO>.Provider.GetTipoNotificacion(notification_type); //ctx.TipoNotificacion.FirstOrDefault(x => x.TipoNotificacionId == tipoNotificacion);

                    if (!ReferenceEquals(null, oTipoNotificacion)
                        && !ReferenceEquals(null, dataEquipoSolicitud))
                    {
                        var body = oTipoNotificacion.Cuerpo;
                        var affair = oTipoNotificacion.Asunto;

                        body = body.Replace("FECHA_APROBACION", dataEquipoSolicitud.FechaAprobacionRechazoToString);
                        body = body.Replace("TIPO_EQUIPO_FINAL", dataEquipoSolicitud.TipoEquipoSolicitadoToString);
                        body = body.Replace("TIPO_EQUIPO_INICIAL", dataEquipoSolicitud.TipoEquipoActualToString);

                        #region Estado de solicitud - Comentarios
                        switch (dataEquipoSolicitud.EstadoSolicitud)
                        {
                            case ((int)EstadoSolicitudActivosTSI.PendienteAtencionOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.Comentarios);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.Desestimado):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosDesestimacion);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.PendienteAtencionCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.Comentarios);
                                break;
                        }
                        #endregion

                        body = body.Replace("SOLICITANTE", dataEquipoSolicitud.NombreUsuarioCreacion);
                        body = body.Replace("TIPO_EQUIPO", dataEquipoSolicitud.TipoEquipoActualToString);
                        body = body.Replace("EQUIPO", dataEquipoSolicitud.NombreEquipo);

                        if (!dataEquipoSolicitud.FlagRegistroEquipo.HasValue)
                        {
                            body = body.Replace("[ACCION]", "cambio de equipo a");
                            affair = affair.Replace("[ACCION]", "cambio de equipo a");
                        }
                        else
                        {
                            if (dataEquipoSolicitud.FlagRegistroEquipo.Value)
                            {
                                body = body.Replace("[ACCION]", "registro de");
                                affair = affair.Replace("[ACCION]", "registro de");
                            }
                            else
                            {
                                body = body.Replace("[ACCION]", "cambio de equipo a");
                                affair = affair.Replace("[ACCION]", "cambio de equipo a");
                            }
                        }

                        if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI)
                        {
                            var para = new List<string>() { owner.ToString() };
                            var cc = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_DESESTIMACION_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { owner.ToString() };
                            var cc = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_CVT)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_CVT)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_PENDIENTE_CVT)
                        {
                            var para = oTipoNotificacion.Para.Split(';').ToList();
                            var cc = oTipoNotificacion.ConCopia.Split(';').ToList();
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                    }
                }
            }
        }

        public override void RevertirSolicitud(int equipoId, string usuario, string nombres)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.EquipoSolicitud
                                   where u.EquipoId == equipoId && u.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoOwner
                                   select u).First();
                    if (entidad != null)
                    {
                        entidad.EstadoSolicitud = (int)EstadoSolicitudActivosTSI.Desestimado;
                        entidad.ModificadoPor = usuario;
                        entidad.NombreUsuarioModificacion = nombres;
                        entidad.FechaModificacion = DateTime.Now;
                        entidad.ComentariosDesestimacion = "Se desestima y revierte el tipo de equipo asociado al servidor";

                        //Si el equipo original era un appliance y se ha desestimado, se tiene que eliminar, caso contrario todo se mantiene
                        var equipo = (from u in ctx.Equipo
                                      where u.EquipoId == equipoId
                                      select u).First();
                        if (equipo != null)
                        {
                            var softwareBaseAppliance = ctx.EquipoSoftwareBase.First(x => x.EquipoId == equipoId);
                            ctx.EquipoSoftwareBase.Remove(softwareBaseAppliance);

                            equipo.TipoEquipoId = entidad.TipoEquipoActual;
                            equipo.FechaModificacion = DateTime.Now;
                            equipo.ModificadoPor = usuario;
                        }
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres)"
                    , new object[] { null });
            }
        }

        private void ProcesarCicloVidaSoftwareBase(int equipoId)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_INSERT_INTO_EQUIPOCICLOVIDA_EQUIPO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@INDICE_OBSOLESCENCIA_POR_DEFECTO", "INDICE_OBSOLESCENCIA_POR_DEFECTO"));
                        comando.Parameters.Add(new SqlParameter("@FRECUENCIA_MESES_CALCULO_INDICE_OBSOLESCENCIA", "FRECUENCIA_MESES_CALCULO_INDICE_OBSOLESCENCIA"));
                        comando.Parameters.Add(new SqlParameter("@EQUIPO", equipoId));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
            }
        }
    }
}
