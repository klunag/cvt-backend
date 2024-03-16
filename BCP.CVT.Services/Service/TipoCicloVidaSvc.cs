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
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class TipoCicloVidaSvc : TipoCicloVidaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditTipoCicloVida(TipoCicloVidaDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var flagDefault = (from u in ctx.TipoCicloVida
                                           where //u.TipoCicloVidaId == objeto.Id &&
                                           u.FlagActivo && !u.FlagEliminado
                                           select u).Any();

                        var entidad = new TipoCicloVida()
                        {
                            TipoCicloVidaId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            NroPeriodosEstadoAmbar = objeto.NroPeriodo,
                            FlagDefault = !flagDefault,
                            FlagActivo = true,
                            //FlagTecnologia = objeto.FlagTecnologia,
                            FlagEliminado = false,
                            CreadoPor = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now
                        };

                        ctx.TipoCicloVida.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.TipoCicloVidaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.TipoCicloVida
                                       where u.TipoCicloVidaId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.NroPeriodosEstadoAmbar = objeto.NroPeriodo;
                            //entidad.FlagTecnologia = objeto.FlagTecnologia;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.TipoCicloVidaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: int AddOrEditTipoCicloVida(TipoCicloVidaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: int AddOrEditTipoCicloVida(TipoCicloVidaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override void CambiarDefault(int id, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.TipoCicloVida
                                  where u.FlagActivo && !u.FlagEliminado
                                  select u);

                    foreach (var item in itemBD)
                    {
                        item.FechaModificacion = DateTime.Now;
                        item.ModificadoPor = usuario;
                        item.FlagDefault = false;
                    }

                    itemBD.FirstOrDefault(x => x.TipoCicloVidaId == id).FlagDefault = true;

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: void CambiarDefault(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: void CambiarDefault(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.TipoCicloVida
                                  where u.TipoCicloVidaId == id
                                  select u).First();

                    if (itemBD != null && !itemBD.FlagDefault)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.ModificadoPor = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }

                    return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.TipoCicloVida
                                         where !u.FlagEliminado
                                         orderby u.Nombre
                                         select new TipoCicloVidaDTO()
                                         {
                                             Id = u.TipoCicloVidaId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             FlagDefault = u.FlagDefault,
                                             NroPeriodo = u.NroPeriodosEstadoAmbar,
                                             FlagTecnologia = u.FlagTecnologia,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion
                                         });

                        totalRows = registros.Count();
                        //registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TipoCicloVidaDTO GetTipoCicloVidaById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.TipoCicloVida
                                       where u.TipoCicloVidaId == id
                                       select new TipoCicloVidaDTO()
                                       {
                                           Id = u.TipoCicloVidaId,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           FlagDefault = u.FlagDefault,
                                           FlagTecnologia = u.FlagTecnologia,
                                           NroPeriodo = u.NroPeriodosEstadoAmbar,
                                           Activo = u.FlagActivo
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: TipoCicloVidaDTO GetTipoCicloVidaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: TipoCicloVidaDTO GetTipoCicloVidaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTipoCicloVidaByFiltro(bool FlagTecnologia)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.TipoCicloVida
                                         where !u.FlagEliminado
                                         orderby u.Nombre
                                         select new CustomAutocomplete()
                                         {
                                             Id = u.TipoCicloVidaId.ToString(),
                                             Descripcion = u.Descripcion,
                                             value = u.Descripcion,
                                             FlagActivo = u.FlagTecnologia
                                         }).ToList();
                        if (FlagTecnologia)
                        {
                            registros = registros.Where(a => a.FlagActivo == FlagTecnologia).ToList();
                        }
                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoCicloVida(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
    }
}
