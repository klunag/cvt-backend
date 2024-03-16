using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
using BCP.CVT.Services.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BCP.CVT.Services.Service
{
    public class ReportePortafolioSvc : ReportePortafolioDAO
    {
        

        public override List<InstanciasDto> GetInstancias(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("NombreReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreReporte")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }


        public override List<InstanciasDto> GetInstanciasGrafico(PaginacionEquipo pag)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Grafico]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@nombreGrafico", pag.Nombre));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                SemanaNro = reader.IsDBNull(reader.GetOrdinal("NroSemana")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroSemana")),
                                Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("NombreReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreReporte")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstanciasGrafico()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasEquipos(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Equipos]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.Nombre));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                Deprecado = reader.IsDBNull(reader.GetOrdinal("Deprecados")) ? 0 : reader.GetInt32(reader.GetOrdinal("Deprecados")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstanciasEquipos()"
                    , new object[] { null });
            }
        }


        public override List<InstanciasDto> GetInstanciasEquiposGrafico(PaginacionEquipo pag)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Equipos_Grafico]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.NombreTecnologia));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@nombreGrafico", pag.Nombre));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                SemanaNro = reader.IsDBNull(reader.GetOrdinal("NroSemana")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroSemana")),
                                Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstanciasEquiposGrafico()"
                    , new object[] { null });
            }
        }


        public override List<InstanciasDto> GetInstanciasEquiposAplicaciones(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Aplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                AreaBIAN = reader.IsDBNull(reader.GetOrdinal("AreaBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("AreaBIAN"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstanciasEquiposAplicaciones()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasEquiposAplicacionesGrafico(PaginacionEquipo pag)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Aplicaciones_Grafico]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.NombreTecnologia));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@nombreGrafico", pag.Nombre));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                SemanaNro = reader.IsDBNull(reader.GetOrdinal("NroSemana")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroSemana")),
                                Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                AreaBIAN = reader.IsDBNull(reader.GetOrdinal("AreaBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("AreaBIAN")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstanciasEquiposAplicacionesGrafico()"
                    , new object[] { null });
            }
        }



        public override List<InstanciasDto> GetInstanciasFamilias(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista4]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("NombreReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreReporte")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstanciasFamilias()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasFamiliasAplicaciones(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista4_Aplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.Nombre));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                AreaBIAN = reader.IsDBNull(reader.GetOrdinal("AreaBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("AreaBIAN"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstanciasFamiliasAplicaciones()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasFamiliasProductos(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista4_Familia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                NombreProducto = reader.IsDBNull(reader.GetOrdinal("NombreProducto")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreProducto")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4"))
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
                    , "Error en el metodo: GetInstanciasFamiliasProductos()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasFamiliasTecnologias(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista4_Tecnologias]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.Fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.Nombre));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                Deprecado = reader.IsDBNull(reader.GetOrdinal("Deprecados")) ? 0 : reader.GetInt32(reader.GetOrdinal("Deprecados")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstanciasFamiliasTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasProductos(PaginacionEquipo pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Familia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                NombreProducto = reader.IsDBNull(reader.GetOrdinal("NombreProducto")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreProducto")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("0")) ? 0 : reader.GetInt32(reader.GetOrdinal("0")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("1")) ? 0 : reader.GetInt32(reader.GetOrdinal("1")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("2")) ? 0 : reader.GetInt32(reader.GetOrdinal("2")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("3")) ? 0 : reader.GetInt32(reader.GetOrdinal("3")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("4")) ? 0 : reader.GetInt32(reader.GetOrdinal("4")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstanciasProductos()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetInstanciasProductosGrafico(PaginacionEquipo pag)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista3_Familia_Grafico]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@apps", pag.AppsId));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.Subdominio));
                        comando.Parameters.Add(new SqlParameter("@orden", pag.Orden));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@nombreGrafico", pag.Nombre));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                SemanaNro = reader.IsDBNull(reader.GetOrdinal("NroSemana")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroSemana")),
                                Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                NombreProducto = reader.IsDBNull(reader.GetOrdinal("NombreProducto")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreProducto")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas"))
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
                    , "Error en el metodo: GetInstanciasGrafico()"
                    , new object[] { null });
            }
        }


        public override List<ReporteAlertasDto> GetReporteAlertas(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAlertasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_RelacionUserIT_Alertas]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAlertasDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Software = reader.IsDBNull(reader.GetOrdinal("Software")) ? string.Empty : reader.GetString(reader.GetOrdinal("Software")),
                                Servidor = reader.IsDBNull(reader.GetOrdinal("Servidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("Servidor")),
                                FechaEjecucion = reader.IsDBNull(reader.GetOrdinal("FechaEjecucion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaEjecucion")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();

                        if (lista.Count > 0)
                            totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAplicacionesInfraestructuraDesarrolloDetalle()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionReporteDto> GetReporteAplicaciones(string gerencia)
        {
            try
            {
                var listaRetorno = GetTotales(gerencia);
                if (listaRetorno != null)
                {
                    listaRetorno = GetTotalesInfraestructura(listaRetorno, gerencia);
                    listaRetorno = GetTotalesInfraestructuraDesarrollo(listaRetorno, gerencia);
                    listaRetorno = GetTotalesObsolescencia(listaRetorno, gerencia);
                }

                return listaRetorno;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionReporteDto> GetReporteAplicacionesBT(PaginacionAplicacion pag)
        {
            try
            {
                var listaRetorno = GetTotales(pag.Gerencia, pag.CapaFuncional);
                if (listaRetorno != null)
                {
                    listaRetorno = GetTotalesInfraestructura(listaRetorno, pag.Gerencia);
                    listaRetorno = GetTotalesInfraestructuraDesarrollo(listaRetorno, pag.Gerencia);
                    listaRetorno = GetTotalesObsolescencia(listaRetorno, pag.Gerencia);
                }

                var itemTotal = new AplicacionReporteDto
                {
                    //Categoria = "Total",
                    CapaFuncional = "Total",
                    TotalEnDesarrollo = listaRetorno.LastOrDefault().SumaTotalEnDesarrollo,
                    TotalVigentes = listaRetorno.LastOrDefault().SumaTotalVigentes,
                    AppsOnCloud = listaRetorno.LastOrDefault().SumaAppsOnCloud,
                    AppsOnPremise = listaRetorno.LastOrDefault().SumaAppsOnPremise,
                    AppsDesarrolladasOnCloud = listaRetorno.LastOrDefault().SumaAppsDesarrolladasOnCloud,
                    AppsDesarrolladasOnPremise = listaRetorno.LastOrDefault().SumaAppsDesarrolladasOnPremise,
                    AppsPaquetesOnPremise = listaRetorno.LastOrDefault().SumaAppsPaquetesOnPremise,
                    AppsPaquetesOnCloud = listaRetorno.LastOrDefault().SumaAppsPaquetesOnCloud,
                    AppsObsolescenciaOnPremise = listaRetorno.LastOrDefault().SumaAppsObsolescenciaOnPremise,
                    AppsObsolescenciaOnCloud = listaRetorno.LastOrDefault().SumaAppsObsolescenciaOnCloud
                };

                listaRetorno.Add(itemTotal);

                return listaRetorno;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionReporteDto> GetExportarAplicacionesBT(string management, string functionalLayer, string sortName, string sortOrder)
        {
            try
            {
                if (string.IsNullOrEmpty(functionalLayer))
                    functionalLayer = string.Empty;

                if (string.IsNullOrEmpty(management))
                    management = string.Empty;

                var listaRetorno = GetTotales(management, functionalLayer);
                if (!string.IsNullOrEmpty(listaRetorno.ToString()))
                {
                    listaRetorno = GetTotalesInfraestructura(listaRetorno, management);
                    listaRetorno = GetTotalesInfraestructuraDesarrollo(listaRetorno, management);
                    listaRetorno = GetTotalesObsolescencia(listaRetorno, management);
                }

                var itemTotal = new AplicacionReporteDto
                {
                    //Categoria = "Total",
                    CapaFuncional = "Total",
                    TotalEnDesarrollo = listaRetorno.LastOrDefault().SumaTotalEnDesarrollo,
                    TotalVigentes = listaRetorno.LastOrDefault().SumaTotalVigentes,
                    AppsOnCloud = listaRetorno.LastOrDefault().SumaAppsOnCloud,
                    AppsOnPremise = listaRetorno.LastOrDefault().SumaAppsOnPremise,
                    AppsDesarrolladasOnCloud = listaRetorno.LastOrDefault().SumaAppsDesarrolladasOnCloud,
                    AppsDesarrolladasOnPremise = listaRetorno.LastOrDefault().SumaAppsDesarrolladasOnPremise,
                    AppsPaquetesOnPremise = listaRetorno.LastOrDefault().SumaAppsPaquetesOnPremise,
                    AppsPaquetesOnCloud = listaRetorno.LastOrDefault().SumaAppsPaquetesOnCloud,
                    AppsObsolescenciaOnPremise = listaRetorno.LastOrDefault().SumaAppsObsolescenciaOnPremise,
                    AppsObsolescenciaOnCloud = listaRetorno.LastOrDefault().SumaAppsObsolescenciaOnCloud
                };

                listaRetorno.Add(itemTotal);

                return listaRetorno;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAplicacionDto> GetReporteAplicacionesInfraestructuraDesarrolloDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesInfraestructuraDesarrollo_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencias", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@clasificacion", pag.Clasificacion));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoInfraestructura));
                        comando.Parameters.Add(new SqlParameter("@desarrollo", pag.TipoDesarrollo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                //ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        totalRows = lista.Count > 0 ? lista.FirstOrDefault().Total : 0;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAplicacionesInfraestructuraDesarrolloDetalle()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAplicacionDto> GetReporteAplicacionesInfraestructuraDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesInfraestructura_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencias", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@infraestructuraFiltro", pag.Infraestructura));
                        comando.Parameters.Add(new SqlParameter("@clasificacionFiltro", pag.Clasificacion));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                //ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        totalRows = lista.Count > 0 ? lista.FirstOrDefault().Total : 0;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAplicacionDto> GetReporteAplicacionesObsolescenciaDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesObsolescencia_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencias", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@clasificacion", pag.Clasificacion));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoInfraestructura));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                //ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();

                        totalRows = lista.Count > 0 ? lista.FirstOrDefault().Total : 0;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAplicacionesObsolescenciaDetalle()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAplicacionDto> GetReporteAplicacionesTotalDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_Totales_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencias", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@estadoFiltro", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@clasificacionFiltro", pag.Clasificacion));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                //ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        totalRows = lista.Count > 0 ? lista.FirstOrDefault().Total : 0;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<AreaBianDto> GetReporteAreaBian(string gerencia, string estado)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AreaBianDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.Parameters.Add(new SqlParameter("@gerencias", gerencia));
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AreaBianDto()
                            {
                                Categoria = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                BankingProducts = reader.IsDBNull(reader.GetOrdinal("BANKING PRODUCTS")) ? 0 : reader.GetInt32(reader.GetOrdinal("BANKING PRODUCTS")),
                                Bice = reader.IsDBNull(reader.GetOrdinal("BICE")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE")),
                                BiceMayoristaTesoreria = reader.IsDBNull(reader.GetOrdinal("BICE - MAYORISTA Y TESORERÍA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE - MAYORISTA Y TESORERÍA")),
                                BiceMinorista = reader.IsDBNull(reader.GetOrdinal("BICE - MINORISTA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE - MINORISTA")),
                                BusinessDirection = reader.IsDBNull(reader.GetOrdinal("BUSINESS DIRECTION")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS DIRECTION")),
                                BusinessDirectionTesoreria = reader.IsDBNull(reader.GetOrdinal("BUSINESS DIRECTION TESORERIA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS DIRECTION TESORERIA")),
                                BusinessIntegration = reader.IsDBNull(reader.GetOrdinal("BUSINESS INTEGRATION")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS INTEGRATION")),
                                BusinessSupport = reader.IsDBNull(reader.GetOrdinal("BUSINESS SUPPORT")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS SUPPORT"))
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
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<AreaBianDto> GetReporteAreaBianBT(PaginacionAplicacion pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AreaBianDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencias", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AreaBianDto()
                            {
                                Categoria = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                BankingProducts = reader.IsDBNull(reader.GetOrdinal("BANKING PRODUCTS")) ? 0 : reader.GetInt32(reader.GetOrdinal("BANKING PRODUCTS")),
                                Bice = reader.IsDBNull(reader.GetOrdinal("BICE")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE")),
                                BiceMayoristaTesoreria = reader.IsDBNull(reader.GetOrdinal("BICE - MAYORISTA Y TESORERÍA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE - MAYORISTA Y TESORERÍA")),
                                BiceMinorista = reader.IsDBNull(reader.GetOrdinal("BICE - MINORISTA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE - MINORISTA")),
                                BusinessDirection = reader.IsDBNull(reader.GetOrdinal("BUSINESS DIRECTION")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS DIRECTION")),
                                BusinessDirectionTesoreria = reader.IsDBNull(reader.GetOrdinal("BUSINESS DIRECTION TESORERIA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS DIRECTION TESORERIA")),
                                BusinessIntegration = reader.IsDBNull(reader.GetOrdinal("BUSINESS INTEGRATION")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS INTEGRATION")),
                                BusinessSupport = reader.IsDBNull(reader.GetOrdinal("BUSINESS SUPPORT")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUSINESS SUPPORT")),
                                BiceMayorista = reader.IsDBNull(reader.GetOrdinal("BICE - MAYORISTA")) ? 0 : reader.GetInt32(reader.GetOrdinal("BICE - MAYORISTA"))
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
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAplicacionDto> GetReporteAreaBianDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista2_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerenciaFiltro", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@estadoFiltro", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@areaFiltro", pag.Area));
                        comando.Parameters.Add(new SqlParameter("@clasificacionFiltro", pag.Clasificacion));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                AreaBIAN = reader.IsDBNull(reader.GetOrdinal("AreaBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("AreaBIAN")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        totalRows = lista[0].Total;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteAreaBian()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetResumenAplicaciones(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DashboardAplicaciones_ResumenApps_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderusuario", pag.LiderUsuarioFiltro));
                        comando.Parameters.Add(new SqlParameter("@ttl", pag.TTLFiltro));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoFiltro));
                        comando.Parameters.Add(new SqlParameter("@codigoApt", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.Matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("NombreReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreReporte")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas")),
                                Deprecado = reader.IsDBNull(reader.GetOrdinal("Deprecados")) ? 0 : reader.GetInt32(reader.GetOrdinal("Deprecados"))
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
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<InstanciasDto> GetResumenAplicacionesDetalle(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<InstanciasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DashboardAplicaciones_ResumenAppsDetalle_V2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipoFiltro));
                        comando.Parameters.Add(new SqlParameter("@liderusuario", pag.LiderUsuarioFiltro));
                        comando.Parameters.Add(new SqlParameter("@ttl", pag.TTLFiltro));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoFiltro));
                        comando.Parameters.Add(new SqlParameter("@codigoApt", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.SubdominioFiltro));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.Matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InstanciasDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                TipoComponente = reader.IsDBNull(reader.GetOrdinal("TipoComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoComponente")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Ahora = reader.IsDBNull(reader.GetOrdinal("Ahora")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ahora")),
                                Meses12 = reader.IsDBNull(reader.GetOrdinal("12Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("12Meses")),
                                Meses24 = reader.IsDBNull(reader.GetOrdinal("24Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("24Meses")),
                                Meses36 = reader.IsDBNull(reader.GetOrdinal("36Meses")) ? 0 : reader.GetInt32(reader.GetOrdinal("36Meses")),
                                Mas36 = reader.IsDBNull(reader.GetOrdinal("36MesesMas")) ? 0 : reader.GetInt32(reader.GetOrdinal("36MesesMas")),
                                Deprecado = reader.IsDBNull(reader.GetOrdinal("Deprecados")) ? 0 : reader.GetInt32(reader.GetOrdinal("Deprecados"))
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
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override ReporteEstadoPortafolioDTO GetReporteEstadoPortafolio(FiltrosReporteEstadoPortafolio filtros)
        {
            var response = new ReporteEstadoPortafolioDTO();

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //Tabla 1 =>  "PARA EL FINAL" --> EN caso de que sus datos tiene que reflejar de la tabla
                    var lstReporteDataGerencia = (from a in ctx.Application
                                                  join b in ctx.Unidad on a.unit equals b.UnidadId into lj4
                                                  from b in lj4.DefaultIfEmpty()
                                                  join c in ctx.Area on b.AreaId equals c.AreaId into lj5
                                                  from c in lj5.DefaultIfEmpty()
                                                  join d in ctx.Division on c.DivisionId equals d.DivisionId into lj6
                                                  from d in lj6.DefaultIfEmpty()
                                                  join e in ctx.Gerencia on d.GerenciaId equals e.GerenciaId into lj7 //!e.FlagEliminado.Value && e.FlagActivo
                                                  from e in lj7.DefaultIfEmpty()
                                                  join f in ctx.TipoActivoInformacion on a.assetType equals f.TipoActivoInformacionId into lj3
                                                  from f in lj3.DefaultIfEmpty()
                                                  join g in ctx.ParametricaDetalle on a.technologyCategory equals g.ParametricaDetalleId into lj1
                                                  from g in lj1.DefaultIfEmpty()
                                                      //validar si debe ir
                                                  join h in ctx.GestionadoPor on a.managed equals h.GestionadoPorId into lj2
                                                  from h in lj2.DefaultIfEmpty()
                                                  where 1 == 1
                                                  && (!filtros.ListEstadoAplicacion.Any() || filtros.ListEstadoAplicacion.Contains(a.status ?? 0))
                                                  && (!filtros.ListTipoActivo.Any() || filtros.ListTipoActivo.Contains(a.assetType ?? 0))
                                                  //&& (filtros.GerenciaId < 0 || filtros.GerenciaId == e.GerenciaId)
                                                  //&& (filtros.DivisionId < 0 || filtros.DivisionId == d.DivisionId)
                                                  //&& (filtros.AreaId < 0 || filtros.AreaId == c.AreaId)
                                                  && a.isActive == true
                                                  //&& a.isApproved == true
                                                  select new ReporteEstadoPortafolio()
                                                  {
                                                      ApplicationId = a.applicationId,
                                                      NombreAplicacion = a.applicationName,
                                                      TipoActivo = f == null ? "" : f.Nombre,
                                                      Estado = a.status ?? 0,
                                                      GerenciaId = e == null ? 0 : e.GerenciaId,
                                                      DivisionId = d == null ? 0 : d.DivisionId,
                                                      AreaId = c == null ? 0 : c.AreaId,
                                                      UnidadId = b == null ? 0 : b.UnidadId,
                                                      Gerencia = e == null ? "" : e.Nombre,
                                                      Division = d == null ? "" : d.Nombre,
                                                      Area = c == null ? "" : c.Nombre,
                                                      Unidad = b == null ? "" : b.Nombre,
                                                      CriticidadBIA = a.finalCriticality ?? 0,
                                                      Categoria = g == null ? "" : g.Valor,
                                                      //NivelCumplimiento = a.complianceLevel.Value
                                                      NivelCumplimiento = h == null ? string.Empty : !h.FlagUserIT.HasValue ? string.Empty : h.FlagUserIT.Value == false ? "No Aplica" : a.complianceLevel.ToString()
                                                  });

                    var lstReporte = (from a in lstReporteDataGerencia
                                      where (filtros.GerenciaId < 0 || filtros.GerenciaId == a.GerenciaId)
                                      && (filtros.DivisionId < 0 || filtros.DivisionId == a.DivisionId)
                                      && (filtros.AreaId < 0 || filtros.AreaId == a.AreaId)
                                      select a).ToList();

                    if (filtros.FlgChange)
                    {
                        //Grafico 1 => Pie Donut
                        response.AplicacionesBancarias = lstReporteDataGerencia.GroupBy(x => x.Gerencia, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                        response.AplicacionesBancarias = response.AplicacionesBancarias.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "SIN GERENCIA" : a.Grupo; return a; }).ToList();

                        //Grafico 2 => Barras
                        if (filtros.AreaId > 0) //Unidades
                        {
                            response.AplicacionesXJerarquia = lstReporte.GroupBy(x => x.Unidad, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                            response.AplicacionesXJerarquia = response.AplicacionesXJerarquia.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "SIN UNIDAD" : a.Grupo; return a; }).ToList();

                        }
                        else if (filtros.DivisionId > 0) //Areas
                        {
                            response.AplicacionesXJerarquia = lstReporte.GroupBy(x => x.Area, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                            response.AplicacionesXJerarquia = response.AplicacionesXJerarquia.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "SIN ÁREA" : a.Grupo; return a; }).ToList();
                        }
                        else if (filtros.GerenciaId > 0) //Divisiones
                        {
                            response.AplicacionesXJerarquia = lstReporte.GroupBy(x => x.Division, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                            response.AplicacionesXJerarquia = response.AplicacionesXJerarquia.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "SIN DIVISIÓN" : a.Grupo; return a; }).ToList();
                        }
                        else //Gerencias
                        {
                            response.AplicacionesXJerarquia = lstReporte.GroupBy(x => x.Gerencia, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                            response.AplicacionesXJerarquia = response.AplicacionesXJerarquia.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "SIN GERENCIA" : a.Grupo; return a; }).ToList();
                        }
                        response.AplicacionesXJerarquia = response.AplicacionesXJerarquia.OrderByDescending(x => x.Valor).ToList();


                        //Grafico 3 => Pie
                        response.AplicacionesXEstado = lstReporte.GroupBy(x => x.Estado, (key, g) => new ReporteEstadoPortafolio() { Estado = key, cantRegistros = g.Count() })
                                                                .Select(y => new ReportePortafolioGrafico() { Grupo = y.EstadoToStr, Valor = y.cantRegistros }).ToList();
                        response.AplicacionesXEstado = response.AplicacionesXEstado.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "Sin estado" : a.Grupo; return a; }).OrderByDescending(x => x.Valor).ToList();



                        //Grafico 4 => Barras
                        response.AplicacionesXCriticidad = lstReporte.GroupBy(x => x.CriticidadBIA, (key, g) => new ReporteEstadoPortafolio() { CriticidadBIA = key, cantRegistros = g.Count() })
                                                                .Select(y => new ReportePortafolioGrafico() { Grupo = y.CriticidadBIAToStr, Valor = y.cantRegistros }).ToList();
                        response.AplicacionesXCriticidad = response.AplicacionesXCriticidad.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "Sin criticidad" : a.Grupo; return a; }).OrderByDescending(x => x.Valor).ToList();



                        //Grafico 5
                        response.AplicacionesXCategoriaTecnologica = lstReporte.GroupBy(x => x.Categoria, (key, g) => new ReportePortafolioGrafico() { Grupo = key, Valor = g.Count() }).ToList();
                        response.AplicacionesXCategoriaTecnologica = response.AplicacionesXCategoriaTecnologica.Select(a => { a.Grupo = string.IsNullOrEmpty(a.Grupo) ? "Sin categoría" : a.Grupo; return a; }).OrderByDescending(x => x.Valor).ToList();

                        //Grafico 6 => Salud de Aplicaciones
                        response.SaludAplicacionesXUnidad = GetDataSaludAplications(filtros);

                    }

                    //Tabla
                    response.Total = lstReporte.Count();
                    response.Rows = lstReporte.Skip(filtros.pageSize * (filtros.pageNumber - 1)).Take(filtros.pageSize).ToList();

                    return response;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReporteEstadoPortafolioDTO GetReporteEstadoPortafolio(FiltrosReporteEstadoPortafolio filtros)"
                    , new object[] { null });
            }
        }

        public override List<ReporteEstadoPortafolio> GetDataSaludAplicationsExport(FiltrosReporteEstadoPortafolio filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteEstadoPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_Portafolio_ReporteEstadoXUnidades_Data]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.EstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@GerenciaId", filtros.GerenciaId));
                        comando.Parameters.Add(new SqlParameter("@DivisionId", filtros.DivisionId));
                        comando.Parameters.Add(new SqlParameter("@AreaId", filtros.AreaId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteEstadoPortafolio()
                            {
                                ApplicationId = reader.IsDBNull(reader.GetOrdinal("Codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Codigo")),
                                NombreAplicacion = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Estado")),
                                NivelCumplimiento = reader.IsDBNull(reader.GetOrdinal("NivelCumplimiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("NivelCumplimiento"))
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
                    , "Error en el metodo: List<ReportePortafolioGrafico> GetDataSaludAplicationsExport(FiltrosReporteEstadoPortafolio filtros)"
                    , new object[] { null });
            }
        }

        public override int GetNroPeriodos(string fechaBase, int frecuencia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                int nroPeriodosBD = 0;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("select [app].[UF_OBTENER_RANGO_CALCULO] (@fechaBase, getdate(), @frecuencia)", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.Text;
                        comando.Parameters.Add(new SqlParameter("@fechaBase", fechaBase));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", frecuencia));

                        nroPeriodosBD = int.Parse(comando.ExecuteScalar().ToString());
                    }
                    cnx.Close();
                }

                return nroPeriodosBD;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int GetNroPeriodos(string fechaBase, int frecuencia)"
                    , new object[] { null });
            }
        }

        #region ReporteCampos
        public override List<CustomAutocomplete> GetListAgrupamiento()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lstAgrupamiento = (from a in ctx.InfoCampoPortafolio
                                           where !a.FlagEliminado.Value && a.FlagActivo
                                           select new CustomAutocomplete()
                                           {
                                               Id = a.InfoCampoPortafolioId.ToString(),
                                               Descripcion = a.Nombre
                                           }).ToList();

                    return lstAgrupamiento;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetListAgrupamiento()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetListFiltros()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lstAgrupamiento = (from a in ctx.InfoCampoPortafolio
                                           where !a.FlagEliminado.Value && a.FlagActivo
                                           select new CustomAutocomplete()
                                           {
                                               Id = a.InfoCampoPortafolioId.ToString(),
                                               Descripcion = a.Nombre
                                           }).ToList();

                    return lstAgrupamiento;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetListFiltros()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetListAgrupadoPor()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lstAgrupamiento = (from a in ctx.InfoCampoPortafolio
                                           where !a.FlagEliminado.Value && a.FlagActivo
                                           orderby a.Nombre
                                           select new CustomAutocomplete()
                                           {
                                               Id = a.InfoCampoPortafolioId.ToString(),
                                               Descripcion = a.Nombre
                                           }).ToList();

                    return lstAgrupamiento;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetListAgrupadoPor()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetListValoresFiltros(int idTablaFiltrar)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = "[app].[USP_ReportePortafolioCampos_ValoresFiltrar]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tablaFiltrarId", idTablaFiltrar));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {

                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")).ToString(),
                                Descripcion = reader.GetString(reader.GetOrdinal("Nombre")),
                            };
                            result.Add(objeto);

                        }


                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetListValoresFiltros()"
                    , new object[] { null });
            }
        }

        public override DataTable ReportePortafolioCampos(FiltrosReporteCamposPortafolio filtros)
        {
            try
            {

                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@lstGerencia", filtros.ListGerencia, SqlDbType.VarChar));
                ListsQLParam.Add(new SQLParam("@lstDivision", filtros.ListDivision, SqlDbType.VarChar));
                ListsQLParam.Add(new SQLParam("@lstArea", filtros.ListArea, SqlDbType.VarChar));
                ListsQLParam.Add(new SQLParam("@lstUnidad", filtros.ListUnidad, SqlDbType.VarChar));
                if (filtros.FechaDesde == DateTime.MinValue)
                {
                    ListsQLParam.Add(new SQLParam("@fechaDesde", "", SqlDbType.VarChar));
                }
                else
                {
                    ListsQLParam.Add(new SQLParam("@fechaDesde", filtros.FechaDesde, SqlDbType.Date));

                }
                if (filtros.FechaHasta == DateTime.MinValue)
                {
                    ListsQLParam.Add(new SQLParam("@fechaHasta", "", SqlDbType.VarChar));
                }
                else
                {
                    ListsQLParam.Add(new SQLParam("@fechaHasta", filtros.FechaHasta, SqlDbType.Date));

                }
                ListsQLParam.Add(new SQLParam("@frecuencia", filtros.Frecuencia, SqlDbType.Int));
                ListsQLParam.Add(new SQLParam("@nroPeriodos", filtros.NroPeriodos, SqlDbType.Int));
                ListsQLParam.Add(new SQLParam("@agruparPor", filtros.AgruparPor, SqlDbType.Int));
                ListsQLParam.Add(new SQLParam("@agruparPorValor", filtros.AgruparPorValores, SqlDbType.VarChar));
                ListsQLParam.Add(new SQLParam("@filtrarPor", filtros.FiltrarPor, SqlDbType.Int));
                ListsQLParam.Add(new SQLParam("@filtrarPorValor", filtros.FiltrarPorValores, SqlDbType.VarChar));

                DataTable data = new SQLManager().GetDataTable("[app].[USP_ReportePortafolioCampos_Grafico]", ListsQLParam);
                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo:  ReportePortafolioCampos(FiltrosReporteCamposPortafolio filtros)"
                    , new object[] { null });
            }
        }

        public override List<ReporteCamposPortafolio> ReportePortafolioCampos_Data(FiltrosReporteCamposPortafolio filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new List<ReporteCamposPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_ReportePortafolioCampos_Grafico_Data]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                        comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                        comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                        comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));

                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }

                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                        }
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.NroPeriodos));
                        comando.Parameters.Add(new SqlParameter("@agruparPor", filtros.AgruparPor));
                        comando.Parameters.Add(new SqlParameter("@agruparPorValor", filtros.AgruparPorValores));
                        comando.Parameters.Add(new SqlParameter("@filtrarPor", filtros.FiltrarPor));
                        comando.Parameters.Add(new SqlParameter("@filtrarPorValor", filtros.FiltrarPorValores));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new ReporteCamposPortafolio()
                            {
                                Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                ApplicationId = reader.IsDBNull(reader.GetOrdinal("ApplicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ApplicationId")),
                                ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                ColumnaAgruparId = reader.IsDBNull(reader.GetOrdinal("ColumnaAgruparId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ColumnaAgruparId")),
                                ColumnaAgruparValor = reader.IsDBNull(reader.GetOrdinal("ColumnaAgruparValor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ColumnaAgruparValor")),
                                ColumnaFiltroId = reader.IsDBNull(reader.GetOrdinal("ColumnaFiltroId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ColumnaFiltroId")),
                                ColumnaFiltroValor = reader.IsDBNull(reader.GetOrdinal("ColumnaFiltroValor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ColumnaFiltroValor")),
                                Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                FechaFreeze = reader.IsDBNull(reader.GetOrdinal("FechaFreeze")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFreeze")),
                                FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),

                            };
                            result.Add(objeto);

                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo:List<ReporteCamposPortafolio> ReportePortafolioCampos_Data(FiltrosReporteCamposPortafolio filtros)"
                    , new object[] { null });
            }
        }

        #endregion

        #region Reporte de Variacion

        public override ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioVariacionDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesCreadas_Data]" : "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesCreadas]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.ListEstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }

                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));
                        }
                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.nroPeriodos));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                            while (reader.Read())
                            {
                                if (!(reader.IsDBNull(reader.GetOrdinal("ApplicationId"))))
                                {
                                    var objeto = new ReporteVariacionPortafolioSolicitudes()
                                    {
                                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? "" : reader.GetString(reader.GetOrdinal("Tipo")),
                                        FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                        FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                        FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),
                                        ApplicationId = reader.GetString(reader.GetOrdinal("ApplicationId")),
                                        AplicacionNombre = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? "" : reader.GetString(reader.GetOrdinal("applicationName")),
                                        FechaMostrar = reader.IsDBNull(reader.GetOrdinal("FechaMostrar")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaMostrar")),
                                        TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? "" : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                        GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? "" : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                        EstadoNombre = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),

                                    };
                                    lista.Add(objeto);
                                }
                            }
                            result.SolicitudesData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha"));

                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    FechaStr = fecha.HasValue ? fecha.Value.ToString(filtros.FormatoFecha) : string.Empty,
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                                };
                                lista.Add(objeto);
                            }
                            result.Solicitudes = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion(FiltrosReporteVariacionPortafolio filtros)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioVariacionDTO();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesEliminadas_Data]" : "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesEliminadas]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.ListEstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }

                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));
                        }
                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.nroPeriodos));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                            while (reader.Read())
                            {
                                if (!(reader.IsDBNull(reader.GetOrdinal("ApplicationId"))))
                                {
                                    var objeto = new ReporteVariacionPortafolioSolicitudes()
                                    {
                                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? "" : reader.GetString(reader.GetOrdinal("Tipo")),
                                        FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                        FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                        FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),
                                        ApplicationId = reader.GetString(reader.GetOrdinal("ApplicationId")),
                                        AplicacionNombre = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? "" : reader.GetString(reader.GetOrdinal("applicationName")),
                                        FechaMostrar = reader.IsDBNull(reader.GetOrdinal("FechaMostrar")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaMostrar")),
                                        TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? "" : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                        GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? "" : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                        EstadoNombre = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),

                                    };
                                    lista.Add(objeto);
                                }
                            }
                            result.SolicitudesData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha"));
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    FechaStr = fecha.HasValue ? fecha.Value.ToString(filtros.FormatoFecha) : string.Empty,
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                                };

                                lista.Add(objeto);
                            }
                            result.Solicitudes = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioVariacionDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesCreadasEliminadas_Data]" : "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesCreadasEliminadas]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.ListEstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }


                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));
                        }

                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.nroPeriodos));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                            while (reader.Read())
                            {
                                if (!(reader.IsDBNull(reader.GetOrdinal("ApplicationId"))))
                                {
                                    var objeto = new ReporteVariacionPortafolioSolicitudes()
                                    {
                                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? "" : reader.GetString(reader.GetOrdinal("Tipo")),
                                        FrecuenciaNombre = reader.IsDBNull(reader.GetOrdinal("FrecuenciaNombre")) ? "" : reader.GetString(reader.GetOrdinal("FrecuenciaNombre")),
                                        FrecuenciaDias = reader.IsDBNull(reader.GetOrdinal("FrecuanciaDias")) ? 0 : reader.GetInt32(reader.GetOrdinal("FrecuanciaDias")),
                                        FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                        ApplicationId = reader.GetString(reader.GetOrdinal("ApplicationId")),
                                        AplicacionNombre = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? "" : reader.GetString(reader.GetOrdinal("applicationName")),
                                        FechaMostrar = reader.IsDBNull(reader.GetOrdinal("FechaMostrar")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaMostrar")),
                                        TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? "" : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                        GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? "" : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                        EstadoNombre = reader.IsDBNull(reader.GetOrdinal("Estado")) ? "" : reader.GetString(reader.GetOrdinal("Estado")),

                                    };
                                    lista.Add(objeto);
                                }
                            }
                            result.SolicitudesData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    Valor = reader.IsDBNull(reader.GetOrdinal("SolicitudesCreadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("SolicitudesCreadas")),
                                    Valor2 = reader.IsDBNull(reader.GetOrdinal("SolicitudesEliminadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("SolicitudesEliminadas"))
                                };
                                lista.Add(objeto);
                            }
                            result.Solicitudes = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioVariacionDTO GetReportePortafolioVariacion_Estados(FiltrosReporteVariacionPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioVariacionDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesxEstado_Data]" : "[dbo].[USP_Portafolio_ReporteVariacion_SolicitudesxEstado]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.ListEstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }

                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));
                        }

                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.nroPeriodos));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                            while (reader.Read())
                            {
                                if (!(reader.IsDBNull(reader.GetOrdinal("ApplicationId"))))
                                {
                                    var objeto = new ReporteVariacionPortafolioSolicitudes()
                                    {
                                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? "" : reader.GetString(reader.GetOrdinal("Tipo")),
                                        FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                        FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),
                                        ApplicationId = reader.GetString(reader.GetOrdinal("ApplicationId")),
                                        EstadoCodigo = reader.IsDBNull(reader.GetOrdinal("EstadoCodigo")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoCodigo")),
                                        EstadoNombre = reader.IsDBNull(reader.GetOrdinal("EstadoNombre")) ? "" : reader.GetString(reader.GetOrdinal("EstadoNombre")),
                                        AplicacionNombre = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? "" : reader.GetString(reader.GetOrdinal("applicationName")),
                                        TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? "" : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                        GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? "" : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                        FreezeDate = reader.IsDBNull(reader.GetOrdinal("FreezeDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FreezeDate")),

                                    };
                                    lista.Add(objeto);
                                }
                            }
                            result.SolicitudesData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();

                            while (reader.Read())
                            {
                                var fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha"));

                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    FechaStr = fecha.HasValue ? fecha.Value.ToString(filtros.FormatoFecha) : "",
                                    Valor = reader.IsDBNull(reader.GetOrdinal("En Desarrollo")) ? 0 : reader.GetInt32(reader.GetOrdinal("En Desarrollo")),
                                    Valor2 = reader.IsDBNull(reader.GetOrdinal("Vigente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vigente")),
                                    Valor3 = reader.IsDBNull(reader.GetOrdinal("No Vigente")) ? 0 : reader.GetInt32(reader.GetOrdinal("No Vigente")),
                                    Valor4 = reader.IsDBNull(reader.GetOrdinal("Eliminada")) ? 0 : reader.GetInt32(reader.GetOrdinal("Eliminada"))
                                };
                                lista.Add(objeto);
                            }
                            result.Solicitudes = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioVariacionDTO GetReportePortafolioVariacion_DistrXGerencia(FiltrosReporteVariacionPortafolio filtros, bool getData = false, int idGerencia = 0)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioVariacionDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReporteVariacion_DistribucionXGerencia_Data]" : "[dbo].[USP_Portafolio_ReporteVariacion_DistribucionXGerencia]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.ListEstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@frecuencia", filtros.Frecuencia));
                        if (filtros.FechaDesde == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        }

                        if (filtros.FechaHasta == DateTime.MinValue)
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", ""));
                        }
                        else
                        {
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));
                        }

                        comando.Parameters.Add(new SqlParameter("@nroPeriodos", filtros.nroPeriodos));
                        comando.Parameters.Add(new SqlParameter("@GerenciaId", idGerencia));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                            while (reader.Read())
                            {
                                if (!(reader.IsDBNull(reader.GetOrdinal("ApplicationId"))))
                                {
                                    var objeto = new ReporteVariacionPortafolioSolicitudes()
                                    {
                                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                        Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                        FreezeDate = reader.IsDBNull(reader.GetOrdinal("FreezeDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FreezeDate")),
                                        ApplicationId = reader.GetString(reader.GetOrdinal("ApplicationId")),
                                        AplicacionNombre = reader.GetString(reader.GetOrdinal("ApplicationName")),
                                        TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? "" : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                        GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? "" : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                        CriticidadNombre = reader.IsDBNull(reader.GetOrdinal("CriticidadNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("CriticidadNombre")),
                                        EstadoNombre = reader.IsDBNull(reader.GetOrdinal("EstadoNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoNombre")),
                                        FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                        FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),

                                    };
                                    lista.Add(objeto);
                                }
                            }
                            result.SolicitudesData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();

                            while (reader.Read())
                            {
                                var fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha"));

                                var objeto = new ReportePortafolioGrafico()
                                {
                                    Grupo = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    FechaStr = fecha.HasValue ? fecha.Value.ToString(filtros.FormatoFecha) : string.Empty,
                                    GrupoId = reader.IsDBNull(reader.GetOrdinal("GerenciaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("GerenciaId")),
                                    Valor = reader.IsDBNull(reader.GetOrdinal("Baja")) ? 0 : reader.GetInt32(reader.GetOrdinal("Baja")),
                                    Valor2 = reader.IsDBNull(reader.GetOrdinal("Media")) ? 0 : reader.GetInt32(reader.GetOrdinal("Media")),
                                    Valor3 = reader.IsDBNull(reader.GetOrdinal("Alta")) ? 0 : reader.GetInt32(reader.GetOrdinal("Alta")),
                                    Valor4 = reader.IsDBNull(reader.GetOrdinal("Muy Alta")) ? 0 : reader.GetInt32(reader.GetOrdinal("Muy Alta")),
                                    Valor5 = reader.IsDBNull(reader.GetOrdinal("Sin criticidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("Sin criticidad"))

                                };
                                lista.Add(objeto);
                            }
                            result.Solicitudes = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }
        public override DateTime? ObtenerFechaBaseReporte(DateTime fechaConsulta, int nroPeriodos, int frecuencia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;


                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = String.Format("SELECT * FROM [app].[UF_LISTA_PERIODOS]('{0}',{1},{2})", fechaConsulta, frecuencia, nroPeriodos);
                    DateTime? fechaBase = null;
                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.Text;


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        var lista = new List<ReporteVariacionPortafolioSolicitudes>();
                        if (reader.Read())
                        {
                            fechaBase = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde"));
                        }


                        reader.Close();
                    }

                    return fechaBase;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioVariacionDTO GetReportePortafolioVariacion_SolicitudesCreadasEliminadas(FiltrosReporteVariacionPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        #endregion

        #region Reporte de Pedidos

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionAcumulada(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_EstadoAtencion_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_EstadoAtencion]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                        comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                        comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                        comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                        comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                    Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))
                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    Grupo = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                                };
                                lista.Add(objeto);
                            }
                            result.ChartReport = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionAcumulada(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }
        public override ReporteCambioBajaResponsablesGDHDTO GetReporteCambioBajaResponsableGDH(FiltrosReporteCambioBajaResponsableGDH filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var response = new ReporteCambioBajaResponsablesGDHDTO();
                var result = new List<ReporteCambioBajaResponsablesGDH>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = "[app].[USP_Portafolio_ReporteCambioBajaResponsableGDH_Data]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaInicio", filtros.fechaInicio));
                        comando.Parameters.Add(new SqlParameter("@fechaFin", filtros.fechaFin));
                        comando.Parameters.Add(new SqlParameter("@matricula", filtros.matricula));
                        comando.Parameters.Add(new SqlParameter("@IdAplicacion", filtros.IdAplicacion));
                        comando.Parameters.Add(new SqlParameter("@tipoCambio", filtros.tipoCambio));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {

                            var objeto = new ReporteCambioBajaResponsablesGDH()
                            {
                                ApplicationId = reader.IsDBNull(reader.GetOrdinal("ID_PORTAFOLIO_APLICACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("ID_PORTAFOLIO_APLICACION")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("DES_UNIDAD")) ? string.Empty : reader.GetString(reader.GetOrdinal("DES_UNIDAD")),
                                UnidadAnterior = reader.IsDBNull(reader.GetOrdinal("DES_UNIDAD_ANTERIOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("DES_UNIDAD_ANTERIOR")),
                                Puesto = reader.IsDBNull(reader.GetOrdinal("DES_PUESTO")) ? string.Empty : reader.GetString(reader.GetOrdinal("DES_PUESTO")),
                                PuestoAnterior = reader.IsDBNull(reader.GetOrdinal("DES_PUESTO_ANTERIOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("DES_PUESTO_ANTERIOR")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("COD_MATRICULA")) ? string.Empty : reader.GetString(reader.GetOrdinal("COD_MATRICULA")),
                                NombreResponsable = reader.IsDBNull(reader.GetOrdinal("NOMBRE_COMPLETO")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOMBRE_COMPLETO")),
                                NombreRol = reader.IsDBNull(reader.GetOrdinal("NOMBRE_ROL")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOMBRE_ROL")),
                                TipoCambio = reader.IsDBNull(reader.GetOrdinal("TIPO_CAMBIO")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIPO_CAMBIO")),
                                FechaReporte = reader.IsDBNull(reader.GetOrdinal("FEC_REPORTE")) ? string.Empty : reader.GetString(reader.GetOrdinal("FEC_REPORTE")),
                            };
                            result.Add(objeto);

                        }
                        reader.Close();
                    }
                    response.Total = result.Count();
                    response.Rows = result.Skip(filtros.pageSize * (filtros.pageNumber - 1)).Take(filtros.pageSize).ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReporteCambioBajaResponsablesGDHDTO GetReporteCambioBajaResponsableGDH(FiltrosReporteCambioBajaResponsableGDH filtros)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_Consultas(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_Consultas_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_Consultas]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                        comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                        comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                        comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                        comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    EstadoId = reader.IsDBNull(reader.GetOrdinal("TipoConsultaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoConsultaId")),
                                    Estado = reader.IsDBNull(reader.GetOrdinal("TipoConsulta")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoConsulta")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))
                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    Grupo = reader.IsDBNull(reader.GetOrdinal("TipoConsulta")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoConsulta")),
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                                };
                                lista.Add(objeto);
                            }
                            result.ChartReport = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionAcumulada(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }


        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionPeriodo(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_EstadoAtencionXTiempo_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_EstadoAtencionXTiempo]";

                    if (getData)
                    {
                        using (var comando = new SqlCommand(sql, cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                            comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                            comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                            comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                            comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                            comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                            var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                    Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))
                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;

                            reader.Close();
                        }
                    }
                    else
                    {
                        List<SQLParam> ListsQLParam = new List<SQLParam>();

                        ListsQLParam.Add(new SQLParam("@lstTipoActivo", filtros.ListTipoActivo, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGestionadoPor", filtros.ListGestionadoPor, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGerencia", filtros.ListGerencia, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstDivision", filtros.ListDivision, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstArea", filtros.ListArea, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstUnidad", filtros.ListUnidad, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@fechaDesde", filtros.FechaDesde, SqlDbType.Date));
                        ListsQLParam.Add(new SQLParam("@fechaHasta", filtros.FechaHasta, SqlDbType.Date));

                        result.Data = new SQLManager().GetDataTable(sql, ListsQLParam);
                    }


                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_DistrByTipoAtencionPeriodo(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_ConsultasPortafolio(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_ConsultasPortafolioData]" : "[dbo].[USP_Portafolio_ReportePedidos_ConsultasPortafolio]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                        comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                        comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                        comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                        comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("FechaIngreso")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                };
                                lista.Add(objeto);
                            }
                            result.ChartReportData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var fecha = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde"));
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),

                                    Grupo = fecha.HasValue ? fecha.Value.ToString("yyyy-MM-dd") : string.Empty,
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
                                };
                                lista.Add(objeto);
                            }
                            result.ChartReport = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReportePortafolioPedido_ConsultasPortafolio(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_CamposMasRequeridos(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_CamposMasActualizados_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_CamposMasActualizados]";

                    if (getData)
                    {
                        using (var comando = new SqlCommand(sql, cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                            comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                            comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                            comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                            comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                            comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                            var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha")),
                                    Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("usuario")),
                                    ColumnaId = reader.IsDBNull(reader.GetOrdinal("columnaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("columnaId")),
                                    ColumnaNombre = reader.IsDBNull(reader.GetOrdinal("columnaNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("columnaNombre")),
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("aplicacionId")) ? string.Empty : reader.GetString(reader.GetOrdinal("aplicacionId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("aplicacionNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("aplicacionNombre")),
                                    ValorAnterior = reader.IsDBNull(reader.GetOrdinal("ValorAnterior")) ? string.Empty : reader.GetString(reader.GetOrdinal("ValorAnterior")),
                                    NuevoValor = reader.IsDBNull(reader.GetOrdinal("NuevoValor")) ? string.Empty : reader.GetString(reader.GetOrdinal("NuevoValor")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))

                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;


                            reader.Close();
                        }
                    }
                    else
                    {
                        List<SQLParam> ListsQLParam = new List<SQLParam>();

                        ListsQLParam.Add(new SQLParam("@lstTipoActivo", filtros.ListTipoActivo, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGestionadoPor", filtros.ListGestionadoPor, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGerencia", filtros.ListGerencia, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstDivision", filtros.ListDivision, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstArea", filtros.ListArea, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstUnidad", filtros.ListUnidad, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@fechaDesde", filtros.FechaDesde, SqlDbType.Date));
                        ListsQLParam.Add(new SQLParam("@fechaHasta", filtros.FechaHasta, SqlDbType.Date));

                        result.Data = new SQLManager().GetDataTable(sql, ListsQLParam);
                    }


                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_CamposMasRequeridos(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_SLA(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_SLA_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_SLA]";
                    if (getData)
                    {
                        using (var comando = new SqlCommand(sql, cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                            comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                            comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                            comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                            comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                            comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                            comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                            comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                            var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    FechaDesde = reader.IsDBNull(reader.GetOrdinal("FechaDesde")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDesde")),
                                    FechaHasta = reader.IsDBNull(reader.GetOrdinal("FechaHasta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHasta")),
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("ApplicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ApplicationId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                    RolDesc = reader.IsDBNull(reader.GetOrdinal("Rol")) ? string.Empty : reader.GetString(reader.GetOrdinal("Rol")),
                                    FechaCreacion = reader.IsDBNull(reader.GetOrdinal("dateCreation")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateCreation")),
                                    FechaRechazada = reader.IsDBNull(reader.GetOrdinal("dateRejected")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateRejected")),
                                    FechaTransferencia = reader.IsDBNull(reader.GetOrdinal("dateTransfer")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateTransfer")),
                                    FechaAprobacion = reader.IsDBNull(reader.GetOrdinal("DateApproved")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateApproved")),
                                    Dias = reader.IsDBNull(reader.GetOrdinal("Dias")) ? 0 : reader.GetInt32(reader.GetOrdinal("Dias")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))
                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;


                            reader.Close();
                        }
                    }
                    else
                    {
                        List<SQLParam> ListsQLParam = new List<SQLParam>();

                        ListsQLParam.Add(new SQLParam("@lstTipoActivo", filtros.ListTipoActivo, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGestionadoPor", filtros.ListGestionadoPor, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstGerencia", filtros.ListGerencia, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstDivision", filtros.ListDivision, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstArea", filtros.ListArea, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@lstUnidad", filtros.ListUnidad, SqlDbType.VarChar));
                        ListsQLParam.Add(new SQLParam("@fechaDesde", filtros.FechaDesde, SqlDbType.Date));
                        ListsQLParam.Add(new SQLParam("@fechaHasta", filtros.FechaHasta, SqlDbType.Date));

                        result.Data = new SQLManager().GetDataTable(sql, ListsQLParam);
                    }


                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_SLA(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }

        public override ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_RegistroAPP(FiltrosReportePedidosPortafolio filtros, bool getData = false)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var result = new ReportePortafolioDTO<ReportePedidosPortafolio>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    string sql = getData ? "[dbo].[USP_Portafolio_ReportePedidos_RegistroAPP_Data]" : "[dbo].[USP_Portafolio_ReportePedidos_RegistroAPP]";

                    using (var comando = new SqlCommand(sql, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.ListTipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstGestionadoPor", filtros.ListGestionadoPor));
                        comando.Parameters.Add(new SqlParameter("@lstGerencia", filtros.ListGerencia));
                        comando.Parameters.Add(new SqlParameter("@lstDivision", filtros.ListDivision));
                        comando.Parameters.Add(new SqlParameter("@lstArea", filtros.ListArea));
                        comando.Parameters.Add(new SqlParameter("@lstUnidad", filtros.ListUnidad));
                        comando.Parameters.Add(new SqlParameter("@fechaDesde", filtros.FechaDesde));
                        comando.Parameters.Add(new SqlParameter("@fechaHasta", filtros.FechaHasta));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        if (getData)
                        {
                            var lista = new List<ReportePedidosPortafolio>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePedidosPortafolio()
                                {
                                    ApplicationId = reader.IsDBNull(reader.GetOrdinal("ApplicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ApplicationId")),
                                    ApplicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("RegisterDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RegisterDate")),
                                    FechaRegistroSituacion = reader.IsDBNull(reader.GetOrdinal("DateRegistrationSituationComplete")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateRegistrationSituationComplete")),
                                    FechaAprobacion = reader.IsDBNull(reader.GetOrdinal("DateApproved")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateApproved")),
                                    Dias = reader.IsDBNull(reader.GetOrdinal("Dias_SituacionCompleta")) ? 0 : reader.GetInt32(reader.GetOrdinal("Dias_SituacionCompleta")),
                                    DiasAprobado = reader.IsDBNull(reader.GetOrdinal("Dias_Aprobado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Dias_Aprobado")),
                                    Gerencia = reader.IsDBNull(reader.GetOrdinal("Gerencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gerencia")),
                                    Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                    Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                    TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                    GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                    Solicitante = reader.IsDBNull(reader.GetOrdinal("Solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Solicitante"))
                                };
                                lista.Add(objeto);

                            }
                            result.ChartReportData = lista;
                        }
                        else
                        {
                            var lista = new List<ReportePortafolioGrafico>();
                            while (reader.Read())
                            {
                                var objeto = new ReportePortafolioGrafico()
                                {
                                    FechaDescripcion = reader.IsDBNull(reader.GetOrdinal("FechaDescripcion")) ? "" : reader.GetString(reader.GetOrdinal("FechaDescripcion")),
                                    Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    Valor = reader.IsDBNull(reader.GetOrdinal("SolicitudesCreadas")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SolicitudesCreadas")),
                                    Valor2 = reader.IsDBNull(reader.GetOrdinal("SolicitudesEliminadas")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SolicitudesEliminadas"))
                                };
                                lista.Add(objeto);
                            }
                            result.ChartReport = lista;
                        }
                        reader.Close();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ReportePortafolioDTO<ReportePedidosPortafolio> GetReportePortafolioPedido_RegistroAPP(FiltrosReportePedidosPortafolio filtros, bool getData = false)"
                    , new object[] { null });
            }
        }
        #endregion

        //Métodos privados
        private List<AplicacionReporteDto> GetTotales(string gerencia, string capafuncional = "")
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionReporteDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_Totales]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.Parameters.Add(new SqlParameter("@gerencias", gerencia));
                        comando.Parameters.Add(new SqlParameter("@CapaFuncional", capafuncional));
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        var SumaTotalVigentes = 0;
                        var SumaTotalEnDesarrollo = 0;

                        while (reader.Read())
                        {
                            var objeto = new AplicacionReporteDto()
                            {
                                //Categoria = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                TotalVigentes = reader.IsDBNull(reader.GetOrdinal("Vigente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vigente")),
                                TotalEnDesarrollo = reader.IsDBNull(reader.GetOrdinal("En Desarrollo")) ? 0 : reader.GetInt32(reader.GetOrdinal("En Desarrollo"))
                            };
                            SumaTotalVigentes += objeto.TotalVigentes;
                            SumaTotalEnDesarrollo += objeto.TotalEnDesarrollo;
                            objeto.SumaTotalVigentes = SumaTotalVigentes;
                            objeto.SumaTotalEnDesarrollo = SumaTotalEnDesarrollo;
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
                    , "Error en el metodo: GetTotales()"
                    , new object[] { null });
            }
        }

        private List<AplicacionReporteDto> GetTotalesInfraestructura(List<AplicacionReporteDto> lista, string gerencia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesInfraestructura]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.Parameters.Add(new SqlParameter("@gerencias", gerencia));
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        var SumaAppsOnPremise = 0;
                        var SumaAppsOnCloud = 0;

                        while (reader.Read())
                        {
                            var objeto = new
                            {
                                //Categoria = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                CloudIaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - IaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - IaaS")),
                                HostingExterno = reader.IsDBNull(reader.GetOrdinal("Hosting Externo")) ? 0 : reader.GetInt32(reader.GetOrdinal("Hosting Externo")),
                                Servidor = reader.IsDBNull(reader.GetOrdinal("Servidor")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor")),
                                Local = reader.IsDBNull(reader.GetOrdinal("Local")) ? 0 : reader.GetInt32(reader.GetOrdinal("Local")),
                                CloudSaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - SaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - SaaS")),
                                FileServer = reader.IsDBNull(reader.GetOrdinal("File Server")) ? 0 : reader.GetInt32(reader.GetOrdinal("File Server")),
                                PcUsuario = reader.IsDBNull(reader.GetOrdinal("Pc Usuario")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pc Usuario")),
                                PlataformaTecnologica = reader.IsDBNull(reader.GetOrdinal("Plataforma tecnológica")) ? 0 : reader.GetInt32(reader.GetOrdinal("Plataforma tecnológica")),
                                ServidoresAIO = reader.IsDBNull(reader.GetOrdinal("Servidores AIO")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores AIO")),
                                ServidorAutogestionado = reader.IsDBNull(reader.GetOrdinal("Servidor Autogestionado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Autogestionado")),
                                ServidorPropio = reader.IsDBNull(reader.GetOrdinal("Servidor Propio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Propio")),
                                CloudPaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - PaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - PaaS")),
                                // Nuevos campos
                                Autogestionado = reader.IsDBNull(reader.GetOrdinal("AUTOGESTIONADO")) ? 0 : reader.GetInt32(reader.GetOrdinal("AUTOGESTIONADO")),
                                TercerizadoNoCloud = reader.IsDBNull(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")) ? 0 : reader.GetInt32(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")),
                                NoAplicaSubsidiaria = reader.IsDBNull(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")) ? 0 : reader.GetInt32(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")),
                                OnPremise = reader.IsDBNull(reader.GetOrdinal("ON PREMISE")) ? 0 : reader.GetInt32(reader.GetOrdinal("ON PREMISE"))
                            };
                            //var item = lista.FirstOrDefault(x => x.Categoria.ToUpper() == objeto.Categoria.ToUpper());
                            var item = lista.FirstOrDefault(x => x.CapaFuncional.ToUpper() == objeto.CapaFuncional.ToUpper());
                            if (item != null)
                            {
                                item.AppsOnPremise = objeto.FileServer + objeto.HostingExterno + objeto.Local + objeto.PcUsuario + objeto.PlataformaTecnologica + objeto.Servidor + objeto.ServidorAutogestionado + objeto.ServidoresAIO + objeto.ServidorPropio + objeto.Autogestionado + objeto.TercerizadoNoCloud + objeto.NoAplicaSubsidiaria + objeto.OnPremise;
                                item.AppsOnCloud = objeto.CloudIaaS + objeto.CloudPaaS + objeto.CloudSaaS;
                                SumaAppsOnPremise += item.AppsOnPremise;
                                SumaAppsOnCloud += item.AppsOnCloud;
                                item.SumaAppsOnPremise = SumaAppsOnPremise;
                                item.SumaAppsOnCloud = SumaAppsOnCloud;
                            }
                        }

                        lista.LastOrDefault().SumaAppsOnPremise = SumaAppsOnPremise;
                        lista.LastOrDefault().SumaAppsOnCloud = SumaAppsOnCloud;

                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTotales()"
                    , new object[] { null });
            }
        }

        private List<AplicacionReporteDto> GetTotalesInfraestructuraDesarrollo(List<AplicacionReporteDto> lista, string gerencia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesInfraestructuraDesarrollo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.Parameters.Add(new SqlParameter("@gerencias", gerencia));
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        var SumaAppsDesarrolladasOnPremise = 0;
                        var SumaAppsDesarrolladasOnCloud = 0;
                        var SumaAppsPaquetesOnPremise = 0;
                        var SumaAppsPaquetesOnCloud = 0;

                        while (reader.Read())
                        {
                            var objeto = new
                            {
                                //Categoria = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                TipoDesarrollo = reader.IsDBNull(reader.GetOrdinal("TipoDesarrollo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoDesarrollo")),
                                CloudIaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - IaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - IaaS")),
                                HostingExterno = reader.IsDBNull(reader.GetOrdinal("Hosting Externo")) ? 0 : reader.GetInt32(reader.GetOrdinal("Hosting Externo")),
                                Servidor = reader.IsDBNull(reader.GetOrdinal("Servidor")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor")),
                                Local = reader.IsDBNull(reader.GetOrdinal("Local")) ? 0 : reader.GetInt32(reader.GetOrdinal("Local")),
                                CloudSaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - SaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - SaaS")),
                                FileServer = reader.IsDBNull(reader.GetOrdinal("File Server")) ? 0 : reader.GetInt32(reader.GetOrdinal("File Server")),
                                PcUsuario = reader.IsDBNull(reader.GetOrdinal("Pc Usuario")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pc Usuario")),
                                PlataformaTecnologica = reader.IsDBNull(reader.GetOrdinal("Plataforma tecnológica")) ? 0 : reader.GetInt32(reader.GetOrdinal("Plataforma tecnológica")),
                                ServidoresAIO = reader.IsDBNull(reader.GetOrdinal("Servidores AIO")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores AIO")),
                                ServidorAutogestionado = reader.IsDBNull(reader.GetOrdinal("Servidor Autogestionado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Autogestionado")),
                                ServidorPropio = reader.IsDBNull(reader.GetOrdinal("Servidor Propio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Propio")),
                                CloudPaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - PaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - PaaS")),

                                // Nuevos campos
                                Autogestionado = reader.IsDBNull(reader.GetOrdinal("AUTOGESTIONADO")) ? 0 : reader.GetInt32(reader.GetOrdinal("AUTOGESTIONADO")),
                                TercerizadoNoCloud = reader.IsDBNull(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")) ? 0 : reader.GetInt32(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")),
                                NoAplicaSubsidiaria = reader.IsDBNull(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")) ? 0 : reader.GetInt32(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")),
                                OnPremise = reader.IsDBNull(reader.GetOrdinal("ON PREMISE")) ? 0 : reader.GetInt32(reader.GetOrdinal("ON PREMISE"))

                            };
                            //var item = lista.FirstOrDefault(x => x.Categoria.ToUpper() == objeto.Categoria.ToUpper());
                            var item = lista.FirstOrDefault(x => x.CapaFuncional.ToUpper() == objeto.CapaFuncional.ToUpper());
                            if (item != null)
                            {
                                if (!objeto.TipoDesarrollo.ToUpper().Contains("ADQUIRIDO"))
                                {
                                    var appsDesarrolladasOnPremiseAnterior = item.AppsDesarrolladasOnPremise;
                                    var appsDesarrolladasOnPremiseActual = 0;
                                    var appsDesarrolladasOnCloudAnterior = item.AppsDesarrolladasOnCloud;
                                    var appsDesarrolladasOnCloudActual = 0;
                                    item.AppsDesarrolladasOnPremise = item.AppsDesarrolladasOnPremise + objeto.FileServer + objeto.HostingExterno + objeto.Local + objeto.PcUsuario + objeto.PlataformaTecnologica + objeto.Servidor + objeto.ServidorAutogestionado + objeto.ServidoresAIO + objeto.ServidorPropio + objeto.Autogestionado + objeto.TercerizadoNoCloud + objeto.NoAplicaSubsidiaria + objeto.OnPremise;
                                    appsDesarrolladasOnPremiseActual = item.AppsDesarrolladasOnPremise - appsDesarrolladasOnPremiseAnterior;
                                    item.AppsDesarrolladasOnCloud = item.AppsDesarrolladasOnCloud + objeto.CloudIaaS + objeto.CloudPaaS + objeto.CloudSaaS;
                                    appsDesarrolladasOnCloudActual = item.AppsDesarrolladasOnCloud - appsDesarrolladasOnCloudAnterior;
                                    SumaAppsDesarrolladasOnPremise = SumaAppsDesarrolladasOnPremise + appsDesarrolladasOnPremiseActual;
                                    SumaAppsDesarrolladasOnCloud = SumaAppsDesarrolladasOnCloud + appsDesarrolladasOnCloudActual;
                                    item.SumaAppsDesarrolladasOnPremise = SumaAppsDesarrolladasOnPremise;
                                    item.SumaAppsDesarrolladasOnCloud = SumaAppsDesarrolladasOnCloud;
                                }
                                else
                                {
                                    var appsPaquetesOnPremiseAnterior = item.AppsPaquetesOnPremise;
                                    var appsPaquetesOnPremiseActual = 0;
                                    var appsPaquetesOnCloudAnterior = item.AppsPaquetesOnCloud;
                                    var appsPaquetesOnCloudActual = 0;
                                    item.AppsPaquetesOnPremise = item.AppsPaquetesOnPremise + objeto.FileServer + objeto.HostingExterno + objeto.Local + objeto.PcUsuario + objeto.PlataformaTecnologica + objeto.Servidor + objeto.ServidorAutogestionado + objeto.ServidoresAIO + objeto.ServidorPropio + objeto.Autogestionado + objeto.TercerizadoNoCloud + objeto.NoAplicaSubsidiaria + objeto.OnPremise;
                                    appsPaquetesOnPremiseActual = item.AppsPaquetesOnPremise - appsPaquetesOnPremiseAnterior;
                                    item.AppsPaquetesOnCloud = item.AppsPaquetesOnCloud + objeto.CloudIaaS + objeto.CloudPaaS + objeto.CloudSaaS;
                                    appsPaquetesOnCloudActual = item.AppsPaquetesOnCloud - appsPaquetesOnCloudAnterior;
                                    SumaAppsPaquetesOnPremise = SumaAppsPaquetesOnPremise + appsPaquetesOnPremiseActual;
                                    SumaAppsPaquetesOnCloud = SumaAppsPaquetesOnCloud + appsPaquetesOnCloudActual;
                                    item.SumaAppsPaquetesOnPremise = SumaAppsPaquetesOnPremise;
                                    item.SumaAppsPaquetesOnCloud = SumaAppsPaquetesOnCloud;
                                }
                            }
                        }

                        lista.LastOrDefault().SumaAppsDesarrolladasOnPremise = SumaAppsDesarrolladasOnPremise;
                        lista.LastOrDefault().SumaAppsDesarrolladasOnCloud = SumaAppsDesarrolladasOnCloud;

                        lista.LastOrDefault().SumaAppsPaquetesOnPremise = SumaAppsPaquetesOnPremise;
                        lista.LastOrDefault().SumaAppsPaquetesOnCloud = SumaAppsPaquetesOnCloud;

                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTotales()"
                    , new object[] { null });
            }
        }

        private List<AplicacionReporteDto> GetTotalesObsolescencia(List<AplicacionReporteDto> lista, string gerencia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_Vista1_TotalesObsolescencia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.Parameters.Add(new SqlParameter("@gerencias", gerencia));
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        var SumaAppsObsolescenciaOnPremise = 0;
                        var SumaAppsObsolescenciaOnCloud = 0;

                        while (reader.Read())
                        {
                            var objeto = new
                            {
                                //Categoria = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                CapaFuncional = reader.IsDBNull(reader.GetOrdinal("CapaFuncional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CapaFuncional")),
                                CloudIaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - IaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - IaaS")),
                                HostingExterno = reader.IsDBNull(reader.GetOrdinal("Hosting Externo")) ? 0 : reader.GetInt32(reader.GetOrdinal("Hosting Externo")),
                                Servidor = reader.IsDBNull(reader.GetOrdinal("Servidor")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor")),
                                Local = reader.IsDBNull(reader.GetOrdinal("Local")) ? 0 : reader.GetInt32(reader.GetOrdinal("Local")),
                                CloudSaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - SaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - SaaS")),
                                FileServer = reader.IsDBNull(reader.GetOrdinal("File Server")) ? 0 : reader.GetInt32(reader.GetOrdinal("File Server")),
                                PcUsuario = reader.IsDBNull(reader.GetOrdinal("Pc Usuario")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pc Usuario")),
                                PlataformaTecnologica = reader.IsDBNull(reader.GetOrdinal("Plataforma tecnológica")) ? 0 : reader.GetInt32(reader.GetOrdinal("Plataforma tecnológica")),
                                ServidoresAIO = reader.IsDBNull(reader.GetOrdinal("Servidores AIO")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores AIO")),
                                ServidorAutogestionado = reader.IsDBNull(reader.GetOrdinal("Servidor Autogestionado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Autogestionado")),
                                ServidorPropio = reader.IsDBNull(reader.GetOrdinal("Servidor Propio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidor Propio")),
                                CloudPaaS = reader.IsDBNull(reader.GetOrdinal("Cloud - PaaS")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cloud - PaaS")),
                                // Nuevos campos
                                Autogestionado = reader.IsDBNull(reader.GetOrdinal("AUTOGESTIONADO")) ? 0 : reader.GetInt32(reader.GetOrdinal("AUTOGESTIONADO")),
                                TercerizadoNoCloud = reader.IsDBNull(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")) ? 0 : reader.GetInt32(reader.GetOrdinal("TERCERIZADO (NO CLOUD)")),
                                NoAplicaSubsidiaria = reader.IsDBNull(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")) ? 0 : reader.GetInt32(reader.GetOrdinal("NO APLICA - SUBSIDIARIA")),
                                OnPremise = reader.IsDBNull(reader.GetOrdinal("ON PREMISE")) ? 0 : reader.GetInt32(reader.GetOrdinal("ON PREMISE"))
                            };
                            //var item = lista.FirstOrDefault(x => x.Categoria.ToUpper() == objeto.Categoria.ToUpper());
                            var item = lista.FirstOrDefault(x => x.CapaFuncional.ToUpper() == objeto.CapaFuncional.ToUpper());
                            if (item != null)
                            {
                                item.AppsObsolescenciaOnPremise = objeto.FileServer + objeto.HostingExterno + objeto.Local + objeto.PcUsuario + objeto.PlataformaTecnologica + objeto.Servidor + objeto.ServidorAutogestionado + objeto.ServidoresAIO + objeto.ServidorPropio + objeto.Autogestionado + objeto.TercerizadoNoCloud + objeto.NoAplicaSubsidiaria + objeto.OnPremise;
                                item.AppsObsolescenciaOnCloud = objeto.CloudIaaS + objeto.CloudPaaS + objeto.CloudSaaS;
                                SumaAppsObsolescenciaOnPremise += item.AppsObsolescenciaOnPremise;
                                SumaAppsObsolescenciaOnCloud += item.AppsObsolescenciaOnCloud;
                                item.SumaAppsObsolescenciaOnPremise = SumaAppsObsolescenciaOnPremise;
                                item.SumaAppsObsolescenciaOnCloud = SumaAppsObsolescenciaOnCloud;
                            }
                        }

                        lista.LastOrDefault().SumaAppsObsolescenciaOnPremise = SumaAppsObsolescenciaOnPremise;
                        lista.LastOrDefault().SumaAppsObsolescenciaOnCloud = SumaAppsObsolescenciaOnCloud;

                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTotales()"
                    , new object[] { null });
            }
        }

        private List<ReportePortafolioGrafico> GetDataSaludAplications(FiltrosReporteEstadoPortafolio filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReportePortafolioGrafico>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_Portafolio_ReporteEstadoXUnidades_Calculo]", cnx))
                    {
                        //string lstTipoActivo = filtros.TipoActivo; //filtros.TipoActivo.Any() ? string.Join("|", filtros.TipoActivo) : "";
                        //string lstEstado = filtros.EstadoAplicacion;//filtros.EstadoAplicacion.Any() ? string.Join("|", filtros.EstadoAplicacion) : "";

                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.EstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@GerenciaId", filtros.GerenciaId));
                        comando.Parameters.Add(new SqlParameter("@DivisionId", filtros.DivisionId));
                        comando.Parameters.Add(new SqlParameter("@AreaId", filtros.AreaId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReportePortafolioGrafico()
                            {
                                Grupo = reader.IsDBNull(reader.GetOrdinal("Cumplimiento")) ? "" : reader.GetString(reader.GetOrdinal("Cumplimiento")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Total"))
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
                    , "Error en el metodo: List<ReportePortafolioGrafico> GetDataSaludAplications(FiltrosReporteEstadoPortafolio filtros)"
                    , new object[] { null });
            }
        }

        public override BootstrapTable<ReporteEstadoPortafolioRoles> PortafolioListadoRolesAplicaciones(FiltrosReporteEstadoPortafolio filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteEstadoPortafolioRoles>();
                var rpta = new BootstrapTable<ReporteEstadoPortafolioRoles>();
                var total = 0;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("app.USP_Portafolio_ReporteEstado_Roles", cnx))
                    {
                        //string lstTipoActivo = filtros.TipoActivo;//filtros.TipoActivo.Any() ? string.Join("|", filtros.TipoActivo) : "";
                        //string lstEstado = filtros.EstadoAplicacion;//filtros.EstadoAplicacion.Any() ? string.Join("|", filtros.EstadoAplicacion) : "";

                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@lstTipoActivo", filtros.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@lstEstado", filtros.EstadoAplicacion));
                        comando.Parameters.Add(new SqlParameter("@GerenciaId", filtros.GerenciaId));
                        comando.Parameters.Add(new SqlParameter("@DivisionId", filtros.DivisionId));
                        comando.Parameters.Add(new SqlParameter("@AreaId", filtros.AreaId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtros.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtros.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteEstadoPortafolioRoles()
                            {
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? "" : reader.GetString(reader.GetOrdinal("Matricula")),
                                ManagerName = reader.IsDBNull(reader.GetOrdinal("ManagerName")) ? "" : reader.GetString(reader.GetOrdinal("ManagerName")),

                                Lider = reader.IsDBNull(reader.GetOrdinal("Lider")) ? 0 : reader.GetInt32(reader.GetOrdinal("Lider")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor")) ? 0 : reader.GetInt32(reader.GetOrdinal("Gestor")),
                                Experto = reader.IsDBNull(reader.GetOrdinal("Experto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Experto")),
                                TL = reader.IsDBNull(reader.GetOrdinal("TL")) ? 0 : reader.GetInt32(reader.GetOrdinal("TL")),
                                TTL = reader.IsDBNull(reader.GetOrdinal("TTL")) ? 0 : reader.GetInt32(reader.GetOrdinal("TTL")),
                                JdE = reader.IsDBNull(reader.GetOrdinal("JdE")) ? 0 : reader.GetInt32(reader.GetOrdinal("JdE")),
                                Broker = reader.IsDBNull(reader.GetOrdinal("Broker")) ? 0 : reader.GetInt32(reader.GetOrdinal("Broker")),


                            };

                            total = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas"));


                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    rpta.Rows = lista;
                    rpta.Total = total;
                    return rpta;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<ReportePortafolioGrafico> GetDataSaludAplications(FiltrosReporteEstadoPortafolio filtros)"
                    , new object[] { null });
            }
        }
    }

}


