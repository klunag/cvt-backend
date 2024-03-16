using BCP.CVT.DTO;
using BCP.CVT.DTO.Storage;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BCP.CVT.Cross;
using BCP.CVT.DTO.Custom;

namespace BCP.CVT.Services.Service
{
    public class StorageSvc : StorageDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<BackupDto> GetBackupMainframe(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<BackupDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarMainframe]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jobname", filtro.jobname));
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", filtro.app));
                        comando.Parameters.Add(new SqlParameter("@interface", filtro.interfaceApp));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new BackupDto()
                            {
                                outcode = reader.IsDBNull(reader.GetOrdinal("outcode")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcode")),
                                jobname = reader.IsDBNull(reader.GetOrdinal("jobname")) ? string.Empty : reader.GetString(reader.GetOrdinal("jobname")),
                                instances = reader.IsDBNull(reader.GetOrdinal("instances")) ? 0 : reader.GetInt32(reader.GetOrdinal("instances")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Frecuencia = reader.IsDBNull(reader.GetOrdinal("Frecuencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Frecuencia")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                InterfaceApp = reader.IsDBNull(reader.GetOrdinal("InterfaceApp")) ? string.Empty : reader.GetString(reader.GetOrdinal("InterfaceApp")),
                                Tamanio = reader.IsDBNull(reader.GetOrdinal("Tamanio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Tamanio")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupMainframe(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<BackupDto> GetBackupMainframeDetalle(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<BackupDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarMainframeDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jobname", filtro.jobname));
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", filtro.app));
                        comando.Parameters.Add(new SqlParameter("@interface", filtro.interfaceApp));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new BackupDto()
                            {
                                outcode = reader.IsDBNull(reader.GetOrdinal("outcode")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcode")),
                                jobname = reader.IsDBNull(reader.GetOrdinal("jobname")) ? string.Empty : reader.GetString(reader.GetOrdinal("jobname")),
                                volser = reader.IsDBNull(reader.GetOrdinal("volser")) ? string.Empty : reader.GetString(reader.GetOrdinal("volser")),
                                dsname = reader.IsDBNull(reader.GetOrdinal("dsname")) ? string.Empty : reader.GetString(reader.GetOrdinal("dsname")),
                                crtdate = reader.IsDBNull(reader.GetOrdinal("crtdate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("crtdate")),
                                lfrdate = reader.IsDBNull(reader.GetOrdinal("lfrdate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("lfrdate")),
                                mbsize = reader.IsDBNull(reader.GetOrdinal("mbsize")) ? 0 : reader.GetInt32(reader.GetOrdinal("mbsize")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Frecuencia = reader.IsDBNull(reader.GetOrdinal("Frecuencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Frecuencia")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                InterfaceApp = reader.IsDBNull(reader.GetOrdinal("InterfaceApp")) ? string.Empty : reader.GetString(reader.GetOrdinal("InterfaceApp")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),

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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupMainframeDetalle(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<OpenDto> GetBackupOpenExportar(string servidor, int mes, int anio)
        {            
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<OpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpenExportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@server", servidor));
                        comando.Parameters.Add(new SqlParameter("@monthBackup", mes));
                        comando.Parameters.Add(new SqlParameter("@yearBackup", anio));                       

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new OpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),
                                zone = reader.IsDBNull(reader.GetOrdinal("zone")) ? string.Empty : reader.GetString(reader.GetOrdinal("zone")),
                                backupday = reader.IsDBNull(reader.GetOrdinal("backupday")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("backupday")),
                                startdatetime = reader.IsDBNull(reader.GetOrdinal("startdatetime")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("startdatetime")),
                                finishdatetime = reader.IsDBNull(reader.GetOrdinal("startdatetime")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("finishdatetime")),
                                product = reader.IsDBNull(reader.GetOrdinal("product")) ? string.Empty : reader.GetString(reader.GetOrdinal("product")),
                                backupserver = reader.IsDBNull(reader.GetOrdinal("backupserver")) ? string.Empty : reader.GetString(reader.GetOrdinal("backupserver")),
                                clientname = reader.IsDBNull(reader.GetOrdinal("clientname")) ? string.Empty : reader.GetString(reader.GetOrdinal("clientname")),
                                target = reader.IsDBNull(reader.GetOrdinal("target")) ? string.Empty : reader.GetString(reader.GetOrdinal("target")),
                                levelbackup = reader.IsDBNull(reader.GetOrdinal("levelbackup")) ? string.Empty : reader.GetString(reader.GetOrdinal("levelbackup")),
                                groupname = reader.IsDBNull(reader.GetOrdinal("groupname")) ? string.Empty : reader.GetString(reader.GetOrdinal("groupname")),

                                outcome = reader.IsDBNull(reader.GetOrdinal("outcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcome")),
                                opensoutcome = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("opensoutcome")),

                                errorcodes = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("errorcodes")),
                                errorcategories = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("errorcategories")),
                                description = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),

                                errocount = reader.IsDBNull(reader.GetOrdinal("errocount")) ? 0 : reader.GetInt32(reader.GetOrdinal("errocount")),
                                datatransferredkb = reader.IsDBNull(reader.GetOrdinal("datatransferredkb")) ? 0 : reader.GetDecimal(reader.GetOrdinal("datatransferredkb")),
                                performancembs = reader.IsDBNull(reader.GetOrdinal("performancembs")) ? 0 : reader.GetDecimal(reader.GetOrdinal("performancembs"))

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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupMainframeExportar(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<OpenDto> GetBackupOpen(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<OpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpen]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", string.IsNullOrEmpty(filtro.codigoAPT) ? string.Empty : filtro.codigoAPT)) ;
                        comando.Parameters.Add(new SqlParameter("@server", string.IsNullOrEmpty(filtro.server) ? string.Empty : filtro.server));
                        comando.Parameters.Add(new SqlParameter("@backupserver", string.IsNullOrEmpty(filtro.backupserver) ? string.Empty : filtro.backupserver));
                        comando.Parameters.Add(new SqlParameter("@target", string.IsNullOrEmpty(filtro.target) ? string.Empty : filtro.target));
                        comando.Parameters.Add(new SqlParameter("@levelbackup", string.IsNullOrEmpty(filtro.levelbackup) ? string.Empty : filtro.levelbackup));
                        comando.Parameters.Add(new SqlParameter("@outcome", string.IsNullOrEmpty(filtro.outcome) ? string.Empty : filtro.outcome));
                        comando.Parameters.Add(new SqlParameter("@date", filtro.Fecha));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new OpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),
                                groupname = reader.IsDBNull(reader.GetOrdinal("groupname")) ? string.Empty : reader.GetString(reader.GetOrdinal("groupname")),
                                dayBackup = reader.IsDBNull(reader.GetOrdinal("dayBackup")) ? 0 : reader.GetInt32(reader.GetOrdinal("dayBackup")),
                                monthBackup = reader.IsDBNull(reader.GetOrdinal("monthBackup")) ? 0 : reader.GetInt32(reader.GetOrdinal("monthBackup")),
                                yearBackup = reader.IsDBNull(reader.GetOrdinal("yearBackup")) ? 0 : reader.GetInt32(reader.GetOrdinal("yearBackup")),
                                total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0 : reader.GetInt32(reader.GetOrdinal("total")),
                                totalAppsRelacionadas = reader.IsDBNull(reader.GetOrdinal("totalAppsRelacionadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalAppsRelacionadas")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<RelacionOpenDto> GetBackupOpenAplicaciones(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionOpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpenAplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        
                        comando.Parameters.Add(new SqlParameter("@server", string.IsNullOrEmpty(filtro.server) ? string.Empty : filtro.server));                        
                        comando.Parameters.Add(new SqlParameter("@date", filtro.Fecha));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));                        

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionOpenDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpenAplicaciones(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<OpenDto> GetBackupOpenDetalle(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<OpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpenDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        
                        comando.Parameters.Add(new SqlParameter("@server", filtro.server));
                        comando.Parameters.Add(new SqlParameter("@groupname", filtro.groupname));
                        comando.Parameters.Add(new SqlParameter("@backupserver", filtro.backupserver));
                        comando.Parameters.Add(new SqlParameter("@target", filtro.target));
                        comando.Parameters.Add(new SqlParameter("@levelbackup", filtro.levelbackup));
                        comando.Parameters.Add(new SqlParameter("@outcome", filtro.outcome));
                        comando.Parameters.Add(new SqlParameter("@dayBackup", filtro.dayBackup));
                        comando.Parameters.Add(new SqlParameter("@monthBackup", filtro.monthBackup));
                        comando.Parameters.Add(new SqlParameter("@yearBackup", filtro.yearBackup));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new OpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),
                                zone = reader.IsDBNull(reader.GetOrdinal("zone")) ? string.Empty : reader.GetString(reader.GetOrdinal("zone")),
                                backupday = reader.IsDBNull(reader.GetOrdinal("backupday")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("backupday")),
                                startdatetime = reader.IsDBNull(reader.GetOrdinal("startdatetime")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("startdatetime")),
                                finishdatetime = reader.IsDBNull(reader.GetOrdinal("startdatetime")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("finishdatetime")),
                                product = reader.IsDBNull(reader.GetOrdinal("product")) ? string.Empty : reader.GetString(reader.GetOrdinal("product")),
                                backupserver = reader.IsDBNull(reader.GetOrdinal("backupserver")) ? string.Empty : reader.GetString(reader.GetOrdinal("backupserver")),
                                clientname = reader.IsDBNull(reader.GetOrdinal("clientname")) ? string.Empty : reader.GetString(reader.GetOrdinal("clientname")),
                                target = reader.IsDBNull(reader.GetOrdinal("target")) ? string.Empty : reader.GetString(reader.GetOrdinal("target")),
                                levelbackup = reader.IsDBNull(reader.GetOrdinal("levelbackup")) ? string.Empty : reader.GetString(reader.GetOrdinal("levelbackup")),
                                groupname = reader.IsDBNull(reader.GetOrdinal("groupname")) ? string.Empty : reader.GetString(reader.GetOrdinal("groupname")),

                                outcome = reader.IsDBNull(reader.GetOrdinal("outcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcome")),
                                opensoutcome = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("opensoutcome")),

                                errorcodes = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("errorcodes")),
                                errorcategories = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("errorcategories")),
                                description = reader.IsDBNull(reader.GetOrdinal("opensoutcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),

                                errocount = reader.IsDBNull(reader.GetOrdinal("errocount")) ? 0 : reader.GetInt32(reader.GetOrdinal("errocount")),
                                datatransferredkb = reader.IsDBNull(reader.GetOrdinal("datatransferredkb")) ? 0 : reader.GetDecimal(reader.GetOrdinal("datatransferredkb")),
                                performancembs = reader.IsDBNull(reader.GetOrdinal("performancembs")) ? 0 : reader.GetDecimal(reader.GetOrdinal("performancembs")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<ResumenOpenDto> GetBackupOpenResumen(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ResumenOpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpenResumen]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                                                                       
                        comando.Parameters.Add(new SqlParameter("@mes", filtro.monthBackup));
                        comando.Parameters.Add(new SqlParameter("@anio", filtro.yearBackup));
                        comando.Parameters.Add(new SqlParameter("@servidor", filtro.server));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));                       

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ResumenOpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),                                
                                Dia1 = reader.IsDBNull(reader.GetOrdinal("1")) ? -1 : reader.GetInt32(reader.GetOrdinal("1")),
                                Dia2 = reader.IsDBNull(reader.GetOrdinal("2")) ? -1 : reader.GetInt32(reader.GetOrdinal("2")),
                                Dia3 = reader.IsDBNull(reader.GetOrdinal("3")) ? -1 : reader.GetInt32(reader.GetOrdinal("3")),
                                Dia4 = reader.IsDBNull(reader.GetOrdinal("4")) ? -1 : reader.GetInt32(reader.GetOrdinal("4")),
                                Dia5 = reader.IsDBNull(reader.GetOrdinal("5")) ? -1 : reader.GetInt32(reader.GetOrdinal("5")),
                                Dia6 = reader.IsDBNull(reader.GetOrdinal("6")) ? -1 : reader.GetInt32(reader.GetOrdinal("6")),
                                Dia7 = reader.IsDBNull(reader.GetOrdinal("7")) ? -1 : reader.GetInt32(reader.GetOrdinal("7")),
                                Dia8 = reader.IsDBNull(reader.GetOrdinal("8")) ? -1 : reader.GetInt32(reader.GetOrdinal("8")),
                                Dia9 = reader.IsDBNull(reader.GetOrdinal("9")) ? -1 : reader.GetInt32(reader.GetOrdinal("9")),
                                Dia10 = reader.IsDBNull(reader.GetOrdinal("10")) ? -1 : reader.GetInt32(reader.GetOrdinal("10")),
                                Dia11 = reader.IsDBNull(reader.GetOrdinal("11")) ? -1 : reader.GetInt32(reader.GetOrdinal("11")),
                                Dia12 = reader.IsDBNull(reader.GetOrdinal("12")) ? -1 : reader.GetInt32(reader.GetOrdinal("12")),
                                Dia13 = reader.IsDBNull(reader.GetOrdinal("13")) ? -1 : reader.GetInt32(reader.GetOrdinal("13")),
                                Dia14 = reader.IsDBNull(reader.GetOrdinal("14")) ? -1 : reader.GetInt32(reader.GetOrdinal("14")),
                                Dia15 = reader.IsDBNull(reader.GetOrdinal("15")) ? -1 : reader.GetInt32(reader.GetOrdinal("15")),
                                Dia16 = reader.IsDBNull(reader.GetOrdinal("16")) ? -1 : reader.GetInt32(reader.GetOrdinal("16")),
                                Dia17 = reader.IsDBNull(reader.GetOrdinal("17")) ? -1 : reader.GetInt32(reader.GetOrdinal("17")),
                                Dia18 = reader.IsDBNull(reader.GetOrdinal("18")) ? -1 : reader.GetInt32(reader.GetOrdinal("18")),
                                Dia19 = reader.IsDBNull(reader.GetOrdinal("19")) ? -1 : reader.GetInt32(reader.GetOrdinal("19")),
                                Dia20 = reader.IsDBNull(reader.GetOrdinal("20")) ? -1 : reader.GetInt32(reader.GetOrdinal("20")),
                                Dia21 = reader.IsDBNull(reader.GetOrdinal("21")) ? -1 : reader.GetInt32(reader.GetOrdinal("21")),
                                Dia22 = reader.IsDBNull(reader.GetOrdinal("22")) ? -1 : reader.GetInt32(reader.GetOrdinal("22")),
                                Dia23 = reader.IsDBNull(reader.GetOrdinal("23")) ? -1 : reader.GetInt32(reader.GetOrdinal("23")),
                                Dia24 = reader.IsDBNull(reader.GetOrdinal("24")) ? -1 : reader.GetInt32(reader.GetOrdinal("24")),
                                Dia25 = reader.IsDBNull(reader.GetOrdinal("25")) ? -1 : reader.GetInt32(reader.GetOrdinal("25")),
                                Dia26 = reader.IsDBNull(reader.GetOrdinal("26")) ? -1 : reader.GetInt32(reader.GetOrdinal("26")),
                                Dia27 = reader.IsDBNull(reader.GetOrdinal("27")) ? -1 : reader.GetInt32(reader.GetOrdinal("27")),
                                Dia28 = reader.IsDBNull(reader.GetOrdinal("28")) ? -1 : reader.GetInt32(reader.GetOrdinal("28")),
                                Dia29 = reader.IsDBNull(reader.GetOrdinal("29")) ? -1 : reader.GetInt32(reader.GetOrdinal("29")),
                                Dia30 = reader.IsDBNull(reader.GetOrdinal("30")) ? -1 : reader.GetInt32(reader.GetOrdinal("30")),
                                Dia31 = reader.IsDBNull(reader.GetOrdinal("31")) ? -1 : reader.GetInt32(reader.GetOrdinal("31")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<GraficoOpenDto> GetBackupOpenResumenDetalle(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GraficoOpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpen_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@mes", filtro.monthBackup));
                        comando.Parameters.Add(new SqlParameter("@anio", filtro.yearBackup));
                        comando.Parameters.Add(new SqlParameter("@servidor", filtro.server));
                        
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GraficoOpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),
                                outcome = reader.IsDBNull(reader.GetOrdinal("outcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcome")),
                                dayBackup = reader.IsDBNull(reader.GetOrdinal("dayBackup")) ? 0 : reader.GetInt32(reader.GetOrdinal("dayBackup")),                                
                                total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0 : reader.GetInt32(reader.GetOrdinal("total"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista.Count();

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<GraficoOpenDto> GetBackupOpenResumenDetalleTrasnsferencia(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GraficoOpenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[T4_USP_BackupListarOpen_DetalleTamanio]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@mes", filtro.monthBackup));
                        comando.Parameters.Add(new SqlParameter("@anio", filtro.yearBackup));
                        comando.Parameters.Add(new SqlParameter("@servidor", filtro.server));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GraficoOpenDto()
                            {
                                serverName = reader.IsDBNull(reader.GetOrdinal("serverName")) ? string.Empty : reader.GetString(reader.GetOrdinal("serverName")),
                                outcome = reader.IsDBNull(reader.GetOrdinal("outcome")) ? string.Empty : reader.GetString(reader.GetOrdinal("outcome")),
                                dayBackup = reader.IsDBNull(reader.GetOrdinal("dayBackup")) ? 0 : reader.GetInt32(reader.GetOrdinal("dayBackup")),
                                totalMB = reader.IsDBNull(reader.GetOrdinal("totalMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("totalMB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista.Count();

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<BackupPeriodoDto> GetBackupPeriodo(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<BackupPeriodoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_BackupPeriodo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@servidor", filtro.server));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new BackupPeriodoDto()
                            {
                                DayOfMonth = reader.IsDBNull(reader.GetOrdinal("DayOfMonth")) ? string.Empty : reader.GetString(reader.GetOrdinal("DayOfMonth")),
                                DayOfWeek = reader.IsDBNull(reader.GetOrdinal("DayOfWeek")) ? string.Empty : reader.GetString(reader.GetOrdinal("DayOfWeek")),
                                DomainName = reader.IsDBNull(reader.GetOrdinal("DomainName")) ? string.Empty : reader.GetString(reader.GetOrdinal("DomainName")),
                                EnhMonth = reader.IsDBNull(reader.GetOrdinal("EnhMonth")) ? string.Empty : reader.GetString(reader.GetOrdinal("EnhMonth")),
                                NodeName = reader.IsDBNull(reader.GetOrdinal("NodeName")) ? string.Empty : reader.GetString(reader.GetOrdinal("NodeName")),
                                Objects = reader.IsDBNull(reader.GetOrdinal("Objects")) ? string.Empty : reader.GetString(reader.GetOrdinal("Objects")),
                                Options = reader.IsDBNull(reader.GetOrdinal("Options")) ? string.Empty : reader.GetString(reader.GetOrdinal("Options")),
                                Period = reader.IsDBNull(reader.GetOrdinal("Period")) ? string.Empty : reader.GetString(reader.GetOrdinal("Period")),
                                PerUnits = reader.IsDBNull(reader.GetOrdinal("PerUnits")) ? string.Empty : reader.GetString(reader.GetOrdinal("PerUnits")),
                                ScheduleName = reader.IsDBNull(reader.GetOrdinal("ScheduleName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ScheduleName")),
                                ServerName = reader.IsDBNull(reader.GetOrdinal("ServerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ServerName")),
                                Site = reader.IsDBNull(reader.GetOrdinal("Site")) ? string.Empty : reader.GetString(reader.GetOrdinal("Site")),
                                StartTime = reader.IsDBNull(reader.GetOrdinal("StartTime")) ? string.Empty : reader.GetString(reader.GetOrdinal("StartTime")),
                                WeekOfMonth = reader.IsDBNull(reader.GetOrdinal("WeekOfMonth")) ? string.Empty : reader.GetString(reader.GetOrdinal("WeekOfMonth")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
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
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetBackupOpen(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override Dictionary<int, List<StorageIndicadorDto>> GetIndicadorGlobal()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var diccionario = new Dictionary<int, List<StorageIndicadorDto>>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Indicadores_N1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        

                        using (var adapter = new SqlDataAdapter(comando))
                        {
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            var capacidadesTotales = new List<StorageIndicadorDto>();
                            var capacidadesReplica = new List<StorageIndicadorDto>();
                            var capacidadesObsolescencia = new List<StorageIndicadorDto>();
                            var capacidadesAplicaciones = new List<StorageIndicadorDto>();
                            var capacidadActual = new List<StorageIndicadorDto>();
                            var capacidadPieAmbiente = new List<StorageIndicadorDto>();
                            var capacidadPieFuente = new List<StorageIndicadorDto>();

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        capacidadesTotales.Add(new StorageIndicadorDto()
                                        {
                                            Mes = int.Parse(dr["Mes"].ToString()),
                                            Anio = int.Parse(dr["Anio"].ToString()),
                                            CapacidadTB = decimal.Parse(dr["CapacidadTB"].ToString()),
                                            UsadoTB = decimal.Parse(dr["UsadoTB"].ToString()),
                                            LibreTB = decimal.Parse(dr["LibreTB"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[1] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[1].Rows)
                                    {
                                        capacidadesReplica.Add(new StorageIndicadorDto()
                                        {
                                            Mes = int.Parse(dr["Mes"].ToString()),
                                            Anio = int.Parse(dr["Anio"].ToString()),
                                            ReplicaTB = decimal.Parse(dr["ReplicaTB"].ToString()),
                                            NoReplicaTB = decimal.Parse(dr["NoReplicaTB"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[2] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                    {
                                        capacidadesObsolescencia.Add(new StorageIndicadorDto()
                                        {
                                            Mes = int.Parse(dr["Mes"].ToString()),
                                            Anio = int.Parse(dr["Anio"].ToString()),
                                            CapacidadNoObsoleta = decimal.Parse(dr["CapacidadNoObsoleta"].ToString()),
                                            CapacidadObsoleta = decimal.Parse(dr["CapacidadObsoleta"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[3] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[3].Rows)
                                    {
                                        capacidadesAplicaciones.Add(new StorageIndicadorDto()
                                        {
                                            CodigoAPT = dr["CodigoAPT"].ToString(),
                                            CapacidadTB = decimal.Parse(dr["CapacidadTB"].ToString()),
                                            UsadoTB = decimal.Parse(dr["UsadoTB"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[4] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[4].Rows)
                                    {
                                        capacidadPieAmbiente.Add(new StorageIndicadorDto()
                                        {
                                            Valor = dr["DetalleAmbiente"].ToString(),
                                            Capacidad = decimal.Parse(dr["CapacidadTB"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[5] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[5].Rows)
                                    {
                                        var fuente = dr["Fuente"].ToString();
                                        var valor = string.Empty;
                                        switch (fuente)
                                        {
                                            case "1": valor = "x86";
                                                break;
                                            case "2":
                                                valor = "AIX";
                                                break;
                                            case "3":
                                                valor = "Z";
                                                break;
                                            case "4":
                                                valor = "Otros";
                                                break;
                                        }
                                        capacidadPieFuente.Add(new StorageIndicadorDto()
                                        {
                                            Valor = valor,
                                            Capacidad = decimal.Parse(dr["TotalTB"].ToString())
                                        });
                                    }
                                }
                            }

                            diccionario.Add(1, capacidadesTotales.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList());
                            diccionario.Add(2, capacidadesReplica.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList());
                            diccionario.Add(3, capacidadesObsolescencia.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList());
                            diccionario.Add(4, capacidadesAplicaciones.OrderByDescending(x => x.CapacidadTB).ToList());
                            if(capacidadesTotales.Count > 0)
                            {
                                var capacidad = capacidadesTotales[capacidadesTotales.Count - 1];
                                capacidadActual.Add(new StorageIndicadorDto() {
                                    CapacidadTB = capacidad.UsadoTB,
                                    CodigoAPT = "Usado en TB"
                                });
                                capacidadActual.Add(new StorageIndicadorDto()
                                {
                                    CapacidadTB = capacidad.LibreTB,
                                    CodigoAPT = "Libre en TB"
                                });
                            }
                            diccionario.Add(5, capacidadActual);
                            diccionario.Add(6, capacidadPieAmbiente);
                            diccionario.Add(7, capacidadPieFuente);
                        }
                    }
                    cnx.Close();                    
                }
                return diccionario;
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetIndicadorGlobal()"
                    , datos: new object[] { null });
            }
        }

        public override Dictionary<int, List<StorageIndicadorDto>> GetIndicadorTier(int tier)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var diccionario = new Dictionary<int, List<StorageIndicadorDto>>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Indicadores_N2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tier", tier));

                        using (var adapter = new SqlDataAdapter(comando))
                        {
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            var capacidadesTotales = new List<StorageIndicadorDto>();
                            var capacidadesReplica = new List<StorageIndicadorDto>();                            
                            var capacidadesAplicaciones = new List<StorageIndicadorDto>();

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        capacidadesTotales.Add(new StorageIndicadorDto()
                                        {
                                            Mes = int.Parse(dr["Mes"].ToString()),
                                            Anio = int.Parse(dr["Anio"].ToString()),
                                            CapacidadTB = decimal.Parse(dr["CapacidadTB"].ToString()),
                                            UsadoTB = decimal.Parse(dr["UsadoTB"].ToString()),
                                            LibreTB = decimal.Parse(dr["LibreTB"].ToString())
                                        });
                                    }
                                }

                                if (ds.Tables[1] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[1].Rows)
                                    {
                                        capacidadesReplica.Add(new StorageIndicadorDto()
                                        {
                                            Mes = int.Parse(dr["Mes"].ToString()),
                                            Anio = int.Parse(dr["Anio"].ToString()),
                                            ReplicaTB = decimal.Parse(dr["ReplicaTB"].ToString()),
                                            NoReplicaTB = decimal.Parse(dr["NoReplicaTB"].ToString())
                                        });
                                    }
                                }                                

                                if (ds.Tables[2] != null)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                    {
                                        capacidadesAplicaciones.Add(new StorageIndicadorDto()
                                        {
                                            CodigoAPT = dr["CodigoAPT"].ToString(),
                                            CapacidadTB = decimal.Parse(dr["CapacidadTB"].ToString())
                                        });
                                    }
                                }
                            }

                            diccionario.Add(1, capacidadesTotales.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList());
                            diccionario.Add(2, capacidadesReplica.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList());                            
                            diccionario.Add(3, capacidadesAplicaciones.OrderByDescending(x=>x.CapacidadTB).ToList());
                        }
                    }
                    cnx.Close();
                }
                return diccionario;
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetIndicadorTier()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageDto> GetStorage(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", filtro.Nombre));
                        comando.Parameters.Add(new SqlParameter("@softwarebase", filtro.SoftwareBase));
                        comando.Parameters.Add(new SqlParameter("@fecha", filtro.Fecha));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageDto()
                            {
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                SoftwareBase = reader.IsDBNull(reader.GetOrdinal("SoftwareBase")) ? string.Empty : reader.GetString(reader.GetOrdinal("SoftwareBase")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),                                
                                EspacioTotalGB = reader.IsDBNull(reader.GetOrdinal("EspacioTotalGB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("EspacioTotalGB")),                                
                                EspacioUsadoGB = reader.IsDBNull(reader.GetOrdinal("EspacioUsadoGB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("EspacioUsadoGB")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                Volumen = reader.IsDBNull(reader.GetOrdinal("Volumen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Volumen")),
                                Etiqueta = reader.IsDBNull(reader.GetOrdinal("Etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("Etiqueta")),
                                StorageTierId = reader.IsDBNull(reader.GetOrdinal("StorageTierId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageTierId")),
                                NombreTier = reader.IsDBNull(reader.GetOrdinal("NombreTier")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreTier")),
                                IndicadorObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndicadorObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorObsolescencia")),
                                PoolName = reader.IsDBNull(reader.GetOrdinal("PoolName")) ? string.Empty : reader.GetString(reader.GetOrdinal("PoolName")),
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista[0].Total;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorage(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageDto> GetStorageAplicacion(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Aplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", filtro.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@storage", filtro.Storage));
                        comando.Parameters.Add(new SqlParameter("@ambiente", filtro.Ambiente));
                        comando.Parameters.Add(new SqlParameter("@equipo", filtro.Equipo));
                        comando.Parameters.Add(new SqlParameter("@fecha", filtro.Fecha));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                SoftwareBase = reader.IsDBNull(reader.GetOrdinal("SoftwareBase")) ? string.Empty : reader.GetString(reader.GetOrdinal("SoftwareBase")),
                                Storage = reader.IsDBNull(reader.GetOrdinal("Storage")) ? string.Empty : reader.GetString(reader.GetOrdinal("Storage")),
                                NombreTier = reader.IsDBNull(reader.GetOrdinal("NombreTier")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreTier")),
                                TieneReplica = reader.IsDBNull(reader.GetOrdinal("TieneReplica")) ? false : reader.GetBoolean(reader.GetOrdinal("TieneReplica")),
                                CapacidadMB = reader.IsDBNull(reader.GetOrdinal("CapacidadMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadMB")),
                                UsadoMB = reader.IsDBNull(reader.GetOrdinal("UsadoMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoMB")),
                                LibreMB = reader.IsDBNull(reader.GetOrdinal("LibreMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreMB")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                PorcentajeLibre = Math.Round(reader.IsDBNull(reader.GetOrdinal("PorcentajeLibre")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeLibre")), 2),
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista[0].Total;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorage(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageDto> GetStorageEquipo(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Equipos]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipo", filtro.Equipo));
                        comando.Parameters.Add(new SqlParameter("@storage", filtro.Storage));
                        comando.Parameters.Add(new SqlParameter("@softwarebase", filtro.SoftwareBase));                        
                        comando.Parameters.Add(new SqlParameter("@fecha", filtro.Fecha));
                        comando.Parameters.Add(new SqlParameter("@tieneReplica", filtro.TieneReplica));
                        comando.Parameters.Add(new SqlParameter("@PageSize", filtro.PageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", filtro.PageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", filtro.OrderBy));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", filtro.OrderByDirection));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageDto()
                            {
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? true : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                Disco = reader.IsDBNull(reader.GetOrdinal("Disco")) ? string.Empty : reader.GetString(reader.GetOrdinal("Disco")),
                                CapacidadMB = reader.IsDBNull(reader.GetOrdinal("CapacidadMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadMB")),
                                UsadoMB = reader.IsDBNull(reader.GetOrdinal("UsadoMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoMB")),
                                LibreMB = reader.IsDBNull(reader.GetOrdinal("LibreMB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreMB")),
                                Storage = reader.IsDBNull(reader.GetOrdinal("Storage")) ? string.Empty : reader.GetString(reader.GetOrdinal("Storage")),
                                SoftwareBase = reader.IsDBNull(reader.GetOrdinal("SoftwareBase")) ? string.Empty : reader.GetString(reader.GetOrdinal("SoftwareBase")),
                                Volumen = reader.IsDBNull(reader.GetOrdinal("StorageVolumen")) ? string.Empty : reader.GetString(reader.GetOrdinal("StorageVolumen")),
                                NombreTier = reader.IsDBNull(reader.GetOrdinal("Tier")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tier")),
                                TieneReplica = reader.IsDBNull(reader.GetOrdinal("TieneReplica")) ? false : reader.GetBoolean(reader.GetOrdinal("TieneReplica")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Fuente")),
                                FlagServidorServicio = reader.IsDBNull(reader.GetOrdinal("FlagServidorServicio")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagServidorServicio")),
                                PorcentajeLibre = Math.Round(reader.IsDBNull(reader.GetOrdinal("PorcentajeLibre")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeLibre")), 2),                                
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista[0].Total;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorage(PaginacionStorage filtro)"
                    , datos: new object[] { null });
            }

        }

        public override StorageEquipoDto GetStorageEquipo(int equipoId)
        {            
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new StorageEquipoDto();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_DetalleServidor]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", equipoId));

                        using (var adapter = new SqlDataAdapter(comando))
                        {
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0] != null)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        lista.LibreMB = decimal.Parse(ds.Tables[0].Rows[0]["LibreMB"].ToString());
                                        lista.UsadoMB = decimal.Parse(ds.Tables[0].Rows[0]["UsadoMB"].ToString());
                                    }
                                }

                                if (ds.Tables[1] != null)
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        lista.Particiones = new List<StorageParticionDto>();
                                        foreach (DataRow dr in ds.Tables[1].Rows)
                                        {
                                            lista.Particiones.Add(new StorageParticionDto()
                                            {
                                                Disco = dr["Disco"].ToString(),
                                                LibreMB = decimal.Parse(dr["LibreMB"].ToString()),
                                                UsadoMB = decimal.Parse(dr["UsadoMB"].ToString())
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    cnx.Close();

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageEquipo()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageResumenDto> GetStorageResumen2Nivel1(out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen2_N1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageResumenDto()
                            {
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId")),
                                DetalleUbicacion = reader.IsDBNull(reader.GetOrdinal("DetalleUbicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleUbicacion")),                                
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                                ReplicadoTB = reader.IsDBNull(reader.GetOrdinal("ReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ReplicadoTB")),
                                NoReplicadoTB = reader.IsDBNull(reader.GetOrdinal("NoReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NoReplicadoTB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageResumenDto> GetStorageResumen2Nivel2(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen2_N2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ubicacion", filtro.UbicacionId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageResumenDto()
                            {
                                StorageTierId = reader.IsDBNull(reader.GetOrdinal("StorageTierId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageTierId")),
                                NombreTier = reader.IsDBNull(reader.GetOrdinal("NombreTier")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreTier")),
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId")),                                
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                                ReplicadoTB = reader.IsDBNull(reader.GetOrdinal("ReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ReplicadoTB")),
                                NoReplicadoTB = reader.IsDBNull(reader.GetOrdinal("NoReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NoReplicadoTB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageResumenDto> GetStorageResumen2Nivel3(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen2_N3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ubicacion", filtro.UbicacionId));
                        comando.Parameters.Add(new SqlParameter("@storagetier", filtro.StorageTierId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageResumenDto()
                            {
                                StorageId = reader.IsDBNull(reader.GetOrdinal("StorageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageId")),
                                NombreStorage = reader.IsDBNull(reader.GetOrdinal("NombreStorage")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreStorage")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                StorageTierId = reader.IsDBNull(reader.GetOrdinal("StorageTierId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageTierId")),                                
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                                ReplicadoTB = reader.IsDBNull(reader.GetOrdinal("ReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ReplicadoTB")),
                                NoReplicadoTB = reader.IsDBNull(reader.GetOrdinal("NoReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NoReplicadoTB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageResumenDto> GetStorageResumen2Nivel4(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen2_N4]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ubicacion", filtro.UbicacionId));
                        comando.Parameters.Add(new SqlParameter("@storagetier", filtro.StorageTierId));
                        comando.Parameters.Add(new SqlParameter("@storageid", filtro.StorageId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageResumenDto()
                            {
                                NombreReporte = reader.IsDBNull(reader.GetOrdinal("NombreReporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreReporte")),                                
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                StorageId = reader.IsDBNull(reader.GetOrdinal("StorageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageId")),                                
                                StorageTierId = reader.IsDBNull(reader.GetOrdinal("StorageTierId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageTierId")),
                                UbicacionId = reader.IsDBNull(reader.GetOrdinal("UbicacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UbicacionId")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                                ReplicadoTB = reader.IsDBNull(reader.GetOrdinal("ReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ReplicadoTB")),
                                NoReplicadoTB = reader.IsDBNull(reader.GetOrdinal("NoReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NoReplicadoTB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageResumenDto> GetStorageResumen2Nivel5(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageResumenDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen2_N5]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ubicacion", filtro.UbicacionId));
                        comando.Parameters.Add(new SqlParameter("@storagetier", filtro.StorageTierId));
                        comando.Parameters.Add(new SqlParameter("@storageid", filtro.StorageId));
                        comando.Parameters.Add(new SqlParameter("@nombreReporte", filtro.NombreReporte));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageResumenDto()
                            {
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                                ReplicadoTB = reader.IsDBNull(reader.GetOrdinal("ReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ReplicadoTB")),
                                NoReplicadoTB = reader.IsDBNull(reader.GetOrdinal("NoReplicadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NoReplicadoTB"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageNivelDto> GetStorageResumenNivel1(out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageNivelDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen_N1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;                        

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageNivelDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                StorageId = reader.IsDBNull(reader.GetOrdinal("StorageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageId")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageNivelDto> GetStorageResumenNivel2(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageNivelDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen_N2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@storageid", filtro.StorageId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageNivelDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                StorageId = reader.IsDBNull(reader.GetOrdinal("StorageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageId")),
                                StorageVolumen = reader.IsDBNull(reader.GetOrdinal("StorageVolumen")) ? string.Empty : reader.GetString(reader.GetOrdinal("StorageVolumen")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageNivelDto> GetStorageResumenNivel3(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageNivelDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen_N3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@storageid", filtro.StorageId));
                        comando.Parameters.Add(new SqlParameter("@storageVolumen", string.IsNullOrEmpty(filtro.StorageVolumen) ? "" : filtro.StorageVolumen));
                        
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageNivelDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                StorageId = reader.IsDBNull(reader.GetOrdinal("StorageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StorageId")),
                                StorageVolumen = reader.IsDBNull(reader.GetOrdinal("StorageVolumen")) ? string.Empty : reader.GetString(reader.GetOrdinal("StorageVolumen")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                TieneReplica = reader.IsDBNull(reader.GetOrdinal("TieneReplica")) ? false : reader.GetBoolean(reader.GetOrdinal("TieneReplica")),
                                CapacidadTB = reader.IsDBNull(reader.GetOrdinal("CapacidadTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CapacidadTB")),
                                UsadoTB = reader.IsDBNull(reader.GetOrdinal("UsadoTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("UsadoTB")),
                                LibreTB = reader.IsDBNull(reader.GetOrdinal("LibreTB")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LibreTB")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }

        public override List<StorageAplicacionDto> GetStorageResumenNivel4(PaginacionStorage filtro, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<StorageAplicacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Storage_Resumen_N4]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", filtro.EquipoId));                        

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new StorageAplicacionDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                SubclasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("SubclasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("SubclasificacionTecnica")),
                                Criticidad = reader.IsDBNull(reader.GetOrdinal("Criticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Criticidad"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(id: CVTExceptionIds.ErrorTecnologiaDTO
                    , message: "Error en el metodo: GetStorageResumenNivel1()"
                    , datos: new object[] { null });
            }
        }
    }
}
