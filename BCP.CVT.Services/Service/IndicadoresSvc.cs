using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BCP.CVT.Services.Service
{
    public class IndicadoresSvc : IndicadoresDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override IndicadoresGerencialEquipoData GetReporteIndicadoresGerencialEquipos(FiltrosIndicadoresGerencialEquipo filtros, bool flagTotales = false)
        {
            try
            {
                IndicadoresGerencialEquipoData dashboardBase = new IndicadoresGerencialEquipoData();

                var cadenaConexion = Constantes.CadenaConexion;



                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {

                    #region DATA FECHA ACTUAL
                    cnx.Open();
                    var dataPieActual = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialEquipoPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoEquipo", string.Join("|", filtros.TipoEquipoFiltro)));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", filtros.SubdominiosFiltroBase == null ? "" : string.Join("|", filtros.SubdominiosFiltroBase)));
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologias", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrSubsidiarias", filtros.SubsidiariasFiltro == null ? "" : string.Join("|", filtros.SubsidiariasFiltro)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataPieActual.Add(objeto);
                        }
                        if (flagTotales)
                        {
                            reader.NextResult();

                            while (reader.Read())
                            {
                                if (reader.GetBoolean(reader.GetOrdinal("FlagTemporal")))
                                {
                                    dashboardBase.TotalEquiposDescubrimientoManual = reader.GetInt32(reader.GetOrdinal("Total"));
                                }
                                else
                                {
                                    dashboardBase.TotalEquiposDescubrimientoAutomatico = reader.GetInt32(reader.GetOrdinal("Total"));
                                }

                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                dashboardBase.TotalEquiposHuerfanos = reader.GetInt32(reader.GetOrdinal("TotalEquiposHuerfanos"));
                            }
                        }
                        reader.Close();
                    }

                    dashboardBase.DataActualPie = dataPieActual;
                    #endregion

                    #region DATA N MESES ATRAS
                    var dataMesAnteriores = new List<CustomAutocomplete>();
                    var fechaConsultaMesAnterior = filtros.FechaConsultaFiltro.AddMonths(-1 * filtros.NroMesesFiltro);
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialEquipoPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoEquipo", string.Join("|", filtros.TipoEquipoFiltro)));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsultaMesAnterior));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", filtros.SubdominiosFiltroBase == null ? "" : string.Join("|", filtros.SubdominiosFiltroBase)));
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologias", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrSubsidiarias", filtros.SubsidiariasFiltro == null ? "" : string.Join("|", filtros.SubsidiariasFiltro)));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataMesAnteriores.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataMesesAtrasPie = dataMesAnteriores;
                    #endregion

                    #region DATA SUBDOMINIOS FECHA ACTUAL
                    cnx.Open();
                    var dataSubdominiosPieActual = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialEquipoSubdominioPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoEquipo", string.Join("|", filtros.TipoEquipoFiltro)));
                        //comando.Parameters.Add(new SqlParameter("@nroSubdominios", filtros.NroSubdominiosFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", string.Join("|", filtros.SubdominiosFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologias", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrSubsidiarias", filtros.SubsidiariasFiltro == null ? "" : string.Join("|", filtros.SubsidiariasFiltro)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                TipoId = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? "0" : reader.GetInt32(reader.GetOrdinal("SubdominioId")).ToString(),
                                TipoDescripcion = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? "" : reader.GetString(reader.GetOrdinal("Subdominio")),
                                value = reader.IsDBNull(reader.GetOrdinal("SubdominioOrden")) ? "0" : reader.GetInt64(reader.GetOrdinal("SubdominioOrden")).ToString(),
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataSubdominiosPieActual.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataActualSubdominiosPie = dataSubdominiosPieActual;
                    #endregion

                    #region DATA SUBDOMINIOS N MESES ATRAS
                    cnx.Open();

                    var dataSubdominiosMesesAnteriores = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialEquipoSubdominioPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoEquipo", string.Join("|", filtros.TipoEquipoFiltro)));
                        //comando.Parameters.Add(new SqlParameter("@nroSubdominios", filtros.NroSubdominiosFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsultaMesAnterior));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", string.Join("|", filtros.SubdominiosFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologias", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@arrSubsidiarias", filtros.SubsidiariasFiltro == null ? "" : string.Join("|", filtros.SubsidiariasFiltro)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                TipoId = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? "0" : reader.GetInt32(reader.GetOrdinal("SubdominioId")).ToString(),
                                TipoDescripcion = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? "" : reader.GetString(reader.GetOrdinal("Subdominio")),
                                value = reader.IsDBNull(reader.GetOrdinal("SubdominioOrden")) ? "0" : reader.GetInt64(reader.GetOrdinal("SubdominioOrden")).ToString(),
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataSubdominiosMesesAnteriores.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataaMesesAtrasSubdominiosPie = dataSubdominiosMesesAnteriores;
                    #endregion

                    return dashboardBase;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialEquipos(FiltrosIndicadoresGerencialEquipo filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialEquipos(FiltrosIndicadoresGerencialEquipo filtros)"
                    , new object[] { null });
            }
        }

        public override IndicadoresGerencialTecnologiaData GetReporteIndicadoresGerencialTecnologias(FiltrosIndicadoresGerencialTecnologia filtros, bool flagTotales)
        {
            try
            {
                IndicadoresGerencialTecnologiaData dashboardBase = new IndicadoresGerencialTecnologiaData();

                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {

                    #region DATA FECHA ACTUAL
                    cnx.Open();
                    var dataPieActual = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialTecnologiaPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologia", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataPieActual.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataActualPie = dataPieActual;
                    #endregion

                    #region DATA N MESES ATRAS
                    var dataMesAnteriores = new List<CustomAutocomplete>();
                    var fechaConsultaMesAnterior = filtros.FechaConsultaFiltro.AddMonths(-1 * filtros.NroMesesFiltro);
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialTecnologiaPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoTecnologia", string.Join("|", filtros.TipoTecnologiaFiltro)));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataMesAnteriores.Add(objeto);
                        }

                        if (flagTotales)
                        {
                            dashboardBase.DataTipoTecnologia = new List<TecnologiasXTipoEquipo>();
                            reader.NextResult();

                            while (reader.Read())
                            {
                                var objeto = new TecnologiasXTipoEquipo()
                                {
                                    TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                    TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? "" : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                    TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                    TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? "" : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                    Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                };
                                dashboardBase.DataTipoTecnologia.Add(objeto);

                            }

                        }


                        reader.Close();
                    }

                    dashboardBase.DataMesesAtrasPie = dataMesAnteriores;
                    #endregion

                    return dashboardBase;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialTecnologias(FiltrosIndicadoresGerencialTecnologia filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialTecnologias(FiltrosIndicadoresGerencialTecnologia filtros)"
                    , new object[] { null });
            }

        }

        public override IndicadoresGerencialAplicacionData GetReporteIndicadoresGerencialAplicaciones(FiltrosIndicadoresGerencialAplicacion filtros, bool flagTotales)
        {
            try
            {
                IndicadoresGerencialAplicacionData dashboardBase = new IndicadoresGerencialAplicacionData();

                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    #region DATA TOTAL APLICACIONES
                    var dataTotalAplicaciones = new List<CustomRetorno>();
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadorGerencialTotalAplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomRetorno()
                            {
                                Estado = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? "" : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TotalEstado = reader.IsDBNull(reader.GetOrdinal("TotalEstado")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEstado")),
                                TotalAplicacion = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones"))
                            };
                            dataTotalAplicaciones.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataTotalAplicaciones = dataTotalAplicaciones;

                    #endregion


                    #region DATA FECHA ACTUAL
                    cnx.Open();
                    var dataPieActual = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialAplicacionPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrEstadoAplicacion", string.Join("|", filtros.EstadoAplicacionFiltro)));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", filtros.JefeEquipoFiltrar));
                        comando.Parameters.Add(new SqlParameter("@liderUsuario", filtros.LiderUsuarioFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gestionadoPor", filtros.GestionadoPorFiltro == "-1" ? "" : filtros.GestionadoPorFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderTTL", filtros.LiderTTLFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gerencia", filtros.GerenciaFiltrar));
                        comando.Parameters.Add(new SqlParameter("@division", filtros.DivisionFiltrar));
                        comando.Parameters.Add(new SqlParameter("@unidad", filtros.UnidadFiltrar));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", filtros.SubdominiosFiltro == null ? "" : string.Join("|", filtros.SubdominiosFiltro)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataPieActual.Add(objeto);
                        }


                        if (flagTotales)
                        {
                            reader.NextResult();

                            while (reader.Read())
                            {
                                if (reader.GetString(reader.GetOrdinal("Tipo")) == "Sin Aplicaciones")
                                {
                                    dashboardBase.TotalAplicacionesSinRelaciones = reader.GetInt32(reader.GetOrdinal("Total"));
                                }
                                else
                                {
                                    dashboardBase.TotalAplicacionesConRelaciones = reader.GetInt32(reader.GetOrdinal("Total"));
                                }

                            }

                        }

                        reader.Close();
                    }

                    dashboardBase.DataActualPie = dataPieActual;
                    #endregion

                    #region DATA N MESES ATRAS
                    var dataMesAnteriores = new List<CustomAutocomplete>();
                    var fechaConsultaMesAnterior = filtros.FechaConsultaFiltro.AddMonths(-1 * filtros.NroMesesFiltro);
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialAplicacionPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrEstadoAplicacion", string.Join("|", filtros.EstadoAplicacionFiltro)));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", filtros.JefeEquipoFiltrar));
                        comando.Parameters.Add(new SqlParameter("@liderUsuario", filtros.LiderUsuarioFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gestionadoPor", filtros.GestionadoPorFiltro == "-1" ? "" : filtros.GestionadoPorFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderTTL", filtros.LiderTTLFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gerencia", filtros.GerenciaFiltrar));
                        comando.Parameters.Add(new SqlParameter("@division", filtros.DivisionFiltrar));
                        comando.Parameters.Add(new SqlParameter("@unidad", filtros.UnidadFiltrar));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsultaMesAnterior));
                        comando.Parameters.Add(new SqlParameter("@arrSubdominios", filtros.SubdominiosFiltro == null ? "" : string.Join("|", filtros.SubdominiosFiltro)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataMesAnteriores.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataMesesAtrasPie = dataMesAnteriores;
                    #endregion



                    #region DATA SUBDOMINIOS FECHA ACTUAL
                    cnx.Open();
                    var dataSubdominiosPieActual = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialAplicacionSubdominiosPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrEstadoAplicacion", string.Join("|", filtros.EstadoAplicacionFiltro)));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", filtros.JefeEquipoFiltrar));
                        comando.Parameters.Add(new SqlParameter("@liderUsuario", filtros.LiderUsuarioFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gestionadoPor", filtros.GestionadoPorFiltro == "-1" ? "" : filtros.GestionadoPorFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderTTL", filtros.LiderTTLFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gerencia", filtros.GerenciaFiltrar));
                        comando.Parameters.Add(new SqlParameter("@division", filtros.DivisionFiltrar));
                        comando.Parameters.Add(new SqlParameter("@unidad", filtros.UnidadFiltrar));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", filtros.FechaConsultaFiltro));
                        comando.Parameters.Add(new SqlParameter("@nroSubdominios", filtros.NroSubdominiosFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                TipoId = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? "0" : reader.GetInt32(reader.GetOrdinal("SubdominioId")).ToString(),
                                TipoDescripcion = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? "" : reader.GetString(reader.GetOrdinal("Subdominio")),
                                value = reader.IsDBNull(reader.GetOrdinal("SubdominioOrden")) ? "0" : reader.GetInt64(reader.GetOrdinal("SubdominioOrden")).ToString(),
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataSubdominiosPieActual.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataActualSubdominiosPie = dataSubdominiosPieActual;
                    #endregion

                    #region DATA SUBDOMINIOS N MESES ATRAS
                    cnx.Open();

                    var dataSubdominiosMesesAnteriores = new List<CustomAutocomplete>();
                    using (var comando = new SqlCommand("[cvt].[USP_IndicadoresGerencialAplicacionSubdominiosPie_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrEstadoAplicacion", string.Join("|", filtros.EstadoAplicacionFiltro)));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", filtros.JefeEquipoFiltrar));
                        comando.Parameters.Add(new SqlParameter("@liderUsuario", filtros.LiderUsuarioFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gestionadoPor", filtros.GestionadoPorFiltro == "-1" ? "" : filtros.GestionadoPorFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderTTL", filtros.LiderTTLFiltrar));
                        comando.Parameters.Add(new SqlParameter("@gerencia", filtros.GerenciaFiltrar));
                        comando.Parameters.Add(new SqlParameter("@division", filtros.DivisionFiltrar));
                        comando.Parameters.Add(new SqlParameter("@unidad", filtros.UnidadFiltrar));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsultaMesAnterior));
                        comando.Parameters.Add(new SqlParameter("@nroSubdominios", filtros.NroSubdominiosFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                TipoId = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? "0" : reader.GetInt32(reader.GetOrdinal("SubdominioId")).ToString(),
                                TipoDescripcion = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? "" : reader.GetString(reader.GetOrdinal("Subdominio")),
                                value = reader.IsDBNull(reader.GetOrdinal("SubdominioOrden")) ? "0" : reader.GetInt64(reader.GetOrdinal("SubdominioOrden")).ToString(),
                                Id = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? string.Empty : reader.GetString(reader.GetOrdinal("Color")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            dataSubdominiosMesesAnteriores.Add(objeto);
                        }
                        reader.Close();
                    }

                    dashboardBase.DataaMesesAtrasSubdominiosPie = dataSubdominiosMesesAnteriores;
                    #endregion

                    return dashboardBase;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialAplicaciones(FiltrosIndicadoresGerencialEquipo filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteIndicadoresGerencialAplicaciones(FiltrosIndicadoresGerencialEquipo filtros)"
                    , new object[] { null });
            }
        }



    }
}
