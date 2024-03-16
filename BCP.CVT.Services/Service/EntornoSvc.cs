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
    public class EntornoSvc : EntornoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditEntorno(EntornoDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Entorno()
                        {
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            EntornoId = objeto.Id
                        };
                        ctx.Entorno.Add(entidad);
                        ctx.SaveChanges();                 

                        return entidad.EntornoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Entorno
                                       where u.EntornoId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.Activo = objeto.Activo;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.EntornoId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: int AddOrEditEntorno(EntornoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: int AddOrEditEntorno(EntornoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Entorno
                                  where u.EntornoId == id
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
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<EntornoDTO> GetEntorno(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Entorno
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))                                    
                                     select new EntornoDTO()
                                     {
                                         Id = u.EntornoId,
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
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: List<EntornoDTO> GetEntorno(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: List<EntornoDTO> GetEntorno(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEntornoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Entorno
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.EntornoId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEntornoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEntornoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override EntornoDTO GetEntornoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Entorno
                                   where u.EntornoId == id
                                   select new EntornoDTO()
                                   {
                                       Id = u.EntornoId,
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
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: EntornoDTO GetEntornoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: EntornoDTO GetEntornoById(int id)"
                    , new object[] { null });
            }
        }
    }
}
