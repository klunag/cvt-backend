using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class TipoEquipoSvc : TipoEquipoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEditTipoEquipo(TipoEquipoDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_TipoEquipo_ConfigurarFlagExcluirKPI]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@TipoEquipoId", objeto.TipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@ModificadoPor", objeto.ModificadoPor));
                        comando.Parameters.Add(new SqlParameter("@FlagExcluirKPI", objeto.FlagExcluirKPI));
                        comando.Parameters.Add(new SqlParameter("@TipoCicloVidaId", objeto.TipoCicloVidaId));
                        comando.Parameters.Add(new SqlParameter("@FlagIncluirEnDiagramaInfra", objeto.FlagIncluirEnDiagramaInfra));
                        comando.Parameters.Add(new SqlParameter("@FlagIncluirHardwareKPI", objeto.FlagIncluirHardwareKPI));
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
                    , "Error al editar FlagExcluirKPI."
                    , new object[] { null });
            }
        }

        public override List<TipoEquipoDTO> GetTipoEquipo(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TipoEquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerTiposDeEquipo]", cnx))
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
                            var objeto = new TipoEquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                CriterioObsolescenciaId = reader.IsDBNull(reader.GetOrdinal("CriterioObsolescenciaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CriterioObsolescenciaId")),
                                NombreCriterioObsolescencia = reader.IsDBNull(reader.GetOrdinal("NombreCriterioObsolescencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreCriterioObsolescencia")),
                                FlagExcluirKPI = reader.GetBoolean(reader.GetOrdinal("FlagExcluirKPI")),
                                NombreTipoCicloVida = reader.IsDBNull(reader.GetOrdinal("NombreTipoCicloVida")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreTipoCicloVida")),
                                FlagIncluirEnDiagramaInfra = reader.GetBoolean(reader.GetOrdinal("FlagIncluirEnDiagramaInfra")),
                                FlagIncluirHardwareKPI = reader.GetBoolean(reader.GetOrdinal("FlagIncluirHardwareKPI"))
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
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoEquipo(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: List<TipoCicloVidaDTO> GetTipoEquipo(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TipoEquipoDTO GetTipoEquipoById(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var objeto = new TipoEquipoDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerTipoDeEquipoPorId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@TipoEquipoId", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto = new TipoEquipoDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                CriterioObsolescenciaId = reader.IsDBNull(reader.GetOrdinal("CriterioObsolescenciaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CriterioObsolescenciaId")),
                                NombreCriterioObsolescencia = reader.IsDBNull(reader.GetOrdinal("NombreCriterioObsolescencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreCriterioObsolescencia")),
                                FlagExcluirKPI = reader.GetBoolean(reader.GetOrdinal("FlagExcluirKPI")),
                                NombreTipoCicloVida = reader.IsDBNull(reader.GetOrdinal("NombreTipoCicloVida")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreTipoCicloVida")),
                                TipoCicloVidaId = reader.IsDBNull(reader.GetOrdinal("TipoCicloVidaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoCicloVidaId")),
                                FlagIncluirEnDiagramaInfra = reader.GetBoolean(reader.GetOrdinal("FlagIncluirEnDiagramaInfra")),
                                FlagIncluirHardwareKPI = reader.GetBoolean(reader.GetOrdinal("FlagIncluirHardwareKPI"))
                            };
                        }
                        reader.Close();
                    }
                    return objeto;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: TipoCicloVidaDTO GetTipoEquipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoCicloVidaDTO
                    , "Error en el metodo: TipoCicloVidaDTO GetTipoEquipoById(int id)"
                    , new object[] { null });
            }
        }
    }
}
