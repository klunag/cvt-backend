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
    class RoadMapSvc : RoadMapDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEdit(RoadMapDTO objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidadBd = (from u in ctx.RoadMap
                                     where u.Nombre == objRegistro.Nombre
                                     select u).FirstOrDefault();
                    if (entidadBd == null)
                    {
                        var entidad = new RoadMap()
                        {
                            Nombre = objRegistro.Nombre,
                            FlagActivo = objRegistro.Activo,
                            FechaCreacion = DateTime.Now,
                            CreadoPor = objRegistro.UsuarioCreacion,
                            FechaModificacion = DateTime.Now,
                            ModificadoPor = objRegistro.UsuarioCreacion
                        };
                        ctx.RoadMap.Add(entidad);
                        ctx.SaveChanges();
                        ID = entidad.RoadMapId;
                    }
                    else
                    {
                        entidadBd.Color = objRegistro.Color;
                        entidadBd.FechaModificacion = DateTime.Now;
                        entidadBd.ModificadoPor = objRegistro.UsuarioModificacion;
                        ctx.SaveChanges();
                        ID = objRegistro.Id;
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: int AddOrEdit(RoadMapDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: int AddOrEdit(RoadMapDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<RoadMapDTO> GetAllRoadMap()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.RoadMap
                                       where u.FlagActivo
                                       select new RoadMapDTO()
                                       {
                                           Id = u.RoadMapId,
                                           Nombre = u.Nombre
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: GetAllRoadMap()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: GetAllRoadMap()"
                    , new object[] { null });
            }
        }

        public override List<RoadMapDTO> GetRoadMap(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                List<RoadMapDTO> registros = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        registros = (from a1 in ctx.RoadMap
                                     where a1.FlagActivo
                                     && (a1.Nombre.ToUpper().Contains(filtro.ToUpper()) || string.IsNullOrEmpty(filtro))
                                     select new RoadMapDTO()
                                     {
                                         Id = a1.RoadMapId,
                                         Nombre = a1.Nombre,
                                         Color = a1.Color,
                                         UsuarioCreacion = a1.CreadoPor,
                                         FechaCreacion = a1.FechaCreacion,
                                         FechaModificacion = a1.FechaModificacion,
                                         UsuarioModificacion = a1.ModificadoPor
                                     }).OrderBy(sortName + " " + sortOrder).ToList();
                    }
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: List<RoadMapDTO> GetRoadMap(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: List<RoadMapDTO> GetRoadMap(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override RoadMapDTO GetRoadMapById(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.RoadMap
                                       where u.RoadMapId == Id && u.FlagActivo
                                       select new RoadMapDTO()
                                       {
                                           Id = Id,
                                           Nombre = u.Nombre,
                                           Color = u.Color,
                                           FechaModificacion = u.FechaModificacion.HasValue ? u.FechaModificacion : u.FechaCreacion,
                                           UsuarioModificacion = !string.IsNullOrEmpty(u.ModificadoPor) ? u.ModificadoPor : u.CreadoPor
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);                
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: RoadMapDTO GetRoadMapById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorRoadMapDTO
                    , "Error en el metodo: RoadMapDTO GetRoadMapById(int Id)"
                    , new object[] { null });
            }
        }
    }
}
