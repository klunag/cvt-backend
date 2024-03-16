﻿using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
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
    public class GuardicoreSvc : GuardicoreDAO
    {
        

        public override List<GuardicoreApiDTO> GetGuardicoreConCvtEquipos(List<GuardicoreApiDTO> datos)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var NombreDatos = (from d in datos
                                       select d.name.ToUpper()).Distinct().ToList();

                    var entidad = (from u in ctx.Equipo
                                   where NombreDatos.Contains(u.Nombre.ToUpper())
                                   select new EquipoDTO
                                   {
                                       Id = u.EquipoId,
                                       Nombre = u.Nombre,
                                       Activo = u.FlagActivo
                                   }).ToList();

                    var lisGuar = (from d in datos
                                   join e in entidad on d.name.ToUpper() equals e.Nombre.ToUpper() into lj1
                                   from e in lj1.DefaultIfEmpty()
                                   select new GuardicoreApiDTO
                                   {
                                       estado = e == null ? 0 : e.Activo == true ? 1 : 2,
                                       name = d.name.ToUpper(),
                                       guest_agent_details = d.guest_agent_details,
                                       last_guest_agent_details_update = d.last_guest_agent_details_update,
                                       ip_addresses = d.ip_addresses,
                                       labels = d.labels
                                   }).ToList();

                    return lisGuar;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAssets
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAssets
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<ConnectionDto> GetGuardicoreConnectionsConCvtEquipos(List<ConnectionDto> datos)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var DatosDestination = (from d in datos
                                            select d.validateDestination).Distinct().ToList();
                    var DatosSourcer = (from d in datos
                                        select d.validateSource).Distinct().ToList();

                    var entidadD = (from u in ctx.Equipo
                                    where DatosDestination.Contains(u.Nombre.ToUpper())
                                    select new EquipoDTO
                                    {
                                        Id = u.EquipoId,
                                        Nombre = u.Nombre,
                                        Activo = u.FlagActivo
                                    }).ToList();

                    var entidadS = (from u in ctx.Equipo
                                    where DatosSourcer.Contains(u.Nombre.ToUpper())
                                    select new EquipoDTO
                                    {
                                        Id = u.EquipoId,
                                        Nombre = u.Nombre,
                                        Activo = u.FlagActivo
                                    }).ToList();

                    var labelAssets = (from d in ctx.GuardicoreAssets
                                       where d.Labels.Contains("APT:")
                                       select new ConnectionDto
                                       {
                                           labels = d.Labels,
                                           hostname = d.hostname
                                       }).ToList();

                    var labelAssetsWithOutAgents = (from d in ctx.GuardicoreEtiquetadoEquipos
                                                    where d.Etiqueta.Contains("APT:") && d.EstadoEtiqueta == true
                                                    select new ConnectionDto
                                                    {
                                                        labelDestino = d.Etiqueta,
                                                        hostname = d.NombreEquipo,
                                                        destination_ip = d.Ip
                                                    }).ToList();

                    foreach (var item in labelAssets) {
                        var splitString = item.labels.Split(';');
                        item.labels = String.Join(";",splitString.Where(x => x.Contains("APT:")).Select(x => x).ToList());
                    }

                    var lisGuar = (from d in datos
                                   join e in entidadD on d.validateDestination.ToUpper() equals e.Nombre.ToUpper() into lj1
                                   from e in lj1.DefaultIfEmpty()
                                   join f in entidadS on d.validateSource.ToUpper() equals f.Nombre.ToUpper() into lj2
                                   from f in lj2.DefaultIfEmpty()
                                   join g in labelAssets on d.validateSource.ToUpper() equals g.hostname into lj3
                                   from g in lj3.DefaultIfEmpty()
                                   join h in labelAssets on d.validateDestination.ToUpper() equals h.hostname into lj4
                                   from h in lj4.DefaultIfEmpty()
                                   select new ConnectionDto
                                   {
                                       id = d.id,
                                       EstadoDestination = e == null ? 0 : e.Activo == true ? 1 : 2,
                                       EstadoSource = f == null ? 0 : f.Activo == true ? 1 : 2,
                                       policy_ruleset = d.policy_ruleset,
                                       source_ip = d.source_ip,
                                       validateSource = d.validateSource,
                                       source_node_type = d.source_node_type,
                                       source_process = d.source_process,
                                       destination_ip = d.destination_ip,
                                       validateDestination = d.validateDestination,
                                       destination_node_type = d.destination_node_type,
                                       destination_process = d.destination_process,
                                       destination_port = d.destination_port,
                                       ip_protocol = d.ip_protocol,
                                       connection_type = d.connection_type,
                                       slot_start_time = d.slot_start_time,
                                       labelsOrigen = g == null ? "" : g.labels,
                                       labelDestino = h == null ? "" : h.labels
                                   }).ToList();

                    foreach (var item in lisGuar.Where(x => string.IsNullOrEmpty(x.validateDestination)))
                    {
                        item.labelDestino = String.Join(";", labelAssetsWithOutAgents.Where(x => x.destination_ip == item.destination_ip).Select(x => x.labelDestino).ToList());
                    }

                    //var lisGuarS = (from d in datos
                    //           join f in entidadS on d.validateSource.ToUpper() equals f.Nombre.ToUpper() into lj2
                    //           from f in lj2.DefaultIfEmpty()
                    //           select new ConnectionDto
                    //           {
                    //               id = d.id,
                    //               EstadoSource = f == null ? 0 : f.Activo == true ? 1 : 2,
                    //               policy_ruleset = d.policy_ruleset,
                    //               source_ip = d.source_ip,
                    //               validateSource = d.validateSource,
                    //               source_node_type = d.source_node_type,
                    //               source_process = d.source_process,
                    //               destination_ip = d.destination_ip,
                    //               validateDestination = d.validateDestination,
                    //               destination_node_type = d.destination_node_type,
                    //               destination_process = d.destination_process,
                    //               destination_port = d.destination_port,
                    //               ip_protocol = d.ip_protocol,
                    //               connection_type = d.connection_type
                    //           }).ToList();

                    return lisGuar;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAssets
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAssets
                    , "Error en el metodo: List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidadoDto> GetGuardicoreConsolidado()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidadoDto()
                            {
                                idestado = reader.IsDBNull(reader.GetOrdinal("idestado")) ? 0 : reader.GetInt32(reader.GetOrdinal("idestado")),
                                estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                cantidadEstado = reader.IsDBNull(reader.GetOrdinal("Cantidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cantidad")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidadoDto> GetGuardicoreConsolidadoDetalle(int idestado, string so)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista1_Detalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idestado", idestado));
                        comando.Parameters.Add(new SqlParameter("@so", so));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidadoDto()
                            {
                                nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                                so = reader.IsDBNull(reader.GetOrdinal("so")) ? string.Empty : reader.GetString(reader.GetOrdinal("so")),
                                fechaescaneo = reader.IsDBNull(reader.GetOrdinal("fechaescaneo")) ? string.Empty : reader.GetString(reader.GetOrdinal("fechaescaneo")),
                                ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString(reader.GetOrdinal("ip")),
                                etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta")),
                                SOTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidadoDto> GetGuardicoreGrupoSO(int estado)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_vista1_SO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idestado", estado));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidadoDto()
                            {
                                idestado = reader.IsDBNull(reader.GetOrdinal("idestado")) ? 0 : reader.GetInt32(reader.GetOrdinal("idestado")),
                                so = reader.IsDBNull(reader.GetOrdinal("so")) ? string.Empty : reader.GetString(reader.GetOrdinal("so")),
                                cantidadSO = reader.IsDBNull(reader.GetOrdinal("cantidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("cantidad"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidadoDto> GetGuardicoreExportar()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista1_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidadoDto()
                            {
                                estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado")),
                                nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                                so = reader.IsDBNull(reader.GetOrdinal("so")) ? string.Empty : reader.GetString(reader.GetOrdinal("so")),
                                fechaescaneo = reader.IsDBNull(reader.GetOrdinal("fechaescaneo")) ? string.Empty : reader.GetString(reader.GetOrdinal("fechaescaneo")),
                                ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString(reader.GetOrdinal("ip")),
                                etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta")),
                                SOTecnologia = reader.IsDBNull(reader.GetOrdinal("soCVT")) ? string.Empty : reader.GetString(reader.GetOrdinal("soCVT"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreApiDTO> GetGuardicoreLabels()
        {
            try
            {
                var lista = new List<GuardicoreApiDTO>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    lista = (from d in ctx.GuardicoreLabels
                             where !String.IsNullOrEmpty(d.CodigoAPT)
                             select new GuardicoreApiDTO
                             {
                                 id = d.LabelId.ToString(),
                                 value = d.Nombrelabel.ToString()
                             }).ToList();
                }
                return lista;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override List<GuardicoreApiDTO> GetGuardicoreLabelsAmbiente()
        {
            try
            {
                var lista = new List<GuardicoreApiDTO>();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    lista = (from d in ctx.GuardicoreLabels
                             where String.IsNullOrEmpty(d.CodigoAPT) && d.Nombrelabel.Contains("AMBIENTE")
                             select new GuardicoreApiDTO
                             {
                                 id = d.LabelId.ToString(),
                                 value = d.Nombrelabel.ToString()
                             }).ToList();
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreComboEstado()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Combo_Estado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                idestado = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id")),
                                nombreestado = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1(string estado, string apps, string gest)
        {
            try
            {
                estado = string.IsNullOrEmpty(estado) ? null : estado;
                apps = string.IsNullOrEmpty(apps) ? null : apps;
                gest = string.IsNullOrEmpty(gest) ? null : gest;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@apps", apps));
                        comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                cantSiEsta = reader.IsDBNull(reader.GetOrdinal("SiCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("SiCVT")),
                                cantNoEsta = reader.IsDBNull(reader.GetOrdinal("NoCVT")) ? 0 : reader.GetInt32(reader.GetOrdinal("NoCVT"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2(string estado, string apps, string gest)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@apps", apps));
                        comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                TotalApps = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1Nivel2(string estado, string apps, string gest)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab1_Nivel2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@apps", apps));
                        comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("EquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoOrigen")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("IpOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpOrigen")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),
                                IpDestino = reader.IsDBNull(reader.GetOrdinal("IpDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpDestino")),
                                EstadoCVT = reader.IsDBNull(reader.GetOrdinal("estaCVT")) ? string.Empty : reader.GetString(reader.GetOrdinal("estaCVT"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2Nivel2(string apps)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab2_Nivel2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@apps", apps));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("DetalleComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleComponente")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1Exportar(string estado, string apps, string gest)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab1_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@apps", apps));
                        comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("EquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoOrigen")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("IpOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpOrigen")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),
                                IpDestino = reader.IsDBNull(reader.GetOrdinal("IpDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IpDestino")),
                                EstadoCVT = reader.IsDBNull(reader.GetOrdinal("estaCVT")) ? string.Empty : reader.GetString(reader.GetOrdinal("estaCVT"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2Exportar(string estado, string apps, string gest)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreConsolidado2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Vista2_Tab2_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@apps", apps));
                        comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreConsolidado2DTO()
                            {
                                codigoapp = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                IpOrigen = reader.IsDBNull(reader.GetOrdinal("DetalleComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleComponente")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override List<GuardicoreFase2DTO> GetGuardicoreFase2Tab2Listado()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreFase2DTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Guardicore_Fase2_Tab2_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        //comando.Parameters.Add(new SqlParameter("@estado", estado));
                        //comando.Parameters.Add(new SqlParameter("@apps", apps));
                        //comando.Parameters.Add(new SqlParameter("@gest", gest));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreFase2DTO()
                            {
                                dominio = reader.IsDBNull(reader.GetOrdinal("dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("dominio")),
                                subdominio = reader.IsDBNull(reader.GetOrdinal("subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("subdominio")),
                                tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                fechaEscane = reader.IsDBNull(reader.GetOrdinal("escaneo")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("escaneo"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<GuardicoreEtiquetado> GetEtiquetado(string etiqueta, string clave)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreEtiquetado>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreEtiquetado_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiqueta));
                        comando.Parameters.Add(new SqlParameter("@clave", clave));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreEtiquetado()
                            {
                                id = reader.IsDBNull(reader.GetOrdinal("GuardicoreEtiquetadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("GuardicoreEtiquetadoId")),
                                clave = reader.IsDBNull(reader.GetOrdinal("palabraclave")) ? string.Empty : reader.GetString(reader.GetOrdinal("palabraclave")),
                                etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta")),
                                comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentario")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override GuardicoreEtiquetado GetEtiquetadoId(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var objeto = new GuardicoreEtiquetado();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreEtiquetado_Editar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto.id = reader.IsDBNull(reader.GetOrdinal("GuardicoreEtiquetadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("GuardicoreEtiquetadoId"));
                            objeto.clave = reader.IsDBNull(reader.GetOrdinal("palabraclave")) ? string.Empty : reader.GetString(reader.GetOrdinal("palabraclave"));
                            objeto.etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta"));
                            objeto.comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentario"));
                        }
                        reader.Close();
                    }

                    return objeto;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override int SetEtiquetado(string clave, string etiquetado, string comentario, string matricula)
        {
            try
            {
                var respuesta = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreEtiquetado_Insertar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@clave", clave));
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiquetado));
                        comando.Parameters.Add(new SqlParameter("@comentario", comentario));
                        comando.Parameters.Add(new SqlParameter("@usuario", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            respuesta = reader.IsDBNull(reader.GetOrdinal("respuesta")) ? 0 : reader.GetInt32(reader.GetOrdinal("respuesta"));
                        }
                        reader.Close();
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override int ActualizarRegistro(int id, string clave, string etiqueta, string comentario, string matricula)
        {
            try
            {
                var respuesta = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreEtiquetado_Actualizar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));
                        comando.Parameters.Add(new SqlParameter("@clave", clave));
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiqueta));
                        comando.Parameters.Add(new SqlParameter("@comentario", comentario));
                        comando.Parameters.Add(new SqlParameter("@usuario", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            respuesta = reader.IsDBNull(reader.GetOrdinal("respuesta")) ? 0 : reader.GetInt32(reader.GetOrdinal("respuesta"));
                        }
                        reader.Close();
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override void EliminarRegistro(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreEtiquetado_Eliminar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));

                        comando.ExecuteNonQuery();
                        cnx.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override List<GuardicoreServidorRelacionDTO> GetServidorRelacion(string etiqueta, string comodin, int prioridad, int tipo)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GuardicoreServidorRelacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreServidorRelacion_Listado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiqueta));
                        comando.Parameters.Add(new SqlParameter("@comodin", comodin));
                        comando.Parameters.Add(new SqlParameter("@prioridad", prioridad.ToString() == "0" ? null : prioridad.ToString()));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo.ToString() == "0" ? null : tipo.ToString()));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GuardicoreServidorRelacionDTO()
                            {
                                id = reader.IsDBNull(reader.GetOrdinal("GuardicoreEtiquetadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("GuardicoreEtiquetadoId")),
                                etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta")),
                                comodin = reader.IsDBNull(reader.GetOrdinal("comodin")) ? string.Empty : reader.GetString(reader.GetOrdinal("comodin")),
                                prioridad = reader.IsDBNull(reader.GetOrdinal("prioridad")) ? 0 : reader.GetInt32(reader.GetOrdinal("prioridad")),
                                comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentario")),
                                tipoaplicacionrelacion = reader.IsDBNull(reader.GetOrdinal("tipoaplicacionrelacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipoaplicacionrelacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override GuardicoreServidorRelacionDTO GetServidorRelacionId(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var objeto = new GuardicoreServidorRelacionDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreServidorRelacion_Buscar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto.id = reader.IsDBNull(reader.GetOrdinal("GuardicoreEtiquetadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("GuardicoreEtiquetadoId"));
                            objeto.etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("etiqueta"));
                            objeto.comodin = reader.IsDBNull(reader.GetOrdinal("comodin")) ? string.Empty : reader.GetString(reader.GetOrdinal("comodin"));
                            objeto.prioridad = reader.IsDBNull(reader.GetOrdinal("prioridad")) ? 0 : reader.GetInt32(reader.GetOrdinal("prioridad"));
                            objeto.comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentario"));
                            objeto.tipoaplicacionrelacion = reader.IsDBNull(reader.GetOrdinal("tipoaplicacionrelacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipoaplicacionrelacion"));
                        }
                        reader.Close();
                    }

                    return objeto;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override int ActualizarServidorRelacion(int id, string etiqueta, string comodin, int prioridad, string comentario, int tipo, string matricula)
        {
            try
            {
                var respuesta = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreServidorRelacion_Actualizar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiqueta));
                        comando.Parameters.Add(new SqlParameter("@comodin", comodin));
                        comando.Parameters.Add(new SqlParameter("@prioridad", prioridad));
                        comando.Parameters.Add(new SqlParameter("@comentario", comentario));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@usuario", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            respuesta = reader.IsDBNull(reader.GetOrdinal("respuesta")) ? 0 : reader.GetInt32(reader.GetOrdinal("respuesta"));
                        }
                        reader.Close();
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override void EliminarServidorRelacion(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreServidorRelacion_Eliminar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));

                        comando.ExecuteNonQuery();
                        cnx.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }
        public override int SetServidorRelacion(string etiqueta, string comodin, int prioridad, string comentario, int tipo, string matricula)
        {
            try
            {
                var respuesta = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GuardicoreServidorRelacion_Insertar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@etiqueta", etiqueta));
                        comando.Parameters.Add(new SqlParameter("@comodin", comodin));
                        comando.Parameters.Add(new SqlParameter("@prioridad", prioridad));
                        comando.Parameters.Add(new SqlParameter("@comentario", comentario));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@usuario", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            respuesta = reader.IsDBNull(reader.GetOrdinal("respuesta")) ? 0 : reader.GetInt32(reader.GetOrdinal("respuesta"));
                        }
                        reader.Close();
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetInstancias()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAplicacionMatricula(string matricula, string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<AplicacionMatriculaDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ObtenerAppsUsuario]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var codigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT"));
                            var item = new AplicacionMatriculaDTO() { CodigoAPT = codigoAPT };
                            dataList.Add(item);
                        }
                        reader.Close();
                    }

                    var entidad = (from u in dataList
                                   where (string.IsNullOrEmpty(filtro) || u.CodigoAPT.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.CodigoAPT
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.CodigoAPT,
                                       Descripcion = u.CodigoAPT,
                                       value = u.CodigoAPT
                                   }).ToList();

                    return entidad;
                }
                return null;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionesByMatricula(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionesByMatricula(string filtro)"
                    , new object[] { null });
            }
        }
    }
}
