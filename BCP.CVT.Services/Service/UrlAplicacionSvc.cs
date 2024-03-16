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

namespace BCP.CVT.Services.Service
{
    public class UrlAplicacionSvc: UrlAplicacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool CambiarEstado(int id, string usuario, string comentario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var retorno = false;
                    var CURRENT_DATE = DateTime.Now;

                    var itemBD = ctx.UrlAplicacion.FirstOrDefault(x => x.UrlAplicacionId == id);
                    if (itemBD != null)
                    {
                        itemBD.Comentario = comentario;
                        itemBD.FechaUltimaActualizacion = CURRENT_DATE;
                        itemBD.UsuarioActualizacion = usuario;
                        itemBD.FlagActivo = !itemBD.FlagActivo;

                        var rowsAffected = ctx.SaveChanges();
                        retorno = rowsAffected > 0;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<UrlAplicacionDTO> GetListado(PaginacionUrlApp pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.UrlAplicacion
                                     join y in ctx.UrlAplicacionEquipo on u.UrlAplicacionId equals y.UrlAplicacionId into lj0
                                     from y in lj0.DefaultIfEmpty()
                                     where (string.IsNullOrEmpty(pag.Equipo) || y.NombreEquipo.ToUpper().Contains(pag.Equipo.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.Aplicacion) || u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.URL) || u.Url.ToUpper().Contains(pag.URL.ToUpper()))
                                     && (pag.FuenteId == -1 || u.UrlFuenteId == pag.FuenteId)                                     
                                     && (pag.IsOrphan.HasValue ? pag.IsOrphan.Value ? string.IsNullOrEmpty(u.CodigoAPT) : !string.IsNullOrEmpty(u.CodigoAPT) : true)
                                     && u.FlagActivo == (pag.IsActive == true)
                                     select new UrlAplicacionDTO()
                                     {
                                         Id = u.UrlAplicacionId,
                                         Url = u.Url,
                                         CodigoAPT = u.CodigoAPT,
                                         NombreAplicacion = string.IsNullOrEmpty(u.CodigoAPT) ? string.Empty : (from x in ctx.Aplicacion
                                                                                                                where x.FlagActivo && x.CodigoAPT == u.CodigoAPT
                                                                                                                select x.Nombre).FirstOrDefault(),
                                         //EquipoId = u.EquipoId,
                                         //NombreEquipo = u.NombreEquipo,
                                         UrlFuenteId = u.UrlFuenteId,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaUltimaActualizacion = u.FechaUltimaActualizacion,
                                         FlagEditarAplicacion = u.FlagEditarAplicacion,
                                         Activo = u.FlagActivo.Value,
                                         Comentario = u.Comentario
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder);
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<UrlAplicacionDTO> GetAlertaFuncional_UrlHuerfana_Detalle(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.UrlAplicacion
                                     join y in ctx.UrlAplicacionEquipo on u.UrlAplicacionId equals y.UrlAplicacionId into lj0
                                     from y in lj0.DefaultIfEmpty()
                                     where u.CodigoAPT == null && u.FlagActivo == true
                                     select new UrlAplicacionDTO()
                                     {
                                         Id = u.UrlAplicacionId,
                                         Url = u.Url,
                                         CodigoAPT = u.CodigoAPT,
                                         NombreAplicacion = u.CodigoAPT == null ? string.Empty : (from x in ctx.Aplicacion
                                                                                                                where x.FlagActivo && x.CodigoAPT == u.CodigoAPT
                                                                                                                select x.Nombre).FirstOrDefault(),
                                         UrlFuenteId = u.UrlFuenteId,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaUltimaActualizacion = u.FechaUltimaActualizacion,
                                         FlagEditarAplicacion = u.FlagEditarAplicacion,
                                         Activo = u.FlagActivo.Value,
                                         Comentario = u.Comentario
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<UrlAplicacionDTO> GetListadoAll(PaginacionUrlApp pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.UrlAplicacion
                                     join y in ctx.UrlAplicacionEquipo on u.UrlAplicacionId equals y.UrlAplicacionId into lj0
                                     from y in lj0.DefaultIfEmpty()
                                     where (string.IsNullOrEmpty(pag.Equipo) || y.NombreEquipo.ToUpper().Contains(pag.Equipo.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.Aplicacion) || u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.URL) || u.Url.ToUpper().Contains(pag.URL.ToUpper()))
                                     && (pag.FuenteId == -1 || u.UrlFuenteId == pag.FuenteId)
                                     && (pag.IsOrphan.HasValue ? pag.IsOrphan.Value ? string.IsNullOrEmpty(u.CodigoAPT) : !string.IsNullOrEmpty(u.CodigoAPT) : true)
                                     && u.FlagActivo == (pag.IsActive == true)
                                     select new UrlAplicacionDTO()
                                     {
                                         Id = u.UrlAplicacionId,
                                         Url = u.Url,
                                         CodigoAPT = u.CodigoAPT,
                                         NombreAplicacion = string.IsNullOrEmpty(u.CodigoAPT) ? string.Empty : (from x in ctx.Aplicacion
                                                                                                                where x.FlagActivo && x.CodigoAPT == u.CodigoAPT
                                                                                                                select x.Nombre).FirstOrDefault(),
                                         //EquipoId = u.EquipoId,
                                         //NombreEquipo = u.NombreEquipo,
                                         UrlFuenteId = u.UrlFuenteId,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaUltimaActualizacion = u.FechaUltimaActualizacion,
                                         FlagEditarAplicacion = u.FlagEditarAplicacion,
                                         Activo = u.FlagActivo.Value,
                                         Comentario = u.Comentario
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder);
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<UrlAplicacionDTO> GetListadoAll(PaginacionUrlApp pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<UrlAplicacionDTO> GetListadoAll(PaginacionUrlApp pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionCandidataDTO> GetListAppCandidatasByUrl(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var CURRENT_DATE = DateTime.Now;
                        var day = CURRENT_DATE.Day;
                        var month = CURRENT_DATE.Month;
                        var year = CURRENT_DATE.Year;

                        var Estados = new int[]
                        {
                            (int)EEstadoRelacion.PendienteEliminacion,
                            (int)EEstadoRelacion.Aprobado,
                        };

                        var equiposIds = ctx.UrlAplicacionEquipo.Where(x => x.UrlAplicacionId == id).Select(x => x.EquipoId).ToArray();

                        var itemsBD = (from x in ctx.Relacion
                                       join y in ctx.Aplicacion on x.CodigoAPT equals y.CodigoAPT
                                       where x.DiaRegistro == day && x.MesRegistro == month && x.AnioRegistro == year
                                       && Estados.Contains(x.EstadoId) && x.FlagActivo && y.FlagActivo
                                       && equiposIds.Contains(x.EquipoId)
                                       orderby x.CodigoAPT
                                       select new AplicacionCandidataDTO()
                                       {
                                           UrlAplicacionId = id,
                                           CodigoAPT = x.CodigoAPT,
                                           Nombre = y.Nombre
                                       }).ToList();

                        return itemsBD;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<AplicacionCandidataDTO> GetListAppCandidatasByUrl(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<AplicacionCandidataDTO> GetListAppCandidatasByUrl(int id)"
                    , new object[] { null });
            }
        }

        public override List<UrlAplicacionEquipoDTO> GetUrlAplicacionEquipo(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.UrlAplicacionEquipo 
                                       join u2 in ctx.EquipoConfiguracion on new { EquipoId = u.EquipoId, FlagActivo = true, Componente = "IP" } equals new { EquipoId = (int?)u2.EquipoId, FlagActivo = u2.FlagActivo, Componente = u2.Componente } into IPs
                                       from rt in IPs.DefaultIfEmpty()
                                       join u3 in ctx.Equipo on u.EquipoId equals u3.EquipoId
                                       where u.UrlAplicacionId == id
                                       && u.FlagActivo
                                       orderby u.NombreEquipo
                                       select new UrlAplicacionEquipoDTO()
                                       {
                                           Id = u.UrlAplicacionEquipoId,
                                           EquipoId = u.EquipoId,
                                           NombreEquipo = u.NombreEquipo,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaCreacion = u.FechaCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           IP = rt.DetalleComponente,
                                           Ubicacion=u3.Ubicacion,
                                           FlagTemporal=u3.FlagTemporal
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<UrlAplicacionEquipoDTO> GetUrlAplicacionEquipo(int? id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<UrlAplicacionEquipoDTO> GetUrlAplicacionEquipo(int? id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteUrlFuente> GetUrlFuenteByFiltro(int? id, string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.UrlFuente
                                       where (string.IsNullOrEmpty(nombre) || u.Nombre.ToUpper().Contains(nombre.ToUpper()))
                                       && (!id.HasValue || u.UrlFuenteId == id.Value) && u.FlagActivo==true
                                       orderby u.Nombre
                                       select new CustomAutocompleteUrlFuente()
                                       {
                                           Id = u.UrlFuenteId.ToString(),
                                           Descripcion = u.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocompleteUrlFuente> GetUrlFuenteByFiltro(int? id, string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocompleteUrlFuente> GetUrlFuenteByFiltro(int? id, string nombre)"
                    , new object[] { null });
            }
        }

        public override bool UpdateAplicacionByUrl(int id, string usuario, string codigoAPT)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var retorno = false;
                    var CURRENT_DATE = DateTime.Now;

                    var itemBD = ctx.UrlAplicacion.FirstOrDefault(x => x.UrlAplicacionId == id);
                    if (itemBD != null)
                    {
                        itemBD.FechaUltimaActualizacion = CURRENT_DATE;
                        itemBD.UsuarioActualizacion = usuario;
                        itemBD.CodigoAPT = codigoAPT;

                        var rowsAffected = ctx.SaveChanges();
                        retorno = rowsAffected > 0;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, string usuario)"
                    , new object[] { null });
            }
        }
    }
}
