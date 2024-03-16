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
    public class CriticidadSvc : CriticidadDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditCriticidad(CriticidadDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var entidad = new Criticidad()
                        {
                            CriticidadId = objeto.Id,
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            DetalleCriticidad = objeto.DetalleCriticidad,
                            PrefijoBase = objeto.PrefijoBase,
                            Prioridad = objeto.Prioridad
                        };
                        ctx.Criticidad.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.CriticidadId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Criticidad
                                       where u.CriticidadId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.DetalleCriticidad = objeto.DetalleCriticidad;
                            entidad.PrefijoBase = objeto.PrefijoBase;
                            entidad.Prioridad = objeto.Prioridad;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.Activo = objeto.Activo;

                            ctx.SaveChanges();

                            return entidad.CriticidadId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: int AddOrEditCriticidad(CriticidadDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: int AddOrEditCriticidad(CriticidadDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Criticidad
                                  where u.CriticidadId == id
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
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CriticidadDTO> GetAllCriticidad()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Criticidad
                                       where u.Activo
                                       select new CriticidadDTO()
                                       {
                                           Id = u.CriticidadId,
                                           DetalleCriticidad = u.DetalleCriticidad,
                                           PrefijoBase = u.PrefijoBase
                                       });
                        return entidad.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: List<CriticidadDTO> GetAllCriticidad()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: List<CriticidadDTO> GetAllCriticidad()"
                    , new object[] { null });
            }
        }

        public override List<CriticidadDTO> GetCriticidad(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Criticidad
                                         where (u.DetalleCriticidad.ToUpper().Contains(filtro.ToUpper())
                                         || u.PrefijoBase.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         orderby u.DetalleCriticidad
                                         select new CriticidadDTO()
                                         {
                                             Id = u.CriticidadId,
                                             DetalleCriticidad = u.DetalleCriticidad,
                                             PrefijoBase = u.PrefijoBase,
                                             Prioridad = u.Prioridad.Value,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion.HasValue ? u.FechaCreacion.Value : DateTime.Now,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: List<CriticidadDTO> GetCriticidad(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: List<CriticidadDTO> GetCriticidad(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetCriticidadByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Criticidad
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro) || (u.DetalleCriticidad).ToUpper().Equals(filtro.ToUpper()))
                                   orderby u.DetalleCriticidad
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.CriticidadId.ToString(),
                                       Descripcion = u.DetalleCriticidad,
                                       value = u.DetalleCriticidad
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

        public override CriticidadDTO GetCriticidadById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Criticidad
                                       where u.CriticidadId == id
                                       select new CriticidadDTO()
                                       {
                                           Id = u.CriticidadId,
                                           DetalleCriticidad = u.DetalleCriticidad,
                                           PrefijoBase = u.PrefijoBase,
                                           Prioridad = u.Prioridad.Value,
                                           Activo = u.Activo,
                                           //FechaCreacion = u.FechaCreacion.HasValue? u.FechaCreacion.Value : DateTime.Now,
                                           UsuarioCreacion = u.UsuarioCreacion
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: CriticidadDTO GetCriticidadById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCriticidadDTO
                    , "Error en el metodo: CriticidadDTO GetCriticidadById(int id)"
                    , new object[] { null });
            }
        }
    }
}
