using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Graficos;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace BCP.CVT.Services.Service
{
    public class ReporteSvc : ReporteDAO
    {

        public override string GetRutaConsolidadoByTipo(int Id)
        {
            try
            {
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_CONSOLIDADOS_REPORTES_RUTA);
                int TipoConsolidadoId = Id;
                string filename = string.Empty;
                string ruta = string.Empty;

                switch (TipoConsolidadoId)
                {
                    case (int)ETipoConsolidado.Aplicaciones:
                        filename = "ConsolidadoAplicaciones.csv";
                        break;
                    case (int)ETipoConsolidado.Tecnologias:
                        filename = "ConsolidadoTecnologias.csv";
                        break;
                    case (int)ETipoConsolidado.Relaciones:
                        filename = "ConsolidadoRelaciones.csv";
                        break;
                    case (int)ETipoConsolidado.Servidores:
                        filename = "ConsolidadoEquipos.csv";
                        break;
                }

                ruta = Path.Combine(parametro.Valor, filename);

                return ruta;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override bool ExisteArchivoConsolidado(int Id)
        {
            try
            {
                string ruta = string.Empty;
                ruta = GetRutaConsolidadoByTipo(Id);
                return File.Exists(ruta) ? true : false;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAgrupacionDto> GetAgrupacion(string tipoEquipoIds, string subdominios, string agrupacion, string subsidiaria, string estados, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAgrupacionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Dashboard_AgrupacionAplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", tipoEquipoIds));
                        comando.Parameters.Add(new SqlParameter("@agrupacion", agrupacion));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", subsidiaria));
                        comando.Parameters.Add(new SqlParameter("@estados", estados));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAgrupacionDto()
                            {
                                Obsoletos = reader.IsDBNull(reader.GetOrdinal("Obsoletos")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoletos")),
                                NoObsoletos = reader.IsDBNull(reader.GetOrdinal("NoObsoletos")) ? 0 : reader.GetInt32(reader.GetOrdinal("NoObsoletos")),
                                Agrupacion = reader.IsDBNull(reader.GetOrdinal("Agrupacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Agrupacion"))
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
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAgrupacionDetalleDto> GetAgrupacionDetalle(string tipoEquipo, string subdominios, string agrupacion, string subsidiaria, string estados, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAgrupacionDetalleDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_AgrupacionAplicacionesDetallado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", tipoEquipo));
                        comando.Parameters.Add(new SqlParameter("@agrupacion", agrupacion));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", subsidiaria));
                        comando.Parameters.Add(new SqlParameter("@estados", estados));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", fecha));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAgrupacionDetalleDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Agrupacion = reader.IsDBNull(reader.GetOrdinal("Agrupacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Agrupacion"))
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetAplicacionTecnologia(PaginaReporteGerencia pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                try
                {
                    pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    pag.FechaFiltro = DateTime.Now;
                }

                var cadenaConexion = Constantes.CadenaConexion;
                var mesesTipoCicloVida = this.GetMesesObsolescencia();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                String valorTipoEquipoCD = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO_VALIDACION").Valor;
                string nombreSP = "";
                var lista = new List<ReporteGerenciaDivisionDto>();
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_AplicacionTecnologia_V3]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_AplicacionTecnologia]";
                //}



                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));
                        comando.Parameters.Add(new SqlParameter("@dominio", pag.DominioId));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.SubdominioId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                Relevancia = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                FactoresKPI_FLooking = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW")),
                                TipoEquipoValidarCD = valorTipoEquipoCD,
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoCicloVidaNombre = reader.IsDBNull(reader.GetOrdinal("TipoCicloVidaNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoCicloVidaNombre")),
                                EstadoIdTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoIdTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoIdTecnologia")),
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ConsolidadoDto> GetConsolidadoBatch()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ConsolidadoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Repositorio_ConsolidadoAplicacionEquipos]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ConsolidadoDto()
                            {
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                AmbienteRelacion = reader.IsDBNull(reader.GetOrdinal("AmbienteRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AmbienteRelacion")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CaracteristicaEquipo")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion")),
                                Experto_Especialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                Gestor_UsuarioAutorizador_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                SO = reader.IsDBNull(reader.GetOrdinal("SO")) ? string.Empty : reader.GetString(reader.GetOrdinal("SO")),
                                Temporal = reader.IsDBNull(reader.GetOrdinal("Temporal")) ? string.Empty : reader.GetString(reader.GetOrdinal("Temporal")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion"))
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
                    , "Error en el metodo: GetDetalleEvolucionSubdominios()"
                    , new object[] { null });
            }
        }

        public override List<VentanaDto> GetConsolidadoVentana()
        {
            try
            {
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var equiposSO = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         where b.SubdominioId == 36
                                         && a.AnioRegistro == fechaConsulta.Year
                                         && a.MesRegistro == fechaConsulta.Month
                                         && a.DiaRegistro == fechaConsulta.Day

                                         select new
                                         {
                                             EquipoId = a.EquipoId,
                                             TecnologiaId = b.TecnologiaId,
                                             SistemaOperativo = b.ClaveTecnologia
                                         }).Distinct();


                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId
                                         join c in ctx.Ambiente on u.AmbienteId equals c.AmbienteId
                                         join dom in ctx.Dominio on u.DominioServidorId equals dom.DominioId
                                         join d in equiposSO on u.EquipoId equals d.EquipoId into lj1
                                         from d in lj1.DefaultIfEmpty()
                                         where u.FlagActivo && c.Activo && u.TipoEquipoId == 1 //Servidores                                         
                                         //orderby u.Nombre
                                         select new VentanaDto()
                                         {
                                             Equipo = u.Nombre,
                                             FlagTemporal = u.FlagTemporal,
                                             Ambiente = new AmbienteDTO()
                                             {
                                                 DetalleAmbiente = c.DetalleAmbiente,
                                                 DiaFin = c.DiaFin,
                                                 DiaFin_HoraFin = c.DiaFin_HoraFin,
                                                 DiaFin_HoraInicio = c.DiaFin_HoraInicio,
                                                 DiaInicio = c.DiaInicio,
                                                 DiaInicio_HoraFin = c.DiaInicio_HoraFin,
                                                 DiaInicio_HoraInicio = c.DiaInicio_HoraInicio
                                             },
                                             Dominio = dom.Nombre,
                                             TipoEquipo = b.Nombre,
                                             CaracteristicaEquipo = u.CaracteristicaEquipo.HasValue ? u.CaracteristicaEquipo.Value : 0,
                                             SO = d == null ? "-" : d.SistemaOperativo
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleEvolucionSubdominios()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoSubdominioDto> GetDetalleEvolucionSubdominios(PaginacionDetalleGraficoSubdominio pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var meses = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                var lista = new List<ReporteDetalladoSubdominioDto>();
                string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_DetalladoAcumuladoPorSubdominios_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_DetalladoAcumuladoPorSubdominios]";
                //}
                try
                {
                    pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    pag.FechaFiltro = DateTime.Now;
                }

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaToString));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", pag.TipoEquipoToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoSubdominioDto()
                            {
                                IndicadorMeses1 = meses,
                                IndicadorMeses2 = meses2,
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleEvolucionSubdominios()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoSubdominioDto> GetDetalleGrafico(PaginacionDetalleGraficoSubdominio pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var meses = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "";

                var lista = new List<ReporteDetalladoSubdominioDto>();

                try
                {
                    pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    pag.FechaFiltro = DateTime.Now;
                }

                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominioDetalle_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominioDetalle]";
                //}
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipos", pag.TipoEquipoToString));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@dia", pag.FechaFiltro.Day));
                        comando.Parameters.Add(new SqlParameter("@mes", pag.FechaFiltro.Month));
                        comando.Parameters.Add(new SqlParameter("@anio", pag.FechaFiltro.Year));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaToString));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.CommandTimeout = 60000000;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        //if (parametroAppliance == "true")
                        //{
                            while (reader.Read())
                            {
                                var objeto = new ReporteDetalladoSubdominioDto()
                                {
                                    IndicadorMeses1 = meses,
                                    IndicadorMeses2 = meses2,
                                    TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                    ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                    Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                    FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                    Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                    Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                    TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                    TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                    EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                    TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                    FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                    TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                    Appliance = reader.IsDBNull(reader.GetOrdinal("Appliance")) ? string.Empty : reader.GetString(reader.GetOrdinal("Appliance")),
                                    EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                };
                                lista.Add(objeto);
                            }
                        //}
                        //else
                        //{
                        //    while (reader.Read())
                        //    {

                        //        var objeto = new ReporteDetalladoSubdominioDto()
                        //        {
                        //            IndicadorMeses1 = meses,
                        //            IndicadorMeses2 = meses2,
                        //            TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                        //            ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                        //            Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                        //            FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                        //            Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                        //            Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                        //            Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                        //            TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                        //            TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                        //            EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                        //            TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                        //            FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                        //            TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId"))
                        //        };
                        //        lista.Add(objeto);
                        //    }

                        //}
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoSubdominioDto> GetDetalleTecnologiasGrafico(PaginacionDetalleGraficoSubdominio pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var meses = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                var lista = new List<ReporteDetalladoSubdominioDto>();
                DateTime fecha = DateTime.Now;
                string nombreSP = "";
                try
                {
                    if (!string.IsNullOrWhiteSpace(pag.Fecha))
                        fecha = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    else
                        fecha = pag.FechaFiltro;
                }
                catch (Exception)
                {
                    fecha = pag.FechaFiltro;
                }
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominio_TecnologiasEquipos_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominio_TecnologiasEquipos]";
                //}

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipos", pag.TipoEquipoToString));
                        comando.Parameters.Add(new SqlParameter("@tiposTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@dia", fecha.Day));
                        comando.Parameters.Add(new SqlParameter("@mes", fecha.Month));
                        comando.Parameters.Add(new SqlParameter("@anio", fecha.Year));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoSubdominioDto()
                            {
                                IndicadorMeses1 = meses,
                                IndicadorMeses2 = meses2,
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAcumuladoDto> GetEvolucionSubdominios(string tipoEquipos, string subdominios, string subsidiaria, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAcumuladoDto>();
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_AcumuladoPorSubdominios_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_AcumuladoPorSubdominios]";
                //}
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", tipoEquipos));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", subsidiaria));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", fecha));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAcumuladoDto()
                            {
                                Anio = reader.IsDBNull(reader.GetOrdinal("Anio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Anio")),
                                Equipos = reader.IsDBNull(reader.GetOrdinal("Equipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("Equipos")),
                                Mes = reader.IsDBNull(reader.GetOrdinal("Mes")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mes")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            if (objeto.Anio != 0 && objeto.Mes != 0)
                            {
                                objeto.Fecha = Utilitarios.DevolverNombreFecha(new DateTime(objeto.Anio, objeto.Mes, 1, 0, 0, 0));
                                objeto.FechaFin = new DateTime(objeto.Anio, objeto.Mes, 1, 0, 0, 0);
                            }
                            else
                                objeto.FechaFin = DateTime.MinValue;

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                    {
                        var listaFinal = lista.Where(x => x.FechaFin < DateTime.Now)
                            .OrderByDescending(x => x.Anio).ThenByDescending(x => x.Mes).ToList();

                        if (listaFinal.Count > 0)
                        {
                            var primero = listaFinal[0];
                            var resultado = lista.Except(listaFinal).ToList();
                            resultado.Add(new ReporteAcumuladoDto()
                            {
                                Anio = fecha.Year,
                                Mes = fecha.Month,
                                Equipos = primero.Equipos,
                                Total = primero.Total,
                                Fecha = Utilitarios.DevolverNombreFecha(new DateTime(fecha.Year, fecha.Month, 1, 0, 0, 0))
                            });

                            return resultado.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList();
                        }
                        else
                        {
                            return new List<ReporteAcumuladoDto>();
                        }
                    }
                    else
                        return new List<ReporteAcumuladoDto>();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ReporteEvolucionTecnologiaDto> GetEvolucionTecnologia(int tecnologia, out int totalRows)
        {
            var meses1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var meses2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            totalRows = 0;
            DateTime fechaActual = DateTime.Now;
            var listaRetorno = new List<ReporteEvolucionTecnologiaDto>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tecnologiaRegistro = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == tecnologia);
                        if (tecnologiaRegistro != null)
                        {
                            var evolucion = new ReporteEvolucionTecnologiaDto();
                            evolucion.ClaveTecnologia = tecnologiaRegistro.ClaveTecnologia;
                            evolucion.TecnologiaId = tecnologiaRegistro.TecnologiaId;

                            if (tecnologiaRegistro.TipoTecnologia.HasValue)
                            {
                                if (tecnologiaRegistro.TipoTecnologia.Value == (int)ETecnologiaTipo.NoEstandar)
                                {
                                    evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                    evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                    evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;

                                    if (tecnologiaRegistro.EliminacionTecObsoleta.HasValue)
                                    {
                                        var roadmapConfigurado = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == tecnologiaRegistro.EliminacionTecObsoleta.Value);
                                        if (roadmapConfigurado != null)
                                        {
                                            evolucion.RoadmapConfigurado = roadmapConfigurado.ClaveTecnologia;
                                            evolucion.RoadmapSugerido = roadmapConfigurado.ClaveTecnologia;
                                        }
                                    }

                                    listaRetorno.Add(evolucion);
                                    totalRows = 1;
                                    return listaRetorno;
                                }
                                else
                                {
                                    if (tecnologiaRegistro.FlagFechaFinSoporte.HasValue)
                                    {
                                        if (tecnologiaRegistro.FlagFechaFinSoporte.Value)
                                        {
                                            switch (tecnologiaRegistro.FechaCalculoTec)
                                            {
                                                case (int)(FechaCalculoTecnologia.FechaFinSoporte):
                                                    if (tecnologiaRegistro.FechaFinSoporte.HasValue)
                                                    {
                                                        if (fechaActual > tecnologiaRegistro.FechaFinSoporte.Value)
                                                        {
                                                            evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                        }
                                                        else
                                                        {
                                                            evolucion.EstadoActual = GetSemaforoTecnologia(tecnologiaRegistro.FechaFinSoporte.Value, fechaActual, meses1);
                                                            evolucion.EstadoIndicador1 = GetSemaforoTecnologia(tecnologiaRegistro.FechaFinSoporte.Value, fechaActual.AddMonths(meses1), meses1);
                                                            evolucion.EstadoIndicador2 = GetSemaforoTecnologia(tecnologiaRegistro.FechaFinSoporte.Value, fechaActual.AddMonths(meses2), meses1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    break;
                                                case (int)(FechaCalculoTecnologia.FechaExtendida):
                                                    if (tecnologiaRegistro.FechaExtendida.HasValue)
                                                    {
                                                        if (fechaActual > tecnologiaRegistro.FechaExtendida.Value)
                                                        {
                                                            evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                        }
                                                        else
                                                        {
                                                            evolucion.EstadoActual = GetSemaforoTecnologia(tecnologiaRegistro.FechaExtendida.Value, fechaActual, meses1);
                                                            evolucion.EstadoIndicador1 = GetSemaforoTecnologia(tecnologiaRegistro.FechaExtendida.Value, fechaActual.AddMonths(meses1), meses1);
                                                            evolucion.EstadoIndicador2 = GetSemaforoTecnologia(tecnologiaRegistro.FechaExtendida.Value, fechaActual.AddMonths(meses2), meses1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    break;
                                                case (int)(FechaCalculoTecnologia.FechaInterna):
                                                    if (tecnologiaRegistro.FechaAcordada.HasValue)
                                                    {
                                                        if (fechaActual > tecnologiaRegistro.FechaAcordada.Value)
                                                        {
                                                            evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                            evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                        }
                                                        else
                                                        {
                                                            evolucion.EstadoActual = GetSemaforoTecnologia(tecnologiaRegistro.FechaAcordada.Value, fechaActual, meses1);
                                                            evolucion.EstadoIndicador1 = GetSemaforoTecnologia(tecnologiaRegistro.FechaAcordada.Value, fechaActual.AddMonths(meses1), meses1);
                                                            evolucion.EstadoIndicador2 = GetSemaforoTecnologia(tecnologiaRegistro.FechaAcordada.Value, fechaActual.AddMonths(meses2), meses1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    break;
                                                default:
                                                    evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                    evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                    evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            evolucion.EstadoActual = (int)ColoresSemaforo.Verde;
                                            evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Verde;
                                            evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Verde;
                                        }
                                    }
                                    else
                                    {
                                        evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                        evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                        evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                    }

                                    if (tecnologiaRegistro.EliminacionTecObsoleta.HasValue)
                                    {
                                        var roadmapConfigurado = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == tecnologiaRegistro.EliminacionTecObsoleta.Value);
                                        if (roadmapConfigurado != null)
                                        {
                                            evolucion.RoadmapConfigurado = roadmapConfigurado.ClaveTecnologia;
                                            evolucion.RoadmapSugerido = roadmapConfigurado.ClaveTecnologia;
                                        }
                                    }

                                    listaRetorno.Add(evolucion);
                                    totalRows = 1;
                                    return listaRetorno;
                                }
                            }
                            else
                            {
                                evolucion.EstadoActual = (int)ColoresSemaforo.Rojo;
                                evolucion.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                evolucion.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;

                                if (tecnologiaRegistro.EliminacionTecObsoleta.HasValue)
                                {
                                    var roadmapConfigurado = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == tecnologiaRegistro.EliminacionTecObsoleta.Value);
                                    if (roadmapConfigurado != null)
                                    {
                                        evolucion.RoadmapConfigurado = roadmapConfigurado.ClaveTecnologia;
                                        evolucion.RoadmapSugerido = roadmapConfigurado.ClaveTecnologia;
                                    }
                                }

                                listaRetorno.Add(evolucion);
                                totalRows = 1;
                                return listaRetorno;
                            }
                        }
                        else
                            return new List<ReporteEvolucionTecnologiaDto>();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinRelaciones()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetAlertaFuncional_EquiposSinRelaciones()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetExportar(string aplicacion, string equipo, int tipo, string estado, string tecnologia, string subdominioIds, string ambiente, DateTime FechaConsulta)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerRelacionesAplicacionExportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", aplicacion));
                        comando.Parameters.Add(new SqlParameter("@equipo", equipo));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@ambiente", ambiente));

                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", subdominioIds));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", FechaConsulta.Day));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", FechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", FechaConsulta.Year));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                Relevancia = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                FechaInicioCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaRegistroCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRegistroCuarentena")),
                                FechaFinCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaFinCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinCuarentena")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2)
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetExportarConsultor(string aplicacion, string equipo, int tipo, string estado, string matricula, string tecnologia, string subdominioIds, string ambiente, DateTime FechaConsulta)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerRelacionesAplicacionExportarConsultor]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", aplicacion));
                        comando.Parameters.Add(new SqlParameter("@equipo", equipo));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));
                        comando.Parameters.Add(new SqlParameter("@estado", estado));
                        comando.Parameters.Add(new SqlParameter("@ambiente", ambiente));

                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", subdominioIds));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", FechaConsulta.Day));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", FechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", FechaConsulta.Year));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                Relevancia = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                FechaInicioCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaRegistroCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRegistroCuarentena")),
                                FechaFinCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaFinCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinCuarentena")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2)
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivision(PaginaReporteGerencia pag, out int totalRows)
        {
            var externo = Settings.Get<string>("Aplicacion.Infraestructura");

            pag.Gestionado = (pag.Gestionado == "-1" || pag.Gestionado == null) ? "" : pag.Gestionado;
            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;
            pag.TipoActivo = (pag.TipoActivo == "-1" || pag.TipoActivo == null) ? "" : pag.TipoActivo;
            pag.Gerencia = (pag.Gerencia == "-1" || pag.Gerencia == null) ? "" : pag.Gerencia;
            pag.ClasificacionTecnica = (pag.ClasificacionTecnica == "-1" || pag.ClasificacionTecnica == null) ? "" : pag.ClasificacionTecnica;
            pag.Area = (pag.Area == null || pag.Area == "-1") ? "" : pag.Area;
            pag.Unidad = (pag.Unidad == null || pag.Unidad == "-1") ? "" : pag.Unidad;
            pag.Gestor = (pag.Gestor == null || pag.Gestor == "-1") ? "" : pag.Gestor;
            pag.TTL = (pag.TTL == null || pag.TTL == "-1") ? "" : pag.TTL;
            pag.Broker = (pag.Broker == null || pag.Broker == "-1") ? "" : pag.Broker;

            pag.JefeEquipo = (pag.JefeEquipo == null || pag.JefeEquipo == "-1") ? "" : pag.JefeEquipo;
            pag.Owner = (pag.Owner == null || pag.Owner == "-1") ? "" : pag.Owner;
            pag.Division = (pag.Division == null || pag.Division == "-1") ? "" : pag.Division;
            pag.Experto = (pag.Experto == null || pag.Experto == "-1") ? "" : pag.Experto;
            pag.SubclasificacionTecnica = (pag.SubclasificacionTecnica == "-1" || pag.SubclasificacionTecnica == null) ? "" : pag.SubclasificacionTecnica;
            pag.UnidadFondeo = (pag.UnidadFondeo == "-1" || pag.UnidadFondeo == null) ? "" : pag.UnidadFondeo;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }
            try
            {
                pag.FechaProyeccionFiltro = DateTime.ParseExact(pag.FechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaProyeccionFiltro = DateTime.Now;
            }

            totalRows = 0;
            var totalRowsDetalle = 0;

            try
            {
                int totalNuevo = 0;
                var TiposEquipos = ServiceManager<TipoEquipoDAO>.Provider.GetTipoEquipo(1, int.MaxValue, "", "", out totalNuevo);
                var TiposEquiposExcluidos = new List<TipoEquipoDTO>();
                var cantidatTiposEquiposExcluidos = TiposEquipos.Where(z => z.FlagExcluirKPI == true).ToList().Count();
                if (cantidatTiposEquiposExcluidos > 0)
                {
                    TiposEquiposExcluidos = TiposEquipos.Where(z => z.FlagExcluirKPI == true).ToList();
                }

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_PorGerenciasDivision]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@experto", pag.Experto));
                        comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                        comando.Parameters.Add(new SqlParameter("@gestionado", pag.Gestionado));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@gestor", pag.Gestor));
                        comando.Parameters.Add(new SqlParameter("@tipoactivo", pag.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@ttl", pag.TTL));
                        comando.Parameters.Add(new SqlParameter("@broker", pag.Broker));
                        comando.Parameters.Add(new SqlParameter("@externo", externo));
                        comando.Parameters.Add(new SqlParameter("@unidadFondeo", pag.UnidadFondeo));
                        comando.Parameters.Add(new SqlParameter("@FlagProyeccion", pag.FlagProyeccion));
                        comando.Parameters.Add(new SqlParameter("@FechaProyeccion", pag.FechaProyeccionFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                DetalleCriticidad = reader.IsDBNull(reader.GetOrdinal("DetalleCriticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleCriticidad")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                RoadMap = reader.IsDBNull(reader.GetOrdinal("RoadMap")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMap")),
                                IndiceObsolescencia = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")), 2),
                                RiesgoAplicacion = reader.IsDBNull(reader.GetOrdinal("RiesgoAplicacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RiesgoAplicacion")),
                                Priorizacion = reader.IsDBNull(reader.GetOrdinal("Priorizacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Priorizacion")),
                                TotalTecnologiasObsoletas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologiasObsoletas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologiasObsoletas")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                JefeEquipo_ExpertoAplicacion = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                ExpertoEspecialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                FlagExterno = reader.IsDBNull(reader.GetOrdinal("FlagExterno")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagExterno")),
                                ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoPCI")),
                                IndiceObsolescencia_FLooking = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")), 4),
                                IndiceObsolescencia_FL_Proyeccion = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")), 4),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo"))
                            };

                            if (objeto.EstadoAplicacion == "Eliminada" && objeto.IndiceObsolescencia == 0 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                            {
                                objeto.DetalleActual = new AplicacionIndicador();
                            }
                            else
                            {
                                if (objeto.FlagExterno == 1 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                {
                                    objeto.DetalleActual = new AplicacionIndicador();
                                    objeto.IndiceObsolescencia = 0;
                                    objeto.RiesgoAplicacion = 0;
                                    objeto.Priorizacion = 0;
                                }
                                else
                                {
                                    if (objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                    {
                                        objeto.DetalleActual = new AplicacionIndicador();
                                        objeto.EquipoSinTecnologiasYServidores = true;
                                    }
                                    else
                                    {
                                        if (objeto.IndiceObsolescencia == 0)
                                        {
                                            pag.Aplicacion = objeto.CodigoApt;
                                            pag.pageSize = int.MaxValue;
                                            pag.pageNumber = 1;
                                            var detalle = this.GetGerenciaDivisionDetalle(pag, out totalRowsDetalle);

                                            //var detalle = new List<ReporteGerenciaDivisionDto>();

                                            ////Cambio para que semaforo no tome en cuenta los tipos de equipos excluidos del calculo KPI.
                                            //if (cantidatTiposEquiposExcluidos > 0)
                                            //{
                                            //    var detalleprev = this.GetGerenciaDivisionDetalle(pag, out totalRowsDetalle);
                                            //    detalle = (from a in detalleprev
                                            //               where !(from o in TiposEquiposExcluidos
                                            //                       select o.Id).Contains(a.TipoEquipoId)
                                            //               select a).ToList();
                                            //    totalRowsDetalle = detalle.Count();
                                            //}
                                            //else
                                            //{
                                            //    if (cantidatTiposEquiposExcluidos == 0)
                                            //    {
                                            //        detalle = this.GetGerenciaDivisionDetalle(pag, out totalRowsDetalle);
                                            //    }
                                            //}

                                            if (totalRowsDetalle > 0)
                                            {
                                                var itemActual = new AplicacionIndicador();
                                                itemActual.CodigoApt = objeto.CodigoApt;
                                                itemActual.TotalTecnologias = totalRowsDetalle;
                                                itemActual.TotalVerdesActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 1);

                                                itemActual.TotalAmarillosActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 0);
                                                objeto.DetalleActual = itemActual;
                                            }
                                        }
                                    }
                                }
                            }

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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionExportar(PaginaReporteGerencia pag, out int totalRows)
        {
            var externo = Settings.Get<string>("Aplicacion.Infraestructura");

            pag.Gestionado = (pag.Gestionado == "-1" || pag.Gestionado == null) ? "" : pag.Gestionado;
            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;
            pag.TipoActivo = (pag.TipoActivo == "-1" || pag.TipoActivo == null) ? "" : pag.TipoActivo;
            pag.Gerencia = (pag.Gerencia == "-1" || pag.Gerencia == null) ? "" : pag.Gerencia;
            pag.ClasificacionTecnica = (pag.ClasificacionTecnica == "-1" || pag.ClasificacionTecnica == null) ? "" : pag.ClasificacionTecnica;
            pag.Area = (pag.Area == null || pag.Area == "-1") ? "" : pag.Area;
            pag.Unidad = (pag.Unidad == null || pag.Unidad == "-1") ? "" : pag.Unidad;
            pag.Gestor = (pag.Gestor == null || pag.Gestor == "-1") ? "" : pag.Gestor;
            pag.TTL = (pag.TTL == null || pag.TTL == "-1") ? "" : pag.TTL;
            pag.Broker = (pag.Broker == null || pag.Broker == "-1") ? "" : pag.Broker;

            pag.JefeEquipo = (pag.JefeEquipo == null || pag.JefeEquipo == "-1") ? "" : pag.JefeEquipo;
            pag.Owner = (pag.Owner == null || pag.Owner == "-1") ? "" : pag.Owner;
            pag.Division = (pag.Division == null || pag.Division == "-1") ? "" : pag.Division;
            pag.Experto = (pag.Experto == null || pag.Experto == "-1") ? "" : pag.Experto;
            pag.SubclasificacionTecnica = (pag.SubclasificacionTecnica == "-1" || pag.SubclasificacionTecnica == null) ? "" : pag.SubclasificacionTecnica;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }
            try
            {
                pag.FechaProyeccionFiltro = DateTime.ParseExact(pag.FechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaProyeccionFiltro = DateTime.Now;
            }
            totalRows = 0;
            var totalRowsDetalle = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_PorGerenciasDivision]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@experto", pag.Experto));
                        comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                        comando.Parameters.Add(new SqlParameter("@gestionado", pag.Gestionado));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@gestor", pag.Gestor));
                        comando.Parameters.Add(new SqlParameter("@tipoactivo", pag.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@ttl", pag.TTL));
                        comando.Parameters.Add(new SqlParameter("@broker", pag.Broker));
                        comando.Parameters.Add(new SqlParameter("@externo", externo));
                        comando.Parameters.Add(new SqlParameter("@unidadFondeo", pag.UnidadFondeo));
                        comando.Parameters.Add(new SqlParameter("@FlagProyeccion", pag.FlagProyeccion));
                        comando.Parameters.Add(new SqlParameter("@FechaProyeccion", pag.FechaProyeccionFiltro));



                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                DetalleCriticidad = reader.IsDBNull(reader.GetOrdinal("DetalleCriticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleCriticidad")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                RoadMap = reader.IsDBNull(reader.GetOrdinal("RoadMap")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMap")),
                                IndiceObsolescencia = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")), 2),
                                RiesgoAplicacion = reader.IsDBNull(reader.GetOrdinal("RiesgoAplicacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RiesgoAplicacion")),
                                Priorizacion = reader.IsDBNull(reader.GetOrdinal("Priorizacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Priorizacion")),
                                TotalTecnologiasObsoletas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologiasObsoletas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologiasObsoletas")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                JefeEquipo_ExpertoAplicacion = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                ExpertoEspecialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                FlagExterno = reader.IsDBNull(reader.GetOrdinal("FlagExterno")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagExterno")),
                                ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoPCI")),
                                IndiceObsolescencia_FLooking = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")), 4),
                                IndiceObsolescencia_FL_Proyeccion = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")), 4),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo"))
                            };

                            if (objeto.EstadoAplicacion == "Eliminada" && objeto.IndiceObsolescencia == 0 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                            {
                                objeto.DetalleActual = new AplicacionIndicador();
                            }
                            else
                            {
                                if (objeto.FlagExterno == 1 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                {
                                    objeto.DetalleActual = new AplicacionIndicador();
                                    objeto.IndiceObsolescencia = 0;
                                    objeto.RiesgoAplicacion = 0;
                                    objeto.Priorizacion = 0;
                                }
                                else
                                {
                                    if (objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                    {
                                        objeto.DetalleActual = new AplicacionIndicador();
                                        objeto.EquipoSinTecnologiasYServidores = true;
                                    }
                                    else
                                    {
                                        if (objeto.IndiceObsolescencia == 0)
                                        {
                                            pag.Aplicacion = objeto.CodigoApt;
                                            pag.pageSize = int.MaxValue;
                                            pag.pageNumber = 1;
                                            //var detalle = this.GetGerenciaDivisionDetalle(pag, out totalRowsDetalle);
                                            //if (totalRowsDetalle > 0)
                                            //{
                                            //    var itemActual = new AplicacionIndicador();
                                            //    itemActual.CodigoApt = objeto.CodigoApt;
                                            //    itemActual.TotalTecnologias = totalRowsDetalle;
                                            //    itemActual.TotalVerdesActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 1);
                                            //    itemActual.TotalVerdesProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 1);
                                            //    itemActual.TotalVerdesProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 1);

                                            //    itemActual.TotalAmarillosActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 0);
                                            //    itemActual.TotalAmarillosProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 0);
                                            //    itemActual.TotalAmarillosProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 0);
                                            //    objeto.DetalleActual = itemActual;
                                            //}
                                        }
                                    }
                                }
                            }

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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetAppsPorSegundNivel(PaginaReporteGerencia pag, out int totalRows)
        {


            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Dashboard_AppsPorSegundoNivel]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@SegundoNivel", pag.SegundoNivel));

                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                DetalleCriticidad = reader.IsDBNull(reader.GetOrdinal("DetalleCriticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleCriticidad")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                RoadMap = reader.IsDBNull(reader.GetOrdinal("RoadMap")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMap")),
                                IndiceObsolescencia = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")), 2),
                                RiesgoAplicacion = reader.IsDBNull(reader.GetOrdinal("RiesgoAplicacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RiesgoAplicacion")),
                                Priorizacion = reader.IsDBNull(reader.GetOrdinal("Priorizacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Priorizacion")),
                                TotalTecnologiasObsoletas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologiasObsoletas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologiasObsoletas")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                JefeEquipo_ExpertoAplicacion = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                ExpertoEspecialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                FlagExterno = reader.IsDBNull(reader.GetOrdinal("FlagExterno")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagExterno")),
                                ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoPCI")),
                                IndiceObsolescencia_FLooking = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")), 4),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo"))
                            };

                            if (objeto.EstadoAplicacion == "Eliminada" && objeto.IndiceObsolescencia == 0 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                            {
                                objeto.DetalleActual = new AplicacionIndicador();
                            }
                            else
                            {
                                if (objeto.FlagExterno == 1 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                {
                                    objeto.DetalleActual = new AplicacionIndicador();
                                    objeto.IndiceObsolescencia = 0;
                                    objeto.RiesgoAplicacion = 0;
                                    objeto.Priorizacion = 0;
                                }
                                else
                                {
                                    if (objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                    {
                                        objeto.DetalleActual = new AplicacionIndicador();
                                        objeto.EquipoSinTecnologiasYServidores = true;
                                    }
                                    else
                                    {
                                        if (objeto.IndiceObsolescencia == 0)
                                        {
                                            pag.Aplicacion = objeto.CodigoApt;
                                            pag.pageSize = int.MaxValue;
                                            pag.pageNumber = 1;

                                        }
                                    }
                                }
                            }

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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionConsultor(PaginaReporteGerencia pag, out int totalRows)
        {
            var externo = Settings.Get<string>("Aplicacion.Infraestructura");


            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }
            try
            {
                pag.FechaProyeccionFiltro = DateTime.ParseExact(pag.FechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaProyeccionFiltro = DateTime.Now;
            }

            totalRows = 0;
            var totalRowsDetalle = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_PorGerenciasDivisionConsultor_vFL]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.Matricula));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@externo", externo));
                        comando.Parameters.Add(new SqlParameter("@FlagProyeccion", pag.FlagProyeccion));
                        comando.Parameters.Add(new SqlParameter("@FechaProyeccion", pag.FechaProyeccionFiltro));
                        comando.Parameters.Add(new SqlParameter("@perfilId", pag.PerfilId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                DetalleCriticidad = reader.IsDBNull(reader.GetOrdinal("DetalleCriticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleCriticidad")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo")),
                                RoadMap = reader.IsDBNull(reader.GetOrdinal("RoadMap")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMap")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                RiesgoAplicacion = reader.IsDBNull(reader.GetOrdinal("RiesgoAplicacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RiesgoAplicacion")),
                                Priorizacion = reader.IsDBNull(reader.GetOrdinal("Priorizacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Priorizacion")),
                                TotalTecnologiasObsoletas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologiasObsoletas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologiasObsoletas")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                JefeEquipo_ExpertoAplicacion = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                ExpertoEspecialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                FlagExterno = reader.IsDBNull(reader.GetOrdinal("FlagExterno")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagExterno")),
                                ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoPCI")),
                                IndiceObsolescencia_FLooking = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_ForwardLooking")), 4),
                                IndiceObsolescencia_FL_Proyeccion = Math.Round(reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia_FL_Proyeccion")), 4),
                            };

                            if (objeto.EstadoAplicacion == "Eliminada" && objeto.IndiceObsolescencia == 0 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                            {
                                objeto.DetalleActual = new AplicacionIndicador();
                            }
                            else
                            {
                                if (objeto.FlagExterno == 1 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                {
                                    objeto.DetalleActual = new AplicacionIndicador();
                                    objeto.IndiceObsolescencia = 0;
                                    objeto.RiesgoAplicacion = 0;
                                    objeto.Priorizacion = 0;
                                }
                                else
                                {
                                    if (objeto.TotalTecnologias == 0 && objeto.TotalTecnologias == 0)
                                    {
                                        objeto.DetalleActual = new AplicacionIndicador();
                                        objeto.EquipoSinTecnologiasYServidores = true;
                                    }
                                    else
                                    {
                                        if (objeto.IndiceObsolescencia == 0)
                                        {
                                            pag.Aplicacion = objeto.CodigoApt;
                                            pag.pageSize = int.MaxValue;
                                            pag.pageNumber = 1;
                                            var detalle = this.GetGerenciaDivisionDetalle(pag, out totalRowsDetalle);
                                            if (totalRowsDetalle > 0)
                                            {
                                                var itemActual = new AplicacionIndicador();
                                                itemActual.CodigoApt = objeto.CodigoApt;
                                                itemActual.TotalTecnologias = totalRowsDetalle;
                                                itemActual.TotalVerdesActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 1);

                                                itemActual.TotalAmarillosActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 0);
                                                objeto.DetalleActual = itemActual;
                                            }
                                        }
                                    }
                                }

                            }
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalle(PaginaReporteGerencia pag, out int totalRows)
        {
            pag.Gestionado = (pag.Gestionado == "-1" || pag.Gestionado == null) ? "" : pag.Gestionado;
            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;
            pag.TipoActivo = (pag.TipoActivo == "-1" || pag.TipoActivo == null) ? "" : pag.TipoActivo;
            pag.JefeEquipo = (pag.JefeEquipo == null || pag.JefeEquipo == "-1") ? "" : pag.JefeEquipo;
            pag.Owner = (pag.Owner == null || pag.Owner == "-1") ? "" : pag.Owner;
            pag.Experto = (pag.Experto == null || pag.Experto == "-1") ? "" : pag.Experto;
            //pag.Gerencia = (pag.Gerencia == null) ? "" : pag.Gerencia;
            pag.Gerencia = (pag.Gerencia == "-1" || pag.Gerencia == null) ? "" : pag.Gerencia;
            pag.ClasificacionTecnica = (pag.ClasificacionTecnica == "-1" || pag.ClasificacionTecnica == null) ? "" : pag.ClasificacionTecnica;
            pag.Division = (pag.Division == null || pag.Division == "-1") ? "" : pag.Division;

            pag.Area = (pag.Area == null || pag.Area == "-1") ? "" : pag.Area;
            pag.Unidad = (pag.Unidad == null || pag.Unidad == "-1") ? "" : pag.Unidad;
            pag.Gestor = (pag.Gestor == null || pag.Gestor == "-1") ? "" : pag.Gestor;
            pag.SubclasificacionTecnica = (pag.SubclasificacionTecnica == null || pag.SubclasificacionTecnica == "-1") ? "" : pag.SubclasificacionTecnica;
            pag.UnidadFondeo = (pag.UnidadFondeo == null || pag.UnidadFondeo == "-1") ? "" : pag.UnidadFondeo;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }
            try
            {
                pag.FechaProyeccionFiltro = DateTime.ParseExact(pag.FechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture); 
            }
            catch (Exception)
            {
                pag.FechaProyeccionFiltro = DateTime.Now;
            }

            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var mesesTipoCicloVida = this.GetMesesObsolescencia();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                String valorTipoEquipoCD = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO_VALIDACION").Valor;
                Int32 varIdTipoEquipoCD = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_DetalladoPorGerenciasDivision_vFL]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@experto", pag.Experto));
                        comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                        comando.Parameters.Add(new SqlParameter("@gestionado", pag.Gestionado));
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@equipo", string.Empty));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@gestor", pag.Gestor));
                        comando.Parameters.Add(new SqlParameter("@tipoactivo", pag.TipoActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@ttl", pag.TTL));
                        comando.Parameters.Add(new SqlParameter("@broker", pag.Broker));
                        comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                        comando.Parameters.Add(new SqlParameter("@unidadFondeo", pag.UnidadFondeo));
                        comando.Parameters.Add(new SqlParameter("@flagProyeccion", pag.FlagProyeccion));
                        comando.Parameters.Add(new SqlParameter("@fechaProyeccion", pag.FechaProyeccionFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                FactoresKPI_FLooking = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW")),
                                FactoresKPI_FL_Proyeccion = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW_Proyeccion")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW_Proyeccion")),
                                //TipoEquipoValidarCD = (reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId"))) == varIdTipoEquipoCD ? valorTipoEquipoCD : "",
                                TipoEquipoValidarCD = valorTipoEquipoCD,
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoCicloVidaNombre = reader.IsDBNull(reader.GetOrdinal("TipoCicloVidaNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoCicloVidaNombre")),
                                EstadoIdTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologiaId")),
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }

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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleConsultor(PaginaReporteGerencia pag, out int totalRows)
        {
            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;
            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }
            try
            {
                pag.FechaProyeccionFiltro = DateTime.ParseExact(pag.FechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaProyeccionFiltro = DateTime.Now;
            }

            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var mesesTipoCicloVida = this.GetMesesObsolescencia();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_DetalladoPorGerenciasDivisionConsultor_vFL]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.Matricula));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@flagProyeccion", pag.FlagProyeccion));
                        comando.Parameters.Add(new SqlParameter("@fechaProyeccion", pag.FechaProyeccionFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                FactoresKPI_FLooking = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW")),
                                FactoresKPI_FL_Proyeccion = reader.IsDBNull(reader.GetOrdinal("FactorKPIFW_Proyeccion")) ? string.Empty : reader.GetString(reader.GetOrdinal("FactorKPIFW_Proyeccion")),
                                TipoCicloVidaNombre = reader.IsDBNull(reader.GetOrdinal("TipoCicloVidaNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoCicloVidaNombre")),
                                EstadoIdTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologiaId")),
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override ReporteGraficoSubdominiosDto GetGrafico(string tipos, string subsidiaria, string subdominios, DateTime fechaConsulta, string tipoTecnologia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var meses = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                ReporteGraficoSubdominiosDto objeto = null;
                string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominio_V3]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_PorcentajePorSubdominio]";
                //}
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipos", tipos));
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@dia", fechaConsulta.Day));
                        comando.Parameters.Add(new SqlParameter("@mes", fechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@anio", fechaConsulta.Year));
                        comando.Parameters.Add(new SqlParameter("@mesproyeccion", meses));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", subsidiaria));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", tipoTecnologia));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto = new ReporteGraficoSubdominiosDto()
                            {
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                TotalRojo = reader.IsDBNull(reader.GetOrdinal("TotalRojo")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalRojo")),
                                TotalAmbar = reader.IsDBNull(reader.GetOrdinal("TotalAmbar")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAmbar")),
                                TotalVerde = reader.IsDBNull(reader.GetOrdinal("TotalVerde")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalVerde"))
                            };
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasDto> GetReporteDetalleTecnologia(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var parametroMeses24 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteDetalleTecnologiasDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@mesProyeccion", parametroMeses24));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                TotalObsoletos = reader.IsDBNull(reader.GetOrdinal("TotalObsoletos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalObsoletos")),
                                TotalPorVencer = reader.IsDBNull(reader.GetOrdinal("TotalPorVencer")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPorVencer")),
                                TotalVigente = reader.IsDBNull(reader.GetOrdinal("TotalVigente")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalVigente")),
                                TotalIndefinida = reader.IsDBNull(reader.GetOrdinal("TotalIndefinida")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIndefinida")),
                                TotalObsoletosRoadmap = reader.IsDBNull(reader.GetOrdinal("TotalObsoletosRoadmap")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalObsoletosRoadmap")),
                                TotalPorVencerRoadmap = reader.IsDBNull(reader.GetOrdinal("TotalPorVencerRoadmap")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPorVencerRoadmap")),
                                TotalVigenteRoadmap = reader.IsDBNull(reader.GetOrdinal("TotalVigenteRoadmap")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalVigenteRoadmap")),
                                TotalIndefinidaRoadmap = reader.IsDBNull(reader.GetOrdinal("TotalIndefinidaRoadmap")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIndefinidaRoadmap")),
                                // TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId"))
                                MesProyeccion = parametroMeses24
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologiaInstalaciones(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasEquipoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_Equipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasEquipoDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                TotalInstalaciones = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                TotalPCs = reader.IsDBNull(reader.GetOrdinal("TotalPCs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPCs")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalServidoresAgencia = reader.IsDBNull(reader.GetOrdinal("TotalServidoresAgencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidoresAgencia")),
                                TotalServicioNube = reader.IsDBNull(reader.GetOrdinal("TotalServicioNube")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServicioNube")),
                                TotalStorage = reader.IsDBNull(reader.GetOrdinal("TotalStorage")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalStorage")),
                                TotalAppliance = reader.IsDBNull(reader.GetOrdinal("TotalAppliance")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAppliance")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                FechaFiltro = pag.FechaFiltro,
                                SubdominioToString = pag.SubdominioToString
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologiaInstalaciones()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologiaInstalacionesByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasEquipoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_Equipo_By_Tipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", pag.TipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@tecnologiaId", pag.TecnologiaId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasEquipoDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                                //TotalInstalaciones = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                //TotalPCs = reader.IsDBNull(reader.GetOrdinal("TotalPCs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPCs")),
                                //TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                //TotalServidoresAgencia = reader.IsDBNull(reader.GetOrdinal("TotalServidoresAgencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidoresAgencia")),
                                //TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                //FechaFiltro = pag.FechaFiltro,
                                //SubdominioToString = pag.SubdominioToString
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologiaInstalacionesByTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasAplicacionDto> GetReporteDetalleTecnologia_Aplicacion(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasAplicacionDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaDetalle_Aplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasAplicacionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TotalAplicacion = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                SubdominioToString = pag.SubdominioToString,
                                Owner = pag.OwnerFiltro,
                                FechaFiltro = pag.FechaFiltro
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Aplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasAplicacionDto> GetReporteDetalleTecnologia_AplicacionByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasAplicacionDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaDetalle_Aplicacion_By_Tecnologia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.NombreTec));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));

                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasAplicacionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia"))
                                //TotalAplicacion = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                //SubdominioToString = pag.SubdominioToString,
                                //Owner = pag.OwnerFiltro,
                                //FechaFiltro = pag.FechaFiltro
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Aplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologia_Equipo(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasEquipoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaDetalle_Equipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasEquipoDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),


                                TotalInstalaciones = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                TotalPCs = reader.IsDBNull(reader.GetOrdinal("TotalPCs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPCs")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalServidoresAgencia = reader.IsDBNull(reader.GetOrdinal("TotalServidoresAgencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidoresAgencia")),
                                SubdominioToString = pag.SubdominioToString,
                                Owner = pag.OwnerFiltro,
                                FechaFiltro = pag.FechaFiltro
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasEquipoDto> GetReporteDetalleTecnologia_EquipoByTecnologia(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasEquipoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaDetalle_Equipo_By_Tipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", pag.TipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@fabricante", pag.FabricanteTec));
                        comando.Parameters.Add(new SqlParameter("@nombre", pag.NombreTec));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasEquipoDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                //TotalInstalaciones = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                //TotalPCs = reader.IsDBNull(reader.GetOrdinal("TotalPCs")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPCs")),
                                //TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                //TotalServidoresAgencia = reader.IsDBNull(reader.GetOrdinal("TotalServidoresAgencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidoresAgencia")),
                                SubdominioToString = pag.SubdominioToString,
                                Owner = pag.OwnerFiltro,
                                FechaFiltro = pag.FechaFiltro
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> GetReporteOwner(PaginacionOwner pag, out int totalRows)
        {
            totalRows = 0;
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor); //12 meses
            var indicadorOwner1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_OWNER_1").Valor); //6 meses
            var indicadorOwner2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_OWNER_2").Valor); //18 meses
            var indicadorOwner3 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_OWNER_3").Valor); //30 meses

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_OwnerTecnologia_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@proyeccion", indicador1));
                        comando.Parameters.Add(new SqlParameter("@proyOwner1", indicadorOwner1));
                        comando.Parameters.Add(new SqlParameter("@proyOwner2", indicadorOwner2));
                        comando.Parameters.Add(new SqlParameter("@proyOwner3", indicadorOwner3));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                IndicadorProyOwnerHoy = reader.IsDBNull(reader.GetOrdinal("IndicadorProyOwnerHoy")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorProyOwnerHoy")),
                                IndicadorProyOwner1 = reader.IsDBNull(reader.GetOrdinal("IndicadorProyOwner1")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorProyOwner1")),
                                IndicadorProyOwner2 = reader.IsDBNull(reader.GetOrdinal("IndicadorProyOwner2")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorProyOwner2")),
                                IndicadorProyOwner3 = reader.IsDBNull(reader.GetOrdinal("IndicadorProyOwner3")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndicadorProyOwner3")),
                                FechaMaxFamilia = reader.IsDBNull(reader.GetOrdinal("FechaMaxFamilia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaMaxFamilia")),
                                TotalIndefinidas = reader.IsDBNull(reader.GetOrdinal("TotalIndefinidas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalIndefinidas")),
                                TotalAplicaciones = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                TotalEquipos = reader.IsDBNull(reader.GetOrdinal("TotalEquipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquipos")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> GetReporteOwnerByTecnologia(PaginacionOwner pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_OwnerTecnologiaDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.TecnologiaId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                NombreComponente = reader.IsDBNull(reader.GetOrdinal("NombreComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreComponente"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> GetReporteOwnerConsolidado(PaginacionOwner pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_OwnerTecnologiaDetalle_Consolidado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologia));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                NombreComponente = reader.IsDBNull(reader.GetOrdinal("NombreComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreComponente"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones(FiltrosDashboardTecnologia pag)
        {
            try
            {
                //totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteTecnologiaIntalacionDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        //comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        //comando.Parameters.Add(new SqlParameter("@owner", pag.OwnerFiltro));
                        //comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        //comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        //comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        //comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteTecnologiaIntalacionDTO()
                            {
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoStr = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    //if (lista.Count > 0)
                    //    totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Equipo_Agrupacion(FiltrosDashboardTecnologia pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteTecnologiaIntalacionDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_Equipo_Agrupacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteTecnologiaIntalacionDTO()
                            {
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoStr = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Total = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                Equipos = reader.IsDBNull(reader.GetOrdinal("Equipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("Equipos")),
                                Tecnologias = reader.IsDBNull(reader.GetOrdinal("Tecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("Tecnologias")),
                                Familias = reader.IsDBNull(reader.GetOrdinal("Familias")) ? 0 : reader.GetInt32(reader.GetOrdinal("Familias"))
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
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_SO(FiltrosDashboardTecnologia pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteTecnologiaIntalacionDTO>();

                var subdominioStr = string.Join("|", pag.SubdominioFiltrar);
                var tipoEquipoStr = string.Join("|", pag.TipoEquipoFiltrar);

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_SO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@subdominioId", subdominioStr));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", tipoEquipoStr));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteTecnologiaIntalacionDTO()
                            {
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Versiones = reader.IsDBNull(reader.GetOrdinal("Versiones")) ? string.Empty : reader.GetString(reader.GetOrdinal("Versiones")),
                                Total = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones"))
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
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Tipo(FiltrosDashboardTecnologia pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteTecnologiaIntalacionDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_Tipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteTecnologiaIntalacionDTO()
                            {
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                TipoTecnologiaStr = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                Total = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
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
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteTecnologiaIntalacionDTO> GetReporteTecnologiaInstalaciones_Tipo_Equipo(FiltrosDashboardTecnologia pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteTecnologiaIntalacionDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiaInstalaciones_Tipo_Equipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteTecnologiaIntalacionDTO()
                            {
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoStr = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Total = reader.IsDBNull(reader.GetOrdinal("TotalInstalaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalInstalaciones")),
                                Estandar = reader.IsDBNull(reader.GetOrdinal("Estandar")) ? 0 : reader.GetInt32(reader.GetOrdinal("Estandar")),
                                EstandarRestringido = reader.IsDBNull(reader.GetOrdinal("EstandarRestringido")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstandarRestringido")),
                                PorRegularizar = reader.IsDBNull(reader.GetOrdinal("PorRegularizar")) ? 0 : reader.GetInt32(reader.GetOrdinal("PorRegularizar")),
                                NoEstandar = reader.IsDBNull(reader.GetOrdinal("NoEstandar")) ? 0 : reader.GetInt32(reader.GetOrdinal("NoEstandar")),
                                EnEvaluacion = reader.IsDBNull(reader.GetOrdinal("EnEvaluacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("EnEvaluacion")),
                                Excepcion = reader.IsDBNull(reader.GetOrdinal("Excepcion")) ? 0 : reader.GetInt32(reader.GetOrdinal("Excepcion"))
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
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteServidoresHuerfanosDto> GetServidoresHuerfanos(PaginaReporteHuerfanos pag, DateTime fechaConsulta, int subdominioSO, out int totalRows)
        {
            totalRows = 0;

            pag.Equipo = (string.IsNullOrEmpty(pag.Equipo) ? "" : pag.Equipo);
            pag.SistemaOperativo = (string.IsNullOrEmpty(pag.SistemaOperativo) ? "" : pag.SistemaOperativo);
            pag.TipoEquipoToString = (string.IsNullOrEmpty(pag.TipoEquipoToString) ? "" : pag.TipoEquipoToString);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteServidoresHuerfanosDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_EquiposSinRelaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fecha", fechaConsulta));
                        comando.Parameters.Add(new SqlParameter("@equipo", pag.Equipo));
                        comando.Parameters.Add(new SqlParameter("@sistema", pag.SistemaOperativo));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipo", pag.TipoEquipoToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteServidoresHuerfanosDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                UltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetServidoresHuerfanos()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologias(PaginaReporteTecnologiasCustom pag, out int totalRows, bool detalle)
        {
            totalRows = 0;
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            pag.DominioIds = pag.DominioIds == null ? string.Empty : pag.DominioIds;
            pag.SubdominioIds = pag.SubdominioIds == null ? string.Empty : pag.SubdominioIds;
            pag.EstadoAprobacionIds = pag.EstadoAprobacionIds == null ? string.Empty : pag.EstadoAprobacionIds;
            pag.Tipos = pag.Tipos == null ? string.Empty : pag.Tipos;
            pag.EstadoObsolescencias = pag.EstadoObsolescencias == null ? string.Empty : pag.EstadoObsolescencias;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_DetalladoTecnologias]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@dominio", pag.DominioIds));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@estadoaprobacion", pag.EstadoAprobacionIds));
                        comando.Parameters.Add(new SqlParameter("@familia", pag.Familia));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.Tipos));
                        comando.Parameters.Add(new SqlParameter("@estadoobsolescencia", pag.EstadoObsolescencias));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Clave));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                EliminacionTecObsoleta = reader.IsDBNull(reader.GetOrdinal("EliminacionTecObsoleta")) ? 0 : reader.GetInt32(reader.GetOrdinal("EliminacionTecObsoleta")),
                                Roadmap = reader.IsDBNull(reader.GetOrdinal("Roadmap")) ? string.Empty : reader.GetString(reader.GetOrdinal("Roadmap")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    if (detalle)
                    {
                        DateTime fechaActual = DateTime.Now;
                        foreach (var item in lista)
                        {
                            if (item.TipoTecnologiaId == (int)ETecnologiaTipo.NoEstandar)
                            {
                                item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                            }
                            else
                            {
                                if (item.FlagFechaFinSoporte.HasValue)
                                {
                                    if (item.FlagFechaFinSoporte.Value)
                                    {
                                        switch (item.FechaCalculoTec)
                                        {
                                            case (int)(FechaCalculoTecnologia.FechaFinSoporte):
                                                if (item.FechaFinSoporte.HasValue)
                                                {
                                                    if (fechaActual > item.FechaFinSoporte.Value)
                                                    {
                                                        item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    else
                                                    {
                                                        item.EstadoActual = GetSemaforoTecnologia(item.FechaFinSoporte.Value, fechaActual, item.MesesIndicador1);
                                                        item.EstadoIndicador1 = GetSemaforoTecnologia(item.FechaFinSoporte.Value, fechaActual.AddMonths(item.MesesIndicador1), item.MesesIndicador1);
                                                        item.EstadoIndicador2 = GetSemaforoTecnologia(item.FechaFinSoporte.Value, fechaActual.AddMonths(item.MesesIndicador2), item.MesesIndicador1);
                                                    }
                                                }
                                                else
                                                {
                                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                }
                                                break;
                                            case (int)(FechaCalculoTecnologia.FechaExtendida):
                                                if (item.FechaExtendida.HasValue)
                                                {
                                                    if (fechaActual > item.FechaExtendida.Value)
                                                    {
                                                        item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    else
                                                    {
                                                        item.EstadoActual = GetSemaforoTecnologia(item.FechaExtendida.Value, fechaActual, item.MesesIndicador1);
                                                        item.EstadoIndicador1 = GetSemaforoTecnologia(item.FechaExtendida.Value, fechaActual.AddMonths(item.MesesIndicador1), item.MesesIndicador1);
                                                        item.EstadoIndicador2 = GetSemaforoTecnologia(item.FechaExtendida.Value, fechaActual.AddMonths(item.MesesIndicador2), item.MesesIndicador1);
                                                    }
                                                }
                                                else
                                                {
                                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                }
                                                break;
                                            case (int)(FechaCalculoTecnologia.FechaInterna):
                                                if (item.FechaAcordada.HasValue)
                                                {
                                                    if (fechaActual > item.FechaAcordada.Value)
                                                    {
                                                        item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                    }
                                                    else
                                                    {
                                                        item.EstadoActual = GetSemaforoTecnologia(item.FechaAcordada.Value, fechaActual, item.MesesIndicador1);
                                                        item.EstadoIndicador1 = GetSemaforoTecnologia(item.FechaAcordada.Value, fechaActual.AddMonths(item.MesesIndicador1), item.MesesIndicador1);
                                                        item.EstadoIndicador2 = GetSemaforoTecnologia(item.FechaAcordada.Value, fechaActual.AddMonths(item.MesesIndicador2), item.MesesIndicador1);
                                                    }
                                                }
                                                else
                                                {
                                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                }
                                                break;
                                            default:
                                                item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                                item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                                item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        item.EstadoActual = (int)ColoresSemaforo.Verde;
                                        item.EstadoIndicador1 = (int)ColoresSemaforo.Verde;
                                        item.EstadoIndicador2 = (int)ColoresSemaforo.Verde;
                                    }
                                }
                                else
                                {
                                    item.EstadoActual = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador1 = (int)ColoresSemaforo.Rojo;
                                    item.EstadoIndicador2 = (int)ColoresSemaforo.Rojo;
                                }
                            }
                        }
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelaciones(PaginaReporteTecnologiasCustom pag, out int totalRows)
        {
            totalRows = 0;
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            pag.DominioIds = pag.DominioIds == null ? string.Empty : pag.DominioIds;
            pag.SubdominioIds = pag.SubdominioIds == null ? string.Empty : pag.SubdominioIds;
            pag.EstadoAprobacionIds = pag.EstadoAprobacionIds == null ? string.Empty : pag.EstadoAprobacionIds;
            pag.Tipos = pag.Tipos == null ? string.Empty : pag.Tipos;
            pag.EstadoObsolescencias = pag.EstadoObsolescencias == null ? string.Empty : pag.EstadoObsolescencias;

            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinRelaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@dominio", pag.DominioIds));
                        comando.Parameters.Add(new SqlParameter("@subdominio", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@estadoaprobacion", pag.EstadoAprobacionIds));
                        comando.Parameters.Add(new SqlParameter("@familia", pag.Familia));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.Tipos));
                        comando.Parameters.Add(new SqlParameter("@estadoobsolescencia", pag.EstadoObsolescencias));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Clave));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", pag.FechaFiltro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        private int GetSemaforoTecnologia(DateTime fechaCalculo, DateTime fechaComparacion, int meses)
        {
            if (fechaCalculo < fechaComparacion)
                return (int)ColoresSemaforo.Rojo;
            else
            {
                fechaComparacion = fechaComparacion.AddMonths(meses);
                if (fechaCalculo < fechaComparacion)
                    return (int)ColoresSemaforo.Amarillo;
                else
                    return (int)ColoresSemaforo.Verde;
            }
        }

        public override byte[] ObtenerArrConsolidado(int Id)
        {
            try
            {
                var ruta = GetRutaConsolidadoByTipo(Id);
                var archivoBytes = File.ReadAllBytes(ruta);

                return archivoBytes;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia_Equipo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasFechasDto> GetReporteTecnologiaSinFechaFin(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasFechasDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinFechaFin]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipoConsulta", pag.TipoConsultaId));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasFechasDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                FlagSiteEstandar = reader.IsDBNull(reader.GetOrdinal("FlagSiteEstandar")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagSiteEstandar")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                TotalComponentes = reader.IsDBNull(reader.GetOrdinal("TotalComponentes")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalComponentes"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalleTecnologiasFechasDto> GetReporteTecnologiaFechaIndefinida(FiltrosDashboardTecnologia pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalleTecnologiasFechasDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasFechaIndefinida]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipoConsulta", pag.TipoConsultaId));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalleTecnologiasFechasDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                //FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                //FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                FlagSiteEstandar = reader.IsDBNull(reader.GetOrdinal("FlagSiteEstandar")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagSiteEstandar")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                TotalComponentes = reader.IsDBNull(reader.GetOrdinal("TotalComponentes")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalComponentes"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteDetalleTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<PieChart> GetIndicadorObsolescenciaSoBd(PaginacionDetalleGraficoSubdominio pag)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<PieChart>();
                try
                {
                    pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    pag.FechaFiltro = DateTime.Now;
                }

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_IndicadorObsolescencia_SoBd]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@dia", pag.FechaFiltro.Day));
                        comando.Parameters.Add(new SqlParameter("@mes", pag.FechaFiltro.Month));
                        comando.Parameters.Add(new SqlParameter("@anio", pag.FechaFiltro.Year));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", pag.SubsidiariaToString));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new PieChart()
                            {
                                cantidad = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                categoria = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia"))
                                //Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                //FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                //Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                //Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                //Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                //TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                //TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                //EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                //TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                //FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte"))
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
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<ConsolidadoDto> GetRelacionesAprobadas()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ConsolidadoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ConsolidadoRemedy_RelacionesAprobadas]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ConsolidadoDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                AmbienteRelacion = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion"))
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
                    , "Error en el metodo: GetRelacionesAprobadas()"
                    , new object[] { null });
            }
        }

        public override List<ConsolidadoDto> GetRelacionesPendientes()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ConsolidadoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ConsolidadoRemedy_RelacionesPendientes]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ConsolidadoDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                AmbienteRelacion = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion"))
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
                    , "Error en el metodo: GetRelacionesPendientes()"
                    , new object[] { null });
            }
        }

        public override List<ConsolidadoDto> GetRelacionesEliminadas()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ConsolidadoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ConsolidadoRemedy_RelacionesEliminadas]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ConsolidadoDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                AmbienteRelacion = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion"))
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
                    , "Error en el metodo: GetRelacionesEliminadas()"
                    , new object[] { null });
            }
        }

        public override List<ConsolidadoDto> GetRelacionesDelta()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ConsolidadoDto>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ConsolidadoRemedy_RelacionesDelta]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ConsolidadoDto()
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                AmbienteRelacion = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                EstadoRelacion = reader.IsDBNull(reader.GetOrdinal("EstadoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRelacion"))
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
                    , "Error en el metodo: GetRelacionesDelta()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionResponsable(string matricula, int tipo)
        {
            var totalRowsDetalle = 0;
            var externo = Settings.Get<string>("Aplicacion.Infraestructura");

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_PorGerenciasDivisionResponsable]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", DateTime.Now));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@externo", externo));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                DetalleCriticidad = reader.IsDBNull(reader.GetOrdinal("DetalleCriticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleCriticidad")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                RoadMap = reader.IsDBNull(reader.GetOrdinal("RoadMap")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMap")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                RiesgoAplicacion = reader.IsDBNull(reader.GetOrdinal("RiesgoAplicacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RiesgoAplicacion")),
                                Priorizacion = reader.IsDBNull(reader.GetOrdinal("Priorizacion")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Priorizacion")),
                                TotalTecnologiasObsoletas = reader.IsDBNull(reader.GetOrdinal("TotalTecnologiasObsoletas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologiasObsoletas")),
                                TotalServidores = reader.IsDBNull(reader.GetOrdinal("TotalServidores")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalServidores")),
                                TotalTecnologias = reader.IsDBNull(reader.GetOrdinal("TotalTecnologias")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalTecnologias")),
                                JefeEquipo_ExpertoAplicacion = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                ExpertoEspecialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                Gestor = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),
                                FlagExterno = reader.IsDBNull(reader.GetOrdinal("FlagExterno")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagExterno"))
                            };

                            if (objeto.EstadoAplicacion == "Eliminada" && objeto.IndiceObsolescencia == 0 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                            {
                                objeto.DetalleActual = new AplicacionIndicador();
                            }
                            else
                            {
                                if (objeto.FlagExterno == 1 && objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                {
                                    objeto.DetalleActual = new AplicacionIndicador();
                                    objeto.IndiceObsolescencia = 0;
                                    objeto.RiesgoAplicacion = 0;
                                    objeto.Priorizacion = 0;
                                }
                                else
                                {
                                    if (objeto.TotalServidores == 0 && objeto.TotalTecnologias == 0)
                                    {
                                        objeto.DetalleActual = new AplicacionIndicador();
                                        objeto.EquipoSinTecnologiasYServidores = true;
                                    }
                                    else
                                    {
                                        if (objeto.IndiceObsolescencia == 0)
                                        {
                                            var detalle = this.GetGerenciaDivisionDetalleResponsable(objeto.CodigoApt);
                                            if (detalle != null)
                                                totalRowsDetalle = detalle.Count;
                                            else
                                                totalRowsDetalle = 0;

                                            if (totalRowsDetalle > 0)
                                            {
                                                var itemActual = new AplicacionIndicador();
                                                itemActual.CodigoApt = objeto.CodigoApt;
                                                itemActual.TotalTecnologias = totalRowsDetalle;
                                                itemActual.TotalVerdesActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 1);
                                                itemActual.TotalVerdesProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 1);

                                                itemActual.TotalAmarillosActual = detalle.Select(x => x.IndicadorObsolescencia).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion1 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion1).Count(x => x == 0);
                                                itemActual.TotalAmarillosProyeccion2 = detalle.Select(x => x.IndicadorObsolescencia_Proyeccion2).Count(x => x == 0);
                                                objeto.DetalleActual = itemActual;
                                            }
                                        }
                                    }
                                }
                            }
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
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleResponsable(string codigoApt)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var mesesTipoCicloVida = this.GetMesesObsolescencia();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_DetalladoPorGerenciasDivisionResponsable_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoApt", codigoApt));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", DateTime.Now));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead"))
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteGerenciaDivisionDto> GetGerenciaDivisionDetalleResponsableAplicaciones(string matricula, int tipo)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var mesesTipoCicloVida = this.GetMesesObsolescencia();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;
                var lista = new List<ReporteGerenciaDivisionDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_DetalladoPorGerenciasDivisionResponsableTodas_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@matricula", matricula));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", DateTime.Now));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteGerenciaDivisionDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                DetalleAmbiente = reader.IsDBNull(reader.GetOrdinal("DetalleAmbiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleAmbiente")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                IndiceObsolescencia = reader.IsDBNull(reader.GetOrdinal("IndiceObsolescencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("IndiceObsolescencia")),
                                Obsoleto = reader.IsDBNull(reader.GetOrdinal("Obsoleto")) ? 1 : reader.GetInt32(reader.GetOrdinal("Obsoleto")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                MesesObsolescencia = reader.IsDBNull(reader.GetOrdinal("MesesObsolescencia")) ? 0 : reader.GetInt32(reader.GetOrdinal("MesesObsolescencia")),
                                Meses = int.Parse(parametroMeses),
                                IndicadorMeses1 = int.Parse(parametroMeses),
                                IndicadorMeses2 = int.Parse(parametroMeses2),
                                TipoTecnologiaId = reader.IsDBNull(reader.GetOrdinal("TipoTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoTecnologiaId")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead"))
                            };
                            if (objeto.MesesObsolescencia == 0)
                            {
                                objeto.MesesObsolescencia = mesesTipoCicloVida;
                            }
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesTodo()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinRelaciones_Todo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinRelacionesTodo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesAplicaciones()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinRelaciones_Aplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinRelacionesAplicaciones()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinRelacionesEquipos()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinRelaciones_Equipos]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinRelacionesEquipos()"
                    , new object[] { null });
            }
        }

        public override List<ReporteServidoresHuerfanosDto> GetServiciosNubeHuerfanos(PaginaReporteHuerfanos pag, DateTime fechaConsulta, out int totalRows)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var tecnologias = (from a in ctx.EquipoTecnologia
                                           join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                           join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                                           where c.TipoEquipoId == pag.TipoEquipo
                                           && a.AnioRegistro == fechaConsulta.Year
                                           && a.MesRegistro == fechaConsulta.Month
                                           && a.DiaRegistro == fechaConsulta.Day
                                           && c.FlagActivo
                                           select new
                                           {
                                               EquipoId = a.EquipoId,
                                               TecnologiaId = b.TecnologiaId,
                                               Tecnologia = b.ClaveTecnologia
                                           }).Distinct();


                        var registros = (from u in ctx.Equipo
                                         join b in ctx.TipoEquipo on u.TipoEquipoId equals b.TipoEquipoId
                                         join d in tecnologias on u.EquipoId equals d.EquipoId into lj1
                                         from d in lj1.DefaultIfEmpty()
                                         where u.FlagActivo && b.FlagActivo
                                            && (u.Nombre.Contains(pag.Equipo) || string.IsNullOrEmpty(pag.Equipo))
                                            && (u.TipoEquipoId == pag.TipoEquipo || pag.TipoEquipo == 0)
                                            && !ctx.Relacion.Any(gi => gi.EquipoId == u.EquipoId
                                                && gi.AnioRegistro == fechaConsulta.Year
                                                && gi.MesRegistro == fechaConsulta.Month
                                                && gi.DiaRegistro == fechaConsulta.Day
                                                && (gi.EstadoId == 2 || gi.EstadoId == 0)
                                                && gi.FlagActivo)
                                         select new ReporteServidoresHuerfanosDto()
                                         {
                                             ClaveTecnologica = d == null ? "-" : d.Tecnologia,
                                             Equipo = u.Nombre,
                                             TipoEquipo = b.Nombre,
                                             UltimaActualizacion = u.FechaUltimoEscaneoCorrecto,
                                             FlagTemporal = u.FlagTemporal,
                                             EquipoId = u.EquipoId,
                                             TecnologiaId = d == null ? 0 : d.TecnologiaId
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder);
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetServiciosNubeHuerfanos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAlertaCVTDTO
                    , "Error en el metodo: GetServiciosNubeHuerfanos()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaTodo()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinFecha_Todo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                Dueno = reader.IsDBNull(reader.GetOrdinal("Dueno")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dueno")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinFechaTodo()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaServidores()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinFecha_Servidores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalEquipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquipos")),
                                Dueno = reader.IsDBNull(reader.GetOrdinal("Dueno")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dueno")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinFechaServidores()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaPCs()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinFecha_PCs]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalEquipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquipos")),
                                Dueno = reader.IsDBNull(reader.GetOrdinal("Dueno")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dueno")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinFechaPCs()"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoTecnologiaDto> GetTecnologiasSinFechaAplicaciones()
        {
            var indicador1 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);
            var indicador2 = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor);

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteDetalladoTecnologiaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_TecnologiasSinFecha_Aplicaciones]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteDetalladoTecnologiaDto()
                            {
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                EstadoTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoTecnologia")),
                                Familia = reader.IsDBNull(reader.GetOrdinal("Familia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Familia")),
                                FechaAcordada = reader.IsDBNull(reader.GetOrdinal("FechaAcordada")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAcordada")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? 0 : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaExtendida = reader.IsDBNull(reader.GetOrdinal("FechaExtendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaExtendida")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FlagFechaFinSoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagFechaFinSoporte")),
                                FuenteId = reader.IsDBNull(reader.GetOrdinal("FuenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FuenteId")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                Dueno = reader.IsDBNull(reader.GetOrdinal("Dueno")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dueno")),
                                MesesIndicador1 = indicador1,
                                MesesIndicador2 = indicador2
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
                    , "Error en el metodo: GetTecnologiasSinFechaAplicaciones()"
                    , new object[] { null });
            }
        }

        public override List<ReporteAcumuladoDto> GetEvolucionSubdominiosEquipo(string tipoEquipos, string subdominios, string subsidiaria, DateTime fecha)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteAcumuladoDto>();
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                nombreSP = "[CVT].[USP_Dashboard_AcumuladoPorSubdominios_Equipos_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[USP_Dashboard_AcumuladoPorSubdominios_Equipos]";
                //}

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", subdominios));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", tipoEquipos));
                        comando.Parameters.Add(new SqlParameter("@subsidiaria", subsidiaria));
                        comando.Parameters.Add(new SqlParameter("@FechaFiltro", fecha));
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteAcumuladoDto()
                            {
                                Anio = reader.IsDBNull(reader.GetOrdinal("Anio")) ? 0 : reader.GetInt32(reader.GetOrdinal("Anio")),
                                Equipos = reader.IsDBNull(reader.GetOrdinal("Equipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("Equipos")),
                                Mes = reader.IsDBNull(reader.GetOrdinal("Mes")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mes")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                            };
                            if (objeto.Anio != 0 && objeto.Mes != 0)
                            {
                                objeto.Fecha = Utilitarios.DevolverNombreFecha(new DateTime(objeto.Anio, objeto.Mes, 1, 0, 0, 0));
                                objeto.FechaFin = new DateTime(objeto.Anio, objeto.Mes, 1, 0, 0, 0);
                            }
                            else
                                objeto.FechaFin = DateTime.MinValue;

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                    {
                        var listaFinal = lista.Where(x => x.FechaFin < DateTime.Now)
                            .OrderByDescending(x => x.Anio).ThenByDescending(x => x.Mes).ToList();

                        if (listaFinal.Count > 0)
                        {
                            var primero = listaFinal[0];
                            var resultado = lista.Except(listaFinal).ToList();
                            resultado.Add(new ReporteAcumuladoDto()
                            {
                                Anio = fecha.Year,
                                Mes = fecha.Month,
                                Equipos = primero.Equipos,
                                Total = primero.Total,
                                Fecha = Utilitarios.DevolverNombreFecha(new DateTime(fecha.Year, fecha.Month, 1, 0, 0, 0))
                            });

                            return resultado.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList();
                        }
                        else
                        {
                            return new List<ReporteAcumuladoDto>();
                        }
                    }
                    else
                        return new List<ReporteAcumuladoDto>();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> GetReporteOwnerByTecnologiav2(PaginacionOwner pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_OwnerTecnologiaDetalle_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        //comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        //comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                EsIndefinida = reader.IsDBNull(reader.GetOrdinal("EsIndefinida")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("EsIndefinida")),
                                FechaCalculoTec = reader.IsDBNull(reader.GetOrdinal("FechaCalculoTec")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FechaCalculoTec")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("FechaCalculoBase")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCalculoBase")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                TotalAplicaciones = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                TotalEquipos = reader.IsDBNull(reader.GetOrdinal("TotalEquipos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquipos")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> GetReporteOwnerByAplicaciones(PaginacionOwner pag, out int totalRows)
        {
            totalRows = 0;
            try
            {
                pag.FechaFiltro = DateTime.ParseExact(pag.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                pag.FechaFiltro = DateTime.Now;
            }

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Dashboard_OwnerTecnologiaAplicaciones_v2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominios", pag.SubdominioToString));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.FechaFiltro));
                        comando.Parameters.Add(new SqlParameter("@tipoTecnologia", pag.TipoTecnologiaToString));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        //comando.Parameters.Add(new SqlParameter("@SortName", pag.sortName));
                        //comando.Parameters.Add(new SqlParameter("@SortOrder", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                NombreEquipo_Squad = reader.IsDBNull(reader.GetOrdinal("NombreEquipo_Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo_Squad")),
                                Criticidad = reader.IsDBNull(reader.GetOrdinal("Criticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Criticidad")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }



        public override List<ReporteEvolucionInstalacionTecnologiasDto> ReporteEvolucionInstalacionTecnologias(string tipoEquipoIds, string subsidiariaIds, DateTime fechaBase, int nroMeses, bool flagAgrupacionFamilia, int? idTecnologia, string fabricante, string nombreTecnologia)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ReporteEvolucionInstalacionTecnologiasDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_EVOLUCION_INSTALACIONES_FAMILIA]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@arrTipoEquipo", tipoEquipoIds));
                        comando.Parameters.Add(new SqlParameter("@arrSubsidiaria", subsidiariaIds));
                        comando.Parameters.Add(new SqlParameter("@fechaBase", fechaBase));
                        comando.Parameters.Add(new SqlParameter("@nroMeses", nroMeses));
                        comando.Parameters.Add(new SqlParameter("@flagFamilia", flagAgrupacionFamilia));
                        comando.Parameters.Add(new SqlParameter("@IdTecnologia", idTecnologia));
                        comando.Parameters.Add(new SqlParameter("@fabricante", fabricante));
                        comando.Parameters.Add(new SqlParameter("@nombreTecnologia", nombreTecnologia));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ReporteEvolucionInstalacionTecnologiasDto()
                            {
                                NroSemana = reader.IsDBNull(reader.GetOrdinal("NroSemana")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroSemana")),
                                Fecha = reader.IsDBNull(reader.GetOrdinal("Fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total"))
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
                    , "Error en el metodo: GetDetalleGrafico()"
                    , new object[] { null });
            }
        }

        private int GetMesesObsolescencia()
        {
            var parametroMeses = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor);

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var tipoCicloDefault = ctx.TipoCicloVida.FirstOrDefault(x => x.FlagActivo && !x.FlagEliminado && x.FlagDefault);
                    if (tipoCicloDefault != null)
                        return tipoCicloDefault.NroPeriodosEstadoAmbar;
                    else
                        return parametroMeses;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: int AddOrEditEntorno(EntornoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEntornoDTO
                    , "Error en el metodo: int AddOrEditEntorno(EntornoDTO objeto)"
                    , new object[] { null });
            }
        }
    }
}
