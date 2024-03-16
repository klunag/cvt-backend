using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class SiemSvc : SiemDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<SiemDTO> GetListado(int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<SiemDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Siem_Reporte_Vista1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new SiemDTO()
                            {
                                servidor = reader.IsDBNull(reader.GetOrdinal("servidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("servidor")),
                                ambiente = reader.IsDBNull(reader.GetOrdinal("ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambiente")),
                                subsidiaria = reader.IsDBNull(reader.GetOrdinal("subsidiaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("subsidiaria")),
                                ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString(reader.GetOrdinal("ip"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count();
                    var resultado = lista.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<SiemDTO> GetListado2(int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<SiemDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Siem_Reporte_Vista2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new SiemDTO()
                            {
                                servidor = reader.IsDBNull(reader.GetOrdinal("servidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("servidor")),
                                ambienter = reader.IsDBNull(reader.GetOrdinal("ambienter")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambienter")),
                                aplicacion = reader.IsDBNull(reader.GetOrdinal("aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("aplicacion")),
                                nombreaplicacion = reader.IsDBNull(reader.GetOrdinal("nombreaplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreaplicacion")),
                                gestionadopor = reader.IsDBNull(reader.GetOrdinal("gestionadopor")) ? string.Empty : reader.GetString(reader.GetOrdinal("gestionadopor")),
                                rolLider = reader.IsDBNull(reader.GetOrdinal("Lider")) ? string.Empty : reader.GetString(reader.GetOrdinal("Lider")),
                                rolTTL = reader.IsDBNull(reader.GetOrdinal("TTL")) ? string.Empty : reader.GetString(reader.GetOrdinal("TTL")),
                                rolUsuario = reader.IsDBNull(reader.GetOrdinal("Usuarios")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuarios")),
                                rolExperto = reader.IsDBNull(reader.GetOrdinal("Experto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count();
                    var resultado = lista.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListado2 Siem Reporte"
                    , new object[] { null });
            }
        }
    }
}
