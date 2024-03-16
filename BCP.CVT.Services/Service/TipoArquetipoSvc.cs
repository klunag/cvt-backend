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

namespace BCP.CVT.Services.Service
{
    public class TipoArquetipoSvc : TipoArquetipoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditTipoArquetipo(TipoArquetipoDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new TipoArquetipo()
                        {
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            TipoArquetipoId = objeto.Id
                        };
                        ctx.TipoArquetipo.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.TipoArquetipoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.TipoArquetipo
                                       where u.TipoArquetipoId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Activo = objeto.Activo;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = DateTime.Now;                           
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.TipoArquetipoId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: int AddOrEditTipoArquetipo(TipoArquetipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: int AddOrEditTipoArquetipo(TipoArquetipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.TipoArquetipo
                                  where u.TipoArquetipoId == id
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
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<TipoArquetipoDTO> GetTipoArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoArquetipo
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     select new TipoArquetipoDTO()
                                     {
                                         Id = u.TipoArquetipoId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion
                                     }).OrderBy(sortName + " " + sortOrder);

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: List<TipoArquetipoDTO> GetTipoArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: List<TipoArquetipoDTO> GetTipoArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTipoArquetipoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoArquetipo
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoArquetipoId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoArquetipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo:  List<CustomAutocomplete> GetTipoArquetipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override TipoArquetipoDTO GetTipoArquetipoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoArquetipo
                                   where u.TipoArquetipoId == id
                                   select new TipoArquetipoDTO()
                                   {
                                       Id = u.TipoArquetipoId,
                                       Activo = u.Activo,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: TipoArquetipoDTO GetTipoArquetipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoArquetipoDTO
                    , "Error en el metodo: TipoArquetipoDTO GetTipoArquetipoById(int id)"
                    , new object[] { null });
            }
        }
    }
}
