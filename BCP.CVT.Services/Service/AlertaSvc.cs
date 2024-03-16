using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace BCP.CVT.Services.Service
{
    public class AlertaSvc : AlertaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region ALERTAS
        public override List<AlertaDTO> GetAlertasXTipo(int idTipoAlerta, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Alerta
                                     join b in ctx.AlertaDetalle on new { u.AlertaId, FechaConsulta = DbFunctions.TruncateTime(fechaConsulta).Value } equals new { b.AlertaId, FechaConsulta = DbFunctions.TruncateTime(b.FechaCreacion).Value } into lj1
                                     from b in lj1.DefaultIfEmpty()
                                     where u.FlagActivo
                                     && u.TipoAlertaId == idTipoAlerta
                                     group new { u, b } by new { u.AlertaId, u.Descripcion } into grp
                                     //&& ctx.AlertaDetalle.Any(gi => gi.AlertaId == u.AlertaId)
                                     select new AlertaDTO()
                                     {
                                         Id = grp.Key.AlertaId,
                                         Descripcion = grp.Key.Descripcion,
                                         NroAlertasDetalle = grp.Count(x => x.b != null),
                                         NroAlertaCriticas = grp.Count(x => x.b.Criticidad == 1),
                                         NroAlertasNoCriticas = grp.Count(x => x.b.Criticidad == 0)
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasXTipo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasXTipo()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_TecnologiasEstadoPendiente()
        {
            try
            {
                var idsEstados = new List<int>{
                                (int)EstadoTecnologia.Registrado,
                                (int)EstadoTecnologia.EnRevision
                            };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var rpta = (from a in ctx.Alerta
                                where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional1
                                select new AlertaDTO
                                {
                                    Id = a.AlertaId,
                                    Nombre = a.Nombre,
                                    Descripcion = a.Descripcion
                                }).FirstOrDefault();

                    if (rpta != null)
                    {

                        var nroRegistrosTmp = (from u in ctx.Tecnologia
                                            join s in ctx.Subdominio on new { u.SubdominioId, Activo = true } equals new { s.SubdominioId, Activo = s.Activo } into lj1
                                            from s in lj1.DefaultIfEmpty()
                                            join d in ctx.Dominio on new { s.DominioId, Activo = true } equals new { d.DominioId, Activo = d.Activo } into lj2
                                            from d in lj1.DefaultIfEmpty()
                                            where u.Activo
                                            && idsEstados.Contains(u.EstadoTecnologia)
                                            select u.TecnologiaId);
                        var nroRegistros = nroRegistrosTmp.Count();
                        rpta.NroAlertasDetalle = nroRegistros;

                    }


                    return rpta;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TeconologiasEstadoPendiente()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TeconologiasEstadoPendiente()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_TecnologiasSinEquivalencias()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional2
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();

                        if (rpta != null)
                        {

                            var nroRegistros = (from u in ctx.Tecnologia
                                                where u.Activo
                                                && !ctx.TecnologiaEquivalencia.Any(gi => gi.TecnologiaId == u.TecnologiaId)
                                                orderby u.Nombre
                                                select u).Count();

                            rpta.NroAlertasDetalle = nroRegistros;

                        }


                        return rpta;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinEquivalencias()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinEquivalencias()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_EquiposSinSistemaOperativo(DateTime fechaConsulta)
        {
            try
            {                
                var rpta = new AlertaDTO();
                var totalSinSO = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional3
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();                        
                                                
                    }
                }

                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Indicador_TotalEquiposSinSO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fechaConsulta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            totalSinSO = reader.IsDBNull(reader.GetOrdinal("TotalSinSO")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalSinSO"));
                        }
                        reader.Close();
                    }
                }

                rpta.NroAlertasDetalle = totalSinSO;

                return rpta;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinSistemaOperativo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
               HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinSistemaOperativo()"
                    , new object[] { null });
            }

        }

        public override AlertaDTO GetAlertaFuncional_EquiposSinTecnologias(DateTime fechaConsulta)
        {
            try
            {
                var rpta = new AlertaDTO();
                var totalSinTecnologia = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional4
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();                                                
                    }
                }

                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Indicador_TotalEquiposSinTecnologia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fechaConsulta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            totalSinTecnologia = reader.IsDBNull(reader.GetOrdinal("TotalSinTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalSinTecnologia"));
                        }
                        reader.Close();
                    }
                }

                rpta.NroAlertasDetalle = totalSinTecnologia;

                return rpta;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinTecnologias()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinTecnologias()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_EquiposSinRelaciones(string TipoEquipoIdList, int TipoAlertaId)
        {
            try
            {
                var fechaActual = DateTime.Now;
                var rpta = new AlertaDTO();
                var totalSinRelaciones = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        rpta = (from a in ctx.Alerta
                                where a.AlertaId == TipoAlertaId
                                select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();                                                                       
                    }                    
                }

                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Indicador_TotalEquiposSinRelaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fechaActual));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoIdStr", TipoEquipoIdList));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            totalSinRelaciones = reader.IsDBNull(reader.GetOrdinal("TotalSinRelaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalSinRelaciones"));
                        }
                        reader.Close();
                    }
                }

                rpta.NroAlertasDetalle = totalSinRelaciones;

                return rpta;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinRelaciones()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinRelaciones()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_TecnologiasSinFechaSoporte()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional6
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();

                        if (rpta != null)
                        {

                            var nroRegistros = (from u in ctx.Tecnologia
                                                join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId into lj1
                                                from s in lj1.DefaultIfEmpty()
                                                join d in ctx.Dominio on s.DominioId equals d.DominioId into lj2
                                                from d in lj1.DefaultIfEmpty()
                                                join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into lj3
                                                from f in lj3.DefaultIfEmpty()
                                                where u.Activo && s.Activo && d.Activo
                                                && u.FlagFechaFinSoporte == true
                                                && !u.FechaFinSoporte.HasValue
                                                && !u.FechaAcordada.HasValue
                                                && !u.FechaExtendida.HasValue
                                                select u).Count();

                            rpta.NroAlertasDetalle = nroRegistros;

                        }

                        return rpta;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_UrlHuerfana()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional8
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();

                        if (rpta != null)
                        {

                            var nroRegistros = (from u in ctx.UrlAplicacion
                                             join y in ctx.UrlAplicacionEquipo on u.UrlAplicacionId equals y.UrlAplicacionId into lj0
                                             from y in lj0.DefaultIfEmpty()
                                             where u.CodigoAPT == null && u.FlagActivo == true
                                             select u).Count();


                            rpta.NroAlertasDetalle = nroRegistros;

                        }

                        return rpta;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
        }

        public override AlertaDTO GetAlertaFuncional_EquipoNoRegistrado()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var rpta = (from a in ctx.Alerta
                                    where a.AlertaId == (int)EAlertaFuncional.AlertaFuncional9
                                    select new AlertaDTO
                                    {
                                        Id = a.AlertaId,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion
                                    }).FirstOrDefault();

                        if (rpta != null)
                        {
                            var totalRows = 0;                            
                            rpta.NroAlertasDetalle = totalRows;

                        }

                        return rpta;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_TecnologiasSinFechaSoporte()"
                    , new object[] { null });
            }
        }
        public override bool ActualizarAlertaFechaUltimaEjecucion(int id, string usuario)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Alerta
                                   where u.AlertaId == id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.FechaUltimaEjecucion = DateTime.Now;
                        ctx.SaveChanges();
                        estado = true;
                    }
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool ActualizarAlertaFechaUltimaEjecucion(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool ActualizarAlertaFechaUltimaEjecucion(int id, string usuario)"
                    , new object[] { null });
            }
        }
        #endregion


        #region ALERTAS DETALLE

        public override List<AlertaDetalleDTO> GetAlertasDetalle(int idAlerta, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AlertaDetalle
                                     where u.AlertaId == idAlerta
                                     && DbFunctions.TruncateTime(u.FechaCreacion) == DbFunctions.TruncateTime(fechaConsulta).Value
                                     orderby u.FechaCreacion descending
                                     select new AlertaDetalleDTO()
                                     {
                                         Id = u.AlertaDetalleId,
                                         AlertaId = u.AlertaId,
                                         Descripcion = u.Descripcion,
                                         Detalle = u.Detalle,
                                         FechaCreacion = u.FechaCreacion,
                                         Criticidad = u.Criticidad
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
        }

        public override List<AlertaDetalleDTO> GetAlertasTecnicasExportar(DateTime fechaConsulta, string sortName, string sortOrder)
        {
            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AlertaDetalle
                                     join b in ctx.Alerta on u.AlertaId equals b.AlertaId
                                     where DbFunctions.TruncateTime(u.FechaCreacion) == DbFunctions.TruncateTime(fechaConsulta).Value
                                     orderby b.Nombre, u.FechaCreacion descending
                                     select new AlertaDetalleDTO()
                                     {
                                         Id = u.AlertaDetalleId,
                                         AlertaId = u.AlertaId,
                                         Descripcion = u.Descripcion,
                                         Detalle = u.Detalle,
                                         FechaCreacion = u.FechaCreacion,
                                         Alerta = new AlertaDTO
                                         {
                                             Nombre = b.Nombre,
                                             Descripcion = b.Descripcion,
                                         }

                                     }).ToList();

                    registros = registros.OrderBy(sortName + " " + sortOrder).ToList();


                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
        }

        public override void AddorEditAlertaDetalle(AlertaDetalleDTO objRegistro)
        {
            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new AlertaDetalle()
                        {
                            AlertaId = objRegistro.AlertaId,
                            Descripcion = objRegistro.Descripcion,
                            Detalle = objRegistro.Detalle,
                            FechaCreacion = DateTime.Now,
                            CreadoPor = objRegistro.UsuarioCreacion,
                            FlagActivo = objRegistro.Activo,
                            Criticidad = objRegistro.Criticidad
                        };
                        ctx.AlertaDetalle.Add(entidad);
                        ctx.SaveChanges();                        
                    }
                    else
                    {
                        var entidad = (from u in ctx.AlertaDetalle
                                       where u.AlertaDetalleId == objRegistro.Id
                                       select u).FirstOrDefault();

                        entidad.AlertaId = objRegistro.AlertaId;
                        entidad.Descripcion = objRegistro.Descripcion;
                        entidad.Detalle = objRegistro.Detalle;
                        entidad.FlagActivo = objRegistro.Activo;                        
                        ctx.SaveChanges();                        
                    }
                }                
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);                
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
            }
        }

        public override AlertaDetalleDTO GetAlertaDetalle(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AlertaDetalle
                                   where u.AlertaDetalleId == id
                                   select new AlertaDetalleDTO()
                                   {
                                       Id = u.AlertaDetalleId,
                                       AlertaId = u.AlertaId,
                                       Descripcion = u.Descripcion,
                                       Detalle = u.Detalle,
                                       //Activo = u.FlagActivo
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: AlertaDetalleDTO GetAlertaDetalle(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: AlertaDetalleDTO GetAlertaDetalle(int id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoAlertaDetalle(int id, string usuario)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AlertaDetalle
                                   where u.AlertaDetalleId == id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.FlagActivo = false;
                        //entidad.FechaModificacion = DateTime.Now;
                        //entidad.ModificadoPor = usuario;
                        ctx.SaveChanges();
                        estado = true;
                    }
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool CambiarEstadoAlertaDetalle(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool CambiarEstadoAlertaDetalle(int id)"
                    , new object[] { null });
            }
        }

        public override List<AlertaDetalleDTO> GetAlertasDetalleAEnviar(int AlertaId)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AlertaDetalle
                                   where u.AlertaId == AlertaId && u.FlagActivo.Value
                                   select new AlertaDetalleDTO()
                                   {
                                       Id = u.AlertaDetalleId,
                                       Descripcion = u.Descripcion,
                                       Detalle = u.Detalle
                                   });
                    return entidad.ToList();
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: List<AlertaDetalleDTO> GetAlertasDetalleAEnviar(int AlertaId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: List<AlertaDetalleDTO> GetAlertasDetalleAEnviar(int AlertaId)"
                    , new object[] { null });
            }
        }
        #endregion

        #region ALERTAS PROGRAMACION
        public override int AddorEditAlertaProgramacion(AlertaProgramacionDTO objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {


                    switch ((ETipoFrecuencia)objRegistro.FrecuenciaEnvio)
                    {
                        case ETipoFrecuencia.Diaria: objRegistro.NroDias = 1; break;
                        case ETipoFrecuencia.Semanal: objRegistro.NroDias = 7; break;
                        case ETipoFrecuencia.Quincenal: objRegistro.NroDias = 15; break;
                        case ETipoFrecuencia.Mensual: objRegistro.NroDias = 30; break;
                        default:
                            break;
                    }

                    if (objRegistro.Id == 0)
                    {
                        var entidad = new AlertaProgramacion()
                        {
                            AlertaId = objRegistro.AlertaId,
                            FrecuenciaEnvio = objRegistro.FrecuenciaEnvio,
                            NroDias = objRegistro.NroDias,
                            FechaInicio = objRegistro.FechaInicio,
                            HoraEnvio = objRegistro.HoraEnvio,
                            FlagActivo = objRegistro.Activo,
                            Buzones = objRegistro.Buzones,
                            Asunto = objRegistro.Asunto
                        };
                        ctx.AlertaProgramacion.Add(entidad);
                        ctx.SaveChanges();
                        ID = entidad.AlertaProgramacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.AlertaProgramacion
                                       where u.AlertaProgramacionId == objRegistro.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.FrecuenciaEnvio = objRegistro.FrecuenciaEnvio;
                            entidad.NroDias = objRegistro.NroDias;
                            entidad.FechaInicio = objRegistro.FechaInicio;
                            entidad.HoraEnvio = objRegistro.HoraEnvio;
                            entidad.FlagActivo = objRegistro.Activo;
                            entidad.Buzones = objRegistro.Buzones;
                            entidad.Asunto = objRegistro.Asunto;
                            ctx.SaveChanges();
                            ID = entidad.AlertaProgramacionId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: int AddorEditAlertaProgramacion(AlertaProgramacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: int AddorEditAlertaProgramacion(AlertaProgramacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override AlertaProgramacionDTO GetAlertaProgramacion(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AlertaProgramacion
                                   where u.AlertaId == id
                                   select new AlertaProgramacionDTO()
                                   {
                                       Id = u.AlertaProgramacionId,
                                       FrecuenciaEnvio = u.FrecuenciaEnvio,
                                       NroDias = u.NroDias,
                                       FechaInicio = u.FechaInicio,
                                       HoraEnvio = u.HoraEnvio,
                                       FechaUltimoEnvio = u.FechaUltimoEnvio,
                                       ComponenteImpactado = u.ComponenteImpactado,
                                       Activo = u.FlagActivo,
                                       Buzones = u.Buzones,
                                       Asunto = u.Asunto
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: AlertaProgramacionDTO GetAlertaProgramacion(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: AlertaProgramacionDTO GetAlertaProgramacion(int id)"
                    , new object[] { null });
            }
        }

        public override List<AlertaProgramacionDTO> GetAllAlertaProgramacionAEnviar()
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int HORA_ACTUAL = FECHA_ACTUAL.Hour;
                int TIPO_ALERTA_TECNICO = (int)ETipoAlerta.Tecnica;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from ap in ctx.AlertaProgramacion
                                     join a in ctx.Alerta on ap.AlertaId equals a.AlertaId
                                     where ap.FlagActivo && a.FlagActivo
                                     && ap.HoraEnvio == HORA_ACTUAL
                                     && DbFunctions.TruncateTime(FECHA_ACTUAL) >= DbFunctions.TruncateTime(ap.FechaInicio)
                                     && a.TipoAlertaId == TIPO_ALERTA_TECNICO
                                     && DbFunctions.TruncateTime(FECHA_ACTUAL) > DbFunctions.TruncateTime(ap.FechaUltimoEnvio.Value)
                                     select new AlertaProgramacionDTO()
                                     {
                                         Id = ap.AlertaProgramacionId,
                                         AlertaId = a.AlertaId,
                                         FrecuenciaEnvio = ap.FrecuenciaEnvio,
                                         FechaInicio = ap.FechaInicio,
                                         HoraEnvio = ap.HoraEnvio,
                                         FechaUltimoEnvio = ap.FechaUltimoEnvio,
                                         Buzones = ap.Buzones,
                                         Asunto = ap.Asunto,
                                         Alerta = new AlertaDTO()
                                         {
                                             Id = a.AlertaId,
                                             Nombre = a.Nombre,
                                             Descripcion = a.Descripcion,
                                             FechaUltimaEjecucion = a.FechaUltimaEjecucion
                                         }
                                     });
                    return registros.ToList();
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: List<AlertaProgramacionDTO> GetAllAlertaProgramacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: List<AlertaProgramacionDTO> GetAllAlertaProgramacion()"
                    , new object[] { null });
            }
        }

        public override bool ActualizarAlertaProgramacionFechaUltimoEnvio(int id, string usuario)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AlertaProgramacion
                                   where u.AlertaId == id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.FechaUltimoEnvio = DateTime.Now;
                        ctx.SaveChanges();
                        estado = true;
                    }
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool ActualizarAlertaProgramacionFechaUltimoEnvio(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool ActualizarAlertaProgramacionFechaUltimoEnvio(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override IndicadoresDto GetIndicadores()
        {
            try
            {
                //var totalRows = 0;
                var indicadores = new IndicadoresDto();
                var filtroVigente = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FILTRO_VIGENTE_APLICACIONES");
                var filtroGerenciaCentral = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FILTRO_GERENCIA_CENTRAL_APLICACIONES");
                var nroMeses_tecnologiasXVencer = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_TECNOLOGIA_POR_VENCER");
                //var tecnologiasPorVencer = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiasPorVencer(1, int.MaxValue, "Dominio", "asc", out totalRows);

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tipo = ctx.Tipo.FirstOrDefault(x => x.FlagEstandar == true);
                        if (filtroVigente != null && filtroGerenciaCentral != null)
                        {
                            var gerenciaCentral = filtroGerenciaCentral.Valor.Split(';');

                            indicadores.Aplicaciones = (from u in ctx.Aplicacion
                                                        where u.EstadoAplicacion == "Vigente"
                                                        && !gerenciaCentral.Contains(u.GerenciaCentral)
                                                        select u).Count();
                        }
                        else
                        {
                            indicadores.Aplicaciones = (from u in ctx.Aplicacion
                                                        where u.FlagActivo
                                                        select 1).Count();
                        }

                        indicadores.Equipos = (from u in ctx.Equipo
                                               where u.FlagActivo
                                               select 1).Count();
                        if (tipo != null)
                        {
                            indicadores.Tecnologias = (from u in ctx.Tecnologia
                                                       where u.Activo && u.TipoTecnologia == tipo.TipoId
                                                       select 1).Count();
                        }
                        else
                        {
                            indicadores.Tecnologias = (from u in ctx.Tecnologia
                                                       where u.Activo
                                                       select 1).Count();
                        }

                        indicadores.NroMesesTecnologiaPorVencer = nroMeses_tecnologiasXVencer != null ? int.Parse(nroMeses_tecnologiasXVencer.Valor) : 12;
                        //indicadores.TecnologiaDetalle = tecnologiasPorVencer;

                        return indicadores;
                    }
                }                             
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool GetIndicadores()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: bool GetIndicadores()"
                    , new object[] { null });
            }
        }

        #endregion


        public override int AddMensaje(MensajeDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    
                        if (objeto.Id == 0)
                        {
                            var entidad = new Mensaje()
                            {
                                Activo = true,//objeto.Activo,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                NombreUsuarioCreacion = objeto.NombreUsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                Asunto = objeto.Asunto,
                                Descripcion = objeto.Descripcion,
                                TipoMensajeId = objeto.TipoMensajeId,
                                MensajeId = objeto.Id,
                            };
                            ctx.Mensaje.Add(entidad);
                            ctx.SaveChanges();

                            return entidad.MensajeId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.Mensaje
                                           where u.MensajeId == objeto.Id
                                           select u).First();
                            if (entidad != null)
                            {
                                entidad.Asunto = objeto.Asunto;
                                entidad.Descripcion = objeto.Descripcion;
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.Activo = objeto.Activo;
                                entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                                ctx.SaveChanges();

                                return entidad.MensajeId;
                            }
                            else
                                return 0;
                        }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<MensajeDTO> GetMensaje(string matricula, string nombre, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Mensaje
                                     where (u.NombreUsuarioCreacion.ToUpper().Contains(nombre.ToUpper())
                                     || string.IsNullOrEmpty(nombre))
                                     && (u.UsuarioCreacion.ToUpper().Contains(matricula) 
                                     || string.IsNullOrEmpty(matricula))
                                     && (DbFunctions.TruncateTime(u.FechaCreacion) == DbFunctions.TruncateTime(fechaRegistro).Value
                                     || !fechaRegistro.HasValue)
                                     && (tipoId == -1 || u.TipoMensajeId == tipoId)
                                     select new MensajeDTO()
                                     {
                                         Id = u.MensajeId,
                                         NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                         TipoMensajeId = u.TipoMensajeId,
                                         Asunto = u.Asunto,
                                         Descripcion = u.Descripcion,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         FechaUltimaVisita = u.FechaUltimaVisita,
                                         UsuarioUltimaVisita = u.UsuarioUltimaVisita
                                     }).OrderBy(sortName + " " + sortOrder);
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override MensajeDTO GetMensajeById(int Id, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Mensaje
                                   where u.MensajeId == Id
                                   select new MensajeDTO()
                                   {
                                       Id = u.MensajeId,
                                       Activo = u.Activo,
                                       Asunto = u.Asunto,
                                       Descripcion = u.Descripcion,
                                       NombreUsuarioCreacion = u.NombreUsuarioCreacion,
                                       FechaUltimaVisita = u.FechaUltimaVisita,
                                       TipoMensajeId = u.TipoMensajeId,
                                       UsuarioUltimaVisita = u.UsuarioUltimaVisita,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion
                                   }).FirstOrDefault();

                    var entidadActualizar = (from u in ctx.Mensaje
                                             where u.MensajeId == Id
                                             select u).FirstOrDefault();
                    if(entidadActualizar != null)
                    {
                        entidadActualizar.FechaUltimaVisita = DateTime.Now;
                        entidadActualizar.UsuarioUltimaVisita = usuario;
                        ctx.SaveChanges();
                    }

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: MensajeDTO GetMensajeById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: MensajeDTO GetMensajeById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<IndicadorResponsableDto> GetResponsablesIndicadores(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;                
                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<IndicadorResponsableDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerIndicadoresResponsablesPortafolio]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));                        

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IndicadorResponsableDto()
                            {
                                TotalBroker = reader.IsDBNull(reader.GetOrdinal("TotalBroker")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalBroker")),
                                TotalBrokerNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalBrokerNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalBrokerNoAplica")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                TotalExperto = reader.IsDBNull(reader.GetOrdinal("TotalExperto")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalExperto")),
                                TotalExpertoNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalExpertoNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalExpertoNoAplica")),
                                TotalGestor = reader.IsDBNull(reader.GetOrdinal("TotalGestor")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalGestor")),
                                TotalGestorNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalGestorNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalGestorNoAplica")),
                                TotalJde = reader.IsDBNull(reader.GetOrdinal("TotalJde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalJde")),
                                TotalJdeNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalJdeNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalJdeNoAplica")),
                                TotalOwner = reader.IsDBNull(reader.GetOrdinal("TotalOwner")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalOwner")),
                                TotalOwnerNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalOwnerNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalOwnerNoAplica")),
                                TotalTtl = reader.IsDBNull(reader.GetOrdinal("TotalTtl")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTtl")),
                                TotalTtlNoAplica = reader.IsDBNull(reader.GetOrdinal("TotalTtlNoAplica")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTtlNoAplica")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),

                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;
                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionResponsableDto> GetResponsableDetalle()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionResponsableDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerIndicadoresResponsablesPortafolioDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionResponsableDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TTL = reader.IsDBNull(reader.GetOrdinal("Tribe Technical Lead")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tribe Technical Lead")),
                                JdE = reader.IsDBNull(reader.GetOrdinal("Jefe de Equipo/Experto User IT/Product Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Jefe de Equipo/Experto User IT/Product Owner")),
                                Broker = reader.IsDBNull(reader.GetOrdinal("Bróker de Sistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("Bróker de Sistemas")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner/Líder Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner/Líder Usuario")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor/Usuario Autorizador")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor/Usuario Autorizador")),
                                Experto = reader.IsDBNull(reader.GetOrdinal("Experto/Especialista/Lider Técnico")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto/Especialista/Lider Técnico"))

                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    
                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<NotificacionDTO> GetNotificaciones(string para, string asunto, string mesesTrimestre, int? anio, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var listaMeses = new List<string>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(mesesTrimestre))
                        listaMeses = mesesTrimestre.Split('|').ToList();

                    var registros = (from u in ctx.Notificacion
                                     where (string.IsNullOrEmpty(para) || u.Para.ToUpper().Contains(para.ToUpper()))
                                     && (string.IsNullOrEmpty(asunto) || u.Asunto.ToUpper().Contains(asunto.ToUpper()))
                                     && (DbFunctions.TruncateTime(u.FechaCreacion) <= DbFunctions.TruncateTime(fechaRegistro).Value
                                     || !fechaRegistro.HasValue)
                                     && (tipoId == 0 || u.TipoNotificacionId == tipoId)
                                     && (string.IsNullOrEmpty(mesesTrimestre) || !u.FechaEnvio.HasValue || listaMeses.Contains(u.FechaEnvio.Value.Month.ToString()))
                                     && (!anio.HasValue || !u.FechaEnvio.HasValue || u.FechaEnvio.Value.Year == anio.Value)
                                     select new NotificacionDTO()
                                     {
                                         Id = u.NotificacionId,
                                         Asunto = u.Asunto,                                         
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         CC = u.CC,
                                         Cuerpo = u.Cuerpo,
                                         De = u.De,
                                         FechaEnvio = u.FechaEnvio,
                                         FlagEnviado = u.FlagEnviado,
                                         Nombre = u.Nombre,
                                         Para = u.Para,
                                         TipoNotificacionId = u.TipoNotificacionId,
                                         Matricula = u.MatriculaResponsable
                                     }).OrderBy(sortName + " " + sortOrder);
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AlertaDetalleDTO> GetAlertasNoCriticas(DateTime fechaConsulta)
        {
            try
            {
                var fechaFin = fechaConsulta;
                var fechaInicio = fechaConsulta.AddDays(-7);

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AlertaDetalle
                                     where DbFunctions.TruncateTime(u.FechaCreacion) >= DbFunctions.TruncateTime(fechaInicio).Value &&
                                     DbFunctions.TruncateTime(u.FechaCreacion) <= DbFunctions.TruncateTime(fechaFin).Value &&
                                     u.Criticidad != 1
                                     orderby u.FechaCreacion descending
                                     select new AlertaDetalleDTO()
                                     {
                                         Id = u.AlertaDetalleId,
                                         AlertaId = u.AlertaId,
                                         Descripcion = u.Descripcion,
                                         Detalle = u.Detalle,
                                         FechaCreacion = u.FechaCreacion,
                                         Criticidad = u.Criticidad
                                     }).ToList();                   

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
        }

        public override DataRetornoAplicacion GetAplicacionesSinInformacion(string matricula)
        {
            var fecha = DateTime.Now;            

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var retorno = new DataRetornoAplicacion();

                    retorno.Aplicaciones = (from u in ctx.Relacion
                                            join b in ctx.RelacionDetalle on u.RelacionId equals b.RelacionId
                                            join a in ctx.Aplicacion on u.CodigoAPT equals a.CodigoAPT                                     
                                            join e in ctx.AplicacionPortafolioResponsables on u.CodigoAPT equals e.CodigoAPT
                                            join t in ctx.Tecnologia on b.TecnologiaId equals t.TecnologiaId
                                            where e.Matricula == matricula && e.PortafolioResponsableId==(int)EPortafolioResponsable.Experto
                                            && t.Activo && a.FlagActivo && e.FlagActivo && a.EstadoAplicacion != "Eliminada"
                                            && u.DiaRegistro == fecha.Day && u.MesRegistro == fecha.Month && u.AnioRegistro==fecha.Year
                                             group new { u, a, t } by new { u.CodigoAPT, a.Nombre, t.SubdominioId, e.Colaborador} into grp
                                             select new AplicacionTecnologiaDto()
                                             {
                                                 CodigoAPT = grp.Key.CodigoAPT,
                                                 Aplicacion = grp.Key.Nombre,
                                                 Experto = grp.Key.Colaborador,
                                                 LP = grp.Key.SubdominioId == 14 ? "X" : "",
                                                 AppServer = grp.Key.SubdominioId == 32 ? "X" : "",
                                                 WebServer = grp.Key.SubdominioId == 45 ? "X" : "",
                                                 Middleware = grp.Key.SubdominioId == 63 ? "X" : "",
                                                 BD = grp.Key.SubdominioId == 68 || grp.Key.SubdominioId == 69 ? "X" : "",
                                                 SO = grp.Key.SubdominioId == 36 ? "X" : "",
                                             }).ToList();                                 

                    if (retorno.Aplicaciones != null && retorno.Aplicaciones.Count() > 0)
                    {


                        retorno.Responsable = retorno.Aplicaciones[0].Experto;

                        var aplicaciones = (from x in retorno.Aplicaciones
                                            group x by new { x.CodigoAPT, x.Aplicacion } into gp
                                            select new AplicacionTecnologiaDto()
                                            {
                                                CodigoAPT = gp.Key.CodigoAPT,
                                                Aplicacion = gp.Key.Aplicacion,
                                                LP = string.Join("", gp.Select(i => i.LP)),
                                                AppServer = string.Join("", gp.Select(i => i.AppServer)),
                                                Middleware = string.Join("", gp.Select(i => i.Middleware)),
                                                WebServer = string.Join("", gp.Select(i => i.WebServer)),
                                                BD = string.Join("", gp.Select(i => i.BD)),
                                                SO = string.Join("", gp.Select(i => i.SO))
                                            }).ToList();

                        retorno.Total = aplicaciones.Count;
                        retorno.Aplicaciones = aplicaciones;
                    }
                    
                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertasDetalle()"
                    , new object[] { null });
            }
        }

        public override NotificacionExpertosDTO GetNotificacionExpertos(string correo)
        {
            try
            {
                var resultado = new NotificacionExpertosDTO();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var colaborador = (from a in ctx.AplicacionPortafolioResponsables
                                       join b in ctx.PortafolioResponsables on a.PortafolioResponsableId equals b.PortafolioResponsableId
                                       where a.FlagActivo && a.CorreoElectronico.Equals(correo)
                                       select a.Colaborador).Distinct().First();

                    var perfiles = (from a in ctx.AplicacionPortafolioResponsables
                                    join b in ctx.PortafolioResponsables on a.PortafolioResponsableId equals b.PortafolioResponsableId
                                    where a.FlagActivo && a.CorreoElectronico.Equals(correo)
                                    select b.Nombre).Distinct().ToList();

                    resultado.nombre = colaborador.ToString();
                    resultado.correo = correo;
                    resultado.perfiles = String.Join(",", perfiles);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override List<IndicadorResponsableDetalleDto> GetResponsableDetalle(int responsableId, int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IndicadorResponsableDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerIndicadoresResponsablesPortafolio_ByResponsable]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ReponsableId", responsableId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        //comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        //comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IndicadorResponsableDetalleDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Comentario = reader.IsDBNull(reader.GetOrdinal("Comentario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Comentario")),
                                Indicador = reader.IsDBNull(reader.GetOrdinal("IndicadorResponsable")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorResponsable"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;
                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<NotificacionAplicacionDTO> GetNotificacionesPortafolio(string para, string asunto, string mesesTrimestre, int? anio, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var listaMeses = new List<string>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(mesesTrimestre))
                        listaMeses = mesesTrimestre.Split('|').ToList();

                    var registros = (from u in ctx.NotificacionApp
                                         //join x in ctx.Solicitud on u.SolicitudAplicacionId equals x.SolicitudAplicacionId
                                         //join y in ctx.Aplicacion on x.AplicacionId equals y.AplicacionId
                                     where (string.IsNullOrEmpty(para) || u.Para.ToUpper().Contains(para.ToUpper()))
                                     && (string.IsNullOrEmpty(asunto) || u.Asunto.ToUpper().Contains(asunto.ToUpper()))
                                     && (DbFunctions.TruncateTime(u.FechaCreacion) <= DbFunctions.TruncateTime(fechaRegistro).Value
                                     || !fechaRegistro.HasValue)
                                     && (tipoId == 0 || tipoId == -1 || u.TipoNotificacionId == tipoId)
                                     && (string.IsNullOrEmpty(mesesTrimestre) || !u.FechaEnvio.HasValue || listaMeses.Contains(u.FechaEnvio.Value.Month.ToString()))
                                     && (!anio.HasValue || !u.FechaEnvio.HasValue || u.FechaEnvio.Value.Year == anio.Value)
                                     select new NotificacionAplicacionDTO()
                                     {
                                         Id = u.NotificacionId,
                                         //CodigoAPT = y.CodigoAPT,
                                         Asunto = u.Asunto,
                                         Activo = u.Activo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         CC = u.CC,
                                         Cuerpo = u.Cuerpo,
                                         De = u.De,
                                         FechaEnvio = u.FechaEnvio,
                                         FlagEnviado = u.FlagEnviado,
                                         Nombre = u.Nombre,
                                         Para = u.Para,
                                         TipoNotificacionId = u.TipoNotificacionId,
                                         //Matricula = u.MatriculaResponsable
                                     }).OrderBy(sortName + " " + sortOrder);
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
    }
}
