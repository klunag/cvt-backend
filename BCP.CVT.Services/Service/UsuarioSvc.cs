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
    public class UsuarioSvc : UsuarioDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void ActivarResponsable(int responsableId, string correo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                   where u.AplicacionPortafolioResponsableId == responsableId
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.FlagActivo = true;
                        entidad.FechaModificacion = DateTime.Now;
                        entidad.ModificadoPor = "auto";
                        entidad.CorreoElectronico = correo;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditUsuario(BaseUsuarioDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //if (objeto.Id == 0)
                    //{
                    //    var entidad = new BaseUsuarios()
                    //    {
                    //        FlagActivo = objeto.Activo,
                    //        Matricula = objeto.Matricula,
                    //        Nombres = objeto.Nombres,
                    //        IdUsuario = objeto.Id
                    //    };
                    //    ctx.BaseUsuarios.Add(entidad);
                    //    ctx.SaveChanges();

                    //    return entidad.IdUsuario;
                    //}
                    //else
                    //{
                    var entidad = (from u in ctx.BaseUsuarios
                                   where u.Matricula == objeto.Matricula && u.FlagActivo.Value
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.Nombres = objeto.Nombres;
                        entidad.Matricula = objeto.Matricula;
                        entidad.FlagActivo = objeto.Estado == 1 ? true : false;
                        //entidad.FechaModificacion = DateTime.Now;
                        //entidad.FlagEstandar = objeto.FlagEstandar;
                        //entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                        ctx.SaveChanges();

                        return entidad.IdUsuario;
                    }
                    else
                    {
                        var entidadNuevo = new BaseUsuarios()
                        {
                            FlagActivo = objeto.Estado == 1 ? true : false,
                            Matricula = objeto.Matricula,
                            Nombres = objeto.Nombres,
                            IdUsuario = objeto.Id
                        };
                        ctx.BaseUsuarios.Add(entidadNuevo);
                        ctx.SaveChanges();

                        return entidadNuevo.IdUsuario;
                    }
                    //}
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        } 

        public override long AddOrEditVisitaSite(VisitaSiteDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {                    
                    var fechaActual = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(objeto.Matricula) && !string.IsNullOrWhiteSpace(objeto.Nombre))
                    {
                        var entidadNuevo = new VisitaSite()
                        {
                            Activo = true,
                            Matricula = objeto.Matricula, //This
                            Nombre = objeto.Nombre, //This
                            FechaIngreso = fechaActual,
                            UsuarioCreacion = "AUTO",
                            FechaCreacion = fechaActual,
                            VisitaSiteId = objeto.VisitaSiteId,
                            UrlSite = objeto.UrlSite //This
                        };

                        ctx.VisitaSite.Add(entidadNuevo);
                        ctx.SaveChanges();

                        return entidadNuevo.VisitaSiteId;
                    }
                    else
                        return 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return 0;
            }
        }

        public override void AddRegistroContexto(string matricula, string grupo, string correo)
        {
            var resultado = false;
            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                var registroActual = ctx.BaseUsuariosGrupo.FirstOrDefault(x => x.Matricula == matricula && x.Grupo == grupo);
                if (registroActual == null)
                {
                    resultado = true;
                }
                else
                {
                    registroActual.FlagValidar = true;
                    registroActual.FlagActivo = true;
                    //registroActual.CorreoElectronico = correo;
                    ctx.SaveChanges();
                }
            }

            if (resultado)
                AddRegistroDB(matricula, grupo, correo);
        }

        public override void AddRegistroDB(string matricula, string grupo, string correo)
        {
            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                var registro = new BaseUsuariosGrupo()
                {
                    Grupo = grupo,
                    Matricula = matricula,
                    FlagActivo = true,
                    FlagValidar = true,
                    //CorreoElectronico = correo
                };
                ctx.BaseUsuariosGrupo.Add(registro);
                ctx.SaveChanges();
            }
        }

        public override void AgregarResponsable(string matricula, string colaborador, string codigoApt, int perfil, string correo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = new AplicacionPortafolioResponsables()
                    {
                        CodigoAPT = codigoApt,
                        Colaborador = colaborador,
                        CreadoPor = "auto",
                        FechaCreacion = DateTime.Now,
                        FlagActivo = true,
                        Matricula = matricula,
                        PortafolioResponsableId = perfil,
                        FechaModificacion = DateTime.Now,
                        CorreoElectronico = correo
                    };
                    ctx.AplicacionPortafolioResponsables.Add(entidad);
                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }
        
        public override int DevolverBandejaRegistro(string matricula)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.RolOndemand
                                     where (u.Email == matricula || u.Username == matricula)
                                     && u.RoleId == (int)ERoles.Administrador
                                     && u.IsActive && u.IsDeleted==false
                                     select u.RoleId).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override string DevolverMatriculaByCorreo(string correo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var matricula = (from u in ctx.BaseUsuariosGrupo
                                     where u.CorreoElectronico.Equals(correo)
                                     && u.FlagActivo.Value
                                     select u.Matricula).FirstOrDefault();
                    return matricula;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        //public override int DevolverPerfil(string matricula)
        //{
        //    try
        //    {
        //        using(var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
        //        {
        //            var grupos = (from u in ctx.BaseUsuariosGrupo
        //                          where u.Matricula == matricula && u.FlagActivo == true
        //                          select u.Grupo).ToList();
        //            if (grupos != null && grupos.Count > 0)
        //            {
        //                //Validar perfil administrador
        //                if (grupos.Contains("E195_Administrador_PRO"))
        //                    return (int)EPerfilBCP.Administrador;
        //                else if (grupos.Contains("E195_ETI-CMDB_PRO"))
        //                    return (int)EPerfilBCP.ETICMDB;
        //                else if (grupos.Contains("E195_GestorTecnologia_PRO"))
        //                    return (int)EPerfilBCP.GestorTecnologia;
        //                else if (grupos.Contains("E195_ArquitectoTecnologia_PRO"))
        //                    return (int)EPerfilBCP.ArquitectoTecnologia;
        //                else if (grupos.Contains("E195_Seguridad_PRO"))
        //                    return (int)EPerfilBCP.Seguridad;
        //                else if (grupos.Contains("E195_Auditoria_PRO"))
        //                    return (int)EPerfilBCP.Auditoria;
        //                else if (grupos.Contains("E195_Coordinador_PRO"))
        //                    return (int)EPerfilBCP.Coordinador;
        //                else if (grupos.Contains("E195_Operador_PRO"))
        //                    return (int)EPerfilBCP.Operador;
        //                else if (grupos.Contains("E195_Gerente_PRO"))
        //                    return (int)EPerfilBCP.Gerente;
        //                else if (grupos.Contains("E195_Subsidiaria_PRO"))
        //                    return (int)EPerfilBCP.Subsidiaria;
        //                else if (grupos.Contains("E195_Consultor_PRO"))
        //                    return (int)EPerfilBCP.Consultor;
        //                else
        //                    return (int)EPerfilBCP.Consultor;


        //            }
        //            else
        //                return (int)EPerfilBCP.Consultor;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message, ex);
        //        return 0;
        //    }
        //}
        

        public override AplicacionResponsableDto GetResponsable(string matricula, int perfil, string codigoApt)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                   where u.Matricula == matricula && u.PortafolioResponsableId == perfil && u.CodigoAPT == codigoApt
                                   select new AplicacionResponsableDto()
                                   {
                                       Activo = u.FlagActivo,
                                       CodigoAPT = u.CodigoAPT,
                                       Colaborador = u.Colaborador,
                                       FechaCreacion = u.FechaCreacion,
                                       Id = u.AplicacionPortafolioResponsableId,
                                       Matricula = u.Matricula,
                                       PortafolioResponsableId = u.PortafolioResponsableId
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }    

        public override List<AplicacionResponsableDto> GetResponsables()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {                    
                    var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                   select new AplicacionResponsableDto()
                                   {
                                       Activo = u.FlagActivo,
                                       CodigoAPT = u.CodigoAPT,
                                       Colaborador = u.Colaborador,
                                       FechaCreacion = u.FechaCreacion,
                                       Id = u.AplicacionPortafolioResponsableId,
                                       Matricula = u.Matricula,
                                       PortafolioResponsableId = u.PortafolioResponsableId
                                   }).ToList();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int GetResponsables(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int GetResponsables(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override BaseUsuarioDTO GetUsuarioByMail(string correoElectronico)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.BaseUsuarios
                                   where u.CorreoElectronico.ToUpper() == correoElectronico.ToUpper()
                                   && u.FlagActivo.HasValue & u.FlagActivo.Value  
                                   select new BaseUsuarioDTO
                                   {
                                       Id = u.IdUsuario,
                                       Matricula = u.Matricula,
                                       CorreoElectronico = u.CorreoElectronico,
                                       Nombres = u.Nombres
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorUsuarioDTO
                    , "Error en el metodo: BaseUsuarioDTO GetUsuarioByMail(string correoElectronico)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorUsuarioDTO
                    , "Error en el metodo: BaseUsuarioDTO GetUsuarioByMail(string correoElectronico)"
                    , new object[] { null });
            }
        }

        public override void UpdateBaseUsuarios(string correoElectronico, string matricula, string colaborador)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.BaseUsuarios
                                   where u.CorreoElectronico.ToUpper() == correoElectronico.ToUpper()
                                   select u).FirstOrDefault();
                    if(entidad==null)
                    {
                        var objeto = new BaseUsuarios()
                        {
                            CorreoElectronico = correoElectronico,
                            FlagActivo = true,
                            Matricula = matricula,
                            Nombres = colaborador
                        };
                        ctx.BaseUsuarios.Add(objeto);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);                
            }
        }
    }
}
