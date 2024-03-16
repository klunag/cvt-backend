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
    public class CodigoCISSvc : CodigoCISDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditCodigoCIS(CodigoCISDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new CodigoCIS()
                        {
                            CodigoTemporal = objeto.CodigoTemporal,
                            Descripcion = objeto.Descripcion,
                            Activo = objeto.Activo,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now                                                                   
                        };
                        ctx.CodigoCIS.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.CodigoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.CodigoCIS
                                       where u.CodigoId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.CodigoTemporal = objeto.CodigoTemporal;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Activo = objeto.Activo;
                            entidad.FechaModificacion = DateTime.Now;                                                     
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.CodigoId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: int AddOrEditCodigoCIS(CodigoCISDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: int AddOrEditCodigoCIS(CodigoCISDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.CodigoCIS
                                  where u.CodigoId == id
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
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CodigoCISDTO> GetCodigoCIS(string username, string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.CodigoCIS
                                     where (u.CodigoTemporal.ToUpper().Contains(filtro.ToUpper())
                                     || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     //&& u.Activo
                                     && u.UsuarioCreacion == (username == null ? u.UsuarioCreacion : username)
                                     select new CodigoCISDTO()
                                     {
                                         Id = u.CodigoId,
                                         CodigoTemporal = u.CodigoTemporal,
                                         Descripcion = u.Descripcion,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion.Value,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         ServidoresAsociados = (from sq in ctx.CodigoCIS_Servidor
                                                                where sq.CodigoId == u.CodigoId
                                                                select 1).Count()
                                     }).OrderBy(sortName + " " + sortOrder);

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<CodigoCISDTO> GetCodigoCIS(string username, string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<CodigoCISDTO> GetCodigoCIS(string username, string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override CodigoCISDTO GetCodigoCISById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.CodigoCIS
                                   where u.CodigoId == id
                                   select new CodigoCISDTO()
                                   {
                                       Id = u.CodigoId,
                                       Activo = u.Activo,
                                       CodigoTemporal = u.CodigoTemporal,
                                       Descripcion = u.Descripcion,                                                                       
                                       FechaCreacion = u.FechaCreacion.Value,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: CodigoCISDTO GetCodigoCISById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: CodigoCISDTO GetCodigoCISById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CodigoCISDTO> GetCodigoCISByUsuario(string nombre, string usuario, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            throw new NotImplementedException();
        }

        public override List<CodigoCISDTO> GetServidoresCIS(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            List<CodigoCISDTO> resultado = new List<CodigoCISDTO>();
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidades = ctx.USP_Cmdb_ListarServidorByCodigoCIS(id).ToList();
                    foreach (var item in entidades)
                    {
                        resultado.Add(new CodigoCISDTO()
                        {
                            Servidor = item.Name,
                            HostnameServidor = item.HostName,
                            Ambiente = item.A_Ambiente
                        });
                    }

                    int ambiente = 0;
                    var listaAmbientes = ServiceManager<AmbienteDAO>.Provider.GetAmbienteByFiltro(null);

                    foreach (var item in resultado)
                    {
                        if (string.IsNullOrWhiteSpace(item.Ambiente))
                            item.Ambiente = "No existe información disponible";
                        else
                        {
                            if (int.TryParse(item.Ambiente, out ambiente))
                            {
                                var estadoRegistro = listaAmbientes.Where(x => int.Parse(x.Id) == ambiente).FirstOrDefault();
                                if (estadoRegistro != null)
                                {
                                    item.Ambiente = estadoRegistro.Descripcion;
                                }
                                else
                                    item.Ambiente = "No existe información disponible";
                            }
                            else
                                item.Ambiente = "No existe información disponible";
                        }
                    }

                    totalRows = resultado.Count();
                    var registros = resultado.OrderBy(sortName + " " + sortOrder);                   
                    var resultadoListado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<CodigoCISDTO> GetServidoresCIS(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<CodigoCISDTO> GetServidoresCIS(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ServidorCISDTO> GetServidoresNoRegistrados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.CodigoCIS
                                     join x in ctx.CodigoCIS_ServidorNoRegistrado on u.CodigoId equals x.CodigoId
                                     where u.CodigoId == id && u.Activo
                                     select new ServidorCISDTO()
                                     {
                                         Id = x.CodigoServidorId,
                                         CodigoId = x.CodigoId,
                                         Servidor = x.Servidor
                                     }).OrderBy(sortName + " " + sortOrder);

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<ServidorCISDTO> GetServidoresNoRegistrados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorCodigoCISDTO
                    , "Error en el metodo: List<ServidorCISDTO> GetServidoresNoRegistrados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ServidorCISDTO> GetServidoresRelacionados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            throw new NotImplementedException();
        }

        public override bool ValidarCodigo(string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
