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
    public class AmbienteSvc : AmbienteDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditAmbiente(AmbienteDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var entidad = new Ambiente()
                        {
                            AmbienteId = objeto.Codigo,
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            DetalleAmbiente = objeto.DetalleAmbiente,
                            PrefijoBase = objeto.PrefijoBase,
                            PrefijoBase2 = objeto.PrefijoBase2
                        };
                        ctx.Ambiente.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.AmbienteId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Ambiente
                                       where u.AmbienteId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.DetalleAmbiente  = objeto.DetalleAmbiente;
                            entidad.PrefijoBase = objeto.PrefijoBase;
                            entidad.PrefijoBase2 = objeto.PrefijoBase2;
                            entidad.FechaModificacion = DateTime.Now;                          
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.Activo = objeto.Activo;

                            ctx.SaveChanges();

                            return entidad.AmbienteId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAmbiente(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int AddOrEditAmbiente(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Ambiente
                                  where u.AmbienteId == id
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
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteCodigoByFiltro(int codigo, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Ambiente
                                    where //u.Activo &&
                                    u.AmbienteId == codigo
                                    && u.AmbienteId != id
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: bool ExisteCodigoByFiltro(int codigo)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: bool ExisteCodigoByFiltro(int codigo)"
                    , new object[] { null });
            }
        }

        public override List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Ambiente
                                     where (u.DetalleAmbiente.ToUpper().Contains(filtro.ToUpper())
                                     || u.PrefijoBase.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     orderby u.DetalleAmbiente
                                     select new AmbienteDTO()
                                     {
                                         Id = u.AmbienteId,
                                         DetalleAmbiente = u.DetalleAmbiente,
                                         PrefijoBase = u.PrefijoBase,
                                         PrefijoBase2 = u.PrefijoBase2,
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

        public override List<AmbienteDTO> GetAmbiente()
        {
            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Ambiente
                                     where u.Activo
                                     select new AmbienteDTO()
                                     {
                                         Id = u.AmbienteId,
                                         DetalleAmbiente = u.DetalleAmbiente                                         
                                     }).ToList();                    

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAmbienteByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Ambiente
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro) || (u.DetalleAmbiente).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.DetalleAmbiente
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.AmbienteId.ToString(),
                                       Descripcion = u.DetalleAmbiente,
                                       value = u.DetalleAmbiente
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAmbienteByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAmbienteByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override AmbienteDTO GetAmbienteById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Ambiente
                                   where u.AmbienteId == id
                                   select new AmbienteDTO()
                                   {
                                       Id = u.AmbienteId,                                      
                                       DetalleAmbiente = u.DetalleAmbiente,
                                       PrefijoBase = u.PrefijoBase,
                                       PrefijoBase2 = u.PrefijoBase2,
                                       Activo = u.Activo,
                                       //FechaCreacion = u.FechaCreacion.HasValue? u.FechaCreacion.Value : DateTime.Now,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       DiaFin = u.DiaFin,
                                       DiaFin_HoraFin = u.DiaFin_HoraFin,
                                       DiaFin_HoraInicio = u.DiaFin_HoraInicio,
                                       DiaInicio = u.DiaInicio,
                                       DiaInicio_HoraFin = u.DiaInicio_HoraFin,
                                       DiaInicio_HoraInicio = u.DiaInicio_HoraInicio
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: AmbienteDTO GetAmbienteById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: AmbienteDTO GetAmbienteById(int id)"
                    , new object[] { null });
            }
        }

        public override List<AmbienteDTO> GetVentana(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var equipos = (from a in ctx.Equipo
                                    where a.FlagActivo && a.TipoEquipoId == 1 //Servidores
                                    group a by a.AmbienteId.Value into grp
                                    select new
                                    {
                                        AmbienteId = grp.Key,
                                        NroEquipos = grp.Count()
                                    }).Distinct();

                    var registros = (from u in ctx.Ambiente
                                     join s in equipos on new { AmbienteId = u.AmbienteId } equals new { AmbienteId = s.AmbienteId } into lj2
                                     from s in lj2.DefaultIfEmpty()
                                     where u.Activo
                                     orderby u.DetalleAmbiente
                                     select new AmbienteDTO()
                                     {
                                         Id = u.AmbienteId,
                                         DetalleAmbiente = u.DetalleAmbiente,
                                         PrefijoBase = u.PrefijoBase,
                                         PrefijoBase2 = u.PrefijoBase2,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion.HasValue ? u.FechaCreacion.Value : DateTime.Now,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         DiaFin = u.DiaFin,
                                         DiaFin_HoraFin = u.DiaFin_HoraFin,
                                         DiaFin_HoraInicio = u.DiaFin_HoraInicio,
                                         DiaInicio = u.DiaInicio,
                                         DiaInicio_HoraFin = u.DiaInicio_HoraFin,
                                         DiaInicio_HoraInicio = u.DiaInicio_HoraInicio,
                                         TotalEquipos = s == null ? 0 : s.NroEquipos
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

        public override int UpdateVentana(AmbienteDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Ambiente
                                   where u.AmbienteId == objeto.Id
                                   select u).First();
                    if (entidad != null)
                    {
                        entidad.DiaInicio = objeto.DiaInicio;
                        entidad.DiaFin = objeto.DiaFin;
                        entidad.DiaFin_HoraFin = objeto.DiaFin_HoraFin;
                        entidad.DiaFin_HoraInicio = objeto.DiaFin_HoraInicio;
                        entidad.DiaInicio_HoraFin = objeto.DiaInicio_HoraFin;
                        entidad.DiaInicio_HoraInicio = objeto.DiaInicio_HoraInicio;

                        entidad.FechaModificacion = DateTime.Now;
                        entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                        ctx.SaveChanges();
                        return entidad.AmbienteId;
                    }
                    else
                        return 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int UpdateVentana(AmbienteDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: int UpdateVentana(AmbienteDTO objeto)"
                    , new object[] { null });
            }
        }
    }
}
