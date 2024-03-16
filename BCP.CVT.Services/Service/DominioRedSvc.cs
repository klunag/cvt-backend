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
using System.Linq.Dynamic;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class DominioRedSvc : DominioRedDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditDominioRed(DominioServidorDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new DominioServidor()
                        {
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Equivalencias = objeto.Equivalencias,                        
                            DominioId = objeto.Id
                        };
                        ctx.DominioServidor.Add(entidad);
                        ctx.SaveChanges();
                      
                        return entidad.DominioId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.DominioServidor
                                       where u.DominioId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Equivalencias = objeto.Equivalencias;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.Activo = objeto.Activo;                      
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.DominioId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error al registrar/editar un dominio de red."
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                var itemBD = (from u in ctx.DominioServidor
                              where u.DominioId == id
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

        public override List<DominioServidorDTO> GetDominioRed(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.DominioServidor
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || u.Equivalencias.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     select new DominioServidorDTO()
                                     {
                                         Id = u.DominioId,
                                         Nombre = u.Nombre,
                                         Equivalencias = u.Equivalencias,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion.Value,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion
                                     }).OrderBy(sortName + " " + sortOrder);
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
        }

        public override List<CustomAutocomplete> GetDominioRedActivos()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.DominioServidor
                                       where u.Activo
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.DominioId.ToString(),
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
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDominioRedActivos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDominioRedActivos()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetDominioRedByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.DominioServidor
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Contains(filtro.ToUpper()))
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.DominioId.ToString(),
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
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDominioByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDominioByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override DominioServidorDTO GetDominioRedById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.DominioServidor
                                       where u.DominioId == id
                                       select new DominioServidorDTO()
                                       {
                                           Id = u.DominioId,
                                           Activo = u.Activo,
                                           Nombre = u.Nombre,
                                           Equivalencias = u.Equivalencias,
                                           FechaCreacion = u.FechaCreacion.Value,
                                           UsuarioCreacion = u.UsuarioCreacion
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error al obtener un dominio de red."
                    , new object[] { null });
            }
        }
    }
}
