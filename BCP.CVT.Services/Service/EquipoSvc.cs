using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.Appliance;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Cross;
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
    public class EquipoSvc : EquipoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool ExisteEquipoById(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Equipo
                                        where u.FlagActivo && u.EquipoId == Id
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTicketByFiltro(string filtro, int idTipoEquipo)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GetTicketByFiltro]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@filtro", filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? string.Empty : reader.GetString(reader.GetOrdinal("Id")),
                                value = reader.IsDBNull(reader.GetOrdinal("value")) ? string.Empty : reader.GetString(reader.GetOrdinal("value")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("value")) ? string.Empty : reader.GetString(reader.GetOrdinal("value"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                }
                return lista;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetEquipoByFiltro(string filtro, int idTipoEquipo)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Equipo
                                       where (!u.FlagExcluirCalculo.HasValue || !u.FlagExcluirCalculo.Value)
                                       && (idTipoEquipo == 0 || u.TipoEquipoId == idTipoEquipo)
                                       && (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre,
                                           Suscripcion = u.Suscripcion
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipoByFiltro_ComponenteDiagramaInfra(string filtro, string codigoApt)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from e in ctx.Equipo
                                            join r in ctx.Relacion on e.EquipoId equals r.EquipoId
                                            where (string.IsNullOrEmpty(filtro) || e.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                            && (e.TipoEquipoId == 1 || e.TipoEquipoId == 4)
                                            && e.FlagActivo == true
                                            && e.AmbienteId == 1
                                            && r.CodigoAPT == codigoApt
                                            && r.EstadoId == 2
                                            select new CustomAutocomplete()
                                            {
                                                Id = e.EquipoId.ToString(),
                                                Descripcion = e.Nombre,
                                                value = e.Nombre,
                                                Suscripcion = e.Suscripcion
                                            }).Distinct().OrderBy(z => z.value).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro_ComponenteDiagramaInfra(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro_ComponenteDiagramaInfra(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<EquipoAutocomplete> GetEquipoDetalladoByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Equipo
                                       join te in ctx.TipoEquipo on u.TipoEquipoId equals te.TipoEquipoId
                                       where u.FlagActivo
                                       && (u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                       orderby u.Nombre
                                       select new EquipoAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre,
                                           Ambiente = te.Nombre,
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoAutocomplete> GetEquipoDetalladoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoAutocomplete> GetEquipoDetalladoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquiposSinSistemaOperativo(int idSubdominio, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                var listaTipoEquipos = new List<int>() { (int)ETipoEquipo.Servidor, (int)ETipoEquipo.ServidorAgencia };
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var idsEquiposSO = (from a in ctx.EquipoTecnologia
                                            join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                            where b.SubdominioId == idSubdominio
                                            && a.AnioRegistro == fechaConsulta.Year
                                            && a.MesRegistro == fechaConsulta.Month
                                            && a.DiaRegistro == fechaConsulta.Day
                                            select a.EquipoId.Value).Distinct();

                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId
                                         where u.FlagActivo && b.FlagActivo && listaTipoEquipos.Contains(u.TipoEquipoId.Value)
                                          && !idsEquiposSO.Any(gi => gi == u.EquipoId)
                                         orderby u.Nombre
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             TipoEquipo = b.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             FechaUltimoEscaneoCorrectoStr = u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = u.FechaUltimoEscaneoError,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.CreadoPor,
                                             Activo = u.FlagActivo
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinSistemaOperativo(int idSubdominio, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinSistemaOperativo(int idSubdominio, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquiposSinTecnologias(DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var listaTipoEquipos = new List<int>() { (int)ETipoEquipo.Servidor, (int)ETipoEquipo.ServidorAgencia, (int)ETipoEquipo.ServicioNube };
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var idsEquiposSinTecnologias = (from a in ctx.EquipoTecnologia
                                                        join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                        where a.AnioRegistro == fechaConsulta.Year
                                                        && a.MesRegistro == fechaConsulta.Month
                                                        && a.DiaRegistro == fechaConsulta.Day
                                                        select a.EquipoId.Value).Distinct();

                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId

                                         //join s in ctx.EquipoTecnologia on new { EquipoId = u.EquipoId, FechaConsulta = DbFunctions.TruncateTime(fechaConsulta) } equals new { EquipoId = s.EquipoId.Value, FechaConsulta = DbFunctions.CreateDateTime(s.AnioRegistro, s.MesRegistro, s.DiaRegistro, null, null, null) } into lj1
                                         //from s in lj1.DefaultIfEmpty()
                                         //join t in ctx.Tecnologia on new { s.TecnologiaId } equals new { t.TecnologiaId } into lj2
                                         //from t in lj2.DefaultIfEmpty()

                                         where u.FlagActivo && b.FlagActivo && listaTipoEquipos.Contains(u.TipoEquipoId.Value)
                                            && !idsEquiposSinTecnologias.Any(gi => gi == u.EquipoId)
                                         //&& t == null
                                         orderby u.Nombre
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             TipoEquipo = b.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             FechaUltimoEscaneoCorrectoStr = u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = u.FechaUltimoEscaneoError,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.CreadoPor,
                                             Activo = u.FlagActivo
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinTecnologias(DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinTecnologias(DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquiposSinRelaciones(int[] arrTipoEquipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                //var idsTipoEquipo = new List<int>{
                //                Utilitarios.TIPO_EQUIPO_SERVIDOR_ID,
                //                Utilitarios.TIPO_EQUIPO_SERVIDOR_AGENCIA_ID
                //            };
                var idsTipoEquipo = arrTipoEquipo;

                var fechaActual = DateTime.Now;
                var ANIO = fechaActual.Year;
                var MES = fechaActual.Month;
                var DIA = fechaActual.Day;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId
                                         where u.FlagActivo && b.FlagActivo
                                         && idsTipoEquipo.Contains(b.TipoEquipoId)
                                         && !ctx.Relacion.Any(gi => gi.EquipoId == u.EquipoId
                                                                        && gi.AnioRegistro == ANIO
                                                                        && gi.MesRegistro == MES
                                                                        && gi.DiaRegistro == DIA
                                                                        //&& gi.TipoId == (int)ETipoRelacion.Equipo
                                                                        && (gi.EstadoId == (int)EEstadoRelacion.Aprobado || gi.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                         )
                                         orderby u.Nombre
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             TipoEquipo = b.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             Ubicacion = u.Ubicacion,
                                             FechaUltimoEscaneoCorrectoStr = u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = u.FechaUltimoEscaneoError,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.CreadoPor,
                                             Activo = u.FlagActivo,
                                             Suscripcion = u.Suscripcion,
                                             GrupoRecursos = u.GrupoRecursos
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinRelaciones(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquiposSinRelaciones(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<PlotlyDataDTO> GetReportAplicacionesXEquipo(FiltrosDashboardEquipo filtros)
        {
            try
            {
                List<PlotlyDataDTO> plotlyData = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        int DIA = DateTime.Now.Day;
                        int MES = DateTime.Now.Month;
                        int ANIO = DateTime.Now.Year;

                        if (filtros.SubdominioFiltrar == null) filtros.SubdominioFiltrar = new List<int>();
                        if (filtros.DominioFiltrar == null) filtros.DominioFiltrar = new List<int>();

                        var aplicaciones = (from a in ctx.AplicacionConfiguracion
                                            join b in ctx.Aplicacion on a.AplicacionId equals b.AplicacionId
                                            join c in ctx.Relacion on b.CodigoAPT equals c.CodigoAPT
                                            where b.FlagActivo && c.FlagActivo
                                            && a.AnioRegistro == ANIO && a.MesRegistro == MES && a.DiaRegistro == DIA
                                            && c.AnioRegistro == ANIO && c.MesRegistro == MES && a.DiaRegistro == DIA
                                            && c.EquipoId == filtros.EquipoId
                                            && (c.EstadoId == (int)EEstadoRelacion.Aprobado || c.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                            && (c.TipoId == (int)ETipoRelacion.Equipo || c.TipoId == (int)ETipoRelacion.Tecnologia)
                                            select new
                                            {
                                                a.AplicacionId,
                                                b.CodigoAPT,
                                                b.Nombre,
                                                a.IndiceObsolescencia,
                                                a.RiesgoAplicacion,
                                                a.Priorizacion,
                                                a.UbicacionGrafico,
                                                b.EstadoAplicacion,
                                                //Proyeccion 1
                                                a.IndiceObsolescencia_Proyeccion1,
                                                a.RiesgoAplicacion_Proyeccion1,
                                                a.Priorizacion_Proyeccion1,
                                                //Proyeccion 2
                                                a.IndiceObsolescencia_Proyeccion2,
                                                a.RiesgoAplicacion_Proyeccion2,
                                                a.Priorizacion_Proyeccion2,
                                            }).Distinct().ToList();

                        plotlyData = aplicaciones.Select(x =>
                                          new PlotlyDataDTO
                                          {
                                              TipoId = "1",
                                              X = x.RiesgoAplicacion.HasValue ? Math.Round(x.RiesgoAplicacion.Value, 3) : 0,
                                              Y = x.IndiceObsolescencia.HasValue ? Math.Round(x.IndiceObsolescencia.Value, 3) : 0,
                                              Value = x.Priorizacion.HasValue ? Math.Round(x.Priorizacion.Value, 3).ToString() : "0",
                                              Text = x.Priorizacion.HasValue ? Math.Round(x.Priorizacion.Value, 3).ToString() : "",
                                              Descripcion = x.Nombre,
                                              Id = x.CodigoAPT,
                                              FiltroEstado = x.EstadoAplicacion
                                          }).ToList();

                        plotlyData.AddRange(aplicaciones.Select(x =>
                                             new PlotlyDataDTO
                                             {
                                                 TipoId = "2",
                                                 X = x.RiesgoAplicacion_Proyeccion1.HasValue ? Math.Round(x.RiesgoAplicacion_Proyeccion1.Value, 3) : 0,
                                                 Y = x.IndiceObsolescencia_Proyeccion1.HasValue ? Math.Round(x.IndiceObsolescencia_Proyeccion1.Value, 3) : 0,
                                                 Value = x.Priorizacion_Proyeccion1.HasValue ? Math.Round(x.Priorizacion_Proyeccion1.Value, 3).ToString() : "0",
                                                 Text = x.Priorizacion_Proyeccion1.HasValue ? Math.Round(x.Priorizacion_Proyeccion1.Value, 3).ToString() : "",
                                                 Descripcion = x.Nombre,
                                                 Id = x.CodigoAPT,
                                                 FiltroEstado = x.EstadoAplicacion
                                             }).ToList());

                        plotlyData.AddRange(aplicaciones.Select(x =>
                                           new PlotlyDataDTO
                                           {
                                               TipoId = "3",
                                               X = x.RiesgoAplicacion_Proyeccion2.HasValue ? Math.Round(x.RiesgoAplicacion_Proyeccion2.Value, 3) : 0,
                                               Y = x.IndiceObsolescencia_Proyeccion2.HasValue ? Math.Round(x.IndiceObsolescencia_Proyeccion2.Value, 3) : 0,
                                               Value = x.Priorizacion_Proyeccion2.HasValue ? Math.Round(x.Priorizacion_Proyeccion2.Value, 3).ToString() : "0",
                                               Text = x.Priorizacion_Proyeccion2.HasValue ? Math.Round(x.Priorizacion_Proyeccion2.Value, 3).ToString() : "",
                                               Descripcion = x.Nombre,
                                               Id = x.CodigoAPT,
                                               FiltroEstado = x.EstadoAplicacion
                                           }).ToList());

                    }
                    return plotlyData;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<DashboardBase> GetReport(FiltrosDashboardEquipo filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<DashboardBase> GetReport(FiltrosDashboardEquipo filtros)"
                    , new object[] { null });
            }
        }

        public override List<DashboardEquipoBase> GetReportTecnologiasXEquipo(FiltrosDashboardEquipo filtros)
        {
            try
            {
                List<DashboardEquipoBase> dashboardBase = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        int DIA = DateTime.Now.Day;
                        int MES = DateTime.Now.Month;
                        int ANIO = DateTime.Now.Year;
                        DateTime fechaActual = new DateTime(ANIO, MES, DIA);
                        int MESES_CERCA_FIN_SOPORTE = 2; //2 MESES


                        if (filtros.SubdominioFiltrar == null) filtros.SubdominioFiltrar = new List<int>();
                        if (filtros.DominioFiltrar == null) filtros.DominioFiltrar = new List<int>();

                        var tecnologia = (from e in ctx.EquipoTecnologia
                                          join t in ctx.Tecnologia on e.TecnologiaId equals t.TecnologiaId
                                          join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                          join d in ctx.Dominio on s.DominioId equals d.DominioId
                                          where t.Activo && s.Activo && d.Activo && e.FlagActivo
                                          && (e.EquipoId == filtros.EquipoId)
                                          && (filtros.SubdominioFiltrar.Count == 0 || filtros.SubdominioFiltrar.Contains(s.SubdominioId))
                                          && (filtros.DominioFiltrar.Count == 0 || filtros.DominioFiltrar.Contains(d.DominioId))
                                          && e.DiaRegistro == DIA && e.MesRegistro == MES && e.AnioRegistro == ANIO

                                          select t).ToList();

                        List<int> tecnologiaIds = tecnologia.Select(x => x.TecnologiaId).ToList();
                        if (tecnologiaIds == null) tecnologiaIds = new List<int>();

                        var tecnologiaCicloVida = (from tcv in ctx.TecnologiaCicloVida
                                                   where (tecnologiaIds.Count == 0 || tecnologiaIds.Contains(tcv.TecnologiaId))
                                                   && tcv.DiaRegistro == DIA && tcv.MesRegistro == MES && tcv.AnioRegistro == ANIO
                                                   select tcv).ToList();

                        var equipoTecnologia = (from et in ctx.EquipoTecnologia
                                                join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                                where (tecnologiaIds.Count == 0 || tecnologiaIds.Contains(et.TecnologiaId))
                                                && et.FlagActivo && et.DiaRegistro == DIA && et.MesRegistro == MES && et.AnioRegistro == ANIO
                                                && et.EquipoId == filtros.EquipoId
                                                select et).ToList();

                        int idCercaFinSoporte = ((int)EDashboardEstadoTecnologia.CercaFinSoporte);
                        int idNoVigente = ((int)EDashboardEstadoTecnologia.NoVigente);
                        int idVigente = ((int)EDashboardEstadoTecnologia.Vigente);
                        int idSinInformacion = ((int)EDashboardEstadoTecnologia.SinInformacion);

                        string tituloCercaFinSoporte = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologia)idCercaFinSoporte);
                        string tituloNoVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologia)idNoVigente);
                        string tituloVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologia)idVigente);
                        string tituloSinInformacion = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologia)idSinInformacion);

                        string colorCercaFinSoporte = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idCercaFinSoporte);
                        string colorNoVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idNoVigente);
                        string colorVigente = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idVigente);
                        string colorSinInformacion = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)idSinInformacion);

                        if (tecnologiaCicloVida != null && equipoTecnologia != null)
                        {
                            var DataCercaFinSoporte = tecnologiaCicloVida
                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                .Where(x => Utilitarios.GetMonthDifference(fechaActual, x.FechaCalculoBase) <= MESES_CERCA_FIN_SOPORTE && x.Obsoleto == 0 && !x.EsIndefinida)
                                .Select(x => new CustomAutocompleteFecha() { Id = x.TecnologiaId.ToString(), Descripcion = x.ClaveTecnologia, TipoId = idCercaFinSoporte.ToString(), TipoDescripcion = tituloCercaFinSoporte, Fecha = x.FechaCalculoBase, Color = colorCercaFinSoporte, value = equipoTecnologia.Where(y => y.TecnologiaId == x.TecnologiaId).Count().ToString() }).ToList();
                            var DataNoVigente = tecnologiaCicloVida
                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto })
                                .Where(x => x.Obsoleto == 1)
                                .Select(x => new CustomAutocompleteFecha() { Id = x.TecnologiaId.ToString(), Descripcion = x.ClaveTecnologia, TipoId = idNoVigente.ToString(), TipoDescripcion = tituloNoVigente, Fecha = x.FechaCalculoBase, Color = colorNoVigente, value = equipoTecnologia.Where(y => y.TecnologiaId == x.TecnologiaId).Count().ToString() }).ToList();
                            var DataVigente = tecnologiaCicloVida
                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto })
                                .Where(x => Utilitarios.GetMonthDifference(fechaActual, x.FechaCalculoBase) > MESES_CERCA_FIN_SOPORTE && x.Obsoleto == 0)
                                .Select(x => new CustomAutocompleteFecha() { Id = x.TecnologiaId.ToString(), Descripcion = x.ClaveTecnologia, TipoId = idVigente.ToString(), TipoDescripcion = tituloVigente, Fecha = x.FechaCalculoBase, Color = colorVigente, value = equipoTecnologia.Where(y => y.TecnologiaId == x.TecnologiaId).Count().ToString() }).ToList();
                            var DataIndefinida = tecnologiaCicloVida
                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                .Where(x => x.EsIndefinida)
                                .Select(x => new CustomAutocompleteFecha() { Id = x.TecnologiaId.ToString(), Descripcion = x.ClaveTecnologia, TipoId = idVigente.ToString(), TipoDescripcion = tituloVigente, Fecha = x.FechaCalculoBase, Color = colorVigente, value = equipoTecnologia.Where(y => y.TecnologiaId == x.TecnologiaId).Count().ToString() }).ToList();
                            DataVigente.AddRange(DataIndefinida);

                            List<int> tecnologiaCicloVidaTecIds = tecnologiaCicloVida.Select(x => x.TecnologiaId).ToList();
                            var DataSinInformacion = tecnologiaCicloVida
                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto })
                                .Where(x => !tecnologiaCicloVidaTecIds.Contains(x.TecnologiaId))
                                .Select(x => new CustomAutocompleteFecha() { Id = x.TecnologiaId.ToString(), Descripcion = x.ClaveTecnologia, TipoId = idSinInformacion.ToString(), TipoDescripcion = tituloSinInformacion, Fecha = x.FechaCalculoBase, Color = colorSinInformacion, value = equipoTecnologia.Where(y => y.TecnologiaId == x.TecnologiaId).Count().ToString() }).ToList();

                            var datadashboardBase = new List<CustomAutocompleteFecha>();
                            datadashboardBase.AddRange(DataCercaFinSoporte);
                            datadashboardBase.AddRange(DataNoVigente);
                            datadashboardBase.AddRange(DataVigente);
                            datadashboardBase.AddRange(DataSinInformacion);

                            dashboardBase = datadashboardBase.GroupBy(x => new
                            {
                                TipoId = x.TipoId,
                                TipoDescripcion = x.TipoDescripcion,
                                Color = x.Color
                            }).Select(x => new DashboardEquipoBase()
                            {
                                TipoId = x.Key.TipoId,
                                TipoDescripcion = x.Key.TipoDescripcion,
                                Color = x.Key.Color,
                                Data = datadashboardBase.Where(y => y.TipoId == x.Key.TipoId).OrderBy(o => int.Parse(o.value)).Select(p => p).ToList()
                            }).ToList();
                        }
                    }
                    return dashboardBase;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteFecha> GetTecnologiasNoRegistradasXEquipo(FiltrosDashboardEquipo filtros)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tecnologiaTmp = (from e in ctx.TecnologiaNoRegistrada
                                             where e.Activo
                                             && (e.EquipoId == filtros.EquipoId
                                             && e.FlagAsociado == false
                                             //|| e.Equipo.ToUpper() == filtros.EquipoNombre.ToUpper()
                                             )
                                             select new
                                             {
                                                 e.Aplicacion,
                                                 e.FechaFinSoporte,
                                                 Estado = "Por calcular", //TODO

                                             });

                        var tecnologia = tecnologiaTmp.ToList().Select(x => new CustomAutocompleteFecha
                        {
                            Descripcion = x.Aplicacion,
                            Fecha = x.FechaFinSoporte,
                            TipoDescripcion = x.Estado
                        }).ToList();

                        return tecnologia;
                    }

                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: GetTecnologiasNoRegistradasXEquipo(FiltrosDashboardEquipo filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: GetTecnologiasNoRegistradasXEquipo(FiltrosDashboardEquipo filtros)"
                    , new object[] { null });
            }
        }

        public override FiltrosDashboardEquipo ListFiltrosDashboard()
        {
            try
            {
                FiltrosDashboardEquipo arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosDashboardEquipo();

                    arr_data.TipoEquipo = (from te in ctx.TipoEquipo
                                           where te.FlagActivo
                                           //orderby te.Nombre
                                           select new CustomAutocomplete()
                                           {
                                               Id = te.TipoEquipoId.ToString(),
                                               Descripcion = te.Nombre
                                           }).OrderBy(z => z.Descripcion).ToList();
                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: FiltrosDashboardEquipo ListFiltrosDashboard()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: FiltrosDashboardEquipo ListFiltrosDashboard()"
                    , new object[] { null });
            }
        }

        public override FiltrosDashboardEquipo ListFiltrosDashboardByEquipo(int idEquipo)
        {
            try
            {
                FiltrosDashboardEquipo arr_data = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        int DIA = DateTime.Now.Day;
                        int MES = DateTime.Now.Month;
                        int ANIO = DateTime.Now.Year;

                        arr_data = new FiltrosDashboardEquipo();


                        arr_data.Dominio = (from et in ctx.EquipoTecnologia
                                            join t in ctx.Tecnologia on et.TecnologiaId equals t.TecnologiaId
                                            join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                            join d in ctx.Dominio on s.DominioId equals d.DominioId
                                            where t.Activo && s.Activo && d.Activo && et.FlagActivo
                                            && et.DiaRegistro == DIA && et.MesRegistro == MES && et.AnioRegistro == ANIO
                                            && et.EquipoId == idEquipo
                                            //&& t.EstadoId == (int)EstadoTecnologia.Aprobado
                                            //group d by new { d.DominioId, d.Nombre } into grp
                                            select new CustomAutocomplete()
                                            {
                                                Id = d.DominioId.ToString(),
                                                Descripcion = d.Nombre
                                            }).Distinct().OrderBy(z => z.Descripcion).ToList();

                        arr_data.Subdominio = (from et in ctx.EquipoTecnologia
                                               join t in ctx.Tecnologia on et.TecnologiaId equals t.TecnologiaId
                                               join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                               join d in ctx.Dominio on s.DominioId equals d.DominioId
                                               where t.Activo && s.Activo && et.FlagActivo
                                               && et.DiaRegistro == DIA && et.MesRegistro == MES && et.AnioRegistro == ANIO
                                                && et.EquipoId == idEquipo
                                               //&& t.EstadoId == (int)EstadoTecnologia.Aprobado
                                               //orderby s.Nombre
                                               //group s by new { s.SubdominioId, s.Nombre } into grp
                                               select new CustomAutocomplete()
                                               {
                                                   Id = s.SubdominioId.ToString(),
                                                   Descripcion = s.Nombre,
                                                   TipoId = s.DominioId.ToString(),
                                                   TipoDescripcion = d.Nombre
                                               }).Distinct().OrderBy(z => z.Descripcion).ToList();

                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: FiltrosDashboardEquipo ListFiltrosDashboard()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: FiltrosDashboardEquipo ListFiltrosDashboard()"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int desId, int subsiId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                nombre = string.IsNullOrEmpty(nombre) ? string.Empty : nombre;
                so = string.IsNullOrEmpty(so) ? string.Empty : so;

                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquiposAdministradores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", nombre));
                        comando.Parameters.Add(new SqlParameter("@so", so));
                        comando.Parameters.Add(new SqlParameter("@ambiente", ambiente));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", subdominioSO));
                        comando.Parameters.Add(new SqlParameter("@desId", desId));
                        comando.Parameters.Add(new SqlParameter("@subsiId", subsiId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                FechaUltimoEscaneoCorrectoStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FechaUltimoEscaneoErrorStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoError")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoError")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CaracteristicaEquipo")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                TecnologiasInstaladas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TipoEquipoDTO> GetTipoEquipos()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoEquipo
                                     orderby u.Nombre
                                     select new TipoEquipoDTO()
                                     {
                                         Id = u.TipoEquipoId,
                                         Nombre = u.Nombre,
                                         TipoEquipoId = u.TipoEquipoId,
                                         Activo = u.FlagActivo
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<TipoEquipoDTO> GetTipoEquipos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<TipoEquipoDTO> GetTipoEquipos()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoEquipo
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || (u.Nombre).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoEquipoId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetPCIByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoPCI
                                   where u.FlagActivo == true

                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoPCIId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
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
        public override List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, List<int> tipoEquipoIds, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        //Sistema Operativo                    
                        var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

                        var equiposSO = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         where b.SubdominioId == idSubdominio
                                         && a.AnioRegistro == fechaConsulta.Year
                                         && a.MesRegistro == fechaConsulta.Month
                                         && a.DiaRegistro == fechaConsulta.Day
                                         select new
                                         {
                                             EquipoId = a.EquipoId,
                                             SistemaOperativo = b.ClaveTecnologia
                                         }).Distinct();

                        var solicitudesAprobadas = (from a in ctx.EquipoSolicitud
                                                    where a.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoCVT
                                                    && a.TipoEquipoActual == (int)ETipoEquipo.Servidor
                                                    select new
                                                    {
                                                        EquipoId = a.EquipoId,
                                                        EquipoSolicitudId = a.EquipoSolicitudId
                                                    }).Distinct();

                        var registros = (from u in ctx.Equipo
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join d in equiposSO on u.EquipoId equals d.EquipoId into lj1
                                         from d in lj1.DefaultIfEmpty()
                                         join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         join es in solicitudesAprobadas on u.EquipoId equals es.EquipoId into lj3
                                         from es in lj3.DefaultIfEmpty()
                                         where (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                         && (flagActivo == -1 || u.FlagActivo == (flagActivo == 1))
                                         && a.Activo && t.FlagActivo
                                         && (tipoEquipoId == -1 || u.TipoEquipoId.Value == tipoEquipoId)
                                         && (desId == -1 || u.FlagTemporal.Value == (desId == 1 || desId == 2))
                                         && (desId == -1 || desId == 0 || (desId == 1 && u.Ubicacion == null) || (desId == 2 && u.Ubicacion != null))
                                         && (exCalculoId == -1 || u.FlagExcluirCalculo == (exCalculoId == 1))
                                         && (subsidiariaId == null || subsidiariaId == -1 || s.DominioId == subsidiariaId)
                                         && (tipoEquipoIds.Count == 0 || tipoEquipoIds.Contains(u.TipoEquipoId.Value)
                                                || ((tipoEquipoId == (int)ETipoEquipo.Servidor || tipoEquipoId == (int)ETipoEquipo.PC || tipoEquipoId == (int)ETipoEquipo.ServidorAgencia))
                                                && u.CaracteristicaEquipo == (int)ECaracteristicaEquipo.Fisico)
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             Subsidiaria = s.Nombre,
                                             DominioServidorId = s.DominioId,
                                             SistemaOperativo = d.SistemaOperativo,
                                             FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                             FlagCambioDominioRed = u.FlagCambioDominioRed,
                                             FechaCambioEstado = u.FechaCambioEstado,
                                             UsuarioCambioEstado = u.UsuarioCambioEstado,
                                             Ubicacion = u.Ubicacion,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             EquipoSolicitudId = es.EquipoSolicitudId
                    }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditEquipo(EquipoDTO objeto)
        {
            DbContextTransaction transaction = null;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.Id == 0)
                        {
                            DateTime FECHA_ACTUAL = DateTime.Now;
                            var entidad = new Equipo()
                            {
                                FlagActivo = true,//objeto.Activo,
                                CreadoPor = objeto.UsuarioCreacion,
                                FechaCreacion = FECHA_ACTUAL,
                                Nombre = objeto.Nombre,
                                AmbienteId = objeto.AmbienteId,
                                DominioServidorId = objeto.DominioServidorId,
                                EquipoId = objeto.Id,
                                FlagCambioDominioRed = objeto.FlagCambioDominioRed,
                                ProcedenciaId = Guid.NewGuid().ToString(),
                                TipoEquipoId = objeto.TipoEquipoId,
                                CaracteristicaEquipo = objeto.CaracteristicaEquipo,
                                TablaProcedenciaId = 5, //objeto.TablaProcedenciaId
                                FlagExcluirCalculo = objeto.FlagExcluirCalculo,
                                FlagServidorServicio = objeto.FlagServidorServicio,
                                FlagTemporal = true
                            };
                            ctx.Equipo.Add(entidad);
                            ctx.SaveChanges();

                            //Add SO
                            if (objeto.TipoEquipoId != (int)ETipoEquipo.Storage)
                            {
                                if (objeto.TecnologiaId != 0)
                                {
                                    var entidadET = new EquipoTecnologia()
                                    {
                                        //ServidorTecnologiaId = objeto.Id,
                                        EquipoId = entidad.EquipoId,
                                        TecnologiaId = objeto.TecnologiaId,
                                        FlagActivo = true,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = FECHA_ACTUAL,
                                        DiaRegistro = FECHA_ACTUAL.Day,
                                        MesRegistro = FECHA_ACTUAL.Month,
                                        AnioRegistro = FECHA_ACTUAL.Year
                                    };
                                    ctx.EquipoTecnologia.Add(entidadET);
                                    ctx.SaveChanges();
                                }
                            }

                            transaction.Commit();

                            return entidad.EquipoId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.Equipo
                                           where u.EquipoId == objeto.Id
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                if (entidad.TipoEquipoId != (int)ETipoEquipo.Appliance)
                                {
                                    if (entidad.FlagTemporal.HasValue)
                                    {
                                        if (entidad.FlagTemporal.Value)
                                        {
                                            entidad.AmbienteId = objeto.AmbienteId;
                                            entidad.DominioServidorId = objeto.DominioServidorId;
                                        }
                                    }
                                }

                                if (objeto.PerfilId == (int)EPerfilBCP.Administrador)
                                {
                                    entidad.AmbienteId = objeto.AmbienteId;
                                    entidad.DominioServidorId = objeto.DominioServidorId;
                                }

                                entidad.TipoEquipoId = objeto.TipoEquipoId ?? entidad.TipoEquipoId;

                                entidad.FlagExcluirCalculo = objeto.FlagExcluirCalculo;
                                entidad.FlagServidorServicio = objeto.FlagServidorServicio;
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.ModificadoPor = objeto.UsuarioModificacion;
                                entidad.FlagCambioDominioRed = objeto.FlagCambioDominioRed;

                                if (entidad.FlagCambioDominioRed.HasValue)
                                {
                                    if (entidad.FlagCambioDominioRed.Value)
                                    {
                                        entidad.FechaCambioDominioRed = DateTime.Now;
                                        entidad.UsuarioCambioDominioRed = objeto.UsuarioModificacion;
                                    }
                                }

                                if (entidad.FlagExcluirCalculo.HasValue)
                                {
                                    if (entidad.FlagExcluirCalculo.Value)
                                    {
                                        var objHistorico = (from u in ctx.HistoricoExclusion
                                                            where u.EquipoId == entidad.EquipoId && u.FlagActivo
                                                            select u).FirstOrDefault();

                                        if (objHistorico != null)
                                        {
                                            objHistorico.TipoExclusionId = objeto.TipoExclusionId;
                                            objHistorico.MotivoExclusion = objeto.MotivoExclusion;
                                            objHistorico.UsuarioModificacion = objeto.UsuarioModificacion;
                                            objHistorico.FechaModificacion = DateTime.Now;
                                        }
                                        else
                                        {
                                            var historicoExclusion = new HistoricoExclusion()
                                            {
                                                EquipoId = entidad.EquipoId,
                                                TipoExclusionId = objeto.TipoExclusionId,
                                                MotivoExclusion = objeto.MotivoExclusion,
                                                FlagActivo = true,
                                                UsuarioCreacion = objeto.UsuarioCreacion,
                                                FechaCreacion = DateTime.Now
                                            };
                                            ctx.HistoricoExclusion.Add(historicoExclusion);
                                        }
                                    }
                                }

                                if (objeto.FlagExcluirCalculo != null && !objeto.FlagExcluirCalculo.Value)
                                {
                                    var excluidos = ctx.HistoricoExclusion.Where(x => x.EquipoId == objeto.Id && x.FlagActivo).ToList();
                                    foreach (var item in excluidos)
                                    {
                                        item.FlagActivo = false;
                                        item.FechaModificacion = DateTime.Now;
                                        item.UsuarioModificacion = objeto.UsuarioModificacion;
                                    }
                                }

                                ctx.SaveChanges();

                                transaction.Commit();

                                return entidad.EquipoId;
                            }
                            else
                                return 0;
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override EquipoDTO GetEquipoById(int id)
        {
            try
            {
                var eSB = new EquipoSoftwareBaseDTO();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Equipo
                                   join h1 in ctx.HistoricoExclusion on u.EquipoId equals h1.EquipoId into lj1
                                   from h1 in lj1.DefaultIfEmpty()
                                   where u.EquipoId == id
                                   select new EquipoDTO()
                                   {
                                       Id = u.EquipoId,
                                       Activo = u.FlagActivo,
                                       AmbienteId = u.AmbienteId,
                                       TipoEquipoId = u.TipoEquipoId,
                                       FlagExcluirCalculo = u.FlagExcluirCalculo,
                                       Nombre = u.Nombre,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.CreadoPor,
                                       TipoExclusionId = (h1 != null ? h1.TipoExclusionId : -1),
                                       MotivoExclusion = (h1 != null ? h1.MotivoExclusion : ""),
                                       DominioServidorId = u.DominioServidorId,
                                       FlagCambioDominioRed = u.FlagCambioDominioRed,
                                       FlagServidorServicio = u.FlagServidorServicio,
                                       FlagTemporal = u.FlagTemporal,
                                       Ubicacion = u.Ubicacion
                                   }).FirstOrDefault();

                    if (entidad != null)
                    {
                        var entidadSB = ctx.EquipoSoftwareBase.Where(x => x.EquipoId == entidad.Id && x.FlagActivo).FirstOrDefault();
                        if (entidadSB != null)
                        {
                            //var dominioId = ctx.Subdominio.Where(x => x.SubdominioId == entidadSB.SubdominioId && x.Activo).FirstOrDefault().DominioId;
                            eSB = new EquipoSoftwareBaseDTO()
                            {
                                Id = entidadSB.EquipoSoftwareBaseId,
                                EquipoId = entidadSB.EquipoId,
                                //DominioId = dominioId,
                                SubdominioId = entidadSB.SubdominioId,
                                SoftwareBase = entidadSB.SoftwareBase,
                                FechaLanzamiento = entidadSB.FechaLanzamiento,
                                FechaFinSoporte = entidadSB.FechaFinSoporte,
                                FechaFinExtendida = entidadSB.FechaFinExtendida,
                                FechaFinInterna = entidadSB.FechaFinInterna,
                                ComentariosFechaFin = entidadSB.ComentariosFechaFin,
                                FechaCalculoId = entidadSB.FechaCalculoId,
                                //Tab Generales
                                TipoActivo = entidadSB.TipoActivo,
                                Serial = entidadSB.Serial,
                                Modelo = entidadSB.Modelo,
                                Vendor = entidadSB.Vendor,
                                Tecnologia = entidadSB.Tecnologia,
                                Version = entidadSB.Version,
                                Hostname = entidadSB.Hostname,
                                IP = entidadSB.IP,
                                Dimension = entidadSB.Dimension,
                                ArquitectoSeguridad = entidadSB.ArquitectoSeguridad,
                                SoportePrimerNivel = entidadSB.SoportePrimerNivel,
                                Proveedor = entidadSB.Proveedor,
                                UrlNube = entidadSB.UrlNube,
                                Criticidad = entidadSB.Criticidad,
                                //Tab Vigencia
                                FechaAdquisicion = entidadSB.FechaAdquisicion,
                                FechaImplementacion = entidadSB.FechaImplementacion,
                                FechaUltimaRenovacion = entidadSB.FechaUltimaRenovacion,
                                VencimientoLicencia = entidadSB.VencimientoLicencia,
                                CantidadLicencia = entidadSB.CantidadLicencia,
                                FormaLicenciamiento = entidadSB.FormaLicenciamiento,
                                //Tab Inventario
                                CodigoInventario = entidadSB.CodigoInventario,
                                CyberSOC = entidadSB.CyberSOC,
                                Sede = entidadSB.Sede,
                                Sala = entidadSB.Sala,
                                RACK = entidadSB.RACK,
                                Ubicacion = entidadSB.Ubicacion,
                                //Tab Configuracion
                                Backup = entidadSB.Backup,
                                BackupFrecuencia = entidadSB.BackupFrecuencia,
                                BackupDescripcion = entidadSB.BackupDescripcion,
                                IntegracionGestorInteligencia = entidadSB.IntegracionGestorInteligencia,
                                ConectorSIEM = entidadSB.ConectorSIEM,
                                CONA = entidadSB.CONA,
                                UmbralCPU = entidadSB.UmbralCPU,
                                UmbralMemoria = entidadSB.UmbralMemoria,
                                UmbralDisco = entidadSB.UmbralDisco,
                                Ventana = entidadSB.Ventana,
                                EquipoDetalle = entidadSB.EquipoDetalle,
                                EstadoIntegracionSIEM = entidadSB.EstadoIntegracionSIEM,
                                CONAAvanzado = entidadSB.CONAAvanzado,
                                EstadoAppliance = entidadSB.EstadoAppliance,
                                CodigoAPT = entidadSB.CodigoAPT,
                                FlagAprobado = entidadSB.FlagAprobado,
                                RelacionId = entidad.RelacionId,
                                NombreArchivo = entidadSB.NombreArchivo,
                                ArchivoConstancia = entidadSB.ArchivoConstancia
                            };
                            entidad.EquipoSoftwareBase = eSB;
                        }
                        else
                        {
                            if (entidad.TipoEquipoId == (int)ETipoEquipo.Storage)
                            {
                                eSB = new EquipoSoftwareBaseDTO()
                                {
                                    DominioId = 9,
                                    SubdominioId = 42
                                };

                                entidad.EquipoSoftwareBase = eSB;
                            }
                        }
                    }

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoDTO GetEquipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoDTO GetEquipoById(int id)"
                    , new object[] { null });
            }
        }

        public override EquipoDTO GetEquipoSoftwareBaseById(int id)
        {
            try
            {
                var eSB = new EquipoDTO();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = ctx.Equipo.Where(x => x.EquipoId == id && x.FlagActivo).FirstOrDefault();
                    if (entidad != null)
                    {
                        eSB.EquipoId = entidad.EquipoId;
                        eSB.Id = entidad.EquipoId;
                        eSB.Nombre = entidad.Nombre;
                    }

                    return eSB;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoDTO GetEquipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoDTO GetEquipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetSOTecnologias(string filtro)
        {
            try
            {
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var entidad = (from u in ctx.Tecnologia
                                           //join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       where u.Activo //&& s.Activo
                                       && (string.IsNullOrEmpty(filtro) || (u.ClaveTecnologia).ToUpper().Contains(filtro.ToUpper()))
                                       && u.SubdominioId == idSubdominio
                                       orderby u.ClaveTecnologia
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override int AsignarSOTecnologias(EquipoTecnologiaDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //ctx.Database.CommandTimeout = 0;
                    if (objeto.Id == -1)
                    {
                        DateTime FECHA_ACTUAL = DateTime.Now;
                        var entidad = new EquipoTecnologia()
                        {
                            ServidorTecnologiaId = objeto.Id,
                            EquipoId = objeto.EquipoId,
                            TecnologiaId = objeto.TecnologiaId,
                            FlagActivo = true,
                            CreadoPor = objeto.UsuarioCreacion,
                            FechaCreacion = FECHA_ACTUAL,
                            DiaRegistro = FECHA_ACTUAL.Day,
                            MesRegistro = FECHA_ACTUAL.Month,
                            AnioRegistro = FECHA_ACTUAL.Year

                        };
                        ctx.EquipoTecnologia.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.ServidorTecnologiaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.EquipoTecnologia
                                       where u.ServidorTecnologiaId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {
                            entidad.TecnologiaId = objeto.TecnologiaId;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.ServidorTecnologiaId;
                        }
                        else
                            return -1;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error al registrar/editar EquipoTecnologia."
                    , new object[] { null });
            }
        }

        public override EquipoDTO GetEquipoDetalleById(int id)
        {
            var idSubdominio = 0;
            var ListIps = string.Empty;
            var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
            idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

            try
            {
                bool hasClientSecret = false;
                var cadenaConexion = Constantes.CadenaConexion;
                var objeto = new EquipoDTO();
                var ojbClientSecret = new EquipoClientSecretDTO();
                int tipoEquipoId_ClientSecrets = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CLIENT_SECRETS_APLICACION_PARAMETRO_TIPOEQUIPO").Valor); ;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquipoPorId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", idSubdominio));
                        comando.Parameters.Add(new SqlParameter("@equipoId", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {

                            var ip = reader.IsDBNull(reader.GetOrdinal("DetalleComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleComponente"));
                            ListIps = string.Format("{0}{1}", ListIps.Length > 0 ? ListIps + " - " : "", ip == string.Empty ? "" : ip);

                            objeto = new EquipoDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                FechaUltimoEscaneoCorrectoStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FechaUltimoEscaneoErrorStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoError")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoError")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                AmbienteId = reader.IsDBNull(reader.GetOrdinal("AmbienteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AmbienteId")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CaracteristicaEquipo")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                IP = ListIps,//reader.IsDBNull(reader.GetOrdinal("DetalleComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleComponente")),
                                CodigoEquipo = reader.IsDBNull(reader.GetOrdinal("CodigoUnico")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoUnico")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                EquipoFisico = reader.IsDBNull(reader.GetOrdinal("EquipoFisico")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoFisico")),
                            };

                            if (objeto.TipoEquipoId == tipoEquipoId_ClientSecrets)
                            {
                                hasClientSecret = true;
                                ojbClientSecret = new EquipoClientSecretDTO()
                                {
                                    NombreCloud = reader.IsDBNull(reader.GetOrdinal("NombreCloud")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreCloud")),
                                    Suscripcion = reader.IsDBNull(reader.GetOrdinal("Suscripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Suscripcion")),
                                    TipoRecurso = reader.IsDBNull(reader.GetOrdinal("TipoRecurso")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRecurso")),
                                    AKSsecret = reader.IsDBNull(reader.GetOrdinal("AKSsecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("AKSsecret")),
                                    Recurso = reader.IsDBNull(reader.GetOrdinal("Recurso")) ? string.Empty : reader.GetString(reader.GetOrdinal("Recurso")),
                                    TipoClientSecret = reader.IsDBNull(reader.GetOrdinal("TipoClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoClientSecret")),
                                    FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                    FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                    IdentificadorClientSecret = reader.IsDBNull(reader.GetOrdinal("IdentificadorClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("IdentificadorClientSecret")),
                                    NamespaceClientSecret = reader.IsDBNull(reader.GetOrdinal("NamespaceClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("NamespaceClientSecret")),
                                    ResourceGroupClientSecret = reader.IsDBNull(reader.GetOrdinal("ResourceGroupClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourceGroupClientSecret")),
                                    ThumbprintClientSecret = reader.IsDBNull(reader.GetOrdinal("ThumbprintClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("ThumbprintClientSecret")),
                                    IssuerClientSecret = reader.IsDBNull(reader.GetOrdinal("IssuerClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("IssuerClientSecret")),
                                };
                            }
                        }
                        reader.Close();

                        if (hasClientSecret)
                            objeto.ClientSecrets.Add(ojbClientSecret);

                    }
                    return objeto;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipoDetalleEscaneadasVsRegistradas(int id, DateTime fecha)
        {
            try
            {
                var data = new List<CustomAutocomplete>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var ANIO = fecha.Year;
                        var MES = fecha.Month;
                        var DIA = fecha.Day;


                        var tecnologiasEscaneadas = (from a in ctx.EquipoTecnologia
                                                     where a.FlagActivo
                                                     && a.AnioRegistro == ANIO && a.MesRegistro == MES && a.DiaRegistro == DIA
                                                     && a.EquipoId == id
                                                     select a.TecnologiaId).Count();

                        data.Add(new CustomAutocomplete { Descripcion = "Escaneadas", Valor = tecnologiasEscaneadas });

                        var tecnologiasNoRegistradas = (from a in ctx.TecnologiaNoRegistrada
                                                        where a.Activo
                                                        && a.FlagAsociado == false
                                                        && a.EquipoId == id
                                                        select a.TecnologiaNoRegistradaId).Count();

                        data.Add(new CustomAutocomplete { Descripcion = "No registradas", Valor = tecnologiasNoRegistradas });

                        return data;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: GetEquipoDetalleEscaneadasVsRegistradas(int id, DateTime fecha)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: GetEquipoDetalleEscaneadasVsRegistradas(int id, DateTime fecha)"
                    , new object[] { null });
            }
        }


        public override EquipoTecnologiaDTO GetSOById(int EquipoId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime FECHA_ACTUAL = DateTime.Now;
                        var entidad = (from u in ctx.EquipoTecnologia
                                       join t in ctx.Tecnologia on u.TecnologiaId equals t.TecnologiaId
                                       where u.EquipoId == EquipoId
                                       && u.DiaRegistro == FECHA_ACTUAL.Day
                                       && u.MesRegistro == FECHA_ACTUAL.Month
                                       && u.AnioRegistro == FECHA_ACTUAL.Year
                                       && u.FlagActivo
                                       select new EquipoTecnologiaDTO()
                                       {
                                           Id = u.ServidorTecnologiaId,
                                           Activo = u.FlagActivo,
                                           EquipoId = u.EquipoId.Value,
                                           TecnologiaId = u.TecnologiaId,
                                           TecnologiaStr = t.ClaveTecnologia,
                                           Dia = u.DiaRegistro.Value,
                                           Mes = u.MesRegistro.Value,
                                           Anio = u.AnioRegistro.Value,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.CreadoPor
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoTecnologiaDTO GetSOById(int EquipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoTecnologiaDTO GetSOById(int EquipoId)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetServidoresRelacionadosByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                        var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;


                        var idsTipoEquipo = new List<int>{
                                Utilitarios.TIPO_EQUIPO_SERVIDOR_ID,
                                Utilitarios.TIPO_EQUIPO_SERVIDOR_AGENCIA_ID
                            };
                        DateTime fechaConsulta = DateTime.Now;
                        //Servidor Relacionado

                        var ANIO = fechaConsulta.Year;
                        var MES = fechaConsulta.Month;
                        var DIA = fechaConsulta.Day;

                        var relacionesActivas = new List<int>() {
                            (int)EEstadoRelacion.Aprobado,
                            (int)EEstadoRelacion.PendienteEliminacion
                        };

                        var registros = (from r in ctx.Relacion
                                         join e in ctx.Equipo on r.EquipoId equals e.EquipoId
                                         join a in ctx.Ambiente on e.AmbienteId equals a.AmbienteId
                                         join te in ctx.TipoEquipo on e.TipoEquipoId equals te.TipoEquipoId
                                         //SISTEMA OPERATIVO

                                         join et in ctx.EquipoTecnologia on new { EquipoId = e.EquipoId, Dia = DIA, Mes = MES, Anio = ANIO } equals new { EquipoId = et.EquipoId.Value, Dia = et.DiaRegistro.Value, Mes = et.MesRegistro.Value, Anio = et.AnioRegistro.Value } into lj3
                                         from et in lj3.DefaultIfEmpty()
                                         join t in ctx.Tecnologia on et.TecnologiaId equals t.TecnologiaId into lj4
                                         from t in lj4.DefaultIfEmpty()
                                         where r.CodigoAPT == codigoAPT
                                         && idsTipoEquipo.Contains(e.TipoEquipoId.Value)
                                         && r.FlagActivo && e.FlagActivo && a.Activo
                                         && r.AnioRegistro == ANIO
                                         && r.MesRegistro == MES
                                         && r.DiaRegistro == DIA
                                         && relacionesActivas.Contains(r.EstadoId)
                                         && (t == null || t.SubdominioId == idSubdominio && t.Activo)
                                         select new EquipoDTO()
                                         {
                                             Id = e.EquipoId,
                                             Nombre = e.Nombre,
                                             FlagTemporal = e.FlagTemporal,
                                             TipoEquipoId = e.TipoEquipoId,
                                             AmbienteId = e.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = te.Nombre,
                                             FlagExcluirCalculo = e.FlagExcluirCalculo,
                                             RelacionId = r.RelacionId,
                                             SistemaOperativo = t.ClaveTecnologia,
                                             EstadoRelacionId = r.EstadoId
                                             //FlagObsoleto = (from e in ctx.Equipo
                                             //                join et in ctx.EquipoTecnologia on e.EquipoId equals et.EquipoId
                                             //                join tcv in ctx.TecnologiaCicloVida on et.TecnologiaId equals tcv.TecnologiaId
                                             //                where tcv.DiaRegistro == fechaConsulta.Day && tcv.MesRegistro == fechaConsulta.Month && tcv.AnioRegistro == fechaConsulta.Year
                                             //                && tcv.FlagActivo && et.FlagActivo && tcv.Obsoleto == 1
                                             //                && e.EquipoId == e.EquipoId
                                             //                select e.EquipoId).FirstOrDefault() != null
                                             // TODO: PROBAR CON MAYOR DATA EN RELACION
                                         }).Distinct().OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        //// REMOVER LUEGO DE PROBAR EL TODO
                        //foreach (var item in resultado)
                        //{
                        //    int equiposTecnologiaObsoleto = (from e in ctx.Equipo
                        //                                     join et in ctx.EquipoTecnologia on e.EquipoId equals et.EquipoId
                        //                                     join tcv in ctx.TecnologiaCicloVida on et.TecnologiaId equals tcv.TecnologiaId
                        //                                     where tcv.DiaRegistro == DIA
                        //                                     && tcv.MesRegistro == MES
                        //                                     && tcv.AnioRegistro == ANIO
                        //                                     && tcv.FlagActivo && et.FlagActivo
                        //                                     && tcv.Obsoleto == 1
                        //                                     && e.EquipoId == item.Id
                        //                                     select e.EquipoId).FirstOrDefault();
                        //    item.FlagObsoleto = equiposTecnologiaObsoleto > 0;

                        //}
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresRelacionadosByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresRelacionadosByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerApisRelacionadasPorCodigoAPT]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                EstadoRelacionId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
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
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEstadoBajaServidorByFiltro()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ListarEstadoBajaServidores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("EstadoBajaServidoresId")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoBajaServidoresId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEstadoBajaServidorByFiltro"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEstadoBajaServidorByFiltro"
                    , new object[] { null });
            }
        }

        public override List<ClientSecretDTO> GetClientSecretsByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                int mesesTipoCicloVida = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CLIENT_SECRETS_APLICACION_PARAMETRO_CICLOVIDA").Valor);
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ClientSecretDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_client_secrets_detalleaplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacionStr", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ClientSecretDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                NombreClientSecret = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                NombreCloud = reader.IsDBNull(reader.GetOrdinal("NombreCloud")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreCloud")),
                                Suscripcion = reader.IsDBNull(reader.GetOrdinal("Suscripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Suscripcion")),
                                TipoRecurso = reader.IsDBNull(reader.GetOrdinal("TipoRecurso")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRecurso")),
                                Recurso = reader.IsDBNull(reader.GetOrdinal("Recurso")) ? string.Empty : reader.GetString(reader.GetOrdinal("Recurso")),
                                TipoClientSecret = reader.IsDBNull(reader.GetOrdinal("TipoClientSecret")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoClientSecret")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                FactoresKPI_FLooking = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW"))
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoByTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime fechaConsulta = DateTime.Now;

                        var registros = (from et in ctx.EquipoTecnologia
                                         join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                         join te in ctx.TipoEquipo on e.TipoEquipoId equals te.TipoEquipoId
                                         join a in ctx.Ambiente on e.AmbienteId equals a.AmbienteId
                                         where et.TecnologiaId == tecnologiaId
                                         && et.FlagActivo && e.FlagActivo && te.FlagActivo
                                         && et.AnioRegistro == fechaConsulta.Year
                                         && et.MesRegistro == fechaConsulta.Month
                                         && et.DiaRegistro == fechaConsulta.Day
                                         select new EquipoDTO()
                                         {
                                             Id = e.EquipoId,
                                             Nombre = e.Nombre,
                                             TipoEquipo = te.Nombre,
                                             Ambiente = a.DetalleAmbiente
                                         }).Distinct().OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoByTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoByTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetEquipoTecnologiaByEqId(int equipoId)
        {
            var dia = DateTime.Now.Day;
            var mes = DateTime.Now.Month;
            var anio = DateTime.Now.Year;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from a1 in ctx.EquipoTecnologia
                                       join a2 in ctx.Tecnologia on a1.TecnologiaId equals a2.TecnologiaId
                                       join a3 in ctx.Subdominio on a2.SubdominioId equals a3.SubdominioId
                                       join a4 in ctx.Dominio on a3.DominioId equals a4.DominioId
                                       where a1.FlagActivo && a2.Activo
                                       && a1.EquipoId == equipoId
                                       && a1.DiaRegistro == dia && a1.MesRegistro == mes && a1.AnioRegistro == anio
                                       orderby a2.Nombre
                                       select new TecnologiaDTO()
                                       {
                                           Id = a1.TecnologiaId,
                                           ClaveTecnologia = a2.ClaveTecnologia,
                                           SubdominioNomb = a3.Nombre,
                                           DominioNomb = a4.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoTecnologiaByEqId(int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoTecnologiaByEqId(int equipoId)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoDetallado(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        //Sistema Operativo
                        var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                        var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

                        var equiposSO = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         where b.SubdominioId == idSubdominio
                                         && a.FlagActivo
                                         && b.Activo
                                         //&& a.AnioRegistro == fechaConsulta.Year
                                         //&& a.MesRegistro == fechaConsulta.Month
                                         //&& a.DiaRegistro == fechaConsulta.Day
                                         select new
                                         {
                                             EquipoId = a.EquipoId,
                                             SistemaOperativo = b.ClaveTecnologia
                                         }).Distinct();


                        var registros = (from u in ctx.Equipo
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join ec in ctx.EquipoConfiguracion on u.EquipoId equals ec.EquipoId
                                         join ds in ctx.DominioServidor on u.DominioServidorId equals ds.DominioId
                                         join d in equiposSO on u.EquipoId equals d.EquipoId into lj1
                                         from d in lj1.DefaultIfEmpty()
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         && u.FlagActivo && a.Activo && t.FlagActivo && ec.FlagActivo && ds.Activo
                                         && u.TipoEquipoId.Value == (tipoEquipoId == -1 ? u.TipoEquipoId.Value : tipoEquipoId)
                                         && u.FlagTemporal.Value == (desId == -1 ? u.FlagTemporal.Value : (desId == 1 ? true : false))
                                         && (exCalculoId == -1 || u.FlagExcluirCalculo == (exCalculoId == 1))
                                         select new EquipoDTO
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             SistemaOperativo = d.SistemaOperativo,
                                             EquipoConfiguracionDTO = new EquipoConfiguracionDTO()
                                             {
                                                 Componente = ec.Componente,
                                                 DetalleComponente = ec.DetalleComponente
                                             },
                                             DominioServidorDTO = new DominioServidorDTO()
                                             {
                                                 Nombre = ds.Nombre,
                                                 Equivalencias = ds.Equivalencias
                                             }
                                         }).OrderBy(sortName + " " + sortOrder);

                        return registros.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoDetalleByEqId(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoDetalleByEqId(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime fechaConsulta = DateTime.Now.Date;
                        var idsEquiposTecnologias = (from a in ctx.EquipoTecnologia
                                                     join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                     where a.AnioRegistro == fechaConsulta.Year
                                                     && a.MesRegistro == fechaConsulta.Month
                                                     && a.DiaRegistro == fechaConsulta.Day
                                                     && a.TecnologiaId == tecnologiaId
                                                     select a.EquipoId.Value).Distinct();

                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId
                                         where u.FlagActivo && b.FlagActivo
                                         && idsEquiposTecnologias.Any(gi => gi == u.EquipoId)
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             TipoEquipo = b.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             FechaUltimoEscaneoCorrectoStr = u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = u.FechaUltimoEscaneoError,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.CreadoPor,
                                             Activo = u.FlagActivo
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override List<EquipoDTO> GetEquipoXTecnologiaIdXFecha(string tecnologia, DateTime fechaConsulta, string subdominios, string owner, int indiceObs, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var parametroMeses24 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;

                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_LISTAR_EQUIPOS_X_TECNOLOGIAS_XFECHA]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsulta));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@owner", owner));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@indexObs", indiceObs));
                        comando.Parameters.Add(new SqlParameter("@mesProyeccion", parametroMeses24));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? "" : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? "" : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                            };

                            totalRows = reader.GetInt32(reader.GetOrdinal("TotalFilas"));
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override bool ExisteServidorByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Servidor
                                    where u.Activo && u.ShortDescription == nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override AmbienteDTO ExisteAmbienteByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var objeto = (from u in ctx.Ambiente
                                  where u.Activo
                                  && u.DetalleAmbiente.ToUpper().Contains(nombre.ToUpper())
                                  select new AmbienteDTO
                                  {
                                      Id = u.AmbienteId,
                                      DetalleAmbiente = u.DetalleAmbiente
                                  }).FirstOrDefault();

                    return objeto;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override List<HistoricoExclusionDTO> GetEquipoExclusion(string filtro, int tipoExclusionId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.HistoricoExclusion
                                     join e in ctx.Equipo on u.EquipoId equals e.EquipoId
                                     join t in ctx.TipoExclusion on u.TipoExclusionId equals t.TipoExclusionId
                                     where (string.IsNullOrEmpty(filtro)
                                     || e.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                     && (tipoExclusionId == -1 || t.TipoExclusionId == tipoExclusionId)
                                     && e.FlagActivo && t.Activo
                                     select new HistoricoExclusionDTO()
                                     {
                                         Id = u.HistoricoExclusionId,
                                         EquipoId = e.EquipoId,
                                         EquipoStr = e.Nombre,
                                         TipoExclusionId = t.TipoExclusionId,
                                         TipoExclusionStr = t.Nombre,
                                         MotivoExclusion = u.MotivoExclusion,
                                         Activo = u.FlagActivo,
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
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override EquipoDTO GetEquipoDetalleAdicional(string hostname)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var ds = new DataSet();
                var respuesta = new EquipoDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Cmdb_ListarAllInfoServidor]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idservidor", hostname));

                        using (var adapter = new SqlDataAdapter(comando))
                        {
                            adapter.Fill(ds);
                        }
                    }
                    cnx.Close();
                }

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    var tabla0 = ds.Tables[0];
                    if (tabla0 != null && tabla0.Rows.Count > 0)
                        respuesta.MemoriaRam = tabla0.Rows[0][0].ToString();

                    var tabla2 = ds.Tables[2];
                    if (tabla2 != null && tabla2.Rows.Count > 0)
                    {
                        respuesta.Procesadores = new List<EquipoProcesadoresDto>();
                        foreach (DataRow dr in tabla2.Rows)
                        {
                            respuesta.Procesadores.Add(new EquipoProcesadoresDto()
                            {
                                Descripcion = dr["Descripcion"].ToString(),
                                Fabricante = dr["Fabricante"].ToString()
                            });
                        }
                    }

                    var tabla3 = ds.Tables[3];
                    if (tabla3 != null && tabla3.Rows.Count > 0)
                    {
                        respuesta.Discos = new List<EquipoEspacioDiscoDto>();
                        foreach (DataRow dr in tabla3.Rows)
                        {
                            respuesta.Discos.Add(new EquipoEspacioDiscoDto()
                            {
                                AvailableSpace = dr["AvailableSpace"].ToString(),
                                FileSystemSize = dr["FileSystemSize"].ToString(),
                                FileSystemType = dr["FileSystemType"].ToString(),
                                DescripcionCorta = dr["DescripcionCorta"].ToString()
                            });
                        }
                    }
                }


                return respuesta;

            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquiposConsultor(string nombre, string IP, string so, string ambienteIds, string tipoIds, int subdominioSO, string desIds, string subsiIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, out int totalRows)
        {
            try
            {
                var fechaConsulta = DateTime.Now;
                totalRows = 0;

                ambienteIds = string.IsNullOrEmpty(ambienteIds) ? string.Empty : ambienteIds;
                tipoIds = string.IsNullOrEmpty(tipoIds) ? string.Empty : tipoIds;
                desIds = string.IsNullOrEmpty(desIds) ? string.Empty : desIds;
                subsiIds = string.IsNullOrEmpty(subsiIds) ? string.Empty : subsiIds;

                IP = string.IsNullOrEmpty(IP) ? string.Empty : IP;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquiposUsuarios]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", nombre));
                        comando.Parameters.Add(new SqlParameter("@so", so));
                        comando.Parameters.Add(new SqlParameter("@ambiente", ambienteIds));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipoIds));
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", subdominioSO));
                        comando.Parameters.Add(new SqlParameter("@desId", desIds));
                        comando.Parameters.Add(new SqlParameter("@subsiId", subsiIds));
                        comando.Parameters.Add(new SqlParameter("@IP", IP));
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));
                        comando.Parameters.Add(new SqlParameter("@flagActivo", flagActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                FechaUltimoEscaneoCorrectoStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FechaUltimoEscaneoErrorStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoError")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoError")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CaracteristicaEquipo")),
                                EquipoFisico = reader.IsDBNull(reader.GetOrdinal("EquipoFisico")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoFisico")),                                
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                //TecnologiasInstaladas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                Subsidiaria = reader.IsDBNull(reader.GetOrdinal("Subsidiaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subsidiaria")),
                                Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Modelo")),
                                CodigoEquipo = reader.IsDBNull(reader.GetOrdinal("CodigoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoEquipo")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("tipoequipoid")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipoequipoid")),
                                ControlAprovisionamiento = reader.IsDBNull(reader.GetOrdinal("ControlAprovisionamiento")) ? 0 : reader.GetInt32(reader.GetOrdinal("ControlAprovisionamiento")),
                                DominioServidor = reader.IsDBNull(reader.GetOrdinal("DominioServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("DominioServidor"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoConsultor(string nombre, int tipoEquipoId, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        //Sistema Operativo                    
                        var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;

                        var equiposSO = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         where b.SubdominioId == idSubdominio
                                         && a.AnioRegistro == fechaConsulta.Year
                                         && a.MesRegistro == fechaConsulta.Month
                                         && a.DiaRegistro == fechaConsulta.Day
                                         select new
                                         {
                                             EquipoId = a.EquipoId,
                                             SistemaOperativo = b.ClaveTecnologia
                                         }).Distinct();


                        var registros = (from u in ctx.Equipo
                                         join r in ctx.Relacion on u.EquipoId equals r.RelacionId
                                         join aa in ctx.AplicacionExpertos on r.CodigoAPT equals aa.CodigoAPT
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join d in equiposSO on u.EquipoId equals d.EquipoId into lj1
                                         from d in lj1.DefaultIfEmpty()
                                         join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         where (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre))
                                         && (u.FlagActivo == (flagActivo != null))
                                         && a.Activo && t.FlagActivo
                                         //&& u.TipoEquipoId.Value == (tipoEquipoId == -1 ? u.TipoEquipoId.Value : tipoEquipoId)
                                         && (tipoEquipoId == -1 || u.TipoEquipoId.Value == tipoEquipoId)
                                         //&& u.FlagTemporal.Value == (desId == -1 ? u.FlagTemporal.Value : (desId == 1 ? true : false))
                                         && (desId == -1 || u.FlagTemporal.Value == (desId == 1))
                                         && (exCalculoId == -1 || u.FlagExcluirCalculo == (exCalculoId == 1))
                                         && (subsidiariaId == null || subsidiariaId == -1 || s.DominioId == subsidiariaId)
                                         && r.DiaRegistro == fechaConsulta.Day && r.MesRegistro == fechaConsulta.Month && r.AnioRegistro == fechaConsulta.Year
                                         && (r.EstadoId == 0 || r.EstadoId == 2)
                                         && aa.Matricula == matricula
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             Subsidiaria = s.Nombre,
                                             DominioServidorId = s.DominioId,
                                             SistemaOperativo = d.SistemaOperativo,
                                             FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                             FechaCambioEstado = u.FechaCambioEstado,
                                             UsuarioCambioEstado = u.UsuarioCambioEstado
                                         }).Distinct().OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override EquipoDTO ExisteEquipoByHostname(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Equipo
                                    where u.FlagActivo && u.Nombre == nombre
                                    select new EquipoDTO()
                                    {
                                        EquipoId = u.EquipoId,
                                        Nombre = u.Nombre
                                    }).FirstOrDefault();

                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario, string motivo)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Equipo
                                  where u.EquipoId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        var itemServidor = (from s in ctx.Servidor
                                            where s.HostName == itemBD.Nombre
                                            select s).FirstOrDefault();

                        if (estado) //Activar equipo
                        {
                            //cvt.Equipo
                            itemBD.FlagActivo = estado;
                            itemBD.ModificadoPor = usuario;
                            itemBD.FlagTemporal = true;
                            itemBD.FlagCambioEstadoTemporal = true;
                            itemBD.FechaModificacion = FECHA_ACTUAL;
                            itemBD.FechaCambioEstado = FECHA_ACTUAL;

                            //cmdb.Servidor
                            if (itemServidor != null)
                            {
                                itemServidor.Activo = estado;
                                itemServidor.Temporal = true;
                            }

                            var softwareBase = ctx.EquipoSoftwareBase.FirstOrDefault(x => x.EquipoId == id);
                            if (softwareBase != null)
                                softwareBase.FlagActivo = true;
                        }
                        else //Desactivar equipo
                        {
                            //cvt.Equipo
                            itemBD.FechaModificacion = FECHA_ACTUAL;
                            itemBD.ModificadoPor = usuario;
                            itemBD.FlagActivo = estado;
                            itemBD.FechaCambioEstado = FECHA_ACTUAL;
                            itemBD.UsuarioCambioEstado = usuario;
                            itemBD.ComentarioEstado = motivo;

                            //cmdb.Servidor
                            if (itemServidor != null)
                                itemServidor.Activo = estado;

                        }

                        ctx.SaveChanges();

                        if (estado)
                            this.ProcesarCicloVidaSoftwareBase(id);

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
        public override bool CambiarEstadoEquipoSoftwareBase(int id, bool estado, string usuario, string nombreUsuario)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.EquipoSoftwareBase
                                  where u.EquipoId == id && u.FlagActivo == true
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FlagActivo = estado;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FechaModificacion = DateTime.Now;
                        //Ejecutar estos pasos solo si el tipo de equipo es appliance
                        var equipo = ctx.Equipo.FirstOrDefault(x => x.EquipoId == id);
                        if (equipo != null)
                        {
                            if (equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                            {
                                if (equipo.FlagTemporal == true && string.IsNullOrEmpty(equipo.Ubicacion))
                                {
                                    equipo.FlagActivo = estado;
                                }
                                equipo.ModificadoPor = usuario;
                                equipo.FechaModificacion = DateTime.Now;
                            }
                        }

                        if (!estado)
                        {
                            //Si desactivamos el appliance hay que desestimar la solicitud 
                            var solicitudAprobada = ctx.EquipoSolicitud.FirstOrDefault(x => x.EquipoId == id && x.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoCVT);
                            if (solicitudAprobada != null)
                            {
                                //Revertir el tipo de equipo
                                equipo.TipoEquipoId = solicitudAprobada.TipoEquipoActual;
                                //if(solicitudAprobada.TipoEquipoActual == (int)ETipoEquipo.Servidor)
                                //{
                                //    equipo.FlagActivo = true;
                                //}                                    

                                //Desestimar la solicitud
                                solicitudAprobada.EstadoSolicitud = (int)EstadoSolicitudActivosTSI.Desestimado;
                                solicitudAprobada.ComentariosDesestimacion = "Appliance desactivado por lo que se revierte la solicitud";
                                solicitudAprobada.ModificadoPor = usuario;
                                solicitudAprobada.NombreUsuarioModificacion = nombreUsuario;
                                solicitudAprobada.FechaModificacion = DateTime.Now;

                                //Eliminarlo de la tabla EquipoCicloVida
                                try
                                {
                                    using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                                    {
                                        cnx.Open();
                                        using (var comando = new SqlCommand("[cvt].[USP_EliminarEquipoCicloVida]", cnx))
                                        {
                                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                                            comando.Parameters.AddWithValue("@equipo", id);
                                            comando.ExecuteNonQuery();
                                        }
                                        cnx.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message, ex);
                                }

                                //Si hubiera relación, consignarla como suspendida o activarla
                                try
                                {
                                    if (solicitudAprobada.TipoEquipoActual == (int)ETipoEquipo.Appliance)
                                    {
                                        using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                                        {
                                            cnx.Open();
                                            using (var comando = new SqlCommand("[cvt].[USP_ActivarReactivarRelacionesAppliance]", cnx))
                                            {
                                                comando.CommandType = System.Data.CommandType.StoredProcedure;
                                                comando.Parameters.AddWithValue("@equipo", id);
                                                comando.Parameters.AddWithValue("@estado", estado);
                                                int filasAfectadas = comando.ExecuteNonQuery();
                                            }
                                            cnx.Close();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message, ex);
                                }
                            }
                        }

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
                    , "Error en el metodo: bool CambiarEstadoEquipoSoftwareBase(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstadoEquipoSoftwareBase(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override TipoEquipoDTO GetTipoEquipoById(int Id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoEquipo
                                   where u.FlagActivo
                                   && u.TipoEquipoId == Id
                                   orderby u.Nombre
                                   select new TipoEquipoDTO()
                                   {
                                       Id = u.TipoEquipoId,
                                       Nombre = u.Nombre
                                   }).FirstOrDefault();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override DominioServidorDTO ExisteDominioServidorByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var objeto = (from u in ctx.DominioServidor
                                  where u.Activo
                                  && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                  select new DominioServidorDTO
                                  {
                                      Id = u.DominioId,
                                      Nombre = u.Nombre
                                  }).FirstOrDefault();

                    return objeto;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override bool ExisteEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Equipo
                                    where u.FlagActivo && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override TipoEquipoDTO GetTipoEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var objeto = (from u in ctx.TipoEquipo
                                  where u.FlagActivo
                                  && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                  select new TipoEquipoDTO
                                  {
                                      Id = u.TipoEquipoId
                                  }).FirstOrDefault();

                    return objeto;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override bool ExisteTipoEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.TipoEquipo
                                    where u.FlagActivo && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoUpdate(string filtro, int tipoEquipoId, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        var registros = (from u in ctx.Equipo
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         && a.Activo && t.FlagActivo
                                         && (tipoEquipoId == -1 || u.TipoEquipoId.Value == tipoEquipoId)
                                         && (desId == -1 || u.FlagTemporal.Value == (desId == 1))
                                         && (exCalculoId == -1 || u.FlagExcluirCalculo == (exCalculoId == 1))
                                         && (subsidiariaId == null || subsidiariaId == -1 || s.DominioId == subsidiariaId)
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             Subsidiaria = s.Nombre,
                                             DominioServidorId = s.DominioId,
                                             FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                             FlagCambioDominioRed = u.FlagCambioDominioRed,
                                             FechaCambioEstado = u.FechaCambioEstado,
                                             UsuarioCambioEstado = u.UsuarioCambioEstado,

                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override EquipoDTO ExisteEquipoAllByHostname(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Equipo
                                    where u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                    /*u.FlagActivo &&*/
                                    select new EquipoDTO()
                                    {
                                        EquipoId = u.EquipoId,
                                        Nombre = u.Nombre
                                    }).FirstOrDefault();

                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquiposDesactivados(string nombre, List<int> tipoEquipoIds, List<int> subsidiariaIds, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        var registros = (from u in ctx.Equipo
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         where (u.Nombre.ToUpper().Contains(nombre.ToUpper()) || string.IsNullOrEmpty(nombre))
                                         //&& (tipoEquipo == -1 || u.TipoEquipoId.Value == tipoEquipo)
                                         //&& (subsidiaria == -1 || s.DominioId == subsidiaria)
                                         && (tipoEquipoIds.Count == 0 || tipoEquipoIds.Contains(u.TipoEquipoId.Value))
                                         && (subsidiariaIds.Count == 0 || subsidiariaIds.Contains(s.DominioId))
                                         && u.FlagActivo == false
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             Subsidiaria = s.Nombre,
                                             DominioServidorId = s.DominioId,
                                             FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                             FlagCambioDominioRed = u.FlagCambioDominioRed,
                                             FechaCambioEstado = u.FechaCambioEstado,
                                             UsuarioCambioEstado = u.UsuarioCambioEstado,

                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipoByFiltroSinServicioNube(string filtro)
        {
            try
            {
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_TIPO_EQUIPO_SERVICIONUBE);
                int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var id_ServicioNube = parametro != null ? int.Parse(parametro.Valor) : 0;

                        var entidad = (from u in ctx.Equipo
                                       where u.FlagActivo
                                       && (!u.FlagExcluirCalculo.HasValue || !u.FlagExcluirCalculo.Value)
                                       && u.TipoEquipoId != id_ServicioNube
                                       && u.TipoEquipoId != paramCD
                                       && (u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override int ValidarEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? equipoId = (from u in ctx.Equipo
                                     where u.FlagActivo && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                     select u.EquipoId).FirstOrDefault();

                    return equipoId != null ? equipoId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarRelevanciaByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? entidadId = (from u in ctx.Relevancia
                                      where u.FlagActivo.Value && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                      select u.RelevanciaId).FirstOrDefault();

                    return entidadId != null ? entidadId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarAmbienteByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? entidadId = (from u in ctx.Ambiente
                                      where u.Activo && u.DetalleAmbiente.ToUpper().Equals(nombre.ToUpper())
                                      select u.AmbienteId).FirstOrDefault();

                    return entidadId != null ? entidadId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override string ValidarCodigoAPTByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string entidadId = (from u in ctx.Aplicacion
                                        where u.FlagActivo && u.CodigoAPT.ToUpper().Equals(nombre.ToUpper())
                                        select u.CodigoAPT).FirstOrDefault();

                    return entidadId != null ? entidadId : "";
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarDominioServidorByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? entidadId = (from u in ctx.DominioServidor
                                      where u.Activo && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                      select u.DominioId).FirstOrDefault();

                    return entidadId != null ? entidadId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarTecnologiaByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? entidadId = (from u in ctx.Tecnologia
                                      where u.Activo && u.ClaveTecnologia.ToUpper().Equals(nombre.ToUpper())
                                      //&& u.SubdominioId == 36
                                      select u.TecnologiaId).FirstOrDefault();

                    return entidadId != null ? entidadId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarTipoEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? entidadId = (from u in ctx.TipoEquipo
                                      where u.FlagActivo && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                      select u.TipoEquipoId).FirstOrDefault();

                    return entidadId != null ? entidadId.Value : 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ValidarCaracteristicaEquipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string c1 = Utilitarios.GetEnumDescription2((ECaracteristicaEquipo)((int)ECaracteristicaEquipo.Fisico));
                    string c2 = Utilitarios.GetEnumDescription2((ECaracteristicaEquipo)((int)ECaracteristicaEquipo.PaaS));
                    string c3 = Utilitarios.GetEnumDescription2((ECaracteristicaEquipo)((int)ECaracteristicaEquipo.Virtual));
                    var ID = 0;

                    if (c1.ToUpper().Contains(nombre.ToUpper()))
                    {
                        ID = (int)ECaracteristicaEquipo.Fisico;
                    }
                    else
                    if (c2.ToUpper().Contains(nombre.ToUpper()))
                    {
                        ID = (int)ECaracteristicaEquipo.PaaS;
                    }
                    else
                    if (c3.ToUpper().Contains(nombre.ToUpper()))
                    {
                        ID = (int)ECaracteristicaEquipo.Virtual;
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }


        public override FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores()
        {
            try
            {
                FiltrosIndicadoresGerencialEquipo arr_data = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosIndicadoresGerencialEquipo();


                        arr_data.ListaSubdominios = (from t in ctx.Tecnologia
                                                     join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                                     join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                     where t.Activo
                                                     //&& t.EstadoId == (int)EstadoTecnologia.Aprobado
                                                     && s.Activo
                                                     //group s by new { s.SubdominioId, s.Nombre, s.DominioId, Dominio = d.Nombre } into grp
                                                     select new CustomAutocomplete()
                                                     {
                                                         Id = s.SubdominioId.ToString(),
                                                         Descripcion = s.Nombre,
                                                         TipoId = s.DominioId.ToString(),
                                                         TipoDescripcion = d.Nombre
                                                     }).Distinct().OrderBy(z => z.Descripcion).ToList();

                        arr_data.ListaTipoEquipos = (from te in ctx.TipoEquipo
                                                     where te.FlagActivo
                                                     //orderby te.Nombre
                                                     select new CustomAutocomplete()
                                                     {
                                                         Id = te.TipoEquipoId.ToString(),
                                                         Descripcion = te.Nombre
                                                     }).OrderBy(z => z.Descripcion).ToList();

                        arr_data.ListaSubsidiarias = (from te in ctx.DominioServidor
                                                      where te.Activo
                                                      //orderby te.Nombre
                                                      select new CustomAutocomplete()
                                                      {
                                                          Id = te.DominioId.ToString(),
                                                          Descripcion = te.Nombre
                                                      }).OrderBy(z => z.Descripcion).ToList();


                        arr_data.ListaTipoTecnologias = (from a in ctx.Tipo
                                                         where a.Activo
                                                         select new CustomAutocomplete()
                                                         {
                                                             Id = a.TipoId.ToString(),
                                                             Descripcion = a.Nombre
                                                         }).Distinct().ToList();



                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores()"
                    , new object[] { null });
            }
        }

        public override FiltrosIndicadoresGerencialEquipo ListFiltrosEvolucionInstalacionEquipos()
        {
            try
            {
                FiltrosIndicadoresGerencialEquipo arr_data = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosIndicadoresGerencialEquipo();


                        arr_data.ListaTipoEquipos = (from te in ctx.TipoEquipo
                                                     where te.FlagActivo
                                                     //orderby te.Nombre
                                                     select new CustomAutocomplete()
                                                     {
                                                         Id = te.TipoEquipoId.ToString(),
                                                         Descripcion = te.Nombre
                                                     }).OrderBy(z => z.Descripcion).ToList();

                        arr_data.ListaSubsidiarias = (from te in ctx.DominioServidor
                                                      where te.Activo
                                                      //orderby te.Nombre
                                                      select new CustomAutocomplete()
                                                      {
                                                          Id = te.DominioId.ToString(),
                                                          Descripcion = te.Nombre
                                                      }).OrderBy(z => z.Descripcion).ToList();

                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores()"
                    , new object[] { null });
            }
        }
        // TO USE EQUIPO LIST AND EXPORT
        public override List<EquipoDTO> GetEquipos(string nombre, string IP, string so, string ambientesIds, string tiposIds, int subdominioSO, string desIds, string subsiIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                nombre = string.IsNullOrEmpty(nombre) ? string.Empty : nombre;
                so = string.IsNullOrEmpty(so) ? string.Empty : so;
                IP = string.IsNullOrEmpty(IP) ? string.Empty : IP;

                ambientesIds = string.IsNullOrEmpty(ambientesIds) ? string.Empty : ambientesIds;
                tiposIds = string.IsNullOrEmpty(tiposIds) ? string.Empty : tiposIds;
                desIds = string.IsNullOrEmpty(desIds) ? string.Empty : desIds;
                subsiIds = string.IsNullOrEmpty(subsiIds) ? string.Empty : subsiIds;

                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquiposAdministradores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", nombre));
                        comando.Parameters.Add(new SqlParameter("@so", so));
                        comando.Parameters.Add(new SqlParameter("@ambiente", ambientesIds));
                        comando.Parameters.Add(new SqlParameter("@tipo", tiposIds));
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", subdominioSO));
                        comando.Parameters.Add(new SqlParameter("@desId", desIds));
                        comando.Parameters.Add(new SqlParameter("@subsiId", subsiIds));
                        comando.Parameters.Add(new SqlParameter("@IP", IP));
                        comando.Parameters.Add(new SqlParameter("@flagActivo", flagActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                Ubicacion = reader.IsDBNull(reader.GetOrdinal("Ubicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ubicacion")),
                                FechaUltimoEscaneoCorrectoStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FechaUltimoEscaneoErrorStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoError")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoError")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CaracteristicaEquipo")),
                                EquipoFisico = reader.IsDBNull(reader.GetOrdinal("EquipoFisico")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoFisico")),                                
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                //TecnologiasInstaladas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                Subsidiaria = reader.IsDBNull(reader.GetOrdinal("Subsidiaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subsidiaria")),
                                Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Modelo")),
                                CodigoEquipo = reader.IsDBNull(reader.GetOrdinal("CodigoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoEquipo")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("tipoequipoid")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipoequipoid")),
                                ControlAprovisionamiento = reader.IsDBNull(reader.GetOrdinal("ControlAprovisionamiento")) ? 0 : reader.GetInt32(reader.GetOrdinal("ControlAprovisionamiento")),
                                DominioServidor = reader.IsDBNull(reader.GetOrdinal("DominioServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("DominioServidor"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<EquipoDTO> GetServidoresBaja(string lstServidores, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                lstServidores = string.IsNullOrEmpty(lstServidores) ? string.Empty : lstServidores;
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_BajaServidoresByExcel]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstServidores", lstServidores));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Ticket = reader.IsDBNull(reader.GetOrdinal("Ticket")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ticket")),
                                FechaCargarArchivoBaja = reader.IsDBNull(reader.GetOrdinal("FechaCargarArchivoBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCargarArchivoBaja")),
                                EstadoServidorStr = reader.IsDBNull(reader.GetOrdinal("EstadoServidorStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoServidorStr")),
                                AplicacionesRelacionadasStr = reader.IsDBNull(reader.GetOrdinal("App_Relacionadas")) ? string.Empty : reader.GetString(reader.GetOrdinal("App_Relacionadas")),
                                DominioServidor = reader.IsDBNull(reader.GetOrdinal("DominioServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("DominioServidor")),
                                Observacion = reader.IsDBNull(reader.GetOrdinal("Observacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observacion"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresBaja(string nombre, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetListarPendienteBajaServidores(string nombre, string ticket, string fechaBusIni, string FechaBusFin, List<int> estadoSolicitudBaja, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                nombre = string.IsNullOrEmpty(nombre) ? string.Empty : nombre;
                ticket = string.IsNullOrEmpty(ticket) ? string.Empty : ticket;
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ListarPendienteBajaServidores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Nombre", nombre));
                        comando.Parameters.Add(new SqlParameter("@Ticket", ticket));
                        comando.Parameters.Add(new SqlParameter("@FechaInicio", fechaBusIni));
                        comando.Parameters.Add(new SqlParameter("@FechaFin", FechaBusFin));
                        comando.Parameters.AddWithValue("@estadoSolicitudBaja ", estadoSolicitudBaja == null ? DBNull.Value : estadoSolicitudBaja.Count == 0 ? DBNull.Value : (object)string.Join(",", estadoSolicitudBaja.ToArray()));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("BandejaCambiosServidorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BandejaCambiosServidorId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Ticket = reader.IsDBNull(reader.GetOrdinal("Ticket")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ticket")),
                                FechaCargarArchivoBaja = reader.IsDBNull(reader.GetOrdinal("FechaCargarArchivoBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCargarArchivoBaja")),
                                EstadoServidorStr = reader.IsDBNull(reader.GetOrdinal("EstadoServidorStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoServidorStr")),
                                AplicacionesRelacionadasStr = reader.IsDBNull(reader.GetOrdinal("App_Relacionadas")) ? string.Empty : reader.GetString(reader.GetOrdinal("App_Relacionadas")),
                                DominioServidor = reader.IsDBNull(reader.GetOrdinal("DominioServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("DominioServidor")),
                                EstadoProcesoBajaStr = reader.IsDBNull(reader.GetOrdinal("EstadoProcesoBajaStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoProcesoBajaStr")),
                                AprobadoPor = reader.IsDBNull(reader.GetOrdinal("AprobadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("AprobadoPor")),
                                FechaAprobado = reader.IsDBNull(reader.GetOrdinal("FechaAprobado")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAprobado")),
                                RechazadoPor = reader.IsDBNull(reader.GetOrdinal("RechazadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("RechazadoPor")),
                                FechaRechazado = reader.IsDBNull(reader.GetOrdinal("FechaRechazo")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRechazo")),
                                CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacionBaja = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresBaja(string nombre, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override string InsertarBajaServidores(string lstServidores, string matricula, string nombreArchivo, out int totalRows)
        {
            try
            {
                lstServidores = string.IsNullOrEmpty(lstServidores) ? string.Empty : lstServidores;
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                string Observacion = string.Empty;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_InsertarBajaServidores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstIDServidores", lstServidores));
                        comando.Parameters.Add(new SqlParameter("@NombreArchivo", nombreArchivo));
                        comando.Parameters.Add(new SqlParameter("@CreadoPor", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            Observacion = reader.IsDBNull(reader.GetOrdinal("Mensaje")) ? string.Empty : reader.GetString(reader.GetOrdinal("Mensaje"));
                        }
                        reader.Close();
                    }

                    return Observacion;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresBaja(string nombre, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override string ProcesarBajaServidores(string lstServidores, string matricula, bool isApprovedBaja, out int totalRows)
        {
            try
            {
                lstServidores = string.IsNullOrEmpty(lstServidores) ? string.Empty : lstServidores;
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                string Observacion = string.Empty;
                var spProcesar = string.Empty;
                if (isApprovedBaja)
                    spProcesar = "[cvt].[USP_ProcesarBajaServidores]";
                else
                    spProcesar = "[cvt].[USP_RechazarBajaServidores]";
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(spProcesar, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstIDServidores", lstServidores));
                        comando.Parameters.Add(new SqlParameter("@procesadoPor", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            Observacion = reader.IsDBNull(reader.GetOrdinal("Mensaje")) ? string.Empty : reader.GetString(reader.GetOrdinal("Mensaje"));
                        }
                        reader.Close();
                    }
                    return Observacion;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetServidoresBaja(string nombre, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool RemoveEquipoSoftwareBase(int id)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = new EquipoSoftwareBase { EquipoSoftwareBaseId = id };
                    if (entidad != null)
                    {
                        ctx.EquipoSoftwareBase.Attach(entidad);
                        ctx.EquipoSoftwareBase.Remove(entidad);
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
                    , "Error en el metodo: bool RemoveEquipoSoftwareBase(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool RemoveEquipoSoftwareBase(int id)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditEquipoSoftwareBase(EquipoSoftwareBaseDTO objeto)
        {
            long relacionId = 0;
            DbContextTransaction transaction = null;
            DateTime FECHA_ACTUAL = DateTime.Now;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //Obtener información del Owner de la aplicación
                    var ownerAplicacion = ctx.ApplicationManagerCatalog.FirstOrDefault(x => x.applicationId == objeto.CodigoAPT
                                                                                        && x.applicationManagerId == (int)ApplicationManagerRole.Owner
                                                                                        && x.isActive);

                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.Id == 0)
                        {
                            var entidad = (from u in ctx.EquipoSoftwareBase
                                           where u.EquipoId == objeto.EquipoId
                                           select u).FirstOrDefault();

                            if (entidad != null)
                            {
                                entidad.FechaModificacion = FECHA_ACTUAL;
                                entidad.SoftwareBase = objeto.SoftwareBase;
                                entidad.FechaLanzamiento = objeto.FechaLanzamiento;
                                entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                                entidad.FechaFinExtendida = objeto.FechaFinExtendida;
                                entidad.FechaFinInterna = objeto.FechaFinInterna;
                                entidad.ComentariosFechaFin = objeto.ComentariosFechaFin;
                                entidad.FechaCalculoId = objeto.FechaCalculoId;
                                //Tab Generales
                                entidad.TipoActivo = objeto.TipoActivo;
                                entidad.Serial = objeto.Serial;
                                entidad.Modelo = objeto.Modelo;
                                entidad.Vendor = objeto.Vendor;
                                entidad.Tecnologia = objeto.Tecnologia;
                                entidad.Version = objeto.Version;
                                entidad.Hostname = objeto.Hostname;
                                entidad.IP = objeto.IP;
                                entidad.Dimension = objeto.Dimension;
                                entidad.ArquitectoSeguridad = objeto.ArquitectoSeguridad;
                                entidad.SoportePrimerNivel = objeto.SoportePrimerNivel;
                                entidad.Proveedor = objeto.Proveedor;

                                entidad.UrlNube = objeto.UrlNube;
                                entidad.Criticidad = objeto.Criticidad;
                                //Tab Vigencia
                                entidad.FechaAdquisicion = objeto.FechaAdquisicion;
                                entidad.FechaImplementacion = objeto.FechaImplementacion;
                                entidad.FechaUltimaRenovacion = objeto.FechaUltimaRenovacion;
                                entidad.VencimientoLicencia = objeto.VencimientoLicencia;
                                entidad.CantidadLicencia = objeto.CantidadLicencia;
                                entidad.FormaLicenciamiento = objeto.FormaLicenciamiento;
                                //Tab Inventario
                                entidad.CodigoInventario = objeto.CodigoInventario;
                                entidad.CyberSOC = objeto.CyberSOC;
                                entidad.Sede = objeto.Sede;
                                entidad.Sala = objeto.Sala;
                                entidad.RACK = objeto.RACK;
                                entidad.Ubicacion = objeto.Ubicacion;
                                //Tab Configuracion
                                entidad.Backup = objeto.Backup;
                                entidad.BackupFrecuencia = objeto.BackupFrecuencia;
                                entidad.BackupDescripcion = objeto.BackupDescripcion;
                                entidad.IntegracionGestorInteligencia = objeto.IntegracionGestorInteligencia;
                                entidad.ConectorSIEM = objeto.ConectorSIEM;
                                entidad.CONA = objeto.CONA;
                                entidad.UmbralCPU = objeto.UmbralCPU;
                                entidad.UmbralMemoria = objeto.UmbralMemoria;
                                entidad.UmbralDisco = objeto.UmbralDisco;
                                entidad.Ventana = objeto.Ventana;
                                entidad.EquipoDetalle = objeto.EquipoDetalle;
                                entidad.EstadoIntegracionSIEM = objeto.EstadoIntegracionSIEM;
                                entidad.CONAAvanzado = objeto.CONAAvanzado;
                                entidad.EstadoAppliance = objeto.EstadoAppliance;
                                entidad.FlagSeguridad = objeto.FlagSeguridad;

                                ctx.SaveChanges();
                                transaction.Commit();

                                this.ProcesarCicloVidaSoftwareBase(entidad.EquipoId);
                                return entidad.EquipoSoftwareBaseId;
                            }
                            else
                            {

                                var relacion = new EquipoSolicitud()
                                {
                                    CodigoAPT = objeto.CodigoAPT,
                                    CreadoPor = objeto.UsuarioCreacion,
                                    EquipoId = objeto.EquipoId,
                                    FechaCreacion = DateTime.Now,
                                    Comentarios = Utilitarios.COMENTARIO_REGISTRO_TSI_APPLIANCE,
                                    CorreoSolicitante = objeto.CorreoSolicitante,
                                    EstadoSolicitud = 2,
                                    FechaFinSoporte = objeto.VencimientoLicencia,
                                    FlagRegistroEquipo = true,
                                    FlagSeguridad = true,
                                    TipoEquipoActual = (int)ETipoEquipo.Appliance,
                                    TipoEquipoSolicitado = (int)ETipoEquipo.Appliance,
                                    UsuarioAsignado = ownerAplicacion != null ? ownerAplicacion.username : string.Empty,
                                    NombreUsuarioCreacion = objeto.NombreCreadoPor,
                                    NombreEquipo = objeto.NombreEquipo
                                };
                                var retorno = ctx.EquipoSolicitud.Add(relacion);
                                ctx.SaveChanges();

                                relacionId = retorno.EquipoSolicitudId;

                                entidad = new EquipoSoftwareBase()
                                {
                                    FlagActivo = true,
                                    UsuarioCreacion = objeto.UsuarioCreacion,
                                    FechaCreacion = FECHA_ACTUAL,
                                    EquipoId = objeto.EquipoId,
                                    SubdominioId = objeto.SubdominioId,
                                    SoftwareBase = objeto.SoftwareBase,
                                    FechaLanzamiento = objeto.FechaLanzamiento,
                                    FechaFinSoporte = objeto.FechaFinSoporte,
                                    FechaFinExtendida = objeto.FechaFinExtendida,
                                    FechaFinInterna = objeto.FechaFinInterna,
                                    ComentariosFechaFin = objeto.ComentariosFechaFin,
                                    FechaCalculoId = objeto.FechaCalculoId,
                                    //Tab Generales
                                    TipoActivo = objeto.TipoActivo,
                                    Serial = objeto.Serial,
                                    Modelo = objeto.Modelo,
                                    Vendor = objeto.Vendor,
                                    Tecnologia = objeto.Tecnologia,
                                    Version = objeto.Version,
                                    Hostname = objeto.Hostname,
                                    IP = objeto.IP,
                                    Dimension = objeto.Dimension,
                                    ArquitectoSeguridad = objeto.ArquitectoSeguridad,
                                    SoportePrimerNivel = objeto.SoportePrimerNivel,
                                    Proveedor = objeto.Proveedor,
                                    UrlNube = objeto.UrlNube,
                                    Criticidad = objeto.Criticidad,
                                    //Tab Vigencia
                                    FechaAdquisicion = objeto.FechaAdquisicion,
                                    FechaImplementacion = objeto.FechaImplementacion,
                                    FechaUltimaRenovacion = objeto.FechaUltimaRenovacion,
                                    VencimientoLicencia = objeto.VencimientoLicencia,
                                    CantidadLicencia = objeto.CantidadLicencia,
                                    FormaLicenciamiento = objeto.FormaLicenciamiento,
                                    //Tab Inventario
                                    CodigoInventario = objeto.CodigoInventario,
                                    CyberSOC = objeto.CyberSOC,
                                    Sede = objeto.Sede,
                                    Sala = objeto.Sala,
                                    RACK = objeto.RACK,
                                    Ubicacion = objeto.Ubicacion,
                                    //Tab Configuracion
                                    Backup = objeto.Backup,
                                    BackupFrecuencia = objeto.BackupFrecuencia,
                                    BackupDescripcion = objeto.BackupDescripcion,
                                    IntegracionGestorInteligencia = objeto.IntegracionGestorInteligencia,
                                    ConectorSIEM = objeto.ConectorSIEM,
                                    CONA = objeto.CONA,
                                    UmbralCPU = objeto.UmbralCPU,
                                    UmbralMemoria = objeto.UmbralMemoria,
                                    UmbralDisco = objeto.UmbralDisco,
                                    Ventana = objeto.Ventana,
                                    EquipoDetalle = objeto.EquipoDetalle,
                                    EstadoIntegracionSIEM = objeto.EstadoIntegracionSIEM,
                                    CONAAvanzado = objeto.CONAAvanzado,
                                    EstadoAppliance = objeto.EstadoAppliance,
                                    FlagSeguridad = objeto.FlagSeguridad,
                                    CodigoAPT = objeto.CodigoAPT,
                                    FlagAprobado = false
                                };
                                ctx.EquipoSoftwareBase.Add(entidad);
                                ctx.SaveChanges();
                                this.ProcesarCicloVidaSoftwareBase(objeto.EquipoId);

                                transaction.Commit();
                            }

                            //Proceso de envío de correos
                            EnviarCorreoAppliance(Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI, (int)relacionId, ownerAplicacion != null ? ownerAplicacion.email : string.Empty);

                            return entidad.EquipoSoftwareBaseId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.EquipoSoftwareBase
                                           where u.EquipoSoftwareBaseId == objeto.Id
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.FechaModificacion = FECHA_ACTUAL;
                                entidad.SoftwareBase = objeto.SoftwareBase;
                                entidad.FechaLanzamiento = objeto.FechaLanzamiento;
                                entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                                entidad.FechaFinExtendida = objeto.FechaFinExtendida;
                                entidad.FechaFinInterna = objeto.FechaFinInterna;
                                entidad.ComentariosFechaFin = objeto.ComentariosFechaFin;
                                entidad.FechaCalculoId = objeto.FechaCalculoId;
                                //Tab Generales
                                entidad.TipoActivo = objeto.TipoActivo;
                                entidad.Serial = objeto.Serial;
                                entidad.Modelo = objeto.Modelo;
                                entidad.Vendor = objeto.Vendor;
                                entidad.Tecnologia = objeto.Tecnologia;
                                entidad.Version = objeto.Version;
                                entidad.Hostname = objeto.Hostname;
                                entidad.IP = objeto.IP;
                                entidad.Dimension = objeto.Dimension;
                                entidad.ArquitectoSeguridad = objeto.ArquitectoSeguridad;
                                entidad.SoportePrimerNivel = objeto.SoportePrimerNivel;
                                entidad.Proveedor = objeto.Proveedor;

                                entidad.UrlNube = objeto.UrlNube;
                                entidad.Criticidad = objeto.Criticidad;
                                //Tab Vigencia
                                entidad.FechaAdquisicion = objeto.FechaAdquisicion;
                                entidad.FechaImplementacion = objeto.FechaImplementacion;
                                entidad.FechaUltimaRenovacion = objeto.FechaUltimaRenovacion;
                                entidad.VencimientoLicencia = objeto.VencimientoLicencia;
                                entidad.CantidadLicencia = objeto.CantidadLicencia;
                                entidad.FormaLicenciamiento = objeto.FormaLicenciamiento;
                                //Tab Inventario
                                entidad.CodigoInventario = objeto.CodigoInventario;
                                entidad.CyberSOC = objeto.CyberSOC;
                                entidad.Sede = objeto.Sede;
                                entidad.Sala = objeto.Sala;
                                entidad.RACK = objeto.RACK;
                                entidad.Ubicacion = objeto.Ubicacion;
                                //Tab Configuracion
                                entidad.Backup = objeto.Backup;
                                entidad.BackupFrecuencia = objeto.BackupFrecuencia;
                                entidad.BackupDescripcion = objeto.BackupDescripcion;
                                entidad.IntegracionGestorInteligencia = objeto.IntegracionGestorInteligencia;
                                entidad.ConectorSIEM = objeto.ConectorSIEM;
                                entidad.CONA = objeto.CONA;
                                entidad.UmbralCPU = objeto.UmbralCPU;
                                entidad.UmbralMemoria = objeto.UmbralMemoria;
                                entidad.UmbralDisco = objeto.UmbralDisco;
                                entidad.Ventana = objeto.Ventana;
                                entidad.EquipoDetalle = objeto.EquipoDetalle;
                                entidad.EstadoIntegracionSIEM = objeto.EstadoIntegracionSIEM;
                                entidad.CONAAvanzado = objeto.CONAAvanzado;
                                entidad.EstadoAppliance = objeto.EstadoAppliance;
                                entidad.FlagSeguridad = objeto.FlagSeguridad;

                                ctx.SaveChanges();
                                transaction.Commit();

                                this.ProcesarCicloVidaSoftwareBase(entidad.EquipoId);
                                return entidad.EquipoSoftwareBaseId;
                            }
                            else
                                return 0;
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool ExisteEquipoByNombre(string clave, int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Equipo
                                        where u.Nombre.ToUpper().Equals(clave.ToUpper())
                                        && u.EquipoId != id
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

        public override List<EquipoDTO> GetEquipoAppliance(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                DateTime fechaConsulta = DateTime.Now;
                var meses1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);
                //var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Equipo
                                         join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                         join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                         join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         join entidadSB in ctx.EquipoSoftwareBase on u.EquipoId equals entidadSB.EquipoId
                                         where a.Activo && t.FlagActivo && u.TipoEquipoId == (int)ETipoEquipo.Appliance
                                         && u.FlagActivo == true
                                         && (string.IsNullOrEmpty(pag.nombre) || entidadSB.Tecnologia.ToUpper().Contains(pag.nombre.ToUpper()))
                                         //&& (pag.flagActivo == -1 || entidadSB.FlagActivo == (pag.flagActivo == 1))
                                         && entidadSB.FlagActivo == true
                                         && entidadSB.FlagSeguridad == true
                                         && entidadSB.FlagAprobado == true
                                         && (string.IsNullOrEmpty(pag.NombreEquipo) || u.Nombre.ToUpper().Contains(pag.NombreEquipo.ToUpper()))
                                         //&& (pag.tipoEquipoId == -1 || u.TipoEquipoId.Value == pag.tipoEquipoId)
                                         //&& (desId == -1 || u.FlagTemporal.Value == (desId == 1))
                                         //&& (exCalculoId == -1 || u.FlagExcluirCalculo == (exCalculoId == 1))
                                         //&& (subsidiariaId == null || subsidiariaId == -1 || s.DominioId == subsidiariaId)
                                         //&& (pag.tipoEquipoIds.Count == 0 || pag.tipoEquipoIds.Contains(u.TipoEquipoId.Value)
                                         //       || ((pag.tipoEquipoId == (int)ETipoEquipo.Servidor || pag.tipoEquipoId == (int)ETipoEquipo.PC || pag.tipoEquipoId == (int)ETipoEquipo.ServidorAgencia))
                                         //       && u.CaracteristicaEquipo == (int)ECaracteristicaEquipo.Fisico)
                                         select new EquipoDTO()
                                         {
                                             Id = u.EquipoId,
                                             Nombre = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             TipoEquipoId = u.TipoEquipoId,
                                             AmbienteId = u.AmbienteId,
                                             Ambiente = a.DetalleAmbiente,
                                             TipoEquipo = t.Nombre,
                                             Activo = u.FlagActivo,
                                             FlagExcluirCalculo = u.FlagExcluirCalculo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             Subsidiaria = s.Nombre,
                                             DominioServidorId = s.DominioId,
                                             FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                             FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                             FlagCambioDominioRed = u.FlagCambioDominioRed,
                                             FechaCambioEstado = u.FechaCambioEstado,
                                             UsuarioCambioEstado = u.UsuarioCambioEstado,
                                             EquipoSoftwareBase = new EquipoSoftwareBaseDTO()
                                             {
                                                 Id = entidadSB != null ? entidadSB.EquipoSoftwareBaseId : 0,
                                                 EquipoId = entidadSB != null ? entidadSB.EquipoId : 0,
                                                 SubdominioId = entidadSB != null ? entidadSB.SubdominioId : 0,
                                                 SoftwareBase = entidadSB != null ? entidadSB.SoftwareBase : null,
                                                 FechaLanzamiento = entidadSB != null ? entidadSB.FechaLanzamiento : null,
                                                 FechaFinSoporte = entidadSB != null ? entidadSB.FechaFinSoporte : null,
                                                 FechaFinExtendida = entidadSB != null ? entidadSB.FechaFinExtendida : null,
                                                 FechaFinInterna = entidadSB != null ? entidadSB.FechaFinInterna : null,
                                                 ComentariosFechaFin = entidadSB != null ? entidadSB.ComentariosFechaFin : null,
                                                 FechaCalculoId = entidadSB != null ? entidadSB.FechaCalculoId : null,
                                                 //Tab Generales
                                                 TipoActivo = entidadSB != null ? entidadSB.TipoActivo : null,
                                                 Serial = entidadSB != null ? entidadSB.Serial : null,
                                                 Modelo = entidadSB != null ? entidadSB.Modelo : null,
                                                 Vendor = entidadSB != null ? entidadSB.Vendor : null,
                                                 Tecnologia = entidadSB != null ? entidadSB.Tecnologia : null,
                                                 Version = entidadSB != null ? entidadSB.Version : null,
                                                 Hostname = entidadSB != null ? entidadSB.Hostname : null,
                                                 IP = entidadSB != null ? entidadSB.IP : null,
                                                 Dimension = entidadSB != null ? entidadSB.Dimension : null,
                                                 ArquitectoSeguridad = entidadSB != null ? entidadSB.ArquitectoSeguridad : null,
                                                 SoportePrimerNivel = entidadSB != null ? entidadSB.SoportePrimerNivel : null,
                                                 Proveedor = entidadSB != null ? entidadSB.Proveedor : null,
                                                 UrlNube = entidadSB != null ? entidadSB.UrlNube : null,
                                                 Criticidad = entidadSB != null ? entidadSB.Criticidad : null,
                                                 //Tab Vigencia
                                                 FechaAdquisicion = entidadSB != null ? entidadSB.FechaAdquisicion : null,
                                                 FechaImplementacion = entidadSB != null ? entidadSB.FechaImplementacion : null,
                                                 FechaUltimaRenovacion = entidadSB != null ? entidadSB.FechaUltimaRenovacion : null,
                                                 VencimientoLicencia = entidadSB != null ? entidadSB.VencimientoLicencia : null,
                                                 CantidadLicencia = entidadSB != null ? entidadSB.CantidadLicencia : null,
                                                 FormaLicenciamiento = entidadSB != null ? entidadSB.FormaLicenciamiento : null,
                                                 //Tab Inventario
                                                 CodigoInventario = entidadSB != null ? entidadSB.CodigoInventario : null,
                                                 CyberSOC = entidadSB != null ? entidadSB.CyberSOC : null,
                                                 Sede = entidadSB != null ? entidadSB.Sede : null,
                                                 Sala = entidadSB != null ? entidadSB.Sala : null,
                                                 RACK = entidadSB != null ? entidadSB.RACK : null,
                                                 Ubicacion = entidadSB != null ? entidadSB.Ubicacion : null,
                                                 //Tab Configuracion
                                                 Backup = entidadSB != null ? entidadSB.Backup : null,
                                                 BackupFrecuencia = entidadSB != null ? entidadSB.BackupFrecuencia : null,
                                                 BackupDescripcion = entidadSB != null ? entidadSB.BackupDescripcion : null,
                                                 IntegracionGestorInteligencia = entidadSB != null ? entidadSB.IntegracionGestorInteligencia : null,
                                                 ConectorSIEM = entidadSB != null ? entidadSB.ConectorSIEM : null,
                                                 CONA = entidadSB != null ? entidadSB.CONA : null,
                                                 UmbralCPU = entidadSB != null ? entidadSB.UmbralCPU : null,
                                                 UmbralMemoria = entidadSB != null ? entidadSB.UmbralMemoria : null,
                                                 UmbralDisco = entidadSB != null ? entidadSB.UmbralDisco : null,
                                                 Ventana = entidadSB != null ? entidadSB.Ventana : null,
                                                 EquipoDetalle = entidadSB != null ? entidadSB.EquipoDetalle : null,
                                                 EstadoIntegracionSIEM = entidadSB != null ? entidadSB.EstadoIntegracionSIEM : null,
                                                 CONAAvanzado = entidadSB != null ? entidadSB.CONAAvanzado : null,
                                                 EstadoAppliance = entidadSB != null ? entidadSB.EstadoAppliance : null,
                                                 Activo = entidadSB != null ? entidadSB.FlagActivo : false,
                                                 CodigoAPT = entidadSB != null ? entidadSB.CodigoAPT : string.Empty,
                                                 FlagAprobado = entidadSB != null ? entidadSB.FlagAprobado : null
                                             }
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        //Indicadores
                        if (totalRows > 0)
                        {
                            foreach (var item in resultado)
                            {
                                string parametrica_valor = string.Empty;
                                var iESB = item.EquipoSoftwareBase;

                                var lEnumsAppliance = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(null, (int)EEntidadParametrica.APPLIANCE);

                                //ENUMS
                                //Tipo activo
                                if (iESB.TipoActivo != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.TipoActivo.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.TipoActivoStr = parametrica_valor;
                                }

                                //Dimension
                                if (iESB.Dimension != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Dimension.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.DimensionStr = parametrica_valor;
                                }

                                //Sede
                                if (iESB.Sede != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Sede.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.SedeStr = parametrica_valor;
                                }

                                //Backup
                                if (iESB.Backup != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Backup.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.BackupStr = parametrica_valor;
                                }

                                //BackupFrecuencia
                                if (iESB.BackupFrecuencia != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.BackupFrecuencia.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.BackupFrecuenciaStr = parametrica_valor;
                                }

                                //IntegracionGestorInteligencia
                                if (iESB.IntegracionGestorInteligencia != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.IntegracionGestorInteligencia.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.IntegracionGestorInteligenciaStr = parametrica_valor;
                                }

                                //CONA
                                if (iESB.CONA != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.CONA.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.ConaStr = parametrica_valor;
                                }

                                //Criticidad
                                if (iESB.Criticidad != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Criticidad.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.CriticidadStr = parametrica_valor;
                                }

                                //CyberSOC
                                if (iESB.CyberSOC != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => x.Id == iESB.CyberSOC);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.CyberSOCStr = parametrica_valor;
                                }

                                item.EquipoSoftwareBase = iESB;

                                //Indicadores
                                var equipoRegistro = ctx.EquipoCicloVida.FirstOrDefault(x => x.EquipoId == item.Id
                                && x.DiaRegistro == fechaConsulta.Day
                                && x.MesRegistro == fechaConsulta.Month
                                && x.AnioRegistro == fechaConsulta.Year
                                && x.FlagActivo);
                                if (equipoRegistro != null)
                                {
                                    if (equipoRegistro.FechaCalculoBase.HasValue)
                                    {
                                        item.EstadoActual = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta, meses1);
                                        item.EstadoIndicador1 = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta.AddMonths(meses1), meses1);
                                        item.EstadoIndicador2 = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta.AddMonths(meses2), meses1);
                                    }
                                    else
                                    {
                                        item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                    }
                                }
                                else
                                {
                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                }
                            }
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override EquipoSoftwareBaseDTO GetEquipoSoftwareBaseConfiguradoById(int id)
        {
            try
            {
                var eSB = new EquipoSoftwareBaseDTO();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidadSB = ctx.EquipoSoftwareBase.Where(x => x.EquipoId == id && x.FlagActivo).FirstOrDefault();
                    if (entidadSB != null)
                    {
                        eSB.VencimientoLicencia = entidadSB.VencimientoLicencia;
                    }

                    return eSB;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoSoftwareBaseDTO GetEquipoSoftwareBaseConfiguradoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EquipoSoftwareBaseDTO GetEquipoSoftwareBaseConfiguradoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetEquipoConfiguracionAppliance(PaginacionEquipo pag, int TipoAcceso, out int totalRows)
        {
            try
            {
                totalRows = 0;
                List<EquipoDTO> resultado;
                DateTime fechaConsulta = DateTime.Now;
                var meses1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        if (TipoAcceso == 1)
                        {
                            var registros = (from u in ctx.Equipo
                                             join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                             join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                             join entidadSB in ctx.EquipoSoftwareBase on u.EquipoId equals entidadSB.EquipoId
                                             join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                             from s in lj2.DefaultIfEmpty()
                                             where a.Activo && t.FlagActivo
                                             //&& u.TipoEquipoId == 6
                                             && (string.IsNullOrEmpty(pag.nombre) || entidadSB.Tecnologia.ToUpper().Contains(pag.nombre.ToUpper()))
                                             && (pag.flagActivo == -1 || u.FlagActivo == (pag.flagActivo == 1))
                                             && entidadSB.FlagSeguridad == false && entidadSB.FlagActivo == true
                                             && entidadSB.FlagAprobado == true
                                             && (string.IsNullOrEmpty(pag.NombreEquipo) || u.Nombre.ToUpper().Contains(pag.NombreEquipo.ToUpper()))

                                             select new EquipoDTO()
                                             {
                                                 Id = u.EquipoId,
                                                 Nombre = u.Nombre,
                                                 FlagTemporal = u.FlagTemporal,
                                                 TipoEquipoId = u.TipoEquipoId,
                                                 AmbienteId = u.AmbienteId,
                                                 Ambiente = a.DetalleAmbiente,
                                                 TipoEquipo = t.Nombre,
                                                 Activo = u.FlagActivo,
                                                 FlagExcluirCalculo = u.FlagExcluirCalculo,
                                                 UsuarioCreacion = u.CreadoPor,
                                                 FechaCreacion = u.FechaCreacion,
                                                 FechaModificacion = u.FechaModificacion,
                                                 UsuarioModificacion = u.ModificadoPor,
                                                 Subsidiaria = s.Nombre,
                                                 DominioServidorId = s.DominioId,
                                                 FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                                 FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                                 FlagCambioDominioRed = u.FlagCambioDominioRed,
                                                 FechaCambioEstado = u.FechaCambioEstado,
                                                 UsuarioCambioEstado = u.UsuarioCambioEstado,
                                                 EquipoSoftwareBase = new EquipoSoftwareBaseDTO()
                                                 {
                                                     Id = entidadSB != null ? entidadSB.EquipoSoftwareBaseId : 0,
                                                     EquipoId = entidadSB != null ? entidadSB.EquipoId : 0,
                                                     SubdominioId = entidadSB != null ? entidadSB.SubdominioId : 0,
                                                     SoftwareBase = entidadSB != null ? entidadSB.SoftwareBase : null,
                                                     FechaLanzamiento = entidadSB != null ? entidadSB.FechaLanzamiento : null,
                                                     FechaFinSoporte = entidadSB != null ? entidadSB.FechaFinSoporte : null,
                                                     FechaFinExtendida = entidadSB != null ? entidadSB.FechaFinExtendida : null,
                                                     FechaFinInterna = entidadSB != null ? entidadSB.FechaFinInterna : null,
                                                     ComentariosFechaFin = entidadSB != null ? entidadSB.ComentariosFechaFin : null,
                                                     FechaCalculoId = entidadSB != null ? entidadSB.FechaCalculoId : null,
                                                     //Tab Generales
                                                     TipoActivo = entidadSB != null ? entidadSB.TipoActivo : null,
                                                     Serial = entidadSB != null ? entidadSB.Serial : null,
                                                     Modelo = entidadSB != null ? entidadSB.Modelo : null,
                                                     Vendor = entidadSB != null ? entidadSB.Vendor : null,
                                                     Tecnologia = entidadSB != null ? entidadSB.Tecnologia : null,
                                                     Version = entidadSB != null ? entidadSB.Version : null,
                                                     Hostname = entidadSB != null ? entidadSB.Hostname : null,
                                                     IP = entidadSB != null ? entidadSB.IP : null,
                                                     Dimension = entidadSB != null ? entidadSB.Dimension : null,
                                                     ArquitectoSeguridad = entidadSB != null ? entidadSB.ArquitectoSeguridad : null,
                                                     SoportePrimerNivel = entidadSB != null ? entidadSB.SoportePrimerNivel : null,
                                                     Proveedor = entidadSB != null ? entidadSB.Proveedor : null,
                                                     UrlNube = entidadSB != null ? entidadSB.UrlNube : null,
                                                     Criticidad = entidadSB != null ? entidadSB.Criticidad : null,
                                                     //Tab Vigencia
                                                     FechaAdquisicion = entidadSB != null ? entidadSB.FechaAdquisicion : null,
                                                     FechaImplementacion = entidadSB != null ? entidadSB.FechaImplementacion : null,
                                                     FechaUltimaRenovacion = entidadSB != null ? entidadSB.FechaUltimaRenovacion : null,
                                                     VencimientoLicencia = entidadSB != null ? entidadSB.VencimientoLicencia : null,
                                                     CantidadLicencia = entidadSB != null ? entidadSB.CantidadLicencia : null,
                                                     FormaLicenciamiento = entidadSB != null ? entidadSB.FormaLicenciamiento : null,
                                                     //Tab Inventario
                                                     CodigoInventario = entidadSB != null ? entidadSB.CodigoInventario : null,
                                                     CyberSOC = entidadSB != null ? entidadSB.CyberSOC : null,
                                                     Sede = entidadSB != null ? entidadSB.Sede : null,
                                                     Sala = entidadSB != null ? entidadSB.Sala : null,
                                                     RACK = entidadSB != null ? entidadSB.RACK : null,
                                                     Ubicacion = entidadSB != null ? entidadSB.Ubicacion : null,
                                                     //Tab Configuracion
                                                     Backup = entidadSB != null ? entidadSB.Backup : null,
                                                     BackupFrecuencia = entidadSB != null ? entidadSB.BackupFrecuencia : null,
                                                     BackupDescripcion = entidadSB != null ? entidadSB.BackupDescripcion : null,
                                                     IntegracionGestorInteligencia = entidadSB != null ? entidadSB.IntegracionGestorInteligencia : null,
                                                     ConectorSIEM = entidadSB != null ? entidadSB.ConectorSIEM : null,
                                                     CONA = entidadSB != null ? entidadSB.CONA : null,
                                                     UmbralCPU = entidadSB != null ? entidadSB.UmbralCPU : null,
                                                     UmbralMemoria = entidadSB != null ? entidadSB.UmbralMemoria : null,
                                                     UmbralDisco = entidadSB != null ? entidadSB.UmbralDisco : null,
                                                     Ventana = entidadSB != null ? entidadSB.Ventana : null,
                                                     EquipoDetalle = entidadSB != null ? entidadSB.EquipoDetalle : null,
                                                     EstadoIntegracionSIEM = entidadSB != null ? entidadSB.EstadoIntegracionSIEM : null,
                                                     CONAAvanzado = entidadSB != null ? entidadSB.CONAAvanzado : null,
                                                     EstadoAppliance = entidadSB != null ? entidadSB.EstadoAppliance : null,
                                                     Activo = entidadSB != null ? entidadSB.FlagActivo : false,
                                                     CodigoAPT = entidadSB.CodigoAPT,
                                                     FlagAprobado = entidadSB != null ? entidadSB.FlagAprobado : null
                                                 }
                                             }).OrderBy(pag.sortName + " " + pag.sortOrder);

                            totalRows = registros.Count();
                            resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();
                        }
                        else
                        {
                            var registros = (from u in ctx.Equipo
                                                 //join r in 
                                             join r in ctx.Relacion on u.EquipoId equals r.EquipoId
                                             join ap in ctx.AplicacionExpertos on r.CodigoAPT equals ap.CodigoAPT
                                             join t in ctx.TipoEquipo on u.TipoEquipoId equals t.TipoEquipoId
                                             join a in ctx.Ambiente on u.AmbienteId equals a.AmbienteId
                                             join entidadSB in ctx.EquipoSoftwareBase on u.EquipoId equals entidadSB.EquipoId
                                             join s in ctx.DominioServidor on u.DominioServidorId equals s.DominioId into lj2
                                             from s in lj2.DefaultIfEmpty()
                                             where a.Activo && t.FlagActivo && u.TipoEquipoId == 6
                                             && (r.EstadoId == 0 || r.EstadoId == 2)
                                             && ap.Matricula == pag.Matricula
                                             && (string.IsNullOrEmpty(pag.nombre) || entidadSB.Tecnologia.ToUpper().Contains(pag.nombre.ToUpper()))
                                             && (pag.flagActivo == -1 || u.FlagActivo == (pag.flagActivo == 1))
                                             && entidadSB.FlagSeguridad != true
                                             && (string.IsNullOrEmpty(pag.NombreEquipo) || u.Nombre.ToUpper().Contains(pag.NombreEquipo.ToUpper()))

                                             select new EquipoDTO()
                                             {
                                                 Id = u.EquipoId,
                                                 Nombre = u.Nombre,
                                                 FlagTemporal = u.FlagTemporal,
                                                 TipoEquipoId = u.TipoEquipoId,
                                                 AmbienteId = u.AmbienteId,
                                                 Ambiente = a.DetalleAmbiente,
                                                 TipoEquipo = t.Nombre,
                                                 Activo = u.FlagActivo,
                                                 FlagExcluirCalculo = u.FlagExcluirCalculo,
                                                 UsuarioCreacion = u.CreadoPor,
                                                 FechaCreacion = u.FechaCreacion,
                                                 FechaModificacion = u.FechaModificacion,
                                                 UsuarioModificacion = u.ModificadoPor,
                                                 Subsidiaria = s.Nombre,
                                                 DominioServidorId = s.DominioId,
                                                 FechaUltimoEscaneoCorrectoStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoCorrecto) ? "-" : u.FechaUltimoEscaneoCorrecto,
                                                 FechaUltimoEscaneoErrorStr = string.IsNullOrEmpty(u.FechaUltimoEscaneoError) ? "-" : u.FechaUltimoEscaneoError,
                                                 FlagCambioDominioRed = u.FlagCambioDominioRed,
                                                 FechaCambioEstado = u.FechaCambioEstado,
                                                 UsuarioCambioEstado = u.UsuarioCambioEstado,
                                                 EquipoSoftwareBase = new EquipoSoftwareBaseDTO()
                                                 {
                                                     Id = entidadSB != null ? entidadSB.EquipoSoftwareBaseId : 0,
                                                     EquipoId = entidadSB != null ? entidadSB.EquipoId : 0,
                                                     SubdominioId = entidadSB != null ? entidadSB.SubdominioId : 0,
                                                     SoftwareBase = entidadSB != null ? entidadSB.SoftwareBase : null,
                                                     FechaLanzamiento = entidadSB != null ? entidadSB.FechaLanzamiento : null,
                                                     FechaFinSoporte = entidadSB != null ? entidadSB.FechaFinSoporte : null,
                                                     FechaFinExtendida = entidadSB != null ? entidadSB.FechaFinExtendida : null,
                                                     FechaFinInterna = entidadSB != null ? entidadSB.FechaFinInterna : null,
                                                     ComentariosFechaFin = entidadSB != null ? entidadSB.ComentariosFechaFin : null,
                                                     FechaCalculoId = entidadSB != null ? entidadSB.FechaCalculoId : null,
                                                     //Tab Generales
                                                     TipoActivo = entidadSB != null ? entidadSB.TipoActivo : null,
                                                     Serial = entidadSB != null ? entidadSB.Serial : null,
                                                     Modelo = entidadSB != null ? entidadSB.Modelo : null,
                                                     Vendor = entidadSB != null ? entidadSB.Vendor : null,
                                                     Tecnologia = entidadSB != null ? entidadSB.Tecnologia : null,
                                                     Version = entidadSB != null ? entidadSB.Version : null,
                                                     Hostname = entidadSB != null ? entidadSB.Hostname : null,
                                                     IP = entidadSB != null ? entidadSB.IP : null,
                                                     Dimension = entidadSB != null ? entidadSB.Dimension : null,
                                                     ArquitectoSeguridad = entidadSB != null ? entidadSB.ArquitectoSeguridad : null,
                                                     SoportePrimerNivel = entidadSB != null ? entidadSB.SoportePrimerNivel : null,
                                                     Proveedor = entidadSB != null ? entidadSB.Proveedor : null,
                                                     UrlNube = entidadSB != null ? entidadSB.UrlNube : null,
                                                     Criticidad = entidadSB != null ? entidadSB.Criticidad : null,
                                                     //Tab Vigencia
                                                     FechaAdquisicion = entidadSB != null ? entidadSB.FechaAdquisicion : null,
                                                     FechaImplementacion = entidadSB != null ? entidadSB.FechaImplementacion : null,
                                                     FechaUltimaRenovacion = entidadSB != null ? entidadSB.FechaUltimaRenovacion : null,
                                                     VencimientoLicencia = entidadSB != null ? entidadSB.VencimientoLicencia : null,
                                                     CantidadLicencia = entidadSB != null ? entidadSB.CantidadLicencia : null,
                                                     FormaLicenciamiento = entidadSB != null ? entidadSB.FormaLicenciamiento : null,
                                                     //Tab Inventario
                                                     CodigoInventario = entidadSB != null ? entidadSB.CodigoInventario : null,
                                                     CyberSOC = entidadSB != null ? entidadSB.CyberSOC : null,
                                                     Sede = entidadSB != null ? entidadSB.Sede : null,
                                                     Sala = entidadSB != null ? entidadSB.Sala : null,
                                                     RACK = entidadSB != null ? entidadSB.RACK : null,
                                                     Ubicacion = entidadSB != null ? entidadSB.Ubicacion : null,
                                                     //Tab Configuracion
                                                     Backup = entidadSB != null ? entidadSB.Backup : null,
                                                     BackupFrecuencia = entidadSB != null ? entidadSB.BackupFrecuencia : null,
                                                     BackupDescripcion = entidadSB != null ? entidadSB.BackupDescripcion : null,
                                                     IntegracionGestorInteligencia = entidadSB != null ? entidadSB.IntegracionGestorInteligencia : null,
                                                     ConectorSIEM = entidadSB != null ? entidadSB.ConectorSIEM : null,
                                                     CONA = entidadSB != null ? entidadSB.CONA : null,
                                                     UmbralCPU = entidadSB != null ? entidadSB.UmbralCPU : null,
                                                     UmbralMemoria = entidadSB != null ? entidadSB.UmbralMemoria : null,
                                                     UmbralDisco = entidadSB != null ? entidadSB.UmbralDisco : null,
                                                     Ventana = entidadSB != null ? entidadSB.Ventana : null,
                                                     EquipoDetalle = entidadSB != null ? entidadSB.EquipoDetalle : null,
                                                     EstadoIntegracionSIEM = entidadSB != null ? entidadSB.EstadoIntegracionSIEM : null,
                                                     CONAAvanzado = entidadSB != null ? entidadSB.CONAAvanzado : null,
                                                     EstadoAppliance = entidadSB != null ? entidadSB.EstadoAppliance : null,
                                                     Activo = entidadSB != null ? entidadSB.FlagActivo : false
                                                 }
                                             }).OrderBy(pag.sortName + " " + pag.sortOrder).Distinct().ToList();

                            totalRows = registros.Count();
                            resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();
                        }

                        //Indicadores
                        if (totalRows > 0)
                        {
                            foreach (var item in resultado)
                            {
                                string parametrica_valor = string.Empty;
                                var iESB = item.EquipoSoftwareBase;

                                var lEnumsAppliance = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(null, (int)EEntidadParametrica.APPLIANCE);

                                //ENUMS
                                //Tipo activo
                                if (iESB.TipoActivo != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.TipoActivo.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.TipoActivoStr = parametrica_valor;
                                }

                                //Dimension
                                if (iESB.Dimension != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Dimension.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.DimensionStr = parametrica_valor;
                                }

                                //Sede
                                if (iESB.Sede != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Sede.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.SedeStr = parametrica_valor;
                                }

                                //Backup
                                if (iESB.Backup != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Backup.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.BackupStr = parametrica_valor;
                                }

                                //BackupFrecuencia
                                if (iESB.BackupFrecuencia != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.BackupFrecuencia.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.BackupFrecuenciaStr = parametrica_valor;
                                }

                                //IntegracionGestorInteligencia
                                if (iESB.IntegracionGestorInteligencia != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.IntegracionGestorInteligencia.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.IntegracionGestorInteligenciaStr = parametrica_valor;
                                }

                                //CONA
                                if (iESB.CONA != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.CONA.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.ConaStr = parametrica_valor;
                                }

                                //Criticidad
                                if (iESB.Criticidad != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => int.Parse(x.Id) == iESB.Criticidad.Value);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.CriticidadStr = parametrica_valor;
                                }

                                //CyberSOC
                                if (iESB.CyberSOC != null)
                                {
                                    var parametro = lEnumsAppliance.FirstOrDefault(x => x.Id == iESB.CyberSOC);
                                    parametrica_valor = parametro != null ? parametro.Descripcion : "";
                                    iESB.CyberSOCStr = parametrica_valor;
                                }

                                item.EquipoSoftwareBase = iESB;

                                //Indicadores
                                var equipoRegistro = ctx.EquipoCicloVida.FirstOrDefault(x => x.EquipoId == item.Id
                                && x.DiaRegistro == fechaConsulta.Day
                                && x.MesRegistro == fechaConsulta.Month
                                && x.AnioRegistro == fechaConsulta.Year
                                && x.FlagActivo);
                                if (equipoRegistro != null)
                                {
                                    if (equipoRegistro.FechaCalculoBase.HasValue)
                                    {
                                        item.EstadoActual = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta, meses1);
                                        item.EstadoIndicador1 = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta.AddMonths(meses1), meses1);
                                        item.EstadoIndicador2 = GetSemaforoEquipo(equipoRegistro.FechaCalculoBase.Value, fechaConsulta.AddMonths(meses2), meses1);
                                    }
                                    else
                                    {
                                        item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                    }
                                }
                                else
                                {
                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                }
                            }
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoConfiguracionAppliance(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoConfiguracionAppliance(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetESBSearch(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                DateTime fechaConsulta = DateTime.Now;
                var meses1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);
                //var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from entidadSB in ctx.Equipo
                                         join u in ctx.TipoEquipo on entidadSB.TipoEquipoId equals u.TipoEquipoId
                                         where
                                         entidadSB.FlagActivo && entidadSB.Nombre.Contains(pag.nombre)
                                         select new EquipoDTO()
                                         {
                                             Id = entidadSB != null ? entidadSB.EquipoId : 0,
                                             EquipoId = entidadSB != null ? entidadSB.EquipoId : 0,
                                             Nombre = entidadSB.Nombre,
                                             TipoEquipo = u.Nombre
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipo(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        private int GetSemaforoEquipo(DateTime fechaCalculo, DateTime fechaComparacion, int meses)
        {
            if (fechaCalculo < fechaComparacion)
                return (int)ColoresSemaforo.Rojo;
            else
            {
                fechaComparacion = fechaComparacion.AddMonths(meses);
                if (fechaCalculo < fechaComparacion)
                    return (int)ColoresSemaforo.Amarillo;
                else
                    return (int)ColoresSemaforo.Verde;
            }
        }

        public override string ExisteEquipoAsociadoSoftwareBaseById(int id)
        {
            var resultado = "";
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = ctx.EquipoSoftwareBase.Where(x => x.EquipoId == id && x.FlagActivo).FirstOrDefault();
                        if (entidad != null)
                        {
                            if (entidad.FlagSeguridad == true)
                            {
                                resultado = "1|true";
                            }
                            else
                            {
                                resultado = "1|false";
                            }
                        }
                        else
                        {
                            resultado = "0|false";
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: string ExisteEquipoAsociadoSoftwareBaseById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: string ExisteEquipoAsociadoSoftwareBaseById(int id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteEquipoAsociadoById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.EquipoSoftwareBase
                                        where u.EquipoId == id && u.FlagActivo
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

        public override List<EquipoDTO> ListarEquiposXTecnologiaTipoEquipo(int tecnologiaId, int tipoEquipoId)
        {

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_tecnologia_tipoequipo_listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@tecnologiaId", tecnologiaId);
                        comando.Parameters.AddWithValue("@tipoEquipoId", tipoEquipoId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TecnologiaId = reader.GetData<int>("TecnologiaId"),
                                Id = reader.GetData<int>("EquipoId"),
                                Nombre = reader.GetData<string>("Nombre"),
                                TipoEquipoId = reader.GetData<int>("TipoEquipoId"),
                                SistemaOperativo = reader.GetData<string>("SistemaOperativo"),
                                Suscripcion = reader.GetData<string>("Suscripcion")
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> ListarEquiposXProductoTipoEquipo(int productoId, int tipoEquipoId)
        {

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_producto_tipoequipo_listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@productoId", productoId);
                        comando.Parameters.AddWithValue("@tipoEquipoId", tipoEquipoId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TecnologiaId = reader.GetData<int>("TecnologiaId"),
                                ClaveTecnologia = reader.GetData<string>("TecnologiaStr"),
                                Id = reader.GetData<int>("EquipoId"),
                                Nombre = reader.GetData<string>("Nombre"),
                                TipoEquipoId = reader.GetData<int>("TipoEquipoId"),
                                SistemaOperativo = reader.GetData<string>("SistemaOperativo"),
                                Suscripcion = reader.GetData<string>("Suscripcion")
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AsignarFechaFin(EquipoDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Equipo_ConfigurarFechaFin]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipo", objeto.Id));
                        comando.Parameters.Add(new SqlParameter("@fechafin", objeto.FechaFinSoporte));

                        resultado = comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error al registrar/editar AsignarFechaFin."
                    , new object[] { null });
            }
        }

        private void ProcesarCicloVidaSoftwareBase(int equipoId)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_INSERT_INTO_EQUIPOCICLOVIDA_EQUIPO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@INDICE_OBSOLESCENCIA_POR_DEFECTO", "INDICE_OBSOLESCENCIA_POR_DEFECTO"));
                        comando.Parameters.Add(new SqlParameter("@FRECUENCIA_MESES_CALCULO_INDICE_OBSOLESCENCIA", "FRECUENCIA_MESES_CALCULO_INDICE_OBSOLESCENCIA"));
                        comando.Parameters.Add(new SqlParameter("@EQUIPO", equipoId));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }


        public override List<DatosAdicionalesEquiposDto> ListarDatosAdicionales()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<DatosAdicionalesEquiposDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ExportarDatosAdicionalesEquipos]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DatosAdicionalesEquiposDto()
                            {
                                Nombre = reader.GetData<string>("Nombre"),
                                Domain = reader.GetData<string>("Domain"),
                                Hostname = reader.GetData<string>("Hostname"),
                                IP = reader.GetData<string>("IP"),
                                ManufacturerName = reader.GetData<string>("ManufacturerName"),
                                Model = reader.GetData<string>("Model"),
                                TipoEquipo = reader.GetData<string>("TipoEquipo")
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> ListarDatosAdicionales()"
                    , new object[] { null });
            }
        }

        //private void EnviarCorreo(int tipoNotificacion, int solicitud, string owner)
        //{
        //    var emailService = new Email.MailingManager();
        //    try
        //    {
        //        using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
        //        {
        //            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
        //            {
        //                ctx.Database.CommandTimeout = 0;

        //                var registros = (from u in ctx.EquipoSolicitud
        //                                 join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId
        //                                 where u.EquipoSolicitudId == solicitud
        //                                 orderby u.FechaCreacion descending
        //                                 select new EquipoSolicitudDTO()
        //                                 {
        //                                     Id = u.EquipoSolicitudId,
        //                                     UsuarioCreacion = u.CreadoPor,
        //                                     FechaCreacion = u.FechaCreacion.Value,
        //                                     FechaModificacion = u.FechaModificacion,
        //                                     UsuarioModificacion = u.ModificadoPor,
        //                                     AprobadoRechazadoPor = u.AprobadoRechazadoPor,
        //                                     Comentarios = u.Comentarios,
        //                                     ArchivoConstancia = u.ArchivoConstancia,
        //                                     EquipoId = u.EquipoId,
        //                                     EstadoSolicitud = u.EstadoSolicitud,
        //                                     FechaAprobacionRechazo = u.FechaAprobacionRechazo,
        //                                     FechaFinSoporte = u.FechaFinSoporte,
        //                                     NombreUsuarioAprobadoRechazo = u.NombreUsuarioAprobadoRechazo,
        //                                     NombreUsuarioCreacion = u.NombreUsuarioCreacion,
        //                                     NombreUsuarioModificacion = u.NombreUsuarioModificacion,
        //                                     TipoEquipoActual = u.TipoEquipoActual,
        //                                     TipoEquipoSolicitado = u.TipoEquipoSolicitado,
        //                                     NombreEquipo = u2.Nombre,
        //                                     ComentariosAprobacionRechazo = u.ComentariosAprobacionRechazo,
        //                                     ComentariosDesestimacion = u.ComentariosDesestimacion,
        //                                     NombreArchivo = u.NombreArchivo,
        //                                     CorreoSolicitante = u.CorreoSolicitante
        //                                 }).FirstOrDefault();

        //                var notificacion = ctx.TipoNotificacion.FirstOrDefault(x => x.TipoNotificacionId == tipoNotificacion);
        //                if (notificacion != null && registros != null)
        //                {
        //                    var cuerpo = notificacion.Cuerpo;
        //                    var asunto = notificacion.Asunto;

        //                    cuerpo = cuerpo.Replace("FECHA_APROBACION", registros.FechaAprobacionRechazoToString);
        //                    cuerpo = cuerpo.Replace("TIPO_EQUIPO_FINAL", registros.TipoEquipoSolicitadoToString);
        //                    cuerpo = cuerpo.Replace("TIPO_EQUIPO_INICIAL", registros.TipoEquipoActualToString);

        //                    if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.PendienteAtencionADDM)
        //                        cuerpo = cuerpo.Replace("COMENTARIOS", registros.Comentarios);
        //                    else if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.Desestimado)
        //                        cuerpo = cuerpo.Replace("COMENTARIOS", registros.ComentariosDesestimacion);
        //                    else if (registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.AprobadoADDM || registros.EstadoSolicitud == (int)EstadoSolicitudActivosTSI.RechazadoADDM)
        //                        cuerpo = cuerpo.Replace("COMENTARIOS", registros.ComentariosAprobacionRechazo);

        //                    cuerpo = cuerpo.Replace("SOLICITANTE", registros.NombreUsuarioCreacion);
        //                    cuerpo = cuerpo.Replace("TIPO_EQUIPO", registros.TipoEquipoActualToString);
        //                    cuerpo = cuerpo.Replace("EQUIPO", registros.NombreEquipo);

        //                    if (!registros.FlagRegistroEquipo.HasValue)
        //                    {
        //                        cuerpo = cuerpo.Replace("[ACCION]", "cambio de equipo a");
        //                        asunto = asunto.Replace("[ACCION]", "cambio de equipo a");
        //                    }
        //                    else
        //                    {
        //                        if (registros.FlagRegistroEquipo.Value)
        //                        {
        //                            cuerpo = cuerpo.Replace("[ACCION]", "registro de");
        //                            asunto = asunto.Replace("[ACCION]", "registro de");
        //                        }
        //                        else
        //                        {
        //                            cuerpo = cuerpo.Replace("[ACCION]", "cambio de equipo a");
        //                            asunto = asunto.Replace("[ACCION]", "cambio de equipo a");
        //                        }
        //                    }

        //                    if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.RegistroSolicitudTSI)
        //                    {
        //                        var para = notificacion.Para.Split(';').ToList();
        //                        var cc = notificacion.ConCopia.Split(';').ToList();
        //                        cc.Add(registros.CorreoSolicitante);

        //                        emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
        //                    }
        //                    else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.DesestimacionSolicitudTSI)
        //                    {
        //                        var para = notificacion.Para.Split(';').ToList();
        //                        var cc = notificacion.ConCopia.Split(';').ToList();
        //                        cc.Add(registros.CorreoSolicitante);

        //                        emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
        //                    }
        //                    else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.AprobacionSolicitudTSI)
        //                    {
        //                        var para = new List<string>() { registros.CorreoSolicitante };
        //                        var cc = notificacion.ConCopia.Split(';').ToList();
        //                        cc.Add(registros.CorreoSolicitante);

        //                        emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
        //                    }
        //                    else if (notificacion.TipoNotificacionId == (int)ETipoNotificacion.RechazoSolicitudTSI)
        //                    {
        //                        var para = new List<string>() { registros.CorreoSolicitante };
        //                        var cc = notificacion.ConCopia.Split(';').ToList();
        //                        cc.Add(registros.CorreoSolicitante);

        //                        emailService.ProcesarEnvioNotificacionTSI(notificacion.BuzonSalida, para, cc, cuerpo, asunto);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        log.ErrorEntity(ex);
        //        throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
        //            , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
        //            , new object[] { null });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error ", ex);
        //        throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
        //            , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
        //            , new object[] { null });
        //    }
        //}
        public void EnviarCorreoAppliance(string notification_type, int id_solicitud, string owner)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;
                    var emailService = new Email.MailingManager();

                    var dataEquipoSolicitud = ServiceManager<ApplianceEquipoDAO>.Provider.GetEquipoSolicitudXId(id_solicitud);
                    var oTipoNotificacion = ServiceManager<ApplianceEquipoDAO>.Provider.GetTipoNotificacion(notification_type); //ctx.TipoNotificacion.FirstOrDefault(x => x.TipoNotificacionId == tipoNotificacion);

                    if (!ReferenceEquals(null, oTipoNotificacion)
                        && !ReferenceEquals(null, dataEquipoSolicitud))
                    {
                        var body = oTipoNotificacion.Cuerpo;
                        var affair = oTipoNotificacion.Asunto;

                        body = body.Replace("FECHA_APROBACION", dataEquipoSolicitud.FechaAprobacionRechazoToString);
                        body = body.Replace("TIPO_EQUIPO_FINAL", dataEquipoSolicitud.TipoEquipoSolicitadoToString);
                        body = body.Replace("TIPO_EQUIPO_INICIAL", dataEquipoSolicitud.TipoEquipoActualToString);

                        #region Estado de solicitud - Comentarios
                        switch (dataEquipoSolicitud.EstadoSolicitud)
                        {
                            case ((int)EstadoSolicitudActivosTSI.PendienteAtencionOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.Comentarios);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.Desestimado):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosDesestimacion);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.AprobadoCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoOwner):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.RechazadoCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.ComentariosAprobacionRechazo);
                                break;
                            case ((int)EstadoSolicitudActivosTSI.PendienteAtencionCVT):
                                body = body.Replace("COMENTARIOS", dataEquipoSolicitud.Comentarios);
                                break;
                        }
                        #endregion

                        body = body.Replace("SOLICITANTE", dataEquipoSolicitud.NombreUsuarioCreacion);
                        body = body.Replace("TIPO_EQUIPO", dataEquipoSolicitud.TipoEquipoActualToString);
                        body = body.Replace("EQUIPO", dataEquipoSolicitud.NombreEquipo);

                        if (!dataEquipoSolicitud.FlagRegistroEquipo.HasValue)
                        {
                            body = body.Replace("[ACCION]", "cambio de equipo a");
                            affair = affair.Replace("[ACCION]", "cambio de equipo a");
                        }
                        else
                        {
                            if (dataEquipoSolicitud.FlagRegistroEquipo.Value)
                            {
                                body = body.Replace("[ACCION]", "registro de");
                                affair = affair.Replace("[ACCION]", "registro de");
                            }
                            else
                            {
                                body = body.Replace("[ACCION]", "cambio de equipo a");
                                affair = affair.Replace("[ACCION]", "cambio de equipo a");
                            }
                        }

                        if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI)
                        {
                            var para = new List<string>() { owner.ToString() };
                            var cc = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_DESESTIMACION_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { owner.ToString() };
                            var cc = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_OWNER)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = new List<string>() { owner.ToString() };
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_CVT)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = oTipoNotificacion.ConCopia.Split(';').ToList();
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_CVT)
                        {
                            var para = new List<string>() { dataEquipoSolicitud.CorreoSolicitante };
                            var cc = oTipoNotificacion.ConCopia.Split(';').ToList();
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                        else if (oTipoNotificacion.Nombre == Utilitarios.NOTIFICACION_REGISTRO_SOLICITUD_PENDIENTE_CVT)
                        {
                            var para = oTipoNotificacion.Para.Split(';').ToList();
                            var cc = oTipoNotificacion.ConCopia.Split(';').ToList();
                            emailService.ProcesarEnvioNotificacionTSI(oTipoNotificacion.BuzonSalida, para, cc, body, affair);
                        }
                    }
                }
            }
        }
        public override List<CustomAutocomplete> GetEquipoByFiltroActivo(string filtro, int idTipoEquipo)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Equipo
                                       where (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                       && u.FlagActivo == true
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre,
                                           Suscripcion = u.Suscripcion
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override int ExisteEquipoApplianceByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Equipo
                                    where u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                    select u).FirstOrDefault();
                    if (registro == null)
                        return 0;
                    else
                    {
                        if (registro.TipoEquipoId == (int)ETipoEquipo.Appliance && registro.FlagActivo == false)
                            return 1;
                        else if (registro.TipoEquipoId == (int)ETipoEquipo.Appliance && registro.FlagActivo == true)
                            return 2;
                        else if (registro.TipoEquipoId == (int)ETipoEquipo.Servidor && registro.FlagActivo == true)
                            return 3;
                        else if (registro.TipoEquipoId == (int)ETipoEquipo.Servidor && registro.FlagActivo == false)
                            return 4;
                        else
                            return 5;
                    }

                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override List<CertificadoDigitalDTO> GetCertificadosDigitalesByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                int mesesTipoCicloVida = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_CLICLOVIDA").Valor);
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;

                var lista = new List<CertificadoDigitalDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_certificado_digital_detalleaplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacionStr", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CertificadoDigitalDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = 0,
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoCertificado = reader.IsDBNull(reader.GetOrdinal("TipoCertificadoCD")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoCertificadoCD")),
                                DescripcionTipoCertificado = reader.IsDBNull(reader.GetOrdinal("DescripcionTipoCD")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionTipoCD")),
                                FechaCreacionCertificado = reader.IsDBNull(reader.GetOrdinal("FechaCreadoCD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreadoCD")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                FactoresKPI_FLooking = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW")),
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }

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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetCertificadosDigitalesByCodigoAPT()"
                    , new object[] { null });
            }
        }

        public override List<EquipoServidorVirtualDTO> GetEquipoServidorVirtual(int id, int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var idSubdominio = 0;
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;


                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<EquipoServidorVirtualDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquipoServidorVirtual]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", idSubdominio));
                        comando.Parameters.Add(new SqlParameter("@equipoId", id));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoServidorVirtualDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CantidadRelaciones = reader.IsDBNull(reader.GetOrdinal("CantidadRelaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadRelaciones"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoServidorVirtualDTO> GetEquipoServidorVirtual(int id, int pageNumber, int pageSize, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoServidorVirtualMasDTO> GetEquipoServidorVirtualDetalle(int id, int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<EquipoServidorVirtualMasDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerEquipoServidorVirtual_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", id));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoServidorVirtualMasDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                OwnerEmail = reader.IsDBNull(reader.GetOrdinal("OwnerEmail")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerEmail")),
                                Experto = reader.IsDBNull(reader.GetOrdinal("Experto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto")),
                                ExpertoEmail = reader.IsDBNull(reader.GetOrdinal("ExpertoEmail")) ? string.Empty : reader.GetString(reader.GetOrdinal("ExpertoEmail"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoServidorVirtualDetalleDTO> GetEquipoServidorVirtualDetalle(int id, int pageNumber, int pageSize, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoServidorVirtualDTO> GetReportEquipoServidorVirtual(int id)
        {
            try
            {
                var idSubdominio = 0;
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;


                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<EquipoServidorVirtualDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Reporte_EquipoServidorVirtual]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominioSO", idSubdominio));
                        comando.Parameters.Add(new SqlParameter("@equipoId", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoServidorVirtualDTO()
                            {
                                EquipoFisico = reader.IsDBNull(reader.GetOrdinal("EquipoFisico")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoFisico")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Equipovirtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipovirtual")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                OwnerEmail = reader.IsDBNull(reader.GetOrdinal("OwnerEmail")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerEmail")),
                                Experto = reader.IsDBNull(reader.GetOrdinal("Experto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto")),
                                ExpertoEmail = reader.IsDBNull(reader.GetOrdinal("ExpertoEmail")) ? string.Empty : reader.GetString(reader.GetOrdinal("ExpertoEmail"))

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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoServidorVirtualDTO> GetReportEquipoServidorVirtual(int id)"
                    , new object[] { null });
            }
        }
    }
}
