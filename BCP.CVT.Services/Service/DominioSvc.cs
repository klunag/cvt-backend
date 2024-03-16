using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
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
    public class DominioSvc : DominioDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public override int AddOrEditDominio(DominioDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Dominio()
                        {
                            Activo = objeto.Activo,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            NombreEquipo = objeto.NombreEquipo,
                            Dueno = objeto.MatriculaDueno,
                            CalObs = objeto.CalculoObs,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            DominioId = objeto.Id,
                            ClusterId = 1
                        };
                        ctx.Dominio.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.DominioId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.DominioId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.NombreEquipo = objeto.NombreEquipo;
                            entidad.Dueno = objeto.MatriculaDueno;
                            entidad.CalObs = objeto.CalculoObs;
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
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: int AddOrEditDominio(DominioDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: int AddOrEditDominio(DominioDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Dominio
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
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<DominioDTO> GetDominio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Dominio
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         select new DominioDTO()
                                         {
                                             Id = u.DominioId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             NombreEquipo = u.NombreEquipo,
                                             MatriculaDueno = u.Dueno,
                                             CalculoObs = u.CalObs,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             NumSubdominios = (from sq in ctx.Subdominio
                                                               where sq.DominioId == u.DominioId && sq.Activo
                                                               select 1).Count()
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                        return resultado.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetDominio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetDominio(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
        
        public override List<DominioDTO> GetAllDominio()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.Activo
                                       orderby u.Nombre
                                       select new DominioDTO()
                                       {
                                           Id = u.DominioId,
                                           Nombre = u.Nombre,
                                           MatriculaDueno = u.Dueno,
                                           Descripcion = u.Descripcion,
                                           Activo = u.Activo
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
        }

        public override DominioDTO GetDominioById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.DominioId == id
                                       select new DominioDTO()
                                       {
                                           Activo = u.Activo,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           NombreEquipo = u.NombreEquipo,
                                           MatriculaDueno = u.Dueno,
                                           CalculoObs = u.CalObs,
                                           Id = u.DominioId,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: DominioDTO GetDominioById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: DominioDTO GetDominioById(int id)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioDTO> GetSubdominiosByDominio(int id, int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where (u.DominioId == id)
                                       orderby u.Nombre
                                       select new SubdominioDTO()
                                       {
                                           Activo = u.Activo,
                                           Id = u.SubdominioId,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           UsuarioModificacion = u.UsuarioModificacion,
                                           UsuarioAsociadoPor = u.UsuarioAsociadoPor,
                                           FechaAsociacion = u.FechaAsociacion,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           MatriculaDueno = u.Dueno,
                                           CalculoObs = u.CalObs,
                                       });

                        totalRows = entidad.Count();
                        var resultado = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominiosByDominio(int id, int pageNumber, int pageSize, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominiosByDominio(int id, int pageNumber, int pageSize, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int CambiarDominio(int SubdomId, int DomId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.SubdominioId == SubdomId
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.DominioId = DomId;
                            entidad.UsuarioAsociadoPor = "Usuario Asociacion";
                            entidad.FechaAsociacion = DateTime.Now;

                            ctx.SaveChanges();
                            return entidad.SubdominioId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: int CambiarDominio(int SubdomId, int DomId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: int CambiarDominio(int SubdomId, int DomId)"
                    , new object[] { null });
            }
        }

        public override List<DominioDTO> GetDomConSubdom()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       join s in ctx.Subdominio on u.DominioId equals s.DominioId
                                       where u.Activo && s.Activo
                                       orderby u.Nombre
                                       group u.Nombre by u.DominioId into g
                                       select new DominioDTO()
                                       {
                                           Id = g.Key,
                                           Nombre = g.FirstOrDefault(),
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetDomConSubdom()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetDomConSubdom()"
                    , new object[] { null });
            }
        }

        public override string GetMatriculaDominio(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.Activo && u.DominioId == id
                                       //orderby u.Nombre
                                       select u.Dueno).FirstOrDefault().ToString();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: string GetMatriculaDominio(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: string GetMatriculaDominio(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllDominioByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Contains(filtro.ToUpper()))
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
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllDominioByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllDominioByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<DtoDominio> GetDominios()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Dominio
                                       where u.Activo
                                       orderby u.Nombre
                                       select new DtoDominio()
                                       {
                                           domainId = u.DominioId,
                                           name = u.Nombre,
                                           description = u.Descripcion
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
        }

        public override List<DtoSubdominio> GetSubdominios(int dominio)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.Activo && u.DominioId == dominio
                                       orderby u.Nombre
                                       select new DtoSubdominio()
                                       {
                                           subdomainId = u.SubdominioId,
                                           domainId = u.DominioId,
                                           name = u.Nombre,
                                           description = u.Descripcion
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<DominioDTO> GetAllDominio()"
                    , new object[] { null });
            }
        }
    }
}
