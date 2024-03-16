using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class MepSvc : MepDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditNotaAplicacion(NotaAplicacionDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new NotaAplicacion()
                        {
                            NotaAplicacionId = objeto.Id,
                            CodigoAPT = objeto.CodigoAPT,
                            FlagActivo = true,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioCreacion,
                            FechaModificacion = FECHA_ACTUAL,
                            FechaCreacion = FECHA_ACTUAL,
                            Distribuida = objeto.Distribuida.HasValue ? Utilitarios.TruncateDecimal(objeto.Distribuida.Value, 2) : objeto.Distribuida,
                            MainFrame = objeto.MainFrame.HasValue ? Utilitarios.TruncateDecimal(objeto.MainFrame.Value, 2) : objeto.MainFrame,
                            PaqueteSaas = objeto.PaqueteSaas.HasValue ? Utilitarios.TruncateDecimal(objeto.PaqueteSaas.Value, 2) : objeto.PaqueteSaas,
                            UserITMacro = objeto.UserITMacro.HasValue ? Utilitarios.TruncateDecimal(objeto.UserITMacro.Value, 2) : objeto.UserITMacro,
                            UserITWeb = objeto.UserITWeb.HasValue ? Utilitarios.TruncateDecimal(objeto.UserITWeb.Value, 2) : objeto.UserITWeb,
                            UserITClientSever = objeto.UserITClientSever.HasValue ? Utilitarios.TruncateDecimal(objeto.UserITClientSever.Value, 2) : objeto.UserITClientSever,
                        };
                        ctx.NotaAplicacion.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.NotaAplicacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.NotaAplicacion
                                       where u.NotaAplicacionId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.CodigoAPT = objeto.CodigoAPT;
                            entidad.Distribuida = objeto.Distribuida;
                            entidad.MainFrame = objeto.MainFrame;
                            entidad.PaqueteSaas = objeto.PaqueteSaas;
                            entidad.UserITMacro = objeto.UserITMacro;
                            entidad.UserITWeb = objeto.UserITWeb;
                            entidad.UserITClientSever = objeto.UserITClientSever;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.NotaAplicacionId;
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

        public override NotaAplicacionDTO GetNotaAplicacionById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.NotaAplicacion
                                   where u.NotaAplicacionId == id
                                   select new NotaAplicacionDTO()
                                   {
                                       Id = u.NotaAplicacionId,
                                       CodigoAPT = u.CodigoAPT,
                                       Distribuida = u.Distribuida,
                                       MainFrame = u.MainFrame,
                                       PaqueteSaas = u.PaqueteSaas,
                                       UserITMacro = u.UserITMacro,
                                       UserITWeb = u.UserITWeb,
                                       UserITClientSever = u.UserITClientSever,
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
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<NotaAplicacionDTO> GetListado(PaginacionMep pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.NotaAplicacion
                                     //join p in ctx.AplicacionPortafolioResponsables on u.CodigoAPT equals p.CodigoAPT
                                     join a in ctx.Aplicacion on u.CodigoAPT equals a.CodigoAPT
                                     where (string.IsNullOrEmpty(pag.CodigoAPT) || u.CodigoAPT.ToUpper().Contains(pag.CodigoAPT.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.Tribu) || a.GestionadoPor.ToUpper().Contains(pag.Tribu.ToUpper())) 
                                     && (string.IsNullOrEmpty(pag.Jde) || a.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.Jde.ToUpper())) 
                                     && (string.IsNullOrEmpty(pag.Experto) || a.Experto_Especialista.ToUpper().Contains(pag.Experto.ToUpper())) 
                                     && a.FlagActivo
                                     select new NotaAplicacionDTO()
                                     {
                                         Id = u.NotaAplicacionId,
                                         CodigoAPT = a.CodigoAPT,
                                         NombreAplicacion = a.Nombre,
                                         Distribuida = u.Distribuida,
                                         MainFrame = u.MainFrame,
                                         PaqueteSaas = u.PaqueteSaas,
                                         UserITMacro = u.UserITMacro,
                                         UserITWeb = u.UserITWeb,
                                         UserITClientSever = u.UserITClientSever,
                                         TTL = a.GestionadoPor,
                                         JDE = a.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                         Experto = a.Experto_Especialista,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion
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

        public override bool ExisteCodigoAPT(string codigoAPT, int? id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.NotaAplicacion
                                        where u.CodigoAPT.ToUpper().Equals(codigoAPT.ToUpper())
                                        && (id == null || u.NotaAplicacionId != id.Value)
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string clave, int? id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string clave, int? id)"
                    , new object[] { null });
            }
        }
    }
}
