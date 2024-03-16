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
    public class AuthSvc : AuthDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override AuthAplicacionDTO ObtenerApiKey(string codAplicacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.AuthAplicacion
                                    where u.FlagActivo
                                    && u.CodigoAplicacion == codAplicacion
                                    select new AuthAplicacionDTO
                                    {
                                        AuthAplicacionId = u.AuthAplicacionId,
                                        ApiKey = u.ApiKey,
                                        CodigoAplicacion = u.CodigoAplicacion
                                    }).FirstOrDefault();


                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ObtenerApiKey(string codAplicacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ObtenerApiKey(string codAplicacion)"
                    , new object[] { null });
            }
        }

        public override List<AuthAplicacionDTO> GetAuthAplicacion(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AuthAplicacion
                                     where u.FlagActivo
                                     select new AuthAplicacionDTO()
                                     {
                                         ApiKey = u.ApiKey,
                                         CodigoAplicacion = u.CodigoAplicacion,
                                         AuthAplicacionId = u.AuthAplicacionId,
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
        }
        public override void RegistrarAuthApiKey(AuthAplicacionDTO objRegistro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Set<AuthAplicacion>().Add(new AuthAplicacion
                    {
                        ApiKey = objRegistro.ApiKey,
                        CodigoAplicacion = objRegistro.CodigoAplicacion,
                        FechaCreacion = DateTime.Now,
                        FlagActivo = objRegistro.Activo,
                        UsuarioCreacion = objRegistro.UsuarioCreacion,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = objRegistro.UsuarioCreacion,
                    });
                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ActualizarAuthApiKey(AuthAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ActualizarAuthApiKey(AuthAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override void ActualizarAuthApiKey(AuthAplicacionDTO objRegistro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.AuthAplicacion
                                    where u.AuthAplicacionId == objRegistro.AuthAplicacionId
                                    select u).FirstOrDefault();

                    if(registro != null)
                    {
                        registro.ApiKey = objRegistro.ApiKey;
                        registro.FlagActivo = objRegistro.Activo;
                        registro.FechaModificacion = DateTime.Now;
                        registro.UsuarioModificacion = objRegistro.UsuarioModificacion;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ActualizarAuthApiKey(AuthAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAuthDTO
                    , "Error en el metodo: ActualizarAuthApiKey(AuthAplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool ExisteAuthKeyByCodigoAPT(string codAplicacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.AuthAplicacion
                                    where u.FlagActivo
                                    && u.CodigoAplicacion == codAplicacion
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAuthKeyByCodigoAPT(string codAplicacion, string key)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAuthKeyByCodigoAPT(string codAplicacion, string key)"
                    , new object[] { null });
            }
        }
    }
}
