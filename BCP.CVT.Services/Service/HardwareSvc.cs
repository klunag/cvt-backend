using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.Data.Entity.SqlServer;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Authenticators;
using System.Net;
using BCP.CVT.Services.Log;
using System.Globalization;

namespace BCP.CVT.Services.Service
{
    public class HardwareSvc : HardwareDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override FiltrosHardware GetFiltros_Detallado()
        {
            try
            {
                FiltrosHardware arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;
                    arr_data = new FiltrosHardware();

                    arr_data.GestionadoPor = (from u in ctx.GestionadoPor
                                              where u.FlagActivo 
                                              select new CustomAutocomplete()
                                              {
                                                  Id = u.GestionadoPorId.ToString(),
                                                  Descripcion = u.Nombre
                                              }).OrderBy(x => x.Descripcion).ToList();

                    arr_data.TeamSquad = (from u in ctx.TeamSquad
                                          where u.FlagActivo
                                          select new CustomAutocomplete()
                                          {
                                              Id = u.EquipoId.ToString(),
                                              Descripcion = u.Nombre,
                                              TipoId = u.GestionadoPorId.ToString()
                                          }).OrderBy(x => x.Descripcion).ToList();

                    arr_data.Modelo = ServiceManager<ModeloDAO>.Provider.ModeloHardware_Combo();

                    arr_data.Fabricante = ServiceManager<ModeloDAO>.Provider.FabricanteHardware_Combo();

                    arr_data.TipoHardware = (from u in ctx.TipoEquipo
                                             where u.FlagActivo && u.FlagIncluirHardwareKPI
                                             select new CustomAutocomplete()
                                             {
                                                 Id = u.TipoEquipoId.ToString(),
                                                 Descripcion = u.Nombre
                                             }).OrderBy(x => x.Descripcion).ToList();

                    var listaEstadoObsolescenciaHardware = Utilitarios.EnumToList<EstadoObsolescenciaHardware>();
                    arr_data.EstadoObsolescencia = listaEstadoObsolescenciaHardware.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).OrderBy(x => x.Descripcion).ToList();
                    
                    arr_data.UnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();



                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipoByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Equipo
                                       join f in ctx.TipoEquipo on u.TipoEquipoId equals f.TipoEquipoId
                                       where (!u.FlagExcluirCalculo.HasValue || !u.FlagExcluirCalculo.Value)
                                       && f.FlagIncluirHardwareKPI
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

        public override List<ReporteEquipoHardwareDTO> GetReporteDetallado(PaginaHardwareDetallado pag, out int totalRows)
        {
            pag.UnidadFondeo = (pag.UnidadFondeo == "-1" || pag.UnidadFondeo == null) ? "" : pag.UnidadFondeo;
            pag.GestionadoPor = (pag.GestionadoPor == "-1" || pag.GestionadoPor == null) ? "" : pag.GestionadoPor;
            pag.TeamSquad = (pag.TeamSquad == "-1" || pag.TeamSquad == null) ? "" : pag.TeamSquad;
            pag.Fabricante = (pag.Fabricante == "-1" || pag.Fabricante == null) ? "" : pag.Fabricante;
            pag.Modelo = (pag.Modelo == "-1" || pag.Modelo == null) ? "" : pag.Modelo;
            //pag.EstadoObsolescencia = (pag.EstadoObsolescencia == "-1" || pag.EstadoObsolescencia == null) ? "" : pag.EstadoObsolescencia;
            pag.TipoHardware = (pag.TipoHardware == "-1" || pag.TipoHardware == null) ? "" : pag.TipoHardware;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                
                var lista = new List<ReporteEquipoHardwareDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_EquipoHardware_Detallado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@UnidadFondeo", pag.UnidadFondeo));
                        comando.Parameters.Add(new SqlParameter("@GestionadoPor", pag.GestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@TeamSquad", pag.TeamSquad));
                        comando.Parameters.Add(new SqlParameter("@EquipoId", pag.EquipoId));
                        comando.Parameters.Add(new SqlParameter("@Fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@Modelo", pag.Modelo));
                        comando.Parameters.Add(new SqlParameter("@EstadoObsolescencia", pag.EstadoObsolescencia));
                        comando.Parameters.Add(new SqlParameter("@TipoHardware", pag.TipoHardware));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@Matricula", pag.Matricula));
                        comando.Parameters.Add(new SqlParameter("@PerfilId", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteEquipoHardwareDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Modelo")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                TeamSquad = reader.IsDBNull(reader.GetOrdinal("TeamSquad")) ? string.Empty : reader.GetString(reader.GetOrdinal("TeamSquad")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                EstadoActual = reader.IsDBNull(reader.GetOrdinal("EstadoActual")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoActual")),
                                Proyeccion1 = reader.IsDBNull(reader.GetOrdinal("Proyeccion1")) ? 0 : reader.GetInt32(reader.GetOrdinal("Proyeccion1")),
                                Proyeccion2 = reader.IsDBNull(reader.GetOrdinal("Proyeccion2")) ? 0 : reader.GetInt32(reader.GetOrdinal("Proyeccion2")),


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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override FiltrosHardware GetFiltros_KPI()
        {
            try
            {
                FiltrosHardware arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;
                    arr_data = new FiltrosHardware();

                    arr_data.GestionadoPor = (from u in ctx.GestionadoPor
                                              where u.FlagActivo
                                              select new CustomAutocomplete()
                                              {
                                                  Id = u.GestionadoPorId.ToString(),
                                                  Descripcion = u.Nombre
                                              }).OrderBy(x => x.Descripcion).ToList();

                    arr_data.TeamSquad = (from u in ctx.TeamSquad
                                          where u.FlagActivo
                                          select new CustomAutocomplete()
                                          {
                                              Id = u.EquipoId.ToString(),
                                              Descripcion = u.Nombre,
                                              TipoId = u.GestionadoPorId.ToString()
                                          }).OrderBy(x => x.Descripcion).ToList();

                    arr_data.Modelo = ServiceManager<ModeloDAO>.Provider.ModeloHardware_Combo();

                    arr_data.Fabricante = ServiceManager<ModeloDAO>.Provider.FabricanteHardware_Combo();

                    //arr_data.TipoHardware = (from u in ctx.TipoEquipo
                    //                         where u.FlagActivo && u.FlagIncluirHardwareKPI
                    //                         select new CustomAutocomplete()
                    //                         {
                    //                             Id = u.TipoEquipoId.ToString(),
                    //                             Descripcion = u.Nombre
                    //                         }).OrderBy(x => x.Descripcion).ToList();

                    //var listaEstadoObsolescenciaHardware = Utilitarios.EnumToList<EstadoObsolescenciaHardware>();
                    //arr_data.EstadoObsolescencia = listaEstadoObsolescenciaHardware.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).OrderBy(x => x.Descripcion).ToList();

                    //arr_data.UnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();

                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override List<ReporteHardwareKpiDTO> GetReporteKPI(string matricula, int perfilId, string UnidadFondeoIds, string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds, int? nivel, string filtrosPadre)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteHardwareKpiDTO>();
                string nombreSP = "[cvt].[USP_EquipoHardware_KPI]";

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@matricula", ((object)matricula) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@UnidadFondeoIds", ((object)UnidadFondeoIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@GestionadoPorIds", ((object)GestionadoPorIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@TeamSquadIds", ((object)TeamSquadIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@FabricanteIds", ((object)FabricanteIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ModeloIds", ((object)ModeloIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@nivel", ((object)nivel) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@filtrosPadre", ((object)filtrosPadre) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteHardwareKpiDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                UnidadFondeoParent = reader.GetData<string>("UnidadFondeoParent"),
                                UnidadFondeoId = reader.GetData<string>("UnidadFondeoId"),
                                UnidadFondeo = reader.GetData<string>("UnidadFondeo"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                Vence12KPI = reader.GetData<int>("Vence12KPI"),
                                PorcentajeVence12KPI = reader.GetData<decimal>("PorcentajeVence12KPI"),
                                Vence24KPI = reader.GetData<int>("Vence24KPI"),
                                PorcentajeVence24KPI = reader.GetData<decimal>("PorcentajeVence24KPI"),
                                //Vence24KPICorto = reader.GetData<int>("Vence24KPICorto"),
                                //PorcentajeVence24KPICorto = reader.GetData<decimal>("PorcentajeVence24KPICorto"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                TotalKPI = reader.GetData<int>("Total"),
                                PorcentajeKPIFlooking = reader.GetData<decimal>("PorcentajeKPIFlooking"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ReporteHardwareKpiDTO> GetReporteKPI_Exportar(string matricula, int perfilId, string UnidadFondeoIds, string GestionadoPorIds, string TeamSquadIds, string FabricanteIds, string ModeloIds)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteHardwareKpiDTO>();
                string nombreSP = "[cvt].[USP_EquipoHardware_KPI_Exportar]";

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@matricula", ((object)matricula) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@UnidadFondeoIds", ((object)UnidadFondeoIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@GestionadoPorIds", ((object)GestionadoPorIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@TeamSquadIds", ((object)TeamSquadIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@FabricanteIds", ((object)FabricanteIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ModeloIds", ((object)ModeloIds) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteHardwareKpiDTO()
                            {
                                
                                UnidadFondeo = reader.GetData<string>("UnidadFondeo"),
                                GestionadoPor = reader.GetData<string>("GestionadoPor"),
                                TeamSquad = reader.GetData<string>("TeamSquad"),
                                Modelo = reader.GetData<string>("Modelo"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                Vence12KPI = reader.GetData<int>("Vence12KPI"),
                                PorcentajeVence12KPI = reader.GetData<decimal>("PorcentajeVence12KPI"),
                                Vence24KPI = reader.GetData<int>("Vence24KPI"),
                                PorcentajeVence24KPI = reader.GetData<decimal>("PorcentajeVence24KPI"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                TotalKPI = reader.GetData<int>("Total"),
                                PorcentajeKPIFlooking = reader.GetData<decimal>("PorcentajeKPIFlooking"),
                                PorcentajeKPI = reader.GetData<decimal>("PorcentajeKPI"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }
    }
}

