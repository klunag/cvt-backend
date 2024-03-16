using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

namespace BCP.CVT.Services.Service
{
    public class MotorJustificacionSvc : MotorJustificacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<MotorJustificacionDTO> GetRegistros(PaginacionMotorJustificacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<MotorJustificacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_MotorJustificacion_List]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@ProcesoId", pag.ProcesoId < 0 ? 0 : pag.ProcesoId));
                        comando.Parameters.Add(new SqlParameter("@ProtocoloId", pag.ProtocoloId < 0 ? 0 : pag.ProtocoloId));
                        comando.Parameters.Add(new SqlParameter("@PuertoId", pag.PuertoId < 0 ? 0 : pag.PuertoId));
                        comando.Parameters.Add(new SqlParameter("@ProductoId", pag.ProductoId < 0 ? 0 : pag.ProductoId));
                        comando.Parameters.Add(new SqlParameter("@TecnologiaId", pag.TecnologiaId < 0 ? 0 : pag.TecnologiaId));
                        comando.Parameters.Add(new SqlParameter("@EquipoId", pag.EquipoId < 0 ? 0 : pag.EquipoId));
                        comando.Parameters.Add(new SqlParameter("@Aplicacion", pag.Aplicacion == null ? "" : pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.Fecha == null ? "" : pag.Fecha));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new MotorJustificacionDTO()
                            {
                                MotorJustificacionId = reader.IsDBNull(reader.GetOrdinal("MotorJustificacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MotorJustificacionId")),
                                ProcesoId = reader.IsDBNull(reader.GetOrdinal("ProcesoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProcesoId")),
                                PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId")),
                                ProtocoloId = reader.IsDBNull(reader.GetOrdinal("ProtocoloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProtocoloId")),
                                ProductoId = reader.IsDBNull(reader.GetOrdinal("ProductoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                ProcesoOrigen = reader.IsDBNull(reader.GetOrdinal("ProcesoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoOrigen")),
                                ProcesoDestino = reader.IsDBNull(reader.GetOrdinal("ProcesoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoDestino")),
                                TipoProceso = reader.IsDBNull(reader.GetOrdinal("TipoProceso")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoProceso")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("IpOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpOrigen")),
                                EquipoIdDestino = reader.IsDBNull(reader.GetOrdinal("EquipoIdDestino")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoIdDestino")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                IpDestino = reader.IsDBNull(reader.GetOrdinal("IpDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpDestino")),
                                CodAccion = reader.IsDBNull(reader.GetOrdinal("CodAccion")) ? 0 : reader.GetInt32(reader.GetOrdinal("CodAccion")),
                                Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("Tipo")),
                                CantidadConexiones = reader.IsDBNull(reader.GetOrdinal("CantidadConexiones")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadConexiones")),
                                FechaEscaneo = reader.IsDBNull(reader.GetOrdinal("FechaEscaneo")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaEscaneo")),

                                Proceso = reader.IsDBNull(reader.GetOrdinal("Proceso")) ? string.Empty : reader.GetString(reader.GetOrdinal("Proceso")),
                                Puerto = reader.IsDBNull(reader.GetOrdinal("Puerto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Puerto")),
                                Protocolo = reader.IsDBNull(reader.GetOrdinal("Protocolo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Protocolo")),
                                Producto = reader.IsDBNull(reader.GetOrdinal("Producto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),

                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                RelacionDetalleId = reader.IsDBNull(reader.GetOrdinal("RelacionDetalleId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionDetalleId")),

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
