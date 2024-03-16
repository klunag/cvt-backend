using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    class IPSvc : IPDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<IPDetalleDto> GetVistaConsolidado(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPDetalleDto()
                            {
                                AmbienteRed = reader.IsDBNull(reader.GetOrdinal("AmbienteRed")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteRed")),
                                NamePrefix = reader.IsDBNull(reader.GetOrdinal("NamePrefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("NamePrefix")),
                                Prefix = reader.IsDBNull(reader.GetOrdinal("Prefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("Prefix")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                AmbienteServidor = reader.IsDBNull(reader.GetOrdinal("AmbienteServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteServidor")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                CoincideCVT = reader.IsDBNull(reader.GetOrdinal("CoincideCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoincideCVT")),
                                CruceAmbientes = reader.IsDBNull(reader.GetOrdinal("CruceAmbientes")) ? string.Empty : reader.GetString(reader.GetOrdinal("CruceAmbientes")),
                                EquipoReporte = reader.IsDBNull(reader.GetOrdinal("EquipoReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoReporte")),
                                ExisteCVT = reader.IsDBNull(reader.GetOrdinal("ExisteCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExisteCVT")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                Gateways = reader.IsDBNull(reader.GetOrdinal("Gateways")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gateways")),
                                IP = reader.IsDBNull(reader.GetOrdinal("IP")) ? string.Empty : reader.GetString(reader.GetOrdinal("IP")),
                                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPAddress")),
                                IPPorSever = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                MAC = reader.IsDBNull(reader.GetOrdinal("MAC")) ? string.Empty : reader.GetString(reader.GetOrdinal("MAC")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                SegredStatus = reader.IsDBNull(reader.GetOrdinal("SegredStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SegredStatus")),
                                StaticRoutes = reader.IsDBNull(reader.GetOrdinal("StaticRoutes")) ? string.Empty : reader.GetString(reader.GetOrdinal("StaticRoutes")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                SecurityStatus = reader.IsDBNull(reader.GetOrdinal("SecurityStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecurityStatus")),
                                Virtual = reader.IsDBNull(reader.GetOrdinal("Virtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("Virtual")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("VLAN")),
                                EstadoAddm = reader.IsDBNull(reader.GetOrdinal("EstadoAddm")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoAddm")),
                                EstadoSegmentacion = reader.IsDBNull(reader.GetOrdinal("EstadoSegmentacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoSegmentacion")),
                                EstadoSeguridad = reader.IsDBNull(reader.GetOrdinal("EstadoSeguridad")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoSeguridad")),
                                EstadoTelecom = reader.IsDBNull(reader.GetOrdinal("EstadoTelecom")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTelecom")),
                                Plataforma = reader.IsDBNull(reader.GetOrdinal("Plataforma")) ? string.Empty : reader.GetString(reader.GetOrdinal("Plataforma"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidado()"
                    , new object[] { null });
            }
        }

        public override List<IPDetalleDto> GetVistaEstadoDetalle(PaginacionIP pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@fecha", pag.Fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPDetalleDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                AmbienteRed = reader.IsDBNull(reader.GetOrdinal("AmbienteRed")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteRed")),                                
                                NamePrefix = reader.IsDBNull(reader.GetOrdinal("NamePrefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("NamePrefix")),
                                Prefix = reader.IsDBNull(reader.GetOrdinal("Prefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("Prefix")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                AmbienteServidor = reader.IsDBNull(reader.GetOrdinal("AmbienteServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteServidor")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                CoincideCVT = reader.IsDBNull(reader.GetOrdinal("CoincideCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoincideCVT")),
                                CruceAmbientes = reader.IsDBNull(reader.GetOrdinal("CruceAmbientes")) ? string.Empty : reader.GetString(reader.GetOrdinal("CruceAmbientes")),
                                EquipoReporte = reader.IsDBNull(reader.GetOrdinal("EquipoReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoReporte")),
                                ExisteCVT = reader.IsDBNull(reader.GetOrdinal("ExisteCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExisteCVT")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                Gateways = reader.IsDBNull(reader.GetOrdinal("Gateways")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gateways")),
                                IP = reader.IsDBNull(reader.GetOrdinal("IP")) ? string.Empty : reader.GetString(reader.GetOrdinal("IP")),
                                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPAddress")),
                                IPPorSever = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                MAC = reader.IsDBNull(reader.GetOrdinal("MAC")) ? string.Empty : reader.GetString(reader.GetOrdinal("MAC")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                SegredStatus = reader.IsDBNull(reader.GetOrdinal("SegredStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SegredStatus")),
                                StaticRoutes = reader.IsDBNull(reader.GetOrdinal("StaticRoutes")) ? string.Empty : reader.GetString(reader.GetOrdinal("StaticRoutes")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                SecurityStatus = reader.IsDBNull(reader.GetOrdinal("SecurityStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecurityStatus")),
                                Virtual = reader.IsDBNull(reader.GetOrdinal("Virtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("Virtual")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("VLAN")),
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Responsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("Responsable"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoDetalle()"
                    , new object[] { null });
            }
        }

        public override List<IPResumenDto> GetVistaEstadoResumen()
        {
            try
            {                
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_VistaResumen]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPResumenDto()
                            {
                                TotalCoincideCVT = reader.IsDBNull(reader.GetOrdinal("TotalCoincideCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalCoincideCVT")),                                
                                TotalExisteIPs = reader.IsDBNull(reader.GetOrdinal("TotalExisteIPs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalExisteIPs")),
                                TotalIPs = reader.IsDBNull(reader.GetOrdinal("TotalIPs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIPs")),                                
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalServidoresAgencia = reader.IsDBNull(reader.GetOrdinal("TotalServidoresAgencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidoresAgencia")),
                                TotalIPsAcumuladas = reader.IsDBNull(reader.GetOrdinal("TotalIPsAcumuladas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIPsAcumuladas"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoResumen()"
                    , new object[] { null });
            }
        }

        public override List<IPConsolidadoDto> GetVistaConsolidadoNivel1(DateTime fecha)
        {
            try
            {                
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1_Nivel1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPConsolidadoDto()
                            {
                                Identificacion = reader.IsDBNull(reader.GetOrdinal("Identificacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("Identificacion")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),                                
                                SegRedOK = reader.IsDBNull(reader.GetOrdinal("SegredOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredOK")),
                                SegRedNOK = reader.IsDBNull(reader.GetOrdinal("SegredNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredNOK")),
                                Servidores = reader.IsDBNull(reader.GetOrdinal("Servidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                ServidoresUnicos = reader.IsDBNull(reader.GetOrdinal("ServidoresUnicos")) ? 0 : reader.GetInt32(reader.GetOrdinal("ServidoresUnicos"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<IPConsolidadoDto> GetVistaConsolidadoNivel2(int identificacion, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1_Nivel2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@identificacion", identificacion));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPConsolidadoDto()
                            {
                                Identificacion = reader.IsDBNull(reader.GetOrdinal("Identificacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("Identificacion")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),
                                SegRedOK = reader.IsDBNull(reader.GetOrdinal("SegredOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredOK")),
                                SegRedNOK = reader.IsDBNull(reader.GetOrdinal("SegredNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredNOK")),
                                Servidores = reader.IsDBNull(reader.GetOrdinal("Servidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                ServidoresUnicos = reader.IsDBNull(reader.GetOrdinal("ServidoresUnicos")) ? 0 : reader.GetInt32(reader.GetOrdinal("ServidoresUnicos"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel2()"
                    , new object[] { null });
            }
        }

        public override List<IPConsolidadoDto> GetVistaConsolidadoNivel3(int identificacion, string cmdb, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1_Nivel3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@identificacion", identificacion));
                        comando.Parameters.Add(new SqlParameter("@cmdb", cmdb));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPConsolidadoDto()
                            {
                                Identificacion = reader.IsDBNull(reader.GetOrdinal("Identificacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("Identificacion")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),
                                SegRedOK = reader.IsDBNull(reader.GetOrdinal("SegredOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredOK")),
                                SegRedNOK = reader.IsDBNull(reader.GetOrdinal("SegredNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredNOK")),
                                Servidores = reader.IsDBNull(reader.GetOrdinal("Servidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                ServidoresUnicos = reader.IsDBNull(reader.GetOrdinal("ServidoresUnicos")) ? 0 : reader.GetInt32(reader.GetOrdinal("ServidoresUnicos"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel3()"
                    , new object[] { null });
            }
        }

        public override List<IPConsolidadoDto> GetVistaConsolidadoNivel4(int identificacion, string cmdb, string zona, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1_Nivel4]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@identificacion", identificacion));
                        comando.Parameters.Add(new SqlParameter("@cmdb", cmdb));
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPConsolidadoDto()
                            {
                                Identificacion = reader.IsDBNull(reader.GetOrdinal("Identificacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("Identificacion")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                IPPorServer = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),
                                SegRedOK = reader.IsDBNull(reader.GetOrdinal("SegredOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredOK")),
                                SegRedNOK = reader.IsDBNull(reader.GetOrdinal("SegredNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegredNOK")),
                                Servidores = reader.IsDBNull(reader.GetOrdinal("Servidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("Servidores")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                ServidoresUnicos = reader.IsDBNull(reader.GetOrdinal("ServidoresUnicos")) ? 0 : reader.GetInt32(reader.GetOrdinal("ServidoresUnicos"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel4()"
                    , new object[] { null });
            }
        }

        public override List<IPDetalleDto> GetVistaConsolidadoNivel5(int identificacion, string cmdb, string zona, string ips, DateTime fecha)
        {
            try
            {                
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Vista1_Nivel5]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@identificacion", identificacion));
                        comando.Parameters.Add(new SqlParameter("@cmdb", cmdb));
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@ips", ips));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPDetalleDto()
                            {                                
                                AmbienteRed = reader.IsDBNull(reader.GetOrdinal("AmbienteRed")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteRed")),
                                NamePrefix = reader.IsDBNull(reader.GetOrdinal("NamePrefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("NamePrefix")),
                                Prefix = reader.IsDBNull(reader.GetOrdinal("Prefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("Prefix")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                AmbienteServidor = reader.IsDBNull(reader.GetOrdinal("AmbienteServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteServidor")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                CoincideCVT = reader.IsDBNull(reader.GetOrdinal("CoincideCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoincideCVT")),
                                CruceAmbientes = reader.IsDBNull(reader.GetOrdinal("CruceAmbientes")) ? string.Empty : reader.GetString(reader.GetOrdinal("CruceAmbientes")),
                                EquipoReporte = reader.IsDBNull(reader.GetOrdinal("EquipoReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoReporte")),
                                ExisteCVT = reader.IsDBNull(reader.GetOrdinal("ExisteCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExisteCVT")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                Gateways = reader.IsDBNull(reader.GetOrdinal("Gateways")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gateways")),
                                IP = reader.IsDBNull(reader.GetOrdinal("IP")) ? string.Empty : reader.GetString(reader.GetOrdinal("IP")),
                                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPAddress")),
                                IPPorSever = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                MAC = reader.IsDBNull(reader.GetOrdinal("MAC")) ? string.Empty : reader.GetString(reader.GetOrdinal("MAC")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                SegredStatus = reader.IsDBNull(reader.GetOrdinal("SegredStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SegredStatus")),
                                StaticRoutes = reader.IsDBNull(reader.GetOrdinal("StaticRoutes")) ? string.Empty : reader.GetString(reader.GetOrdinal("StaticRoutes")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                SecurityStatus = reader.IsDBNull(reader.GetOrdinal("SecurityStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecurityStatus")),
                                Virtual = reader.IsDBNull(reader.GetOrdinal("Virtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("Virtual")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("VLAN")),
                                EquipoInactivo = reader.IsDBNull(reader.GetOrdinal("EquipoInactivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoInactivo")),
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Responsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("Responsable"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel5()"
                    , new object[] { null });
            }
        }

        public override List<IPEstadoDto> GetVistaEstadoNivel1(DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPEstadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Estado_Nivel1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPEstadoDto()
                            {
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),                                
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                AddmVerde = reader.IsDBNull(reader.GetOrdinal("AddmVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmVerde")),
                                AddmAmarillo = reader.IsDBNull(reader.GetOrdinal("AddmAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmAmarillo")),
                                AddmRojo = reader.IsDBNull(reader.GetOrdinal("AddmRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmRojo")),
                                SegmentacionVerde = reader.IsDBNull(reader.GetOrdinal("SegmentacionVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionVerde")),
                                SegmentacionAmarillo = reader.IsDBNull(reader.GetOrdinal("SegmentacionAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionAmarillo")),
                                SegmentacionRojo = reader.IsDBNull(reader.GetOrdinal("SegmentacionRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionRojo")),
                                SeguridadVerde = reader.IsDBNull(reader.GetOrdinal("SeguridadVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadVerde")),
                                SeguridadRojo = reader.IsDBNull(reader.GetOrdinal("SeguridadRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadRojo")),
                                TelecomVerde = reader.IsDBNull(reader.GetOrdinal("TelecomVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomVerde")),
                                TelecomRojo = reader.IsDBNull(reader.GetOrdinal("TelecomRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomRojo"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<IPEstadoDto> GetVistaEstadoNivel2(string zona, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPEstadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Estado_Nivel2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPEstadoDto()
                            {
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),                                
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                AddmVerde = reader.IsDBNull(reader.GetOrdinal("AddmVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmVerde")),
                                AddmAmarillo = reader.IsDBNull(reader.GetOrdinal("AddmAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmAmarillo")),
                                AddmRojo = reader.IsDBNull(reader.GetOrdinal("AddmRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmRojo")),
                                SegmentacionVerde = reader.IsDBNull(reader.GetOrdinal("SegmentacionVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionVerde")),
                                SegmentacionAmarillo = reader.IsDBNull(reader.GetOrdinal("SegmentacionAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionAmarillo")),
                                SegmentacionRojo = reader.IsDBNull(reader.GetOrdinal("SegmentacionRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionRojo")),
                                SeguridadVerde = reader.IsDBNull(reader.GetOrdinal("SeguridadVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadVerde")),
                                SeguridadRojo = reader.IsDBNull(reader.GetOrdinal("SeguridadRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadRojo")),
                                TelecomVerde = reader.IsDBNull(reader.GetOrdinal("TelecomVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomVerde")),
                                TelecomRojo = reader.IsDBNull(reader.GetOrdinal("TelecomRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomRojo"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoNivel2()"
                    , new object[] { null });
            }
        }

        public override List<IPEstadoDto> GetVistaEstadoNivel3(string zona, string addm, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPEstadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Estado_Nivel3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@addm", addm));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPEstadoDto()
                            {
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),                                
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                AddmVerde = reader.IsDBNull(reader.GetOrdinal("AddmVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmVerde")),
                                AddmAmarillo = reader.IsDBNull(reader.GetOrdinal("AddmAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmAmarillo")),
                                AddmRojo = reader.IsDBNull(reader.GetOrdinal("AddmRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmRojo")),
                                SegmentacionVerde = reader.IsDBNull(reader.GetOrdinal("SegmentacionVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionVerde")),
                                SegmentacionAmarillo = reader.IsDBNull(reader.GetOrdinal("SegmentacionAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionAmarillo")),
                                SegmentacionRojo = reader.IsDBNull(reader.GetOrdinal("SegmentacionRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionRojo")),
                                SeguridadVerde = reader.IsDBNull(reader.GetOrdinal("SeguridadVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadVerde")),
                                SeguridadRojo = reader.IsDBNull(reader.GetOrdinal("SeguridadRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadRojo")),
                                TelecomVerde = reader.IsDBNull(reader.GetOrdinal("TelecomVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomVerde")),
                                TelecomRojo = reader.IsDBNull(reader.GetOrdinal("TelecomRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomRojo"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoNivel3()"
                    , new object[] { null });
            }
        }

        public override List<IPEstadoDto> GetVistaEstadoNivel4(string zona, string addm, string fuente, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPEstadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Estado_Nivel4]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@addm", addm));
                        comando.Parameters.Add(new SqlParameter("@fuente", fuente));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPEstadoDto()
                            {
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                IPPorServer = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                IPTotal = reader.IsDBNull(reader.GetOrdinal("IPTotal")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPTotal")),                                
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? 0 : reader.GetInt32(reader.GetOrdinal("VLAN")),
                                AddmVerde = reader.IsDBNull(reader.GetOrdinal("AddmVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmVerde")),
                                AddmAmarillo = reader.IsDBNull(reader.GetOrdinal("AddmAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmAmarillo")),
                                AddmRojo = reader.IsDBNull(reader.GetOrdinal("AddmRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddmRojo")),
                                SegmentacionVerde = reader.IsDBNull(reader.GetOrdinal("SegmentacionVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionVerde")),
                                SegmentacionAmarillo = reader.IsDBNull(reader.GetOrdinal("SegmentacionAmarillo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionAmarillo")),
                                SegmentacionRojo = reader.IsDBNull(reader.GetOrdinal("SegmentacionRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SegmentacionRojo")),
                                SeguridadVerde = reader.IsDBNull(reader.GetOrdinal("SeguridadVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadVerde")),
                                SeguridadRojo = reader.IsDBNull(reader.GetOrdinal("SeguridadRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("SeguridadRojo")),
                                TelecomVerde = reader.IsDBNull(reader.GetOrdinal("TelecomVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomVerde")),
                                TelecomRojo = reader.IsDBNull(reader.GetOrdinal("TelecomRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("TelecomRojo"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel4()"
                    , new object[] { null });
            }
        }

        public override List<IPSeguimientoDto> GetSeguimiento(PaginacionIP pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPSeguimientoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Seguimiento]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", pag.Fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPSeguimientoDto()
                            {
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Responsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("Responsable")),
                                GatewaysNOK = reader.IsDBNull(reader.GetOrdinal("GatewaysNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("GatewaysNOK")),
                                GatewaysOK = reader.IsDBNull(reader.GetOrdinal("GatewaysOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("GatewaysOK")),
                                StaticNOK = reader.IsDBNull(reader.GetOrdinal("StaticNOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("StaticNOK")),
                                StaticOK = reader.IsDBNull(reader.GetOrdinal("StaticOK")) ? 0 : reader.GetInt32(reader.GetOrdinal("StaticOK")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                TotalIP = reader.IsDBNull(reader.GetOrdinal("TotalIP")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIP")),
                                NroEquipos = reader.IsDBNull(reader.GetOrdinal("NroEquipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroEquipos")),
                                TotalExisteCVT = reader.IsDBNull(reader.GetOrdinal("TotalExisteCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalExisteCVT")),
                                PorcAvanceCVT = reader.IsDBNull(reader.GetOrdinal("PorcAvanceCVT")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcAvanceCVT"))    
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaEstadoResumen()"
                    , new object[] { null });
            }
        }

        public override List<IPDetalleDto> GetVistaEstadoNivel5(string zona, string addm, string fuente, string ips, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<IPDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_IP_Dashboard_Estado_Nivel5]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@zona", zona));
                        comando.Parameters.Add(new SqlParameter("@addm", addm));
                        comando.Parameters.Add(new SqlParameter("@fuente", fuente));
                        comando.Parameters.Add(new SqlParameter("@ips", ips));
                        comando.Parameters.Add(new SqlParameter("@fecha", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new IPDetalleDto()
                            {
                                AmbienteRed = reader.IsDBNull(reader.GetOrdinal("AmbienteRed")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteRed")),
                                NamePrefix = reader.IsDBNull(reader.GetOrdinal("NamePrefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("NamePrefix")),
                                Prefix = reader.IsDBNull(reader.GetOrdinal("Prefix")) ? string.Empty : reader.GetString(reader.GetOrdinal("Prefix")),
                                Zona = reader.IsDBNull(reader.GetOrdinal("Zona")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zona")),
                                AmbienteServidor = reader.IsDBNull(reader.GetOrdinal("AmbienteServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteServidor")),
                                CMDB = reader.IsDBNull(reader.GetOrdinal("CMDB")) ? string.Empty : reader.GetString(reader.GetOrdinal("CMDB")),
                                CoincideCVT = reader.IsDBNull(reader.GetOrdinal("CoincideCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoincideCVT")),
                                CruceAmbientes = reader.IsDBNull(reader.GetOrdinal("CruceAmbientes")) ? string.Empty : reader.GetString(reader.GetOrdinal("CruceAmbientes")),
                                EquipoReporte = reader.IsDBNull(reader.GetOrdinal("EquipoReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoReporte")),
                                ExisteCVT = reader.IsDBNull(reader.GetOrdinal("ExisteCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExisteCVT")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fuente")),
                                Gateways = reader.IsDBNull(reader.GetOrdinal("Gateways")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gateways")),
                                IP = reader.IsDBNull(reader.GetOrdinal("IP")) ? string.Empty : reader.GetString(reader.GetOrdinal("IP")),
                                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPAddress")),
                                IPPorSever = reader.IsDBNull(reader.GetOrdinal("IPPorSever")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPPorSever")),
                                MAC = reader.IsDBNull(reader.GetOrdinal("MAC")) ? string.Empty : reader.GetString(reader.GetOrdinal("MAC")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                SegredStatus = reader.IsDBNull(reader.GetOrdinal("SegredStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SegredStatus")),
                                StaticRoutes = reader.IsDBNull(reader.GetOrdinal("StaticRoutes")) ? string.Empty : reader.GetString(reader.GetOrdinal("StaticRoutes")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                SecurityStatus = reader.IsDBNull(reader.GetOrdinal("SecurityStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecurityStatus")),
                                Virtual = reader.IsDBNull(reader.GetOrdinal("Virtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("Virtual")),
                                VLAN = reader.IsDBNull(reader.GetOrdinal("VLAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("VLAN")),
                                EquipoInactivo = reader.IsDBNull(reader.GetOrdinal("EquipoInactivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoInactivo")),
                                Responsable = reader.IsDBNull(reader.GetOrdinal("Responsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("Responsable"))
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
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetVistaConsolidadoNivel5()"
                    , new object[] { null });
            }
        }
    }
}
