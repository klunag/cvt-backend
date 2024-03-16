using BCP.CVT.Cross;
using BCP.CVT.DTO.Graficos;
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
    class GraficoSvc : GraficoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<ReportePrincipalDTO> GetDataGraficoRenovacionTecnologica(int anio, int mes, int  dia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReportePrincipalDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GRAFICO_RENOVACION_TECNOLOGIA_TI]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ANIO", anio));
                        comando.Parameters.Add(new SqlParameter("@MES", mes));
                        
                        //comando.Parameters.Add(new SqlParameter("@DIA", dia));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReportePrincipalDTO()
                            {
                                //AreaId = reader.IsDBNull(reader.GetOrdinal("AreaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AreaId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Categoria")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("NroServidoresTotales")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroServidoresTotales")),
                                TotalServidoresObsoletos = reader.IsDBNull(reader.GetOrdinal("NroServidoresObsoletos")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroServidoresObsoletos"))
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
                throw new CVTException(CVTExceptionIds.ErrorGraficoDTO
                    , "Error en el metodo: List<ReportePrincipalDTO> GetDataGraficoRenovacionTecnologica(int anio, int mes, int  dia)"
                    , new object[] { null });
            }
        }
    }
}
