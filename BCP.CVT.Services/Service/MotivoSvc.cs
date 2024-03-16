using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class MotivoSvc : MotivoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int AddOrEditMotivo(MotivoDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Motivo()
                        {
                            MotivoId = objeto.Id,
                            FlagActivo = objeto.Activo,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            CreadoPor = objeto.UsuarioCreacion
                        };
                        ctx.Motivo.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.MotivoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Motivo
                                       where u.MotivoId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FlagActivo = objeto.Activo;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.CreadoPor = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.MotivoId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: int AddOrEditMotivo(MotivoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: int AddOrEditMotivo(MotivoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Motivo
                                  where u.MotivoId == id
                                  select u).First();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.CreadoPor = usuario;
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
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: int bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: int bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteMotivoById(int? Id, string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Motivo
                                        where u.FlagActivo
                                        && (Id == null || u.MotivoId == Id)
                                        && (string.IsNullOrEmpty(nombre) || u.Nombre.ToUpper().Equals(nombre.ToUpper()))
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: bool ExisteMotivoById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: bool ExisteMotivoById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<MotivoDTO> GetAllMotivo()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Motivo
                                       where u.FlagActivo
                                       orderby u.Nombre
                                       select new MotivoDTO()
                                       {
                                           Id = u.MotivoId,
                                           Nombre = u.Nombre,
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<MotivoDTO> GetAllMotivo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<MotivoDTO> GetAllMotivo()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllMotivoByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Motivo
                                       where u.FlagActivo
                                       && (string.IsNullOrEmpty(filtro)
                                       || (u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                       || (u.Descripcion).ToUpper().Contains(filtro.ToUpper())
                                       )
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.MotivoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllMotivoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllMotivoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<MotivoDTO> GetMotivo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Motivo
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         select new MotivoDTO()
                                         {
                                             Id = u.MotivoId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                         }).OrderBy(sortName + " " + sortOrder).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        if(resultado != null)
                        {
                            foreach(var item in resultado)
                            {
                                item.CantidadTecnologiasAsociadas = (from u in ctx.Tecnologia
                                                                     where u.Activo
                                                                     && !u.FlagEliminado
                                                                     && u.MotivoId == item.Id
                                                                     select true).Count();
                            }
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<MotivoDTO> GetMotivo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: List<MotivoDTO> GetMotivo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override MotivoDTO GetMotivoById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Motivo
                                       where u.MotivoId == id
                                       select new MotivoDTO()
                                       {
                                           Id = u.MotivoId,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           Activo = u.FlagActivo,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.CreadoPor,
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: MotivoDTO GetMotivoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: MotivoDTO GetMotivoById(int id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteMotivoByNombre(int? Id, string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Motivo
                                        where u.FlagActivo
                                        && (Id == null || u.MotivoId != Id)
                                        && (string.IsNullOrEmpty(nombre) || u.Nombre.ToUpper().Equals(nombre.ToUpper()))
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: bool ExisteMotivoByNombre(int? Id, string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorMotivoDTO
                    , "Error en el metodo: bool ExisteMotivoByNombre(int? Id, string nombre)"
                    , new object[] { null });
            }
        }
    }
}
