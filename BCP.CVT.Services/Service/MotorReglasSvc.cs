using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

namespace BCP.CVT.Services.Service
{
    public class MotorReglasSvc : MotorReglasDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<MotorReglasDTO> GetMotorReglas(PaginacionMotorReglas pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<MotorReglasDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_MotorRegla_List]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ProcesoId", pag.ProcesoId < 0 ? 0: pag.ProcesoId));
                        comando.Parameters.Add(new SqlParameter("@ProtocoloId", pag.ProtocoloId < 0 ? 0: pag.ProtocoloId));
                        comando.Parameters.Add(new SqlParameter("@PuertoId", pag.PuertoId < 0 ? 0 : pag.PuertoId));
                        comando.Parameters.Add(new SqlParameter("@Producto", pag.Producto == null ? "" : pag.Producto));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new MotorReglasDTO()
                            {
                                MotorReglaId = reader.IsDBNull(reader.GetOrdinal("MotorReglaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MotorReglaId")),
                                ProcesoId = reader.IsDBNull(reader.GetOrdinal("ProcesoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProcesoId")),
                                PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId")),
                                ProtocoloId = reader.IsDBNull(reader.GetOrdinal("ProtocoloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProtocoloId")),
                                ProductoId = reader.IsDBNull(reader.GetOrdinal("ProductoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductoId")),

                                Proceso = reader.IsDBNull(reader.GetOrdinal("Proceso")) ? string.Empty : reader.GetString(reader.GetOrdinal("Proceso")),
                                Puerto = reader.IsDBNull(reader.GetOrdinal("Puerto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Puerto")),
                                Protocolo = reader.IsDBNull(reader.GetOrdinal("Protocolo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Protocolo")),
                                Producto = reader.IsDBNull(reader.GetOrdinal("Producto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto")),
                                ProductoCodigo = reader.IsDBNull(reader.GetOrdinal("ProductoCodigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductoCodigo")),
                                ProcesoTipo = reader.IsDBNull(reader.GetOrdinal("ProcesoTipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProcesoTipo")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),

                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                
                            };

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    
                    totalRows = lista.Count();
                    var resultado = lista.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;

                }

            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetMotorReglas(PaginacionMotorReglas pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override string CambiarEstado(int id, bool estado, string usuario)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_MotorRegla_Estado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@MotorReglaId", id);
                        comando.Parameters.AddWithValue("@FlagActivo", estado);
                        comando.Parameters.AddWithValue("@UsuarioModificacion", usuario);
                        resultado = comando.ExecuteScalar().ToString();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
        }

        public override string AddOrEditMotorReglas(MotorReglasDTO objeto)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_MotorRegla_Insert]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@ProductoId", objeto.ProductoId);
                        comando.Parameters.AddWithValue("@ProcesoId", objeto.ProcesoId);
                        comando.Parameters.AddWithValue("@ProtocoloId", objeto.ProtocoloId);
                        comando.Parameters.AddWithValue("@PuertoId", objeto.PuertoId);
                        comando.Parameters.AddWithValue("@ProcesoTipo", objeto.ProcesoTipo);
                        comando.Parameters.AddWithValue("@Funcion", objeto.Funcion);
                        comando.Parameters.AddWithValue("@FlagActivo", objeto.FlagActivo);
                        comando.Parameters.AddWithValue("@UsuarioCreacion", objeto.UsuarioCreacion);
                        comando.Parameters.Add(new SqlParameter { ParameterName = "@Resultado", Value = "", Direction = System.Data.ParameterDirection.InputOutput, SqlDbType = System.Data.SqlDbType.VarChar });

                        comando.ExecuteNonQuery();
                        resultado = comando.Parameters["@Resultado"].Value.ToString();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Unidad_Fondeo_Insert(entidad)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Unidad_Fondeo_Insert(entidad)"
                    , new object[] { null });
            }
        }
    }
}
