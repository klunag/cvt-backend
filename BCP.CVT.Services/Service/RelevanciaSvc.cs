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
    class RelevanciaSvc : RelevanciaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditRelevancia(RelevanciaDTO objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new Relevancia()
                        {
                            Nombre = objRegistro.Nombre,
                            Peso = objRegistro.Peso,
                            FlagActivo = objRegistro.Activo,
                            CreadoPor = objRegistro.UsuarioCreacion,
                            FechaCreacion = DateTime.Now
                        };
                        ctx.Relevancia.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.RelevanciaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Relevancia
                                       where u.RelevanciaId == objRegistro.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objRegistro.Nombre;
                            entidad.Peso = objRegistro.Peso;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.ModificadoPor = objRegistro.UsuarioModificacion;
                            entidad.FlagActivo = objRegistro.Activo;

                            ctx.SaveChanges();

                            ID = entidad.RelevanciaId;
                        }

                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: int AddOrEditRelevancia(RelevanciaDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: int AddOrEditRelevancia(RelevanciaDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool EditRelevanciaFlagActivo(int id, string usuario)
        {
            try
            {
                bool estado = false;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var itemBD = (from u in ctx.Relevancia
                                      where u.RelevanciaId == id
                                      select u).FirstOrDefault();

                        if (itemBD != null)
                        {
                            itemBD.FechaModificacion = DateTime.Now;
                            itemBD.ModificadoPor = usuario;
                            itemBD.FlagActivo = !itemBD.FlagActivo.Value;

                            ctx.SaveChanges();

                            estado = true;
                        }
                    }
                    scope.Complete();
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: bool EliminarRelevancia(int id, string Usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: bool EliminarRelevancia(int id, string Usuario)"
                    , new object[] { null });
            }
        }

        public override List<RelevanciaDTO> GetAllRelevancia()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Relevancia
                                       where u.FlagActivo.Value
                                       select new RelevanciaDTO()
                                       {
                                           Id = u.RelevanciaId,
                                           Nombre = u.Nombre,
                                           CodigoInterno = u.CodigoInterno.Value,
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: List<RelevanciaDTO> GetAllRelevancia()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: List<RelevanciaDTO> GetAllRelevancia()"
                    , new object[] { null });
            }
        }

        public override List<RelevanciaDTO> GetRelevancia(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Relevancia
                                         where (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre))
                                         //&& u.FlagActivo.Value
                                         select new RelevanciaDTO()
                                         {
                                             Id = u.RelevanciaId,
                                             Nombre = u.Nombre,
                                             Peso = u.Peso,
                                             CodigoInterno = u.CodigoInterno,
                                             Activo = u.FlagActivo.Value,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion.Value,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor
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
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: List<RelevanciaDTO> GetRelevancia(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: List<RelevanciaDTO> GetRelevancia(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override RelevanciaDTO GetRelevanciaById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Relevancia
                                       where u.RelevanciaId == id
                                       select new RelevanciaDTO()
                                       {
                                           Id = u.RelevanciaId,
                                           Nombre = u.Nombre,
                                           Peso = u.Peso,
                                           CodigoInterno = u.CodigoInterno
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: RelevanciaDTO GetRelevanciaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRelevanciaDTO
                    , "Error en el metodo: RelevanciaDTO GetRelevanciaById(int id)"
                    , new object[] { null });
            }
        }
    }
}
