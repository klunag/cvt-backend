using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;

namespace BCP.CVT.Services.Service
{
    public class TipoSvc : TipoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override TipoDTO GetTipoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tipo
                                   where u.TipoId == id
                                   select new TipoDTO()
                                   {
                                       Activo = u.Activo,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       FlagEstandar = u.FlagEstandar,
                                       FlagMostrarEstado = u.FlagMostrarEstado,
                                       Id = u.TipoId,
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

        public override List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Tipo
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                      || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     select new TipoDTO()
                                     {
                                         Id = u.TipoId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.Activo,
                                         FlagEstandar = u.FlagEstandar,
                                         FlagMostrarEstado = u.FlagMostrarEstado,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         NumTecAsociadas = (from sq in ctx.Tecnologia
                                                            where sq.TipoTecnologia == u.TipoId && sq.Activo
                                                            select 1).Count()
                                     }).OrderBy(sortName + " " + sortOrder).ToList();
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
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

        public override int AddOrEditTipo(TipoDTO objeto)
        {
            DbContextTransaction transaction = null;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.Id == 0)
                        {
                            var entidad = new Tipo()
                            {
                                Activo = objeto.Activo,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                Nombre = objeto.Nombre,
                                Descripcion = objeto.Descripcion,
                                FlagEstandar = objeto.FlagEstandar,
                                FlagMostrarEstado = objeto.FlagMostrarEstado,
                                TipoId = objeto.Id
                            };
                            ctx.Tipo.Add(entidad);
                            ctx.SaveChanges();

                            if (objeto.FlagEstandar.Value)
                            {
                                ctx.Tipo.Where(x => x.TipoId != entidad.TipoId).ToList().ForEach(x => x.FlagEstandar = false);
                                ctx.SaveChanges();
                            }

                            transaction.Commit();

                            return entidad.TipoId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.Tipo
                                           where u.TipoId == objeto.Id
                                           select u).First();
                            if (entidad != null)
                            {
                                entidad.Nombre = objeto.Nombre;
                                entidad.Descripcion = objeto.Descripcion;
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.Activo = objeto.Activo;
                                entidad.FlagEstandar = objeto.FlagEstandar;
                                entidad.FlagMostrarEstado = objeto.FlagMostrarEstado;
                                entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                                ctx.SaveChanges();                                

                                if (objeto.FlagEstandar.Value)
                                {
                                    ctx.Tipo.Where(x => x.TipoId != entidad.TipoId).ToList().ForEach(x => x.FlagEstandar = false);
                                    ctx.SaveChanges();
                                }

                                transaction.Commit();

                                return entidad.TipoId;
                            }
                            else
                                return 0;
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Tipo
                                  where u.TipoId == id
                                  select u).First();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.Activo = estado;

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

        public override List<TipoDTO> GetAllTipo()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tipo
                                   where u.Activo
                                   orderby u.Nombre
                                   select new TipoDTO()
                                   {
                                       Id = u.TipoId,
                                       Nombre = u.Nombre,
                                       Activo = u.Activo
                                   }).ToList();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetAllTipo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetAllTipo()"
                    , new object[] { null });
            }
        }

        public override bool TieneFlagEstandar()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int entidad = (from u in ctx.Tipo
                                   where u.Activo 
                                   && u.FlagEstandar.Value
                                   //&& u.TipoId != id
                                   //&& u.CodigoAPT == Id
                                   //orderby u.Nombre
                                   select u.Nombre).Count();

                    return entidad == 1;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool TieneFlagEstandar()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool TieneFlagEstandar()"
                    , new object[] { null });
            }
        }

        public override bool GetFlagEstandar(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tipo
                                   where u.Activo && u.TipoId == id
                                   //orderby u.Nombre
                                   select u.FlagEstandar.Value).FirstOrDefault();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool GetFlagEstandar(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool GetFlagEstandar(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllTipoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tipo
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool ValidarFlagEstandar(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? entidad = (from u in ctx.Tipo
                                   where u.Activo && u.TipoId == id && u.FlagEstandar.Value
                                   select true).FirstOrDefault();

                    return entidad == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool GetFlagEstandar(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool GetFlagEstandar(int id)"
                    , new object[] { null });
            }
        }

        public override List<DtoTipoTecnologia> GetAllTipoActivos()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tipo
                                   where u.Activo
                                   select new DtoTipoTecnologia()
                                   {
                                       technologyTypeId = u.TipoId,
                                       name = u.Nombre
                                   }).ToList();
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
    }
}
