using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class InfoAplicacionSvc : InfoAplicacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<InfoAplicacionDTO> PostInfoApliListado(string username,string soportado,string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber,int pageSize, out int totalRows)
        {
            totalRows = 0;
            codigoAPT = codigoAPT == "0" ? null : codigoAPT;

            var diaInicial = 0;
            var mesInicial = 0;
            var anioInicial = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<InfoAplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var cmd = new SqlCommand("[cvt].[USP_ObtenerUltimoDiaRelacion]", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            diaInicial = reader.IsDBNull(reader.GetOrdinal("DiaRegistro")) ? 0 : reader.GetInt32(reader.GetOrdinal("DiaRegistro"));
                            mesInicial = reader.IsDBNull(reader.GetOrdinal("MesRegistro")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesRegistro"));
                            anioInicial = reader.IsDBNull(reader.GetOrdinal("AnioRegistro")) ? 0 : reader.GetInt32(reader.GetOrdinal("AnioRegistro"));
                        }
                        reader.Close();
                    }

                    var fechaProceso = DateTime.Now;

                    if (anioInicial == 0 && mesInicial == 0 && diaInicial == 0)
                        fechaProceso = DateTime.Now;
                    else
                        fechaProceso = new DateTime(anioInicial, mesInicial, diaInicial);

                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigo", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@dominio", dominio));
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));
                        comando.Parameters.Add(new SqlParameter("@comboJE", comboJE));
                        comando.Parameters.Add(new SqlParameter("@comboLU", comboLU));
                        comando.Parameters.Add(new SqlParameter("@gestion", soportado));
                        comando.Parameters.Add(new SqlParameter("@username", username));
                        comando.Parameters.Add(new SqlParameter("@DiaRegistro", fechaProceso.Day));
                        comando.Parameters.Add(new SqlParameter("@MesRegistro", fechaProceso.Month));
                        comando.Parameters.Add(new SqlParameter("@AnioRegistro", fechaProceso.Year));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InfoAplicacionDTO()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                NombreAPT = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FechaValida = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }


                totalRows = respuesta.Count();
                var resultado = respuesta.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public override List<InfoAplicacionDTO> GetInfoAppExportar(string username, string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU)
        {
            codigoAPT = codigoAPT == "0" ? null : codigoAPT;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<InfoAplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigo", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@dominio", dominio));
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));
                        comando.Parameters.Add(new SqlParameter("@comboJE", comboJE));
                        comando.Parameters.Add(new SqlParameter("@comboLU", comboLU));
                        comando.Parameters.Add(new SqlParameter("@gestion", soportado));
                        comando.Parameters.Add(new SqlParameter("@username", username));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InfoAplicacionDTO()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                NombreAPT = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FechaValida = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                gestionadopor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                TTL = reader.IsDBNull(reader.GetOrdinal("TTL")) ? string.Empty : reader.GetString(reader.GetOrdinal("TTL")),
                                TL = reader.IsDBNull(reader.GetOrdinal("TL")) ? string.Empty : reader.GetString(reader.GetOrdinal("TL")),
                                owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                PO = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                jefe = reader.IsDBNull(reader.GetOrdinal("Jefe")) ? string.Empty : reader.GetString(reader.GetOrdinal("Jefe")),
                                experto = reader.IsDBNull(reader.GetOrdinal("Experto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto"))
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<CustomAutocompleteApplication> GetInfoApliComboSoportado()
        {
            try
            {
                var respuesta = new List<CustomAutocompleteApplication>();

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    respuesta = (from a in ctx.GestionadoPor
                                 where a.FlagActivo
                                 select new CustomAutocompleteApplication()
                                 {
                                     Id = a.GestionadoPorId.ToString(),
                                     Descripcion = a.Nombre,
                                     Value = a.Nombre
                                 }).ToList();
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<CustomAutocompleteApplication> GetInfoApliComboJefe()
        {
            try
            {
                var respuesta = new List<CustomAutocompleteApplication>();

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    respuesta = (from a in ctx.ApplicationManagerCatalog
                                 where a.isActive && a.applicationManagerId == 2
                                 select new CustomAutocompleteApplication()
                                 {
                                     Id = a.managerName.ToString(),
                                     Descripcion = a.managerName,
                                     Value = a.managerName
                                 }).Distinct().ToList();
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<CustomAutocompleteApplication> GetInfoApliComboLider()
        {
            try
            {
                var respuesta = new List<CustomAutocompleteApplication>();

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    respuesta = (from a in ctx.ApplicationManagerCatalog
                                 where a.isActive && a.applicationManagerId == 4
                                 select new CustomAutocompleteApplication()
                                 {
                                     Id = a.managerName.ToString(),
                                     Descripcion = a.managerName,
                                     Value = a.managerName
                                 }).Distinct().ToList();
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<InfoAplicacionDTO> GetHistoricoAplicacion(string username, string codigoAPT, int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<InfoAplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_Historico]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigo", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@username", username));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InfoAplicacionDTO()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                NombreAPT = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FechaValida = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                totalRows = respuesta.Count();
                var resultado = respuesta.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<InfoAplicacionDTO> GetConsultaValidadaHistorico(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;

            codigoAPT = codigoAPT == "0" ? null : codigoAPT;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<InfoAplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_ConsultaValidacion_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigo", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@dominio", dominio));
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));
                        comando.Parameters.Add(new SqlParameter("@comboJE", comboJE));
                        comando.Parameters.Add(new SqlParameter("@comboLU", comboLU));
                        comando.Parameters.Add(new SqlParameter("@gestion", soportado));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new InfoAplicacionDTO()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")).ToString(),
                                NombreAPT = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")).ToString(),
                                FechaValida = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")).ToString(),
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                totalRows = respuesta.Count();
                var resultado = respuesta.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string GetRolAplicacion(string username)
        {
            try
            {
                string rol = "";

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    rol = (from a in ctx.ApplicationManagerCatalog
                           where a.isActive &&
                                (a.applicationManagerId == 2 ||
                                 a.applicationManagerId == 4 ||
                                 a.applicationManagerId == 6 ||
                                 a.applicationManagerId == 14) &&
                            a.username == username
                           select a.applicationManagerId).FirstOrDefault().ToString();
                }

                return rol;
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
        }

        public override List<DetalleInforAplicacionNivel0> GetDetalleTecnicoNivel0(string codigoAPT)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<DetalleInforAplicacionNivel0>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@app", codigoAPT));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DetalleInforAplicacionNivel0()
                            {
                                idSubdominio = reader.IsDBNull(reader.GetOrdinal("SubdominioId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubdominioId")),
                                subdominio = reader.IsDBNull(reader.GetOrdinal("subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("subdominio")),
                                tipoComponente = reader.IsDBNull(reader.GetOrdinal("TipoStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoStr")),
                                nombreComponente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                //cantidad = reader.IsDBNull(reader.GetOrdinal("cantidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("cantidad")),
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<DetalleInforAplicacionNivel1> GetDetalleTecnicoNivel1(string username, string codigoAPT, int subdominio, int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<DetalleInforAplicacionNivel1>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_DetalleAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@username", username));
                        comando.Parameters.Add(new SqlParameter("@app", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DetalleInforAplicacionNivel1()
                            {
                                tipoComponente = reader.IsDBNull(reader.GetOrdinal("tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo")),
                                nombreComponente = reader.IsDBNull(reader.GetOrdinal("componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("componente")),
                                ambiente = reader.IsDBNull(reader.GetOrdinal("ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambiente")),
                                tecnologia = reader.IsDBNull(reader.GetOrdinal("tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("tecnologia")),
                            };
                            respuesta.Add(objeto);
                        }
                        reader.Close();
                    }
                }

                totalRows = respuesta.Count();
                var resultado = respuesta.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override int GetValidarInfoApp(string username, string codigoAPT)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var respuesta = new List<DetalleInforAplicacionNivel1>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Portafolio_InformacionAplicacion_ValidarApp]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigo", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@username", username));
                        comando.ExecuteNonQuery();
                        cnx.Close();
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
            }

            return 1;
        }
    }
}
