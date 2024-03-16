using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class BCPUnidadSvc : BCPUnidadDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoePorFiltro(string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_TRIBUCOE_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@filtro", filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                //Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                //Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                //value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_SQUAD_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codUnidad", codigoUnidad));
                        comando.Parameters.Add(new SqlParameter("@filtro", string.IsNullOrEmpty(filtro) ? DBNull.Value : (object)filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
        }
    }
}
