using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;

namespace BCP.CVT.Services.Service.PortafolioAplicaciones
{
	public class ClasificacionTecnicaSvc : ClasificacionTecnicaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<ClasificacionTecnicaDTO> GetClasificacion(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ClasificacionTecnica
                                     where (string.IsNullOrEmpty(pag.nombre) || u.Nombre.ToUpper().Contains(pag.nombre.ToUpper()))
                                     && !u.FlagEliminado.Value
                                     select new ClasificacionTecnicaDTO()
                                     {
                                         Id = u.ClasificacionTecnicaId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetClasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetClasificacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ClasificacionTecnicaDTO GetClasificacionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ClasificacionTecnica
                                   where u.ClasificacionTecnicaId == id
                                   select new ClasificacionTecnicaDTO()
                                   {
                                       Id = u.ClasificacionTecnicaId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Activo = u.FlagActivo,
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
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProcesoVitalDTO GetProcesoVitalById(int id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoClasificacion(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = ctx.ClasificacionTecnica.FirstOrDefault(x => x.ClasificacionTecnicaId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
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

        public override int AddOrEditClasificacion(ClasificacionTecnicaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new ClasificacionTecnica()
                        {
                            ClasificacionTecnicaId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.ClasificacionTecnica.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.ClasificacionTecnicaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ClasificacionTecnica
                                       where u.ClasificacionTecnicaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            ID = entidad.ClasificacionTecnicaId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }


        public override int AddSubClasificacion(SubClasificacionTecnicaDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                  
                        var entidad = new SubClasificacionTecnica()
                        {
                            ClasificacionTecnicaId = objeto.Id,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            FlagActivo = true,
                            FlagEliminado = false,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL
                        };
                        ctx.SubClasificacionTecnica.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.SubClasificacionTecnicaId;
                   

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditProcesoVital(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }


    }
}
