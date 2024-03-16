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
    public class MotorDeudaTecnicaSvc : MotorDeudaTecnicaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<MotorDeudaTecnicaDTO> GetRegistros(PaginacionDeudaTecnica pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<MotorDeudaTecnicaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_MotorDeudaTecnica_List]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ProcesoId", pag.ProcesoId < 0 ? 0 : pag.ProcesoId));
                        comando.Parameters.Add(new SqlParameter("@ProtocoloId", pag.ProtocoloId < 0 ? 0 : pag.ProtocoloId));
                        comando.Parameters.Add(new SqlParameter("@PuertoId", pag.PuertoId < 0 ? 0 : pag.PuertoId));
                        comando.Parameters.Add(new SqlParameter("@ProductoId", pag.ProductoId < 0 ? 0 : pag.ProductoId));
                        comando.Parameters.Add(new SqlParameter("@Aplicacion", pag.Aplicacion == null ? "" : pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.Fecha == null ? "" : pag.Fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new MotorDeudaTecnicaDTO()
                            {
                                MotorDeudaTecnicaId = reader.IsDBNull(reader.GetOrdinal("MotorDeudaTecnicaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MotorDeudaTecnicaId")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                ProcesoId = reader.IsDBNull(reader.GetOrdinal("ProcesoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProcesoId")),
                                ProcesoOrigen = reader.IsDBNull(reader.GetOrdinal("ProcesoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoOrigen")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("IpOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpOrigen")),

                                EquipoIdDestino = reader.IsDBNull(reader.GetOrdinal("EquipoIdDestino")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoIdDestino")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                IpDestino = reader.IsDBNull(reader.GetOrdinal("IpDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpDestino")),
                                PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId")),
                                Puerto = reader.IsDBNull(reader.GetOrdinal("Puerto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Puerto")),
                                Proceso = reader.IsDBNull(reader.GetOrdinal("Proceso")) ? string.Empty : reader.GetString(reader.GetOrdinal("Proceso")),

                                ProtocoloId = reader.IsDBNull(reader.GetOrdinal("ProtocoloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProtocoloId")),
                                Protocolo = reader.IsDBNull(reader.GetOrdinal("Protocolo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Protocolo")),
                                ProductoId = reader.IsDBNull(reader.GetOrdinal("ProductoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                Producto = reader.IsDBNull(reader.GetOrdinal("Producto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto")),
                                CodAccion = reader.IsDBNull(reader.GetOrdinal("CodAccion")) ? 0 : reader.GetInt32(reader.GetOrdinal("CodAccion")),

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
    }
}
