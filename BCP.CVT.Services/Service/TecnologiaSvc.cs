using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.CVT.Services.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using BCP.PAPP.Common.Cross;
using BCP.CVT.Services.Email;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.Services.Interface.TechnologyConfiguration;
using System.Text.RegularExpressions;
using BCP.CVT.Services.Log;

namespace BCP.CVT.Services.Service
{
    public class TecnologiaSvc : TecnologiaDAO
    {
        #region Formatos fechas
        //DateTime? fecha_lanzamiento, fechaExtendida, fechaFinSoporte, fechaAcordada; 
        #endregion
        
        private void ActualizarEstadoTecnologias(int tecnologia, int familia, int estado)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_CambiarEstadoTecnologiasFamilia]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@familia", familia));
                        comando.Parameters.Add(new SqlParameter("@estado", estado));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        private void ActualizarListasTecnologias(ObjTecnologia prmt)
        {
            try
            {
                var param1 = prmt.param1;
                var param2 = string.IsNullOrEmpty(prmt.param2) ? string.Empty : prmt.param2;
                var param3 = string.IsNullOrEmpty(prmt.param3) ? string.Empty : prmt.param3;
                var param4 = string.IsNullOrEmpty(prmt.param4) ? string.Empty : prmt.param4;
                var param5 = string.IsNullOrEmpty(prmt.param5) ? string.Empty : prmt.param5;
                var param6 = string.IsNullOrEmpty(prmt.param6) ? string.Empty : prmt.param6;
                var param7 = string.IsNullOrEmpty(prmt.param7) ? string.Empty : prmt.param7;
                var param8 = prmt.param8;
                var param9 = prmt.param9;

                //******************SP******************//
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@tecnologiaId", param1, SqlDbType.Int));
                ListsQLParam.Add(new SQLParam("@lstAddAutorizadores", param2, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@lstRemoveAutorizadores", param3, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@lstAddArquetipos", param4, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@lstRemoveArquetipos", param5, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@lstAddTecnologiaVinculada", param6, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@lstRemoveTecnologiaVinculada", param7, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@usuarioCreacion", param8, SqlDbType.NChar));
                ListsQLParam.Add(new SQLParam("@usuarioModificacion", param9, SqlDbType.NChar));

                string SP = "[CVT].[USP_ActualizarListaTecnologia]";

                new SQLManager().EjecutarStoredProcedure_2(SP, ListsQLParam);

            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        private void ActualizarListasTecnologias2(ObjTecnologia prmt)
        {
            try
            {
                var param1 = prmt.param1;
                var param2 = string.IsNullOrEmpty(prmt.param2) ? string.Empty : prmt.param2;
                var param3 = string.IsNullOrEmpty(prmt.param3) ? string.Empty : prmt.param3;
                var param4 = string.IsNullOrEmpty(prmt.param4) ? string.Empty : prmt.param4;
                var param5 = string.IsNullOrEmpty(prmt.param5) ? string.Empty : prmt.param5;
                var param6 = string.IsNullOrEmpty(prmt.param6) ? string.Empty : prmt.param6;
                var param7 = string.IsNullOrEmpty(prmt.param7) ? string.Empty : prmt.param7;
                var param8 = prmt.param8;
                var param9 = prmt.param9;

                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ActualizarListaTecnologia]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tecnologiaId", param1));
                        comando.Parameters.Add(new SqlParameter("@lstAddAutorizadores", param2));
                        comando.Parameters.Add(new SqlParameter("@lstRemoveAutorizadores", param3));
                        comando.Parameters.Add(new SqlParameter("@lstAddArquetipos", param4));
                        comando.Parameters.Add(new SqlParameter("@lstRemoveArquetipos", param5));
                        comando.Parameters.Add(new SqlParameter("@lstAddTecnologiaVinculada", param6));
                        comando.Parameters.Add(new SqlParameter("@lstRemoveTecnologiaVinculada", param7));
                        comando.Parameters.Add(new SqlParameter("@usuarioCreacion", param8));
                        comando.Parameters.Add(new SqlParameter("@usuarioModificacion", param9));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }

            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        private bool ValidarFlagEstandar(int TipoTecnologiaId)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var estado = ServiceManager<TipoDAO>.Provider.ValidarFlagEstandar(TipoTecnologiaId);
                    return estado;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        private void EnviarNotificacionTecnologia(string UsuarioCreacion, string Cuerpo, bool esEstandar = false)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string nombreBuzon, buzon, buzonDefectoTecnologia, asunto = string.Empty;

                    buzon = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TECNOLOGIA_GESTION_BUZON").Valor;
                    nombreBuzon = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TECNOLOGIA_GESTION_BUZON_NOMBRE").Valor;
                    buzonDefectoTecnologia = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TECNOLOGIA_GESTION_BUZON_DEFECTO").Valor;
                    if (!esEstandar)
                        asunto = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TECNOLOGIA_GESTION_ASUNTO_NOTIFICACION").Valor;
                    else
                        asunto = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TECNOLOGIA_GESTION_ASUNTO_ESTANDAR_NOTIFICACION").Valor;

                    NotificacionDTO notificacion = new NotificacionDTO();
                    //notificacion.TipoNotificacionId = (int)ETipoNotificacion.Tecnologia; // ENUM
                    notificacion.Nombre = nombreBuzon;
                    notificacion.De = buzon;
                    notificacion.Para = buzonDefectoTecnologia;
                    notificacion.CC = buzonDefectoTecnologia;
                    notificacion.BCC = string.Empty;
                    notificacion.Cuerpo = Cuerpo;
                    notificacion.Asunto = asunto;
                    notificacion.UsuarioCreacion = UsuarioCreacion;
                    notificacion.FechaCreacion = DateTime.Now;
                    notificacion.Activo = true;
                    notificacion.FlagEnviado = false;
                    notificacion.FechaEnvio = null;
                    int NotificacionId = ServiceManager<NotificacionDAO>.Provider.AddOrEditNotificacion(notificacion);
                }

            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override int AddOrEditTecnologia(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 0;
            var CURRENT_DATE = DateTime.Now;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.Id == 0)
                        {
                            var entidad = new Tecnologia()
                            {
                                EstadoTecnologia = objeto.EstadoTecnologia,
                                SubdominioId = objeto.SubdominioId,
                                TecnologiaId = objeto.Id,
                                Activo = objeto.Activo,
                                ClaveTecnologia = objeto.ClaveTecnologia,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                Nombre = objeto.Nombre,
                                Descripcion = objeto.Descripcion,
                                Versiones = objeto.Versiones,
                                TipoTecnologia = objeto.TipoTecnologiaId,
                                FamiliaId = objeto.FamiliaId,
                                FlagConfirmarFamilia = objeto.FlagConfirmarFamilia,
                                FlagFechaFinSoporte = objeto.FlagFechaFinSoporte,
                                FechaCalculoTec = objeto.FechaCalculoTec,
                                FechaLanzamiento = objeto.FechaLanzamiento,
                                FechaExtendida = objeto.FechaExtendida,
                                FechaFinSoporte = objeto.FechaFinSoporte,
                                FechaAcordada = objeto.FechaAcordada,
                                ComentariosFechaFin = objeto.ComentariosFechaFin,
                                FuenteId = objeto.Fuente,
                                Existencia = objeto.Existencia,
                                Facilidad = objeto.Facilidad,
                                Riesgo = objeto.Riesgo,
                                Vulnerabilidad = objeto.Vulnerabilidad,
                                CasoUso = objeto.CasoUso,
                                Requisitos = objeto.Requisitos,
                                Compatibilidad = objeto.Compatibilidad,
                                Aplica = objeto.Aplica,
                                FlagAplicacion = objeto.FlagAplicacion,
                                CodigoAPT = objeto.CodigoAPT,
                                Fabricante = objeto.Fabricante,
                                //Fin tab 1                        
                                EliminacionTecObsoleta = objeto.EliminacionTecObsoleta,
                                RoadmapOpcional = objeto.RoadmapOpcional,
                                Referencias = objeto.Referencias,
                                PlanTransConocimiento = objeto.PlanTransConocimiento,
                                EsqMonitoreo = objeto.EsqMonitoreo,
                                LineaBaseSeg = objeto.LineaBaseSeg,
                                EsqPatchManagement = objeto.EsqPatchManagement,
                                //Fin tab 2
                                DuenoId = objeto.Dueno,
                                EqAdmContacto = objeto.EqAdmContacto,
                                GrupoSoporteRemedy = objeto.GrupoSoporteRemedy,
                                ConfArqSegId = objeto.ConfArqSeg,
                                ConfArqTecId = objeto.ConfArqTec,
                                EncargRenContractual = objeto.EncargRenContractual,
                                EsqLicenciamiento = objeto.EsqLicenciamiento,
                                SoporteEmpresarial = objeto.SoporteEmpresarial,
                                //TipoId = objeto.TipoId,
                                EstadoId = objeto.EstadoId,
                                //Fin tab 3
                            };
                            ctx.Tecnologia.Add(entidad);
                            ctx.SaveChanges();
                            ID = entidad.TecnologiaId;
                            registroNuevo = true;
                        }
                        else
                        {
                            var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                            if (entidad != null)
                            {
                                //CambiarEstadoObsolescenciaTecnologia(entidad, objeto);

                                //string CuerpoNotificacion = string.Empty;
                                //string CuerpoNotificacionFlagEstandar = string.Empty;
                                //CuerpoNotificacion = string.Format("Tecnología en estado {0}", objeto.EstadoTecnologiaStr);
                                entidad.EstadoId = objeto.EstadoId == -1 ? entidad.EstadoId : objeto.EstadoId;

                                if (objeto.EstadoId == (int)ETecnologiaEstado.Deprecado || objeto.EstadoId == (int)ETecnologiaEstado.Vigente)
                                //if (objeto.EstadoId == (int)ETecnologiaEstado.VigentePorVencer || objeto.EstadoId == (int)ETecnologiaEstado.Vigente)
                                {
                                    if (objeto.TipoTecnologiaId == (int)ETipoTecnologia.NoEstandar)
                                        entidad.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                                }

                                entidad.FlagSiteEstandar = objeto.FlagSiteEstandar;
                                entidad.TipoTecnologia = objeto.TipoTecnologiaId;
                                //entidad.EstadoId = objeto.EstadoId;
                                entidad.FamiliaId = objeto.FamiliaId;
                                entidad.FlagConfirmarFamilia = objeto.FlagConfirmarFamilia;
                                entidad.ClaveTecnologia = objeto.ClaveTecnologia;

                                if (objeto.FlagCambioEstado)
                                    entidad.EstadoTecnologia = objeto.EstadoTecnologia;

                                switch (objeto.EstadoTecnologia)
                                {
                                    //case 1:
                                    //    entidad.EstadoTecnologia = (int)EstadoTecnologia.Registrado;
                                    //    break;
                                    //case 2:
                                    //    entidad.EstadoTecnologia = (int)EstadoTecnologia.EnRevision;
                                    //    break;
                                    case (int)EstadoTecnologia.Aprobado:
                                        //entidad.EstadoTecnologia = (int)EstadoTecnologia.Aprobado;
                                        entidad.FlagSiteEstandar = objeto.FlagSiteEstandar;
                                        entidad.FechaAprobacion = !entidad.FechaAprobacion.HasValue ? CURRENT_DATE : entidad.FechaAprobacion;
                                        entidad.UsuarioAprobacion = objeto.UsuarioModificacion;
                                        entidad.CodigoTecnologiaAsignado = objeto.CodigoTecnologiaAsignado;
                                        entidad.UrlConfluence = objeto.UrlConfluence;
                                        break;
                                    case (int)EstadoTecnologia.Observado:
                                        //entidad.EstadoTecnologia = (int)EstadoTecnologia.Observado;
                                        entidad.Observacion = objeto.Observacion;
                                        break;
                                    default: break;
                                }

                                entidad.Nombre = objeto.Nombre;
                                entidad.Descripcion = objeto.Descripcion;
                                entidad.Versiones = objeto.Versiones;
                                entidad.FlagFechaFinSoporte = objeto.FlagFechaFinSoporte;
                                entidad.FechaCalculoTec = objeto.FechaCalculoTec;
                                entidad.FechaLanzamiento = objeto.FechaLanzamiento;
                                entidad.FechaExtendida = objeto.FechaExtendida;
                                entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                                entidad.FechaAcordada = objeto.FechaAcordada;
                                entidad.ComentariosFechaFin = objeto.ComentariosFechaFin;
                                entidad.FuenteId = objeto.Fuente;
                                entidad.Existencia = objeto.Existencia;
                                entidad.Facilidad = objeto.Facilidad;
                                entidad.Riesgo = objeto.Riesgo;
                                entidad.Vulnerabilidad = objeto.Vulnerabilidad;
                                entidad.CasoUso = objeto.CasoUso;
                                entidad.Requisitos = objeto.Requisitos;
                                entidad.Compatibilidad = objeto.Compatibilidad;
                                entidad.Aplica = objeto.Aplica;
                                entidad.FlagAplicacion = objeto.FlagAplicacion;
                                entidad.CodigoAPT = objeto.CodigoAPT;
                                entidad.Fabricante = objeto.Fabricante;
                                //Fin tab 1
                                entidad.SubdominioId = objeto.SubdominioId;
                                entidad.EliminacionTecObsoleta = objeto.EliminacionTecObsoleta;
                                entidad.RoadmapOpcional = objeto.RoadmapOpcional;
                                entidad.Referencias = objeto.Referencias;
                                entidad.PlanTransConocimiento = objeto.PlanTransConocimiento;
                                entidad.EsqMonitoreo = objeto.EsqMonitoreo;
                                entidad.LineaBaseSeg = objeto.LineaBaseSeg;
                                entidad.EsqPatchManagement = objeto.EsqPatchManagement;
                                //Fin tab 2
                                entidad.DuenoId = objeto.Dueno;
                                entidad.EqAdmContacto = objeto.EqAdmContacto;
                                entidad.GrupoSoporteRemedy = objeto.GrupoSoporteRemedy;
                                entidad.ConfArqSegId = objeto.ConfArqSeg;
                                entidad.ConfArqTecId = objeto.ConfArqTec;
                                entidad.EncargRenContractual = objeto.EncargRenContractual;
                                entidad.EsqLicenciamiento = objeto.EsqLicenciamiento;
                                entidad.SoporteEmpresarial = objeto.SoporteEmpresarial;
                                //entidad.EstadoId = objeto.EstadoId;
                                //Fin tab 3

                                //RONALD
                                entidad.AutomatizacionImplementadaId = objeto.AutomatizacionImplementadaId;
                                entidad.AplicacionId = objeto.AplicacionId;

                                entidad.FechaModificacion = CURRENT_DATE;
                                entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                                ctx.SaveChanges();

                                //if (ValidarFlagEstandar(objeto.TipoTecnologiaId.Value))
                                //{
                                //    CuerpoNotificacionFlagEstandar = Utilitarios.GetBodyNotification();
                                //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[FECHA_HORA]", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[CLAVE_TECNOLOGIA]", objeto.ClaveTecnologia);
                                //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[MATRICULA]", objeto.UsuarioCreacion);
                                //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[NOMBRES]", objeto.UsuarioCreacion);
                                //    this.EnviarNotificacionTecnologia(objeto.UsuarioCreacion, CuerpoNotificacionFlagEstandar, true);
                                //}

                                //if(objeto.FlagCambioEstado)
                                //{
                                //    this.EnviarNotificacionTecnologia(objeto.UsuarioCreacion, CuerpoNotificacion);
                                //}                                                                
                                ID = entidad.TecnologiaId;
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

            try
            {
                if (registroNuevo)
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }
                else
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }

                if ((objeto.EstadoId != (int)ETecnologiaEstado.Obsoleto && objeto.EstadoId.HasValue)
                                   && objeto.EstadoTecnologia == (int)EstadoTecnologia.Aprobado && objeto.FlagUnicaVigente == true)
                {
                    //this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.VigentePorVencer);
                    this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.Deprecado);
                }

                return ID;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        private void CambiarEstadoObsolescenciaTecnologia(Tecnologia entidad, TecnologiaDTO objeto)
        {
            if (objeto.EstadoId == (int)ETecnologiaEstado.Deprecado || objeto.EstadoId == (int)ETecnologiaEstado.Vigente)
            //if (objeto.EstadoId == (int)ETecnologiaEstado.VigentePorVencer || objeto.EstadoId == (int)ETecnologiaEstado.Vigente)
            {
                if (objeto.TipoTecnologiaId == (int)ETipoTecnologia.NoEstandar)
                    entidad.EstadoId = (int)ETecnologiaEstado.Obsoleto;
            }
            else if (objeto.EstadoId == (int)ETecnologiaEstado.Obsoleto)
            {
                var flagCambioFecha = entidad.FechaCalculoTec != objeto.FechaCalculoTec;
                var flagCambioTipo = entidad.TipoTecnologia != objeto.TipoTecnologiaId;

                if (flagCambioFecha || flagCambioTipo)
                {
                    DateTime? FFC = null;
                    DateTime FECHA_ACTUAL = DateTime.Now;
                    if (objeto.TipoTecnologiaId != (int)ETipoTecnologia.NoEstandar)
                    {
                        if (objeto.FechaCalculoTec.HasValue)
                        {
                            switch (objeto.FechaCalculoTec.Value)
                            {
                                case (int)FechaCalculoTecnologia.FechaExtendida:
                                    FFC = objeto.FechaExtendida;
                                    break;
                                case (int)FechaCalculoTecnologia.FechaFinSoporte:
                                    FFC = objeto.FechaFinSoporte;
                                    break;
                                case (int)FechaCalculoTecnologia.FechaInterna:
                                    FFC = objeto.FechaAcordada;
                                    break;
                            }

                            if (FFC > FECHA_ACTUAL)
                                //entidad.EstadoId = (int)ETecnologiaEstado.VigentePorVencer;
                                entidad.EstadoId = (int)ETecnologiaEstado.Deprecado;
                        }
                        else
                            //entidad.EstadoId = (int)ETecnologiaEstado.VigentePorVencer;
                            entidad.EstadoId = (int)ETecnologiaEstado.Deprecado;
                    }
                }
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //Activar tecnologia
                    var oTecnologia = ctx.Tecnologia.Where(o => o.TecnologiaId == id).FirstOrDefault();  

                    if (!ReferenceEquals(null, oTecnologia))
                    {
                        oTecnologia.FechaModificacion = DateTime.Now;
                        oTecnologia.UsuarioModificacion = usuario;
                        oTecnologia.Activo = estado; 
                        ctx.SaveChanges(); 
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<RelacionTecnologiaDTO> GetAllTecnologia()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join f in ctx.Familia on u.FamiliaId equals f.FamiliaId
                                       where u.Activo && s.Activo && d.Activo
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       && f.Activo
                                       orderby u.Nombre
                                       select new RelacionTecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           Tecnologia = u.Nombre,
                                           Dominio = d.Nombre,
                                           Subdominio = s.Nombre,
                                           Familia = f.Nombre,
                                           Activo = u.Activo
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetAllTecnologia()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetAllTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecnologiasByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo
                                       && u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                       //&& u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           ClaveTecnologia = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetAllTecnologia()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetAllTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTec(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tecnologiaIds = (from et in ctx.EquipoTecnologia
                                             join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                             where et.FlagActivo && e.FlagActivo
                                             && e.Nombre.ToUpper().Contains(equipo.ToUpper())
                                             select et.TecnologiaId);

                        var tecEquivalenciaIds = (from e in ctx.TecnologiaEquivalencia
                                                  where e.FlagActivo && e.Nombre.ToUpper().Contains(nombre.ToUpper())
                                                  select e.TecnologiaId);

                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join p in ctx.Producto on u.ProductoId equals p.ProductoId
                                            into TecnologiaProducto
                                         from tp in TecnologiaProducto.DefaultIfEmpty()

                                             //join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId into lj2
                                             //from te in lj2.DefaultIfEmpty()
                                             //join e in ctx.Equipo on et.EquipoId equals e.EquipoId into lj3
                                             //from e in lj3.DefaultIfEmpty()
                                         where u.Activo && u.SubdominioId == (subdomId == -1 ? u.SubdominioId : subdomId) &&
                                         (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre)
                                         || u.Descripcion.ToUpper().Contains(nombre.ToUpper())
                                         || u.ClaveTecnologia.ToUpper().Contains(nombre.ToUpper())
                                         //|| te.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || tecEquivalenciaIds.Contains(u.TecnologiaId)
                                         )
                                         && (s.DominioId == (domId == -1 ? s.DominioId : domId))
                                         && (u.Aplica.ToUpper().Contains(aplica.ToUpper()) || string.IsNullOrEmpty(aplica))
                                         && (u.CodigoTecnologiaAsignado.ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))
                                         && (u.DuenoId.ToUpper().Contains(dueno.ToUpper()) || string.IsNullOrEmpty(dueno))
                                         && (string.IsNullOrEmpty(equipo) || tecnologiaIds.Contains(u.TecnologiaId))
                                         //&& (e.Nombre.ToUpper().Contains(equipo.ToUpper()) || string.IsNullOrEmpty(equipo))
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Tipo = t.Nombre,
                                             Estado = u.EstadoTecnologia,
                                             //TipoId = u.TipoId,
                                             EstadoId = u.EstadoId,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             TribuCoeId = tp.TribuCoeId,
                                             TribuCoeName = tp.TribuCoeDisplayName,
                                             SquadId = tp.SquadId,
                                             SquadName = tp.SquadDisplayName,
                                             OwnerId = tp.OwnerId,
                                             OwnerDisplay = tp.OwnerDisplayName
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTec(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTec(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TecnologiaDTO GetTecById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into ljf
                                       from f in ljf.DefaultIfEmpty()
                                       join t in ctx.Aplicacion on u.CodigoAPT equals t.CodigoAPT into lj1
                                       from t in lj1.DefaultIfEmpty()
                                           //join a in ctx.Arquetipo on u.ArquetipoId equals a.ArquetipoId into lj2
                                           //from a in lj2.DefaultIfEmpty()
                                       where u.TecnologiaId == id
                                       //&& t.FlagActivo
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           Activo = u.Activo,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           EstadoTecnologia = u.EstadoTecnologia,
                                           Versiones = u.Versiones,
                                           FamiliaId = u.FamiliaId,
                                           FamiliaNomb = f == null ? null : f.Nombre,
                                           FlagConfirmarFamilia = u.FlagConfirmarFamilia.HasValue ? u.FlagConfirmarFamilia.Value : false,
                                           ClaveTecnologia = u.ClaveTecnologia,
                                           TipoTecnologiaId = u.TipoTecnologia,
                                           FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                           FechaCalculoTec = u.FechaCalculoTec,
                                           FechaLanzamiento = u.FechaLanzamiento,
                                           FechaExtendida = u.FechaExtendida,
                                           FechaFinSoporte = u.FechaFinSoporte,
                                           FechaAcordada = u.FechaAcordada,
                                           ComentariosFechaFin = u.ComentariosFechaFin,
                                           Fuente = u.FuenteId,
                                           Existencia = u.Existencia,
                                           Facilidad = u.Facilidad,
                                           Riesgo = u.Riesgo,
                                           Vulnerabilidad = u.Vulnerabilidad,
                                           CasoUso = u.CasoUso,
                                           Requisitos = u.Requisitos,
                                           Compatibilidad = u.Compatibilidad,
                                           Aplica = u.Aplica,
                                           FlagAplicacion = u.FlagAplicacion,
                                           CodigoAPT = u.CodigoAPT,
                                           AplicacionNomb = u.CodigoAPT == null ? "" : t.CodigoAPT + " - " + t.Nombre,
                                           //Fin Tab 1
                                           DominioId = s.DominioId,
                                           SubdominioId = u.SubdominioId,
                                           SubdominioNomb = s.Nombre,
                                           EliminacionTecObsoleta = u.EliminacionTecObsoleta,
                                           RoadmapOpcional = u.RoadmapOpcional,
                                           Referencias = u.Referencias,
                                           PlanTransConocimiento = u.PlanTransConocimiento,
                                           EsqMonitoreo = u.EsqMonitoreo,
                                           LineaBaseSeg = u.LineaBaseSeg,
                                           EsqPatchManagement = u.EsqPatchManagement,
                                           //Fin Tab 2
                                           Dueno = u.DuenoId,
                                           EqAdmContacto = u.EqAdmContacto,
                                           GrupoSoporteRemedy = u.GrupoSoporteRemedy,
                                           ConfArqSeg = u.ConfArqSegId,
                                           ConfArqTec = u.ConfArqTecId,
                                           EncargRenContractual = u.EncargRenContractual,
                                           EsqLicenciamiento = u.EsqLicenciamiento,
                                           SoporteEmpresarial = u.SoporteEmpresarial,
                                           //Fin Tab 3
                                           FlagSiteEstandar = u.FlagSiteEstandar,
                                           Fabricante = u.Fabricante,
                                           EstadoId = u.EstadoId,
                                           CodigoTecnologiaAsignado = u.CodigoTecnologiaAsignado,
                                           UrlConfluence = u.UrlConfluence,
                                           //ArquetipoId = u.ArquetipoId,
                                           //ArquetipoNombre = a.Nombre
                                           //Fin Tab 4
                                           ProductoId = u.ProductoId,
                                           AutomatizacionImplementadaId = u.AutomatizacionImplementadaId,
                                           AplicacionId = u.AplicacionId
                                       }).FirstOrDefault();

                        if (entidad != null)
                        {
                            if (entidad.EliminacionTecObsoleta != null)
                            {
                                //var tecIdObs = entidad.EliminacionTecObsoleta;
                                var itemTecObs = ctx.Tecnologia.Find(entidad.EliminacionTecObsoleta);
                                if (itemTecObs != null)
                                    entidad.EliminacionTecNomb = itemTecObs.Nombre;
                            }
                            else
                            {
                                entidad.EliminacionTecNomb = string.Empty;
                            }

                            var listAutorizadores = (from u in ctx.Tecnologia
                                                     join at in ctx.AutorizadorTecnologia on u.TecnologiaId equals at.TecnologiaId
                                                     where u.TecnologiaId == id && u.Activo && at.Activo
                                                     select new AutorizadorDTO()
                                                     {
                                                         MatriculaBanco = at.MatriculaBanco,
                                                         Id = at.AutorizadorId,
                                                         Activo = at.Activo
                                                     }).ToList();

                            var listArquetipo = (from u in ctx.Arquetipo
                                                 join at in ctx.ArquetipoTecnologia on new { ArquetipoId = u.ArquetipoId, TecnologiaId = entidad.Id } equals new { ArquetipoId = at.ArquetipoId, TecnologiaId = at.TecnologiaId }
                                                 where at.Activo
                                                 select new ArquetipoDTO()
                                                 {
                                                     Id = u.ArquetipoId,
                                                     Nombre = u.Nombre
                                                 }).ToList();

                            var listTecnologiaVinculadas = (from u in ctx.Tecnologia
                                                            join tv in ctx.TecnologiaVinculada on u.TecnologiaId equals tv.VinculoId
                                                            join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                                            join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                            where tv.Activo && tv.TecnologiaId == entidad.Id
                                                            select new TecnologiaDTO()
                                                            {
                                                                Id = tv.VinculoId,
                                                                Nombre = u.ClaveTecnologia,
                                                                DominioNomb = s.Nombre,
                                                                SubdominioNomb = d.Nombre
                                                            }).ToList();

                            var listEquivalencias = (from u in ctx.TecnologiaEquivalencia
                                                     where u.FlagActivo && u.TecnologiaId == id
                                                     select 1).ToList();


                            entidad.ListAutorizadores = listAutorizadores;
                            entidad.ListArquetipo = listArquetipo;
                            entidad.ListTecnologiaVinculadas = listTecnologiaVinculadas;
                            entidad.FlagTieneEquivalencias = listEquivalencias.Count > 0 ? true : false;

                            int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                                       where t.CodigoInterno == (int)ETablaProcedencia.CVT_Tecnologia
                                                       && t.FlagActivo
                                                       select t.TablaProcedenciaId).FirstOrDefault();
                            if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + (int)ETablaProcedencia.CVT_Tecnologia);

                            var archivo = (from u in ctx.ArchivosCVT
                                           where u.Activo && u.EntidadId == id.ToString() && u.TablaProcedenciaId == TablaProcedenciaId
                                           select new ArchivosCvtDTO()
                                           {
                                               Id = u.ArchivoId,
                                               Nombre = u.Nombre
                                           }).FirstOrDefault();

                            if (archivo != null)
                            {
                                entidad.ArchivoId = archivo.Id;
                                entidad.ArchivoStr = archivo.Nombre;
                            }
                        }

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioDTO> GetSubByDom(int domId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where (u.DominioId == domId)
                                       orderby u.Nombre
                                       select new SubdominioDTO()
                                       {
                                           Activo = u.Activo,
                                           Id = u.SubdominioId,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           UsuarioModificacion = u.UsuarioModificacion,
                                           UsuarioAsociadoPor = u.UsuarioAsociadoPor,
                                           FechaAsociacion = u.FechaAsociacion,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           MatriculaDueno = u.Dueno,
                                           CalculoObs = u.CalObs,
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubByDom(int domId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubByDom(int domId)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecSTD(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, 
            string dueno, string equipo, List<int> tipoTecIds, List<int> estObsIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            { 
                totalRows = 0;  
                var result = ServiceManager<TechnologyConfigurationDAO>.Provider.GetAllTechnologyDesactivated(domIds, subdomIds, casoUso, filtro, estadoIds, famId, fecId, aplica, codigo, dueno, equipo, tipoTecIds, estObsIds, sortName, sortOrder, flagActivo); 
                
                if(result != null)
                    totalRows = result.Count();
                //Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                return result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); 

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoSTD(int id, int estadoTec, string obs, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Tecnologia
                                  where u.TecnologiaId == id
                                  select u).First();

                    if (itemBD != null)
                    {
                        itemBD.EstadoTecnologia = estadoTec;
                        string CuerpoNotificacion = string.Empty;
                        switch (estadoTec)
                        {
                            case (int)EstadoTecnologia.Aprobado:
                                itemBD.FechaAprobacion = DateTime.Now;
                                itemBD.UsuarioAprobacion = usuario;
                                CuerpoNotificacion = "Tecnología en estado Aprobado";
                                break;

                            case (int)EstadoTecnologia.Observado:
                                itemBD.Observacion = obs;
                                CuerpoNotificacion = "Tecnología en estado Observado";
                                break;
                        }

                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        ctx.SaveChanges();

                        //this.EnviarNotificacionTecnologia(usuario, CuerpoNotificacion);

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarEstadoSTD(int id, int estadoTec, string obs, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarEstadoSTD(int id, int estadoTec, string obs, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaEquivalenciaDTO> GetTecEqByTec(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                         join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId
                                         where u.TecnologiaId == (id == 0 ? u.TecnologiaId : id)
                                         && u.Activo
                                         && te.FlagActivo
                                         select new TecnologiaEquivalenciaDTO
                                         {
                                             Id = te.TecnologiaEquivalenciaId,
                                             TecnologiaId = te.TecnologiaId,
                                             NombreTecnologia = u.ClaveTecnologia,
                                             DominioTecnologia = d.Nombre,
                                             SubdominioTecnologia = s.Nombre,
                                             TipoTecnologia = t.Nombre,
                                             EstadoId = u.EstadoId,
                                             Nombre = te.Nombre
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaEquivalenciaDTO> GetTecEqByTec(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaEquivalenciaDTO> GetTecEqByTec(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTec()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           Nombre = u.Nombre
                                       });

                        return entidad.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTec()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTec()"
                    , new object[] { null });
            }
        }

        public override bool AsociarTecEq(int tecId, string equivalencia, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = new TecnologiaEquivalencia()
                    {
                        TecnologiaId = tecId,
                        Nombre = equivalencia,
                        FlagActivo = true,
                        FechaCreacion = DateTime.Now,
                        CreadoPor = usuario
                    };

                    ctx.TecnologiaEquivalencia.Add(entidad);
                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool AsociarTecEq(int tecId, string equivalencia, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool AsociarTecEq(int tecId, string equivalencia, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteTecnologiaById(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Tecnologia
                                        where u.Activo && u.TecnologiaId == Id
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteTecnologiaById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteTecnologiaById(int Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteEquivalencia(string equivalencia)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.TecnologiaEquivalencia
                                        where u.FlagActivo && u.Nombre.ToUpper() == equivalencia.ToUpper()
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteEquivalencia(string equivalencia)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteEquivalencia(string equivalencia)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllTecnologia(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join f in ctx.Familia on u.FamiliaId equals f.FamiliaId
                                       where u.Activo && s.Activo && d.Activo && f.Activo
                                       && (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologia(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologia(string filtro)"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocompleteRelacion> GetAllTecnologiaByClaveTecnologia(string filtro, string subdominioIds = null)
        {
            try
            {
                var subdominioIntIds = new List<int>();
                if (!string.IsNullOrEmpty(subdominioIds))
                    subdominioIntIds = subdominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                       join p in ctx.Producto on u.ProductoId equals p.ProductoId
                                       join s in ctx.Subdominio on p.SubDominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       //join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into ljf
                                       //from f in ljf.DefaultIfEmpty()
                                       where u.Activo && s.Activo && d.Activo //&& f.Activo
                                       && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       && (string.IsNullOrEmpty(subdominioIds) || subdominioIntIds.Contains(p.SubDominioId))
                                       //&& u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new CustomAutocompleteRelacion()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia,
                                           TipoTecnologia = t.Nombre,
                                           FechaFinSoporte = u.FechaFinSoporte,
                                           Dominio = d.Nombre,
                                           Subdominio = s.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteTecnologia> GetTecnologiaArquetipoByClaveTecnologia(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join f in ctx.Familia on u.FamiliaId equals f.FamiliaId
                                       where u.Activo && s.Activo && d.Activo && f.Activo
                                       && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new CustomAutocompleteTecnologia()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia,
                                           Dominio = d.Nombre,
                                           Subdominio = s.Nombre,
                                           Familia = f.Nombre,
                                           ActivoDetalle = u.Activo ? "Activo" : "Inactivo"
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarFlagEquivalencia(int id, string usuario)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var itemBD = (from u in ctx.TecnologiaEquivalencia
                                      where u.TecnologiaEquivalenciaId == id
                                      select u).FirstOrDefault();

                        if (itemBD != null)
                        {
                            itemBD.FlagActivo = false;
                            itemBD.FechaModificacion = DateTime.Now;
                            itemBD.ModificadoPor = usuario;

                            ctx.SaveChanges();
                            scope.Complete();

                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteClaveTecnologia(string clave, int? id, int? flagActivo)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Tecnologia
                                        where (u.Activo == (flagActivo != null))
                                        && u.ClaveTecnologia.ToUpper() == clave.ToUpper()
                                        && (id == null || u.TecnologiaId != id.Value)
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string clave, int? id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string clave, int? id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteTecnologiaVinculada> GetAllTecnologia(string filtro, int? id, int[] idsTec)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join f in ctx.Familia on u.FamiliaId equals f.FamiliaId
                                       where u.Activo && s.Activo && d.Activo && f.Activo
                                       && (id == null || u.TecnologiaId != id)
                                       && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       && (idsTec.Count() == 0 || !idsTec.Contains(u.TecnologiaId))
                                       orderby u.Nombre
                                       select new CustomAutocompleteTecnologiaVinculada()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           Familia = f.Nombre,
                                           Dominio = d.Nombre,
                                           Subdominio = s.Nombre,
                                           value = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocompleteTecnologiaVinculada> GetAllTecnologia(string filtro, int? id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocompleteTecnologiaVinculada> GetAllTecnologia(string filtro, int? id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetListaAutoCompletarTecnologia(string filtro, int? id, string dominioIds = null, string subDominioIds = null)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Buscar_Tecnologia_Autocomplete_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<int>("TecnologiaId").ToString(),
                                Descripcion = reader.GetData<string>("ClaveTecnologia"),
                                value = reader.GetData<string>("ClaveTecnologia"),
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
                    , "Error en el metodo: GetListaAutoCompletarTecnologia()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro, int? id, string dominioIds = null, string subDominioIds = null)
        {
            try
            {
                IEnumerable<int> listaDominioIds = string.IsNullOrEmpty(dominioIds) ? Enumerable.Empty<int>() : dominioIds.Split(',').Select(x => int.Parse(x)).AsEnumerable();
                IEnumerable<int> listaSubDominioIds = string.IsNullOrEmpty(subDominioIds) ? Enumerable.Empty<int>() : subDominioIds.Split(',').Select(x => int.Parse(x)).AsEnumerable();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join b in ctx.Subdominio on u.SubdominioId equals b.SubdominioId
                                       //join a in ctx.Dominio on b.DominioId equals a.DominioId
                                       where u.Activo
                                       && !u.FlagEliminado
                                       && u.TecnologiaId != id
                                       && (string.IsNullOrEmpty(filtro) || (u.ClaveTecnologia).ToUpper().Contains(filtro.ToUpper()))
                                       && (u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado) 
                                       && 
                                         (listaDominioIds.Contains(b.DominioId) || listaDominioIds.Count() == 0) &&
                                         (listaSubDominioIds.Contains(b.SubdominioId) || listaSubDominioIds.Count() == 0)
                                       //orderby u.Nombre
                                       orderby u.ClaveTecnologia
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro, int? id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro, int? id)"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetAllTecnologiaAzure()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo
                                       && !u.FlagEliminado
                                       && ((u.ClaveTecnologia).ToUpper().Contains("AZURE"))
                                       && (u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado)
                                       orderby u.ClaveTecnologia
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaAzure()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaAzure()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTecnologiaForBusqueda(string filtro, int? id, bool flagActivo)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo == flagActivo
                                       && (string.IsNullOrEmpty(filtro)
                                       || (u.ClaveTecnologia).ToUpper().Contains(filtro.ToUpper())
                                       || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                       && (id == null || u.TecnologiaId != id.Value)
                                       orderby u.ClaveTecnologia
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaForBusqueda(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaForBusqueda(string filtro)"
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

        #region Dashboard
        public override FiltrosDashboardTecnologia ListFiltrosDashboard()
        {
            try
            {
                FiltrosDashboardTecnologia arr_data = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosDashboardTecnologia();


                        arr_data.Dominio = (from t in ctx.Tecnologia
                                            join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                            join d in ctx.Dominio on s.DominioId equals d.DominioId
                                            where t.Activo
                                            && s.Activo && d.Activo
                                            select new CustomAutocomplete()
                                            {
                                                Id = d.DominioId.ToString(),
                                                Descripcion = d.Nombre
                                            }).Distinct().OrderBy(z => z.Descripcion).ToList();

                        arr_data.Subdominio = (from t in ctx.Tecnologia
                                               join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                               join d in ctx.Dominio on s.DominioId equals d.DominioId
                                               where t.Activo
                                               //&& t.EstadoId == (int)EstadoTecnologia.Aprobado
                                               && s.Activo
                                               //group s by new { s.SubdominioId, s.Nombre, s.DominioId, Dominio = d.Nombre } into grp
                                               select new CustomAutocomplete()
                                               {
                                                   Id = s.SubdominioId.ToString(),
                                                   Descripcion = s.Nombre,
                                                   TipoId = s.DominioId.ToString(),
                                                   TipoDescripcion = d.Nombre
                                               }).Distinct().OrderBy(z => z.Descripcion).ToList();

                        arr_data.TipoEquipo = (from te in ctx.TipoEquipo
                                               where te.FlagActivo
                                               //orderby te.Nombre
                                               select new CustomAutocomplete()
                                               {
                                                   Id = te.TipoEquipoId.ToString(),
                                                   Descripcion = te.Nombre
                                               }).OrderBy(z => z.Descripcion).ToList();

                        arr_data.DominioRed = (from te in ctx.DominioServidor
                                               where te.Activo
                                               //orderby te.Nombre
                                               select new CustomAutocomplete()
                                               {
                                                   Id = te.DominioId.ToString(),
                                                   Descripcion = te.Nombre
                                               }).OrderBy(z => z.Descripcion).ToList();

                        var listAgrupacion = Utilitarios.EnumToList<Agrupacion>().ToList();
                        arr_data.AgrupacionFiltro = (from ag in listAgrupacion
                                                     select new CustomAutocomplete()
                                                     {
                                                         Id = Utilitarios.GetEnumDescription2(ag),
                                                         Descripcion = Utilitarios.GetEnumDescription2(ag),
                                                     }).ToList();

                        arr_data.EstadoAplicacion = (from a in ctx.Aplicacion
                                                     select new CustomAutocomplete()
                                                     {
                                                         Id = a.EstadoAplicacion,
                                                         Descripcion = a.EstadoAplicacion
                                                     }).Distinct().ToList();

                        arr_data.TipoTecnologia = (from a in ctx.Tipo
                                                   where a.Activo
                                                   select new CustomAutocomplete()
                                                   {
                                                       Id = a.TipoId.ToString(),
                                                       Descripcion = a.Nombre
                                                   }).Distinct().ToList();

                        var listTipoConsulta = Utilitarios.EnumToList<ETipoConsulta>();
                        //arr_data.TipoConsulta = listTipoConsulta.Select(x => new CustomAutocomplete { Id = x.ToString(), Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
                        arr_data.TipoConsulta = listTipoConsulta.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
                        //arr_data.TipoConsulta = Fuente;
                        //arr_data.TipoConsulta = (from ag in listTipoConsulta
                        //                         select new CustomAutocomplete()
                        //                         {
                        //                             Id = ag
                        //                             Descripcion = Utilitarios.GetEnumDescription2(ag),
                        //                         }).ToList();

                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboard()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboard()"
                    , new object[] { null });
            }
        }
        public override FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio(List<int> idsSubdominio)
        {
            try
            {
                FiltrosDashboardTecnologia arr_data = null;
                var arrIdsSubdominios = idsSubdominio.ToArray();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosDashboardTecnologia();

                        arr_data.Familia = (from t in ctx.Tecnologia
                                            join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                                            join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                            where t.Activo
                                            && f.Activo
                                            && arrIdsSubdominios.Contains(t.SubdominioId)
                                            select new CustomAutocomplete()
                                            {
                                                Id = f.FamiliaId.ToString(),
                                                Descripcion = f.Nombre,
                                                //TipoId = s.SubdominioId.ToString(),
                                                //TipoDescripcion = s.Nombre
                                            }).Distinct().OrderBy(z => z.Descripcion).ToList();



                        arr_data.Fabricante = (from t in ctx.Tecnologia
                                               join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                               where t.Activo
                                               && !string.IsNullOrEmpty(t.Fabricante)
                                               && arrIdsSubdominios.Contains(t.SubdominioId)
                                               //orderby s.Nombre, t.Fabricante
                                               select new CustomAutocomplete()
                                               {
                                                   Id = t.Fabricante,
                                                   Descripcion = t.Fabricante,
                                                   //TipoId = s.SubdominioId.ToString(),
                                                   //TipoDescripcion = s.Nombre
                                               }).Distinct().OrderBy(z => z.Descripcion).ToList();


                        arr_data.ClaveTecnologia = (from t in ctx.Tecnologia
                                                    join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                                    where t.Activo
                                                    && !string.IsNullOrEmpty(t.ClaveTecnologia)
                                                   && arrIdsSubdominios.Contains(t.SubdominioId)
                                                    select new CustomAutocompleteTecnologia()
                                                    {
                                                        Id = t.ClaveTecnologia,
                                                        Descripcion = t.ClaveTecnologia,
                                                        //TipoId = s.SubdominioId.ToString(),
                                                        //TipoDescripcion = s.Nombre,
                                                        //FamiliaId = t.FamiliaId,
                                                        //Fabricante = t.Fabricante
                                                    }).Distinct().OrderBy(z => z.Descripcion).ToList();


                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
        }
        public override FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominioXFabricante(List<int> idsSubdominio, List<string> idsFabricante)
        {
            try
            {
                FiltrosDashboardTecnologia arr_data = null;
                var arrIdsSubdominios = idsSubdominio.ToArray();
                var arrIdsFabricante = idsFabricante.ToArray();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosDashboardTecnologia();


                        arr_data.ClaveTecnologia = (from t in ctx.Tecnologia
                                                    join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                                    where t.Activo
                                                    && !string.IsNullOrEmpty(t.ClaveTecnologia)
                                                    && arrIdsSubdominios.Contains(t.SubdominioId)
                                                    && arrIdsFabricante.Contains(t.Fabricante)
                                                    select new CustomAutocompleteTecnologia()
                                                    {
                                                        Id = t.ClaveTecnologia,
                                                        Descripcion = t.ClaveTecnologia,
                                                        TipoId = s.SubdominioId.ToString(),
                                                        TipoDescripcion = s.Nombre,
                                                        FamiliaId = t.FamiliaId,
                                                        Fabricante = t.Fabricante
                                                    }).Distinct().OrderBy(z => z.Descripcion).ToList();


                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
        }


        public override List<DashboardBase> GetReport(FiltrosDashboardTecnologia filtros, bool isExportar = false)
        {
            try
            {
                List<DashboardBase> dashboardBase = null;
                try
                {
                    filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    filtros.FechaFiltro = DateTime.Now;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        int DIA = filtros.FechaFiltro.Day;
                        int MES = filtros.FechaFiltro.Month;
                        int ANIO = filtros.FechaFiltro.Year;
                        DateTime fechaActual = new DateTime(ANIO, MES, DIA);
                        int MESES_CERCA_FIN_SOPORTE = 12; //2 MESES
                                                          //int MESES_vigente = 2; //2 MESES

                        //DateTime FECHA_2MESES = DateTime.Now.AddMonths(2);
                        //DateTime FECHA_12MESES = DateTime.Now.AddMonths(12);

                        if (filtros.SubdominioFiltrar == null) filtros.SubdominioFiltrar = new List<int>();
                        if (filtros.DominioFiltrar == null) filtros.DominioFiltrar = new List<int>();
                        if (filtros.ClaveTecnologiaFiltrar == null) filtros.ClaveTecnologiaFiltrar = new List<string>();
                        if (filtros.FabricanteFiltrar == null) filtros.FabricanteFiltrar = new List<string>();
                        if (filtros.FamiliaFiltrar == null) filtros.FamiliaFiltrar = new List<int>();
                        if (filtros.TipoEquipoFiltrar == null) filtros.TipoEquipoFiltrar = new List<int>();
                        if (filtros.SubsidiariaFiltrar == null) filtros.SubsidiariaFiltrar = new List<string>();

                        var tecnologia = (from t in ctx.Tecnologia
                                          join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                          join d in ctx.Dominio on s.DominioId equals d.DominioId
                                          where t.Activo && s.Activo && d.Activo
                                          && (filtros.SubdominioFiltrar.Count == 0 || filtros.SubdominioFiltrar.Contains(s.SubdominioId))
                                          && (filtros.DominioFiltrar.Count == 0 || filtros.DominioFiltrar.Contains(d.DominioId))
                                          && (filtros.FabricanteFiltrar.Count == 0 || filtros.FabricanteFiltrar.Contains(t.Fabricante))
                                          && (filtros.ClaveTecnologiaFiltrar.Count == 0 || filtros.ClaveTecnologiaFiltrar.Contains(t.ClaveTecnologia))
                                          && (filtros.FamiliaFiltrar.Count == 0 || filtros.FamiliaFiltrar.Contains(t.FamiliaId.Value))
                                          select new
                                          {
                                              t.TecnologiaId,
                                              t.ClaveTecnologia
                                          }).ToList();

                        List<int> tecnologiaIds = tecnologia.Select(x => x.TecnologiaId).ToList();
                        if (tecnologiaIds == null) tecnologiaIds = new List<int>();

                        var tecnologiaCicloVida = (from tcv in ctx.TecnologiaCicloVida
                                                   where (tecnologiaIds.Count == 0 || tecnologiaIds.Contains(tcv.TecnologiaId))
                                                   && tcv.DiaRegistro == DIA && tcv.MesRegistro == MES && tcv.AnioRegistro == ANIO
                                                   select new
                                                   {
                                                       tcv.TecnologiaId,
                                                       tcv.FechaCalculoBase,
                                                       tcv.Obsoleto,
                                                       tcv.EsIndefinida
                                                   }).ToList();

                        var equipoTecnologiaTmp = (from et in ctx.EquipoTecnologia
                                                   join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                                   where (tecnologiaIds.Count == 0 || tecnologiaIds.Contains(et.TecnologiaId))
                                                   && e.FlagActivo && !e.FlagExcluirCalculo.Value
                                                   && et.FlagActivo &&
                                                   et.DiaRegistro == DIA && et.MesRegistro == MES && et.AnioRegistro == ANIO
                                                   && (filtros.TipoEquipoFiltrar.Count == 0 || filtros.TipoEquipoFiltrar.Contains(e.TipoEquipoId.Value))
                                                   && (filtros.SubsidiariaFiltrar.Count == 0 || filtros.SubsidiariaFiltrar.Contains(e.DominioServidorId.Value.ToString()))
                                                   select et.TecnologiaId);

                        var equipoTecnologia = equipoTecnologiaTmp.ToList();
                        if (tecnologiaCicloVida != null && equipoTecnologia != null)
                        {
                            var listEstadoTecnologia = Utilitarios.EnumToList<EDashboardEstadoTecnologia>();
                            dashboardBase = new List<DashboardBase>();

                            List<int> tecnologiaCicloVidaTecIds = tecnologiaCicloVida.Select(x => x.TecnologiaId).ToList();

                            foreach (var itemEnum in listEstadoTecnologia)
                            {
                                List<CustomAutocomplete> data = new List<CustomAutocomplete>();
                                int _TipoId = 0;
                                string _TipoDescripcion = Utilitarios.GetEnumDescription2(itemEnum);
                                string _Color = Utilitarios.GetEnumDescription2((EDashboardEstadoTecnologiaColor)itemEnum);
                                switch (itemEnum)
                                {
                                    case EDashboardEstadoTecnologia.CercaFinSoporte:
                                        {
                                            _TipoId = (int)EDashboardEstadoTecnologia.CercaFinSoporte;
                                            data = tecnologiaCicloVida
                                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                                .Where(x => Utilitarios.GetMonthDifference(fechaActual, x.FechaCalculoBase) <= MESES_CERCA_FIN_SOPORTE && x.Obsoleto == 0 && !x.EsIndefinida)
                                                .Select(x => new CustomAutocomplete()
                                                {
                                                    Id = x.TecnologiaId.ToString(),
                                                    Descripcion = x.ClaveTecnologia,
                                                    TipoId = _TipoId.ToString(),
                                                    TipoDescripcion = _TipoDescripcion,
                                                    Color = _Color,
                                                    value = equipoTecnologia.Count(y => y == x.TecnologiaId).ToString(),
                                                    Total = equipoTecnologia.Count(y => y == x.TecnologiaId)
                                                }).ToList();

                                            data = (from a in data
                                                    join b in equipoTecnologia on a.Id equals b.ToString()
                                                    select a).Distinct().ToList();
                                        }
                                        break;
                                    case EDashboardEstadoTecnologia.NoVigente:
                                        {
                                            _TipoId = (int)EDashboardEstadoTecnologia.NoVigente;
                                            data = tecnologiaCicloVida
                                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                                .Where(x => x.Obsoleto == 1)
                                                .Select(x => new CustomAutocomplete()
                                                {
                                                    Id = x.TecnologiaId.ToString(),
                                                    Descripcion = x.ClaveTecnologia,
                                                    TipoId = _TipoId.ToString(),
                                                    TipoDescripcion = _TipoDescripcion,
                                                    Color = _Color,
                                                    value = equipoTecnologia.Count(y => y == x.TecnologiaId).ToString(),
                                                    Total = equipoTecnologia.Count(y => y == x.TecnologiaId)
                                                }).ToList();

                                            data = (from a in data
                                                    join b in equipoTecnologia on a.Id equals b.ToString()
                                                    select a).Distinct().ToList();
                                        }
                                        break;
                                    case EDashboardEstadoTecnologia.Vigente:
                                        {
                                            _TipoId = (int)EDashboardEstadoTecnologia.Vigente;
                                            data = tecnologiaCicloVida
                                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                                .Where(x => Utilitarios.GetMonthDifference(fechaActual, x.FechaCalculoBase) > MESES_CERCA_FIN_SOPORTE && x.Obsoleto == 0)
                                                .Select(x => new CustomAutocomplete()
                                                {
                                                    Id = x.TecnologiaId.ToString(),
                                                    Descripcion = x.ClaveTecnologia,
                                                    TipoId = _TipoId.ToString(),
                                                    TipoDescripcion = _TipoDescripcion,
                                                    Color = _Color,
                                                    value = equipoTecnologia.Count(y => y == x.TecnologiaId).ToString(),
                                                    Total = equipoTecnologia.Count(y => y == x.TecnologiaId)
                                                }).ToList();
                                            var data2 = tecnologiaCicloVida
                                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto, EsIndefinida = x.EsIndefinida })
                                                .Where(x => x.EsIndefinida)
                                                .Select(x => new CustomAutocomplete()
                                                {
                                                    Id = x.TecnologiaId.ToString(),
                                                    Descripcion = x.ClaveTecnologia,
                                                    TipoId = _TipoId.ToString(),
                                                    TipoDescripcion = _TipoDescripcion,
                                                    Color = _Color,
                                                    value = equipoTecnologia.Count(y => y == x.TecnologiaId).ToString(),
                                                    Total = equipoTecnologia.Count(y => y == x.TecnologiaId)
                                                }).ToList();
                                            data.AddRange(data2);

                                            data = (from a in data
                                                    join b in equipoTecnologia on a.Id equals b.ToString()
                                                    select a).Distinct().ToList();
                                            //data = data.Distinct().ToList();
                                        }
                                        break;
                                    case EDashboardEstadoTecnologia.SinInformacion:
                                        {
                                            _TipoId = (int)EDashboardEstadoTecnologia.SinInformacion;
                                            data = tecnologiaCicloVida
                                                .Join(tecnologia, x => x.TecnologiaId, y => y.TecnologiaId, (x, y) => new { TecnologiaId = x.TecnologiaId, ClaveTecnologia = y.ClaveTecnologia, FechaCalculoBase = x.FechaCalculoBase, Obsoleto = x.Obsoleto })
                                                .Where(x => !tecnologiaCicloVidaTecIds.Contains(x.TecnologiaId))
                                                .Select(x => new CustomAutocomplete()
                                                {
                                                    Id = x.TecnologiaId.ToString(),
                                                    Descripcion = x.ClaveTecnologia,
                                                    TipoId = _TipoId.ToString(),
                                                    TipoDescripcion = _TipoDescripcion,
                                                    Color = _Color,
                                                    value = equipoTecnologia.Count(y => y == x.TecnologiaId).ToString(),
                                                    Total = equipoTecnologia.Count(y => y == x.TecnologiaId)
                                                }).ToList();

                                            data = (from a in data
                                                    join b in equipoTecnologia on a.Id equals b.ToString()
                                                    select a).Distinct().ToList();
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                int _paramTop = int.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("TOP_REGISTROS_DASHBOARD_TECNOLOGIA").Valor);
                                int _Cantidad = data.Count;
                                data = data.Where(x => x.value != "0").ToList();
                                if (data.Count > _paramTop && !isExportar)
                                {
                                    // el sexto concatenar
                                    int cantidadOtros = 0;
                                    int indice = 0;

                                    data = data.OrderByDescending(x => x.Total).ToList();

                                    foreach (var item in data)
                                    {
                                        indice++;
                                        if (indice > _paramTop)
                                        {
                                            cantidadOtros += int.Parse(item.value);

                                        }
                                    }

                                    var nroTecnologiasOtros = data.Count - _paramTop;
                                    data = data.Take(_paramTop).ToList();
                                    data = data.OrderBy(x => x.Total).ToList();
                                    data.Insert(0, (new CustomAutocomplete()
                                    {
                                        //Descripcion = "Otras tecnologías" + cantidadOtros + "... " + _TipoDescripcion,
                                        Descripcion = string.Format("Otras {0} tecnologías ... {1}", nroTecnologiasOtros, _TipoDescripcion),
                                        TipoId = _TipoId.ToString(),
                                        TipoDescripcion = _TipoDescripcion,
                                        Color = _Color,
                                        value = cantidadOtros.ToString(),
                                        Total = cantidadOtros
                                    }));
                                }
                                else
                                {
                                    data = data.OrderBy(x => x.Total).ToList();
                                }

                                dashboardBase.Add(new DashboardBase()
                                {
                                    TipoId = _TipoId.ToString(),
                                    TipoDescripcion = _TipoDescripcion,
                                    Color = _Color,
                                    Data = data,
                                    Cantidad = _Cantidad
                                });
                            }
                        }
                    }
                    return dashboardBase;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
        }

        public override List<ReporteDetalladoSubdominioDto> GetReportEquipos(FiltrosDashboardTecnologia filtros)
        {
            try
            {
                try
                {
                    filtros.FechaFiltro = DateTime.ParseExact(filtros.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    filtros.FechaFiltro = DateTime.Now;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        int DIA = filtros.FechaFiltro.Day;
                        int MES = filtros.FechaFiltro.Month;
                        int ANIO = filtros.FechaFiltro.Year;

                        if (filtros.SubdominioFiltrar == null) filtros.SubdominioFiltrar = new List<int>();
                        if (filtros.DominioFiltrar == null) filtros.DominioFiltrar = new List<int>();
                        if (filtros.ClaveTecnologiaFiltrar == null) filtros.ClaveTecnologiaFiltrar = new List<string>();
                        if (filtros.FabricanteFiltrar == null) filtros.FabricanteFiltrar = new List<string>();
                        if (filtros.FamiliaFiltrar == null) filtros.FamiliaFiltrar = new List<int>();
                        if (filtros.TipoEquipoFiltrar == null) filtros.TipoEquipoFiltrar = new List<int>();
                        if (filtros.SubsidiariaFiltrar == null) filtros.SubsidiariaFiltrar = new List<string>();

                        //var tecnologia = (from t in ctx.Tecnologia
                        //                  join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                        //                  join d in ctx.Dominio on s.DominioId equals d.DominioId
                        //                  where t.Activo && s.Activo && d.Activo
                        //                  && (filtros.SubdominioFiltrar.Count == 0 || filtros.SubdominioFiltrar.Contains(s.SubdominioId))
                        //                  && (filtros.DominioFiltrar.Count == 0 || filtros.DominioFiltrar.Contains(d.DominioId))
                        //                  && (filtros.FabricanteFiltrar.Count == 0 || filtros.FabricanteFiltrar.Contains(t.Fabricante))
                        //                  && (filtros.ClaveTecnologiaFiltrar.Count == 0 || filtros.ClaveTecnologiaFiltrar.Contains(t.ClaveTecnologia))
                        //                  && (filtros.FamiliaFiltrar.Count == 0 || filtros.FamiliaFiltrar.Contains(t.FamiliaId.Value))

                        //                  select t).ToList();

                        //List<int> tecnologiaIds = tecnologia.Select(x => x.TecnologiaId).ToList();
                        //if (tecnologiaIds == null) tecnologiaIds = new List<int>();

                        var equipoTecnologia = (from et in ctx.EquipoTecnologia
                                                join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                                join d in ctx.TipoEquipo on e.TipoEquipoId equals d.TipoEquipoId
                                                join f in ctx.Tecnologia on et.TecnologiaId equals f.TecnologiaId
                                                join g in ctx.TecnologiaCicloVida on new { f.TecnologiaId, Anio = ANIO, Mes = MES, Dia = DIA } equals new { g.TecnologiaId, Anio = g.AnioRegistro, Mes = g.MesRegistro, Dia = g.DiaRegistro }
                                                join s in ctx.Subdominio on f.SubdominioId equals s.SubdominioId
                                                join r in ctx.Dominio on s.DominioId equals r.DominioId

                                                where e.FlagActivo && !e.FlagExcluirCalculo.Value && f.Activo && s.Activo && r.Activo
                                                && et.FlagActivo && et.DiaRegistro == DIA && et.MesRegistro == MES && et.AnioRegistro == ANIO
                                                && (filtros.SubdominioFiltrar.Count == 0 || filtros.SubdominioFiltrar.Contains(s.SubdominioId))
                                                && (filtros.DominioFiltrar.Count == 0 || filtros.DominioFiltrar.Contains(r.DominioId))
                                                && (filtros.FabricanteFiltrar.Count == 0 || filtros.FabricanteFiltrar.Contains(f.Fabricante))
                                                && (filtros.ClaveTecnologiaFiltrar.Count == 0 || filtros.ClaveTecnologiaFiltrar.Contains(f.ClaveTecnologia))
                                                && (filtros.FamiliaFiltrar.Count == 0 || filtros.FamiliaFiltrar.Contains(f.FamiliaId.Value))
                                                && (filtros.TipoEquipoFiltrar.Count == 0 || filtros.TipoEquipoFiltrar.Contains(e.TipoEquipoId.Value))
                                                && (filtros.SubsidiariaFiltrar.Count == 0 || filtros.SubsidiariaFiltrar.Contains(e.DominioServidorId.Value.ToString()))
                                                select new ReporteDetalladoSubdominioDto
                                                {
                                                    TipoEquipo = d.Nombre,
                                                    Nombre = e.Nombre,
                                                    Subdominio = s.Nombre,
                                                    Dominio = r.Nombre,
                                                    ClaveTecnologia = f.ClaveTecnologia,
                                                    FechaCalculoBase = g.FechaCalculoBase,
                                                    Obsoleto = g.Obsoleto,
                                                    FlagFechaFinSoporte = f.FlagFechaFinSoporte,
                                                    TipoTecnologiaId = f.TipoTecnologia.HasValue ? f.TipoTecnologia.Value : 0,
                                                    EstadoId = f.EstadoId,
                                                }).ToList();

                        return equipoTecnologia;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: DashboardBase GetReport(FiltrosDashboardTecnologia filtros)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiasXEstado(List<int> idEstados, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join s in ctx.Subdominio on new { u.SubdominioId, Activo = true } equals new { s.SubdominioId, Activo = s.Activo } into lj1
                                         from s in lj1.DefaultIfEmpty()
                                         join d in ctx.Dominio on new { s.DominioId, Activo = true } equals new { d.DominioId, Activo = d.Activo } into lj2
                                         from d in lj1.DefaultIfEmpty()
                                         where u.Activo
                                         && idEstados.Contains(u.EstadoTecnologia)
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Dominio = d == null ? "-" : d.Nombre,
                                             Subdominio = d == null ? "-" : s.Nombre,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             Activo = u.Activo
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasXEstado(List<int> idEstados, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasXEstado(List<int> idEstados, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiasSinEquivalencia(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var fechaActual = new DateTime().Date;

                        var registros = (from u in ctx.Tecnologia
                                         join s in ctx.Subdominio on new { u.SubdominioId, Activo = true } equals new { s.SubdominioId, Activo = s.Activo } into lj1
                                         from s in lj1.DefaultIfEmpty()
                                         join d in ctx.Dominio on new { s.DominioId, Activo = true } equals new { d.DominioId, Activo = d.Activo } into lj2
                                         from d in lj1.DefaultIfEmpty()
                                         join f in ctx.Familia on new { FamiliaId = u.FamiliaId.Value, Activo = true } equals new { f.FamiliaId, Activo = f.Activo } into lj3
                                         from f in lj3.DefaultIfEmpty()
                                         where u.Activo
                                         && !ctx.TecnologiaEquivalencia.Any(gi => gi.TecnologiaId == u.TecnologiaId)
                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Familia = f == null ? "-" : f.Nombre,
                                             Dominio = d == null ? "-" : d.Nombre,
                                             Subdominio = d == null ? "-" : s.Nombre,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             Activo = u.Activo,
                                             NroInstancias = (from et in ctx.EquipoTecnologia
                                                              where et.FlagActivo && et.TecnologiaId == u.TecnologiaId
                                                              && et.DiaRegistro == fechaActual.Day && et.MesRegistro == fechaActual.Month && et.AnioRegistro == fechaActual.Year
                                                              select 1).Count()
                                             + (from ex in ctx.Excepcion where ex.FlagActivo && ex.TecnologiaId == u.TecnologiaId select 1).Count()
                                             + (from rd in ctx.RelacionDetalle
                                                join r in ctx.Relacion on rd.RelacionId equals r.RelacionId
                                                where rd.FlagActivo && r.FlagActivo && rd.TecnologiaId == u.TecnologiaId
                                                && r.DiaRegistro == fechaActual.Day && r.MesRegistro == fechaActual.Month && r.AnioRegistro == fechaActual.Year
                                                select 1).Count()
                                             + (from tv in ctx.TecnologiaVinculada
                                                join t in ctx.Tecnologia on tv.TecnologiaId equals u.TecnologiaId
                                                where tv.Activo && tv.VinculoId == u.TecnologiaId && t.Activo
                                                select 1).Count()
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasSinEquivalencia(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasSinEquivalencia(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiasSinFechasSoporte(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId into lj1
                                         from s in lj1.DefaultIfEmpty()
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId into lj2
                                         from d in lj1.DefaultIfEmpty()
                                         join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into lj3
                                         from f in lj3.DefaultIfEmpty()
                                         where u.Activo && s.Activo && d.Activo
                                         && u.FlagFechaFinSoporte == true
                                         && !u.FechaFinSoporte.HasValue
                                         && !u.FechaAcordada.HasValue
                                         && !u.FechaExtendida.HasValue

                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Familia = f == null ? "-" : f.Nombre,
                                             Dominio = d == null ? "-" : d.Nombre,
                                             Subdominio = d == null ? "-" : s.Nombre,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             Activo = u.Activo
                                         });

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasSinFechasSoporte(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasSinFechasSoporte(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecnologiaByEquipoId(int equipoId, string fecha, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    if (string.IsNullOrEmpty(fecha)) fecha = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime fechaFiltro = DateTime.Now;
                    try
                    {
                        fechaFiltro = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        fechaFiltro = DateTime.Now;
                    }

                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_list_by_equipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@equipoId", ((object)equipoId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@anioRegistro", ((object)fechaFiltro.Year) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@mesRegistro", ((object)fechaFiltro.Month) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@diaRegistro", ((object)fechaFiltro.Day) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageNumber", ((object)pageNumber) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageSize", ((object)pageSize) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@sortName", ((object)sortName) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@sortOrder", ((object)sortOrder) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaDTO()
                            {
                                Id = reader.GetData<int>("Id"),
                                ClaveTecnologia = reader.GetData<string>("ClaveTecnologia"),
                                DominioNomb = reader.GetData<string>("DominioNomb"),
                                SubdominioNomb = reader.GetData<string>("SubdominioNomb"),
                                FechaCalculoBase = reader.GetData<DateTime?>("FechaCalculoBase"),
                                Obsoleto = reader.GetData<bool>("Obsoleto"),
                                ObsolescenciaTecnologia = reader.GetData<decimal?>("ObsolescenciaTecnologia"),
                                ObsolescenciaTecnologiaProyectado1 = reader.GetData<decimal?>("ObsolescenciaTecnologiaProyectado1"),
                                ObsolescenciaTecnologiaProyectado2 = reader.GetData<decimal?>("ObsolescenciaTecnologiaProyectado2"),
                                Meses = reader.GetData<int>("Meses"),
                                IndicadorMeses1 = reader.GetData<int>("IndicadorMeses1"),
                                IndicadorMeses2 = reader.GetData<int>("IndicadorMeses2"),
                                FlagFechaFinSoporte = reader.GetData<bool?>("FlagFechaFinSoporte"),
                                TipoTecnologiaId = reader.GetData<int>("TipoTecnologiaId"),
                                TipoTecNomb = reader.GetData<string>("TipoTecNomb"),
                                CantidadVulnerabilidades = reader.GetData<int>("CantidadVulnerabilidades"),
                                MesesObsolescencia = reader.GetData<int>("Meses"),
                                EstadoId = reader.GetData<int?>("EstadoTecnologia"),
                            };

                            totalRows = reader.GetData<int>("TotalRows");

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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }

            //try
            //{
            //    var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
            //    var proyeccionMeses1 = int.Parse(paramProyeccion1 != null ? paramProyeccion1.Valor : "12");
            //    var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
            //    var proyeccionMeses2 = int.Parse(paramProyeccion2 != null ? paramProyeccion2.Valor : "24");

            //    using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //    {
            //        using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            //        {
            //            if (string.IsNullOrEmpty(fecha)) fecha = DateTime.Now.ToString("dd/MM/yyyy");

            //            DateTime fechaFiltro = DateTime.Now;
            //            try
            //            {
            //                fechaFiltro = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //            }
            //            catch (Exception)
            //            {
            //                fechaFiltro = DateTime.Now;
            //            }

            //            int DIA_REGISTRO = fechaFiltro.Day;
            //            int MES_REGISTRO = fechaFiltro.Month;
            //            int ANIO_REGISTRO = fechaFiltro.Year;
            //            var registros = (from et in ctx.EquipoTecnologia
            //                             join t in ctx.Tecnologia on et.TecnologiaId equals t.TecnologiaId
            //                             join tt in ctx.Tipo on t.TipoTecnologia equals tt.TipoId
            //                             join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
            //                             join d in ctx.Dominio on s.DominioId equals d.DominioId
            //                             join tcv in ctx.TecnologiaCicloVida on new { TecnologiaId = t.TecnologiaId, Anio = ANIO_REGISTRO, Mes = MES_REGISTRO, Dia = DIA_REGISTRO } equals new { TecnologiaId = tcv.TecnologiaId, Anio = tcv.AnioRegistro, Mes = tcv.MesRegistro, Dia = tcv.DiaRegistro }
            //                             where et.EquipoId == equipoId && et.FlagActivo && t.Activo
            //                             //&& tcv.AnioRegistro == ANIO_REGISTRO && tcv.MesRegistro == MES_REGISTRO && tcv.DiaRegistro == DIA_REGISTRO
            //                             && et.AnioRegistro == ANIO_REGISTRO && et.MesRegistro == MES_REGISTRO && et.DiaRegistro == DIA_REGISTRO
            //                             && s.Activo && d.Activo
            //                             //group new { et, t, s, d, tcv } by new { t.TecnologiaId, t.ClaveTecnologia, Dominio = d.Nombre, Subdominio = s.Nombre, tcv.FechaCalculoBase, tcv.Obsoleto } into grp
            //                             select new TecnologiaDTO()
            //                             {
            //                                 Id = t.TecnologiaId,
            //                                 ClaveTecnologia = t.ClaveTecnologia,
            //                                 DominioNomb = d.Nombre,
            //                                 SubdominioNomb = s.Nombre,
            //                                 FechaCalculoBase = tcv.FechaCalculoBase,
            //                                 Obsoleto = tcv.Obsoleto == 1,
            //                                 ObsolescenciaTecnologia = tcv.IndiceObsolescencia,
            //                                 ObsolescenciaTecnologiaProyectado1 = tcv.IndiceObsolescenciaProyectado1,
            //                                 ObsolescenciaTecnologiaProyectado2 = tcv.IndiceObsolescenciaProyectado2,
            //                                 Meses = proyeccionMeses1,
            //                                 IndicadorMeses1 = proyeccionMeses1,
            //                                 IndicadorMeses2 = proyeccionMeses2,
            //                                 FlagFechaFinSoporte = t.FlagFechaFinSoporte,
            //                                 TipoTecnologiaId = t.TipoTecnologia,
            //                                 TipoTecNomb = tt.Nombre,
            //                                 //CantidadVulnerabilidades = (from a in ctx.QualyTecnologia
            //                                 //                            join b in ctx.).Count()
            //                             }).OrderBy(sortName + " " + sortOrder);

            //            totalRows = registros.Count();
            //            var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            //            return resultado.ToList();
            //        }
            //    }
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    HelperLog.ErrorEntity(ex);
            //    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
            //        , "Error en el metodo: List<TecnologiaDTO> GetTecnologiaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
            //        , new object[] { null });
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
            //        , "Error en el metodo: List<TecnologiaDTO> GetTecnologiaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
            //        , new object[] { null });
            //}
        }

        public override List<CustomAutocomplete> GetSistemasOperativoByFiltro(string filtro)
        {
            try
            {
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro(Utilitarios.CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO);
                var idSubdominio = parametro != null ? int.Parse(parametro.Valor) : 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo && u.SubdominioId == idSubdominio
                                       && u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetSistemasOperativoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetSistemasOperativoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
        #endregion
        public override List<TecnologiaDTO> GetTecnologiasXAplicacionByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var mesesCicloVida = this.GetMesesObsolescencia();
                var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
                var proyeccionMeses1 = int.Parse(paramProyeccion1 != null ? paramProyeccion1.Valor : "12");
                var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
                var proyeccionMeses2 = int.Parse(paramProyeccion2 != null ? paramProyeccion2.Valor : "24");
                var relacionesActivas = new List<int>() {
                    (int)EEstadoRelacion.Aprobado,
                    (int)EEstadoRelacion.PendienteEliminacion
                };

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime fechaConsulta = DateTime.Now;
                        //Servidor Relacionado

                        var ANIO = DateTime.Now.Year;
                        var MES = DateTime.Now.Month;
                        var DIA = DateTime.Now.Day;

                        var registros = (from r in ctx.Relacion
                                         join rd in ctx.RelacionDetalle on r.RelacionId equals rd.RelacionId
                                         join t in ctx.Tecnologia on rd.TecnologiaId equals t.TecnologiaId
                                         join tt in ctx.Tipo on t.TipoTecnologia equals tt.TipoId
                                         join tcv in ctx.TecnologiaCicloVida on new { TecnologiaId = t.TecnologiaId, Anio = ANIO, Mes = MES, Dia = DIA } equals new { TecnologiaId = tcv.TecnologiaId, Anio = tcv.AnioRegistro, Mes = tcv.MesRegistro, Dia = tcv.DiaRegistro }
                                         join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join p in ctx.Producto on t.ProductoId equals p.ProductoId into ProductoTecnologia
                                         from pt in ProductoTecnologia.DefaultIfEmpty()
                                         join tipo in ctx.TipoCicloVida on pt.TipoCicloVidaId equals tipo.TipoCicloVidaId into TipoCicloVidaProducto
                                         from tcvp in TipoCicloVidaProducto.DefaultIfEmpty()
                                         where r.CodigoAPT == codigoAPT
                                         && r.FlagActivo && rd.FlagActivo && t.Activo && s.Activo && d.Activo
                                         && r.AnioRegistro == fechaConsulta.Year
                                         && r.MesRegistro == fechaConsulta.Month
                                         && r.DiaRegistro == fechaConsulta.Day
                                         && relacionesActivas.Contains(r.EstadoId)
                                         select new TecnologiaDTO()
                                         {
                                             Id = t.TecnologiaId,
                                             DominioNomb = d.Nombre,
                                             SubdominioNomb = s.Nombre,
                                             ClaveTecnologia = t.ClaveTecnologia,
                                             ObsolescenciaTecnologia = tcv.IndiceObsolescencia,
                                             ObsolescenciaTecnologiaProyectado1 = tcv.IndiceObsolescenciaProyectado1,
                                             ObsolescenciaTecnologiaProyectado2 = tcv.IndiceObsolescenciaProyectado2,
                                             Obsoleto = (tcv.Obsoleto == 1),
                                             FechaCalculoBase = tcv.FechaCalculoBase,
                                             MesesObsolescencia = (tcvp != null ? (tcvp.NroPeriodosEstadoAmbar == 0 ? mesesCicloVida : tcvp.NroPeriodosEstadoAmbar) : mesesCicloVida),
                                             Meses = proyeccionMeses1,
                                             IndicadorMeses1 = proyeccionMeses1,
                                             IndicadorMeses2 = proyeccionMeses2,
                                             FlagFechaFinSoporte = t.FlagFechaFinSoporte,
                                             TipoTecNomb = tt.Nombre,
                                             TipoTecnologiaId = t.TipoTecnologia,
                                             EstadoId = t.EstadoId
                                         }).Distinct().OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        foreach (var item in registros)
                        {

                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecnologiasXAplicacionByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecnologiasXAplicacionByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override FiltrosDashboardTecnologia ListFiltrosDashboardXDominio(List<int> idsDominio)
        {
            try
            {
                FiltrosDashboardTecnologia arr_data = null;
                var arrIdsDominios = idsDominio.ToArray();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosDashboardTecnologia();

                        //arr_data.Familia = (from t in ctx.Tecnologia
                        //                    join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                        //                    join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                        //                    where t.Activo
                        //                    && f.Activo
                        //                    && arrIdsDominios.Contains(t.SubdominioId)
                        //                    select new CustomAutocomplete()
                        //                    {
                        //                        Id = f.FamiliaId.ToString(),
                        //                        Descripcion = f.Nombre,
                        //                        TipoId = s.SubdominioId.ToString(),
                        //                        TipoDescripcion = s.Nombre
                        //                    }).Distinct().OrderBy(z => z.Descripcion).ToList();


                        arr_data.Subdominio = (from t in ctx.Tecnologia
                                                   //join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                                               join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                               join d in ctx.Dominio on s.DominioId equals d.DominioId
                                               where t.Activo
                                               && s.Activo
                                               && s.FlagIsVisible.Value
                                               && d.Activo
                                               && arrIdsDominios.Contains(d.DominioId)
                                               select new CustomAutocomplete()
                                               {
                                                   Id = s.SubdominioId.ToString(),
                                                   Descripcion = s.Nombre
                                                   //TipoId = s.SubdominioId.ToString(),
                                                   //TipoDescripcion = s.Nombre
                                               }).Distinct().OrderBy(z => z.Descripcion).ToList();

                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiaVinculadaXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join tv in ctx.TecnologiaVinculada on u.TecnologiaId equals tv.VinculoId
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         where tv.Activo && tv.TecnologiaId == tecnologiaId
                                         select new TecnologiaG()
                                         {
                                             Id = tv.VinculoId,
                                             Nombre = u.Nombre,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             Dominio = s.Nombre,
                                             Subdominio = d.Nombre
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaVinculadaXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaVinculadaXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecnologiaEstandar(string _subdominioIds = null, string _dominiosId = null)
        {
            try
            {
                var subdominioIntIds = new List<int>();
                var dominiosIntIds = new List<int>();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tecnologia = (from t in ctx.Tecnologia
                                          join t2 in ctx.Tipo on t.TipoTecnologia equals t2.TipoId
                                          where t.Activo && t.FlagSiteEstandar.Value
                                          select new TecnologiaBase()
                                          {
                                              Id = t.TecnologiaId,
                                              Nombre = t.ClaveTecnologia,
                                              SubdominioId = t.SubdominioId,
                                              EstadoId = t.EstadoId.Value,
                                              CodigoTecnologia = t.CodigoTecnologiaAsignado,
                                              Url = t.UrlConfluence,
                                              FlagFechaFinSoporte = t.FlagFechaFinSoporte,
                                              FechaCalculoTec = t.FechaCalculoTec,
                                              FechaExtendida = t.FechaExtendida,
                                              FechaFinSoporte = t.FechaFinSoporte,
                                              FechaAcordada = t.FechaAcordada,
                                              TipoTecnologia = t.TipoTecnologia,
                                              TipoTecnologiaToString = t2.Nombre
                                          }).ToList();

                        var dominio = (from d in ctx.Dominio
                                       where d.Activo
                                       select d).ToList();

                        var subdominio = (from s in ctx.Subdominio
                                          where s.Activo
                                          select s).ToList();

                        var tipotecnologia = (from t in ctx.Tipo
                                              where t.Activo
                                              select t).ToList();

                        //foreach(var item in tecnologia)
                        //{
                        //    if (item.TipoTecnologia == 4)
                        //        item.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                        //    else
                        //    {
                        //        if (item.FlagFechaFinSoporte.HasValue)
                        //        {
                        //            if (!item.FlagFechaFinSoporte.Value)
                        //                item.EstadoId = (int)ETecnologiaEstado.Vigente;                                    
                        //            else
                        //            {
                        //                DateTime? fechaFinSoporte = null;
                        //                if (!item.FechaCalculoTec.HasValue)
                        //                {
                        //                    item.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                        //                }
                        //                else
                        //                {
                        //                    switch (item.FechaCalculoTec.Value)
                        //                    {
                        //                        case (int)FechaCalculoTecnologia.FechaInterna:
                        //                            fechaFinSoporte = item.FechaFinSoporte;
                        //                            break;
                        //                        case (int)FechaCalculoTecnologia.FechaExtendida:
                        //                            fechaFinSoporte = item.FechaExtendida;
                        //                            break;
                        //                        case (int)FechaCalculoTecnologia.FechaFinSoporte:
                        //                            fechaFinSoporte = item.FechaFinSoporte;
                        //                            break;
                        //                    }
                        //                    if (fechaFinSoporte != null)
                        //                    {
                        //                        if (fechaFinSoporte.HasValue)
                        //                        {
                        //                            if (fechaFinSoporte < DateTime.Now)
                        //                                item.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                        //                            else
                        //                            {
                        //                                if (fechaFinSoporte > DateTime.Now && fechaFinSoporte <= DateTime.Now.AddMonths(12))
                        //                                    item.EstadoId = (int)ETecnologiaEstado.Deprecado;
                        //                                else
                        //                                    item.EstadoId = (int)ETecnologiaEstado.Vigente;
                        //                            }
                        //                        }
                        //                        else
                        //                            item.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                        //                    }
                        //                    else
                        //                        item.EstadoId = (int)ETecnologiaEstado.Obsoleto;
                        //                }
                        //            }
                        //        }
                        //        else
                        //            item.EstadoId = (int)ETecnologiaEstado.Obsoleto;

                        //    }
                        //}
                        if (!string.IsNullOrEmpty(_subdominioIds))
                            subdominioIntIds = _subdominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                        if (!string.IsNullOrEmpty(_dominiosId))
                            dominiosIntIds = _dominiosId.Split('|').Select(x => int.Parse(x)).ToList();

                        var registros = (from d in dominio
                                         join s in subdominio on d.DominioId equals s.DominioId
                                         join t in tecnologia on s.SubdominioId equals t.SubdominioId
                                         where (string.IsNullOrEmpty(_dominiosId) || dominiosIntIds.Contains(s.DominioId)) &&
                                         (string.IsNullOrEmpty(_subdominioIds) || subdominioIntIds.Contains(s.SubdominioId))
                                         group new { d, s, t } by new { DomId = d.DominioId, Dominio = d.Nombre, SubId = s.SubdominioId, Subdomino = s.Nombre } into grp
                                         orderby grp.Key.Dominio
                                         select grp).ToList().Select(grp =>
                                         new TecnologiaDTO()
                                         {
                                             DominioNomb = grp.Key.Dominio,
                                             SubdominioId = grp.Key.SubId,
                                             SubdominioNomb = grp.Key.Subdomino,
                                             TecnologiaVigenteStr = string.Join("</br>",
                                                 tecnologia
                                                 .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                                 .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Vigente).ToArray()
                                                 .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "vigenteClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                             //TecnologiaDeprecadoStr = string.Join("</br>",
                                             //    tecnologia
                                             //    .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                             //    .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.VigentePorVencer).ToArray()
                                             //    .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "deprecadoClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                             TecnologiaDeprecadoStr = string.Join("</br>",
                                                 tecnologia
                                                 .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                                 .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Deprecado).ToArray()
                                                 .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "deprecadoClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                             TecnologiaObsoletoStr = string.Join("</br>",
                                                 tecnologia
                                                 .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                                 .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Obsoleto).ToArray()
                                                 .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "obsoletoClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaEstandar()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaEstandar()"
                    , new object[] { null });
            }
        }


        public override DashboardTecnologiaEquipoData GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros)
        {
            try
            {
                DashboardTecnologiaEquipoData dashboardBase = new DashboardTecnologiaEquipoData();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        int DIA = DateTime.Now.Day;
                        int MES = DateTime.Now.Month;
                        int ANIO = DateTime.Now.Year;

                        if (filtros.FechaConsulta.HasValue)
                        {
                            DIA = filtros.FechaConsulta.Value.Day;
                            MES = filtros.FechaConsulta.Value.Month;
                            ANIO = filtros.FechaConsulta.Value.Year;
                        }

                        #region DATOS TECNOLOGIA

                        var objTecnologia = (from a in ctx.Tecnologia
                                             join p in ctx.Producto on a.ProductoId equals p.ProductoId
                                             join s in ctx.Subdominio on p.SubDominioId equals s.SubdominioId
                                             join d in ctx.Dominio on s.DominioId equals d.DominioId
                                             join t in ctx.TecnologiaCicloVida on a.TecnologiaId equals t.TecnologiaId
                                             join tt in ctx.Tipo on a.TipoTecnologia equals tt.TipoId into gj
                                             from x in gj.DefaultIfEmpty()
                                                 //where a.ClaveTecnologia.ToUpper() == filtros.ClaveTecnologiaFiltrar.ToUpper()
                                             where a.TecnologiaId == filtros.TecnologiaIdFiltrar
                                             && a.Activo
                                             && t.AnioRegistro == ANIO && t.MesRegistro == MES && t.DiaRegistro == DIA
                                             select new
                                             {
                                                 Id = a.TecnologiaId,
                                                 ClaveTecnologia = a.ClaveTecnologia,
                                                 DominioNomb = d.Nombre,
                                                 SubdominioNomb = s.Nombre,
                                                 TipoTecnologiaId = a.TipoTecnologia,
                                                 FechaAcordada = a.FechaAcordada,
                                                 FechaExtendida = a.FechaExtendida,
                                                 FechaFinSoporte = a.FechaFinSoporte,
                                                 FechaCalculoTec = a.FechaCalculoTec,
                                                 IndiceObsolescencia = t.IndiceObsolescencia,
                                                 Riesgo = t.Riesgo,
                                                 t.Obsoleto,
                                                 a.FlagFechaFinSoporte,
                                                 TipoTecnologia = x.Nombre,
                                                 EstadoId = a.EstadoId,
                                             }).ToList().Select(x => new TecnologiaDTO
                                             {
                                                 Meses = int.Parse(parametroMeses),
                                                 IndicadorMeses1 = int.Parse(parametroMeses),
                                                 IndicadorMeses2 = int.Parse(parametroMeses2),
                                                 Id = x.Id,
                                                 ClaveTecnologia = x.ClaveTecnologia,
                                                 DominioNomb = x.DominioNomb,
                                                 SubdominioNomb = x.SubdominioNomb,
                                                 TipoTecnologiaId = x.TipoTecnologiaId,
                                                 RiesgoTecnologia = x.Riesgo,
                                                 ObsolescenciaTecnologia = x.IndiceObsolescencia,
                                                 TipoTecNomb = x.TipoTecnologia,
                                                 Obsoleto = (x.Obsoleto == 1),
                                                 FlagFechaFinSoporte = x.FlagFechaFinSoporte,
                                                 EstadoId = x.EstadoId,
                                                 FechaCalculoBase = x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida ? x.FechaExtendida
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte ? x.FechaFinSoporte
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna ? x.FechaAcordada : null


                                             }).FirstOrDefault();

                        #endregion

                        #region DATA TIPO EQUIPO
                        var dataTipoEquipo = (from a in ctx.EquipoTecnologia
                                              join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                              join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                                              join d in ctx.TipoEquipo on c.TipoEquipoId equals d.TipoEquipoId
                                              where b.TecnologiaId == filtros.TecnologiaIdFiltrar
                                              && a.AnioRegistro == ANIO
                                              && a.MesRegistro == MES
                                              && a.DiaRegistro == DIA
                                              && b.Activo && a.FlagActivo
                                              group a by new { d.TipoEquipoId, d.Nombre } into grp
                                              select new CustomAutocomplete
                                              {
                                                  Id = grp.Key.TipoEquipoId.ToString(),
                                                  Descripcion = grp.Key.Nombre,
                                                  Total = grp.Count()
                                              }).ToList();

                        #endregion



                        dashboardBase.Tecnologia = objTecnologia;
                        dashboardBase.DataPie = dataTipoEquipo;


                        return dashboardBase;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros)"
                    , new object[] { null });
            }
        }


        public override List<EquipoDTO> GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                                         join d in ctx.TipoEquipo on c.TipoEquipoId equals d.TipoEquipoId
                                         join e in ctx.Ambiente on c.AmbienteId equals e.AmbienteId
                                         join f in ctx.DominioServidor on c.DominioServidorId equals f.DominioId
                                         where b.TecnologiaId == filtros.TecnologiaIdFiltrar
                                         && a.AnioRegistro == ANIO
                                         && a.MesRegistro == MES
                                         && a.DiaRegistro == DIA
                                         && b.Activo && a.FlagActivo
                                         select new EquipoDTO
                                         {
                                             Nombre = c.Nombre,
                                             TipoEquipo = d.Nombre,
                                             FlagTemporal = c.FlagTemporal,
                                             Ambiente = e.DetalleAmbiente,
                                             Subsidiaria = f.Nombre
                                         });

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.RelacionDetalle
                                         join b in ctx.Relacion on a.RelacionId equals b.RelacionId
                                         join e in ctx.Equipo on b.EquipoId equals e.EquipoId into lj1
                                         from e in lj1.DefaultIfEmpty()
                                         join c in ctx.Aplicacion on b.CodigoAPT equals c.CodigoAPT
                                         join d in ctx.AplicacionConfiguracion on c.AplicacionId equals d.AplicacionId
                                         where a.TecnologiaId == filtros.TecnologiaIdFiltrar
                                         && b.AnioRegistro == ANIO
                                         && b.MesRegistro == MES
                                         && b.DiaRegistro == DIA
                                         && d.AnioRegistro == ANIO
                                         && d.MesRegistro == MES
                                         && d.DiaRegistro == DIA
                                         select new AplicacionDTO
                                         {
                                             Nombre = c.Nombre,
                                             CodigoAPT = c.CodigoAPT,
                                             EstadoAplicacion = c.EstadoAplicacion,
                                             KPIObsolescencia = d.IndiceObsolescencia,
                                             Equipo = e.Nombre,
                                             EstadoRelacionId = b.EstadoId,
                                             IndiceObsolescencia_FLooking = d.IndiceObsolescencia_ForwardLooking,
                                         }).Distinct();

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<TecnologiaDTO> GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.TecnologiaVinculada
                                         join b in ctx.Tecnologia on a.VinculoId equals b.TecnologiaId
                                         join p in ctx.Producto on b.ProductoId equals p.ProductoId
                                         join s in ctx.Subdominio on p.SubDominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join t in ctx.Tipo on b.TipoTecnologia equals t.TipoId into x
                                         from y in x.DefaultIfEmpty()
                                         where a.TecnologiaId == filtros.TecnologiaIdFiltrar
                                         && a.Activo
                                         select new TecnologiaDTO
                                         {

                                             ClaveTecnologia = b.ClaveTecnologia,
                                             DominioNomb = d.Nombre,
                                             SubdominioNomb = d.Nombre,
                                             TipoTecnologiaId = b.TipoTecnologia,
                                             FechaAcordada = b.FechaAcordada,
                                             FechaExtendida = b.FechaExtendida,
                                             FechaFinSoporte = b.FechaFinSoporte,
                                             FechaCalculoTec = b.FechaCalculoTec,
                                             TipoTecNomb = y.Nombre,
                                             EstadoId = b.EstadoId,
                                         });

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        resultado = resultado.Select(x => new TecnologiaDTO
                        {

                            ClaveTecnologia = x.ClaveTecnologia,
                            DominioNomb = x.DominioNomb,
                            SubdominioNomb = x.SubdominioNomb,
                            TipoTecnologiaId = x.TipoTecnologiaId,
                            TipoTecNomb = x.TipoTecNomb,
                            FechaCalculoBase = x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida ? x.FechaExtendida
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte ? x.FechaFinSoporte
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna ? x.FechaAcordada : null,
                            EstadoId = x.EstadoId

                        }).ToList();


                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool MigrarEquivalenciasTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.TecnologiaEquivalencia.Where(x => x.TecnologiaId == TecnologiaEmisorId && x.FlagActivo).ToList().ForEach(x => x.TecnologiaId = TecnologiaReceptorId);
                    ctx.SaveChanges();
                    var retorno = ServiceManager<TecnologiaDAO>.Provider.CambiarEstado(TecnologiaEmisorId, false, Usuario);

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetReporteTecnologia(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, string dueno, string equipo, List<int> tipoTecIds, List<int> estObsIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                //casoUso = "";
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var tecnologiaIds = new List<int>();

                        if (!string.IsNullOrEmpty(equipo))
                        {
                            tecnologiaIds = (from et in ctx.EquipoTecnologia
                                             join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                             where et.FlagActivo && e.FlagActivo
                                             && (e.Nombre.ToUpper().Contains(equipo.ToUpper())
                                             /*|| string.IsNullOrEmpty(equipo)*/)
                                             select et.TecnologiaId).ToList();
                        }

                        var tecEquivalenciaIds = (from e in ctx.TecnologiaEquivalencia
                                                  where e.FlagActivo && e.Nombre.ToUpper().Contains(filtro.ToUpper())
                                                  select e.TecnologiaId
                                                ).ToList();

                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into lj2
                                         from f in lj2.DefaultIfEmpty()
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro)
                                         || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                         || tecEquivalenciaIds.Contains(u.TecnologiaId))
                                         && (u.Activo == (flagActivo != null))

                                         //&& (domId == -1 || s.DominioId == domId)
                                         //&& (subdomId == -1 || u.SubdominioId == subdomId)
                                         //&& (estadoId == -1 || u.EstadoTecnologia == estadoId)


                                         && (domIds.Count == 0 || domIds.Contains(s.DominioId))
                                         && (subdomIds.Count == 0 || subdomIds.Contains(u.SubdominioId))
                                         && (estadoIds.Count == 0 || estadoIds.Contains(u.EstadoTecnologia))
                                         && (estObsIds.Count == 0 || estObsIds.Contains(u.EstadoId.HasValue ? u.EstadoId.Value : 0))
                                         && (tipoTecIds.Count == 0 || tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))

                                         && (string.IsNullOrEmpty(famId) || f == null || f.Nombre.ToUpper().Equals(famId.ToUpper()))
                                         && (fecId == -1 || u.FechaFinSoporte.HasValue == (fecId == 1))
                                         && (u.Aplica.ToUpper().Contains(aplica.ToUpper()) || string.IsNullOrEmpty(aplica))
                                         && (u.CodigoTecnologiaAsignado.ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))
                                         && (u.DuenoId.ToUpper().Contains(dueno.ToUpper()) || string.IsNullOrEmpty(dueno))
                                         && (string.IsNullOrEmpty(equipo) || tecnologiaIds.Contains(u.TecnologiaId))

                                         //&& (estObsId == -1 || u.EstadoId == estObsId)
                                         //&& (tipoTecId == -1 || u.TipoTecnologia == tipoTecId)

                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Tipo = t.Nombre,
                                             Familia = f.Nombre,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             FechaModificacion = u.FechaModificacion,
                                             Estado = u.EstadoTecnologia,
                                             Versiones = u.Versiones,
                                             FechaLanzamiento = u.FechaLanzamiento,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FechaAcordada = u.FechaAcordada,
                                             FechaExtendida = u.FechaExtendida,
                                             ComentariosFechaFin = u.ComentariosFechaFin,
                                             Existencia = u.Existencia,
                                             Facilidad = u.Facilidad,
                                             Riesgo = u.Riesgo,
                                             Vulnerabilidad = u.Vulnerabilidad,
                                             CasoUso = u.CasoUso,
                                             Requisitos = u.Requisitos,
                                             Compatibilidad = u.Compatibilidad,
                                             Aplica = u.Aplica,
                                             EliminacionTecObsoleta = u.EliminacionTecObsoleta,
                                             Referencias = u.Referencias,
                                             PlanTransConocimiento = u.PlanTransConocimiento,
                                             EsqMonitoreo = u.EsqMonitoreo,
                                             LineaBaseSeg = u.LineaBaseSeg,
                                             EsqPatchManagement = u.EsqPatchManagement,
                                             Dueno = u.DuenoId,
                                             EqAdmContacto = u.EqAdmContacto,
                                             GrupoSoporteRemedy = u.GrupoSoporteRemedy,
                                             ConfArqSeg = u.ConfArqSegId,
                                             ConfArqTec = u.ConfArqTecId,
                                             EncargRenContractual = u.EncargRenContractual,
                                             EsqLicenciamiento = u.EsqLicenciamiento,
                                             SoporteEmpresarial = u.SoporteEmpresarial,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                             FechaAprobacion = u.FechaAprobacion,
                                             UsuarioAprobacion = u.UsuarioAprobacion,
                                             Observacion = u.Observacion,
                                             FlagAplicacion = u.FlagAplicacion,
                                             CodigoAPT = u.CodigoAPT,
                                             Fuente = u.FuenteId,
                                             FlagSiteEstandar = u.FlagSiteEstandar,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             Fabricante = u.Fabricante,
                                             EstadoId = u.EstadoId,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             CodigoTecnologiaAsignado = u.CodigoTecnologiaAsignado,
                                             RoadmapOpcional = u.RoadmapOpcional,
                                             FlagConfirmarFamilia = u.FlagConfirmarFamilia
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool MigrarDataTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var retorno = false;

                    var TEC_PROVEEDOR = (from u in ctx.Tecnologia
                                         where u.TecnologiaId == TecnologiaEmisorId
                                         select u).FirstOrDefault();
                    var TEC_RECEPTOR = (from u in ctx.Tecnologia
                                        where u.TecnologiaId == TecnologiaReceptorId
                                        select u).FirstOrDefault();

                    if (TEC_PROVEEDOR != null && TEC_RECEPTOR != null)
                    {
                        //TAB 1
                        TEC_RECEPTOR.Descripcion = TEC_PROVEEDOR.Descripcion;
                        TEC_RECEPTOR.ComentariosFechaFin = TEC_PROVEEDOR.ComentariosFechaFin;
                        TEC_RECEPTOR.Existencia = TEC_PROVEEDOR.Existencia;
                        TEC_RECEPTOR.Facilidad = TEC_PROVEEDOR.Facilidad;
                        TEC_RECEPTOR.Riesgo = TEC_PROVEEDOR.Riesgo;
                        TEC_RECEPTOR.Vulnerabilidad = TEC_PROVEEDOR.Vulnerabilidad;
                        TEC_RECEPTOR.CasoUso = TEC_PROVEEDOR.CasoUso;
                        TEC_RECEPTOR.Requisitos = TEC_PROVEEDOR.Requisitos;
                        TEC_RECEPTOR.Compatibilidad = TEC_PROVEEDOR.Compatibilidad;
                        TEC_RECEPTOR.Aplica = TEC_PROVEEDOR.Aplica;

                        //TAB 2
                        TEC_RECEPTOR.EliminacionTecObsoleta = TEC_PROVEEDOR.EliminacionTecObsoleta;
                        TEC_RECEPTOR.RoadmapOpcional = TEC_PROVEEDOR.RoadmapOpcional;
                        TEC_RECEPTOR.Referencias = TEC_PROVEEDOR.Referencias;
                        TEC_RECEPTOR.PlanTransConocimiento = TEC_PROVEEDOR.PlanTransConocimiento;
                        TEC_RECEPTOR.EsqMonitoreo = TEC_PROVEEDOR.EsqMonitoreo;
                        TEC_RECEPTOR.LineaBaseSeg = TEC_PROVEEDOR.LineaBaseSeg;
                        TEC_RECEPTOR.EsqPatchManagement = TEC_PROVEEDOR.EsqPatchManagement;

                        //TAB 3
                        TEC_RECEPTOR.DuenoId = TEC_PROVEEDOR.DuenoId;
                        TEC_RECEPTOR.EqAdmContacto = TEC_PROVEEDOR.EqAdmContacto;
                        TEC_RECEPTOR.GrupoSoporteRemedy = TEC_PROVEEDOR.GrupoSoporteRemedy;
                        TEC_RECEPTOR.ConfArqSegId = TEC_PROVEEDOR.ConfArqSegId;
                        TEC_RECEPTOR.ConfArqTecId = TEC_PROVEEDOR.ConfArqTecId;
                        TEC_RECEPTOR.EncargRenContractual = TEC_PROVEEDOR.EncargRenContractual;
                        TEC_RECEPTOR.EsqLicenciamiento = TEC_PROVEEDOR.EsqLicenciamiento;
                        TEC_RECEPTOR.SoporteEmpresarial = TEC_PROVEEDOR.SoporteEmpresarial;
                        TEC_RECEPTOR.FechaModificacion = DateTime.Now;
                        TEC_RECEPTOR.UsuarioModificacion = Usuario;

                        ctx.SaveChanges();
                        retorno = true;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool MigrarDataTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool MigrarDataTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTecnologiaByFabricanteNombre(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro)
                                       || (u.Fabricante).ToUpper().StartsWith(filtro.ToUpper()))
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.Fabricante,
                                           value = u.Fabricante
                                       }).GroupBy(g => new { g.Descripcion })
                                       .Select(s => s.FirstOrDefault())
                                       .OrderBy(x => x.value)
                                       .ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaByFabricanteNombre(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaByFabricanteNombre(string filtro)"
                    , new object[] { null });
            }
        }

        public override DashboardTecnologiaEquipoData GetReporteTecnologiaEquiposFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros)
        {
            try
            {
                DashboardTecnologiaEquipoData dashboardBase = new DashboardTecnologiaEquipoData();
                var parametroMeses = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                var parametroMeses2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2").Valor;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        int DIA = DateTime.Now.Day;
                        int MES = DateTime.Now.Month;
                        int ANIO = DateTime.Now.Year;

                        if (filtros.FechaConsulta.HasValue)
                        {
                            DIA = filtros.FechaConsulta.Value.Day;
                            MES = filtros.FechaConsulta.Value.Month;
                            ANIO = filtros.FechaConsulta.Value.Year;
                        }

                        #region DATOS TECNOLOGIA

                        var objTecnologia = (from a in ctx.Tecnologia
                                             join s in ctx.Subdominio on a.SubdominioId equals s.SubdominioId
                                             join d in ctx.Dominio on s.DominioId equals d.DominioId
                                             join t in ctx.TecnologiaCicloVida on a.TecnologiaId equals t.TecnologiaId
                                             join tt in ctx.Tipo on a.TipoTecnologia equals tt.TipoId into gj
                                             from x in gj.DefaultIfEmpty()
                                             where
                                             (a.Fabricante == filtros.Fabricante.Trim() || string.IsNullOrEmpty(filtros.Fabricante.Trim()))
                                             && (a.Nombre == filtros.Nombre.Trim() || string.IsNullOrEmpty(filtros.Nombre.Trim()))
                                             && (a.Versiones == filtros.Version.Trim() || string.IsNullOrEmpty(filtros.Version.Trim()))
                                             && a.Activo
                                             && t.DiaRegistro == DIA && t.MesRegistro == MES && t.AnioRegistro == ANIO
                                             select new
                                             {
                                                 Id = a.TecnologiaId,
                                                 ClaveTecnologia = a.ClaveTecnologia,
                                                 DominioNomb = d.Nombre,
                                                 SubdominioNomb = s.Nombre,
                                                 TipoTecnologiaId = a.TipoTecnologia,
                                                 FechaAcordada = a.FechaAcordada,
                                                 FechaExtendida = a.FechaExtendida,
                                                 FechaFinSoporte = a.FechaFinSoporte,
                                                 FechaCalculoTec = a.FechaCalculoTec,
                                                 IndiceObsolescencia = t.IndiceObsolescencia,
                                                 Riesgo = t.Riesgo,
                                                 t.Obsoleto,
                                                 a.FlagFechaFinSoporte,
                                                 TipoTecnologia = x.Nombre,
                                                 EstadoId = a.EstadoId,
                                             }).ToList().Select(x => new TecnologiaDTO
                                             {
                                                 Meses = int.Parse(parametroMeses),
                                                 IndicadorMeses1 = int.Parse(parametroMeses),
                                                 IndicadorMeses2 = int.Parse(parametroMeses2),
                                                 Id = x.Id,
                                                 ClaveTecnologia = x.ClaveTecnologia,
                                                 DominioNomb = x.DominioNomb,
                                                 SubdominioNomb = x.SubdominioNomb,
                                                 TipoTecnologiaId = x.TipoTecnologiaId,
                                                 RiesgoTecnologia = x.Riesgo,
                                                 ObsolescenciaTecnologia = x.IndiceObsolescencia,
                                                 TipoTecNomb = x.TipoTecnologia,
                                                 Obsoleto = (x.Obsoleto == 1),
                                                 FlagFechaFinSoporte = x.FlagFechaFinSoporte,
                                                 FechaCalculoBase = x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida ? x.FechaExtendida
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte ? x.FechaFinSoporte
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna ? x.FechaAcordada : null,
                                                 EstadoId = x.EstadoId,
                                             }).ToList();

                        #endregion

                        #region DATA TIPO EQUIPO
                        var dataTipoEquipo = (from a in ctx.EquipoTecnologia
                                              join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                              join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                                              join d in ctx.TipoEquipo on c.TipoEquipoId equals d.TipoEquipoId
                                              where
                                                   (b.Fabricante == filtros.Fabricante.Trim() || string.IsNullOrEmpty(filtros.Fabricante.Trim()))
                                                && (b.Nombre == filtros.Nombre.Trim() || string.IsNullOrEmpty(filtros.Nombre.Trim()))
                                                && (b.Versiones == filtros.Version.Trim() || string.IsNullOrEmpty(filtros.Version.Trim()))
                                              && a.AnioRegistro == ANIO
                                              && a.MesRegistro == MES
                                              && a.DiaRegistro == DIA
                                              && b.Activo && a.FlagActivo
                                              group a by new { d.TipoEquipoId, d.Nombre } into grp
                                              select new CustomAutocomplete
                                              {
                                                  Id = grp.Key.TipoEquipoId.ToString(),
                                                  Descripcion = grp.Key.Nombre,
                                                  Total = grp.Count()
                                              }).ToList();

                        #endregion

                        dashboardBase.TecnologiaList = objTecnologia;
                        dashboardBase.DataPie = dataTipoEquipo;

                        return dashboardBase;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros)"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> GetListadoTecnologiaEquiposFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.EquipoTecnologia
                                         join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                         join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                                         join d in ctx.TipoEquipo on c.TipoEquipoId equals d.TipoEquipoId
                                         join e in ctx.Ambiente on c.AmbienteId equals e.AmbienteId
                                         join f in ctx.DominioServidor on c.DominioServidorId equals f.DominioId
                                         where
                                         (b.Fabricante == filtros.Fabricante.Trim() || string.IsNullOrEmpty(filtros.Fabricante.Trim()))
                                                && (b.Nombre == filtros.Nombre.Trim() || string.IsNullOrEmpty(filtros.Nombre.Trim()))
                                                && (b.Versiones == filtros.Version.Trim() || string.IsNullOrEmpty(filtros.Version.Trim()))
                                         //&& (filtros.SubdominioId == -1 || b.SubdominioId == filtros.SubdominioId)
                                         //b.TecnologiaId == filtros.TecnologiaIdFiltrar
                                         && a.AnioRegistro == ANIO
                                         && a.MesRegistro == MES
                                         && a.DiaRegistro == DIA
                                         && b.Activo && a.FlagActivo
                                         select new EquipoDTO
                                         {
                                             Nombre = c.Nombre,
                                             TipoEquipo = d.Nombre,
                                             FlagTemporal = c.FlagTemporal,
                                             Ambiente = e.DetalleAmbiente,
                                             Subsidiaria = f.Nombre,
                                             ClaveTecnologia = b.ClaveTecnologia
                                         });

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetListadoTecnologiaAplicacionesFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.Relacion
                                         join b in ctx.RelacionDetalle on a.RelacionId equals b.RelacionId
                                         join t in ctx.Tecnologia on b.TecnologiaId equals t.TecnologiaId
                                         join e in ctx.Equipo on a.EquipoId equals e.EquipoId into lj1
                                         from e in lj1.DefaultIfEmpty()
                                         where
                                         a.DiaRegistro == DIA && a.MesRegistro == MES && a.AnioRegistro == ANIO
                                         && a.FlagActivo
                                         && (t.Fabricante == filtros.Fabricante.Trim() || string.IsNullOrEmpty(filtros.Fabricante.Trim()))
                                         && (t.Nombre == filtros.Nombre.Trim() || string.IsNullOrEmpty(filtros.Nombre.Trim()))
                                         && (t.Versiones == filtros.Version.Trim() || string.IsNullOrEmpty(filtros.Version.Trim()))

                                         select new AplicacionDTO
                                         {
                                             Nombre = a.CodigoAPT,
                                             CodigoAPT = a.CodigoAPT,
                                             Equipo = e.Nombre,
                                             EstadoRelacionId = a.EstadoId,
                                             ClaveTecnologia = t.ClaveTecnologia
                                         }).Distinct();

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetListadoTecnologiasVinculadasFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)
        {
            try
            {
                totalRows = 0;

                int DIA = DateTime.Now.Day;
                int MES = DateTime.Now.Month;
                int ANIO = DateTime.Now.Year;

                if (filtros.FechaConsulta.HasValue)
                {
                    DIA = filtros.FechaConsulta.Value.Day;
                    MES = filtros.FechaConsulta.Value.Month;
                    ANIO = filtros.FechaConsulta.Value.Year;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.TecnologiaVinculada
                                         join b in ctx.Tecnologia on a.VinculoId equals b.TecnologiaId
                                         join s in ctx.Subdominio on b.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join t in ctx.Tipo on b.TipoTecnologia equals t.TipoId into x
                                         from y in x.DefaultIfEmpty()
                                         where
                                            (b.Fabricante == filtros.Fabricante.Trim() || string.IsNullOrEmpty(filtros.Fabricante.Trim()))
                                                && (b.Nombre == filtros.Nombre.Trim() || string.IsNullOrEmpty(filtros.Nombre.Trim()))
                                                && (b.Versiones == filtros.Version.Trim() || string.IsNullOrEmpty(filtros.Version.Trim()))
                                         //&& (filtros.SubdominioId == -1 || b.SubdominioId == filtros.SubdominioId)
                                         //a.TecnologiaId == filtros.TecnologiaIdFiltrar
                                         && a.Activo
                                         select new TecnologiaDTO
                                         {
                                             Fabricante = b.Fabricante,
                                             Nombre = b.Nombre,
                                             ClaveTecnologia = b.ClaveTecnologia,
                                             DominioNomb = d.Nombre,
                                             SubdominioNomb = d.Nombre,
                                             TipoTecnologiaId = b.TipoTecnologia,
                                             FechaAcordada = b.FechaAcordada,
                                             FechaExtendida = b.FechaExtendida,
                                             FechaFinSoporte = b.FechaFinSoporte,
                                             FechaCalculoTec = b.FechaCalculoTec,
                                             TipoTecNomb = y.Nombre,
                                             EstadoId = b.EstadoId,
                                         });

                        totalRows = registros.Count();

                        registros = registros.OrderBy(filtros.sortName + " " + filtros.sortOrder);
                        var resultado = registros.Skip((filtros.pageNumber - 1) * filtros.pageSize).Take(filtros.pageSize).ToList();

                        resultado = resultado.Select(x => new TecnologiaDTO
                        {

                            ClaveTecnologia = x.ClaveTecnologia,
                            DominioNomb = x.DominioNomb,
                            SubdominioNomb = x.SubdominioNomb,
                            TipoTecnologiaId = x.TipoTecnologiaId,
                            TipoTecNomb = x.TipoTecNomb,
                            FechaCalculoBase = x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida ? x.FechaExtendida
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte ? x.FechaFinSoporte
                                                                    : x.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna ? x.FechaAcordada : null,
                            EstadoId = x.EstadoId

                        }).ToList();


                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool ExisteEquivalenciaByTecnologiaId(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.TecnologiaEquivalencia
                                        where u.FlagActivo && u.TecnologiaId == Id
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteTecnologiaById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteTecnologiaById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTecnologiaByNombre(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro)
                                       || (u.Nombre).ToUpper().StartsWith(filtro.ToUpper()))
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.Nombre,
                                           Descripcion = u.Nombre,
                                           value = u.Nombre
                                       }).GroupBy(g => new { g.Descripcion })
                                       .Select(s => s.FirstOrDefault())
                                       .OrderBy(x => x.value)
                                       .ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaByNombre(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaByNombre(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiasPendientes()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into lj2
                                         from f in lj2.DefaultIfEmpty()
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         where (u.Activo == true)
                                         && u.EstadoTecnologia == (int)EstadoTecnologia.Registrado
                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Tipo = t.Nombre,
                                             Estado = u.EstadoTecnologia,
                                             FechaAprobacion = u.FechaAprobacion,
                                             UsuarioAprobacion = u.UsuarioAprobacion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             EstadoId = u.EstadoId,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasPendientes()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasPendientes()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaPorVencerDto> GetTecnologiasPorVencer(string subdominio, string tecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                subdominio = string.IsNullOrEmpty(subdominio) ? "" : subdominio;
                tecnologia = string.IsNullOrEmpty(tecnologia) ? "" : tecnologia;
                var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_TECNOLOGIA_POR_VENCER");
                var nroMeses = parametro != null ? int.Parse(parametro.Valor) : 12;
                //var fechaConsulta = DateTime.Now;
                //var fechaConsulta = DateTime.ParseExact(Convert.ToString(DateTime.Now.Date), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime time = DateTime.Now;
                String format = "yyyy-MM-dd";
                var fechaConsulta = Convert.ToDateTime(time.ToString(format));
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaPorVencerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Dashboard_TecnologiasPorVencer_v1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@nroMeses", nroMeses));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsulta));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaPorVencerDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                                TotalAplicaciones = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                NroMeses = nroMeses
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
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaPorVencerDto> GetTecnologiasVencidas(string subdominio, string tecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                subdominio = string.IsNullOrEmpty(subdominio) ? "" : subdominio;
                tecnologia = string.IsNullOrEmpty(tecnologia) ? "" : tecnologia;
                //var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_TECNOLOGIA_POR_VENCER");
                //var nroMeses = parametro != null ? int.Parse(parametro.Valor) : 12;
                var nroMeses = 12;
                //var fechaConsulta = DateTime.Now;
                var fechaConsulta = DateTime.ParseExact("10/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaPorVencerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Dashboard_TecnologiasVencidas_v1]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@subdominio", subdominio));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@nroMeses", nroMeses));
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", fechaConsulta));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@SortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaPorVencerDto()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                                TotalAplicaciones = reader.IsDBNull(reader.GetOrdinal("TotalAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalAplicaciones")),
                                NroMeses = nroMeses
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
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiaUpdate(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         join f in ctx.Familia on u.FamiliaId equals f.FamiliaId into lj2
                                         from f in lj2.DefaultIfEmpty()
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         where u.Activo
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Estado = u.EstadoTecnologia,
                                             Fabricante = u.Fabricante,
                                             Nombre = u.Nombre,
                                             Versiones = u.Versiones,
                                             Tipo = t.Nombre,
                                             TipoTecnologiaId = u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0,
                                             CodigoTecnologiaAsignado = u.CodigoTecnologiaAsignado,
                                             FlagSiteEstandar = u.FlagSiteEstandar,
                                             CasoUso = u.CasoUso,
                                             Descripcion = u.Descripcion,
                                             FechaLanzamiento = u.FechaLanzamiento,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                             Fuente = u.FuenteId,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FechaExtendida = u.FechaExtendida,
                                             FechaAcordada = u.FechaAcordada,
                                             EqAdmContacto = u.EqAdmContacto,
                                             GrupoSoporteRemedy = u.GrupoSoporteRemedy,
                                             ConfArqSeg = u.ConfArqSegId,
                                             ConfArqTec = u.ConfArqTecId,
                                             UrlConfluence = u.UrlConfluence,
                                             EstadoId = u.EstadoId
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaUpdate(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaUpdate(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TecnologiaEstandarDTO GetTecnologiaEstandarById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Tecnologia
                                   join a in ctx.Producto on u.ProductoId equals a.ProductoId into lja
                                   from a in lja.DefaultIfEmpty()
                                   where u.TecnologiaId == id && u.Activo
                                   select new TecnologiaEstandarDTO()
                                   {
                                       Id = u.TecnologiaId,
                                       ClaveTecnologia = u.ClaveTecnologia,
                                       CodigoTecnologia = u.CodigoTecnologiaAsignado,
                                       CodigoProducto = a == null ? null : a.Codigo,
                                       PublicacionLBS = u.LineaBaseSeg,
                                       UrlConfluence = u.UrlConfluence,
                                       EqAdmContacto = a.EquipoAdmContacto,
                                       GrupoSoporteRemedy = a.GrupoTicketRemedyNombre,
                                       EstadoTecnologiaId = u.EstadoTecnologia,
                                       FuenteId = u.FuenteId,
                                       FechaCalculoTec = u.FechaCalculoTec,
                                       FechaLanzamiento = u.FechaLanzamiento,
                                       FechaFinSoporte = u.FechaFinSoporte,
                                       FechaAcordada = u.FechaAcordada,
                                       FechaExtendida = u.FechaExtendida,
                                       FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                       EquipoAprovisionamiento = a.EquipoAprovisionamiento,
                                       TribuCoeDisplayName = a == null ? null : a.TribuCoeDisplayName,
                                       SquadDisplayName = a == null ? null : a.SquadDisplayName,
                                       OwnerStr = a.OwnerDisplayName,
                                       EsquemaLicenciamientoId = u.EsqLicenciamiento,
                                       LineamientoTecnologiaId = u.UrlConfluenceId,
                                       LineamientoTecnologia = u.UrlConfluence,
                                       LineamientoBaseSeguridadId = u.RevisionSeguridadId,
                                       LineamientoBaseSeguridad = u.RevisionSeguridadDescripcion,
                                       //LineamientoBaseSeguridad = u.LineaBaseSeg,
                                       CompatibilidadSOIds = u.CompatibilidadSOId,
                                       CompatibilidadCloudIds = u.CompatibilidadCloudId,
                                       Aplica = u.Aplica,
                                       TecReemplazoDepId = u.TecReemplazoDepId,
                                       EstadoId = u.EstadoId,
                                   }).FirstOrDefault();

                    if (entidad.EstadoId == (int)ETecnologiaEstado.Deprecado && entidad.TecReemplazoDepId != null)
                        entidad.TecnologiaReemplazo = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == entidad.TecReemplazoDepId).ClaveTecnologia;

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: TipoDTO GetTipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaEstandarDTO> GetListadoTecnologiaEstandar(PaginacionEstandar pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var subdominioIntIds = new List<int>();
                var dominioIntIds = new List<int>();
                var tipoTecIntIds = new List<int>();
                var estadoTecIntIds = new List<int>();
                var estadoTecIntIdsV = new List<int>();

                var parametroMeses1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                int cantidadMeses1 = int.Parse(parametroMeses1);

                DateTime fechaActual = DateTime.Now;
                DateTime fechaAgregada = fechaActual.AddMonths(cantidadMeses1);
                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(pag.SubdominioIds))
                        subdominioIntIds = pag.SubdominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                    if (!string.IsNullOrEmpty(pag.DominioIds))
                        dominioIntIds = pag.DominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                    if (!string.IsNullOrEmpty(pag.TipoTecnologiaIds))
                        tipoTecIntIds = pag.TipoTecnologiaIds.Split('|').Select(x => int.Parse(x)).ToList();

                    if (!string.IsNullOrEmpty(pag.EstadoIds))
                    {
                        estadoTecIntIds = pag.EstadoIds.Split('|').Select(x => int.Parse(x)).ToList();
                        estadoTecIntIdsV = pag.EstadoIds.Split('|').Select(x => int.Parse(x)).ToList();
                    }

                    if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.VigentePorVencer) != -1)
                        estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Vigente);

                    if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.DeprecadoPorVencer) != -1)
                        estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Deprecado);

                    if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.RestringidoPorVencer) != -1)
                        estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Restringido);

                    var aplicaIds = string.IsNullOrEmpty(pag.AplicaIds) ? (new List<string>()).AsQueryable() : pag.AplicaIds.Split('|').ToList().AsQueryable();
                    var compatibilidadSOIds = string.IsNullOrEmpty(pag.CompatibilidadSOIds) ? (new List<string>()).AsQueryable() : pag.CompatibilidadSOIds.Split('|').AsQueryable();
                    var compatibilidadCloudIds = string.IsNullOrEmpty(pag.CompatibilidadCloudIds) ? (new List<string>()).AsQueryable() : pag.CompatibilidadCloudIds.Split('|').AsQueryable();

                    var registros = (from u in ctx.Tecnologia
                                     join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                     join a in ctx.Producto on u.ProductoId equals a.ProductoId into lja
                                     from a in lja.DefaultIfEmpty()
                                     join s in ctx.Subdominio on a.SubDominioId equals s.SubdominioId
                                     join d in ctx.Dominio on s.DominioId equals d.DominioId


                                     join tcv in ctx.TipoCicloVida on a.TipoCicloVidaId equals tcv.TipoCicloVidaId into lja2
                                     from tcv in lja2.DefaultIfEmpty()

                                     //let fechaAgregada= fechaActual.AddMonths(tcv.NroPeriodosEstadoAmbar)
                                     let fechaCalculo = !u.FechaCalculoTec.HasValue ? null : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : null
                                     //let estadoEstandar = !fechaCalculo.HasValue ? (int)ETecnologiaEstadoEstandar.Vigente : 0
                                     //let estadoEstandar = !fechaCalculo.HasValue ? (int)ETecnologiaEstadoEstandar.Vigente : GetEstadoTecnologiaEstandar(fechaCalculo.Value, fechaActual, fechaAgregada)
                                     

                                     let estadoCalculadoNew = !(u.FlagFechaFinSoporte ?? false) ? ((u.TipoTecnologia == 13 || u.TipoTecnologia == 15 || u.TipoTecnologia == 16) ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : (int)ETecnologiaEstadoEstandar.Vigente) : (((u.FlagFechaFinSoporte ?? false) && !u.FechaCalculoTec.HasValue) ? (int)ETecnologiaEstadoEstandar.Obsoleto : (!fechaCalculo.HasValue ? (int)ETecnologiaEstadoEstandar.Obsoleto : (fechaCalculo < fechaActual ? (int)ETecnologiaEstadoEstandar.Obsoleto : (fechaCalculo < fechaAgregada ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : ((u.TipoTecnologia == 13 || u.TipoTecnologia == 15 || u.TipoTecnologia == 16) ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : (int)ETecnologiaEstadoEstandar.Vigente) ))))

                                     let estadoEstandar = !fechaCalculo.HasValue ? ((u.TipoTecnologia == 13 || u.TipoTecnologia == 15 || u.TipoTecnologia==16) ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : (int)ETecnologiaEstadoEstandar.Vigente) : ((fechaCalculo.Value < fechaActual) ? (int)ETecnologiaEstadoEstandar.Obsoleto : ((fechaCalculo.Value < fechaAgregada) ? (int)ETecnologiaEstadoEstandar.VigentePorVencer : (int)ETecnologiaEstadoEstandar.Vigente))
                                     
                                     where (string.IsNullOrEmpty(pag.DominioIds) || dominioIntIds.Contains(s.DominioId))
                                        && (string.IsNullOrEmpty(pag.SubdominioIds) || subdominioIntIds.Contains(s.SubdominioId))
                                        && (string.IsNullOrEmpty(pag.TipoTecnologiaIds) || tipoTecIntIds.Contains(u.TipoTecnologia.Value))
                                        //&& (string.IsNullOrEmpty(pag.EstadoIds) || estadoTecIntIds.Contains(u.EstadoId.Value))
                                        && (string.IsNullOrEmpty(pag.EstadoIds) || estadoTecIntIds.Contains(u.EstadoId ?? 0))
                                        && (string.IsNullOrEmpty(pag.Tecnologia) || u.ClaveTecnologia.ToUpper().Contains(pag.Tecnologia.ToUpper()))
                                        && (pag.GetAll || u.FlagSiteEstandar.Value)
                                        && s.Activo && d.Activo && u.Activo
                                        && (aplicaIds.Count() == 0 || aplicaIds.Contains(u.Aplica))
                                          && (compatibilidadSOIds.Count() == 0 || compatibilidadSOIds.Count(x => u.CompatibilidadSOId.Contains(x)) > 0)
                                          && (compatibilidadCloudIds.Count() == 0 || compatibilidadCloudIds.Count(x => u.CompatibilidadCloudId.Contains(x)) > 0)
                                     select new TecnologiaEstandarDTO()
                                     {
                                         Id = u.TecnologiaId,
                                         Tipo = t.Nombre,
                                         ClaveTecnologia = u.ClaveTecnologia,
                                         EstadoId = u.EstadoId,
                                         Dominio = d.Nombre,
                                         Subdominio = s.Nombre,
                                         Activo = u.Activo,
                                         GrupoSoporteRemedy = u.GrupoSoporteRemedy,
                                         PublicacionLBS = u.RevisionSeguridadDescripcion,
                                         UrlConfluence = u.UrlConfluence,
                                         EqAdmContacto = u.EqAdmContacto,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         CodigoTecnologia = u.CodigoTecnologiaAsignado,
                                         CodigoProducto = a == null ? null : a.Codigo,
                                         FuenteId = u.FuenteId,
                                         FechaCalculoTec = u.FechaCalculoTec,
                                         FechaLanzamiento = u.FechaLanzamiento,
                                         FechaFinSoporte = u.FechaFinSoporte,
                                         FechaAcordada = u.FechaAcordada,
                                         FechaExtendida = u.FechaExtendida,
                                         FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                         EquipoAprovisionamiento = u.EquipoAprovisionamiento,
                                         TribuCoeDisplayName = a == null ? null : a.TribuCoeDisplayName,
                                         SquadDisplayName = a == null ? null : a.SquadDisplayName,
                                         OwnerStr = u.DuenoId,
                                         EsquemaLicenciamientoId = u.EsqLicenciamiento,
                                         LineamientoTecnologiaId = u.UrlConfluenceId,
                                         LineamientoTecnologia = u.UrlConfluence,
                                         LineamientoBaseSeguridadId = u.RevisionSeguridadId,
                                         LineamientoBaseSeguridad = u.RevisionSeguridadDescripcion,
                                         CompatibilidadSOIds = u.CompatibilidadSOId,
                                         CompatibilidadCloudIds = u.CompatibilidadCloudId,
                                         Aplica = u.Aplica,
                                         CantidadMeses1 = tcv == null ? cantidadMeses1 : tcv.NroPeriodosEstadoAmbar,
                                         TipoTecnologia = u.TipoTecnologia,
                                     }).ToList();

                    registros = registros.Where(x => estadoTecIntIdsV.Contains(x.EstadoIdCalculado ?? 0)).ToList();

                    totalRows = registros.Count();
                    registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder).ToList();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();


                    if (resultado.Count > 0) resultado.ForEach(x => x.FlagVigentePorVencer = !x.FechaCalculada.HasValue ? false : GetSemaforoTecnologia(x.FechaCalculada.Value, fechaActual, cantidadMeses1) == 0);

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
                    , "Error en el metodo: List<AmbienteDTO> GetAmbiente(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro, string subdominioList, string soPcUsuarioList = null)
        {
            try
            {
                var subdominioListInt = new List<int>();
                var soPcUsuarioListInt = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(subdominioList))
                            subdominioListInt = subdominioList.Split(';').Select(x => int.Parse(x)).ToList();

                        if (!string.IsNullOrEmpty(soPcUsuarioList))
                            soPcUsuarioListInt = soPcUsuarioList.Split(';').Select(x => int.Parse(x)).ToList();

                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                       where u.Activo && s.Activo && t.Activo
                                       && (string.IsNullOrEmpty(soPcUsuarioList) || soPcUsuarioListInt.Contains(u.TecnologiaId))
                                       && (string.IsNullOrEmpty(subdominioList) || subdominioListInt.Contains(s.SubdominioId))
                                       && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia,
                                           EstadoId = u.EstadoId
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override EntidadRetorno ActualizarRetorno(ActualizarTecnologia pag)
        {
            var query = string.Empty;
            var control = true;
            try
            {
                var retorno = new EntidadRetorno();

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var tipo = ctx.Tipo.Where(x => x.Nombre.ToUpper() == pag.TipoTecnologia.ToUpper()).FirstOrDefault();
                    if (tipo != null)
                    {
                        var tecnologia = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == pag.TecnologiaId);
                        if (tecnologia != null)
                        {
                            query = string.Format("update cvt.Tecnologia set TipoTecnologia={0}, UsuarioModificacion='{1}', FechaModificacion=getdate() where TecnologiaId={2}"
                                , tipo.TipoId
                                , pag.Usuario
                                , tecnologia.TecnologiaId);
                            retorno.Descripcion = "Operación exitosa";
                        }
                        else
                        {
                            retorno.CodigoRetorno = -2;
                            retorno.Descripcion = "ID de la tecnología no registrada";
                            control = false;
                        }
                    }
                    else
                    {
                        retorno.CodigoRetorno = -1;
                        retorno.Descripcion = "Tipo de tecnología no encontrada";
                        control = false;
                    }
                }

                if (control)
                {
                    using (var cnx = new SqlConnection(Constantes.CadenaConexion))
                    {
                        cnx.Open();
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = cnx;
                            cmd.CommandText = query;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                        cnx.Close();
                    }
                }

                return retorno;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);

                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);

                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro)"
                    , new object[] { null });
            }
        }


        private DtoResultCode ValidarObjTecnologia(DtoTecnologia objeto, GestionCMDB_ProdEntities ctx)
        {
            //default value
            var retornoRC = new DtoResultCode()
            {
                Code = (int)EResultCode.OK,
                Description = Utilitarios.GetEnumDescription2(EResultCode.OK)
            };

            if (string.IsNullOrWhiteSpace(objeto.vendor) || string.IsNullOrWhiteSpace(objeto.technologyName) || string.IsNullOrWhiteSpace(objeto.version)
                || string.IsNullOrWhiteSpace(objeto.family) || string.IsNullOrWhiteSpace(objeto.technologyType)
                || string.IsNullOrWhiteSpace(objeto.technologyType))
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.CamposRequeridosIncompletos,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.CamposRequeridosIncompletos)
                };
            }

            var tipo = ctx.Tipo.FirstOrDefault(x => x.Nombre.ToUpper().Equals(objeto.technologyType.ToUpper()));
            if (tipo == null)
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.TipoNoExiste,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.TipoNoExiste)
                };
            }

            var dominio = ctx.Dominio.FirstOrDefault(x => x.DominioId == objeto.domainId);
            if (dominio == null)
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.DominioSubdominioNoExiste,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.DominioSubdominioNoExiste)
                };
            }

            var subdominio = ctx.Subdominio.FirstOrDefault(x => x.SubdominioId == objeto.subdomainId);
            if (subdominio == null)
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.DominioSubdominioNoExiste,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.DominioSubdominioNoExiste)
                };
            }
            else
            {
                var hasRelacion = subdominio.DominioId == dominio.DominioId;
                if (!hasRelacion)
                {
                    return new DtoResultCode()
                    {
                        Code = (int)EResultCode.DominioSubdominioNoExiste,
                        Description = Utilitarios.GetEnumDescription2(EResultCode.DominioSubdominioNoExiste)
                    };
                }
            }

            var arrEstadoTecnologia = new int[]
            {
                (int)ETecnologiaEstado.Vigente,
                //(int)ETecnologiaEstado.VigentePorVencer,
                (int)ETecnologiaEstado.Deprecado,
                (int)ETecnologiaEstado.Obsoleto,
            };

            if (!arrEstadoTecnologia.Contains(objeto.technologyState))
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.EstadoNoExiste,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.EstadoNoExiste)
                };
            }

            var clave = $"{objeto.vendor} {objeto.technologyName} {objeto.version}";
            var tecnologia = ctx.Tecnologia.FirstOrDefault(x => x.ClaveTecnologia.Trim().ToUpper().Equals(clave.Trim().ToUpper()));
            if (tecnologia != null)
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.ClaveExiste,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.ClaveExiste)
                };
            }

            var arrFuenteFechaFin = new int[]
            {
                (int)Fuente.ADDM,
                (int)Fuente.SNOW,
                (int)Fuente.Manual
            };

            if (!arrFuenteFechaFin.Contains(objeto.endDateSupportSource))
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.FuenteFechaFinIncorrecta,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.FuenteFechaFinIncorrecta)
                };
            }

            var arrTipoFechaFinSoporte = new int[]
            {
                (int)FechaCalculoTecnologia.FechaExtendida,
                (int)FechaCalculoTecnologia.FechaFinSoporte,
                (int)FechaCalculoTecnologia.FechaInterna,
            };

            if (!arrTipoFechaFinSoporte.Contains(objeto.typeEndDateSupport))
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.TipoFechaFinSoporteIncorrecta,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.TipoFechaFinSoporteIncorrecta)
                };
            }

            var arrParametros = new int[] { 0, 1, 2, 3, 4, 5 };
            if (!arrParametros.Contains(objeto.valueParameterExistencia) || !arrParametros.Contains(objeto.valueParameterFacilidad)
                || !arrParametros.Contains(objeto.valueParameterRiesgo) || !arrParametros.Contains(objeto.valueParameterVulnerabilidad))
            {
                return new DtoResultCode()
                {
                    Code = (int)EResultCode.CriterioIncorrecto,
                    Description = Utilitarios.GetEnumDescription2(EResultCode.CriterioIncorrecto)
                };
            }

            return retornoRC;
        }

        public override int AddOrEditTecnologiaPowerApps(DtoTecnologia objeto)
        {
            DbContextTransaction transaction = null;

            int ID = 0;
            int tipoInicial = 0;
            int estado = 0;
            DateTime fechaActual = DateTime.Now;
            DateTime? fechaFin = null;
            DateTime? fechaFinExtendida = null;
            DateTime? fechaFinAcordada = null;

            const string ESTADO_ESTANDAR = "Estándar";
            const string ESTADO_ESTANDAR_RESTRINGIDO = "Estándar Restringido";
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        var dataValidation = ValidarObjTecnologia(objeto, ctx);
                        if (dataValidation.Code == (int)EResultCode.OK)
                        {
                            tipoInicial = ctx.Tipo.FirstOrDefault(x => x.Nombre.ToUpper().Equals(objeto.technologyType.ToUpper())).TipoId;

                            if (ESTADO_ESTANDAR == objeto.finalTechnologyType)
                                estado = (int)ETecnologiaEstado.Vigente;
                            else if (ESTADO_ESTANDAR_RESTRINGIDO == objeto.finalTechnologyType)
                                //estado = (int)ETecnologiaEstado.VigentePorVencer;
                                estado = (int)ETecnologiaEstado.Deprecado;
                            else
                            {
                                if (tipoInicial == (int)ETipoTecnologia.NoEstandar)
                                    estado = (int)ETecnologiaEstado.Obsoleto;
                                else
                                    estado = objeto.technologyState;
                            }

                            if (objeto.typeEndDateSupport == (int)FechaCalculoTecnologia.FechaFinSoporte)
                                fechaFin = objeto.endDateSupport;
                            if (objeto.typeEndDateSupport == (int)FechaCalculoTecnologia.FechaExtendida)
                                fechaFinExtendida = objeto.endDateSupport;
                            if (objeto.typeEndDateSupport == (int)FechaCalculoTecnologia.FechaInterna)
                                fechaFinAcordada = objeto.endDateSupport;

                            if (objeto.technologyId == 0)
                            {
                                var entidad = new Tecnologia()
                                {
                                    EstadoTecnologia = (int)EstadoTecnologia.Aprobado,
                                    SubdominioId = objeto.subdomainId,
                                    TecnologiaId = objeto.technologyId,
                                    Activo = true,
                                    ClaveTecnologia = string.Format("{0} {1} {2}", objeto.vendor, objeto.technologyName, objeto.version),
                                    UsuarioCreacion = "power-apps",
                                    FechaCreacion = fechaActual,
                                    Nombre = objeto.technologyName,
                                    Descripcion = objeto.description,
                                    Versiones = objeto.version,
                                    TipoTecnologia = tipoInicial,
                                    FamiliaId = 1,
                                    FlagConfirmarFamilia = true,
                                    FlagFechaFinSoporte = objeto.hasEndDateSupport,
                                    FechaCalculoTec = objeto.typeEndDateSupport,
                                    FechaLanzamiento = null,
                                    FechaExtendida = fechaFinExtendida,
                                    FechaFinSoporte = fechaFin,
                                    FechaAcordada = fechaFinAcordada,
                                    ComentariosFechaFin = "",
                                    FuenteId = objeto.endDateSupportSource,
                                    Existencia = objeto.valueParameterExistencia,
                                    Facilidad = objeto.valueParameterFacilidad,
                                    Riesgo = objeto.valueParameterRiesgo,
                                    Vulnerabilidad = objeto.valueParameterVulnerabilidad,
                                    CasoUso = objeto.useCases,
                                    Requisitos = "",
                                    Compatibilidad = "",
                                    Aplica = "",
                                    FlagAplicacion = null,
                                    CodigoAPT = "",
                                    Fabricante = objeto.vendor,
                                    //Fin tab 1                        
                                    EliminacionTecObsoleta = null,
                                    RoadmapOpcional = "",
                                    Referencias = "",
                                    PlanTransConocimiento = "",
                                    EsqMonitoreo = "",
                                    LineaBaseSeg = "",
                                    EsqPatchManagement = "",
                                    //Fin tab 2
                                    DuenoId = "",
                                    EqAdmContacto = Utilitarios.FullStr(objeto.managementTeam),
                                    GrupoSoporteRemedy = Utilitarios.FullStr(objeto.remedySupportGroup),
                                    ConfArqSegId = Utilitarios.FullStr(objeto.complianceSecurityArchitect),
                                    ConfArqTecId = Utilitarios.FullStr(objeto.complianceTechnologyArchitect),
                                    EncargRenContractual = "",
                                    EsqLicenciamiento = "",
                                    SoporteEmpresarial = "",
                                    UrlConfluence = objeto.confluenceUrl,
                                    //TipoId = objeto.TipoId,
                                    EstadoId = estado,
                                    FlagSiteEstandar = objeto.shownStandarsSite,
                                    FechaAprobacion = fechaActual
                                    //Fin tab 3                                                  
                                };
                                ctx.Tecnologia.Add(entidad);
                                ctx.SaveChanges();
                                ID = entidad.TecnologiaId;
                            }
                            else
                            {
                                var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.technologyId);
                                if (entidad != null)
                                {
                                    entidad.EqAdmContacto = string.IsNullOrWhiteSpace(objeto.managementTeam) ? entidad.EqAdmContacto : objeto.managementTeam;
                                    entidad.GrupoSoporteRemedy = string.IsNullOrWhiteSpace(objeto.remedySupportGroup) ? entidad.GrupoSoporteRemedy : objeto.remedySupportGroup;
                                    entidad.ConfArqSegId = string.IsNullOrWhiteSpace(objeto.complianceSecurityArchitect) ? entidad.ConfArqSegId : objeto.complianceSecurityArchitect;
                                    entidad.ConfArqTecId = string.IsNullOrWhiteSpace(objeto.complianceTechnologyArchitect) ? entidad.ConfArqTecId : objeto.complianceTechnologyArchitect;
                                    entidad.UrlConfluence = string.IsNullOrWhiteSpace(objeto.confluenceUrl) ? entidad.UrlConfluence : objeto.confluenceUrl;

                                    entidad.FechaModificacion = fechaActual;
                                    entidad.UsuarioModificacion = "power-apps";

                                    ctx.SaveChanges();

                                    ID = entidad.TecnologiaId;
                                }
                            }

                            transaction.Commit();
                        }
                        else
                        {
                            ID = dataValidation.Code;
                        }

                        return ID;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecnologiaEstandar_2(string _tecnologia, string _tipoTecIds, string _estadoTecIds, bool _getAll, string _subdominioIds = null, string _dominiosId = null, string _aplica = null, string _compatibilidadSO = null, string _compatibilidadCloud = null)
        {
            try
            {
                var fechaActual = DateTime.Now.Date;
                var subdominioIntIds = new List<int>();
                var dominiosIntIds = new List<int>();

                var tipoTecIntIds = new List<int>();
                var estadoTecIntIds = new List<int>();
                var estadoTecIntIdsV = new List<int>();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var parametroMeses1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                        int cantidadMeses1 = int.Parse(parametroMeses1);

                        if (!string.IsNullOrEmpty(_dominiosId))
                            dominiosIntIds = _dominiosId.Split('|').Select(x => int.Parse(x)).ToList();

                        if (!string.IsNullOrEmpty(_subdominioIds))
                            subdominioIntIds = _subdominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                        if (!string.IsNullOrEmpty(_tipoTecIds))
                            tipoTecIntIds = _tipoTecIds.Split('|').Select(x => int.Parse(x)).ToList();

                        if (!string.IsNullOrEmpty(_estadoTecIds))
                        {
                            estadoTecIntIds = _estadoTecIds.Split('|').Select(x => int.Parse(x)).ToList();
                            estadoTecIntIdsV = _estadoTecIds.Split('|').Select(x => int.Parse(x)).ToList();
                        }
                        
                        if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.VigentePorVencer) != -1)
                            estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Vigente);

                        if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.DeprecadoPorVencer) != -1)
                            estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Deprecado);

                        if (estadoTecIntIds.IndexOf((int)ETecnologiaEstadoEstandar.RestringidoPorVencer) != -1)
                            estadoTecIntIds.Add((int)ETecnologiaEstadoEstandar.Restringido);

                        var aplicaIds = string.IsNullOrEmpty(_aplica) ? (new List<string>()).AsQueryable() : _aplica.Split('|').ToList().AsQueryable();
                        var compatibilidadSOIds = string.IsNullOrEmpty(_compatibilidadSO) ? (new List<string>()).AsQueryable() : _compatibilidadSO.Split('|').AsQueryable();
                        var compatibilidadCloudIds = string.IsNullOrEmpty(_compatibilidadCloud) ? (new List<string>()).AsQueryable() : _compatibilidadCloud.Split('|').AsQueryable();

                        var tecnologia = (from t in ctx.Tecnologia
                                          join t2 in ctx.Tipo on t.TipoTecnologia equals t2.TipoId
                                          join t3 in ctx.Producto on t.ProductoId equals t3.ProductoId into ljt3
                                          from t3 in ljt3.DefaultIfEmpty()
                                          join t4 in ctx.TipoCicloVida on t3.TipoCicloVidaId equals t4.TipoCicloVidaId into ljt4
                                          from t4 in ljt4.DefaultIfEmpty()
                                          where t.Activo
                                          && !t.FlagEliminado
                                          && (_getAll || t.FlagSiteEstandar.Value)
                                          && (string.IsNullOrEmpty(_tecnologia) || t.ClaveTecnologia.ToUpper().Contains(_tecnologia.ToUpper()))
                                          && (aplicaIds.Count() == 0 || aplicaIds.Contains(t.Aplica))
                                          && (compatibilidadSOIds.Count() == 0 || compatibilidadSOIds.Count(x => t.CompatibilidadSOId.Contains(x)) > 0)
                                          && (compatibilidadCloudIds.Count() == 0 || compatibilidadCloudIds.Count(x => t.CompatibilidadCloudId.Contains(x)) > 0)
                                          select new TecnologiaBase()
                                          {
                                              Id = t.TecnologiaId,
                                              Nombre = t.ClaveTecnologia,
                                              SubdominioId = t3 == null ? 0 : t3.SubDominioId,
                                              EstadoId = t.EstadoId ?? 3,
                                              //EstadoId = t.EstadoId ?? 3,
                                              CodigoTecnologia = t.CodigoTecnologiaAsignado,
                                              CodigoProducto = t3 == null ? null : t3.Codigo,
                                              Url = t.UrlConfluence,
                                              FlagFechaFinSoporte = t.FlagFechaFinSoporte,
                                              FechaCalculoTec = t.FechaCalculoTec,
                                              FechaExtendida = t.FechaExtendida,
                                              FechaFinSoporte = t.FechaFinSoporte,
                                              FechaAcordada = t.FechaAcordada,
                                              TipoTecnologia = t.TipoTecnologia,
                                              TipoTecnologiaToString = t2.Nombre,
                                              Indicador1 = t4 == null ? cantidadMeses1 : t4.NroPeriodosEstadoAmbar,
                                              CantidadMeses1 = t4 == null ? cantidadMeses1 : t4.NroPeriodosEstadoAmbar
                                          }).ToList();

                        var dominio = (from d in ctx.Dominio
                                       where d.Activo && (string.IsNullOrEmpty(_dominiosId) || dominiosIntIds.Contains(d.DominioId))
                                       select d).ToList();

                        var subdominio = (from s in ctx.Subdominio
                                          where s.Activo && (string.IsNullOrEmpty(_subdominioIds) || subdominioIntIds.Contains(s.SubdominioId))
                                          select s).ToList();

                        var tipotecnologia = (from t in ctx.Tipo
                                              where t.Activo && (string.IsNullOrEmpty(_tipoTecIds) || tipoTecIntIds.Contains(t.TipoId))
                                              select t).ToList();

                        var registros = (from d in dominio
                                         join s in subdominio on d.DominioId equals s.DominioId
                                         join t in tecnologia on s.SubdominioId equals t.SubdominioId
                                         join tt in tipotecnologia on t.TipoTecnologia equals tt.TipoId
                                         where
                                         (string.IsNullOrEmpty(_estadoTecIds) || estadoTecIntIds.Contains(t.EstadoId))
                                         group new { d, s, t, tt } by new
                                         {
                                             DomId = d.DominioId,
                                             Dominio = d.Nombre,
                                             SubId = s.SubdominioId,
                                             Subdomino = s.Nombre,
                                             TipoTec = tt.Nombre,
                                             TipoId = tt.TipoId,
                                         } into grp
                                         orderby grp.Key.Dominio
                                         select grp).ToList().Select(grp =>
                                         new TecnologiaDTO()
                                         {
                                             DominioNomb = grp.Key.Dominio,
                                             DominioId = grp.Key.DomId,
                                             SubdominioId = grp.Key.SubId,
                                             SubdominioNomb = grp.Key.Subdomino,
                                             TipoTecNomb = grp.Key.TipoTec,
                                             TipoTecnologiaId = grp.Key.TipoId,
                                             //TecnologiaVigenteStr = string.Join("<hr class='custom-hr'></br>",
                                             //    tecnologia.Where(x=>x.EstadoId == (int)ETecnologiaEstado.Vigente).ToList()
                                             //    .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                             //    .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Vigente).ToArray()
                                             //    .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "vigenteClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                             //TecnologiaDeprecadoStr = string.Join("<hr class='custom-hr'></br>",
                                             //    tecnologia.Where(x => x.EstadoId == (int)ETecnologiaEstado.Deprecado).ToList()
                                             //    .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                             //    .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Deprecado).ToArray()
                                             //    .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "deprecadoClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                             //TecnologiaObsoletoStr = string.Join("<hr class='custom-hr'></br>",
                                             //    tecnologia.Where(x => x.EstadoId == (int)ETecnologiaEstado.Obsoleto).ToList()
                                             //    .Join(tipotecnologia, t1 => t1.TipoTecnologia, t2 => t2.TipoId, (t1, t2) => new { t1, t2 })
                                             //    .Where(y => y.t1.SubdominioId == grp.Key.SubId && y.t1.EstadoId == (int)ETecnologiaEstado.Obsoleto).ToArray()
                                             //    .Select(m => Utilitarios.DevolverTecnologiaEstandarStr(m.t1.Url, m.t1.CodigoTecnologia, m.t1.Nombre, "obsoletoClass", m.t2.FlagMostrarEstado, m.t1.TipoTecnologiaToString, m.t1.Id))),
                                         }).ToList();

                        var tecnologiaIds = tecnologia.Select(x => x.Id).ToArray();

                        //var tecCicloVida = (from u in ctx.TecnologiaCicloVida
                        //                    where
                        //                    tecnologiaIds.Contains(u.TecnologiaId) &&
                        //                    u.DiaRegistro == fechaActual.Day &&
                        //                    u.MesRegistro == fechaActual.Month &&
                        //                    u.AnioRegistro == fechaActual.Year
                        //                    select new
                        //                    {
                        //                        TecnologiaId = u.TecnologiaId,
                        //                        Obsoleto = u.Obsoleto,
                        //                        EsIndefinida = u.EsIndefinida,
                        //                        FechaCalculoBase = u.FechaCalculoBase
                        //                    }
                        //    ).ToList();


                        foreach (var item in registros)
                        {
                            var vigenteList = (from u in tecnologia
                                    join u2 in tipotecnologia on u.TipoTecnologia equals u2.TipoId
                                    where (u.EstadoId == (int)ETecnologiaEstadoEstandar.Vigente)
                                        && (string.IsNullOrEmpty(_estadoTecIds) || estadoTecIntIds.Contains(u.EstadoId))
                                        && u.TipoTecnologia == item.TipoTecnologiaId
                                        && u.SubdominioId == item.SubdominioId
                                    select new
                                    {
                                        EstadoStr = !u.FechaCalculada.HasValue ?
                                            Utilitarios.DevolverTecnologiaEstandarStr(u.Url, u.CodigoProducto, u.Nombre, "vigenteClass", u2.FlagMostrarEstado, item.EstadoStr, u.Id)
                                            : GetSemaforoTecnologiaClass(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, u.Url, u.CodigoProducto, u.Nombre, u2.FlagMostrarEstado, item.EstadoStr, u.Id, new string[] { "vigentePorVencer", "vigenteClass" }, u.TipoTecnologia),
                                        EstadoId = !u.FechaCalculada.HasValue ?
                                            (int)ETecnologiaEstadoEstandar.Vigente
                                            : GetEstadoTecnologiaId(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, new int[] { (int)ETecnologiaEstadoEstandar.VigentePorVencer, (int)ETecnologiaEstadoEstandar.Vigente }, u.TipoTecnologia)
                                    }).ToList();

                            var vigente = vigenteList.Where(x => estadoTecIntIdsV.Contains(x.EstadoId)).Select(x => x.EstadoStr).ToList();

                            var deprecadoList = (from u in tecnologia
                                    join u2 in tipotecnologia on u.TipoTecnologia equals u2.TipoId
                                    where (u.EstadoId == (int)ETecnologiaEstadoEstandar.Deprecado)
                                        && (string.IsNullOrEmpty(_estadoTecIds) || estadoTecIntIds.Contains(u.EstadoId))
                                        && u.TipoTecnologia == item.TipoTecnologiaId
                                        && u.SubdominioId == item.SubdominioId
                                    select new
                                    {
                                        EstadoStr = !u.FechaCalculada.HasValue ?
                                            Utilitarios.DevolverTecnologiaEstandarStr(u.Url, u.CodigoProducto, u.Nombre, "deprecadoClass", u2.FlagMostrarEstado, item.EstadoStr, u.Id)
                                            : GetSemaforoTecnologiaClass(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, u.Url, u.CodigoProducto, u.Nombre, u2.FlagMostrarEstado, item.EstadoStr, u.Id, new string[] { "deprecadoPorVencer", "deprecadoClass" }, u.TipoTecnologia),
                                        EstadoId = !u.FechaCalculada.HasValue ?
                                            (int)ETecnologiaEstadoEstandar.Deprecado
                                            : GetEstadoTecnologiaId(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, new int[] { (int)ETecnologiaEstadoEstandar.DeprecadoPorVencer, (int)ETecnologiaEstadoEstandar.Deprecado }, u.TipoTecnologia)
                                    }).ToList();

                            var deprecado = deprecadoList.Where(x => estadoTecIntIdsV.Contains(x.EstadoId)).Select(x => x.EstadoStr).ToList();

                            var restringidoList = (from u in tecnologia
                                        join u2 in tipotecnologia on u.TipoTecnologia equals u2.TipoId
                                        where (u.EstadoId == (int)ETecnologiaEstadoEstandar.Restringido)
                                            && (string.IsNullOrEmpty(_estadoTecIds) || estadoTecIntIds.Contains(u.EstadoId))
                                            && u.TipoTecnologia == item.TipoTecnologiaId
                                            && u.SubdominioId == item.SubdominioId
                                        select new
                                        {
                                            EstadoStr = !u.FechaCalculada.HasValue ?
                                                Utilitarios.DevolverTecnologiaEstandarStr(u.Url, u.CodigoProducto, u.Nombre, "restringidoClass", u2.FlagMostrarEstado, item.EstadoStr, u.Id)
                                                : GetSemaforoTecnologiaClass(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, u.Url, u.CodigoProducto, u.Nombre, u2.FlagMostrarEstado, item.EstadoStr, u.Id, new string[] { "restringidoPorVencer", "restringidoClass" }, u.TipoTecnologia),
                                            EstadoId = !u.FechaCalculada.HasValue ?
                                                (int)ETecnologiaEstadoEstandar.Restringido
                                                : GetEstadoTecnologiaId(u.FechaCalculada.Value, fechaActual, u.CantidadMeses1, new int[] { (int)ETecnologiaEstadoEstandar.RestringidoPorVencer, (int)ETecnologiaEstadoEstandar.Restringido }, u.TipoTecnologia)
                                        }).ToList();

                            var restringido = restringidoList.Where(x => estadoTecIntIdsV.Contains(x.EstadoId)).Select(x => x.EstadoStr).ToList();

                            var obsoleto = (from u in tecnologia
                                    join u2 in tipotecnologia on u.TipoTecnologia equals u2.TipoId
                                    where u.EstadoId == (int)ETecnologiaEstadoEstandar.Obsoleto
                                        && (string.IsNullOrEmpty(_estadoTecIds) || estadoTecIntIds.Contains(u.EstadoId))
                                        && u.TipoTecnologia == item.TipoTecnologiaId
                                        && u.SubdominioId == item.SubdominioId
                                    select
                                        Utilitarios.DevolverTecnologiaEstandarStr(u.Url, u.CodigoProducto, u.Nombre, "obsoletoClass", u2.FlagMostrarEstado, u.TipoTecnologiaToString, u.Id)
                                    ).ToList();

                            item.TecnologiaVigenteStr = string.Join("<br/>", vigente);
                            item.TecnologiaDeprecadoStr = string.Join("<br/>", deprecado);
                            item.TecnologiaRestringidoStr = string.Join("<br/>", restringido);
                            item.TecnologiaObsoletoStr = string.Join("<br/>", obsoleto);
                        }
                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaEstandar()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiaEstandar()"
                    , new object[] { null });
            }
        }

        public override TecnologiaAutocomplete GetTecnologiaById(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var item = (from u in ctx.Tecnologia
                                    join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                    join d in ctx.Dominio on s.DominioId equals d.DominioId
                                    join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                    where u.Activo && s.Activo && d.Activo && t.Activo
                                    && u.TecnologiaId == Id
                                    orderby u.Nombre
                                    select new TecnologiaAutocomplete()
                                    {
                                        Id = u.TecnologiaId,
                                        Dominio = d.Nombre,
                                        Subdominio = s.Nombre,
                                        FechaFinSoporte = u.FechaFinSoporte,
                                        TipoTecnologia = t.Nombre
                                    }).FirstOrDefault();

                        return item;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaAutocomplete GetTecnologiaById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaAutocomplete GetTecnologiaById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteRelacion> GetTecnologiaEstandarByClaveTecnologia(string filtro, bool? getAll = false)
        {
            try
            {
                //var subdominioIntIds = new List<int>();
                //if (!string.IsNullOrEmpty(subdominioIds))
                //    subdominioIntIds = subdominioIds.Split('|').Select(x => int.Parse(x)).ToList();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       //join f in ctx.Familia on u.FamiliaId equals f.FamiliaId
                                       where u.Activo && s.Activo && d.Activo //&& f.Activo
                                       && (getAll.Value || u.FlagSiteEstandar.Value)
                                       && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                       //&& (string.IsNullOrEmpty(subdominioIds) || subdominioIntIds.Contains(u.SubdominioId))
                                       //&& u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new CustomAutocompleteRelacion()
                                       {
                                           Id = u.TecnologiaId.ToString(),
                                           Descripcion = u.ClaveTecnologia,
                                           value = u.ClaveTecnologia,
                                           TipoTecnologia = t.Nombre,
                                           FechaFinSoporte = u.FechaFinSoporte,
                                           Dominio = d.Nombre,
                                           Subdominio = s.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditNewTecnologia(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 0;
            var CURRENT_DATE = DateTime.Now;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.Id == 0)
                        {
                            var entidad = new Tecnologia()
                            {
                                ProductoId = objeto.ProductoId,
                                Producto = objeto.Producto == null ? null : new Producto
                                {
                                    Fabricante = objeto.Producto.Fabricante,
                                    Nombre = objeto.Producto.Nombre,
                                    Descripcion = objeto.Producto.Descripcion,
                                    DominioId = objeto.Producto.DominioId,
                                    SubDominioId = objeto.Producto.SubDominioId,
                                    TipoProductoId = objeto.Producto.TipoProductoId,
                                    Codigo = objeto.Producto.Codigo,
                                    TribuCoeDisplayName = objeto.Producto.TribuCoeDisplayName,
                                    TribuCoeId = objeto.Producto.TribuCoeId,
                                    SquadDisplayName = objeto.Producto.SquadDisplayName,
                                    SquadId = objeto.Producto.SquadId,
                                    OwnerDisplayName = objeto.Producto.OwnerDisplayName,
                                    OwnerId = objeto.Producto.OwnerId,
                                    OwnerMatricula = objeto.Producto.OwnerMatricula,
                                    GrupoTicketRemedyNombre = objeto.Producto.GrupoTicketRemedyNombre,
                                    GrupoTicketRemedyId = objeto.Producto.GrupoTicketRemedyId,
                                    EsquemaLicenciamientoSuscripcionId = objeto.Producto.EsquemaLicenciamientoSuscripcionId,
                                    FlagActivo = true,
                                    CreadoPor = objeto.UsuarioCreacion,
                                    FechaCreacion = DateTime.Now,
                                    EquipoAprovisionamiento = objeto.EquipoAprovisionamiento,
                                    EquipoAdmContacto = objeto.EqAdmContacto
                                },
                                Fabricante = objeto.Fabricante,
                                Nombre = objeto.Nombre,
                                Versiones = objeto.Versiones,
                                ClaveTecnologia = objeto.ClaveTecnologia,
                                Descripcion = objeto.Descripcion,
                                FlagSiteEstandar = objeto.FlagSiteEstandar,
                                FlagTieneEquivalencias = objeto.FlagTieneEquivalencias,
                                MotivoId = objeto.MotivoId,
                                TipoTecnologia = objeto.TipoTecnologiaId,
                                CodigoProducto = objeto.CodigoProducto,
                                AutomatizacionImplementadaId = objeto.AutomatizacionImplementadaId,
                                RevisionSeguridadId = objeto.RevisionSeguridadId,
                                RevisionSeguridadDescripcion = objeto.RevisionSeguridadDescripcion,
                                FlagFechaFinSoporte = objeto.FlagFechaFinSoporte,
                                FuenteId = objeto.Fuente,
                                FechaCalculoTec = objeto.FechaCalculoTec,
                                FechaCalculo = objeto.FechaCalculoBase,
                                FechaLanzamiento = objeto.FechaLanzamiento,
                                FechaExtendida = objeto.FechaExtendida,
                                FechaFinSoporte = objeto.FechaFinSoporte,
                                FechaAcordada = objeto.FechaAcordada,
                                TipoFechaInterna = objeto.TipoFechaInterna,
                                ComentariosFechaFin = objeto.ComentariosFechaFin,
                                SustentoMotivo = objeto.SustentoMotivo,
                                SustentoUrl = objeto.SustentoUrl,
                                UrlConfluenceId = objeto.UrlConfluenceId,
                                UrlConfluence = objeto.UrlConfluence,
                                CasoUsoArchivo = objeto.CasoUsoArchivo,
                                CasoUso = objeto.CasoUso,
                                Aplica = objeto.Aplica,
                                CompatibilidadSOId = objeto.CompatibilidadSOId,
                                CompatibilidadCloudId = objeto.CompatibilidadCloudId,
                                Requisitos = objeto.Requisitos,
                                Existencia = objeto.Existencia,
                                Riesgo = objeto.Riesgo,
                                Facilidad = objeto.Facilidad,
                                Vulnerabilidad = objeto.Vulnerabilidad,


                                EstadoTecnologia = (int)EstadoTecnologia.Aprobado,
                                //EstadoId = !(objeto.FlagFechaFinSoporte ?? false) ? (objeto.Fuente ?? -1) != (int)Fuente.Manual ? (int)ETecnologiaEstado.Obsoleto : (int)ETecnologiaEstado.Vigente : objeto.FechaCalculoTec == null ? (int)ETecnologiaEstado.Obsoleto : (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida && objeto.FechaExtendida.Value >= DateTime.Now) || (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte && objeto.FechaFinSoporte.Value >= DateTime.Now) || (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna && objeto.FechaAcordada.Value >= DateTime.Now) ? (int)ETecnologiaEstado.Vigente : (int)ETecnologiaEstado.Obsoleto,
                                //EstadoId = (int)EEstadoObsolescenciaProducto.Vigente,
                                Activo = true,
                                FlagEliminado = false,
                                //Activo = objeto.Activo,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                //Fin tab 1
                                SubdominioId = objeto.SubdominioId,
                                RoadmapOpcional = objeto.RoadmapOpcional,
                                Referencias = objeto.Referencias,
                                PlanTransConocimiento = objeto.PlanTransConocimiento,
                                EsqMonitoreo = objeto.EsqMonitoreo,
                                EsqPatchManagement = objeto.EsqPatchManagement,
                                //Fin tab 2
                                DuenoId = objeto.Dueno,
                                EqAdmContacto = objeto.EqAdmContacto,
                                GrupoSoporteRemedy = objeto.GrupoSoporteRemedy,
                                ConfArqSegId = objeto.ConfArqSeg,
                                ConfArqTecId = objeto.ConfArqTec,
                                EncargRenContractual = objeto.EncargRenContractual,
                                EsqLicenciamiento = objeto.EsqLicenciamiento,
                                SoporteEmpresarial = objeto.SoporteEmpresarial,
                                EquipoAprovisionamiento = objeto.EquipoAprovisionamiento,
                                //Fin tab 3
                            };

                            ctx.Tecnologia.Add(entidad);
                            ctx.SaveChanges();

                            if (objeto.ListAutorizadores != null)
                            {
                                foreach (var item in objeto.ListAutorizadores)
                                {
                                    var autorizador = new TecnologiaAutorizador
                                    {
                                        TecnologiaId = entidad.TecnologiaId,
                                        AutorizadorId = item.AutorizadorId,
                                        Matricula = item.Matricula,
                                        Nombres = item.Nombres,
                                        FlagActivo = true,
                                        FlagEliminado = false,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = DateTime.Now
                                    };

                                    ctx.TecnologiaAutorizador.Add(autorizador);
                                    ctx.SaveChanges();
                                }
                            }

                            if (objeto.ListArquetipo != null)
                            {
                                foreach (var item in objeto.ListArquetipo)
                                {
                                    var arquetipo = new TecnologiaArquetipo
                                    {
                                        TecnologiaId = entidad.TecnologiaId,
                                        ArquetipoId = item.Id,
                                        FlagActivo = true,
                                        FlagEliminado = false,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = DateTime.Now
                                    };

                                    ctx.TecnologiaArquetipo.Add(arquetipo);
                                    ctx.SaveChanges();
                                }
                            }

                            if (objeto.ListAplicaciones != null && objeto.TipoTecnologiaId == (int)ETipo.EstandarRestringido)
                            {
                                foreach (var item in objeto.ListAplicaciones)
                                {
                                    var aplicacion = new TecnologiaAplicacion
                                    {
                                        TecnologiaId = entidad.TecnologiaId,
                                        AplicacionId = item.AplicacionId,
                                        FlagActivo = true,
                                        FlagEliminado = false,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = DateTime.Now
                                    };

                                    ctx.TecnologiaAplicacion.Add(aplicacion);
                                    ctx.SaveChanges();
                                }
                            }
                            else
                            {
                                var aplicacionLista = ctx.TecnologiaAplicacion.Where(x => x.TecnologiaId == entidad.TecnologiaId && x.FlagActivo && !x.FlagEliminado).ToList();

                                foreach (var item in aplicacionLista)
                                {
                                    item.FlagActivo = false;
                                    item.FlagEliminado = true;
                                    item.ModificadoPor = objeto.UsuarioModificacion;
                                    item.FechaModificacion = DateTime.Now;

                                    ctx.SaveChanges();
                                }
                            }

                            if (objeto.ListEquivalencias != null)
                            {

                                foreach (var item in objeto.ListEquivalencias)
                                {
                                    var equivalencia = new TecnologiaEquivalencia
                                    {
                                        TecnologiaId = entidad.TecnologiaId,
                                        Nombre = item.Nombre,
                                        FlagActivo = true,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = DateTime.Now
                                    };

                                    ctx.TecnologiaEquivalencia.Add(equivalencia);
                                    ctx.SaveChanges();
                                }
                            }

                            ID = entidad.TecnologiaId;
                            registroNuevo = true;
                        }
                        else
                        {
                            var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                            if (entidad != null)
                            {
                                entidad.ProductoId = objeto.ProductoId;
                                if (objeto.Producto != null)
                                {
                                    entidad.Producto = new Producto();
                                    entidad.Producto.Fabricante = objeto.Producto.Fabricante;
                                    entidad.Producto.Nombre = objeto.Producto.Nombre;
                                    entidad.Producto.DominioId = objeto.Producto.DominioId;
                                    entidad.Producto.SubDominioId = objeto.Producto.SubDominioId;
                                    entidad.Producto.TipoProductoId = objeto.Producto.TipoProductoId;
                                    entidad.Producto.Codigo = objeto.Producto.Codigo;
                                    entidad.Producto.TribuCoeDisplayName = objeto.Producto.TribuCoeDisplayName;
                                    entidad.Producto.TribuCoeId = objeto.Producto.TribuCoeId;
                                    entidad.Producto.SquadDisplayName = objeto.Producto.SquadDisplayName;
                                    entidad.Producto.SquadId = objeto.Producto.SquadId;
                                    entidad.Producto.OwnerDisplayName = objeto.Producto.OwnerDisplayName;
                                    entidad.Producto.OwnerId = objeto.Producto.OwnerId;
                                    entidad.Producto.OwnerMatricula = objeto.Producto.OwnerMatricula;
                                    entidad.Producto.GrupoTicketRemedyNombre = objeto.Producto.GrupoTicketRemedyNombre;
                                    entidad.Producto.GrupoTicketRemedyId = objeto.Producto.GrupoTicketRemedyId;
                                    entidad.Producto.EsquemaLicenciamientoSuscripcionId = objeto.Producto.EsquemaLicenciamientoSuscripcionId;
                                    entidad.Producto.CreadoPor = objeto.UsuarioCreacion;
                                    entidad.Producto.FechaCreacion = DateTime.Now;
                                    entidad.Producto.EquipoAprovisionamiento = objeto.EquipoAprovisionamiento;
                                    entidad.Producto.EquipoAdmContacto = objeto.EqAdmContacto;
                                }
                                entidad.Fabricante = objeto.Fabricante;
                                entidad.Nombre = objeto.Nombre;
                                entidad.Versiones = objeto.Versiones;
                                entidad.ClaveTecnologia = objeto.ClaveTecnologia;
                                entidad.Descripcion = objeto.Descripcion;
                                entidad.FlagSiteEstandar = objeto.FlagSiteEstandar;
                                entidad.FlagTieneEquivalencias = objeto.FlagTieneEquivalencias;
                                entidad.MotivoId = objeto.MotivoId;
                                entidad.TipoTecnologia = objeto.TipoTecnologiaId;
                                entidad.CodigoProducto = objeto.CodigoProducto;
                                entidad.AutomatizacionImplementadaId = objeto.AutomatizacionImplementadaId;
                                entidad.RevisionSeguridadId = objeto.RevisionSeguridadId;
                                entidad.RevisionSeguridadDescripcion = objeto.RevisionSeguridadDescripcion;
                                entidad.FlagFechaFinSoporte = objeto.FlagFechaFinSoporte;
                                entidad.FuenteId = objeto.Fuente;
                                //entidad.EstadoId = !(objeto.FlagFechaFinSoporte ?? false) ? (int)ETecnologiaEstado.Vigente : objeto.FechaCalculoTec == null ? (int)ETecnologiaEstado.Obsoleto : (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida && objeto.FechaExtendida.Value >= DateTime.Now) || (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte && objeto.FechaFinSoporte.Value >= DateTime.Now) || (objeto.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna && objeto.FechaAcordada.Value >= DateTime.Now) ? (int)ETecnologiaEstado.Vigente : (int)ETecnologiaEstado.Obsoleto;
                                entidad.FechaCalculoTec = objeto.FechaCalculoTec;
                                entidad.FechaLanzamiento = objeto.FechaLanzamiento;
                                entidad.FechaExtendida = objeto.FechaExtendida;
                                entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                                entidad.FechaAcordada = objeto.FechaAcordada;
                                entidad.ComentariosFechaFin = objeto.ComentariosFechaFin;
                                entidad.SustentoMotivo = objeto.SustentoMotivo;
                                entidad.SustentoUrl = objeto.SustentoUrl;
                                entidad.UrlConfluenceId = objeto.UrlConfluenceId;
                                entidad.UrlConfluence = objeto.UrlConfluence;
                                entidad.CasoUsoArchivo = objeto.CasoUsoArchivo;
                                entidad.CasoUso = objeto.CasoUso;
                                entidad.Aplica = objeto.Aplica;
                                entidad.CompatibilidadSOId = objeto.CompatibilidadSOId;
                                entidad.CompatibilidadCloudId = objeto.CompatibilidadCloudId;
                                entidad.Requisitos = objeto.Requisitos;
                                entidad.Existencia = objeto.Existencia;
                                entidad.Riesgo = objeto.Riesgo;
                                entidad.Facilidad = objeto.Facilidad;
                                entidad.Vulnerabilidad = objeto.Vulnerabilidad;


                                entidad.UsuarioModificacion = objeto.UsuarioCreacion;
                                entidad.FechaModificacion = DateTime.Now;
                                //Fin tab 1
                                entidad.SubdominioId = objeto.SubdominioId;
                                entidad.RoadmapOpcional = objeto.RoadmapOpcional;
                                entidad.Referencias = objeto.Referencias;
                                entidad.PlanTransConocimiento = objeto.PlanTransConocimiento;
                                entidad.EsqMonitoreo = objeto.EsqMonitoreo;
                                entidad.EsqPatchManagement = objeto.EsqPatchManagement;
                                //Fin tab 2
                                entidad.DuenoId = objeto.Dueno;
                                entidad.EqAdmContacto = objeto.EqAdmContacto;
                                entidad.GrupoSoporteRemedy = objeto.GrupoSoporteRemedy;
                                entidad.ConfArqSegId = objeto.ConfArqSeg;
                                entidad.ConfArqTecId = objeto.ConfArqTec;
                                entidad.EncargRenContractual = objeto.EncargRenContractual;
                                entidad.EsqLicenciamiento = objeto.EsqLicenciamiento;
                                entidad.SoporteEmpresarial = objeto.SoporteEmpresarial;
                                entidad.EquipoAprovisionamiento = objeto.EquipoAprovisionamiento;
                                //Fin tab 3

                                ctx.SaveChanges();
                                //if (objeto.ItemsRemoveAutId != null)
                                //{
                                //    foreach (int id in objeto.ItemsRemoveAutId)
                                //    {
                                //        var item = ctx.TecnologiaAutorizador.FirstOrDefault(x => x.TecnologiaAutorizadorId == id);
                                //        if (item != null)
                                //        {
                                //            item.FlagActivo = false;
                                //            item.FlagEliminado = true;
                                //            item.ModificadoPor = objeto.UsuarioModificacion;
                                //            item.FechaModificacion = DateTime.Now;
                                //        };
                                //        ctx.SaveChanges();
                                //    }
                                //}

                                //if (objeto.ListAutorizadores != null)
                                //{
                                //    foreach (var item in objeto.ListAutorizadores)
                                //    {
                                //        var autorizador = new TecnologiaAutorizador
                                //        {
                                //            TecnologiaId = entidad.TecnologiaId,
                                //            AutorizadorId = item.AutorizadorId,
                                //            Matricula = item.Matricula,
                                //            Nombres = item.Nombres,
                                //            FlagActivo = true,
                                //            FlagEliminado = false,
                                //            CreadoPor = objeto.UsuarioCreacion,
                                //            FechaCreacion = DateTime.Now
                                //        };

                                //        ctx.TecnologiaAutorizador.Add(autorizador);
                                //        ctx.SaveChanges();
                                //    }
                                //}

                                //if (objeto.ItemsRemoveArqId != null)
                                //{
                                //    foreach (int id in objeto.ItemsRemoveArqId)
                                //    {
                                //        var item = ctx.TecnologiaArquetipo.FirstOrDefault(x => x.TecnologiaArquetipoId == id);
                                //        if (item != null)
                                //        {
                                //            item.FlagActivo = false;
                                //            item.FlagEliminado = true;
                                //            item.ModificadoPor = objeto.UsuarioModificacion;
                                //            item.FechaModificacion = DateTime.Now;
                                //        };
                                //        ctx.SaveChanges();
                                //    }
                                //}

                                //if (objeto.ListArquetipo != null)
                                //{
                                //    foreach (var item in objeto.ListArquetipo)
                                //    {
                                //        var arquetipo = new TecnologiaArquetipo
                                //        {
                                //            TecnologiaId = entidad.TecnologiaId,
                                //            ArquetipoId = item.Id,
                                //            FlagActivo = true,
                                //            FlagEliminado = false,
                                //            CreadoPor = objeto.UsuarioCreacion,
                                //            FechaCreacion = DateTime.Now
                                //        };

                                //        ctx.TecnologiaArquetipo.Add(arquetipo);
                                //        ctx.SaveChanges();
                                //    }
                                //}

                                if (objeto.ItemsRemoveAppId != null)
                                {
                                    foreach (int id in objeto.ItemsRemoveAppId)
                                    {
                                        var item = ctx.TecnologiaAplicacion.FirstOrDefault(x => x.TecnologiaAplicacionId == id);
                                        if (item != null)
                                        {
                                            item.FlagActivo = false;
                                            item.FlagEliminado = true;
                                            item.ModificadoPor = objeto.UsuarioModificacion;
                                            item.FechaModificacion = DateTime.Now;
                                        };
                                        ctx.SaveChanges();
                                    }
                                }

                                if (objeto.ListAplicaciones != null && objeto.TipoTecnologiaId == (int)ETipo.EstandarRestringido)
                                {
                                    foreach (var item in objeto.ListAplicaciones)
                                    {
                                        var aplicacion = new TecnologiaAplicacion
                                        {
                                            TecnologiaId = entidad.TecnologiaId,
                                            AplicacionId = item.AplicacionId,
                                            FlagActivo = true,
                                            FlagEliminado = false,
                                            CreadoPor = objeto.UsuarioCreacion,
                                            FechaCreacion = DateTime.Now
                                        };

                                        ctx.TecnologiaAplicacion.Add(aplicacion);
                                        ctx.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var aplicacionLista = ctx.TecnologiaAplicacion.Where(x => x.TecnologiaId == entidad.TecnologiaId && x.FlagActivo && !x.FlagEliminado).ToList();

                                    foreach (var item in aplicacionLista)
                                    {
                                        item.FlagActivo = false;
                                        item.FlagEliminado = true;
                                        item.ModificadoPor = objeto.UsuarioModificacion;
                                        item.FechaModificacion = DateTime.Now;

                                        ctx.SaveChanges();
                                    }
                                }



                                if (objeto.ListEquivalencias != null)
                                {

                                    foreach (var item in objeto.ListEquivalencias)
                                    {
                                        var equivalencia = new TecnologiaEquivalencia
                                        {
                                            TecnologiaId = entidad.TecnologiaId,
                                            Nombre = item.Nombre,
                                            FlagActivo = true,
                                            CreadoPor = objeto.UsuarioCreacion,
                                            FechaCreacion = DateTime.Now
                                        };

                                        ctx.TecnologiaEquivalencia.Add(equivalencia);
                                        ctx.SaveChanges();
                                    }
                                }

                                if (objeto.ItemsRemoveEqTecId != null)
                                {
                                    foreach (int id in objeto.ItemsRemoveEqTecId)
                                    {
                                        var item = ctx.TecnologiaEquivalencia.FirstOrDefault(x => x.TecnologiaEquivalenciaId == id);
                                        if (item != null)
                                        {
                                            item.FlagActivo = false;
                                            item.ModificadoPor = objeto.UsuarioModificacion;
                                            item.FechaModificacion = DateTime.Now;
                                        };
                                        ctx.SaveChanges();
                                    }
                                }

                                var archivo = (from u in ctx.ArchivosCVT
                                               where u.ArchivoId == objeto.ArchivoId
                                               select u).FirstOrDefault();
                                if (archivo != null)
                                {
                                    archivo.Activo = false;
                                    archivo.FechaModificacion = DateTime.Now;
                                    archivo.UsuarioModificacion = objeto.UsuarioModificacion;
                                    ctx.SaveChanges();
                                    //ID = entidad.ArchivoId;
                                }

                                ID = entidad.TecnologiaId;
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

            try
            {
                if (registroNuevo)
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }
                else
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }

                if ((objeto.EstadoId != (int)ETecnologiaEstado.Obsoleto && objeto.EstadoId.HasValue)
                                   && objeto.EstadoTecnologia == (int)EstadoTecnologia.Aprobado && objeto.FlagUnicaVigente == true)
                {
                    //this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.VigentePorVencer);
                    this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.Deprecado);
                }

                return ID;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetNewTec(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                //casoUso = "";
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var parametroMeses1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                        int cantidadMeses1 = int.Parse(parametroMeses1);

                        DateTime fechaActual = DateTime.Now;
                        DateTime fechaAgregada = fechaActual.AddMonths(cantidadMeses1);

                        var tecnologiaIds = new List<int>();

                        //if (!string.IsNullOrEmpty(equipo))
                        //{
                        //    tecnologiaIds = (from et in ctx.EquipoTecnologia
                        //                     join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                        //                     where et.FlagActivo && e.FlagActivo
                        //                     && (e.Nombre.ToUpper().Contains(equipo.ToUpper())
                        //                     /*|| string.IsNullOrEmpty(equipo)*/)
                        //                     select et.TecnologiaId).ToList();
                        //}

                        var tecEquivalenciaIds = (from e in ctx.TecnologiaEquivalencia
                                                  where e.FlagActivo && e.Nombre.ToUpper().Contains(filtro.ToUpper())
                                                  select e.TecnologiaId
                                                ).ToList();

                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId// into lj1
                                         join a in ctx.Producto on u.ProductoId equals a.ProductoId into lja
                                         from a in lja.DefaultIfEmpty()
                                         join b in ctx.Tipo on a.TipoProductoId equals b.TipoId into ljb
                                         from b in ljb.DefaultIfEmpty()
                                         join s in ctx.Subdominio on a.SubDominioId equals s.SubdominioId into ljs
                                         from s in ljs.DefaultIfEmpty()
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId into ljd
                                         from d in ljd.DefaultIfEmpty()
                                         join e in ctx.Motivo on u.MotivoId equals e.MotivoId into lje
                                         from e in lje.DefaultIfEmpty()
                                         //let fechaCalculo = !u.FechaCalculoTec.HasValue ? null : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : u.FechaCalculoTec.Value == (int)FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : null
                                         //let estadoEstandar = !fechaCalculo.HasValue ? (int)ETecnologiaEstado.Vigente : ((fechaCalculo.Value < fechaActual) ? (int)ETecnologiaEstado.Obsoleto : ((fechaCalculo.Value < fechaAgregada) ? (int)ETecnologiaEstado.VigentePorVencer : (int)ETecnologiaEstado.Vigente))
                                         let fechaCalculada = !u.FechaCalculoTec.HasValue ? null : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : null
                                         let estado = b.Nombre.ToLower().Contains("deprecado") ? (int)ETecnologiaEstado.Deprecado : ((u.FlagFechaFinSoporte ?? false) ? (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : ((fechaCalculada.Value < fechaActual) ? (int)ETecnologiaEstado.Obsoleto : (int)ETecnologiaEstado.Vigente))) : (int)ETecnologiaEstado.Vigente)
                                         where u.Activo == true
                                         &&
                                         (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro)
                                         || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                         || tecEquivalenciaIds.Contains(u.TecnologiaId))

                                         && (productoId == null || u.ProductoId == productoId)
                                         && (domIds.Count == 0 || domIds.Contains(s.DominioId))
                                         && (subdomIds.Count == 0 || subdomIds.Contains(a.SubDominioId))
                                         
                                         && (estObsIds.Count == 0 || estObsIds.Contains(u.EstadoId.HasValue ? u.EstadoId.Value : 0))
                                         //&& (estObsIds.Count == 0 || estObsIds.Contains(estado))
                                         && (tipoTecIds.Count == 0 || tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))
                                         //&& (string.IsNullOrEmpty(famId) || f == null || f.Nombre.ToUpper().Equals(famId.ToUpper()))
                                         //&& (fecId == -1 || u.FechaFinSoporte.HasValue == (fecId == 1))
                                         && (u.Aplica.ToUpper().Contains(aplica.ToUpper()) || string.IsNullOrEmpty(aplica))
                                         && ((u.CodigoProducto ?? (a != null ? (a.Codigo ?? "") : "")).ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))
                                         && (u.DuenoId.ToUpper().Contains(dueno.ToUpper()) || string.IsNullOrEmpty(dueno))
                                         //&& (string.IsNullOrEmpty(equipo) || tecnologiaIds.Contains(u.TecnologiaId))

                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             TipoTecnologiaId = u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 4,
                                             Fabricante = a.Fabricante,
                                             Nombre = u.Nombre,
                                             Versiones = u.Versiones,
                                             Descripcion = u.Descripcion,
                                             MotivoId = u.MotivoId ?? 0,
                                             MotivoStr = e == null ? null : e.Nombre,
                                             AutomatizacionImplementadaId = u.AutomatizacionImplementadaId,
                                             RevisionSeguridadId = u.RevisionSeguridadId,
                                             RevisionSeguridadDescripcion = u.RevisionSeguridadDescripcion,
                                             FechaLanzamiento = u.FechaLanzamiento,
                                             Fuente = u.FuenteId,
                                             ComentariosFechaFin = u.ComentariosFechaFin,
                                             SustentoMotivo = u.SustentoMotivo,
                                             SustentoUrl = u.SustentoUrl,
                                             CasoUso = u.CasoUso,
                                             Aplica = u.Aplica,
                                             CompatibilidadSO = u.CompatibilidadSOId,
                                             CompatibilidadCloud = u.CompatibilidadCloudId,
                                             Requisitos = u.Requisitos,
                                             Existencia = u.Existencia,
                                             Riesgo = u.Riesgo,
                                             Facilidad = u.Facilidad,
                                             Vulnerabilidad = u.Vulnerabilidad,
                                             RoadmapOpcional = u.RoadmapOpcional,
                                             Referencias = u.Referencias,
                                             PlanTransConocimiento = u.PlanTransConocimiento,
                                             EsqMonitoreo = u.EsqMonitoreo,
                                             EsqPatchManagement = u.EsqPatchManagement,
                                             ConfArqSeg = u.ConfArqSegId,
                                             ConfArqTec = u.ConfArqTecId,
                                             EsqLicenciamiento = u.EsqLicenciamiento,
                                             EquipoAprovisionamiento = a.EquipoAprovisionamiento,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Tipo = t.Nombre,
                                             TipoProductoStr = b.Nombre,
                                             Estado = u.EstadoTecnologia,
                                             FechaAprobacion = u.FechaAprobacion,
                                             UsuarioAprobacion = u.UsuarioAprobacion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             //TipoId = u.TipoId,
                                             EstadoId = u.EstadoId,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FechaAcordada = u.FechaAcordada,
                                             FechaExtendida = u.FechaExtendida,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             FlagSiteEstandar = u.FlagSiteEstandar,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                             FlagTieneEquivalencias = (from x in ctx.TecnologiaEquivalencia
                                                                       where x.FlagActivo && x.TecnologiaId == u.TecnologiaId
                                                                       select true).FirstOrDefault() == true,
                                             ProductoNombre = a == null ? "" : a.Fabricante + " " + a.Nombre,
                                             TribuCoeStr = a == null ? "" : a.TribuCoeDisplayName,
                                             Dueno = u.DuenoId,
                                             CodigoProducto = a.Codigo,
                                             GrupoSoporteRemedy = a.GrupoTicketRemedyNombre,
                                             EqAdmContacto = a.EquipoAdmContacto,
                                             UrlConfluenceId = u.UrlConfluenceId,
                                             UrlConfluence = u.UrlConfluence,
                                             TipoFechaInterna = u.TipoFechaInterna,
                                             CantidadMeses1 = cantidadMeses1
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        //if(resultado != null)
                        //{
                        //    foreach(var item in resultado)
                        //    {
                        //        var itemListaAplicacion = (from a in ctx.TecnologiaAplicacion
                        //                                   join b in ctx.Aplicacion on a.AplicacionId equals b.AplicacionId
                        //                                   where a.TecnologiaId == item.Id
                        //                                   select b).ToList();

                        //        item.ListaAplicacionStr = string.Join(", ", itemListaAplicacion.Select(x => x.CodigoAPT).ToArray());
                        //    }
                        //}

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetNewTecSP(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string tribuCoeStr = null, string squadStr = null)
        {
            try
            {
                List<TecnologiaG> registros = new List<TecnologiaG>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_TECNOLOGIA_LISTAR_EXPORTAR]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@filtro", filtro == null ? DBNull.Value : (object)filtro);
                        //comando.Parameters.AddWithValue("@equipo", equipo == null ? DBNull.Value : (object)equipo);
                        comando.Parameters.AddWithValue("@productoId", productoId == null ? DBNull.Value : (object)productoId);
                        comando.Parameters.AddWithValue("@domIds", domIds == null ? DBNull.Value : domIds.Count == 0 ? DBNull.Value : (object)string.Join(",", domIds.ToArray()));
                        comando.Parameters.AddWithValue("@subdomIds", subdomIds == null ? DBNull.Value : subdomIds.Count == 0 ? DBNull.Value : (object)string.Join(",", subdomIds.ToArray()));
                        comando.Parameters.AddWithValue("@estObsIds", estObsIds == null ? DBNull.Value : estObsIds.Count == 0 ? DBNull.Value : (object)string.Join(",", estObsIds.ToArray()));
                        comando.Parameters.AddWithValue("@tipoTecIds", tipoTecIds == null ? DBNull.Value : tipoTecIds.Count == 0 ? DBNull.Value : (object)string.Join(",", tipoTecIds.ToArray()));
                        comando.Parameters.AddWithValue("@aplica", aplica == null ? DBNull.Value : (object)aplica);
                        comando.Parameters.AddWithValue("@codigo ", codigo == null ? DBNull.Value : (object)codigo);
                        comando.Parameters.AddWithValue("@dueno", dueno == null ? DBNull.Value : (object)dueno);
                        comando.Parameters.AddWithValue("@tribuCoeStr", tribuCoeStr == null ? DBNull.Value : (object)tribuCoeStr);
                        comando.Parameters.AddWithValue("@squadStr", squadStr == null ? DBNull.Value : (object)squadStr);
                        comando.Parameters.AddWithValue("@withApps", withApps);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);

                        var dr = comando.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new TecnologiaG();
                                item.Id = dr.GetData<int>("Id");
                                item.TipoTecnologiaId = dr.GetData<int>("TipoTecnologiaId");
                                item.Fabricante = dr.GetData<string>("Fabricante");
                                item.Nombre = dr.GetData<string>("Nombre");
                                //item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Versiones = dr.GetData<string>("Versiones");
                                item.Descripcion = dr.GetData<string>("Descripcion");
                                item.MotivoId = dr.GetData<int>("MotivoId");
                                item.MotivoStr = dr.GetData<string>("MotivoStr");
                                item.AutomatizacionImplementadaId = dr.GetData<int>("AutomatizacionImplementadaId");
                                item.RevisionSeguridadId = dr.GetData<int>("RevisionSeguridadId");
                                item.RevisionSeguridadDescripcion = dr.GetData<string>("RevisionSeguridadDescripcion");
                                item.FechaLanzamiento = dr.GetData<DateTime?>("FechaLanzamiento");
                                item.Fuente = dr.GetData<int>("Fuente");
                                item.ComentariosFechaFin = dr.GetData<string>("ComentariosFechaFin");
                                item.SustentoMotivo = dr.GetData<string>("SustentoMotivo");
                                item.SustentoUrl = dr.GetData<string>("SustentoUrl");
                                item.CasoUso = dr.GetData<string>("CasoUso");
                                item.Aplica = dr.GetData<string>("Aplica");
                                item.CompatibilidadSO = dr.GetData<string>("CompatibilidadSO");
                                item.CompatibilidadCloud = dr.GetData<string>("CompatibilidadCloud");
                                item.Requisitos = dr.GetData<string>("Requisitos");
                                item.Existencia = dr.GetData<int>("Existencia");
                                item.Riesgo = dr.GetData<int>("Riesgo");
                                item.Facilidad = dr.GetData<int>("Facilidad");
                                item.Vulnerabilidad = dr.GetData<decimal>("Vulnerabilidad");
                                item.RoadmapOpcional = dr.GetData<string>("RoadmapOpcional");
                                item.Referencias = dr.GetData<string>("Referencias");
                                item.PlanTransConocimiento = dr.GetData<string>("PlanTransConocimiento");
                                item.EsqMonitoreo = dr.GetData<string>("EsqMonitoreo");
                                item.EsqPatchManagement = dr.GetData<string>("EsqPatchManagement");
                                item.ConfArqSeg = dr.GetData<string>("ConfArqSeg");
                                item.ConfArqTec = dr.GetData<string>("ConfArqTec");
                                item.EsqLicenciamiento = dr.GetData<string>("EsqLicenciamiento");
                                item.EquipoAprovisionamiento = dr.GetData<string>("EquipoAprovisionamiento");
                                item.Activo = dr.GetData<bool>("Activo");
                                item.Dominio = dr.GetData<string>("Dominio");
                                item.Subdominio = dr.GetData<string>("Subdominio");
                                item.Tipo = dr.GetData<string>("Tipo");
                                item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Estado = dr.GetData<int>("Estado");
                                item.FechaAprobacion = dr.GetData<DateTime>("FechaAprobacion");
                                item.UsuarioAprobacion = dr.GetData<string>("UsuarioAprobacion");
                                item.ClaveTecnologia = dr.GetData<string>("ClaveTecnologia");
                                item.FechaFinSoporte = dr.GetData<DateTime?>("FechaFinSoporte");
                                item.FechaAcordada = dr.GetData<DateTime?>("FechaAcordada");
                                item.FechaExtendida = dr.GetData<DateTime?>("FechaExtendida");
                                item.FechaCalculoTec = dr.GetData<int>("FechaCalculoTec");
                                item.FlagSiteEstandar = dr.GetData<bool?>("FlagSiteEstandar");
                                item.FlagFechaFinSoporte = dr.GetData<bool?>("FlagFechaFinSoporte");
                                item.FlagTieneEquivalencias = dr.GetData<bool>("FlagTieneEquivalencias");
                                item.ProductoNombre = dr.GetData<string>("ProductoNombre");
                                item.TribuCoeStr = dr.GetData<string>("TribuCoeStr");
                                item.ResponsableTribuCoeStr = dr.GetData<string>("ResponsableTribuCoeStr");
                                item.SquadStr = dr.GetData<string>("SquadStr");
                                item.ResponsableSquadStr = dr.GetData<string>("ResponsableSquadStr");
                                item.Dueno = dr.GetData<string>("Dueno");
                                item.CodigoProducto = dr.GetData<string>("CodigoProducto");
                                item.GrupoSoporteRemedy = dr.GetData<string>("GrupoSoporteRemedy");
                                item.EqAdmContacto = dr.GetData<string>("EqAdmContacto");
                                item.UrlConfluenceId = dr.GetData<int?>("UrlConfluenceId");
                                item.UrlConfluence = dr.GetData<string>("UrlConfluence");
                                item.TipoFechaInterna = dr.GetData<string>("TipoFechaInterna");
                                item.EstadoId = dr.GetData<int>("EstadoId");
                                //item.EstadoId = !(item.FlagFechaFinSoporte ?? false) ? (int)ETecnologiaEstado.Vigente : item.FechaCalculoTec == null ? (int)ETecnologiaEstado.Obsoleto : (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida && item.FechaExtendida.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte && item.FechaFinSoporte.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna && item.FechaAcordada.Value > DateTime.Now) ? (int)ETecnologiaEstado.Vigente : (int)ETecnologiaEstado.Obsoleto;
                                item.UsuarioCreacion = dr.GetData<string>("UsuarioCreacion");
                                item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                item.UsuarioModificacion = dr.GetData<string>("UsuarioModificacion");
                                item.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");
                                item.CantRoles = dr.GetData<int>("CantRoles");
                                item.CantFunciones = dr.GetData<int>("CantFunciones");
                                item.ProductoId = dr.GetData<int>("ProductoId");
                                totalRows = dr.GetData<int>("TotalRows");
                                item.LineamientoBaseSeguridadId = dr.GetData<int?>("LineamientoBaseSeguridadId");
                                item.LineamientoBaseSeguridad = dr.GetData<string>("LineamientoBaseSeguridad");

                                registros.Add(item);
                            }
                        }

                        //var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<TecnologiaG> GetBajasOwnerTecSP(List<int> domIds, List<int> subdomIds, int? productoId, string codigo, List<int> resolucionCambio, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null)
        {
            try
            { 
                List<TecnologiaG> registros = new List<TecnologiaG>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[ReporteCambioBajas_OwnerTecnologia]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@productoId", productoId == null ? DBNull.Value : (object)productoId);
                        comando.Parameters.AddWithValue("@domIds", domIds == null ? DBNull.Value : domIds.Count == 0 ? DBNull.Value : (object)string.Join(",", domIds.ToArray()));
                        comando.Parameters.AddWithValue("@subdomIds", subdomIds == null ? DBNull.Value : subdomIds.Count == 0 ? DBNull.Value : (object)string.Join(",", subdomIds.ToArray()));
                        comando.Parameters.AddWithValue("@codigo ", codigo == null ? DBNull.Value : (object)codigo);
                        comando.Parameters.AddWithValue("@resolucionCambio ", resolucionCambio == null ? DBNull.Value : resolucionCambio.Count == 0 ? DBNull.Value : (object)string.Join(",", resolucionCambio.ToArray()));
                        comando.Parameters.AddWithValue("@tribuCoeStr", responsableTribu == null ? DBNull.Value : (object)responsableTribu);
                        comando.Parameters.AddWithValue("@squadStr", responsableSquad == null ? DBNull.Value : (object)responsableSquad);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new TecnologiaG();
                                item.Id = dr.GetData<int>("Id");
                                item.Dominio = dr.GetData<string>("Dominio");
                                item.Subdominio = dr.GetData<string>("Subdominio");
                                item.CodigoProducto = dr.GetData<string>("CodigoProducto");
                                item.ProductoNombre = dr.GetData<string>("NombreProducto");
                                item.TribuCoeStr = dr.GetData<string>("TribuCoeStrActual");
                                item.ResponsableTribuCoeStr = dr.GetData<string>("ResponsableTribuCoeStrActual");
                                item.SquadStr = dr.GetData<string>("SquadStrActual");
                                item.ResponsableSquadStr = dr.GetData<string>("ResponsableSquadStrActual");
                                item.TribuCoeStrAnterior = dr.GetData<string>("TribuCoeStrAnterior");
                                item.ResponsableTribuCoeStrAnterior = dr.GetData<string>("ResponsableTribuCoeStrAnterior");
                                item.SquadStrAnterior = dr.GetData<string>("SquadStrAnterior");
                                item.ResponsableSquadStrAnterior = dr.GetData<string>("ResponsableSquadStrAnterior");
                                item.TipoCambioSquad = dr.GetData<string>("TipoCambio");
                                item.EstadoResolucion = dr.GetData<string>("EstadoResolucion");
                                totalRows = dr.GetData<int>("TotalRows");

                                registros.Add(item);
                            }
                        }
                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetBajasOwnerTecSP(List<int> domIds, List<int> subdomIds, int? productoId, string codigo, List<int> resolucionCambio, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetBajasOwnerTecSP(List<int> domIds, List<int> subdomIds, int? productoId, string codigo, List<int> resolucionCambio, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiasGUmbrales(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string tribuCoeStr = null, string squadStr = null)
        {
            try
            {
                List<TecnologiaG> registros = new List<TecnologiaG>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_TECNOLOGIA_LISTAR_GUMBRALES]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@filtro", filtro == null ? DBNull.Value : (object)filtro);
                        //comando.Parameters.AddWithValue("@equipo", equipo == null ? DBNull.Value : (object)equipo);
                        comando.Parameters.AddWithValue("@productoId", productoId == null ? DBNull.Value : (object)productoId);
                        comando.Parameters.AddWithValue("@domIds", domIds == null ? DBNull.Value : domIds.Count == 0 ? DBNull.Value : (object)string.Join(",", domIds.ToArray()));
                        comando.Parameters.AddWithValue("@subdomIds", subdomIds == null ? DBNull.Value : subdomIds.Count == 0 ? DBNull.Value : (object)string.Join(",", subdomIds.ToArray()));
                        comando.Parameters.AddWithValue("@estObsIds", estObsIds == null ? DBNull.Value : estObsIds.Count == 0 ? DBNull.Value : (object)string.Join(",", estObsIds.ToArray()));
                        comando.Parameters.AddWithValue("@tipoTecIds", tipoTecIds == null ? DBNull.Value : tipoTecIds.Count == 0 ? DBNull.Value : (object)string.Join(",", tipoTecIds.ToArray()));
                        comando.Parameters.AddWithValue("@aplica", aplica == null ? DBNull.Value : (object)aplica);
                        comando.Parameters.AddWithValue("@codigo ", codigo == null ? DBNull.Value : (object)codigo);
                        comando.Parameters.AddWithValue("@dueno", dueno == null ? DBNull.Value : (object)dueno);
                        comando.Parameters.AddWithValue("@tribuCoeStr", tribuCoeStr == null ? DBNull.Value : (object)tribuCoeStr);
                        comando.Parameters.AddWithValue("@squadStr", squadStr == null ? DBNull.Value : (object)squadStr);
                        comando.Parameters.AddWithValue("@withApps", withApps);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);

                        var dr = comando.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new TecnologiaG();
                                item.Id = dr.GetData<int>("Id");
                                item.TipoTecnologiaId = dr.GetData<int>("TipoTecnologiaId");
                                item.Fabricante = dr.GetData<string>("Fabricante");
                                item.Nombre = dr.GetData<string>("Nombre");
                                //item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Versiones = dr.GetData<string>("Versiones");
                                item.Descripcion = dr.GetData<string>("Descripcion");
                                item.MotivoId = dr.GetData<int>("MotivoId");
                                item.MotivoStr = dr.GetData<string>("MotivoStr");
                                item.AutomatizacionImplementadaId = dr.GetData<int>("AutomatizacionImplementadaId");
                                item.RevisionSeguridadId = dr.GetData<int>("RevisionSeguridadId");
                                item.RevisionSeguridadDescripcion = dr.GetData<string>("RevisionSeguridadDescripcion");
                                item.FechaLanzamiento = dr.GetData<DateTime?>("FechaLanzamiento");
                                item.Fuente = dr.GetData<int>("Fuente");
                                item.ComentariosFechaFin = dr.GetData<string>("ComentariosFechaFin");
                                item.SustentoMotivo = dr.GetData<string>("SustentoMotivo");
                                item.SustentoUrl = dr.GetData<string>("SustentoUrl");
                                item.CasoUso = dr.GetData<string>("CasoUso");
                                item.Aplica = dr.GetData<string>("Aplica");
                                item.CompatibilidadSO = dr.GetData<string>("CompatibilidadSO");
                                item.CompatibilidadCloud = dr.GetData<string>("CompatibilidadCloud");
                                item.Requisitos = dr.GetData<string>("Requisitos");
                                item.Existencia = dr.GetData<int>("Existencia");
                                item.Riesgo = dr.GetData<int>("Riesgo");
                                item.Facilidad = dr.GetData<int>("Facilidad");
                                item.Vulnerabilidad = dr.GetData<decimal>("Vulnerabilidad");
                                item.RoadmapOpcional = dr.GetData<string>("RoadmapOpcional");
                                item.Referencias = dr.GetData<string>("Referencias");
                                item.PlanTransConocimiento = dr.GetData<string>("PlanTransConocimiento");
                                item.EsqMonitoreo = dr.GetData<string>("EsqMonitoreo");
                                item.EsqPatchManagement = dr.GetData<string>("EsqPatchManagement");
                                item.ConfArqSeg = dr.GetData<string>("ConfArqSeg");
                                item.ConfArqTec = dr.GetData<string>("ConfArqTec");
                                item.EsqLicenciamiento = dr.GetData<string>("EsqLicenciamiento");
                                item.EquipoAprovisionamiento = dr.GetData<string>("EquipoAprovisionamiento");
                                item.Activo = dr.GetData<bool>("Activo");
                                item.Dominio = dr.GetData<string>("Dominio");
                                item.Subdominio = dr.GetData<string>("Subdominio");
                                item.Tipo = dr.GetData<string>("Tipo");
                                item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Estado = dr.GetData<int>("Estado");
                                item.FechaAprobacion = dr.GetData<DateTime>("FechaAprobacion");
                                item.UsuarioAprobacion = dr.GetData<string>("UsuarioAprobacion");
                                item.ClaveTecnologia = dr.GetData<string>("ClaveTecnologia");
                                item.FechaFinSoporte = dr.GetData<DateTime?>("FechaFinSoporte");
                                item.FechaAcordada = dr.GetData<DateTime?>("FechaAcordada");
                                item.FechaExtendida = dr.GetData<DateTime?>("FechaExtendida");
                                item.FechaCalculoTec = dr.GetData<int>("FechaCalculoTec");
                                item.FlagSiteEstandar = dr.GetData<bool?>("FlagSiteEstandar");
                                item.FlagFechaFinSoporte = dr.GetData<bool?>("FlagFechaFinSoporte");
                                item.FlagTieneEquivalencias = dr.GetData<bool>("FlagTieneEquivalencias");
                                item.ProductoNombre = dr.GetData<string>("ProductoNombre");
                                item.TribuCoeStr = dr.GetData<string>("TribuCoeStr");
                                item.ResponsableTribuCoeStr = dr.GetData<string>("ResponsableTribuCoeStr");
                                item.SquadStr = dr.GetData<string>("SquadStr");
                                item.ResponsableSquadStr = dr.GetData<string>("ResponsableSquadStr");
                                item.Dueno = dr.GetData<string>("Dueno");
                                item.CodigoProducto = dr.GetData<string>("CodigoProducto");
                                item.GrupoSoporteRemedy = dr.GetData<string>("GrupoSoporteRemedy");
                                item.EqAdmContacto = dr.GetData<string>("EqAdmContacto");
                                item.UrlConfluenceId = dr.GetData<int?>("UrlConfluenceId");
                                item.UrlConfluence = dr.GetData<string>("UrlConfluence");
                                item.TipoFechaInterna = dr.GetData<string>("TipoFechaInterna");
                                item.EstadoId = dr.GetData<int>("EstadoId");
                                //item.EstadoId = !(item.FlagFechaFinSoporte ?? false) ? (int)ETecnologiaEstado.Vigente : item.FechaCalculoTec == null ? (int)ETecnologiaEstado.Obsoleto : (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida && item.FechaExtendida.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte && item.FechaFinSoporte.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna && item.FechaAcordada.Value > DateTime.Now) ? (int)ETecnologiaEstado.Vigente : (int)ETecnologiaEstado.Obsoleto;
                                item.UsuarioCreacion = dr.GetData<string>("UsuarioCreacion");
                                item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                item.UsuarioModificacion = dr.GetData<string>("UsuarioModificacion");
                                item.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");
                                item.CantRoles = dr.GetData<int>("CantRoles");
                                item.CantFunciones = dr.GetData<int>("CantFunciones");
                                item.ProductoId = dr.GetData<int>("ProductoId");
                                item.FlagComponenteCross = dr.GetData<bool>("FlagComponenteCross");
                                item.OwnerTecnologia = dr.GetData<int>("EsOwnerTecnologia");
                                totalRows = dr.GetData<int>("TotalRows");

                                registros.Add(item);
                            }
                        }

                        //var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasGUmbrales(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecnologiasGUmbrales(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
         
        public override List<SolicitudFlujoDTO> GetBandejaSolicitudes(PaginacionSolicitud objeto, out int totalRows)
        {
            using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
            { 
                try
                {
                    List<SolicitudFlujoDTO> registros = new List<SolicitudFlujoDTO>();
                    var estados = string.Join(",", objeto.EstadoSolicitudFlujo);
                    var fechaCreacion = objeto.FechaRegistroSolicitud2.ToString();
                    var fechaSeleccionada =objeto.FechaRegistroSolicitud.ToString("yyyy-MM-dd");
                    totalRows = 0;
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ListBandejaSolicitudes]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@nombre", objeto.nombre.Trim());
                        comando.Parameters.AddWithValue("@productoId", objeto.ProductoId);
                        comando.Parameters.AddWithValue("@dominioId", objeto.DominioId);
                        comando.Parameters.AddWithValue("@subdominioId", objeto.SubDominioId);
                        comando.Parameters.AddWithValue("@estadoSolicitud", estados);
                        comando.Parameters.AddWithValue("@CodigoProducto", objeto.CodigoApt);
                        comando.Parameters.AddWithValue("@fechaCreacion", fechaCreacion == "" ? "" :  fechaSeleccionada);//objeto.FechaRegistroSolicitud2 == null ? null : DbFunctions.TruncateTime(objeto.FechaRegistroSolicitud).Value);
                        comando.Parameters.AddWithValue("@ConfigTecCampCorrelativo", (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula);                       
                        comando.Parameters.AddWithValue("@pageNumber", objeto.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", objeto.pageSize);
                        comando.Connection = cnx;

                        var dr = comando.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new SolicitudFlujoDTO();
                                item.SolicitudTecnologiaId = dr.GetData<long>("SolicitudTecnologiaId");
                                item.CodigoProducto = dr.GetData<string>("Codigo");
                                item.TecnologiaId = dr.GetData<int>("TecnologiaId");
                                item.Tecnologia = dr.GetData<string>("ClaveTecnologia");
                                item.DominioId = dr.GetData<int>("DominioId");
                                item.Dominio = dr.GetData<string>("Dominio");
                                item.SubDominioId = dr.GetData<int>("SubDominioId");
                                item.SubDominio = dr.GetData<string>("SubDominio");
                                item.EstadoSolicitud = dr.GetData<int>("EstadoSolicitud");
                                item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                item.UsuarioCreacion = dr.GetData<string>("CreadoPor");
                                item.IdTipoSolicitud = dr.GetData<int>("IdTipoSolicitud");
                                item.TipoFlujo = dr.GetData<int>("TipoFlujo");
                                item.UsuarioSolicitante = dr.GetData<string>("UsuarioSolicitante");
                                item.ResponsableBuzon = dr.GetData<string>("ResponsableBuzon");
                                item.ResponsableMatricula = dr.GetData<string>("ResponsableMatricula");
                                item.ProductoId = dr.GetData<int>("ProductoId");
                                item.Producto = dr.GetData<string>("NombreProducto");
                                item.OwnerDestino = dr.GetData<string>("OwnerDestino");
                                totalRows = dr.GetData<int>("TotalRows");

                                registros.Add(item);
                            }
                        }
                    }

                    return registros;
                }
                catch (DbEntityValidationException ex)
                {
                    HelperLog.ErrorEntity(ex);
                    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                        , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                        , new object[] { null });
                }
                catch (Exception ex)
                {
                    HelperLog.Error(ex.Message);
                    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                        , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                        , new object[] { null });
                }
                finally
                {
                    cnx.Close();

                }
            }
        }

        public override TecnologiaDTO GetNewTecById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join b in ctx.Producto on u.ProductoId equals b.ProductoId into ljb
                                       from b in ljb.DefaultIfEmpty()
                                       join s in ctx.Subdominio on b.SubDominioId equals s.SubdominioId
                                       join a in ctx.Dominio on s.DominioId equals a.DominioId
                                           //join t in ctx.Aplicacion on u.CodigoAPT equals t.CodigoAPT into lj1
                                           //from t in lj1.DefaultIfEmpty()
                                           //join a in ctx.Arquetipo on u.ArquetipoId equals a.ArquetipoId into lj2
                                           //from a in lj2.DefaultIfEmpty()
                                       where u.TecnologiaId == id
                                       //&& t.FlagActivo
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           ProductoId = u.ProductoId,
                                           Fabricante = b.Fabricante,
                                           Versiones = u.Versiones,
                                           ClaveTecnologia = u.ClaveTecnologia,
                                           Nombre = u.Nombre,
                                           //Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           FlagSiteEstandar = u.FlagSiteEstandar,
                                           FlagTieneEquivalencias = u.FlagTieneEquivalencias ?? false,
                                           MotivoId = u.MotivoId,
                                           TipoTecnologiaId = u.TipoTecnologia,
                                           CodigoProducto = b.Codigo,
                                           AutomatizacionImplementadaId = u.AutomatizacionImplementadaId,
                                           RevisionSeguridadId = u.RevisionSeguridadId,
                                           RevisionSeguridadDescripcion = u.RevisionSeguridadDescripcion,
                                           FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                           Fuente = u.FuenteId,
                                           FechaCalculoBase = u.FechaCalculo,
                                           FechaCalculoTec = u.FechaCalculoTec,
                                           FechaLanzamiento = u.FechaLanzamiento,
                                           FechaExtendida = u.FechaExtendida,
                                           FechaFinSoporte = u.FechaFinSoporte,
                                           FechaAcordada = u.FechaAcordada,
                                           TipoFechaInterna = u.TipoFechaInterna,
                                           ComentariosFechaFin = u.ComentariosFechaFin,
                                           SustentoMotivo = u.SustentoMotivo,
                                           SustentoUrl = u.SustentoUrl,
                                           UrlConfluenceId = u.UrlConfluenceId,
                                           UrlConfluence = u.UrlConfluence,
                                           CasoUso = u.CasoUso,
                                           Aplica = u.Aplica,
                                           CompatibilidadSOId = u.CompatibilidadSOId,
                                           CompatibilidadCloudId = u.CompatibilidadCloudId,
                                           Requisitos = u.Requisitos,
                                           Existencia = u.Existencia,
                                           Riesgo = u.Riesgo,
                                           Facilidad = u.Facilidad,
                                           Vulnerabilidad = u.Vulnerabilidad,
                                           DominioNomb = a.Nombre,
                                           SubdominioNomb = s.Nombre,
                                           SubdominioId = b.SubDominioId,
                                           RoadmapOpcional = u.RoadmapOpcional,
                                           Referencias = u.Referencias,
                                           PlanTransConocimiento = u.PlanTransConocimiento,
                                           EsqMonitoreo = u.EsqMonitoreo,
                                           EsqPatchManagement = u.EsqPatchManagement,
                                           Dueno = u.DuenoId,
                                           EqAdmContacto = b.EquipoAdmContacto,
                                           GrupoSoporteRemedy = b.GrupoTicketRemedyNombre,
                                           ConfArqSeg = u.ConfArqSegId,
                                           ConfArqTec = u.ConfArqTecId,
                                           EncargRenContractual = u.EncargRenContractual,
                                           EsqLicenciamiento = u.EsqLicenciamiento,
                                           SoporteEmpresarial = u.SoporteEmpresarial,
                                           EquipoAprovisionamiento = b.EquipoAprovisionamiento,

                                           //Activo = u.Activo,
                                           //FechaCreacion = u.FechaCreacion,
                                           //UsuarioCreacion = u.UsuarioCreacion,
                                           //EstadoTecnologia = u.EstadoTecnologia,
                                           //FlagConfirmarFamilia = u.FlagConfirmarFamilia.HasValue ? u.FlagConfirmarFamilia.Value : false,
                                           //FechaCalculoTec = u.FechaCalculoTec,
                                           //Compatibilidad = u.Compatibilidad,
                                           //FlagAplicacion = u.FlagAplicacion,
                                           //CodigoAPT = u.CodigoAPT,
                                           //AplicacionNomb = u.CodigoAPT == null ? "" : t.CodigoAPT + " - " + t.Nombre,
                                           ////Fin Tab 1
                                           //DominioId = s.DominioId,
                                           //EliminacionTecObsoleta = u.EliminacionTecObsoleta,
                                           //LineaBaseSeg = u.LineaBaseSeg,
                                           ////Fin Tab 2
                                           ////Fin Tab 3
                                           //FlagSiteEstandar = u.FlagSiteEstandar,
                                           //Fabricante = u.Fabricante,
                                           //EstadoId = u.EstadoId,
                                           //CodigoTecnologiaAsignado = u.CodigoTecnologiaAsignado,
                                           ////ArquetipoId = u.ArquetipoId,
                                           ////ArquetipoNombre = a.Nombre
                                           ////Fin Tab 4
                                           //AplicacionId = u.AplicacionId
                                       }).FirstOrDefault();

                        if (entidad != null)
                        {
                            if (entidad.ProductoId != null)
                            {
                                entidad.Producto = (from a in ctx.Producto
                                                    where
                                                    a.ProductoId == entidad.ProductoId
                                                    select new ProductoDTO
                                                    {
                                                        Id = a.ProductoId,
                                                        Fabricante = a.Fabricante,
                                                        Nombre = a.Nombre
                                                    }).FirstOrDefault();
                            }

                            if (withAutorizadores)
                            {
                                var listAutorizadores = (from u in ctx.Tecnologia
                                                         join at in ctx.TecnologiaAutorizador on u.TecnologiaId equals at.TecnologiaId
                                                         where u.TecnologiaId == id && u.Activo && at.FlagActivo && !at.FlagEliminado
                                                         select new AutorizadorDTO()
                                                         {
                                                             TecnologiaId = at.TecnologiaId,
                                                             AutorizadorId = at.AutorizadorId,
                                                             Matricula = at.Matricula,
                                                             Nombres = at.Nombres,
                                                             Activo = at.FlagActivo
                                                         }).ToList();
                                entidad.ListAutorizadores = listAutorizadores;
                            }

                            if (withArquetipos)
                            {
                                var listArquetipo = (from u in ctx.Arquetipo
                                                     join at in ctx.TecnologiaArquetipo on new { ArquetipoId = u.ArquetipoId, TecnologiaId = (int)entidad.Id } equals new { ArquetipoId = at.ArquetipoId ?? 0, TecnologiaId = at.TecnologiaId }
                                                     where at.FlagActivo
                                                     select new ArquetipoDTO()
                                                     {
                                                         Id = u.ArquetipoId,
                                                         Nombre = u.Nombre,
                                                     }).ToList();
                                entidad.ListArquetipo = listArquetipo;
                            }

                            if (withAplicaciones)
                            {
                                var listAplicaciones = (from u in ctx.Aplicacion
                                                        join ta in ctx.TecnologiaAplicacion on new { u.AplicacionId, TecnologiaId = (int)entidad.Id } equals new { ta.AplicacionId, ta.TecnologiaId }
                                                        where ta.FlagActivo && !ta.FlagEliminado
                                                        select new TecnologiaAplicacionDTO
                                                        {
                                                            Id = ta.TecnologiaAplicacionId,
                                                            TecnologiaId = ta.TecnologiaId,
                                                            AplicacionId = ta.AplicacionId,
                                                            Aplicacion = new AplicacionDTO
                                                            {
                                                                Id = u.AplicacionId,
                                                                CodigoAPT = u.CodigoAPT,
                                                                Nombre = u.Nombre,
                                                                CategoriaTecnologica = u.CategoriaTecnologica,
                                                                TipoActivoInformacion = u.TipoActivoInformacion,
                                                                Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner
                                                            }
                                                        }).ToList();

                                entidad.ListAplicaciones = listAplicaciones;

                            }

                            if (withEquivalencias)
                            {
                                var listEquivalencias = (from u in ctx.Tecnologia
                                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                                         join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId
                                                         where u.TecnologiaId == id
                                                         && u.Activo
                                                         && te.FlagActivo
                                                         select new TecnologiaEquivalenciaDTO
                                                         {
                                                             Id = te.TecnologiaEquivalenciaId,
                                                             TecnologiaId = te.TecnologiaId,
                                                             NombreTecnologia = u.ClaveTecnologia,
                                                             DominioTecnologia = d.Nombre,
                                                             SubdominioTecnologia = s.Nombre,
                                                             TipoTecnologia = t.Nombre,
                                                             EstadoId = u.EstadoId,
                                                             Nombre = te.Nombre
                                                         }).ToList();

                                entidad.ListEquivalencias = listEquivalencias;
                                entidad.FlagTieneEquivalencias = listEquivalencias.Count > 0;
                                //entidad.FlagTieneEquivalencias = listEquivalencias.Count > 0 ? true : false;
                            }

                            var listTecnologiaVinculadas = (from u in ctx.Tecnologia
                                                            join tv in ctx.TecnologiaVinculada on u.TecnologiaId equals tv.VinculoId
                                                            join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                                            join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                            where tv.Activo && tv.TecnologiaId == entidad.Id
                                                            select new TecnologiaDTO()
                                                            {
                                                                Id = tv.VinculoId,
                                                                Nombre = u.ClaveTecnologia,
                                                                DominioNomb = s.Nombre,
                                                                SubdominioNomb = d.Nombre
                                                            }).ToList();

                            //var listEquivalencias = (from u in ctx.TecnologiaEquivalencia
                            //                         where u.FlagActivo && u.TecnologiaId == id
                            //                         select 1).ToList();


                            entidad.ListTecnologiaVinculadas = listTecnologiaVinculadas;

                            int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                                       where t.CodigoInterno == (int)ETablaProcedencia.CVT_Tecnologia
                                                       && t.FlagActivo
                                                       select t.TablaProcedenciaId).FirstOrDefault();
                            if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + (int)ETablaProcedencia.CVT_Tecnologia);

                            var archivo = (from u in ctx.ArchivosCVT
                                           where u.Activo && u.EntidadId == id.ToString() && u.TablaProcedenciaId == TablaProcedenciaId
                                           select new ArchivosCvtDTO()
                                           {
                                               Id = u.ArchivoId,
                                               Nombre = u.Nombre
                                           }).FirstOrDefault();

                            if (archivo != null)
                            {
                                entidad.ArchivoId = archivo.Id;
                                entidad.ArchivoStr = archivo.Nombre;
                            }
                        }

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteClaveTecnologia(string claveTecnologia, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Tecnologia
                                    where
                                    u.Activo
                                    && !u.FlagEliminado
                                    && u.ProductoId != id
                                    && u.ClaveTecnologia.ToUpper() == claveTecnologia.ToUpper()
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string claveTecnologia, int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteClaveTecnologia(string claveTecnologia, int id)"
                    , new object[] { null });
            }
        }

        #region TecnologiaByProducto
        public override List<TecnologiaDTO> GetTecnologiaByProducto(int productoId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join a in ctx.Subdominio on u.SubdominioId equals a.SubdominioId
                                         join b in ctx.Dominio on a.DominioId equals b.DominioId
                                         join c in ctx.Producto on u.ProductoId equals c.ProductoId
                                         join d in ctx.Tipo on c.TipoProductoId equals d.TipoId
                                         join e in ctx.Tipo on u.TipoTecnologia equals e.TipoId
                                         where
                                         u.ProductoId == productoId &&
                                         u.Activo &&
                                         !u.FlagEliminado
                                         select new TecnologiaDTO()
                                         {
                                             Id = u.TecnologiaId,
                                             ProductoId = u.ProductoId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             Fabricante = u.Fabricante,
                                             SubdominioNomb = a.Nombre,
                                             DominioNomb = b.Nombre,
                                             TipoTecnologiaId = u.TipoTecnologia,
                                             TipoTecnologiaStr = e.Nombre,
                                             EstadoId = u.EstadoId,
                                             EstadoTecnologia = u.EstadoTecnologia,
                                             TipoProductoStr = d.Nombre,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             CodigoProducto = c.Codigo,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             FechaAcordada = u.FechaAcordada,
                                             FechaExtendida = u.FechaExtendida,
                                             FechaFinSoporte = u.FechaFinSoporte
                                         });

                        var resultado = registros.ToList();

                        foreach (var item in resultado)
                        {
                            item.Aplicacion = (from u in ctx.Aplicacion
                                               where
                                               u.AplicacionId == item.AplicacionId
                                               select new AplicacionDTO
                                               {
                                                   Id = u.AplicacionId,
                                                   CategoriaTecnologica = u.CategoriaTecnologica,
                                                   Nombre = u.Nombre,
                                                   Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                                   TipoActivoInformacion = u.TipoActivoInformacion,
                                               }).FirstOrDefault();
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecnologiaByProductoWithPagination(int productoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Tecnologia
                                         join a in ctx.Subdominio on u.SubdominioId equals a.SubdominioId
                                         join b in ctx.Dominio on a.DominioId equals b.DominioId
                                         join c in ctx.Producto on u.ProductoId equals c.ProductoId
                                         join d in ctx.Tipo on c.TipoProductoId equals d.TipoId
                                         join e in ctx.Tipo on u.TipoTecnologia equals e.TipoId
                                         where
                                         u.ProductoId == productoId &&
                                         !u.FlagEliminado
                                         select new TecnologiaDTO()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             Fabricante = u.Fabricante,
                                             SubdominioNomb = a.Nombre,
                                             DominioNomb = b.Nombre,
                                             TipoTecnologiaId = u.TipoTecnologia,
                                             TipoTecnologiaStr = e.Nombre,
                                             EstadoId = u.EstadoId,
                                             EstadoTecnologia = u.EstadoTecnologia,
                                             TipoProductoStr = d.Nombre,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        foreach (var item in resultado)
                        {
                            item.Aplicacion = (from u in ctx.Aplicacion
                                               where
                                               u.AplicacionId == item.AplicacionId
                                               select new AplicacionDTO
                                               {
                                                   Id = u.AplicacionId,
                                                   CategoriaTecnologica = u.CategoriaTecnologica,
                                                   Nombre = u.Nombre,
                                                   Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                                   TipoActivoInformacion = u.TipoActivoInformacion,
                                               }).FirstOrDefault();
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> PostListTecnologiasByProducto(PaginacionNewTec pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime fechaActual = DateTime.Now;
                        var registros = (from u in ctx.Tecnologia
                                         join c in ctx.Producto on u.ProductoId equals c.ProductoId
                                         join a in ctx.Subdominio on c.SubDominioId equals a.SubdominioId
                                         join b in ctx.Dominio on a.DominioId equals b.DominioId
                                         join d in ctx.Tipo on c.TipoProductoId equals d.TipoId
                                         join e in ctx.Tipo on u.TipoTecnologia equals e.TipoId
                                         let fechaCalculada = !u.FechaCalculoTec.HasValue ? null : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : null
                                         //let estado = b.Nombre.ToLower().Contains("deprecado") ? (int)ETecnologiaEstado.Deprecado : ((u.FlagFechaFinSoporte ?? false) ? (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : ((fechaCalculada.Value < fechaActual) ? (int)ETecnologiaEstado.Obsoleto : (int)ETecnologiaEstado.Vigente))) : (int)ETecnologiaEstado.Vigente)
                                         where
                                         u.ProductoId == pag.prodId
                                         && u.CodigoProducto==c.Codigo
                                         && (u.Nombre.ToUpper().Contains(pag.nombre.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(pag.nombre.ToUpper())
                                         || string.IsNullOrEmpty(pag.nombre)
                                         || u.ClaveTecnologia.ToUpper().Contains(pag.nombre.ToUpper()))
                                         && (pag.estObsIds.Count == 0 || pag.estObsIds.Contains(u.EstadoId.HasValue ? u.EstadoId.Value : 0)) // pag.estObsIds.Contains(estado))
                                         && (pag.tipoTecIds.Count == 0 || pag.tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))
                                         && (c.FlagActivo) 
                                         && !u.FlagEliminado && u.Activo
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             Fabricante = u.Fabricante,
                                             Subdominio = a.Nombre,
                                             Dominio = b.Nombre,
                                             TipoTecnologiaId = u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 4,
                                             Tipo = e.Nombre,
                                             EstadoId = u.EstadoId,
                                             TipoProductoStr = d.Nombre,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FechaAcordada = u.FechaAcordada,
                                             FechaExtendida = u.FechaExtendida,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             FlagSiteEstandar = u.FlagSiteEstandar,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                             Estado = u.EstadoTecnologia,
                                             Activo = u.Activo,
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        foreach (var item in resultado)
                        {
                            item.Aplicacion = (from u in ctx.Aplicacion
                                               where
                                               u.AplicacionId == item.AplicacionId
                                               select new AplicacionDTO
                                               {
                                                   Id = u.AplicacionId,
                                                   CategoriaTecnologica = u.CategoriaTecnologica,
                                                   Nombre = u.Nombre,
                                                   Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                                   TipoActivoInformacion = u.TipoActivoInformacion,
                                               }).FirstOrDefault();
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<TecnologiaDTO> PostListTecnologiasByProducto(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<TecnologiaDTO> PostListTecnologiasByProducto(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int EditTecnologiaFromProducto(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 0;
            var CURRENT_DATE = DateTime.Now;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {

                        var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                        if (entidad != null)
                        {
                            entidad.Fabricante = objeto.Fabricante;
                            entidad.Nombre = objeto.Nombre;
                            entidad.Versiones = objeto.Versiones;
                            entidad.ClaveTecnologia = objeto.ClaveTecnologia;
                            entidad.TipoTecnologia = objeto.TipoTecnologiaId;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.FlagFechaFinSoporte = objeto.FlagFechaFinSoporte;
                            entidad.FechaLanzamiento = objeto.FechaLanzamiento;
                            entidad.FechaExtendida = objeto.FechaExtendida;
                            entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                            entidad.FechaAcordada = objeto.FechaAcordada;
                            entidad.TipoFechaInterna = objeto.TipoFechaInterna;
                            entidad.ComentariosFechaFin = objeto.ComentariosFechaFin;
                            entidad.SustentoMotivo = objeto.SustentoMotivo;
                            entidad.SustentoUrl = objeto.SustentoUrl;
                            entidad.AutomatizacionImplementadaId = objeto.AutomatizacionImplementadaId;
                            entidad.Activo = true;
                            entidad.FlagEliminado = false;
                            entidad.UsuarioCreacion = objeto.UsuarioCreacion;
                            entidad.FechaCreacion = DateTime.Now;

                            ctx.SaveChanges();

                            if (objeto.ItemsRemoveAppId != null)
                            {
                                foreach (var id in objeto.ItemsRemoveAppId)
                                {
                                    var aplicacion = (from a in ctx.TecnologiaAplicacion
                                                      where a.TecnologiaAplicacionId == id
                                                      select a).FirstOrDefault();

                                    if (aplicacion != null)
                                    {
                                        aplicacion.FlagActivo = false;
                                        aplicacion.FlagEliminado = true;
                                        aplicacion.ModificadoPor = objeto.UsuarioModificacion;
                                        aplicacion.FechaCreacion = DateTime.Now;
                                        ctx.SaveChanges();
                                    };

                                }
                            }

                            if (objeto.ListAplicaciones != null && objeto.TipoTecnologiaId == (int)ETipo.EstandarRestringido)
                            {
                                foreach (var itemAplicacion in objeto.ListAplicaciones)
                                {
                                    var aplicacion = new TecnologiaAplicacion
                                    {
                                        TecnologiaId = objeto.Id,
                                        AplicacionId = itemAplicacion.AplicacionId,
                                        FlagActivo = true,
                                        FlagEliminado = false,
                                        CreadoPor = objeto.UsuarioCreacion,
                                        FechaCreacion = DateTime.Now,
                                    };

                                    ctx.TecnologiaAplicacion.Add(aplicacion);
                                    ctx.SaveChanges();
                                }
                            }
                            else
                            {

                            }

                            //if (ValidarFlagEstandar(objeto.TipoTecnologiaId.Value))
                            //{
                            //    CuerpoNotificacionFlagEstandar = Utilitarios.GetBodyNotification();
                            //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[FECHA_HORA]", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[CLAVE_TECNOLOGIA]", objeto.ClaveTecnologia);
                            //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[MATRICULA]", objeto.UsuarioCreacion);
                            //    CuerpoNotificacionFlagEstandar = CuerpoNotificacionFlagEstandar.Replace("[NOMBRES]", objeto.UsuarioCreacion);
                            //    this.EnviarNotificacionTecnologia(objeto.UsuarioCreacion, CuerpoNotificacionFlagEstandar, true);
                            //}

                            //if(objeto.FlagCambioEstado)
                            //{
                            //    this.EnviarNotificacionTecnologia(objeto.UsuarioCreacion, CuerpoNotificacion);
                            //}                                                                
                            ID = entidad.TecnologiaId;
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

            try
            {
                if (registroNuevo)
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }
                else
                {
                    var paramSP = new ObjTecnologia()
                    {
                        param1 = ID,
                        param2 = objeto.ItemsAddAutorizadorSTR,
                        param3 = objeto.ItemsRemoveAutIdSTR,
                        param4 = objeto.ItemsAddTecEqIdSTR,
                        param5 = objeto.ItemsRemoveTecEqIdSTR,
                        param6 = objeto.ItemsAddTecVinculadaIdSTR,
                        param7 = objeto.ItemsRemoveTecVinculadaIdSTR,
                        param8 = objeto.UsuarioCreacion,
                        param9 = objeto.UsuarioModificacion
                    };

                    this.ActualizarListasTecnologias(paramSP);
                }

                if ((objeto.EstadoId != (int)ETecnologiaEstado.Obsoleto && objeto.EstadoId.HasValue)
                                   && objeto.EstadoTecnologia == (int)EstadoTecnologia.Aprobado && objeto.FlagUnicaVigente == true)
                {
                    //this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.VigentePorVencer);
                    this.ActualizarEstadoTecnologias(objeto.Id, objeto.FamiliaId.Value, (int)ETecnologiaEstado.Deprecado);
                }

                return ID;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }
        #endregion

        #region ProductoTecnologiaAplicacion
        public override List<TecnologiaAplicacionDTO> GetTecnologiaAplicaciones(int tecnologiaId)
        {
            var fecha = DateTime.Now;
            var estados = new List<int>() { 0, 2 };
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.RelacionDetalle
                                         join u2 in ctx.Relacion on u.RelacionId equals u2.RelacionId      
                                         join u3 in ctx.Application on u2.CodigoAPT equals u3.applicationId
                                         where u2.DiaRegistro == fecha.Day && u2.MesRegistro == fecha.Month && u2.AnioRegistro == fecha.Year && estados.Contains(u2.EstadoId)
                                         && u.TecnologiaId == tecnologiaId && u3.isActive == true && u3.status!=4
                                         select new TecnologiaAplicacionDTO()
                                         {
                                             Id = 0,
                                             TecnologiaId = u.TecnologiaId.Value,
                                             CodigoAPT = u2.CodigoAPT
                                         });

                        var resultado = registros.Distinct().ToList();

                        foreach (var item in resultado)
                        {
                            item.Aplicacion = (from u in ctx.Aplicacion
                                               where
                                               u.CodigoAPT == item.CodigoAPT
                                               select new AplicacionDTO
                                               {
                                                   Id = u.AplicacionId,
                                                   CodigoAPT = u.CodigoAPT,
                                                   CategoriaTecnologica = u.CategoriaTecnologica,
                                                   Nombre = u.Nombre,
                                                   Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                                   TipoActivoInformacion = u.TipoActivoInformacion,
                                                   GestionadoPor = u.GestionadoPor
                                               }).FirstOrDefault();
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool DeleteTecnologiaById(int id, string userName)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Tecnologia
                                    where //u.Activo && 
                                    u.TecnologiaId == id
                                    select u).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.Activo = false;
                        registro.FlagEliminado = true;
                        registro.UsuarioModificacion = userName;
                        registro.FechaModificacion = DateTime.Now;
                    }
                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool DeleteTecnologiaById(int id, string userName)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool DeleteTecnologiaById(int id, string userName)"
                    , new object[] { null });
            }
        }

        public override bool DeleteTecnologiaAplicacionById(int id, string userName)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.TecnologiaAplicacion
                                    where //u.Activo && 
                                    u.TecnologiaAplicacionId != id
                                    select u).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.FlagActivo = false;
                        registro.FlagEliminado = true;
                        registro.ModificadoPor = userName;
                        registro.FechaModificacion = DateTime.Now;
                    }
                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool EliminarProductoTecnologiaAplicacionById(int id, string userName)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool EliminarProductoTecnologiaAplicacionById(int id, string userName)"
                    , new object[] { null });
            }
        }

        public override bool GuardarMasivoTecnologiaAplicacion(List<TecnologiaAplicacionDTO> lista, int[] listaEliminar, string userName)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (listaEliminar != null)
                    {
                        foreach (int id in listaEliminar)
                        {
                            var itemBD = (from u in ctx.TecnologiaAplicacion
                                          where //u.Activo && 
                                          u.TecnologiaAplicacionId == id
                                          select u).FirstOrDefault();

                            if (itemBD != null)
                            {
                                itemBD.FlagActivo = false;
                                itemBD.FlagEliminado = true;
                                itemBD.ModificadoPor = userName;
                                itemBD.FechaModificacion = DateTime.Now;
                            }
                            ctx.SaveChanges();
                        }
                    }

                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            var itemBD = (from u in ctx.TecnologiaAplicacion
                                          where //u.Activo && 
                                          u.TecnologiaAplicacionId == item.Id
                                          select u).FirstOrDefault();

                            if (itemBD == null)
                            {
                                itemBD = new TecnologiaAplicacion
                                {
                                    TecnologiaId = item.TecnologiaId,
                                    AplicacionId = item.AplicacionId,
                                    FlagActivo = true,
                                    FlagEliminado = false,
                                    CreadoPor = userName,
                                    FechaCreacion = DateTime.Now
                                };

                                ctx.TecnologiaAplicacion.Add(itemBD);
                            }
                            else
                            {
                                itemBD.AplicacionId = item.AplicacionId;
                                itemBD.ModificadoPor = userName;
                                itemBD.FechaModificacion = DateTime.Now;
                            }

                            ctx.SaveChanges();
                        }
                    }

                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool GuardarMasivoProductoTecnologiaAplicacion(List<ProductoTecnologiaAplicacionDTO> lista, int[] listaEliminar, string userName)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: bool GuardarMasivoProductoTecnologiaAplicacion(List<ProductoTecnologiaAplicacionDTO> lista, int[] listaEliminar, string userName)"
                    , new object[] { null });
            }
        }
        #endregion

        public override bool ExisteRelacionByTecnologiaId(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var CURRENT_DAY = DateTime.Now;
                        var day = CURRENT_DAY.Day;
                        var month = CURRENT_DAY.Month;
                        var year = CURRENT_DAY.Year;

                        var estadoRelacionIds = new int[]
                        {
                            (int)EEstadoRelacion.PendienteEliminacion,
                            (int)EEstadoRelacion.Aprobado,
                        };

                        bool? estado = (from u in ctx.Relacion
                                        join rd in ctx.RelacionDetalle on u.RelacionId equals rd.RelacionId
                                        where u.FlagActivo && rd.FlagActivo
                                        && u.DiaRegistro == day && u.MesRegistro == month && u.AnioRegistro == year
                                        && estadoRelacionIds.Contains(u.EstadoId)
                                        && rd.TecnologiaId == Id
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteTecnologiaAsociadaAlMotivo(int motivoId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        int cantidadRegistros = (from u in ctx.Tecnologia
                                                 where u.Activo
                                                 && !u.FlagEliminado
                                                 && u.MotivoId == motivoId
                                                 select true).Count();

                        bool existe = cantidadRegistros > 0;

                        return existe;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
        }

        #region Tecnología Owner
        public override List<TecnologiaOwnerDto> BuscarTecnologiaXOwner(string correo, int perfilId, string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, bool? flagTribuCoe, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_buscar_x_owner]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@correoOwner", correo));
                        comando.Parameters.Add(new SqlParameter("@perfilId", perfilId));
                        comando.Parameters.Add(new SqlParameter("@dominioIds", ((object)dominioIds) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@productoStr", ((object)productoStr) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@tribuCoeId", ((object)tribuCoeId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@squadId", ((object)squadId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@flagTribuCoe", ((object)flagTribuCoe) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                TecnologiaId = reader.GetData<int>("TecnologiaId"),
                                ProductoId = reader.GetData<int>("ProductoId"),
                                ProductoStr = reader.GetData<string>("ProductoStr"),
                                Fabricante = reader.GetData<string>("Fabricante"),
                                Nombre = reader.GetData<string>("Nombre"),
                                Dominio = reader.GetData<string>("DominioStr"),
                                Subdominio = reader.GetData<string>("SubdominioStr"),
                                ClaveTecnologia = reader.GetData<string>("ClaveTecnologia"),
                                TipoTecnologia = reader.GetData<string>("TipoTecnologiaStr"),
                                TotalInstanciasServidores = reader.GetData<int>("TotalInstanciasServidores"),
                                TotalInstanciasServicioNube = reader.GetData<int>("TotalInstanciasServicioNube"),
                                TotalInstanciasPcs = reader.GetData<int>("TotalInstanciasPcs"),
                                TotalAplicaciones = reader.GetData<int>("TotalAplicaciones"),
                                TribuCoeId = reader.GetData<int?>("TribuCoeId"),
                                TribuCoeDisplayName = reader.GetData<string>("TribuCoeDisplayName"),
                                TribuCoeDisplayNameResponsable = reader.GetData<string>("TribuCoeDisplayNameResponsable"),
                                SquadId = reader.GetData<int>("SquadId"),
                                SquadDisplayName = reader.GetData<string>("SquadDisplayName"),
                                SquadDisplayNameResponsable = reader.GetData<string>("SquadDisplayNameResponsable"),
                                FlagFechaFinSoporte = reader.GetData<bool?>("FlagFechaFinSoporte"),
                                FechaCalculoTec = reader.GetData<int?>("FechaCalculoTec"),
                                FechaAcordada = reader.GetData<DateTime?>("FechaAcordada"),
                                FechaExtendida = reader.GetData<DateTime?>("FechaExtendida"),
                                FechaFinSoporte = reader.GetData<DateTime?>("FechaFinSoporte"),
                                EstadoId = reader.GetData<int>("EstadoId"),
                                Codigo = reader.GetData<string>("Codigo"),
                                EstadoTecnologia = reader.GetData<int>("EstadoTecnologia")
                                //EstadoIdActual = reader.GetData<int>("EstadoIdActual"),
                                //EstadoIdDoceMeses = reader.GetData<int>("EstadoIdDoceMeses"),
                                //EstadoIdVeintiCuatroMeses = reader.GetData<int>("EstadoIdVeintiCuatroMeses")
                            };
                            totalRows = reader.GetData<int>("TotalRows");
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerDto> BuscarTecnologiaXOwnerProducto(string correo, int perfilId, int productoId)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_buscar_x_owner_producto]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@correoOwner", correo));
                        comando.Parameters.Add(new SqlParameter("@perfilId", perfilId));
                        comando.Parameters.Add(new SqlParameter("@productoId", productoId));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerDto()
                            {
                                TecnologiaId = reader.GetData<int>("TecnologiaId"),
                                ProductoId = reader.GetData<int>("ProductoId"),
                                ProductoStr = reader.GetData<string>("ProductoStr"),
                                Fabricante = reader.GetData<string>("Fabricante"),
                                Nombre = reader.GetData<string>("Nombre"),
                                Dominio = reader.GetData<string>("DominioStr"),
                                Subdominio = reader.GetData<string>("SubdominioStr"),
                                ClaveTecnologia = reader.GetData<string>("ClaveTecnologia"),
                                TipoTecnologia = reader.GetData<string>("TipoTecnologiaStr"),
                                TotalInstanciasServidores = reader.GetData<int>("TotalInstanciasServidores"),
                                TotalInstanciasServicioNube = reader.GetData<int>("TotalInstanciasServicioNube"),
                                TotalInstanciasPcs = reader.GetData<int>("TotalInstanciasPcs"),
                                TotalAplicaciones = reader.GetData<int>("TotalAplicaciones"),
                                TribuCoeId = reader.GetData<int>("TribuCoeId"),
                                TribuCoeDisplayName = reader.GetData<string>("TribuCoeDisplayName"),
                                TribuCoeDisplayNameResponsable = reader.GetData<string>("TribuCoeDisplayNameResponsable"),
                                SquadId = reader.GetData<int>("SquadId"),
                                SquadDisplayName = reader.GetData<string>("SquadDisplayName"),
                                SquadDisplayNameResponsable = reader.GetData<string>("SquadDisplayNameResponsable"),
                                FlagFechaFinSoporte = reader.GetData<bool?>("FlagFechaFinSoporte"),
                                FechaCalculoTec = reader.GetData<int?>("FechaCalculoTec"),
                                FechaAcordada = reader.GetData<DateTime?>("FechaAcordada"),
                                FechaExtendida = reader.GetData<DateTime?>("FechaExtendida"),
                                FechaFinSoporte = reader.GetData<DateTime?>("FechaFinSoporte"),
                                EstadoId = reader.GetData<int>("EstadoId"),
                                Codigo = reader.GetData<string>("Codigo"),
                                EstadoTecnologia = reader.GetData<int>("EstadoTecnologia")
                                //EstadoIdActual = reader.GetData<int>("EstadoIdActual"),
                                //EstadoIdDoceMeses = reader.GetData<int>("EstadoIdDoceMeses"),
                                //EstadoIdVeintiCuatroMeses = reader.GetData<int>("EstadoIdVeintiCuatroMeses")
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }
        #endregion

        #region Tecnología Owner Consolidado
        public override List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescencia(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId, int? nivel, string ownerParentIds, string TipoEquipoIds, string FechaFiltro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerConsolidadoObsolescenciaDTO>();
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "";
                DateTime dtFechaFiltro;
                //if (parametroAppliance == "true")
                //{
                    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_owner_x_nivel_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_owner_x_nivel]";
                //}

                try
                {
                    dtFechaFiltro = DateTime.ParseExact(FechaFiltro, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    dtFechaFiltro = DateTime.Now;
                }
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerStr", ((object)ownerStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@squadId", ((object)squadId) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@estadoId", ((object)estadoId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@nivel", ((object)nivel) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerParentIds", ((object)ownerParentIds) ?? DBNull.Value);
                        //if (parametroAppliance == "true")
                        //{
                            comando.Parameters.AddWithValue("@TipoEquipoIds", ((object)TipoEquipoIds) ?? DBNull.Value);
                            comando.Parameters.AddWithValue("@FechaFiltro", ((object)dtFechaFiltro) ?? DBNull.Value);
                        //}
                        


                            var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerConsolidadoObsolescenciaDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                OwnerParentId = reader.GetData<string>("OwnerParentId"),
                                OwnerId = reader.GetData<int?>("OwnerId"),
                                Owner = reader.GetData<string>("Owner"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                PorcentajeVigenteKPI = reader.GetData<decimal>("PorcentajeVigenteKPI"),
                                Vigente12MesesKPI = reader.GetData<int>("Vigente12MesesKPI"),
                                Vigente12MesesAMasKPI = reader.GetData<int>("Vigente12MesesAMasKPI"),
                                TotalKPI = reader.GetData<int>("TotalKPI"),
                                PorcentajeTotalKPI = reader.GetData<decimal>("PorcentajeTotalKPI"),
                                PorcentajeTotalVigente12MesesKPI = reader.GetData<decimal>("PorcentajeTotalVigente12MesesKPI"),
                                PorcentajeTotalVigente12MesesAMasKPI = reader.GetData<decimal>("PorcentajeTotalVigente12MesesAMasKPI")
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescenciaReporte(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerConsolidadoObsolescenciaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_obsolescencia_consolidado_owner_reporte]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerStr", ((object)ownerStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@squadId", ((object)squadId) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@estadoId", ((object)estadoId) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerConsolidadoObsolescenciaDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                OwnerParentId = reader.GetData<string>("OwnerParentId"),
                                OwnerId = reader.GetData<int?>("OwnerId"),
                                Owner = reader.GetData<string>("Owner"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                PorcentajeVigenteKPI = reader.GetData<decimal>("PorcentajeVigenteKPI"),
                                DeprecadoKPI = reader.GetData<int>("DeprecadoKPI"),
                                PorcentajeDeprecadoKPI = reader.GetData<decimal>("PorcentajeDeprecadoKPI"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                TotalKPI = reader.GetData<int>("TotalKPI"),
                                PorcentajeTotalKPI = reader.GetData<decimal>("PorcentajeTotalKPI"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescenciaReporte2(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId, string TipoEquipoIds = null, string FechaFiltro = null)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaOwnerConsolidadoObsolescenciaDTO>();
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_owner_reporte2_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_owner_reporte2]";
                //}

                DateTime dtFechaFiltro;
                try
                {
                    dtFechaFiltro = DateTime.ParseExact(FechaFiltro, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    dtFechaFiltro = DateTime.Now;
                }

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerStr", ((object)ownerStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@squadId", ((object)squadId) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@estadoId", ((object)estadoId) ?? DBNull.Value);
                        //if (parametroAppliance == "true")
                        //{
                            comando.Parameters.AddWithValue("@TipoEquipoIds", ((object)TipoEquipoIds) ?? DBNull.Value);
                            comando.Parameters.AddWithValue("@FechaFiltro", ((object)dtFechaFiltro) ?? DBNull.Value);
                        //}

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaOwnerConsolidadoObsolescenciaDTO()
                            {
                                Owner = reader.GetData<string>("Owner"),
                                ProductoStr = reader.GetData<string>("ProductoStr"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                PorcentajeVigenteKPI = reader.GetData<decimal>("PorcentajeVigenteKPI"),                                
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                TotalKPI = reader.GetData<int>("TotalKPI"),
                                PorcentajeTotalKPI = reader.GetData<decimal>("PorcentajeTotalKPI"),
                                Vigente12MesesKPI = reader.GetData<int>("Vigente12MesesKPI"),
                                Vigente12MesesAMasKPI = reader.GetData<int>("Vigente12MesesAMasKPI"),
                                PorcentajeTotalVigente12MesesKPI = reader.GetData<decimal>("PorcentajeVigente12MesesKPI"),
                                PorcentajeTotalVigente12MesesAMasKPI = reader.GetData<decimal>("PorcentajeVigente12MesesAMasKPI")
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }
        #endregion

        #region Tecnología SoportadoPor Consolidado
        public override List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescencia(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int? nivel, string soportadoPorParents)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO>();
                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                string nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_x_nivel_V3]";
                //if (parametroAppliance == "true")
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_x_nivel_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_x_nivel]";
                //}

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@correoOwner", ((object)correoOwner) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@aplicacionStr", ((object)aplicacionStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionadoPor) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@nivel", ((object)nivel) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerParentIds", ((object)soportadoPorParents) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaGestionadoPorConsolidadoObsolescenciaDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                SoportadoPorParent = reader.GetData<string>("SoportadoPorParent"),
                                SoportadoPorId = reader.GetData<string>("SoportadoPorId"),
                                SoportadoPor = reader.GetData<string>("SoportadoPor"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                Vence12KPI = reader.GetData<int>("Vence12KPI"),
                                PorcentajeVence12KPI = reader.GetData<decimal>("PorcentajeVence12KPI"),
                                Vence24KPI = reader.GetData<int>("Vence24KPI"),
                                PorcentajeVence24KPI = reader.GetData<decimal>("PorcentajeVence24KPI"),
                                Vence24KPICorto = reader.GetData<int>("Vence24KPICorto"),
                                PorcentajeVence24KPICorto = reader.GetData<decimal>("PorcentajeVence24KPICorto"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                TotalKPI = reader.GetData<int>("Total"),
                                PorcentajeKPIFlooking = reader.GetData<decimal>("PorcentajeKPIFlooking"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaUdFConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaUdF(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int? nivel, string soportadoPorParents, string unidadFondeo, bool flagProyeccion, string fechaProyeccion)
        {
            try
            {
                var fechaProy = DateTime.Now;
                try
                {
                    fechaProy = DateTime.ParseExact(fechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    fechaProy = DateTime.Now;
                }
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaUdFConsolidadoObsolescenciaDTO>();
                string nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_x_nivel]";
                
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@correoOwner", ((object)correoOwner) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@aplicacionStr", ((object)aplicacionStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionadoPor) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@nivel", ((object)nivel) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@ownerParentIds", ((object)soportadoPorParents) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@unidadFondeo", ((object)unidadFondeo) ?? "");
                        comando.Parameters.AddWithValue("@flagProyeccion", flagProyeccion);
                        comando.Parameters.AddWithValue("@fechaProyeccion", fechaProy);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaUdFConsolidadoObsolescenciaDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                UnidadFondeoParent = reader.GetData<string>("UnidadFondeoParent"),
                                UnidadFondeoId = reader.GetData<string>("UnidadFondeoId"),
                                UnidadFondeo = reader.GetData<string>("UnidadFondeo"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                Vence12KPI = reader.GetData<int>("Vence12KPI"),
                                PorcentajeVence12KPI = reader.GetData<decimal>("PorcentajeVence12KPI"),
                                Vence24KPI = reader.GetData<int>("Vence24KPI"),
                                PorcentajeVence24KPI = reader.GetData<decimal>("PorcentajeVence24KPI"),
                                Vence24KPICorto = reader.GetData<int>("Vence24KPICorto"),
                                PorcentajeVence24KPICorto = reader.GetData<decimal>("PorcentajeVence24KPICorto"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                TotalKPI = reader.GetData<int>("Total"),
                                PorcentajeKPIFlooking = reader.GetData<decimal>("PorcentajeKPIFlooking"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaReporte(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_reporte]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@correoOwner", ((object)correoOwner) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@aplicacionStr", ((object)aplicacionStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionadoPor) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaGestionadoPorConsolidadoObsolescenciaDTO()
                            {
                                Nivel = reader.GetData<int>("Nivel"),
                                SoportadoPorParent = reader.GetData<string>("SoportadoPorParent"),
                                SoportadoPorId = reader.GetData<string>("SoportadoPorId"),
                                SoportadoPor = reader.GetData<string>("SoportadoPor"),
                                VigenteKPI = reader.GetData<int>("VigenteKPI"),
                                PorcentajeVigenteKPI = reader.GetData<decimal>("PorcentajeVigenteKPI"),
                                ObsoletoKPI = reader.GetData<int>("ObsoletoKPI"),
                                PorcentajeObsoletoKPI = reader.GetData<decimal>("PorcentajeObsoletoKPI"),
                                TotalKPI = reader.GetData<int>("TotalKPI"),
                                PorcentajeTotalKPI = reader.GetData<decimal>("PorcentajeTotalKPI"),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaReporte2(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO>();

                //var parametroAppliance = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("ACTIVAR_FECHAVENCIMIENTO_APPLIANCE").Valor;
                //string nombreSP = "";
                //if (parametroAppliance == "true")
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_reporte2_V2]";
                //}
                //else
                //{
                //    nombreSP = "[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_reporte2]";
                //}

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_obsolescencia_consolidado_soportadopor_reporte2_V3]", cnx))
                    //using (var comando = new SqlCommand(nombreSP, cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@correoOwner", ((object)correoOwner) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@aplicacionStr", ((object)aplicacionStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionadoPor) ?? DBNull.Value);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaGestionadoPorConsolidadoObsolescenciaDTO()
                            {
                                SoportadoPor = reader.IsDBNull(reader.GetOrdinal("SoportadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("SoportadoPor")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("AplicacionStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionStr")),
                                ObsoletoKPI = reader.IsDBNull(reader.GetOrdinal("ObsoletoKPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("ObsoletoKPI")),
                                PorcentajeObsoletoKPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeObsoletoKPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeObsoletoKPI")),
                                Vence12KPI = reader.IsDBNull(reader.GetOrdinal("Vence12KPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence12KPI")),
                                PorcentajeVence12KPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence12KPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence12KPI")),
                                Vence24KPI =reader.IsDBNull(reader.GetOrdinal("Vence24KPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence24KPI")),
                                PorcentajeVence24KPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence24KPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence24KPI")),
                                Vence24KPICorto = reader.IsDBNull(reader.GetOrdinal("Vence24KPICorto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence24KPICorto")),
                                PorcentajeVence24KPICorto = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence24KPICorto")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence24KPICorto")),
                                VigenteKPI = reader.IsDBNull(reader.GetOrdinal("VigenteKPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("VigenteKPI")),
                                TotalKPI = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                PorcentajeKPIFlooking = reader.IsDBNull(reader.GetOrdinal("PorcentajeKPIFlooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeKPIFlooking")),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }
        public override List<TecnologiaUdFConsolidadoObsolescenciaDTO> ListarTecnologiaUdfConsolidadoObsolescenciaReporte(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, string unidadFondeo, bool flagProyeccion, string fechaProyeccion)
        {
            try
            {
                var fechaProy = DateTime.Now;
                try
                {
                    fechaProy = DateTime.ParseExact(fechaProyeccion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    fechaProy = DateTime.Now;
                }
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaUdFConsolidadoObsolescenciaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_tecnologia_obsolescencia_consolidado_soportadopor_reporte]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@correoOwner", ((object)correoOwner) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@perfilId", ((object)perfilId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@aplicacionStr", ((object)aplicacionStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionadoPor) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@unidadFondeo", ((object)unidadFondeo) ?? "");
                        comando.Parameters.AddWithValue("@flagProyeccion", flagProyeccion);
                        comando.Parameters.AddWithValue("@fechaProyeccion", fechaProy);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaUdFConsolidadoObsolescenciaDTO()
                            {
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo")),
                                SegundoNivel = reader.IsDBNull(reader.GetOrdinal("SegundoNivel")) ? string.Empty : reader.GetString(reader.GetOrdinal("SegundoNivel")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("AplicacionStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionStr")),
                                ObsoletoKPI = reader.IsDBNull(reader.GetOrdinal("ObsoletoKPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("ObsoletoKPI")),
                                PorcentajeObsoletoKPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeObsoletoKPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeObsoletoKPI")),
                                Vence12KPI = reader.IsDBNull(reader.GetOrdinal("Vence12KPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence12KPI")),
                                PorcentajeVence12KPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence12KPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence12KPI")),
                                Vence24KPI = reader.IsDBNull(reader.GetOrdinal("Vence24KPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence24KPI")),
                                PorcentajeVence24KPI = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence24KPI")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence24KPI")),
                                Vence24KPICorto = reader.IsDBNull(reader.GetOrdinal("Vence24KPICorto")) ? 0 : reader.GetInt32(reader.GetOrdinal("Vence24KPICorto")),
                                PorcentajeVence24KPICorto = reader.IsDBNull(reader.GetOrdinal("PorcentajeVence24KPICorto")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeVence24KPICorto")),
                                VigenteKPI = reader.IsDBNull(reader.GetOrdinal("VigenteKPI")) ? 0 : reader.GetInt32(reader.GetOrdinal("VigenteKPI")),
                                TotalKPI = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                PorcentajeKPIFlooking = reader.IsDBNull(reader.GetOrdinal("PorcentajeKPIFlooking")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeKPIFlooking")),
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
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        #endregion

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

        //private int GetEstadoTecnologiaEstandar(DateTime fechaCalculo, DateTime fechaComparacion, int meses)
        //{
        //    if (fechaCalculo < fechaComparacion)
        //        return (int)ETecnologiaEstadoEstandar.Obsoleto;
        //    else
        //    {
        //        fechaComparacion = fechaComparacion.AddMonths(meses);
        //        if (fechaCalculo < fechaComparacion)
        //            return (int)ETecnologiaEstadoEstandar.PorVencer;
        //        else
        //            return (int)ETecnologiaEstadoEstandar.Vigente;
        //    }
        //}

        //private int GetEstadoTecnologiaEstandar(DateTime fechaCalculo, DateTime fechaComparacion, DateTime fechaAgregada)
        //{
        //    if (fechaCalculo < fechaComparacion)
        //        return (int)ETecnologiaEstadoEstandar.Obsoleto;
        //    else
        //    {
        //        //fechaComparacion = fechaComparacion.AddMonths(meses);
        //        if (fechaCalculo < fechaAgregada)
        //            return (int)ETecnologiaEstadoEstandar.PorVencer;
        //        else
        //            return (int)ETecnologiaEstadoEstandar.Vigente;
        //    }
        //}

        private static string GetSemaforoTecnologiaClass(DateTime fechaCalculo, DateTime fechaComparacion, int meses, string url, string codigoProducto, string nombre, bool? flagMostrarEstado, string tipoTecnologia, int id, string[] clasesCss, int? tipoTecnologiaId = 0)
        {
            fechaComparacion = fechaComparacion.AddMonths(meses);
            if (fechaCalculo < fechaComparacion)
            {
                return Utilitarios.DevolverTecnologiaEstandarStr(url, codigoProducto, nombre, clasesCss[0], flagMostrarEstado, tipoTecnologia, id);
            }
            else
            {
                return Utilitarios.DevolverTecnologiaEstandarStr(url, codigoProducto, nombre, clasesCss[1], flagMostrarEstado, tipoTecnologia, id);
            }
        }

        private static int GetEstadoTecnologiaId(DateTime fechaCalculo, DateTime fechaComparacion, int meses, int[] estadoTecV, int? tipoTecnologiaId = 0)
        {
            fechaComparacion = fechaComparacion.AddMonths(meses);
            if (fechaCalculo < fechaComparacion)
            {
                return estadoTecV[0];
            }
            else
            {
                return estadoTecV[1];
            }
        }

        public override FiltrosDashboardTecnologia ListFiltrosDashboardXGestionadoPor(List<int> idsGestionadoPor)
        {
            try
            {
                FiltrosDashboardTecnologia arr_data = null;
                var arrIdsDominios = idsGestionadoPor.ToArray();

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        arr_data = new FiltrosDashboardTecnologia();

                        arr_data.Equipos = (from t in ctx.GestionadoPor
                                                //join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                                            join s in ctx.TeamSquad on t.GestionadoPorId equals s.GestionadoPorId
                                            where t.FlagActivo
                                            && s.FlagActivo
                                            && arrIdsDominios.Contains(t.GestionadoPorId)
                                            select new CustomAutocomplete()
                                            {
                                                Id = s.EquipoId.ToString(),
                                                Descripcion = s.Nombre
                                                //TipoId = s.SubdominioId.ToString(),
                                                //TipoDescripcion = s.Nombre
                                            }).Distinct().OrderBy(z => z.Descripcion).ToList();

                    }
                    return arr_data;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio()"
                    , new object[] { null });
            }
        }

        #region Flujo de Aprobación de Tecnologías
        public override TecnologiaDTO GetDatosTecnologiaById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var entidad = ServiceManager<TechnologyConfigurationDAO>.Provider.GetAllTechonologyXId(id);
                        
                        //tech.technology.ProductoId == null ? tech.technology.CodigoProducto : string.IsNullOrEmpty(tech.technology.CodigoProducto) ? tech.product == null ? "" : tech.product.Codigo : tech.technology.CodigoProducto,
                        if(entidad.TecReemplazoDepId != null)
                        {
                            var TecReemplazoId = entidad.TecReemplazoDepId;
                            var entidadTecReemplazo = ServiceManager<TechnologyConfigurationDAO>.Provider.GetAllTechonologyXId((int)TecReemplazoId);
                            entidad.TecReemplazoDepNomb = entidadTecReemplazo.ClaveTecnologia;
                        }
                        else
                        {
                            entidad.TecReemplazoDepNomb = "";
                        }
                        //

                        if (entidad != null)
                        {
                            if (entidad.TribuCoeId != null) // CAR 1
                            {
                                var datosTribu = ServiceManager<TecnologiaDAO>.Provider.GetDatosPersonaPorMatricula(entidad.TribuCoeId, 1);
                                entidad.TribuCoeMatricula = datosTribu.Matricula;
                                entidad.TribuCoeResponsable = datosTribu.Nombre;
                            }

                            if (entidad.ConfArqSegId != null) // CAR 2
                            {
                                var datosConfArq = ServiceManager<TecnologiaDAO>.Provider.GetDatosPersonaPorMatricula(entidad.ConfArqSegId, 2);
                                entidad.ConfArqSegDisplayName = datosConfArq.Nombre;
                            }

                            if (entidad.ConfArqTecId != null) // CAR 3
                            {
                                var datosConfTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosPersonaPorMatricula(entidad.ConfArqTecId, 2);
                                entidad.ConfArqTecDisplayName = datosConfTec.Nombre;
                            }
                                
                            if (entidad.ProductoId != null)
                            {
                                entidad.Producto = (from a in ctx.Producto
                                                    where
                                                    a.ProductoId == entidad.ProductoId
                                                    select new ProductoDTO
                                                    {
                                                        Id = a.ProductoId,
                                                        Fabricante = a.Fabricante,
                                                        Nombre = a.Nombre,
                                                        Descripcion = a.Descripcion,
                                                        SubDominioId = a.SubDominioId,
                                                        DominioId = a.DominioId,
                                                        Codigo = a.Codigo,
                                                        EquipoAdmContacto = a.EquipoAdmContacto,
                                                        EquipoAprovisionamiento = a.EquipoAprovisionamiento
                                                    }).FirstOrDefault();
                            }

                            if (withAutorizadores)
                            {
                                var listAutorizadores = (from u in ctx.Tecnologia
                                                         join at in ctx.TecnologiaAutorizador on u.TecnologiaId equals at.TecnologiaId
                                                         where u.TecnologiaId == id && u.Activo && at.FlagActivo && !at.FlagEliminado
                                                         select new AutorizadorDTO()
                                                         {
                                                             TecnologiaId = at.TecnologiaId,
                                                             AutorizadorId = at.AutorizadorId,
                                                             Matricula = at.Matricula,
                                                             Nombres = at.Nombres,
                                                             Activo = at.FlagActivo
                                                         }).ToList();
                                entidad.ListAutorizadores = listAutorizadores;
                            }

                            if (withArquetipos)
                            {
                                var listArquetipo = (from u in ctx.Arquetipo
                                                     join at in ctx.TecnologiaArquetipo on new { ArquetipoId = u.ArquetipoId, TecnologiaId = (int)entidad.Id } equals new { ArquetipoId = at.ArquetipoId ?? 0, TecnologiaId = at.TecnologiaId }
                                                     where at.FlagActivo
                                                     select new ArquetipoDTO()
                                                     {
                                                         Id = u.ArquetipoId,
                                                         Nombre = u.Nombre,
                                                     }).ToList();
                                entidad.ListArquetipo = listArquetipo;
                            }

                            if (withAplicaciones)
                            {
                                var listAplicaciones = (from u in ctx.Aplicacion
                                                        join ta in ctx.TecnologiaAplicacion on new { u.AplicacionId, TecnologiaId = (int)entidad.Id } equals new { ta.AplicacionId, ta.TecnologiaId }
                                                        where ta.FlagActivo && !ta.FlagEliminado
                                                        select new TecnologiaAplicacionDTO
                                                        {
                                                            Id = ta.TecnologiaAplicacionId,
                                                            TecnologiaId = ta.TecnologiaId,
                                                            AplicacionId = ta.AplicacionId,
                                                            Aplicacion = new AplicacionDTO
                                                            {
                                                                Id = u.AplicacionId,
                                                                CodigoAPT = u.CodigoAPT,
                                                                Nombre = u.CodigoAPT + " - " + u.Nombre,
                                                                CategoriaTecnologica = u.CategoriaTecnologica,
                                                                TipoActivoInformacion = u.TipoActivoInformacion,
                                                                Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner
                                                            }
                                                        }).ToList();

                                entidad.ListAplicaciones = listAplicaciones;

                            }

                            if (withEquivalencias)
                            {
                                var listEquivalencias = (from u in ctx.Tecnologia
                                                         join p in ctx.Producto on u.ProductoId equals p.ProductoId
                                                         join s in ctx.Subdominio on p.SubDominioId equals s.SubdominioId
                                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                                         join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId
                                                         where u.TecnologiaId == id
                                                         && u.Activo
                                                         && te.FlagActivo
                                                         select new TecnologiaEquivalenciaDTO
                                                         {
                                                             Id = te.TecnologiaEquivalenciaId,
                                                             TecnologiaId = te.TecnologiaId,
                                                             NombreTecnologia = u.ClaveTecnologia,
                                                             DominioTecnologia = d.Nombre,
                                                             SubdominioTecnologia = s.Nombre,
                                                             TipoTecnologia = t.Nombre,
                                                             EstadoId = u.EstadoId,
                                                             Nombre = te.Nombre
                                                         }).ToList();

                                entidad.ListEquivalencias = listEquivalencias;
                                entidad.FlagTieneEquivalencias = listEquivalencias.Count > 0;
                            }

                            var listTecnologiaVinculadas = (from u in ctx.Tecnologia
                                                            join tv in ctx.TecnologiaVinculada on u.TecnologiaId equals tv.VinculoId
                                                            join p in ctx.Producto on u.ProductoId equals p.ProductoId
                                                            join s in ctx.Subdominio on p.SubDominioId equals s.SubdominioId
                                                            join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                            where tv.Activo && tv.TecnologiaId == entidad.Id
                                                            select new TecnologiaDTO()
                                                            {
                                                                Id = tv.VinculoId,
                                                                Nombre = u.ClaveTecnologia,
                                                                DominioNomb = d.Nombre,
                                                                SubdominioNomb = s.Nombre
                                                            }).ToList();
                            
                            //==================================================
                            var listExpertos = (from u in ctx.ProductoManagerRoles
                                                    join ta in ctx.ProductoManager on u.ProductoManagerId equals ta.ProductoManagerId
                                                    where u.FlagActivo 
                                                    && u.ProductoId == entidad.ProductoId
                                                    select new ProductoManagerRolesDTO
                                                    {
                                                        Id = (int)u.ProductoManagerRolesId,
                                                        ProductoId = (int)u.ProductoId,
                                                        ManagerMatricula = u.ManagerMatricula,
                                                        ManagerNombre = u.ManagerNombre,
                                                        ManagerEmail = u.ManagerEmail,
                                                        ProductoManagerId = (int)u.ProductoManagerId,
                                                        ProductoManagerStr = ta.Nombre,
                                                        Registro = 1,
                                                        UsuarioCreacion = u.CreadoPor,
                                                        FechaCreacion = (DateTime)u.FechaCreacion
                                                    }).ToList();

                            entidad.ListExpertos = listExpertos;
                            //==================================================

                            entidad.ListTecnologiaVinculadas = listTecnologiaVinculadas;

                            int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                                       where t.CodigoInterno == (int)ETablaProcedencia.CVT_Tecnologia
                                                       && t.FlagActivo
                                                       select t.TablaProcedenciaId).FirstOrDefault();
                            if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + (int)ETablaProcedencia.CVT_Tecnologia);

                            var archivo = (from u in ctx.ArchivosCVT
                                           where u.Activo && u.EntidadId == id.ToString() && u.TablaProcedenciaId == TablaProcedenciaId
                                           select new ArchivosCvtDTO()
                                           {
                                               Id = u.ArchivoId,
                                               Nombre = u.Nombre
                                           }).FirstOrDefault();

                            if (archivo != null)
                            {
                                entidad.ArchivoId = archivo.Id;
                                entidad.ArchivoStr = archivo.Nombre;
                            }
                        }

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetTecById(int id)"
                    , new object[] { null });
            }
        }

        public override TecnologiaDTO GetDatosTecnologiaId(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from x in ctx.Tecnologia
                                       join p in ctx.Producto on x.ProductoId equals p.ProductoId
                                        where x.TecnologiaId == id
                                        select new TecnologiaDTO
                                        {
                                            Id = x.TecnologiaId,
                                            Nombre = x.ClaveTecnologia,
                                            Codigo = p.Codigo,
                                        }).FirstOrDefault();
                           
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetDatosTecnologiaId(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetDatosTecnologiaId(int id)"
                    , new object[] { null });
            }
        }

        public override ProductoDTO GetDatosProductoById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from a in ctx.Producto
                                    where a.ProductoId == id
                                    select new ProductoDTO
                                    {
                                        Id = a.ProductoId,
                                        Codigo = a.Codigo,
                                        Fabricante = a.Fabricante,
                                        Nombre = a.Nombre,
                                        Descripcion = a.Descripcion,
                                        SubDominioId = a.SubDominioId,
                                        DominioId = a.DominioId,
                                        TipoProductoId = a.TipoProductoId,
                                        TipoCicloVidaId = a.TipoCicloVidaId,
                                        EsquemaLicenciamientoSuscripcionId = a.EsquemaLicenciamientoSuscripcionId,
                                        // === Tab 2 - Responsabilidades
                                        TribuCoeId = (string.IsNullOrEmpty(a.TribuCoeId) ? string.Empty : a.TribuCoeId),
                                        TribuCoeDisplayName = (string.IsNullOrEmpty(a.TribuCoeDisplayName) ? string.Empty : a.TribuCoeDisplayName),
                                        // CAR 1
                                        SquadId = (string.IsNullOrEmpty(a.SquadId) ? string.Empty : a.SquadId),
                                        SquadDisplayName = (string.IsNullOrEmpty(a.SquadDisplayName) ? string.Empty : a.SquadDisplayName),
                                        OwnerId = (string.IsNullOrEmpty(a.OwnerId) ? string.Empty : a.OwnerId),
                                        OwnerMatricula = (string.IsNullOrEmpty(a.OwnerMatricula) ? string.Empty : a.OwnerMatricula),
                                        OwnerDisplayName = (string.IsNullOrEmpty(a.OwnerDisplayName) ? string.Empty : a.OwnerDisplayName),
                                        LiderUsuarioAutorizador = "",
                                        EquipoAprovisionamiento = (string.IsNullOrEmpty(a.EquipoAprovisionamiento) ? string.Empty : a.EquipoAprovisionamiento),
                                        GrupoTicketRemedyId = a.GrupoTicketRemedyId,
                                        GrupoTicketRemedyStr = (string.IsNullOrEmpty(a.GrupoTicketRemedyNombre) ? string.Empty : a.GrupoTicketRemedyNombre)
                                    }).FirstOrDefault();

                        if (entidad.TribuCoeId != null) // CAR 1
                        {
                            var datosTribu = ServiceManager<TecnologiaDAO>.Provider.GetDatosPersonaPorMatricula(entidad.TribuCoeId, 1);
                            entidad.TribuResponsableMatricula = datosTribu.Matricula;
                            entidad.TribuResponsableNombre = datosTribu.Nombre;
                        }

                        //==================================================
                        var listExpertos = (from u in ctx.ProductoManagerRoles
                                            join ta in ctx.ProductoManager on u.ProductoManagerId equals ta.ProductoManagerId
                                            where u.FlagActivo
                                            && u.ProductoId == entidad.Id
                                            select new ProductoManagerRolesDTO
                                            {
                                                Id = (int)u.ProductoManagerRolesId,
                                                ProductoId = (int)u.ProductoId,
                                                ManagerMatricula = u.ManagerMatricula,
                                                ManagerNombre = u.ManagerNombre,
                                                ManagerEmail = u.ManagerEmail,
                                                ProductoManagerId = (int)u.ProductoManagerId,
                                                ProductoManagerStr = ta.Nombre,
                                                Registro = 1,
                                                UsuarioCreacion = u.CreadoPor,
                                                FechaCreacion = (DateTime)u.FechaCreacion
                                            }).ToList();

                        entidad.ListExpertos = listExpertos;

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ProductoDTO GetDatosProductoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ProductoDTO GetDatosProductoById(int id)"
                    , new object[] { null });
            }
        }

        public override TecnologiaDTO GetDatosResponsablePorId(string codigo, int idTipo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    TecnologiaDTO datos = new TecnologiaDTO();
                    var datosTribu = ServiceManager<TecnologiaDAO>.Provider.GetDatosPersonaPorMatricula(codigo, idTipo);
                    datos.TribuCoeMatricula = datosTribu.Matricula;
                    datos.TribuCoeResponsable = datosTribu.Nombre;

                    return datos;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: TecnologiaDTO GetDatosResponsablePorId(string codigo, int idTipo)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: TecnologiaDTO GetDatosResponsablePorId(string codigo, int idTipo)"
                    , new object[] { null });
            }
        }
        public override OwnerDTO GetDatosPersonaPorMatricula(string codigo, int idTipo)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            //List<OwnerDTO> lista = new List<OwnerDTO>();
            OwnerDTO lista = new OwnerDTO();

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("app.Buscar_Datos_Por_Matricula_BCP_CATG_GH_INFO_EMPLEADOS", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@pCodigo", codigo));
                    comando.Parameters.Add(new SqlParameter("@pIdTipo", idTipo));

                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        lista.Matricula = reader.GetString(reader.GetOrdinal("Matricula"));
                        lista.Correo = reader.GetString(reader.GetOrdinal("Correo"));
                        lista.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                    }
                }

                cnx.Close();

                return lista;
            }
        }
        public override bool VerificarDiferenciaDeDatos(TecnologiaDTO objeto)
        {
            try
            {
                //bool estado = false;
                int control = 0;
                int rolContador = 0;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    
                    if (objeto.TipoSave == 1 || objeto.TipoSave==4)
                    {
                        var entidad = ctx.Producto.FirstOrDefault(x => x.ProductoId == objeto.ProductoId);
                        if (entidad != null)
                        {
                            var objProd = ServiceManager<TecnologiaDAO>.Provider.GetDatosProductoById(objeto.ProductoId.HasValue ? objeto.ProductoId.Value : 0, false, false, false, false);
                            if (objProd != null)
                            {
                                switch (objeto.TipoSave)
                                {
                                    case 1:
                                        // Acondicionamiento de datos de base de datos
                                        objProd.TipoCicloVidaId = (objProd.TipoCicloVidaId == null) ? -1 : objProd.TipoCicloVidaId;
                                        entidad.EsquemaLicenciamientoSuscripcionId = (objProd.EsquemaLicenciamientoSuscripcionId == null) ? -1 : objProd.EsquemaLicenciamientoSuscripcionId;

                                        if (objProd.Fabricante.ToUpper().Trim() != objeto.Producto.Fabricante.ToUpper().Trim() ||  // 1.- Fabricante
                                            (objProd.Descripcion == null ? ("" != objeto.Producto.Descripcion.Trim()) : (objProd.Descripcion.ToUpper().Trim() != objeto.Producto.Descripcion.ToUpper().Trim())) ||
                                            objProd.Nombre.ToUpper().Trim() != objeto.Producto.Nombre.ToUpper().Trim() ||
                                            objProd.DominioId != objeto.Producto.DominioId ||    // 7. Dominio
                                            objProd.SubDominioId != objeto.Producto.SubDominioId ||  // 8. Subdominio
                                            //objProd.TipoProductoId != objeto.Producto.TipoProductoId ||  // 9. Tipo de Producto
                                            objProd.TipoCicloVidaId != objeto.Producto.TipoCicloVidaId ||   // 10. Tipo de Ciclo de Vida
                                            objProd.EsquemaLicenciamientoSuscripcionId != objeto.Producto.EsquemaLicenciamientoSuscripcionId // 11. Esquema de licenciamiento
                                            )
                                        {
                                            control = 1;
                                        }
                                        break;
                                    case 4:
                                        objProd.GrupoTicketRemedyId = (objProd.GrupoTicketRemedyId == null) ? -1 : objProd.GrupoTicketRemedyId;

                                        if (objProd.TribuCoeId.Trim() != objeto.Producto.TribuCoeId.Trim() || // B1. Tribu/COE/Unidad Organizacional
                                            objProd.TribuCoeDisplayName.Trim() != objeto.Producto.TribuCoeDisplayName.Trim() ||
                                            objProd.SquadId.Trim() != objeto.Producto.SquadId.Trim() || // B3. SQUAD
                                            objProd.SquadDisplayName.Trim() != objeto.Producto.SquadDisplayName.Trim() ||
                                            objProd.OwnerId.Trim() != objeto.Producto.OwnerId.Trim() || // B4. Responsable de SQUAD
                                            objProd.OwnerMatricula.Trim() != objeto.Producto.OwnerMatricula.Trim() ||
                                            objProd.OwnerDisplayName.Trim() != objeto.Producto.OwnerDisplayName.Trim() ||

                                            objProd.EquipoAprovisionamiento.ToUpper().Trim() != objeto.EquipoAprovisionamiento.ToUpper().Trim() || // B6. Equipo de Aprovisionamiento
                                            objProd.GrupoTicketRemedyId != objeto.Producto.GrupoTicketRemedyId ||
                                            objeto.ListExpertos.Count() > 0 ||
                                            objeto.ListExpertosEliminar.Count() > 0
                                        )
                                        {
                                            control = 1;
                                        }
                                        break;
                                }

                            }
                        }
                    }
                    else
                    {
                        var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                        if (entidad != null)
                        {
                            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(objeto.Id, false, false, false, false);
                            if (objTec != null)
                            {
                                // Verificación de nulos
                                objeto = ServiceManager<TecnologiaDAO>.Provider.AcondicionamientoTecnologia(objeto);

                                switch (objeto.TipoSave)
                                {
                                    case 2:
                                        objTec.AutomatizacionImplementadaId = (objTec.AutomatizacionImplementadaId == null) ? -1 : objTec.AutomatizacionImplementadaId;
                                        objTec.UrlConfluenceId = (objTec.UrlConfluenceId == null) ? -1 : objTec.UrlConfluenceId;

                                        if ((objTec.Descripcion == null ? ("" != objeto.Descripcion.Trim()) : (objTec.Descripcion.ToUpper().Trim() != objeto.Descripcion.ToUpper().Trim())) || // 5.- Descripción de la tecnología
                                            objTec.TipoTecnologiaId != objeto.TipoTecnologiaId || // A.1. Tipo de Tecnología
                                            objTec.AutomatizacionImplementadaId != objeto.AutomatizacionImplementadaId || // A.2. ¿La tecnología tiene script que automatiza su implementación?
                                            objTec.UrlConfluenceId != objeto.UrlConfluenceId || // A.12. Revisión de Lineamiento de Tecnología (url confluence)
                                            objTec.UrlConfluence.ToUpper().Trim() != objeto.UrlConfluence.ToUpper().Trim() || // A.13. URL Lineamiento de Tecnología
                                            (objTec.CasoUso == null ? ("" != objeto.CasoUso.Trim()) : (objTec.CasoUso.ToUpper().Trim() != objeto.CasoUso.ToUpper().Trim())) || // A.14. Caso de uso
                                            objTec.Aplica != objeto.Aplica || // A.15. Indicar la Plataforma a la que aplica
                                            objTec.CompatibilidadSOId != objeto.CompatibilidadSOId || // A.16. Compatibilidad de SO
                                            objTec.CompatibilidadCloudId != objeto.CompatibilidadCloudId || // A.17. Compatibilidad Cloud
                                            objTec.EsqMonitoreo.ToUpper().Trim() != objeto.EsqMonitoreo.ToUpper().Trim() || // A.18. Esquema de Monitoreo
                                            objeto.ListAplicaciones != null ||
                                            objeto.ListAplicacionesEliminar.Count() > 0 ||
                                            objTec.RevisionSeguridadId != objeto.RevisionSeguridadId ||
                                            objTec.RevisionSeguridadDescripcion != objeto.RevisionSeguridadDescripcion ||
                                            objTec.EstadoId != objeto.EstadoId ||
                                            objTec.FechaDeprecada != objeto.FechaDeprecada || // Fecha Deprecada
                                            objTec.TecReemplazoDepId != objeto.TecReemplazoDepId //Tec Reemplazo de Deprecado
                                            )
                                        {
                                            control = 1;
                                        }
                                        /*if(objTec.EstadoId == 1)
                                        {
                                            if (objeto.FlagRestringido)
                                            {
                                                control = 1;
                                            }
                                        }*/
                                        break;
                                    case 3:
                                        // Acondicionamiento de datos de base de datos
                                        objTec.Fuente = (objTec.Fuente == null || objTec.Fuente == -1) ? null : objTec.Fuente;
                                        objTec.FechaCalculoTec = (objTec.FechaCalculoTec == null || objTec.FechaCalculoTec == -1) ? null : objTec.FechaCalculoTec;

                                        if (objTec.FechaLanzamiento != objeto.FechaLanzamiento || // A.3. Fecha de Lanzamiento de Tecnología
                                            objTec.FlagFechaFinSoporte != objeto.FlagFechaFinSoporte || // A.4. ¿Tiene fecha fin de soporte?
                                            objTec.Fuente != objeto.Fuente || // A.5. [1]-Fuente
                                            objTec.FechaCalculoTec != objeto.FechaCalculoTec || // A.6. [1]-Fecha para cálculo de obsolescencia
                                            objTec.FechaExtendida != objeto.FechaExtendida || // A.6. [1]-Fecha fin extendida de la tecnología
                                            objTec.FechaFinSoporte != objeto.FechaFinSoporte || // A.7. [1]-Fecha fin soporte de la tecnología
                                            objTec.FechaAcordada != objeto.FechaAcordada || // A.8. [1]-Fecha fin interna de la tecnología
                                            (objTec.ComentariosFechaFin == null ? ("" != objeto.ComentariosFechaFin.Trim()) : (objTec.ComentariosFechaFin.ToUpper().Trim() != objeto.ComentariosFechaFin.ToUpper().Trim())) || // A.9. [1]-Comentarios asociados a la fecha fin de soporte de tecnología
                                            objTec.SustentoMotivo != objeto.SustentoMotivo || // A.10. [2]-Motivo de fecha indefinida
                                            (objTec.SustentoUrl == null ? ("" != objeto.SustentoUrl.Trim()) : (objTec.SustentoUrl.ToUpper().Trim() != objeto.SustentoUrl.ToUpper().Trim()))// A.11. [2]-Url de fecha indefinida
                                        )
                                        {
                                            control = 1;
                                        }
                                        break;
                                }

                            }
                        }
                    }

                    if (control > 0)
                        return true;
                    else return false;
                }


            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool VerificarDiferenciaDeDatos(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool VerificarDiferenciaDeDatos(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<SolicitudFlujoDTO> GetFlujosSolicitudes(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                       
                        List<SolicitudFlujoDTO> datos = new List<SolicitudFlujoDTO>();
                        List<SolicitudFlujoDTO> datosGrupos = new List<SolicitudFlujoDTO>();
                        List<SolicitudFlujoDTO> datosOwner = new List<SolicitudFlujoDTO>();
                        List<SolicitudFlujoDTO> datosCreados = new List<SolicitudFlujoDTO>();

                        var registros = (from s1 in ctx.SolicitudTecnologia
                                         join t in ctx.Tecnologia on s1.TecnologiaId equals t.TecnologiaId
                                         join p in ctx.Producto on t.ProductoId equals p.ProductoId
                                         join u2 in ctx.Dominio on p.DominioId equals u2.DominioId
                                         join u3 in ctx.Subdominio on p.SubDominioId equals u3.SubdominioId
                                         join f in ctx.SolicitudTecnologiaFlujo on (int)s1.SolicitudTecnologiaId equals f.SolicitudTecnologiaId
                                         where
                                         (t.Nombre.ToUpper().Contains(pag.nombre.ToUpper().Trim())
                                         || t.Descripcion.ToUpper().Contains(pag.nombre.ToUpper().Trim())
                                         || string.IsNullOrEmpty(pag.nombre.ToUpper().Trim())
                                         || t.ClaveTecnologia.ToUpper().Contains(pag.nombre.ToUpper().Trim())
                                         ) &&
                                         (pag.productoId == null || pag.productoId == 0 || p.ProductoId == pag.productoId) &&
                                         (p.DominioId == pag.DominioId || pag.DominioId == -1) &&
                                         (p.SubDominioId == pag.SubDominioId || pag.SubDominioId == -1) &&
                                         (pag.EstadoSolicitudFlujo.Count == 0 || pag.EstadoSolicitudFlujo.Contains((int)s1.EstadoSolicitud)) &&
                                         //(s1.EstadoSolicitud == pag.EstadoSolicitudFlujo || pag.EstadoSolicitudFlujo == -1) &&
                                         (pag.FechaRegistroSolicitud2 == null
                                         || DbFunctions.TruncateTime(s1.FechaCreacion) == DbFunctions.TruncateTime(pag.FechaRegistroSolicitud).Value) 
                                         &&
                                         (p.Codigo.ToUpper().Contains(pag.CodigoApt.ToUpper()) || string.IsNullOrEmpty(pag.CodigoApt)) 
                                         select new SolicitudFlujoDTO()
                                         {
                                             SolicitudTecnologiaId = s1.SolicitudTecnologiaId,
                                             CodigoProducto = p.Codigo,
                                             TecnologiaId = t.TecnologiaId,
                                             Tecnologia = t.ClaveTecnologia,
                                             DominioId = u2.DominioId,
                                             Dominio = u2.Nombre,
                                             SubDominioId = u3.SubdominioId,
                                             SubDominio = u3.Nombre,
                                             EstadoSolicitud = s1.EstadoSolicitud,
                                             FechaCreacion = s1.FechaCreacion,
                                             CreadoPor = s1.UsuarioCreacion,
                                             IdTipoSolicitud = s1.TipoSolicitud,
                                             Codigo = p.Codigo,
                                             TipoFlujo = f.TipoFlujo,
                                             UsuarioSolicitante = s1.UsuarioSolicitante,
                                             ResponsableBuzon = f.ResponsableBuzon,
                                             ResponsableMatricula = f.ResponsableMatricula,
                                         }).Distinct().OrderBy("FechaCreacion" + " " + pag.sortOrder, "EstadoSolicitud asc").ToList();

                        if (pag.Perfil.Contains("E195_Administrador"))
                        {
                            datos = registros;
                        }
                        else
                        {
                            // Filtros para consolidar aprobadores
                            datosGrupos = registros.Where(x => x.TipoFlujo == ((int)FlujoTipo.CVT) || x.TipoFlujo == ((int)FlujoTipo.Estandares)).ToList();

                            if (pag.Perfil.Contains("E195_GestorCVTCatalogoTecnologias"))
                            {
                                datos = datosGrupos.Where(x => x.TipoFlujo == ((int)FlujoTipo.CVT)).ToList();
                            }

                            if (pag.Perfil.Contains("E195_GestorTecnologia"))
                            {
                                datos = datosGrupos.Where(x => x.TipoFlujo == ((int)FlujoTipo.Estandares)).ToList();
                            }

                            //===== Owner
                            datosOwner = registros.Where(x => x.TipoFlujo == ((int)FlujoTipo.Squad) && x.ResponsableMatricula == pag.Matricula).ToList();
                            datos.AddRange(datosOwner);
                            //== Obtener los creados
                            datosCreados = registros.Where(x => x.UsuarioSolicitante == pag.Matricula).ToList();
                            datos.AddRange(datosCreados);

                            datos = datos.Distinct().OrderBy("FechaCreacion" + " " + pag.sortOrder).ToList();
                        }

                        totalRows = datos.Count();

                        var resultado = datos.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ConfiguracionTecnologiaCamposDTO> GetConfiguracionTecnologiaCampos()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.ConfiguracionTecnologiaCampos
                                         where
                                         u.FlagActivo == 1
                                         orderby u.ConfiguracionTecnologiaCamposId descending
                                         select new ConfiguracionTecnologiaCamposDTO()
                                         {
                                             Id = (int)u.CorrelativoCampo,
                                             NombreCampo = u.NombreCampo,
                                             TablaProcedencia = u.TablaProcedencia,
                                             DescripcionCampo = u.DescripcionCampo,
                                             FlagActivo = (int)u.FlagActivo,
                                             RolAprueba = u.RolAprueba,
                                             ConfiguracionTecnologiaCamposId = (int)u.ConfiguracionTecnologiaCamposId,
                                             CorrelativoCampo = (int)u.CorrelativoCampo
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<SolicitudTecnologiaCamposDTO> GetConfiguracionTecnologiaCampos()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<SolicitudTecnologiaCamposDTO> GetConfiguracionTecnologiaCampos()"
                    , new object[] { null });
            }
        }
        public override List<TecnologiaAplicacionDTO> GetTecnologiaAplicacionesPorTecnologia(int idTecnologia)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lista = (from u in ctx.Aplicacion
                                 join ta in ctx.TecnologiaAplicacion on new { u.AplicacionId, TecnologiaId = idTecnologia } equals new { ta.AplicacionId, ta.TecnologiaId }
                                 where ta.FlagActivo && !ta.FlagEliminado
                                 select new TecnologiaAplicacionDTO()
                                 {
                                     Id = ta.TecnologiaAplicacionId,
                                     AplicacionId = ta.AplicacionId,
                                     Aplicacion = new AplicacionDTO
                                     {
                                         Id = u.AplicacionId,
                                         CodigoAPT = u.CodigoAPT,
                                         Nombre = u.CodigoAPT + " - " + u.Nombre,
                                       
                                     }
                                 }).ToList();
                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaAplicacionDTO> GetTecnologiaAplicacionesPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaAplicacionDTO> GetTecnologiaAplicacionesPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
        }
        public void InsertarTecnologiaCampos2(GestionCMDB_ProdEntities ctx, int idSolicitudTecnologia, int idCampo, string valorAnterior, string valorNuevo, string usuarioMatricula)
        {
            var campos = new SolicitudTecnologiaCampos()
            {
                SolicitudTecnologiaId = idSolicitudTecnologia,
                ConfiguracionTecnologiaCamposId = idCampo,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                EstadoCampo = ((int)FlujoEstadoSolicitud.Pendiente),
                UsuarioCreacion = usuarioMatricula,
                FechaCreacion = DateTime.Now,
            };
            ctx.SolicitudTecnologiaCampos.Add(campos);
            ctx.SaveChanges();
        }
        public override SolicitudTecnologiaCamposDTO InsertarTecnologiaCampoActualizacion(List<ConfiguracionTecnologiaCamposDTO> diccionario, int idCampo, string valorAnterior, string valorNuevo, string usuarioMatricula, string valorCampoPropuesto)
        {
            var dicc1 = diccionario.Where(x => x.ConfiguracionTecnologiaCamposId == idCampo).FirstOrDefault();

            var result = new SolicitudTecnologiaCamposDTO();

            result.ConfiguracionTecnologiaCamposId = idCampo;
            result.NombreCampo = dicc1.NombreCampo;
            result.ValorAnterior = valorAnterior;
            result.ValorNuevo = valorNuevo;
            result.EstadoCampo = ((int)FlujoEstadoSolicitud.Pendiente);
            result.UsuarioCreacion = usuarioMatricula;
            result.FechaCreacion = DateTime.Now;
            result.RolAprueba = dicc1.RolAprueba;
            result.ValorCampoPropuesto = valorCampoPropuesto;
            result.CorrelativoCampo = dicc1.CorrelativoCampo;


            return result;
        }
        public override List<SolicitudTecnologiaCamposDTO> InsertarTecnologiaCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, TecnologiaDTO objTec, TecnologiaDTO objeto, List<TecnologiaAplicacionDTO> aplicacionLista)
        {
            // Obtener configuración
            var listaCampos = new List<SolicitudTecnologiaCamposDTO>();
            var campoNuevo = new SolicitudTecnologiaCamposDTO();

            switch (objeto.TipoSave)
            {
                case 2:
                    #region Datos Generales

                    // 3.- Versión
                    if (objTec.Versiones.ToUpper().Trim() != objeto.Versiones.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Version),
                            objTec.Versiones.Trim(),
                            objeto.Versiones.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // 4.- Clave de la Tecnología
                    if (objTec.ClaveTecnologia.ToUpper().Trim() != objeto.ClaveTecnologia.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ClaveTecnologia),
                            objTec.ClaveTecnologia.Trim(),
                            objeto.ClaveTecnologia.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    //// Estado de Tecnología
                    //if(objTec.EstadoId == 1)
                    //{
                    //    if(objeto.FlagRestringido == true)
                    //    {
                    //        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    //       configuracion,
                    //       ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EstadoId),
                    //       ((int)ETecnologiaEstado.Vigente).ToString(),
                    //       ((int)ETecnologiaEstado.Restringido).ToString(),
                    //       objeto.UsuarioMatricula,
                    //       string.Empty);
                    //        listaCampos.Add(campoNuevo);
                    //    }
                    //}


                    #endregion

                    #region TAB 1: General
                    // ============TAB 1: GENERAL ============
                    // =======================================

                    var oDataTipoTecnologia = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(objTec.TipoTecnologiaId);
                    if (!string.IsNullOrEmpty(oDataTipoTecnologia))
                    {
                        // A.1. Tipo de Tecnología
                        if (objTec.TipoTecnologiaId != objeto.TipoTecnologiaId)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia),
                                objTec.TipoTecnologiaId.ToString(),
                                objeto.TipoTecnologiaId.ToString(),
                                objeto.UsuarioMatricula,
                                objeto.TipoTecnologiaStr);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objTec.TipoTecnologiaId.ToString()) && objeto.TipoTecnologiaId != -1)
                        {
                            if (objTec.TipoTecnologiaId != objeto.TipoTecnologiaId)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                    configuracion,
                                    ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia),
                                    objTec.TipoTecnologiaId.ToString(),
                                    objeto.TipoTecnologiaId.ToString(),
                                    objeto.UsuarioMatricula,
                                    objeto.TipoTecnologiaStr);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }


                    var oDataAutomatizacionImplemetnacion = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(objTec.AutomatizacionImplementadaId);
                    if (!string.IsNullOrEmpty(oDataAutomatizacionImplemetnacion))
                    {
                        // A.2. ¿La tecnología tiene script que automatiza su implementación?
                        if (objTec.AutomatizacionImplementadaId == null) { objTec.AutomatizacionImplementadaId = -1; }
                        if (objeto.AutomatizacionImplementadaId == null) { objeto.AutomatizacionImplementadaId = -1; }
                        if (objTec.AutomatizacionImplementadaId != objeto.AutomatizacionImplementadaId)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId),
                                objTec.AutomatizacionImplementadaId.ToString(),
                                objeto.AutomatizacionImplementadaId.ToString(),
                                objeto.UsuarioMatricula,
                                string.Empty);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objTec.AutomatizacionImplementadaId.ToString()) && objeto.AutomatizacionImplementadaId != -1)
                        {
                            if (objTec.AutomatizacionImplementadaId == null) { objTec.AutomatizacionImplementadaId = -1; }
                            if (objeto.AutomatizacionImplementadaId == null) { objeto.AutomatizacionImplementadaId = -1; }
                            if (objTec.AutomatizacionImplementadaId != objeto.AutomatizacionImplementadaId)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                    configuracion,
                                    ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId),
                                    objTec.AutomatizacionImplementadaId.ToString(),
                                    objeto.AutomatizacionImplementadaId.ToString(),
                                    objeto.UsuarioMatricula,
                                    string.Empty);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }


                    // A.12. Revisión de Lineamiento de Tecnología (url confluence)
                    if (objTec.UrlConfluenceId == null) { objTec.UrlConfluenceId = -1; }
                    if (objeto.UrlConfluenceId == null) { objeto.UrlConfluenceId = -1; }
                    if (objTec.UrlConfluenceId != objeto.UrlConfluenceId)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.UrlConfluenceId),
                            objTec.UrlConfluenceId.ToString(),
                            objeto.UrlConfluenceId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.13. URL Lineamiento de Tecnología
                    if (objTec.UrlConfluence.ToUpper().Trim() != objeto.UrlConfluence.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.UrlConfluence),
                            objTec.UrlConfluence.Trim(),
                            objeto.UrlConfluence.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.14. Caso de uso
                    if (objTec.CasoUso.ToUpper().Trim() != objeto.CasoUso.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.CasoUso),
                            objTec.CasoUso.Trim(),
                            objeto.CasoUso.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    var data = Utilitarios.EnumToList<EAplicaATecnologia>()
                                         .Where(o => Utilitarios.GetEnumDescription3(o) == objTec.Aplica) //CORRECCION ENUM
                                         .Select(x => new MasterDetail
                                         {
                                             Id = (int)x,
                                             Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                 })
                                         .FirstOrDefault();

                    if (!ReferenceEquals(null, data))
                    {
                        // A.15. Indicar la Plataforma a la que aplica
                        //if (objTec.Aplica == null) { objTec.Aplica = -1; }
                        //if (objeto.Aplica == null) { objeto.Aplica = -1; }
                        if (objTec.Aplica != objeto.Aplica)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                               configuracion,
                               ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma),
                                objTec.Aplica.ToString(),
                                objeto.Aplica.ToString(),
                                objeto.UsuarioMatricula,
                                objeto.AplicaStr);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (objTec.Aplica == "")
                        {
                            if (objTec.Aplica != objeto.Aplica)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                   configuracion,
                                   ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma),
                                    objTec.Aplica.ToString(),
                                    objeto.Aplica.ToString(),
                                    objeto.UsuarioMatricula,
                                    objeto.AplicaStr);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }


                    // A.16. Compatibilidad de SO
                    if (objTec.CompatibilidadSOId != objeto.CompatibilidadSOId)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.CompatibilidadSoId),
                            objTec.CompatibilidadSOId.ToString(),
                            objeto.CompatibilidadSOId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.17. Compatibilidad Cloud
                    if (objTec.CompatibilidadCloudId != objeto.CompatibilidadCloudId)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.CompatibilidadCloudId),
                            objTec.CompatibilidadCloudId.ToString(),
                            objeto.CompatibilidadCloudId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.18. Esquema de Monitoreo
                    if (objTec.EsqMonitoreo.ToUpper().Trim() != objeto.EsqMonitoreo.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EsquemaMonitoreo),
                            objTec.EsqMonitoreo.Trim(),
                            objeto.EsqMonitoreo.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.19. Estado de Tecnología
                    if (objTec.EstadoId != objeto.EstadoId)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EstadoId),
                            objTec.EstadoId.ToString(),
                            objeto.EstadoId.ToString(),
                            objeto.UsuarioMatricula,
                            objeto.EstadoStr);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.20. Fecha Deprecado
                    if (objTec.FechaDeprecada != objeto.FechaDeprecada && objeto.EstadoId == (int)ETecnologiaEstado.Deprecado)
                    {
                        string fechaActual = (objTec.FechaDeprecada.HasValue) ? objTec.FechaDeprecada.Value.ToString("dd/MM/yyyy") : string.Empty;
                        string fechaNuevo = (objeto.FechaDeprecada.HasValue) ? objeto.FechaDeprecada.Value.ToString("dd/MM/yyyy") : string.Empty;

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaDeprecada),
                            fechaActual,
                            fechaNuevo,
                            objeto.UsuarioMatricula,
                            fechaNuevo);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.21. Tecnología de Reemplazo Deprecado
                    if (objTec.TecReemplazoDepId != objeto.TecReemplazoDepId && objeto.EstadoId == (int)ETecnologiaEstado.Deprecado)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId),
                            objTec.TecReemplazoDepId.ToString(),
                            objeto.TecReemplazoDepId.ToString(),
                            objeto.UsuarioMatricula,
                            objeto.TecReemplazoDepNomb);
                        listaCampos.Add(campoNuevo);
                    }
                    #endregion

                    #region TAB 3: Aplicaciones
                    // ============TAB 3: APLICACIONES ============
                    // C1. Agregar aplicaciones
                    //if (objeto.ListAplicaciones != null && objeto.TipoTecnologiaId == (int)ETipo.EstandarRestringido)
                    if (objeto.ListAplicaciones != null && objeto.EstadoId == (int)ETecnologiaEstado.Restringido)
                    {
                        foreach (var item in objeto.ListAplicaciones)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AplicacionAsociada),
                                string.Empty,
                                item.AplicacionId.ToString(),
                                objeto.UsuarioMatricula,
                                item.Aplicacion.Nombre);
                            listaCampos.Add(campoNuevo);
                        }
                    }

                    // C2. Eliminar aplicaciones
                    if (objeto.ListAplicacionesEliminar != null)
                    {
                        foreach (var item in objeto.ListAplicacionesEliminar)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada),
                            item.AplicacionId.ToString(),
                            string.Empty,
                            objeto.UsuarioMatricula,
                            item.Aplicacion.Nombre); // Nombre de Equivalencia
                            listaCampos.Add(campoNuevo);
                        }
                    }

                    #endregion
                    break;
                case 3:
                    #region TAB 1: General
                    // ============TAB 1: GENERAL ============
                    // =======================================
                    // A.3. Fecha de Lanzamiento de Tecnología
                    if (objTec.FechaLanzamiento != objeto.FechaLanzamiento)
                    {
                        string fechaActual = (objTec.FechaLanzamiento.HasValue) ? objTec.FechaLanzamiento.Value.ToString("dd/MM/yyyy") : string.Empty;
                        string fechaNuevo = (objeto.FechaLanzamiento.HasValue) ? objeto.FechaLanzamiento.Value.ToString("dd/MM/yyyy") : string.Empty;

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaLanzamiento),
                            fechaActual, //objTec.FechaLanzamiento.ToString("dd/MM/yyyy"),
                            fechaNuevo, //objeto.FechaLanzamiento.ToString("dd/MM/yyyy"),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.4. ¿Tiene fecha fin de soporte?
                    if (objTec.FlagFechaFinSoporte.ToString() != objeto.FlagFechaFinSoporte.ToString())
                    {
                        string campoPropuesto = "No";
                        if ((bool)objeto.FlagFechaFinSoporte) { campoPropuesto = "Si"; }

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FlagFechaFinSoporte),
                            objTec.FlagFechaFinSoporte.ToString(),
                            objeto.FlagFechaFinSoporte.ToString(),
                            objeto.UsuarioMatricula,
                            campoPropuesto);
                        listaCampos.Add(campoNuevo);
                    }


                    if (objTec.Fuente.ToString() != objeto.Fuente.ToString())
                    {
                        // A.5. [1]-Fuente
                        if (objTec.Fuente == null) { objTec.Fuente = null; }
                        if (objeto.Fuente == null) { objeto.Fuente = null; }

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Fuente),
                            objTec.Fuente.ToString(),
                            objeto.Fuente.ToString(),
                            objeto.UsuarioMatricula,
                            objeto.FuenteStr);
                        listaCampos.Add(campoNuevo);
                    }

                    if (objTec.FechaCalculoTec.ToString() != objeto.FechaCalculoTec.ToString())
                    {
                        // A.6. [1]-Fecha para cálculo de obsolescencia
                        if (objTec.FechaCalculoTec == null) { objTec.FechaCalculoTec = -1; }
                        if (objeto.FechaCalculoTec == null) { objeto.FechaCalculoTec = -1; }

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaCalculo),
                            objTec.FechaCalculoTec.ToString(),
                            objeto.FechaCalculoTec.ToString(),
                            objeto.UsuarioMatricula,
                            objeto.FechaCalculoTecStr);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.6. [1]-Fecha fin extendida de la tecnología
                    if (objTec.FechaExtendida != objeto.FechaExtendida)
                    {
                        string fechaActual = (objTec.FechaExtendida.HasValue) ? objTec.FechaExtendida.Value.ToString("dd/MM/yyyy") : string.Empty;
                        string fechaNuevo = (objeto.FechaExtendida.HasValue) ? objeto.FechaExtendida.Value.ToString("dd/MM/yyyy") : string.Empty;

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaExtendida),
                            fechaActual,
                            fechaNuevo,
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.7. [1]-Fecha fin soporte de la tecnología
                    if (objTec.FechaFinSoporte != objeto.FechaFinSoporte)
                    {
                        string fechaActual = (objTec.FechaFinSoporte.HasValue) ? objTec.FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
                        string fechaNuevo = (objeto.FechaFinSoporte.HasValue) ? objeto.FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaFinSoporte),
                            fechaActual,
                            fechaNuevo,
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.8. [1]-Fecha fin interna de la tecnología
                    if (objTec.FechaAcordada != objeto.FechaAcordada)
                    {
                        string fechaActual = (objTec.FechaAcordada.HasValue) ? objTec.FechaAcordada.Value.ToString("dd/MM/yyyy") : string.Empty;
                        string fechaNuevo = (objeto.FechaAcordada.HasValue) ? objeto.FechaAcordada.Value.ToString("dd/MM/yyyy") : string.Empty;

                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaFinInterna),
                            fechaActual,
                            fechaNuevo,
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // A.9. [1]-Comentarios asociados a la fecha fin de soporte de tecnología
                    if (objTec.ComentariosFechaFin.ToUpper().Trim() != objeto.ComentariosFechaFin.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ComentariosFechaFin),
                            objTec.ComentariosFechaFin.Trim(),
                            objeto.ComentariosFechaFin.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    var oMotivoFechaIndefinida = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetailXId(objTec.SustentoMotivo.ToUpper().Trim(), "MFI");
                    if (!ReferenceEquals(null, oMotivoFechaIndefinida))
                    {
                        // A.10. [2]-Motivo de fecha indefinida
                        if (objTec.SustentoMotivo.ToUpper().Trim() != objeto.SustentoMotivo.ToUpper().Trim())
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                               configuracion,
                               ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida),
                                objTec.SustentoMotivo.ToString(),
                                objeto.SustentoMotivo.ToString(),
                               objeto.UsuarioMatricula,
                                string.Empty);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (objTec.SustentoMotivo.ToUpper().Trim() == "")
                        {
                            if (objTec.SustentoMotivo.ToUpper().Trim() != objeto.SustentoMotivo.ToUpper().Trim())
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                   configuracion,
                                   ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida),
                                    objTec.SustentoMotivo.ToString(),
                                    objeto.SustentoMotivo.ToString(),
                                   objeto.UsuarioMatricula,
                                    string.Empty);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }

                    // A.11. [2]-Url de fecha indefinida
                    if (objTec.SustentoUrl.ToUpper().Trim() != objeto.SustentoUrl.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.UrlFechaIndefinida),
                            objTec.SustentoUrl.Trim(),
                            objeto.SustentoUrl.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    #endregion
                    break;
            }

            return listaCampos;
        }
        
        public override List<SolicitudTecnologiaCamposDTO> InsertarProductoCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, ProductoDTO objPro, TecnologiaDTO objeto)
        {
            // Obtener configuración
            var listaCampos = new List<SolicitudTecnologiaCamposDTO>();
            var campoNuevo = new SolicitudTecnologiaCamposDTO();

            switch (objeto.TipoSave)
            {
                case 1:
                    #region TAB 1: PRODUCTO
                    // 1.- Fabricante 
                    if (objPro.Fabricante.ToUpper().Trim() != objeto.Producto.Fabricante.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Fabricante),
                            objPro.Fabricante.Trim(),
                            objeto.Producto.Fabricante.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // 2.- Nombre Producto 
                    if (objPro.Nombre.ToUpper().Trim() != objeto.Producto.Nombre.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.NombreProducto),
                            objPro.Nombre.Trim(),
                            objeto.Producto.Nombre.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // 7. Dominio
                    //objeto:valornuevo
                    var dataDominio = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewDomainValue(objPro.DominioId);
                    if (!string.IsNullOrEmpty(dataDominio))
                    {
                        if (objPro.DominioId != objeto.Producto.DominioId)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Dominio),
                                objPro.DominioId.ToString(),
                                objeto.Producto.DominioId.ToString(),
                                objeto.UsuarioMatricula,
                                objeto.Producto.DominioStr);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objPro.DominioId.ToString()) && objeto.DominioId != -1)
                        {
                            if (objPro.DominioId != objeto.Producto.DominioId)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                    configuracion,
                                    ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Dominio),
                                    objPro.DominioId.ToString(),
                                    objeto.Producto.DominioId.ToString(),
                                    objeto.UsuarioMatricula,
                                    objeto.Producto.DominioStr);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }

                    // 8. Subdominio
                    var oDataSubDominio = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewSubDomainValue(objPro.SubDominioId);
                    if (!string.IsNullOrEmpty(oDataSubDominio))
                    {
                        if (objPro.SubDominioId != objeto.Producto.SubDominioId)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Subdominio),
                                objPro.SubDominioId.ToString(),
                                objeto.Producto.SubDominioId.ToString(),
                                objeto.UsuarioMatricula,
                                objeto.Producto.SubDominioStr);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objPro.SubDominioId.ToString()) && objeto.SubdominioId != -1)
                        {
                            if (objPro.SubDominioId != objeto.Producto.SubDominioId)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                    configuracion,
                                    ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Subdominio),
                                    objPro.SubDominioId.ToString(),
                                    objeto.Producto.SubDominioId.ToString(),
                                    objeto.UsuarioMatricula,
                                    objeto.Producto.SubDominioStr);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }


                    //var oDataTipoProducto = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(objPro.TipoProductoId);
                    //if (!string.IsNullOrEmpty(oDataTipoProducto))
                    //{
                    //    // 9. Tipo de Producto
                    //    if (objPro.TipoProductoId != objeto.Producto.TipoProductoId)
                    //    {
                    //        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    //            configuracion,
                    //            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoProducto),
                    //            objPro.TipoProductoId.ToString(),
                    //            objeto.Producto.TipoProductoId.ToString(),
                    //            objeto.UsuarioMatricula,
                    //            objeto.Producto.TipoProductoStr);
                    //        listaCampos.Add(campoNuevo);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(objPro.TipoProductoId.ToString()) && objeto.TipoProductoId != -1)
                    //    {
                    //        if (objPro.TipoProductoId != objeto.Producto.TipoProductoId)
                    //        {
                    //            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    //                configuracion,
                    //                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoProducto),
                    //                objPro.TipoProductoId.ToString(),
                    //                objeto.Producto.TipoProductoId.ToString(),
                    //                objeto.UsuarioMatricula,
                    //                objeto.Producto.TipoProductoStr);
                    //            listaCampos.Add(campoNuevo);
                    //        }
                    //    }
                    //}

                    var oDataTipoCicloVida = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueTypeLifeCycle(objPro.TipoCicloVidaId);
                    if (!string.IsNullOrEmpty(oDataTipoCicloVida))
                    {
                        // 10. Tipo de Ciclo de Vida
                        if (objPro.TipoCicloVidaId == null) { objPro.TipoCicloVidaId = -1; }
                        if (objeto.Producto.TipoCicloVidaId == null) { objeto.Producto.TipoCicloVidaId = -1; }
                        if (objPro.TipoCicloVidaId != objeto.Producto.TipoCicloVidaId)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida),
                                objPro.TipoCicloVidaId.ToString(),
                                objeto.Producto.TipoCicloVidaId.ToString(),
                                objeto.UsuarioMatricula,
                                objeto.Producto.TipoCicloVidaStr);
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objPro.TipoCicloVidaId.ToString()) && objeto.TipoCicloVidaId != -1)
                        {
                            if (objPro.TipoCicloVidaId == null) { objPro.TipoCicloVidaId = -1; }
                            if (objeto.Producto.TipoCicloVidaId == null) { objeto.Producto.TipoCicloVidaId = -1; }
                            if (objPro.TipoCicloVidaId != objeto.Producto.TipoCicloVidaId)
                            {
                                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                    configuracion,
                                    ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida),
                                    objPro.TipoCicloVidaId.ToString(),
                                    objeto.Producto.TipoCicloVidaId.ToString(),
                                    objeto.UsuarioMatricula,
                                    objeto.Producto.TipoCicloVidaStr);
                                listaCampos.Add(campoNuevo);
                            }
                        }
                    }
                    #endregion
                    break;
                case 4:
                    #region TAB 2: Responsabilidades
                    // B1. Tribu/COE/Unidad Organizacional
                    if (objPro.TribuCoeId.Trim() != objeto.Producto.TribuCoeId.Trim())
                    {
                        // TribuCoeId
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TribuCOE),
                            objPro.TribuCoeId.ToString(),
                            objeto.Producto.TribuCoeId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);

                        // TribuCoeDisplayName
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TribuCOENombre),
                            objPro.TribuCoeDisplayName.ToString(),
                            objeto.Producto.TribuCoeDisplayName.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);

                    }

                    // B3. SQUAD
                    if (objPro.SquadId.Trim() != objeto.Producto.SquadId.Trim())
                    {
                        //SquadId
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.SquadEquipo),
                            objPro.SquadId.ToString(),
                            objeto.Producto.SquadId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);

                        // SquadDisplayName
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.SquadEquipoNombre),
                            objPro.SquadDisplayName.ToString(),
                            objeto.Producto.SquadDisplayName.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // B4. Responsable de SQUAD
                    if (objPro.OwnerId.Trim() != objeto.Producto.OwnerId.Trim())
                    {
                        // OwnerId
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquad),
                            objPro.OwnerId.ToString(),
                            objeto.Producto.OwnerId.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);

                        // OwnerMatricula
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula),
                            objPro.OwnerMatricula.ToString(),
                            objeto.Producto.OwnerMatricula.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);

                        // OwnerDisplayName
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadNombre),
                            objPro.OwnerDisplayName.ToString(),
                            objeto.Producto.OwnerDisplayName.ToString(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // B6. Equipo de Aprovisionamiento
                    if (objPro.EquipoAprovisionamiento.ToUpper().Trim() != objeto.EquipoAprovisionamiento.ToUpper().Trim())
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EquipoAprovisionamiento),
                            objPro.EquipoAprovisionamiento.Trim(),
                            objeto.EquipoAprovisionamiento.Trim(),
                            objeto.UsuarioMatricula,
                            string.Empty);
                        listaCampos.Add(campoNuevo);
                    }

                    // B7. Grupo de Soporte Remedy
                    if (objPro.GrupoTicketRemedyId == null) { objPro.GrupoTicketRemedyId = -1; }
                    if (objeto.Producto.GrupoTicketRemedyId == null) { objeto.Producto.GrupoTicketRemedyId = -1; }
                    if (objPro.GrupoTicketRemedyId != objeto.Producto.GrupoTicketRemedyId)
                    {
                        campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                           configuracion,
                           ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.GrupoSoporteRemedy),
                            objPro.GrupoTicketRemedyId.ToString(),
                            objeto.Producto.GrupoTicketRemedyId.ToString(),
                            objeto.UsuarioMatricula,
                            objeto.Producto.GrupoTicketRemedyNombre.Trim());
                        listaCampos.Add(campoNuevo);
                    }

                    // B8. Expertos
                    if (objeto.ListExpertos != null)
                    {
                        foreach (var item in objeto.ListExpertos)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                                configuracion,
                                ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ListaExpertos),
                                string.Empty,
                                (item.ManagerMatricula + "|" + item.ManagerNombre + "|" + item.ManagerEmail + "|" + item.ProductoManagerId.ToString()), // ManagerMatricula | ManagerNombre | ManagerEmail | ProductoManagerId
                                objeto.UsuarioMatricula,
                                string.Empty);
                            listaCampos.Add(campoNuevo);
                        }
                    }

                    if (objeto.ListExpertosEliminar != null)
                    {
                        foreach (var item in objeto.ListExpertosEliminar)
                        {
                            campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                            configuracion,
                            ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ListaExpertosEliminar),
                            (item.ManagerMatricula + "|" + item.ProductoManagerId.ToString()), // ManagerMatricula | ProductoManagerId
                            string.Empty,
                            objeto.UsuarioMatricula,
                            string.Empty); // Nombre de Equivalencia
                            listaCampos.Add(campoNuevo);
                        }
                    }
                    #endregion
                    break;
            }
            
            return listaCampos;
        }

        public override List<SolicitudTecnologiaCamposDTO> InsertarTecnologiaEquivalenciaCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, TecnologiaDTO objTec, TecnologiaDTO objeto)
        {
            // Obtener configuración
            List<SolicitudTecnologiaCamposDTO> listaCampos = new List<SolicitudTecnologiaCamposDTO>();
            var campoNuevo = new SolicitudTecnologiaCamposDTO();

            //// 12. Tiene equivalencias
            if (objTec.FlagTieneEquivalencias != objeto.FlagTieneEquivalencias || objTec.FlagTieneEquivalencias == objeto.FlagTieneEquivalencias)
            {
                string campoPropuesto = "No";
                if (objeto.FlagTieneEquivalencias)
                {
                    campoPropuesto = "Si";
                }

                var idTechnologyConfiguration = configuracion.Where(x => x.Id == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias).FirstOrDefault(); 

                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    configuracion,
                    idTechnologyConfiguration.ConfiguracionTecnologiaCamposId,
                    objTec.FlagTieneEquivalencias.ToString(),
                    objeto.FlagTieneEquivalencias.ToString(),
                    objeto.UsuarioMatricula,
                    campoPropuesto);
                listaCampos.Add(campoNuevo);
            }

            // 13 Motivo por el cual no tiene equivalencias
            if (objTec.MotivoId == null)
                objTec.MotivoId = -1;

            if (objTec.MotivoId != objeto.MotivoId || objTec.MotivoId == objeto.MotivoId)
            {
                // 13.1 Tiene equivalencias
                var id = configuracion.Where(x => x.Id == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias).FirstOrDefault();

                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    configuracion,
                    id.ConfiguracionTecnologiaCamposId,
                    objTec.FlagTieneEquivalencias.ToString().Trim(),
                    (objeto.FlagTieneEquivalencias.ToString().Trim() == "True" ? string.Empty : objeto.FlagTieneEquivalencias.ToString()),
                    objeto.UsuarioMatricula,
                    "No");
                listaCampos.Add(campoNuevo);

                var idTechnologyConfiguration = configuracion.Where(x => x.Id == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias).FirstOrDefault();
                campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    configuracion,
                    idTechnologyConfiguration.ConfiguracionTecnologiaCamposId,
                    objTec.MotivoId.ToString(),
                    objeto.MotivoId.ToString(),
                    objeto.UsuarioMatricula,
                    objeto.MotivosSinEquivalenciasStr);
                listaCampos.Add(campoNuevo);
            }

            // 14. Agregar equivalencias
            if (objeto.ListEquivalencias != null)
            {
                char[] charsToTrim = { ' ' };
                foreach (var item in objeto.ListEquivalencias)
                {
                    var idTechnologyConfiguration = configuracion.Where(x => x.Id == (int)FlujoConfiguracionTecnologiaCampos.ActualizarEquivalencias).FirstOrDefault();
                    campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    configuracion,
                    idTechnologyConfiguration.ConfiguracionTecnologiaCamposId,
                    string.Empty,
                    item.Nombre.ToString().Trim(charsToTrim),
                    objeto.UsuarioMatricula,
                    string.Empty);
                    listaCampos.Add(campoNuevo);
                }
            }

            // 15. Quitar equivalencias
            if (objeto.ListEquivalenciasEliminar != null)
            {
                foreach (var item in objeto.ListEquivalenciasEliminar)
                {
                    var idTechnologyConfiguration = configuracion.Where(x => x.Id == (int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias).FirstOrDefault();
                    campoNuevo = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampoActualizacion(
                    configuracion,
                    idTechnologyConfiguration.ConfiguracionTecnologiaCamposId,
                    item.Id.ToString(),
                    string.Empty,
                    objeto.UsuarioMatricula,
                    item.Nombre); // Nombre de Equivalencia
                    listaCampos.Add(campoNuevo);
                }
            }
         
            return listaCampos;
        }
        public override TecnologiaDTO AcondicionamientoTecnologia(TecnologiaDTO objeto)
        {
            // Verificación de nulos
            switch (objeto.TipoSave)
            {
                case 1:
                    objeto.Producto.Fabricante = string.IsNullOrEmpty(objeto.Producto.Fabricante) ? string.Empty : objeto.Producto.Fabricante;
                    objeto.Nombre = string.IsNullOrEmpty(objeto.Nombre) ? string.Empty : objeto.Nombre;
                    objeto.Producto.Descripcion = string.IsNullOrEmpty(objeto.Producto.Descripcion) ? string.Empty : objeto.Producto.Descripcion;
                    objeto.Producto.TipoCicloVidaId = (objeto.Producto.TipoCicloVidaId == null) ? -1 : objeto.Producto.TipoCicloVidaId;
                    objeto.Producto.EsquemaLicenciamientoSuscripcionId = (objeto.Producto.EsquemaLicenciamientoSuscripcionId == null) ? -1 : objeto.Producto.EsquemaLicenciamientoSuscripcionId;
                    objeto.AutomatizacionImplementadaId = (objeto.AutomatizacionImplementadaId == null) ? -1 : objeto.AutomatizacionImplementadaId;
                    break;
                case 2:
                    objeto.Versiones = string.IsNullOrEmpty(objeto.Versiones) ? string.Empty : objeto.Versiones;
                    objeto.ClaveTecnologia = string.IsNullOrEmpty(objeto.ClaveTecnologia) ? string.Empty : objeto.ClaveTecnologia;
                    objeto.Descripcion = string.IsNullOrEmpty(objeto.Descripcion) ? string.Empty : objeto.Descripcion;
                    objeto.UrlConfluenceId = (objeto.UrlConfluenceId == null) ? -1 : objeto.UrlConfluenceId;
                    objeto.UrlConfluence = string.IsNullOrEmpty(objeto.UrlConfluence) ? string.Empty : objeto.UrlConfluence;
                    objeto.CasoUso = string.IsNullOrEmpty(objeto.CasoUso) ? string.Empty : objeto.CasoUso;
                    objeto.Aplica = string.IsNullOrEmpty(objeto.Aplica) ? string.Empty : objeto.Aplica;
                    objeto.CompatibilidadSOId = string.IsNullOrEmpty(objeto.CompatibilidadSOId) ? string.Empty : objeto.CompatibilidadSOId;
                    objeto.CompatibilidadCloudId = string.IsNullOrEmpty(objeto.CompatibilidadCloudId) ? string.Empty : objeto.CompatibilidadCloudId;
                    objeto.EsqMonitoreo = string.IsNullOrEmpty(objeto.EsqMonitoreo) ? string.Empty : objeto.EsqMonitoreo;
                    break;
                case 3:
                    objeto.Fuente = (objeto.Fuente == null) ? null : objeto.Fuente;
                    objeto.FechaCalculoTec = (objeto.FechaCalculoTec == null) ? null : objeto.FechaCalculoTec;
                    objeto.ComentariosFechaFin = string.IsNullOrEmpty(objeto.ComentariosFechaFin) ? string.Empty : objeto.ComentariosFechaFin;
                    objeto.SustentoMotivo = string.IsNullOrEmpty(objeto.SustentoMotivo) ? string.Empty : objeto.SustentoMotivo;
                    objeto.SustentoUrl = string.IsNullOrEmpty(objeto.SustentoUrl) ? string.Empty : objeto.SustentoUrl;
                    break;
                case 4:
                    objeto.Producto.TribuCoeId = string.IsNullOrEmpty(objeto.Producto.TribuCoeId) ? string.Empty : objeto.Producto.TribuCoeId;
                    objeto.Producto.TribuCoeDisplayName = string.IsNullOrEmpty(objeto.Producto.TribuCoeDisplayName) ? string.Empty : objeto.Producto.TribuCoeDisplayName;
                    objeto.Producto.SquadId = string.IsNullOrEmpty(objeto.Producto.SquadId) ? string.Empty : objeto.Producto.SquadId;
                    objeto.Producto.SquadDisplayName = string.IsNullOrEmpty(objeto.Producto.SquadDisplayName) ? string.Empty : objeto.Producto.SquadDisplayName;
                    objeto.Producto.OwnerId = string.IsNullOrEmpty(objeto.Producto.OwnerId) ? string.Empty : objeto.Producto.OwnerId;
                    objeto.Producto.OwnerMatricula = string.IsNullOrEmpty(objeto.Producto.OwnerMatricula) ? string.Empty : objeto.Producto.OwnerMatricula;
                    objeto.Producto.OwnerDisplayName = string.IsNullOrEmpty(objeto.Producto.OwnerDisplayName) ? string.Empty : objeto.Producto.OwnerDisplayName;
                    objeto.EquipoAprovisionamiento = string.IsNullOrEmpty(objeto.EquipoAprovisionamiento) ? string.Empty : objeto.EquipoAprovisionamiento;
                    objeto.Producto.GrupoTicketRemedyId = (objeto.Producto.GrupoTicketRemedyId == null) ? -1 : objeto.Producto.GrupoTicketRemedyId;
                    break;
            }

            return objeto;
        }
        public override int EditNewTecnologia(FlujoActualizacionTecnologiaCamposDTO entrada)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 0;
            var CURRENT_DATE = DateTime.Now;
            var objeto = new FlujoActualizacionTecnologiaCamposDTO();
            string NombreTecnologiaTitulo = string.Empty;

            var technologyData = ServiceManager<TechnologyConfigurationDAO>.Provider.GetRequestFlowDetail(entrada.SolicitudId);
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                       if (entrada.TecnologiaId != 0)
                        {
                            var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == entrada.TecnologiaId);
                            if (entidad != null)
                            {

                                switch (entrada.TipoSolicitudId)     // Match Expression - can be any non-null expression
                                {
                                    case (int)FlujoTipoSolicitud.Actualizacion:

                                        NombreTecnologiaTitulo = "de actualización de tecnología - ";
                                        // Obtener Datos de Solicitud
                                        objeto = ServiceManager<TecnologiaDAO>.Provider.ObtenerDatosDeSolicitudParaActualizar(entrada.SolicitudId, entrada.TipoSolicitudId, entrada.TecnologiaId, entrada.UsuarioModificacion);
                                        entidad.ProductoId = objeto.ProductoId;

                                        if (objeto.UsoProducto)
                                        {
                                            var producto = ctx.Producto.FirstOrDefault(x => x.ProductoId == objeto.ProductoId);

                                            producto.Fabricante = objeto.Fabricante;
                                            producto.Descripcion = objeto.Descripcion;
                                            producto.DominioId = objeto.DominioId;
                                            producto.SubDominioId = objeto.SubdominioId;
                                            producto.TipoProductoId = (int)objeto.TipoProductoId;
                                            producto.TipoCicloVidaId = objeto.TipoCicloVidaId;
                                            producto.EsquemaLicenciamientoSuscripcionId = objeto.EsqLicenciamientoId;

                                            if (!ReferenceEquals(null, technologyData))
                                            {
                                                foreach (var flujoDetalleDTO in technologyData)
                                                {

                                                    switch (flujoDetalleDTO.CorrelativoCampo)
                                                    {
                                                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOE:
                                                            producto.TribuCoeId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOENombre:
                                                            producto.TribuCoeDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquad:
                                                            producto.OwnerId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula:
                                                            producto.OwnerMatricula = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadNombre:
                                                            producto.OwnerDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipo:
                                                            producto.SquadId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipoNombre:
                                                            producto.SquadDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                producto.TribuCoeId = objeto.TribuCoeId;
                                                producto.TribuCoeDisplayName = objeto.TribuCoeDisplayName;
                                                producto.SquadId = objeto.SquadId;
                                                producto.SquadDisplayName = objeto.SquadDisplayName;
                                                producto.OwnerId = objeto.OwnerId;
                                                producto.OwnerMatricula = objeto.OwnerMatricula;
                                                producto.OwnerDisplayName = objeto.OwnerNombre;
                                            }

                                            producto.ModificadoPor = objeto.UsuarioModificacion;
                                            producto.FechaModificacion = DateTime.Now;

                                        }

                                        var list_techonology = new List<int>();


                                        foreach (var y in technologyData)
                                        {
                                            switch (y.ConfiguracionTecnologiaCamposId)
                                            {
                                                case (int)FlujoConfiguracionTecnologiaCampos.Dominio:
                                                    list_techonology.Add((int)y.ConfiguracionTecnologiaCamposId);
                                                    break;
                                                case (int)FlujoConfiguracionTecnologiaCampos.TipoProducto:
                                                    list_techonology.Add((int)y.ConfiguracionTecnologiaCamposId);
                                                    break;
                                                case (int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida:
                                                    list_techonology.Add((int)y.ConfiguracionTecnologiaCamposId);
                                                    break;
                                            }
                                        }

                                        var itemRemove = technologyData.Where(x => list_techonology.Contains((int)x.ConfiguracionTecnologiaCamposId)).ToList();
                                        technologyData = technologyData.Except(itemRemove).ToList();

                                        if (technologyData != null)
                                        {
                                            technologyData.ForEach(k =>
                                            {
                                                switch (k.CorrelativoCampo)
                                                {
                                                    case (int)FlujoConfiguracionTecnologiaCampos.ClaveTecnologia:
                                                        entidad.ClaveTecnologia = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.Fabricante:
                                                        entidad.Fabricante = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias:
                                                        entidad.FlagTieneEquivalencias = (string.IsNullOrEmpty(k.ValorNuevo) ? bool.Parse(k.ValorAnterior) : bool.Parse(k.ValorNuevo));
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.NombreTecnologia:
                                                        entidad.Nombre = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.Version:
                                                        entidad.Versiones = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.DescripcionTecnologia:
                                                        entidad.Descripcion = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    //case (int)FlujoConfiguracionTecnologiaCampos.Dominio:
                                                    //    entidad.DominioId = Int32.Parse(k.ValorNuevo);
                                                    //    break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.Subdominio:
                                                        entidad.SubdominioId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia:
                                                        entidad.TipoTecnologia = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId:
                                                        entidad.AutomatizacionImplementadaId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaLanzamiento:
                                                        if (!string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            DateTime? fecha_lanzamiento = DateTime.ParseExact(k.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            entidad.FechaLanzamiento = fecha_lanzamiento;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaLanzamiento = null;
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FlagFechaFinSoporte:
                                                        entidad.FlagFechaFinSoporte = bool.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.Fuente:
                                                        if (string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            entidad.FuenteId = null;
                                                        }
                                                        else
                                                        {
                                                            entidad.FuenteId = Int32.Parse(k.ValorNuevo);
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaCalculo:
                                                        if (string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            entidad.FechaCalculoTec = null;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaCalculoTec = Int32.Parse(k.ValorNuevo);
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaExtendida:
                                                        if (!string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            DateTime? fechaExtendida = DateTime.ParseExact(k.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            entidad.FechaExtendida = fechaExtendida;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaExtendida = null;
                                                        }

                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaFinSoporte:
                                                        if (!string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            DateTime? fechaFinSoporte = DateTime.ParseExact(k.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            entidad.FechaFinSoporte = fechaFinSoporte;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaFinSoporte = null;
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaFinInterna:
                                                        if (!string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            DateTime? fechaAcordada = DateTime.ParseExact(k.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            entidad.FechaAcordada = fechaAcordada;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaAcordada = null;
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.ComentariosFechaFin:
                                                        entidad.ComentariosFechaFin = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida:
                                                        entidad.SustentoMotivo = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.UrlFechaIndefinida:
                                                        entidad.SustentoUrl = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.UrlConfluenceId:
                                                        entidad.UrlConfluenceId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.UrlConfluence:
                                                        entidad.UrlConfluence = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.CasoUso:
                                                        entidad.CasoUso = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma:
                                                        entidad.Aplica = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.CompatibilidadSoId:
                                                        entidad.CompatibilidadSOId = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.CompatibilidadCloudId:
                                                        entidad.CompatibilidadCloudId = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.EsquemaMonitoreo:
                                                        entidad.EsqMonitoreo = (string.IsNullOrEmpty(k.ValorNuevo) ? string.Empty : k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.EstadoId:
                                                        entidad.EstadoId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FechaDeprecada:
                                                        if (!string.IsNullOrEmpty(k.ValorNuevo))
                                                        {
                                                            DateTime? fechaDep = DateTime.ParseExact(k.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            entidad.FechaDeprecada = fechaDep;
                                                        }
                                                        else
                                                        {
                                                            entidad.FechaDeprecada = null;
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId:
                                                        entidad.TecReemplazoDepId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                }


                                            });
                                        }
                                        else
                                        {
                                            entidad.ClaveTecnologia = objeto.ClaveTecnologia;
                                            entidad.Fabricante = objeto.Fabricante;
                                            entidad.FlagTieneEquivalencias = objeto.FlagTieneEquivalencias;
                                            // ==== Datos Principales
                                            //
                                            entidad.Nombre = objeto.Nombre;  // 2.- Nombre de Tecnología
                                            entidad.Versiones = objeto.Versiones;   // 3.- Versión

                                            entidad.Descripcion = objeto.Descripcion;   // 5.- Descripción de la tecnología
                                            entidad.SubdominioId = objeto.SubdominioId;   // 8. Subdominio
                                                                                          //entidad.FlagSiteEstandar = objeto.FlagSiteEstandar;
                                                                                          //entidad.FlagTieneEquivalencias = objeto.FlagTieneEquivalencias;
                                                                                          //entidad.MotivoId = objeto.MotivoId;

                                            // ============TAB 1: GENERAL ============
                                            entidad.TipoTecnologia = objeto.TipoTecnologiaId; // A.1. Tipo de Tecnología
                                            entidad.AutomatizacionImplementadaId = objeto.AutomatizacionImplementadaId;// A.2. ¿La tecnología tiene script que automatiza su implementación?
                                            entidad.FechaLanzamiento = objeto.FechaLanzamiento; // A.3. Fecha de Lanzamiento de Tecnología
                                            entidad.FlagFechaFinSoporte = objeto.FlagFechaFinSoporte; // A.4. ¿Tiene fecha fin de soporte?
                                            entidad.FuenteId = objeto.Fuente; // A.5. [1]-Fuente
                                            entidad.FechaCalculoTec = objeto.FechaCalculoTec; // A.6. [1]-Fecha para cálculo de obsolescencia
                                            entidad.FechaExtendida = objeto.FechaExtendida; // A.6. [1]-Fecha fin extendida de la tecnología
                                            entidad.FechaFinSoporte = objeto.FechaFinSoporte; // A.7. [1]-Fecha fin soporte de la tecnología
                                            entidad.FechaAcordada = objeto.FechaAcordada; // A.8. [1]-Fecha fin interna de la tecnología
                                            entidad.ComentariosFechaFin = objeto.ComentariosFechaFin; // A.9. [1]-Comentarios asociados a la fecha fin de soporte de tecnología
                                            entidad.SustentoMotivo = objeto.SustentoMotivo; // A.10. [2]-Motivo de fecha indefinida
                                            entidad.SustentoUrl = objeto.SustentoUrl; // A.11. [2]-Url de fecha indefinida

                                            entidad.UrlConfluenceId = objeto.UrlConfluenceId; // A.12. Revisión de Lineamiento de Tecnología (url confluence)
                                            entidad.UrlConfluence = objeto.UrlConfluence; // A.13. URL Lineamiento de Tecnología
                                            entidad.CasoUso = objeto.CasoUso; // A.14. Caso de uso
                                            entidad.Aplica = objeto.Aplica; // A.15. Indicar la Plataforma a la que aplica
                                            entidad.CompatibilidadSOId = objeto.CompatibilidadSOId; // A.16. Compatibilidad de SO
                                            entidad.CompatibilidadCloudId = objeto.CompatibilidadCloudId; // A.17. Compatibilidad Cloud
                                            entidad.EsqMonitoreo = objeto.EsqMonitoreo; // A.18. Esquema de Monitoreo
                                            entidad.EstadoId = objeto.EstadoId; // A.19. Estado Tecnología
                                            entidad.FechaDeprecada = objeto.FechaDeprecada; // A.20. Fecha deprecada
                                            entidad.TecReemplazoDepId = objeto.TecReemplazoDepId;   // A.21. Tecnología Reemplazo Deprecada
                                            // ============== TAB 2: Responsabilidades ============
                                            entidad.DuenoId = objeto.OwnerId;
                                        }


                                        // ============== TAB 3 - Aplicaciones ============
                                        if (objeto.ListAplicaciones != null && objeto.EstadoId == (int)ETecnologiaEstado.Restringido)
                                        {
                                            foreach (var item in objeto.ListAplicaciones)
                                            {
                                                var aplicacion = new TecnologiaAplicacion
                                                {
                                                    TecnologiaId = entidad.TecnologiaId,
                                                    AplicacionId = Int32.Parse(item.CodigoAPT), // item.AplicacionId
                                                    FlagActivo = true,
                                                    FlagEliminado = false,
                                                    CreadoPor = objeto.UsuarioModificacion,
                                                    FechaCreacion = DateTime.Now
                                                };

                                                ctx.TecnologiaAplicacion.Add(aplicacion);
                                                ctx.SaveChanges();
                                            }
                                        }

                                        if (!ReferenceEquals(null, technologyData))
                                        {
                                            technologyData.ForEach(i =>
                                            {
                                                if (i.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada)
                                                {
                                                    var idAplicacion = Int32.Parse(i.ValorAnterior);
                                                    var item = ctx.TecnologiaAplicacion.FirstOrDefault(x => x.TecnologiaId == entrada.TecnologiaId && x.AplicacionId == idAplicacion && x.FlagActivo);
                                                    if (!ReferenceEquals(null, item))
                                                    {
                                                        item.FlagActivo = false;
                                                        item.FlagEliminado = true;
                                                        item.ModificadoPor = objeto.UsuarioModificacion;
                                                        item.FechaModificacion = DateTime.Now;
                                                        ctx.SaveChanges();
                                                    };
                                                }
                                            });
                                        }

                                        entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                                        entidad.FechaModificacion = DateTime.Now;
                                        ctx.SaveChanges();
                                        break;

                                    case (int)FlujoTipoSolicitud.Equivalencias:
                                        NombreTecnologiaTitulo = "para agregar y/o eliminar equivalencias de tecnología - ";
                                        objeto = ServiceManager<TecnologiaDAO>.Provider.ObtenerDatosDeSolicitudParaEquivalencias(entrada.SolicitudId, entrada.TipoSolicitudId, entrada.TecnologiaId, entrada.UsuarioModificacion);
                                        var position = 0;
                                        if (technologyData != null)
                                        {
                                            #region Campos Generales
                                            foreach (var k in technologyData)
                                            {

                                                switch (k.CorrelativoCampo)
                                                {
                                                    case (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias:
                                                        if (position == 1)
                                                        {
                                                            var flagEquivalence = technologyData[1].CorrelativoCampo;
                                                            var idsCorrelative = technologyData.Select(tec => tec.CorrelativoCampo).ToList();
                                                            if (idsCorrelative.Contains((int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias))
                                                            {
                                                                var valor = technologyData.Where(h => h.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias).FirstOrDefault();

                                                                if (valor.ValorNuevo == "-1")
                                                                    entidad.FlagTieneEquivalencias = true;
                                                                else
                                                                    entidad.FlagTieneEquivalencias = (string.IsNullOrEmpty(k.ValorNuevo) ? bool.Parse(k.ValorAnterior) : bool.Parse(k.ValorNuevo));
                                                            }
                                                            else
                                                            {
                                                                entidad.FlagTieneEquivalencias = (string.IsNullOrEmpty(k.ValorNuevo) ? bool.Parse(k.ValorAnterior) : bool.Parse(k.ValorNuevo));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            entidad.FlagTieneEquivalencias = (string.IsNullOrEmpty(k.ValorNuevo) ? bool.Parse(k.ValorAnterior) : bool.Parse(k.ValorNuevo));
                                                        }
                                                        break;
                                                    case (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias:
                                                        entidad.MotivoId = Int32.Parse(k.ValorNuevo);
                                                        break;
                                                }

                                                ctx.SaveChanges();
                                                position++;
                                            }
                                            #endregion

                                            #region Registro de Equivalencias
                                            if (objeto.ListEquivalencias != null)
                                            {

                                                foreach (var item in objeto.ListEquivalencias)
                                                {
                                                    var equivalencia = new TecnologiaEquivalencia
                                                    {
                                                        TecnologiaId = entidad.TecnologiaId,
                                                        Nombre = item.Nombre.ToString().Trim(),
                                                        FlagActivo = true,
                                                        CreadoPor = objeto.UsuarioModificacion,
                                                        FechaCreacion = DateTime.Now
                                                    };

                                                    ctx.TecnologiaEquivalencia.Add(equivalencia);
                                                    ctx.SaveChanges();
                                                }
                                            }
                                            #endregion

                                            #region Eliminar Equivalencias
                                            if (objeto.ItemsRemoveEqTecId != null)
                                            {
                                                foreach (int id in objeto.ItemsRemoveEqTecId)
                                                {
                                                    var item = ctx.TecnologiaEquivalencia.FirstOrDefault(x => x.TecnologiaEquivalenciaId == id);
                                                    if (item != null)
                                                    {
                                                        item.FlagActivo = false;
                                                        item.ModificadoPor = objeto.UsuarioModificacion;
                                                        item.FechaModificacion = DateTime.Now;
                                                    };
                                                    ctx.SaveChanges();
                                                }
                                            }
                                            #endregion

                                        }
                                        break;

                                    case (int)FlujoTipoSolicitud.Desactivacion:

                                        NombreTecnologiaTitulo = "de desactivación de tecnología - ";
                                        objeto = ServiceManager<TecnologiaDAO>.Provider.ObtenerDatosDeSolicitudParaDesactivacion(entrada.SolicitudId, entrada.TipoSolicitudId, entrada.TecnologiaId, entrada.UsuarioModificacion);
                                        //objeto.ProductoId  

                                        entidad.Activo = objeto.ActivoTecnologia;
                                        entidad.FechaModificacion = DateTime.Now;
                                        entidad.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper();
                                        ctx.SaveChanges();

                                        //Desactivar el producto
                                        var oProducto = ctx.Tecnologia.Where(d => d.TecnologiaId == entrada.TecnologiaId).FirstOrDefault();
                                        if (!ReferenceEquals(null, oProducto))
                                        {
                                            DesactivarProductoXTecnologia((int)oProducto.ProductoId, (int)entrada.TecnologiaId);
                                        }

                                        break;
                                }

                                // ========================================
                                //  Actualizar Flujos
                                var flujos = ctx.SolicitudTecnologiaFlujo.FirstOrDefault(x => x.SolicitudTecnologiaId == entrada.SolicitudId);
                                flujos.EstadoFlujo = (int)FlujoEstadoSolicitud.Aprobado;
                                flujos.AprobadoPor = entrada.UsuarioModificacion.ToUpper();
                                flujos.FechaAprobacion = DateTime.Now;
                                flujos.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper();
                                flujos.FechaModificacion = DateTime.Now;

                                // Actualizar Detalle
                                var detalle = ctx.SolicitudTecnologiaCampos.Where(x => x.SolicitudTecnologiaId == entrada.SolicitudId).ToList();
                                detalle.ForEach(x => x.EstadoCampo = (int)FlujoEstadoSolicitud.Aprobado);
                                detalle.ForEach(x => x.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper());
                                detalle.ForEach(x => x.FechaModificacion = DateTime.Now);

                                // Actualizar Cabecera
                                var cabecera = ctx.SolicitudTecnologia.FirstOrDefault(x => x.SolicitudTecnologiaId == entrada.SolicitudId);
                                cabecera.EstadoSolicitud = (int)FlujoEstadoSolicitud.Aprobado;
                                cabecera.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper();
                                cabecera.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();
                                // ========================================
                                //==== Envío de correo
                                string listCorreo = cabecera.EmailSolicitante;
                                var listaPara = string.IsNullOrWhiteSpace(listCorreo) ? null : listCorreo.Split(';').ToList();
                                var listConCopia = string.IsNullOrWhiteSpace(entrada.UsuarioMail) ? null : entrada.UsuarioMail.Split(';').ToList();

                                try
                                {
                                    string NombreCorreo = string.Empty;
                                    if (entrada.TipoSolicitudId == (int)FlujoTipoSolicitud.Desactivacion)
                                    {
                                        NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_APROBACION_ELIMINACION";
                                        NombreTecnologiaTitulo = entidad.ClaveTecnologia; //[NombreTecnologia]
                                    }
                                    else
                                    {
                                        NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_APROBACION";
                                        NombreTecnologiaTitulo = NombreTecnologiaTitulo + entidad.ClaveTecnologia; //[NombreTecnologia]
                                    }

                                    string SolicitudId = entrada.SolicitudId.ToString(); //[SolicitudId]
                                    string NombreTecnologia = entidad.ClaveTecnologia;

                                    var mailManager = new MailingManager();
                                    var diccionario = new Dictionary<string, string>();
                                    diccionario.Add("[NombreTecnologia]", NombreTecnologia);
                                    diccionario.Add("[SolicitudId]", SolicitudId);

                                    mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologiaTitulo, listaPara, listConCopia);
                                }
                                catch (Exception ex)
                                {
                                    HelperLog.Error(ex.Message);
                                }

                                ID = entidad.TecnologiaId;
                            }
                        } 
                        else
                        {
                            var entidad = ctx.Producto.FirstOrDefault(x => x.ProductoId == entrada.ProductoId);
                            if (entidad != null)
                            {

                                switch (entrada.TipoSolicitudId)     // Match Expression - can be any non-null expression
                                {
                                    case (int)FlujoTipoSolicitud.Actualizacion:

                                        NombreTecnologiaTitulo = "de actualización de producto - ";
                                        // Obtener Datos de Solicitud
                                        objeto = ServiceManager<TecnologiaDAO>.Provider.ObtenerDatosDeSolicitudProductoParaActualizar(entrada.SolicitudId, entrada.TipoSolicitudId, entrada.ProductoId.HasValue ? entrada.ProductoId.Value : 0, entrada.UsuarioModificacion);

                                        if (objeto.UsoProducto)
                                        {
                                            entidad.Fabricante = objeto.Producto.Fabricante;
                                            entidad.Nombre = objeto.Producto.Nombre;
                                            entidad.DominioId = objeto.Producto.DominioId;
                                            entidad.SubDominioId = objeto.Producto.SubDominioId;
                                            entidad.TipoProductoId = (int)objeto.Producto.TipoProductoId;
                                            entidad.TipoCicloVidaId = objeto.Producto.TipoCicloVidaId;
                                            entidad.EsquemaLicenciamientoSuscripcionId = objeto.Producto.EsquemaLicenciamientoSuscripcionId;

                                            if (!ReferenceEquals(null, technologyData))
                                            {
                                                foreach (var flujoDetalleDTO in technologyData)
                                                {

                                                    switch (flujoDetalleDTO.CorrelativoCampo)
                                                    {
                                                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOE:
                                                            entidad.TribuCoeId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOENombre:
                                                            entidad.TribuCoeDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquad:
                                                            entidad.OwnerId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula:
                                                            entidad.OwnerMatricula = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadNombre:
                                                            entidad.OwnerDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipo:
                                                            entidad.SquadId = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipoNombre:
                                                            entidad.SquadDisplayName = flujoDetalleDTO.ValorNuevo;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                entidad.TribuCoeId = objeto.TribuCoeId;
                                                entidad.TribuCoeDisplayName = objeto.TribuCoeDisplayName;
                                                entidad.SquadId = objeto.SquadId;
                                                entidad.SquadDisplayName = objeto.SquadDisplayName;
                                                entidad.OwnerId = objeto.OwnerId;
                                                entidad.OwnerMatricula = objeto.OwnerMatricula;
                                                entidad.OwnerDisplayName = objeto.OwnerNombre;
                                            }

                                            entidad.ModificadoPor = objeto.UsuarioModificacion;
                                            entidad.FechaModificacion = DateTime.Now;

                                        }

                                        entidad.FechaModificacion = DateTime.Now;
                                        ctx.SaveChanges();
                                        // GUARDAR TECNOLOGIAS ASOCIADAS
                                        var tecnologias = ctx.Tecnologia.Where(x => x.ProductoId == entrada.ProductoId && x.Activo).ToList();
                                        tecnologias.ForEach(
                                            x =>
                                            {
                                                x.SubdominioId = objeto.Producto.SubDominioId;
                                                x.Nombre = objeto.Producto.Nombre;
                                                x.Fabricante = objeto.Producto.Fabricante;
                                                x.ClaveTecnologia = objeto.Producto.Fabricante + " " + objeto.Producto.Nombre + " " + x.Versiones;
                                                x.DuenoId = objeto.Producto.OwnerDisplayName;
                                            }
                                        );
                                        ctx.SaveChanges();
                                        break;
                            }

                                // ========================================
                                //  Actualizar Flujos
                                var flujos = ctx.SolicitudTecnologiaFlujo.FirstOrDefault(x => x.SolicitudTecnologiaId == entrada.SolicitudId);
                                flujos.EstadoFlujo = (int)FlujoEstadoSolicitud.Aprobado;
                                flujos.AprobadoPor = entrada.UsuarioModificacion.ToUpper();
                                flujos.FechaAprobacion = DateTime.Now;
                                flujos.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper();
                                flujos.FechaModificacion = DateTime.Now;

                                // Actualizar Detalle
                                var detalle = ctx.SolicitudTecnologiaCampos.Where(x => x.SolicitudTecnologiaId == entrada.SolicitudId).ToList();
                                detalle.ForEach(x => x.EstadoCampo = (int)FlujoEstadoSolicitud.Aprobado);
                                detalle.ForEach(x => x.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper());
                                detalle.ForEach(x => x.FechaModificacion = DateTime.Now);

                                // Actualizar Cabecera
                                var cabecera = ctx.SolicitudTecnologia.FirstOrDefault(x => x.SolicitudTecnologiaId == entrada.SolicitudId);
                                cabecera.EstadoSolicitud = (int)FlujoEstadoSolicitud.Aprobado;
                                cabecera.UsuarioModificacion = entrada.UsuarioModificacion.ToUpper();
                                cabecera.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();
                                // ========================================
                                //==== Envío de correo
                                string listCorreo = cabecera.EmailSolicitante;
                                var listaPara = string.IsNullOrWhiteSpace(listCorreo) ? null : listCorreo.Split(';').ToList();
                                var listConCopia = string.IsNullOrWhiteSpace(entrada.UsuarioMail) ? null : entrada.UsuarioMail.Split(';').ToList();

                                try
                                {
                                    string NombreCorreo = string.Empty;
                                    if (entrada.TipoSolicitudId == (int)FlujoTipoSolicitud.Desactivacion)
                                    {
                                        NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_APROBACION_ELIMINACION";
                                        NombreTecnologiaTitulo = entidad.Nombre; //[NombreTecnologia]
                                    }
                                    else
                                    {
                                        NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_APROBACION_PRODUCTO";
                                        NombreTecnologiaTitulo = NombreTecnologiaTitulo + entidad.Nombre; //[NombreTecnologia]
                                    }

                                    string SolicitudId = entrada.SolicitudId.ToString(); //[SolicitudId]
                                    string NombreProducto = entidad.Nombre;

                                    var mailManager = new MailingManager();
                                    var diccionario = new Dictionary<string, string>();
                                    diccionario.Add("[NombreProducto]", NombreProducto);
                                    diccionario.Add("[SolicitudId]", SolicitudId);

                                    mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologiaTitulo, listaPara, listConCopia);
                                }
                                catch (Exception ex)
                                {
                                    HelperLog.Error(ex.Message);
                                }

                                ID = entidad.ProductoId;
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EditNewTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                var error = ex.StackTrace;
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

            return ID;

            
        }

        public static void DesactivarProductoXTecnologia(int idProducto, int idTecnologia)
        {
            DbContextTransaction transaction = null;

            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                var tecnologias = ctx.Tecnologia.FirstOrDefault(d => d.ProductoId == idProducto && d.Activo && d.TecnologiaId != idTecnologia);
                if (ReferenceEquals(null, tecnologias))
                {
                    var producto = ctx.Producto.Where(k => k.ProductoId == idProducto).FirstOrDefault();
                    producto.FlagActivo = false;
                    ctx.SaveChanges();
                }
            }
        }

        public override FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaActualizar(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula)
        {
            var datos = new FlujoActualizacionTecnologiaCamposDTO();

            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                // ============TAB 1: GENERAL ============
                DateTime? fechaNuevo = null;
                int? nuevoId = 0;
                // II. Obtener datos actuales de tecnología
                var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(idTecnologia, false, false, false, false);
                datos.UsoProducto = false;

                datos.Fabricante = objTec.Fabricante;
                datos.Nombre = objTec.Nombre;
                datos.Versiones = objTec.Versiones;
                datos.ClaveTecnologia = objTec.ClaveTecnologia;
                datos.Descripcion = objTec.Descripcion;
                datos.DominioId = objTec.DominioId;
                datos.SubdominioId = objTec.SubdominioId;
                datos.TipoProductoId = objTec.TipoProductoId;
                datos.TipoCicloVidaId = objTec.TipoCicloVidaId;
                datos.EsqLicenciamientoId = objTec.EsqLicenciamientoId;
                datos.EstadoId = objTec.EstadoId;
                // ============TAB 1: GENERAL ============
                datos.TipoTecnologiaId = objTec.TipoTecnologiaId; // A.1. Tipo de Tecnología
                datos.AutomatizacionImplementadaId = objTec.AutomatizacionImplementadaId; // A.2. ¿La tecnología tiene script que automatiza su implementación?
                datos.FechaLanzamiento = objTec.FechaLanzamiento; // A.3. Fecha de Lanzamiento de Tecnología
                datos.FlagFechaFinSoporte = objTec.FlagFechaFinSoporte; // A.4. ¿Tiene fecha fin de soporte?
                datos.Fuente = objTec.Fuente; // A.5. [1]-Fuente
                datos.FechaCalculoTec = objTec.FechaCalculoTec; // A.6. [1]-Fecha para cálculo de obsolescencia
                datos.FechaExtendida = objTec.FechaExtendida; // A.6. [1]-Fecha fin extendida de la tecnología
                datos.FechaFinSoporte = objTec.FechaFinSoporte; // A.7. [1]-Fecha fin soporte de la tecnología
                datos.FechaAcordada = objTec.FechaAcordada; // A.8. [1]-Fecha fin interna de la tecnología
                datos.ComentariosFechaFin = objTec.ComentariosFechaFin; // A.9. [1]-Comentarios asociados a la fecha fin de soporte de tecnología
                datos.SustentoMotivo = objTec.SustentoMotivo; // A.10. [2]-Motivo de fecha indefinida
                datos.SustentoUrl = objTec.SustentoUrl; // A.11. [2]-Url de fecha indefinida

                // A.12. Revisión de Lineamiento de Tecnología (url confluence)
                datos.UrlConfluenceId = objTec.UrlConfluenceId;
                // A.13. URL Lineamiento de Tecnología
                datos.UrlConfluence = objTec.UrlConfluence;
                // A.14. Caso de uso
                datos.CasoUso = objTec.CasoUso;
                // A.15. Indicar la Plataforma a la que aplica
                datos.Aplica = objTec.Aplica;
                // A.16. Compatibilidad de SO
                datos.CompatibilidadSOId = objTec.CompatibilidadSOId;
                // A.17. Compatibilidad Cloud
                datos.CompatibilidadCloudId = objTec.CompatibilidadCloudId;
                // A.18. Esquema de Monitoreo
                datos.EsqMonitoreo = objTec.EsqMonitoreo;
                
                datos.EstadoId = objTec.EstadoId;   // A.19. Estado de Tecnologia
                datos.FechaDeprecada = objTec.FechaDeprecada;   // A.20. Fecha deprecada
                datos.TecReemplazoDepId = objTec.TecReemplazoDepId; // A.21. Tecnología de Reemplazo


                // III. Obtención de datos desde cvt.SolicitudTecnologiaCampos
                //var listaCamposSolicitud = (from a in ctx.SolicitudTecnologiaCampos
                //                            where
                //                            a.SolicitudTecnologiaId == idSolicitud
                //                            select a).ToList();
                var listaCamposSolicitud = (from a in ctx.SolicitudTecnologiaCampos
                                            join b in ctx.ConfiguracionTecnologiaCampos on (int)a.ConfiguracionTecnologiaCamposId equals (int)b.ConfiguracionTecnologiaCamposId
                                            where a.SolicitudTecnologiaId == idSolicitud
                                            select new SolicitudTecnologiaCamposDTO()
                                            {
                                                ConfiguracionTecnologiaCamposId = (int)a.ConfiguracionTecnologiaCamposId,
                                                ValorNuevo = a.ValorNuevo,
                                                CorrelativoCampo = (int)b.CorrelativoCampo
                                            }).ToList();

                // IV. Evaluar existencia de datos
                foreach (var item in listaCamposSolicitud)
                {

                    //switch (item.ConfiguracionTecnologiaCamposId)     // Match Expression - can be any non-null expression
                    switch (item.CorrelativoCampo)     // Match Expression - can be any non-null expression
                    {
                        case (int)FlujoConfiguracionTecnologiaCampos.Fabricante:             // 1.- Fabricante 
                            datos.Fabricante = item.ValorNuevo;
                            datos.UsoProducto = true;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.NombreTecnologia:       // 2.- Nombre de Tecnología     
                            datos.Nombre = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.Version:               // 3.- Versión
                            datos.Versiones = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.ClaveTecnologia:       // 4.- Clave de la Tecnología
                            datos.ClaveTecnologia = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.DescripcionTecnologia: // 5.- Descripción de la tecnología
                            datos.Descripcion = item.ValorNuevo;
                            break;
                        //case (int)FlujoConfiguracionTecnologiaCampos.TipoActivo:            // 6. Tipo de Activo
                        //    datos.TipoActivoId = Int32.Parse(item.ValorNuevo);
                        //    datos.UsoProducto = true;
                        //    break;
                        case (int)FlujoConfiguracionTecnologiaCampos.Dominio:               // 7. Dominio
                            datos.DominioId = Int32.Parse(item.ValorNuevo);
                            datos.UsoProducto = true;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.Subdominio:            // 8. Subdominio
                            datos.SubdominioId = Int32.Parse(item.ValorNuevo);
                            datos.UsoProducto = true;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.TipoProducto:          // 9. Tipo de Producto
                            datos.TipoProductoId = Int32.Parse(item.ValorNuevo);
                            datos.UsoProducto = true;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida:         // 10. Tipo de Ciclo de Vida
                            datos.TipoCicloVidaId = Int32.Parse(item.ValorNuevo);
                            datos.UsoProducto = true;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.EsquemaLicenciamiento: // 11. Esquema de licenciamiento
                            datos.EsqLicenciamientoId = Int32.Parse(item.ValorNuevo);
                            datos.UsoProducto = true;
                            break;

                        // ============TAB 1: GENERAL ============
                        // A.1. Tipo de Tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia:
                            datos.TipoTecnologiaId = Int32.Parse(item.ValorNuevo);
                            break;
                        // A.2. ¿La tecnología tiene script que automatiza su implementación?
                        case (int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId:
                            datos.AutomatizacionImplementadaId = Int32.Parse(item.ValorNuevo);
                            break;
                        // A.3. Fecha de Lanzamiento de Tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaLanzamiento:
                            //fechaNuevo = (!string.IsNullOrEmpty(item.ValorNuevo) == true) ? Convert.ToDateTime(item.ValorNuevo) : Convert.ToDateTime(null);
                            if (!string.IsNullOrEmpty(item.ValorNuevo))
                            {
                                fechaNuevo = DateTime.ParseExact(item.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            datos.FechaLanzamiento = fechaNuevo;
                            break;
                        // A.4. ¿Tiene fecha fin de soporte?
                        case (int)FlujoConfiguracionTecnologiaCampos.FlagFechaFinSoporte:
                            datos.FlagFechaFinSoporte = bool.Parse(item.ValorNuevo);
                            break;
                        // A.5. [1]-Fuente
                        case (int)FlujoConfiguracionTecnologiaCampos.Fuente:
                            nuevoId = -1;
                            if (item.ValorNuevo == "" || item.ValorNuevo == null)
                            {
                                nuevoId = null;
                            }
                            else
                            {
                                nuevoId = Int32.Parse(item.ValorNuevo);
                            }
                            datos.Fuente = nuevoId;
                            break;
                        // A.6. [1]-Fecha para cálculo de obsolescencia
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaCalculo:
                            nuevoId = -1;
                            if (item.ValorNuevo == "" || item.ValorNuevo == null)
                            {
                                nuevoId = null;
                            }
                            else
                            {
                                nuevoId = Int32.Parse(item.ValorNuevo);
                            }
                            datos.FechaCalculoTec = nuevoId;
                            //datos.FechaCalculoTec = Int32.Parse(item.ValorNuevo);
                            break;
                        // A.6. [1]-Fecha fin extendida de la tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaExtendida:
                            //fechaNuevo = (!string.IsNullOrEmpty(item.ValorNuevo) == true) ? Convert.ToDateTime(item.ValorNuevo) : Convert.ToDateTime(null);
                            if (!string.IsNullOrEmpty(item.ValorNuevo))
                            {
                                fechaNuevo = DateTime.ParseExact(item.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            datos.FechaExtendida = fechaNuevo;
                            break;
                        // A.7. [1]-Fecha fin soporte de la tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaFinSoporte:
                            //fechaNuevo = (!string.IsNullOrEmpty(item.ValorNuevo) == true) ? Convert.ToDateTime(item.ValorNuevo) : Convert.ToDateTime(null);
                            if (!string.IsNullOrEmpty(item.ValorNuevo))
                            {
                                fechaNuevo = DateTime.ParseExact(item.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            datos.FechaFinSoporte = fechaNuevo;
                            break;
                        // A.8. [1]-Fecha fin interna de la tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaFinInterna:
                            //fechaNuevo = (!string.IsNullOrEmpty(item.ValorNuevo) == true) ? Convert.ToDateTime(item.ValorNuevo) : Convert.ToDateTime(null);
                            if (!string.IsNullOrEmpty(item.ValorNuevo))
                            {
                                fechaNuevo = DateTime.ParseExact(item.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            datos.FechaAcordada = fechaNuevo;
                            break;
                        // A.9. [1]-Comentarios asociados a la fecha fin de soporte de tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.ComentariosFechaFin:
                            datos.ComentariosFechaFin = item.ValorNuevo;
                            break;
                        // A.10. [2]-Motivo de fecha indefinida
                        case (int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida:
                            datos.SustentoMotivo = item.ValorNuevo;
                            break;
                        // A.11. [2]-Url de fecha indefinida
                        case (int)FlujoConfiguracionTecnologiaCampos.UrlFechaIndefinida:
                            datos.SustentoUrl = item.ValorNuevo;
                            break;

                        // A.12. Revisión de Lineamiento de Tecnología (url confluence)
                        case (int)FlujoConfiguracionTecnologiaCampos.UrlConfluenceId:
                            datos.UrlConfluenceId = Int32.Parse(item.ValorNuevo);
                            break;
                        // A.13. URL Lineamiento de Tecnología
                        case (int)FlujoConfiguracionTecnologiaCampos.UrlConfluence:
                            datos.UrlConfluence = item.ValorNuevo;
                            break;
                        // A.14. Caso de uso
                        case (int)FlujoConfiguracionTecnologiaCampos.CasoUso:
                            datos.CasoUso = item.ValorNuevo;
                            break;
                        // A.15. Indicar la Plataforma a la que aplica
                        case (int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma:
                            datos.Aplica = item.ValorNuevo;
                            break;
                        // A.16. Compatibilidad de SO
                        case (int)FlujoConfiguracionTecnologiaCampos.CompatibilidadSoId:
                            datos.CompatibilidadSOId = item.ValorNuevo;
                            break;
                        // A.17. Compatibilidad Cloud
                        case (int)FlujoConfiguracionTecnologiaCampos.CompatibilidadCloudId:
                            datos.CompatibilidadCloudId = item.ValorNuevo;
                            break;
                        // A.18. Esquema de Monitoreo
                        case (int)FlujoConfiguracionTecnologiaCampos.EsquemaMonitoreo:
                            datos.EsqMonitoreo = item.ValorNuevo;
                            break;
                        // A.19. Estado de Tecnologia
                        case (int)FlujoConfiguracionTecnologiaCampos.EstadoId:
                            datos.EstadoId = Int32.Parse(item.ValorNuevo);
                            break;
                        // A.20. Fecha Deprecada
                        case (int)FlujoConfiguracionTecnologiaCampos.FechaDeprecada:
                            if (!string.IsNullOrEmpty(item.ValorNuevo))
                            {
                                fechaNuevo = DateTime.ParseExact(item.ValorNuevo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            datos.FechaDeprecada = fechaNuevo;
                            break;
                        // A.21. Tecnología Reemplazo Deprecada
                        case (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId:
                            datos.TecReemplazoDepId = Int32.Parse(item.ValorNuevo);
                            break;
                        default:
                            break;
                    }
                }

                // IV.1. Recorrer campos con aplicaciones y agregarlas a lista
                var listAplicaciones = (from a in ctx.SolicitudTecnologiaCampos
                                        where
                                        a.SolicitudTecnologiaId == idSolicitud
                                        && a.ConfiguracionTecnologiaCamposId == (int)FlujoConfiguracionTecnologiaCampos.AplicacionAsociada
                                        select new TecnologiaAplicacionDTO
                                        {
                                            //TecnologiaId = idTecnologia,
                                            CodigoAPT = a.ValorNuevo // AplicacionUD
                                                                     //FlagActivo = true,
                                                                     //FechaCreacion = DateTime.Now,
                                                                     //CreadoPor = matricula
                                        }).ToList();

                datos.ListAplicaciones = listAplicaciones;

                // IV.2. Recorrer campos con aplicaciones y eliminarlos
                var itemsRemoveAppId = (from a in ctx.SolicitudTecnologiaCampos
                                        where
                                        a.SolicitudTecnologiaId == idSolicitud
                                        && a.ConfiguracionTecnologiaCamposId == (int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada
                                        select new TecnologiaAplicacionDTO
                                        {
                                            CodigoAPT = a.ValorAnterior // AplicacionUD
                                        }).ToList();

                List<int> listaAplicacionesRemover = new List<int>();
                foreach (var itemR in itemsRemoveAppId)
                {
                    listaAplicacionesRemover.Add(Int32.Parse(itemR.CodigoAPT));
                }

                datos.ItemsRemoveAppId = listaAplicacionesRemover;

                //// V. Agregar datos en la lista de retorno
                datos.TecnologiaId = idTecnologia;
                datos.SolicitudId = idSolicitud;
                datos.TipoSolicitudId = idTipoSolicitud;
                datos.ProductoId = objTec.ProductoId;
                datos.UsuarioModificacion = matricula;

                //datos.Fabricante = Fabricante;
            }

            return datos;
        }

        public override FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudProductoParaActualizar(int idSolicitud, int idTipoSolicitud, int idProducto, string matricula)
        {
            var datos = new FlujoActualizacionTecnologiaCamposDTO();

            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            {
                // ============TAB 1: GENERAL ============
                DateTime? fechaNuevo = null;
                int? nuevoId = 0;
                // II. Obtener datos actuales de producto
                var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosProductoById(idProducto, false, false, false, false);
                datos.UsoProducto = false;

                datos.Producto = new ProductoDTO();
                datos.Producto.Fabricante = objTec.Fabricante;
                datos.Producto.Nombre = objTec.Nombre;
                datos.Producto.DominioId = objTec.DominioId;
                datos.Producto.SubDominioId = objTec.SubDominioId;
                datos.Producto.TipoProductoId = objTec.TipoProductoId;
                datos.Producto.TipoCicloVidaId = objTec.TipoCicloVidaId;
                datos.Producto.EsquemaLicenciamientoSuscripcionId = objTec.EsquemaLicenciamientoSuscripcionId;
                // ============TAB 2: RESPONSABILIDADES ============
                // B.1. Tribu/COE/Unidad Organizacional        
                datos.Producto.TribuCoeId = objTec.TribuCoeId;
                datos.Producto.TribuCoeDisplayName = objTec.TribuCoeDisplayName;
                datos.Producto.TribuResponsableNombre = objTec.TribuResponsableNombre;
                datos.Producto.TribuResponsableMatricula = objTec.TribuResponsableMatricula;
                // B.3. SQUAD
                datos.Producto.SquadId = objTec.SquadId;
                datos.Producto.SquadDisplayName = objTec.SquadDisplayName; /**/
                // B.4. Responsable de SQUAD
                datos.Producto.OwnerId = objTec.OwnerId;
                datos.Producto.OwnerMatricula = objTec.OwnerMatricula;
                datos.Producto.OwnerDisplayName = objTec.OwnerDisplayName;

                // III. Obtención de datos desde cvt.SolicitudTecnologiaCampos
                var listaCamposSolicitud = (from a in ctx.SolicitudTecnologiaCampos
                                            join b in ctx.ConfiguracionTecnologiaCampos on (int)a.ConfiguracionTecnologiaCamposId equals (int)b.ConfiguracionTecnologiaCamposId
                                            where a.SolicitudTecnologiaId == idSolicitud
                                            select new SolicitudTecnologiaCamposDTO()
                                            {
                                                ConfiguracionTecnologiaCamposId = (int)a.ConfiguracionTecnologiaCamposId,
                                                ValorNuevo = a.ValorNuevo,
                                                CorrelativoCampo = (int)b.CorrelativoCampo
                                            }).ToList();

                // IV. Evaluar existencia de datos
                foreach (var item in listaCamposSolicitud)
                {

                    switch (item.CorrelativoCampo)     // Match Expression - can be any non-null expression
                    {
                        case (int)FlujoConfiguracionTecnologiaCampos.Fabricante:             // 1.- Fabricante 
                            datos.Producto.Fabricante = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.NombreProducto:       // 2.- Nombre de Producto     
                            datos.Producto.Nombre = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.Dominio:               // 7. Dominio
                            datos.Producto.DominioId = Int32.Parse(item.ValorNuevo);
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.Subdominio:            // 8. Subdominio
                            datos.Producto.SubDominioId = Int32.Parse(item.ValorNuevo);
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.TipoProducto:          // 9. Tipo de Producto
                            datos.Producto.TipoProductoId = Int32.Parse(item.ValorNuevo);
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida:         // 10. Tipo de Ciclo de Vida
                            datos.Producto.TipoCicloVidaId = Int32.Parse(item.ValorNuevo);
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.EsquemaLicenciamiento: // 11. Esquema de licenciamiento
                            datos.Producto.EsquemaLicenciamientoSuscripcionId = Int32.Parse(item.ValorNuevo);
                            break;
                        // ============TAB 2: RESPONSABILIDADES ============
                        // B.1. Tribu/COE/Unidad Organizacional
                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOE:
                            datos.Producto.TribuCoeId = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.TribuCOENombre:
                            datos.Producto.TribuCoeDisplayName = item.ValorNuevo;
                            break;
                        // B.2. Responsable de la Unidad
                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableUnidad:
                            datos.Producto.TribuResponsableMatricula = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableUnidadNombre:
                            datos.Producto.TribuResponsableNombre = item.ValorNuevo;
                            break;
                        // B.3. SQUAD
                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipo:
                            datos.Producto.SquadId = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.SquadEquipoNombre:
                            datos.Producto.SquadDisplayName = item.ValorNuevo;
                            break;
                        // B.4. Responsable de SQUAD
                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquad:
                            datos.Producto.OwnerId = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula:
                            datos.Producto.OwnerMatricula = item.ValorNuevo;
                            break;
                        case (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadNombre:
                            datos.Producto.OwnerDisplayName = item.ValorNuevo;
                            break;
                        default:
                            break;
                    }
                }

                //// V. Agregar datos en la lista de retorno
                datos.UsoProducto = true;
                datos.ProductoId = idProducto;
                datos.SolicitudId = idSolicitud;
                datos.TipoSolicitudId = idTipoSolicitud;
                datos.UsuarioModificacion = matricula;

                //datos.Fabricante = Fabricante;
            }

            return datos;
        }
        public override FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaEquivalencias(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula)
        {
            try 
            {
                var updateFlow = new FlujoActualizacionTecnologiaCamposDTO();
                // I. Obtener datos actuales de tecnología
                var dataTechnology = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(idTecnologia, false, false, false, false);
                // II. Obtener datos configuracion de tecnologia x IdSolicitud  
                var listTechnologyConfiguration = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechonologyConfigurationFields();
                var technologyData = listTechnologyConfiguration.Where(x => x.SolicitudTecnologiaId == idSolicitud && x.FlagConfiguracion == 1).ToList();
                var technologyobj = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnology(idTecnologia);
                // III. Evaluar existencia de datos
                technologyData.ForEach(o =>
                {
                    if (o.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias)
                    {
                        if (!string.IsNullOrEmpty(o.ValorNuevo))
                        {
                            updateFlow.FlagTieneEquivalencias = bool.Parse(o.ValorNuevo);
                        }
                        else
                        {
                            if(technologyobj.TecnologiaEquivalencia != null)
                                updateFlow.FlagTieneEquivalencias = true;
                        }   
                    }
                         
                    if (o.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias)
                    {
                        if (!string.IsNullOrEmpty(o.ValorNuevo))
                            updateFlow.MotivoId = Int32.Parse(o.ValorNuevo); 
                    }
                        
                });

                // IV. Recorrer campos con equivalencias y agregarlas a lista 
                var listEquivalencies = listTechnologyConfiguration.Where(op => op.SolicitudTecnologiaId == idSolicitud
                                            && op.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.ActualizarEquivalencias).ToList();

                // V. Equivalencias a retirar 
                var listEquivalencesWithdraw = listTechnologyConfiguration.Where(op => op.SolicitudTecnologiaId == idSolicitud
                                                    && op.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias).ToList();
                var listRemoveEquivalence = new List<int>();
                var listEquivalenceTechnology = new List<TecnologiaEquivalenciaDTO>();

                listEquivalencies.ForEach(p =>
                {
                    var technologyEquivalence = new TecnologiaEquivalenciaDTO
                    {
                        Nombre = p.ValorNuevo.ToString()
                    };
                    listEquivalenceTechnology.Add(technologyEquivalence);
                });

                listEquivalencesWithdraw.ForEach(w =>
                {
                    var removeEquivalence = new TecnologiaEquivalenciaDTO
                    {
                        Nombre = w.ValorAnterior
                    };

                    listRemoveEquivalence.Add(Int32.Parse(removeEquivalence.Nombre));
                });

                // VI. Agregar datos en la lista de retorno
                updateFlow.ListEquivalencias = listEquivalenceTechnology;
                updateFlow.ItemsRemoveEqTecId = listRemoveEquivalence;
                updateFlow.TecnologiaId = idTecnologia;
                updateFlow.SolicitudId = idSolicitud;
                updateFlow.TipoSolicitudId = idTipoSolicitud;
                updateFlow.ProductoId = dataTechnology.ProductoId;
                updateFlow.UsuarioModificacion = matricula;
                updateFlow.FlagTieneEquivalencias = dataTechnology.FlagTieneEquivalencias;
                updateFlow.MotivoId = dataTechnology.MotivoId;
                return updateFlow;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO , "Error en el metodo: FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaEquivalencias(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula)" , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO , "Error en el metodo: FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaEquivalencias(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula)" , new object[] { null });
            } 
        }

        public override FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaDesactivacion(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula)
        {
            var datos = new FlujoActualizacionTecnologiaCamposDTO();

            using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
            { 
                // I. Obtener datos actuales de tecnología
                var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(idTecnologia, false, false, false, false);

                datos.ActivoTecnologia = false; // Desactivar tecnologia 
                datos.TecnologiaId = idTecnologia;
                datos.SolicitudId = idSolicitud;
                datos.TipoSolicitudId = idTipoSolicitud;
                datos.ProductoId = objTec.ProductoId;
                datos.UsuarioModificacion = matricula;



            }

            return datos;
        }
        public override DataResultAplicacion ObservarSolicitud(int idSolicitud, string matricula, string comentario, int idTecnologia, int idProducto, string email)
        {

            try
            {
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    // Actualizar flujos
                    var flujos = ctx.SolicitudTecnologiaFlujo.FirstOrDefault(x => x.SolicitudTecnologiaId == idSolicitud);
                    flujos.EstadoFlujo = (int)FlujoEstadoSolicitud.Rechazado;
                    flujos.RechazadoPor = matricula.ToUpper();
                    flujos.FechaRechazo = DateTime.Now;
                    flujos.MotivoRechazo = comentario.Trim();
                    flujos.UsuarioModificacion = matricula.ToUpper();
                    flujos.FechaModificacion = DateTime.Now;

                    // Actualizar Detalle
                    var detalle = ctx.SolicitudTecnologiaCampos.Where(x => x.SolicitudTecnologiaId == idSolicitud).ToList();
                    detalle.ForEach(x => x.EstadoCampo = (int)FlujoEstadoSolicitud.Rechazado);
                    detalle.ForEach(x => x.UsuarioModificacion = matricula.ToUpper());
                    detalle.ForEach(x => x.FechaModificacion = DateTime.Now);

                    // Actualizar Cabecera
                    var cabecera = ctx.SolicitudTecnologia.FirstOrDefault(x => x.SolicitudTecnologiaId == idSolicitud);
                    cabecera.EstadoSolicitud = (int)FlujoEstadoSolicitud.Rechazado;
                    //cabecera.MotivoDesactiva = comentario.Trim();
                    cabecera.UsuarioModificacion = matricula.ToUpper();
                    cabecera.FechaModificacion = DateTime.Now;

                    ctx.SaveChanges();

                    // ========================================
                    //==== Envío de correo
                    string listCorreo = cabecera.EmailSolicitante;
                    var listaPara = string.IsNullOrWhiteSpace(listCorreo) ? null : listCorreo.Split(';').ToList();
                    var listConCopia = string.IsNullOrWhiteSpace(email) ? null : email.Split(';').ToList();

                    try
                    {
                        string NombreCorreo = idProducto != 0 ? "NOTIFICACION_FLUJOS_SOLICITUD_RECHAZO_PRODUCTO" : "NOTIFICACION_FLUJOS_SOLICITUD_RECHAZO";
                        string NombreTecnologiaTitulo = string.Empty; //[NombreTecnologia]

                        if (cabecera.TipoSolicitud == (int)FlujoTipoSolicitud.Actualizacion)
                        {
                            NombreTecnologiaTitulo = idProducto != 0 ? "de actualización de producto - " : "de actualización de tecnología - ";
                        }
                        else if (cabecera.TipoSolicitud == (int)FlujoTipoSolicitud.Equivalencias)
                        {
                            NombreTecnologiaTitulo = "para agregar y/o eliminar equivalencias de tecnología - ";
                        }
                        else if (cabecera.TipoSolicitud == (int)FlujoTipoSolicitud.Desactivacion)
                        {
                            NombreTecnologiaTitulo = "de desactivación de tecnología - ";
                        }

                        var NombreTecnologia = "";
                        if (idProducto == 0)
                        {
                            var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == idTecnologia);
                            NombreTecnologiaTitulo = NombreTecnologiaTitulo + entidad.ClaveTecnologia;
                            NombreTecnologia = entidad.ClaveTecnologia; // [NombreTecnologia]
                        }
                        else
                        {
                            var entidad = ctx.Producto.FirstOrDefault(x => x.ProductoId == idProducto);
                            NombreTecnologiaTitulo = NombreTecnologiaTitulo + entidad.Nombre;
                            NombreTecnologia = entidad.Nombre; // [NombreTecnologia]
                        }
                        string SolicitudId = idSolicitud.ToString(); //[SolicitudId]
                        string DetalleSolicitud = comentario;//[DetalleSolicitud]

                        var mailManager = new MailingManager();
                        var diccionario = new Dictionary<string, string>();
                        if (idProducto == 0)
                        {
                            diccionario.Add("[NombreTecnologia]", NombreTecnologia);
                        } else
                        {
                            diccionario.Add("[NombreProducto]", NombreTecnologia);
                        }
                        diccionario.Add("[SolicitudId]", SolicitudId);
                        diccionario.Add("[DetalleSolicitud]", DetalleSolicitud);

                        mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologiaTitulo, listaPara, listConCopia);
                    }
                    catch (Exception ex)
                    {
                        HelperLog.Error(ex.Message);
                    }

                    return dataResult;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
        }
        public override TecnologiaDTO GetEquivalenciasPropuestaById(int id, bool withEquivalencias = false)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join a in ctx.Dominio on s.DominioId equals a.DominioId
                                       join b in ctx.Producto on u.ProductoId equals b.ProductoId into ljb
                                       from b in ljb.DefaultIfEmpty()
                                       where u.TecnologiaId == id
                                       //&& t.FlagActivo
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           ProductoId = u.ProductoId,
                                           FlagTieneEquivalencias = u.FlagTieneEquivalencias ?? false,
                                           MotivoId = u.MotivoId,
                                       }).FirstOrDefault();

                        if (entidad != null)
                        {
                            if (withEquivalencias)
                            {

                                var listEquivalencias = (from u in ctx.Tecnologia
                                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                                         join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId
                                                         where u.TecnologiaId == (id == 0 ? u.TecnologiaId : id)
                                                         && u.Activo
                                                         && te.FlagActivo
                                                         select new TecnologiaEquivalenciaDTO
                                                         {
                                                             Id = te.TecnologiaEquivalenciaId,
                                                             TecnologiaId = te.TecnologiaId,
                                                             NombreTecnologia = u.ClaveTecnologia,
                                                             DominioTecnologia = d.Nombre,
                                                             SubdominioTecnologia = s.Nombre,
                                                             TipoTecnologia = t.Nombre,
                                                             EstadoId = u.EstadoId,
                                                             Nombre = te.Nombre
                                                         }).ToList();

                                entidad.ListEquivalencias = listEquivalencias;
                                entidad.FlagTieneEquivalencias = listEquivalencias.Count > 0;
                            }
                        }
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetEquivalenciasPropuestaById(int id, bool withEquivalencias = false)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetEquivalenciasPropuestaById(int id, bool withEquivalencias = false)"
                    , new object[] { null });
            }
        }
 

        public override int EnviarSolicitudAprobacionTecnologia(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 1;
            var CURRENT_DATE = DateTime.Now;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {

                        var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                        if (entidad != null)
                        {
                            // ===== Obtención de datos actuales
                            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(objeto.Id, false, false, false, false);
                            if (objTec != null)
                            {
                                string aplicacionAgregar = string.Empty;
                                bool flagAplAgregar = false;
                                string aplicacionEliminar = string.Empty;
                                bool flagAplEliminar = false;

                                //objeto : valor_nuevo
                                //
                                // Acondicionamiento de datos recibidos
                                objeto = ServiceManager<TecnologiaDAO>.Provider.AcondicionamientoTecnologia(objeto);
                                // Registros del diccionario de campos 
                                var configuracion = ServiceManager<TecnologiaDAO>.Provider.GetConfiguracionTecnologiaCampos();
                                // Lista de aplicaciones para Estándar Restringido
                                var aplicacionLista = ServiceManager<TecnologiaDAO>.Provider.GetTecnologiaAplicacionesPorTecnologia(objeto.Id);
                                // Inserción de datos en listas
                                var listaCampos = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaCampos(configuracion, objTec, objeto, aplicacionLista);
                               
                                var rolAprueba = configuracion.Select(x => x.RolAprueba).Distinct().ToList();
                                
                                foreach (var item in rolAprueba)
                                {
                                    if (item != "SIN_APROBACION")
                                    {
                                        var regCampos = listaCampos.Where(x => x.RolAprueba == item).ToList(); 
                                        if (regCampos.Count > 0)
                                        {
                                            //=========== Inserción de Solicitud ===========
                                            var cabecera = new SolicitudTecnologia()
                                            {
                                                TipoSolicitud = ((int)FlujoTipoSolicitud.Actualizacion),
                                                TecnologiaId = objeto.Id,
                                                EstadoSolicitud = ((int)FlujoEstadoSolicitud.Pendiente),
                                                UsuarioSolicitante = objeto.UsuarioMatricula,
                                                NombreUsuarioSolicitante = objeto.UsuarioNombre.ToUpper(),
                                                EmailSolicitante = objeto.UsuarioMail,
                                                UsuarioCreacion = objeto.UsuarioMatricula,
                                                FechaCreacion = DateTime.Now,
                                            };
                                            ctx.SolicitudTecnologia.Add(cabecera);
                                            ctx.SaveChanges();

                                            
                                            //===== Obtener Correo
                                            var azureManger = new AzureGroupsManager();
                                            ParametroDTO parametroCorreos = new ParametroDTO();
                                            ParametroDTO parametroGrupo = new ParametroDTO();
                                            string grupoRedPro = string.Empty;
                                            string correoPro = string.Empty;
                                           
                                            //=========== Inserción de Flujo ===========
                                            int idTipoRolAprobacion = 1;
                                            switch (item)
                                            {
                                                case "CVT":
                                                    idTipoRolAprobacion = (int)FlujoTipo.CVT;
                                                    parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_CVT");
                                                    parametroGrupo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_GRUPO_CVT");
                                                    break;
                                                case "ESTANDARES":
                                                    idTipoRolAprobacion = (int)FlujoTipo.Estandares;
                                                    parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_ESTANDARES");
                                                    parametroGrupo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_GRUPO_ESTANDARES");
                                                    break;
                                                case "OWNER_SQUAD_RECEPTOR":
                                                    idTipoRolAprobacion = (int)FlujoTipo.Squad;
                                                    //parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_OWNER");
                                                    //string correoOwner = entidad.Producto.OwnerMatricula;
                                                    var idResponsableSquadMatricula = ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula);
                                                    var listaTemporal = listaCampos.Where(x => x.ConfiguracionTecnologiaCamposId == idResponsableSquadMatricula).FirstOrDefault();
                                                    string correoOwner = string.Empty;
                                                    if (!ReferenceEquals(null, listaTemporal))
                                                        correoOwner = listaTemporal.ValorNuevo;
                                                    string correoOwnerStr = string.Empty;
                                                    
                                                    if (!string.IsNullOrEmpty(correoOwner))
                                                        correoOwnerStr = correoOwner.Substring(correoOwner.Length - 6);

                                                    var correoOwnerPro = ServiceManager<ApplicationDAO>.Provider.GetCorreoPorMatricula(correoOwnerStr);
                                                    grupoRedPro = correoOwnerStr;
                                                    correoPro = correoOwnerPro.Correo;
                                                    break;
                                                    //case default:
                                                    //    break;
                                            }

                                            string[] listCorreos = new string[] { };
                                            string grupoRed = string.Empty;

                                            if (idTipoRolAprobacion == (int)FlujoTipo.CVT 
                                                || idTipoRolAprobacion == (int)FlujoTipo.Estandares)
                                            {
                                                listCorreos = parametroCorreos.Valor.ToString().Split(';');
                                                grupoRed = parametroGrupo.Valor;
                                            }
                                            else 
                                            {
                                                listCorreos = correoPro.ToString().Split(';');
                                                grupoRed = grupoRedPro;
                                            }
                                            

                                            var flujo = new SolicitudTecnologiaFlujo()
                                            {
                                                SolicitudTecnologiaId = (int)cabecera.SolicitudTecnologiaId,
                                                TipoFlujo = idTipoRolAprobacion,  
                                                EstadoFlujo = ((int)FlujoEstadoSolicitud.Pendiente),
                                                ResponsableMatricula = grupoRed,
                                                ResponsableBuzon = (string.IsNullOrEmpty(parametroCorreos.Valor) ? objeto.UsuarioMatricula : parametroCorreos.Valor),
                                                UsuarioCreacion = objeto.UsuarioMatricula,
                                                FechaCreacion = DateTime.Now,
                                            };
                                            ctx.SolicitudTecnologiaFlujo.Add(flujo);
                                            ctx.SaveChanges();

                                            //=========== Inserción de Campos ===========
                                            string DetalleSolicitud = String.Empty;
                                            var motivo_validacion = regCampos.Where(u => u.ConfiguracionTecnologiaCamposId == (int)FlujoConfiguracionTecnologiaCampos.MotivoEliminacionTecnologia).FirstOrDefault();
                                            foreach (var camp in regCampos)
                                            {
                                                // Inserción en la tabla detalle

                                                    this.InsertarTecnologiaCampos2(
                                                       ctx,
                                                       (int)cabecera.SolicitudTecnologiaId,
                                                       camp.ConfiguracionTecnologiaCamposId,
                                                       camp.ValorAnterior,
                                                       camp.ValorNuevo,
                                                       objeto.UsuarioMatricula);

                                                if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Dominio) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Subdominio) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoProducto) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EsquemaLicenciamiento) ||

                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FlagFechaFinSoporte) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Fuente) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaCalculo) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EstadoId) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FechaDeprecada) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId))
                                                {
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;
                                                }
                                                else if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AplicacionAsociada))
                                                {
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;
                                                    if (!flagAplAgregar)
                                                    {
                                                        aplicacionAgregar = "<br/> - Aplicaciones a agregar:";
                                                        aplicacionAgregar = aplicacionAgregar + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorNuevo + "</strong>";
                                                        flagAplAgregar = true;
                                                    }
                                                    else
                                                    {
                                                        aplicacionAgregar = aplicacionAgregar + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorNuevo + "</strong>";
                                                    }
                                                    camp.ValorNuevo = string.Empty;
                                                }
                                                else if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada))
                                                {
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;
                                                    if (!flagAplEliminar)
                                                    {
                                                        aplicacionEliminar = "<br/> - Aplicaciones a eliminar:";
                                                        aplicacionEliminar = aplicacionEliminar + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorNuevo + "</strong>";
                                                        flagAplEliminar = true;
                                                    }
                                                    else
                                                    {
                                                        aplicacionEliminar = aplicacionEliminar + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorNuevo + "</strong>";
                                                    }
                                                    camp.ValorNuevo = string.Empty;
                                                }

                                                // ==========================================
                                                if (motivo_validacion != null)
                                                {
                                                    if (!string.IsNullOrEmpty(camp.ValorNuevo))
                                                        DetalleSolicitud = DetalleSolicitud + "<br/> - " + camp.NombreCampo + ": <strong>" + camp.ValorNuevo + "</strong>";
                                                }
                                                else
                                                {
                                                    DetalleSolicitud = DetalleSolicitud + "<br/> - " + camp.NombreCampo + ": <strong>" + camp.ValorNuevo + "</strong>";
                                                }

                                            }

                                            //==== Envío de correo
                                            try
                                            {
                                                DetalleSolicitud = DetalleSolicitud + aplicacionAgregar + aplicacionEliminar;

                                                string NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_MODIFICACION";
                                                string NombreTecnologia = entidad.ClaveTecnologia; //[NombreTecnologia]
                                                string NombreCreador = objeto.UsuarioNombre.ToUpper();  //[NombreCreador]
                                                string SolicitudId = cabecera.SolicitudTecnologiaId.ToString(); //[SolicitudId]
                                                string CodigoProducto = entidad.Producto.Codigo; // [CodigoProducto]

                                                var listaPara = new List<string>(); 
                                                //string.IsNullOrWhiteSpace(listCorreos) ? null : listCorreos.Split(';').ToList();

                                                foreach (var mail in listCorreos)
                                                {
                                                    var oListParaMail = new Object();

                                                    oListParaMail = mail.ToString();

                                                    listaPara.Add(oListParaMail.ToString());
                                                }

                                                var listConCopia = string.IsNullOrWhiteSpace(objeto.UsuarioMail) ? null : objeto.UsuarioMail.Split(';').ToList(); 
                                                var mailManager = new MailingManager();
                                                var diccionario = new Dictionary<string, string>();
                                                diccionario.Add("[NombreTecnologia]", NombreTecnologia);
                                                diccionario.Add("[NombreCreador]", NombreCreador);
                                                diccionario.Add("[SolicitudId]", SolicitudId);
                                                diccionario.Add("[CodigoProducto]", CodigoProducto);
                                                diccionario.Add("[DetalleSolicitud]", DetalleSolicitud);

                                                mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologia, listaPara, listConCopia);
                                                ID = 2;
                                            }
                                            catch (Exception ex)
                                            {
                                                HelperLog.Error(ex.Message);
                                            }
                                        }
                                    }
                                    //i = i + 1;
                                }

                                // B. Actualización de información que no requiere aprobación 
                                var campos_sin_aprobacion = ctx.ConfiguracionTecnologiaCampos.Where(f => f.RolAprueba == "SIN_APROBACION" && f.FlagActivo == 1).ToList();

                                #region 1. Actualizacion de Campos de tecnologia
                                foreach (var item in campos_sin_aprobacion)
                                { 
                                   switch (objeto.TipoSave)
                                    {
                                        case 2:
                                            switch (item.CorrelativoCampo)
                                            {
                                                case ((int)FlujoConfiguracionTecnologiaCampos.DescripcionTecnologia):
                                                    entidad.Descripcion = objeto.Descripcion.ToString();
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.UrlConfluenceId):
                                                    entidad.UrlConfluenceId = objeto.UrlConfluenceId;
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.UrlConfluence):
                                                    entidad.UrlConfluence = (string.IsNullOrEmpty(objeto.UrlConfluence.ToString()) ? string.Empty : objeto.UrlConfluence.ToString());
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.CasoUso):
                                                    entidad.CasoUso = (string.IsNullOrEmpty(objeto.CasoUso.ToString()) ? string.Empty : objeto.CasoUso.ToString());
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.CompatibilidadSoId):
                                                    entidad.CompatibilidadSOId = (string.IsNullOrEmpty(objeto.CompatibilidadSOId) ? string.Empty : objeto.CompatibilidadSOId);
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.CompatibilidadCloudId):
                                                    entidad.CompatibilidadCloudId = (string.IsNullOrEmpty(objeto.CompatibilidadCloudId) ? string.Empty : objeto.CompatibilidadCloudId);
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId):
                                                    entidad.AutomatizacionImplementadaId = (string.IsNullOrEmpty(objeto.AutomatizacionImplementadaId.ToString()) ? 0 : objeto.AutomatizacionImplementadaId);
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.EsquemaMonitoreo):
                                                    entidad.EsqMonitoreo = (string.IsNullOrEmpty(objeto.EsqMonitoreo.ToString()) ? string.Empty : objeto.EsqMonitoreo.ToString());
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.EstadoId):
                                                    if (objTec.EstadoId == 1)
                                                        if (objeto.FlagRestringido == true)
                                                            entidad.EstadoId = (int)ETecnologiaEstado.Restringido;
                                                    break;
                                            }
                                            break;
                                    }
                                    ctx.SaveChanges();
                                }
                                #endregion

                                ctx.SaveChanges();

                            }

                            // === Archivo
                            var archivo = (from u in ctx.ArchivosCVT
                                           where u.ArchivoId == objeto.ArchivoId
                                           select u).FirstOrDefault();
                            if (archivo != null)
                            {
                                archivo.Activo = false;
                                archivo.FechaModificacion = DateTime.Now;
                                archivo.UsuarioModificacion = objeto.UsuarioModificacion;
                                ctx.SaveChanges();
                                //ID = entidad.ArchivoId;
                            }

                            //ID = entidad.TecnologiaId;
                        }


                        transaction.Commit();
                    }
                }

                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

        }

        public override int EnviarSolicitudAprobacionProducto(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            int ID = 1;
            var CURRENT_DATE = DateTime.Now;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {

                        var entidad = ctx.Producto.FirstOrDefault(x => x.ProductoId == objeto.ProductoId);
                        if (entidad != null)
                        {
                            // ===== Obtención de datos actuales
                            var objProd = ServiceManager<TecnologiaDAO>.Provider.GetDatosProductoById(objeto.ProductoId.HasValue ? objeto.ProductoId.Value : 0, false, false, false, false);
                            if (objProd != null)
                            {
                                //objeto : valor_nuevo
                                // Acondicionamiento de datos recibidos
                                objeto = ServiceManager<TecnologiaDAO>.Provider.AcondicionamientoTecnologia(objeto);
                                // Registros del diccionario de campos 
                                var configuracion = ServiceManager<TecnologiaDAO>.Provider.GetConfiguracionTecnologiaCampos();
                                // Lista de aplicaciones
                                // Inserción de datos en listas
                                var listaCampos = ServiceManager<TecnologiaDAO>.Provider.InsertarProductoCampos(configuracion, objProd, objeto);

                                var rolAprueba = configuracion.Select(x => x.RolAprueba).Distinct().ToList();

                                foreach (var item in rolAprueba)
                                {
                                    if (item != "SIN_APROBACION")
                                    {
                                        var regCampos = listaCampos.Where(x => x.RolAprueba == item).ToList();
                                        if (regCampos.Count > 0)
                                        {
                                            var SolicitudProductoId = InsertSolicitudProducto(((int)FlujoTipoSolicitud.Actualizacion), objeto.ProductoId, ((int)FlujoEstadoSolicitud.Pendiente), objeto.UsuarioMatricula, objeto.UsuarioNombre.ToUpper(), objeto.UsuarioMail, objeto.UsuarioMatricula, DateTime.Now);

                                            //===== Obtener Correo
                                            var azureManger = new AzureGroupsManager();
                                            ParametroDTO parametroCorreos = new ParametroDTO();
                                            ParametroDTO parametroGrupo = new ParametroDTO();
                                            string grupoRedPro = string.Empty;
                                            string correoPro = string.Empty;

                                            //=========== Inserción de Flujo ===========
                                            int idTipoRolAprobacion = 1;
                                            switch (item)
                                            {
                                                case "CVT":
                                                    idTipoRolAprobacion = (int)FlujoTipo.CVT;
                                                    parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_CVT");
                                                    parametroGrupo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_GRUPO_CVT");
                                                    break;
                                                case "ESTANDARES":
                                                    idTipoRolAprobacion = (int)FlujoTipo.Estandares;
                                                    parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_ESTANDARES");
                                                    parametroGrupo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_GRUPO_ESTANDARES");
                                                    break;
                                                case "OWNER_SQUAD_RECEPTOR":
                                                    idTipoRolAprobacion = (int)FlujoTipo.Squad;
                                                    var idResponsableSquadMatricula = ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula);
                                                    var listaTemporal = listaCampos.Where(x => x.ConfiguracionTecnologiaCamposId == idResponsableSquadMatricula).FirstOrDefault();
                                                    string correoOwner = string.Empty;
                                                    if (!ReferenceEquals(null, listaTemporal))
                                                        correoOwner = listaTemporal.ValorNuevo;
                                                    string correoOwnerStr = string.Empty;

                                                    if (!string.IsNullOrEmpty(correoOwner))
                                                        correoOwnerStr = correoOwner.Substring(correoOwner.Length - 6);

                                                    grupoRedPro = correoOwnerStr;

                                                    var user = new ADUsuarios().GetUserbyMatricula(correoOwnerStr);

                                                    if(user.Count > 0)
                                                    {
                                                        correoPro = user.FirstOrDefault().mail;
                                                    }
                                                    else
                                                    {
                                                        var correoOwnerPro = ServiceManager<ApplicationDAO>.Provider.GetCorreoPorMatricula(correoOwnerStr);
                                                        correoPro = correoOwnerPro.Correo;
                                                    }                                                    
                                                    break;
                                            }

                                            string[] listCorreos = new string[] { };
                                            string grupoRed = string.Empty;

                                            if (idTipoRolAprobacion == (int)FlujoTipo.CVT
                                                || idTipoRolAprobacion == (int)FlujoTipo.Estandares)
                                            {
                                                listCorreos = parametroCorreos.Valor.ToString().Split(';');
                                                grupoRed = parametroGrupo.Valor;
                                            }
                                            else
                                            {
                                                listCorreos = correoPro.ToString().Split(';');
                                                grupoRed = grupoRedPro;
                                            }


                                            var flujo = new SolicitudTecnologiaFlujo()
                                            {
                                                SolicitudTecnologiaId = (int)SolicitudProductoId,
                                                TipoFlujo = idTipoRolAprobacion,
                                                EstadoFlujo = ((int)FlujoEstadoSolicitud.Pendiente),
                                                ResponsableMatricula = grupoRed,
                                                ResponsableBuzon = (string.IsNullOrEmpty(parametroCorreos.Valor) ? objeto.UsuarioMatricula : parametroCorreos.Valor),
                                                UsuarioCreacion = objeto.UsuarioMatricula,
                                                FechaCreacion = DateTime.Now,
                                            };
                                            ctx.SolicitudTecnologiaFlujo.Add(flujo);
                                            ctx.SaveChanges();

                                            //=========== Inserción de Campos ===========
                                            string DetalleSolicitud = String.Empty;
                                            var motivo_validacion = regCampos.Where(u => u.ConfiguracionTecnologiaCamposId == (int)FlujoConfiguracionTecnologiaCampos.MotivoEliminacionTecnologia).FirstOrDefault();
                                            foreach (var camp in regCampos)
                                            {
                                                // Inserción en la tabla detalle

                                                this.InsertarTecnologiaCampos2(
                                                   ctx,
                                                   (int)SolicitudProductoId,
                                                   camp.ConfiguracionTecnologiaCamposId,
                                                   camp.ValorAnterior,
                                                   camp.ValorNuevo,
                                                   objeto.UsuarioMatricula);

                                                if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Dominio) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.Subdominio) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoProducto) ||
                                                    camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida))
                                                {
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;
                                                }

                                                // ==========================================
                                                if (motivo_validacion != null)
                                                {
                                                    if (!string.IsNullOrEmpty(camp.ValorNuevo))
                                                        DetalleSolicitud = DetalleSolicitud + "<br/> - " + camp.NombreCampo + ": <strong>" + (camp.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula ? camp.ValorNuevo.Substring(1) : camp.ValorNuevo) + "</strong>";
                                                }
                                                else
                                                {
                                                    DetalleSolicitud = DetalleSolicitud + "<br/> - " + camp.NombreCampo + ": <strong>" + (camp.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula ? camp.ValorNuevo.Substring(1) : camp.ValorNuevo) + "</strong>";
                                                }

                                            }

                                            //==== Envío de correo
                                            try
                                            {
                                                string NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_MODIFICACION_PRODUCTO";
                                                string NombreProducto = entidad.Nombre; //[NombreTecnologia]
                                                string NombreCreador = objeto.UsuarioNombre.ToUpper();  //[NombreCreador]
                                                string SolicitudId = SolicitudProductoId.ToString(); //[SolicitudId]
                                                string CodigoProducto = entidad.Codigo; // [CodigoProducto]

                                                var listaPara = new List<string>();
                                                //string.IsNullOrWhiteSpace(listCorreos) ? null : listCorreos.Split(';').ToList();

                                                foreach (var mail in listCorreos)
                                                {
                                                    var oListParaMail = new Object();

                                                    oListParaMail = mail.ToString();

                                                    listaPara.Add(oListParaMail.ToString());
                                                }

                                                var listConCopia = string.IsNullOrWhiteSpace(objeto.UsuarioMail) ? null : objeto.UsuarioMail.Split(';').ToList();
                                                var mailManager = new MailingManager();
                                                var diccionario = new Dictionary<string, string>();
                                                diccionario.Add("[NombreProducto]", NombreProducto);
                                                diccionario.Add("[NombreCreador]", NombreCreador);
                                                diccionario.Add("[SolicitudId]", SolicitudId);
                                                diccionario.Add("[CodigoProducto]", CodigoProducto);
                                                diccionario.Add("[DetalleSolicitud]", DetalleSolicitud);

                                                mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreProducto, listaPara, listConCopia);
                                                ID = 2;
                                            }
                                            catch (Exception ex)
                                            {
                                                HelperLog.Error(ex.Message);
                                            }
                                        }
                                    }
                                    //i = i + 1;
                                }

                                // B. Actualización de información que no requiere aprobación 
                                var campos_sin_aprobacion = ctx.ConfiguracionTecnologiaCampos.Where(f => f.RolAprueba == "SIN_APROBACION" && f.FlagActivo == 1).ToList();

                                #region 1. Actualizacion de Campos de producto
                                foreach (var item in campos_sin_aprobacion)
                                {
                                    switch (objeto.TipoSave)
                                    {
                                        case 1:
                                            switch (item.CorrelativoCampo)
                                            {
                                                case ((int)FlujoConfiguracionTecnologiaCampos.DescripcionTecnologia):
                                                    entidad.Descripcion = objeto.Descripcion.ToString();
                                                    entidad.Descripcion = objeto.Producto.Descripcion.ToString();
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.EsquemaLicenciamiento):
                                                    entidad.EsquemaLicenciamientoSuscripcionId = (string.IsNullOrEmpty(objeto.Producto.EsquemaLicenciamientoSuscripcionId.ToString()) ? null : objeto.Producto.EsquemaLicenciamientoSuscripcionId);
                                                    break;
                                            }
                                            break;
                                        case 4:
                                            switch (item.CorrelativoCampo)
                                            {
                                                case ((int)FlujoConfiguracionTecnologiaCampos.GrupoSoporteRemedy):
                                                    entidad.GrupoTicketRemedyId = Int32.Parse(objeto.Producto.GrupoTicketRemedyId.ToString());
                                                    entidad.GrupoTicketRemedyNombre = (string.IsNullOrEmpty(objeto.Producto.GrupoTicketRemedyNombre) ? string.Empty : objeto.Producto.GrupoTicketRemedyNombre);
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.EquipoAprovisionamiento):
                                                    entidad.EquipoAprovisionamiento = (string.IsNullOrEmpty(objeto.EquipoAprovisionamiento.ToString()) ? string.Empty : objeto.EquipoAprovisionamiento.ToString());
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.ListaExpertos):
                                                    if (objeto.ListExpertos != null)
                                                    {
                                                        foreach (var itemCampo in objeto.ListExpertos)
                                                        {
                                                            var productoManagerRoles = new ProductoManagerRoles
                                                            {
                                                                ProductoId = objProd.Id,
                                                                ManagerMatricula = itemCampo.ManagerMatricula.ToString(),
                                                                ManagerNombre = itemCampo.ManagerNombre.ToString(),
                                                                ManagerEmail = itemCampo.ManagerEmail.ToString(),
                                                                ProductoManagerId = itemCampo.ProductoManagerId,
                                                                FlagActivo = true,
                                                                CreadoPor = objeto.UsuarioMatricula,
                                                                FechaCreacion = DateTime.Now
                                                            };
                                                            ctx.ProductoManagerRoles.Add(productoManagerRoles);
                                                            ctx.SaveChanges();
                                                        }
                                                    }
                                                    break;
                                                case ((int)FlujoConfiguracionTecnologiaCampos.ListaExpertosEliminar):
                                                    if (objeto.ListExpertosEliminar != null)
                                                    {
                                                        foreach (var itemEliminar in objeto.ListExpertosEliminar)
                                                        {
                                                            var data = ctx.ProductoManagerRoles.Where(u => u.ProductoManagerRolesId == itemEliminar.Id).FirstOrDefault();
                                                            data.ModificadoPor = objeto.UsuarioMatricula;
                                                            data.FechaModificacion = DateTime.Now;
                                                            data.FlagActivo = false;
                                                            ctx.SaveChanges();
                                                        }
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    ctx.SaveChanges();
                                }
                                #endregion

                                ctx.SaveChanges();

                            }
                        }


                        transaction.Commit();
                    }
                }

                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionProducto(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionProducto(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

        }

        public static int InsertSolicitudProducto (int tipoSolicitud, int? productoId, int estadoSolicitud, string usuarioSolicitante, string nombreUsuarioSolicitante, string emailSolicitante, string usuarioCreacion, DateTime fechaCreacion)
        {
            using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
            {
                try
                {
                    using (var comando = new SqlCommand("[cvt].[USP_InsertSolicitudProducto]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@tipoSolicitud", tipoSolicitud);
                        comando.Parameters.AddWithValue("@productoId", productoId);
                        comando.Parameters.AddWithValue("@estadoSolicitud", estadoSolicitud);
                        comando.Parameters.AddWithValue("@usuarioSolicitante", usuarioSolicitante);
                        comando.Parameters.AddWithValue("@nombreUsuarioSolicitante", nombreUsuarioSolicitante);
                        comando.Parameters.AddWithValue("@emailSolicitante", emailSolicitante);
                        comando.Parameters.AddWithValue("@usuarioCreacion", usuarioCreacion);
                        comando.Parameters.AddWithValue("@fechaCreacion", fechaCreacion);
                        comando.Parameters.Add("@SolicitudTecnologiaId", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        comando.Connection = cnx;
                        cnx.Open();

                        comando.ExecuteNonQuery();
                        return Convert.ToInt32(comando.Parameters["@SolicitudTecnologiaId"].Value);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    HelperLog.ErrorEntity(ex);
                    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                        , "Error en el metodo: InsertSolicitudProducto (int tipoSolicitud, int productoId, int estadoSolicitud, string usuarioSolicitud, string nombreUsuarioSolicitud, string emailSolicitante, string usuarioCreacion, DateTime fechaCreacion)"
                        , new object[] { null });
                }
                catch (Exception ex)
                {
                    HelperLog.Error(ex.Message);
                    throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                        , "Error en el metodo: InsertSolicitudProducto (int tipoSolicitud, int productoId, int estadoSolicitud, string usuarioSolicitud, string nombreUsuarioSolicitud, string emailSolicitante, string usuarioCreacion, DateTime fechaCreacion)"
                        , new object[] { null });
                }
                finally
                {
                    cnx.Close();
                    cnx.Dispose();
                }
            }
        }


        public override int EnviarSolicitudAprobacionEquivalencia(TecnologiaDTO objeto)
        {
            DbContextTransaction transaction = null;
            var registroNuevo = false;
            int ID = 0;
            var CURRENT_DATE = DateTime.Now;

            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        var entidad = ctx.Tecnologia.FirstOrDefault(x => x.TecnologiaId == objeto.Id);
                        var position = 0;

                        if (entidad != null)
                        { 
                            //objTec = VALOR ANTERIOR
                            //objeto = VALOR NUEVO
                            
                            //1. Se obtiene todo el registro de la tecnologia por el Id "TecnologiaId"
                            var objTec = ServiceManager<TecnologiaDAO>.Provider.GetDatosTecnologiaById(objeto.Id, false, false, false, false); 
                             
                            if (objTec != null)
                            {
                                //2.Obtener las equivalencias registradas como "VALOR ANTERIOR" ya que serán actualizadas/eliminadas
                                if (entidad.TecnologiaEquivalencia != null)
                                {
                                    var listTecnologiaEquivalencia = entidad.TecnologiaEquivalencia.Select(x => new TecnologiaEquivalenciaDTO
                                    {
                                        Id = objTec.Id,
                                        Activo = objTec.Activo,
                                        FlagEliminado = (string.IsNullOrEmpty(objTec.FlagEliminado.ToString()) ? false : true),
                                        Nombre = x.Nombre.ToString(),
                                        TecnologiaId = x.TecnologiaId,
                                        TecnologiaEquivalenciaId = x.TecnologiaEquivalenciaId,
                                        FlagActivo = (x.FlagActivo == false ? false : true)
                                    }).ToList();
                                     
                                    objTec.ListEquivalencias = listTecnologiaEquivalencia; 
                                }
                                // Registros del diccionario de campos 
                                var configuracion = ServiceManager<TecnologiaDAO>.Provider.GetConfiguracionTecnologiaCampos();
                                // Inserción de datos en listas
                                var listaCampos = ServiceManager<TecnologiaDAO>.Provider.InsertarTecnologiaEquivalenciaCampos(configuracion, objTec, objeto);

                                var rolAprueba = configuracion.Select(x => x.RolAprueba).Distinct().ToList();
                                int i = 0;
                                foreach (var item in rolAprueba)
                                {
                                    var regCampos = listaCampos.Where(x => x.RolAprueba == rolAprueba[i]).ToList();

                                    if (regCampos.Count > 0)
                                    {
                                        //=========== Inserción de Solicitud ===========
                                        var cabecera = new SolicitudTecnologia()
                                        {
                                            TipoSolicitud = ((int)FlujoTipoSolicitud.Equivalencias),
                                            TecnologiaId = objeto.Id,
                                            EstadoSolicitud = ((int)FlujoEstadoSolicitud.Pendiente),
                                            UsuarioSolicitante = objeto.UsuarioMatricula,
                                            NombreUsuarioSolicitante = objeto.UsuarioNombre.ToUpper(),
                                            EmailSolicitante = objeto.UsuarioMail,
                                            UsuarioCreacion = objeto.UsuarioMatricula,
                                            FechaCreacion = DateTime.Now,
                                        };
                                        ctx.SolicitudTecnologia.Add(cabecera);
                                        ctx.SaveChanges();

                                        //==== Correo
                                        var azureManger = new AzureGroupsManager();
                                        var parametroCorreos = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_CVT");
                                        string listCorreos = parametroCorreos.Valor;

                                        var parametroGrupo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_GRUPO_CVT");
                                        string grupoRed = parametroGrupo.Valor;

                                        //=========== Inserción de Flujo ===========
                                        var flujo = new SolicitudTecnologiaFlujo()
                                        {
                                            SolicitudTecnologiaId = (int)cabecera.SolicitudTecnologiaId,
                                            TipoFlujo = ((int)FlujoTipo.CVT), //((int)FlujoTipo.UnaAprobacion),
                                            EstadoFlujo = ((int)FlujoEstadoSolicitud.Pendiente),
                                            ResponsableMatricula = grupoRed,
                                            ResponsableBuzon = listCorreos,
                                            UsuarioCreacion = objeto.UsuarioMatricula,
                                            FechaCreacion = DateTime.Now,
                                        };
                                        ctx.SolicitudTecnologiaFlujo.Add(flujo);
                                        ctx.SaveChanges();

                                        string DetalleSolicitud = String.Empty;
                                        string detalleCrear = "<br/> - Equivalencias a crear:";
                                        bool flagCrear = false;
                                        string detalleEliminar = "<br/> - Equivalencias a eliminar:";
                                        bool flagEliminar = false;

                                        //=========== Inserción de Campos ===========
                                        foreach (var camp in regCampos)
                                        {
                                            InsertarTecnologiaCampos2(ctx, (int)cabecera.SolicitudTecnologiaId, camp.ConfiguracionTecnologiaCamposId, camp.ValorAnterior, camp.ValorNuevo, objeto.UsuarioMatricula);

                                            if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.ActualizarEquivalencias))
                                            { 
                                                if (!string.IsNullOrEmpty(camp.ValorNuevo))
                                                {
                                                    flagCrear = true;
                                                    detalleCrear = detalleCrear + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorNuevo + "</strong>"; 
                                                    camp.ValorNuevo = string.Empty;
                                                }    
                                            }

                                            if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias))
                                            {
                                                flagEliminar = true;
                                                detalleEliminar = detalleEliminar + "<br/>&nbsp;&nbsp;- <strong>" + camp.ValorCampoPropuesto + "</strong>";
                                                camp.ValorNuevo = string.Empty;
                                            }

                                            if (position == 1)
                                            { 
                                                if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias))
                                                {
                                                    camp.NombreCampo = string.Empty;
                                                    camp.ValorNuevo = string.Empty;
                                                }
                                                else
                                                {
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;
                                                }
                                            }
                                            else
                                            { 
                                                if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias))
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto;

                                                if (camp.ConfiguracionTecnologiaCamposId == ServiceManager<TechnologyConfigurationDAO>.Provider.GetIdTechnologyConfiguration((int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias))
                                                    camp.ValorNuevo = camp.ValorCampoPropuesto; 
                                            }
                                             
                                            //===========================================
                                            if (!string.IsNullOrEmpty(camp.ValorNuevo))
                                            {

                                                DetalleSolicitud = DetalleSolicitud + "<br/> - " + camp.NombreCampo + ": <strong>" + camp.ValorNuevo + "</strong>";
                                            }

                                            position++;

                                        }

                                        //==== Envío de correo ===========
                                        try
                                        {
                                            if (flagCrear)
                                            {
                                                DetalleSolicitud = DetalleSolicitud + detalleCrear;
                                            }
                                            if (flagEliminar)
                                            {
                                                DetalleSolicitud = DetalleSolicitud + detalleEliminar;
                                            }

                                            string NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_EQUIVALENCIAS";
                                            string NombreTecnologia = entidad.ClaveTecnologia; //[NombreTecnologia]
                                            string NombreCreador = objeto.UsuarioNombre.ToUpper();  //[NombreCreador]
                                            string SolicitudId = cabecera.SolicitudTecnologiaId.ToString(); //[SolicitudId]
                                            string CodigoProducto = entidad.Producto.Codigo; // [CodigoProducto]
                                            //string DetalleSolicitud = cabecera.MotivoDesactiva; // [DetalleSolicitud]
                                            
                                            var listaPara = string.IsNullOrWhiteSpace(listCorreos) ? null : listCorreos.Split(';').ToList();
                                            var listConCopia = string.IsNullOrWhiteSpace(objeto.UsuarioMail) ? null : objeto.UsuarioMail.Split(';').ToList();

                                            var mailManager = new MailingManager();
                                            var diccionario = new Dictionary<string, string>();
                                            diccionario.Add("[NombreTecnologia]", NombreTecnologia);
                                            diccionario.Add("[NombreCreador]", NombreCreador);
                                            diccionario.Add("[SolicitudId]", SolicitudId);
                                            diccionario.Add("[CodigoProducto]", CodigoProducto);
                                            diccionario.Add("[DetalleSolicitud]", DetalleSolicitud);

                                            mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologia, listaPara, listConCopia);
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            HelperLog.Error(ex.Message);
                                        }

                                    }
                                    i = i + 1;
                                }
                                //
                            }
                            ID = entidad.TecnologiaId;
                        }

                        transaction.Commit();
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionEquivalencia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int EnviarSolicitudAprobacionEquivalencia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }

        }
        public override int EnviarSolicitudAprobacionDesactivacion(TecnologiaDTO technologyDto)
        {
            try
            { 
                DbContextTransaction transaction;
                var technology = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnology(technologyDto.Id); 
                using(var context = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = context.Database.BeginTransaction())
                    {
                        if (!ReferenceEquals(null, technology))
                        {
                            //1. Inserción de solicitud
                            var technologyRequestObj = new SolicitudTecnologia()
                            {
                                TipoSolicitud = ((int)FlujoTipoSolicitud.Desactivacion),
                                TecnologiaId = technologyDto.Id,
                                EstadoSolicitud = ((int)FlujoEstadoSolicitud.Pendiente),
                                MotivoDesactivaId = technologyDto.MotivoDesactivacionId,
                                MotivoDesactiva = (string.IsNullOrEmpty(technologyDto.MotivoDesactivacion) ? null : technologyDto.MotivoDesactivacion),
                                UsuarioSolicitante = technologyDto.UsuarioMatricula,
                                NombreUsuarioSolicitante = technologyDto.UsuarioNombre.ToUpper(),
                                EmailSolicitante = technologyDto.UsuarioMail,
                                UsuarioCreacion = technologyDto.UsuarioMatricula,
                                FechaCreacion = DateTime.Now,
                            };
                            context.SolicitudTecnologia.Add(technologyRequestObj);
                            context.SaveChanges();

                            #region Validation email AzureManager
                            var azureManager = new AzureGroupsManager();
                            var parameterEmail = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FLUJO_CORREO_CVT");
                            var netwokr_group = context.Parametro.Where(h => h.Codigo == "FLUJO_GRUPO_CVT").FirstOrDefault();
                            //var emailOwner = technology.Producto.OwnerMatricula;
                            string[] listnetwork_group = new string[] { };

                            if (!ReferenceEquals(null, parameterEmail))
                                listnetwork_group = parameterEmail.Valor.ToString().Split(';');

                            //var responsible_mailbox = ServiceManager<ApplicationDAO>.Provider.GetCorreoPorMatricula(network_group);
                            var requestTechnologyFlowObj = new SolicitudTecnologiaFlujo()
                            {
                                SolicitudTecnologiaId = (int)technologyRequestObj.SolicitudTecnologiaId,
                                TipoFlujo = ((int)FlujoTipo.CVT), 
                                EstadoFlujo = ((int)FlujoEstadoSolicitud.Pendiente),
                                ResponsableMatricula = (string.IsNullOrEmpty(netwokr_group.Valor) ? technologyDto.UsuarioMatricula : netwokr_group.Valor),
                                ResponsableBuzon = (string.IsNullOrEmpty(parameterEmail.Valor) ? technologyDto.UsuarioMatricula : parameterEmail.Valor),
                                UsuarioCreacion = technologyDto.UsuarioMatricula,
                                FechaCreacion = DateTime.Now,
                            };
                            context.SolicitudTecnologiaFlujo.Add(requestTechnologyFlowObj);
                            context.SaveChanges();
                            #endregion

                            var DetalleSolicitud = string.Empty;
                            var requestTechFields = new SolicitudTecnologiaCampos();
                            var descripcionTech = new SolicitudTecnologiaCampos();
                            var lisrequestTechFields = new List<SolicitudTecnologiaCampos>();
                            var idTechConfiguration = context.ConfiguracionTecnologiaCampos.Where(x => x.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoEliminacionTecnologia).FirstOrDefault();
                            var idDescTechConfiguration = context.ConfiguracionTecnologiaCampos.Where(x => x.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.DescripcionEliminacionTecnologia).FirstOrDefault();
                            var deactivation_reason = context.TecnologiaDetalleMaestra.Where(o => o.TecnologiaDetalleMaestraId == technologyDto.MotivoDesactivacionId && o.KeyCampo == "MDN" && o.Activo).Select(i => new { Descripcion = i.Descripcion }).FirstOrDefault();

                            #region Inserción de Motivo de desactivación
                            if (deactivation_reason.Descripcion == "Otros")
                            { 
                                requestTechFields.SolicitudTecnologiaId = (int)technologyRequestObj.SolicitudTecnologiaId;
                                requestTechFields.ConfiguracionTecnologiaCamposId = (int)idTechConfiguration.ConfiguracionTecnologiaCamposId;
                                requestTechFields.ValorAnterior = string.Empty;
                                requestTechFields.ValorNuevo = technologyDto.MotivoDesactivacionId.ToString();
                                requestTechFields.EstadoCampo = ((int)FlujoEstadoSolicitud.Pendiente);
                                requestTechFields.UsuarioCreacion = technologyDto.UsuarioMatricula.ToString();
                                requestTechFields.FechaCreacion = DateTime.Now;
                                lisrequestTechFields.Add(requestTechFields);
                                context.SolicitudTecnologiaCampos.Add(requestTechFields);
                                context.SaveChanges();

                                if (!string.IsNullOrEmpty(technologyDto.MotivoDesactivacion))
                                {
                                    descripcionTech.SolicitudTecnologiaId = (int)technologyRequestObj.SolicitudTecnologiaId;
                                    descripcionTech.ConfiguracionTecnologiaCamposId = (int)idDescTechConfiguration.ConfiguracionTecnologiaCamposId;
                                    descripcionTech.ValorAnterior = string.Empty;
                                    descripcionTech.ValorNuevo = technologyDto.MotivoDesactivacion;
                                    descripcionTech.EstadoCampo = ((int)FlujoEstadoSolicitud.Pendiente);
                                    descripcionTech.UsuarioCreacion = technologyDto.UsuarioMatricula.ToString();
                                    descripcionTech.FechaCreacion = DateTime.Now;
                                    lisrequestTechFields.Add(descripcionTech);
                                    context.SolicitudTecnologiaCampos.Add(descripcionTech);
                                    context.SaveChanges();
                                } 
                            }
                            else
                            {
                                requestTechFields.SolicitudTecnologiaId = (int)technologyRequestObj.SolicitudTecnologiaId;
                                requestTechFields.ConfiguracionTecnologiaCamposId = (int)idTechConfiguration.ConfiguracionTecnologiaCamposId;
                                requestTechFields.ValorAnterior = string.Empty;
                                requestTechFields.ValorNuevo = technologyDto.MotivoDesactivacionId.ToString();
                                requestTechFields.EstadoCampo = ((int)FlujoEstadoSolicitud.Pendiente);
                                requestTechFields.UsuarioCreacion = technologyDto.UsuarioMatricula.ToString();
                                requestTechFields.FechaCreacion = DateTime.Now;
                                lisrequestTechFields.Add(requestTechFields);
                                context.SolicitudTecnologiaCampos.Add(requestTechFields);
                                context.SaveChanges();
                            }
                            #endregion

                            var configuracionTecnologiaCampos = context.ConfiguracionTecnologiaCampos.Where(x => x.RolAprueba == "CVT" && x.FlagActivo == 1).ToList();
                            var description_tech = new TecnologiaDetalleMaestra();
                            foreach (var item in lisrequestTechFields)
                            { 
                                var idConfig = configuracionTecnologiaCampos.Where(k => k.ConfiguracionTecnologiaCamposId == item.ConfiguracionTecnologiaCamposId).FirstOrDefault();
                                 
                                if (!string.IsNullOrEmpty(item.ValorNuevo))
                                { 
                                    if(Regex.IsMatch(item.ValorNuevo, @"^[0-9]+$"))
                                    {
                                        var id_valor = int.Parse(item.ValorNuevo);
                                        description_tech = context.TecnologiaDetalleMaestra.Where(k => k.TecnologiaDetalleMaestraId == id_valor && k.KeyCampo == "MDN" && k.Activo).FirstOrDefault();
                                        item.ValorNuevo = description_tech.Descripcion;
                                    }

                                    DetalleSolicitud = DetalleSolicitud + "<br/> - " + idConfig.NombreCampo + ": <strong>" + item.ValorNuevo + "</strong>";
                                }
                            }

                            #region Send email  
                            var NombreCorreo = "NOTIFICACION_FLUJOS_SOLICITUD_ELIMINACION";
                            var NombreTecnologia = technology.ClaveTecnologia; //[NombreTecnologia]
                            var NombreCreador = technologyDto.UsuarioNombre.ToUpper();  //[NombreCreador]
                            var SolicitudId = technologyRequestObj.SolicitudTecnologiaId.ToString(); //[SolicitudId]
                            var CodigoProducto = technology.Producto.Codigo; // [CodigoProducto]
                            //var DetalleSolicitud = technologyRequestObj.MotivoDesactiva; // [DetalleSolicitud]
                           
                            var listaPara = new List<string>();
                            //string.IsNullOrWhiteSpace(listnetwork_group.ToString()) ? null : listnetwork_group.ToString().Split(';').ToList();
                            
                            foreach (var item in listnetwork_group)
                            {
                                var oListParaMail = new Object();

                                oListParaMail = item.ToString();

                                listaPara.Add(oListParaMail.ToString());
                            }
                             
                            var listConCopia = string.IsNullOrWhiteSpace(technologyDto.UsuarioMail) ? null : technologyDto.UsuarioMail.Split(';').ToList();

                            var mailManager = new MailingManager();
                            var diccionario = new Dictionary<string, string>();

                            diccionario.Add("[NombreTecnologia]", NombreTecnologia);
                            diccionario.Add("[NombreCreador]", NombreCreador);
                            diccionario.Add("[SolicitudId]", SolicitudId);
                            diccionario.Add("[CodigoProducto]", CodigoProducto);
                            diccionario.Add("[DetalleSolicitud]", DetalleSolicitud);

                            mailManager.ProcesarEnvioNotificacionesSolicitudFlujoActualizacion(NombreCorreo, diccionario, NombreTecnologia, listaPara, listConCopia);

                            #endregion

                        }
                        transaction.Commit();
                        var idTechnology = technology.TecnologiaId.ToString() == null ? 0 : technology.TecnologiaId;
                        return idTechnology;
                    }
                } 
            } 
            catch (Exception ex)
            { 
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO, "Error en el metodo: int EnviarSolicitudAprobacionDesactivacion(TecnologiaDTO objeto)", new object[] { null });
            } 
        }
         
        public override bool ExisteInstanciasByTecnologiaId(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = ServiceManager<TecnologiaDAO>.Provider.BuscarInstanciasXTecnologia(Id);

                        int total = registros.TotalInstanciasServidores + registros.TotalInstanciasServicioNube + registros.TotalInstanciasPcs;

                        bool estado = false;
                        if (total > 0)
                        {
                            estado = true;
                        }

                        return estado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteRelacionByTecnologiaId(int Id)"
                    , new object[] { null });
            }
        }
        public override TecnologiaOwnerDto BuscarInstanciasXTecnologia(int idTecnologia)
        {
            // BuscarTecnologiaXOwner
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new TecnologiaOwnerDto();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_tecnologia_existe_instancias]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tecnologiaId", idTecnologia));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            lista.TecnologiaId = reader.GetData<int>("TecnologiaId");
                            lista.TotalInstanciasServidores = reader.GetData<int>("TotalInstanciasServidores");
                            lista.TotalInstanciasServicioNube = reader.GetData<int>("TotalInstanciasServicioNube");
                            lista.TotalInstanciasPcs = reader.GetData<int>("TotalInstanciasPcs");
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
                    , "Error en el metodo: TecnologiaOwnerDto BuscarInstanciasXTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
        }

        public override List<SolicitudFlujoDetalleDTO> GetDetalleSolicitudFlujo(PaginacionSolicitud pag, out int totalRows)
        {
            totalRows = 0;
            var position = 0;
            //1. Se obtiene el valor nuevo de los registros
            var listRequestFlowDetail = ServiceManager<TechnologyConfigurationDAO>.Provider.GetRequestFlowDetail(pag.SolicitudAplicacionId);
            //2. Se obtiene el valor anterior de los registros
            var technologyData = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnology(pag.TecnologiaId);
            var solicitudFlujo = new SolicitudFlujoDetalleDTO();
            var listSolicitudFlujo = new List<SolicitudFlujoDetalleDTO>();
            var techonologyType = new TipoFlujoConfiguracion();
            var listEquivalences = new List<SolicitudFlujoDetalleDTO>();

            foreach (var item in listRequestFlowDetail)
            {
                    #region Validacion de data
                    var oTipoFlujoConfiguracion = new TipoFlujoConfiguracion();
                    switch (item.CorrelativoCampo)
                    {
                        case (int)FlujoConfiguracionTecnologiaCampos.Dominio:
                            #region Dominio
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case (int)FlujoConfiguracionTecnologiaCampos.Subdominio:
                            #region Subdominio
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.TipoProducto):
                            #region TipoProducto
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida):
                            #region TipoCicloVida
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia):
                            #region TipoTecnologia 
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId):
                            #region TipoTecnologia 
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida):
                            #region TipoTecnologia  
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.Fuente):
                            #region Fuente 
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case ((int)FlujoConfiguracionTecnologiaCampos.FechaCalculo):
                            #region FechaCalculo 
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            break;
                        #endregion
                        case (int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma:
                            #region Indicar la plataforma a la que aplica
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            #endregion
                            break;
                        case ((int)FlujoConfiguracionTecnologiaCampos.EstadoId):
                            #region EstadoId 
                            oTipoFlujoConfiguracion = GetValidateRegistrationInformation(item.ValorAnterior, item.ValorNuevo, item.CorrelativoCampo);
                            #endregion
                            break;
                    }
                    #endregion
                    if (position.Equals(1) && pag.TecnologiaId != 0)
                    {
                        if (listRequestFlowDetail[1].CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias)
                        {
                            var flagEquivalence = listRequestFlowDetail[1].CorrelativoCampo;
                            var idsCorrelative = listRequestFlowDetail.Select(k => k.CorrelativoCampo).ToList();
                            if (idsCorrelative.Contains((int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias))
                                techonologyType = GetTechnologyValues(flagEquivalence, item.ValorNuevo, item.ValorAnterior, (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias);
                        }
                        else
                        {
                            if (pag.TecnologiaId != 0)
                            {
                                if (technologyData.TecnologiaEquivalencia != null)
                                {
                                    if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias) { techonologyType = GetTechnologyValues(item.CorrelativoCampo, "True", item.ValorAnterior); }
                                    else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.AplicacionAsociada) { techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior); }
                                    else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada)
                                    {
                                        var application_description = ServiceManager<TechnologyConfigurationDAO>.Provider.GetApplicationTechnology(Int32.Parse(item.ValorAnterior));
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, application_description, application_description);
                                    }
                                    else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId)
                                    {
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                    }
                                    else { techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior); }
                                }
                                else
                                {
                                    var nombre_equivalencia = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyEquivalence(pag.TecnologiaId, Int32.Parse(item.ValorAnterior));
                                    //technologyData.TecnologiaEquivalencia.Where(o => o.TecnologiaEquivalenciaId == Int32.Parse(item.ValorAnterior)).Select(k => k.Nombre).FirstOrDefault();
                                    if (string.IsNullOrEmpty(nombre_equivalencia))
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                    else
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, nombre_equivalencia, nombre_equivalencia);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pag.TecnologiaId != 0)
                        {
                            if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.EstadoTecnologia)
                            {
                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior, null, pag.SolicitudAplicacionId);
                            }
                            else
                            {
                                if (technologyData.TecnologiaEquivalencia != null)
                                {
                                    var ids = listRequestFlowDetail.Where(g => g.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias).FirstOrDefault();

                                    if (!ReferenceEquals(null, ids))
                                    {
                                        if (ids.ValorNuevo != "-1" || ids.ValorNuevo != null)
                                        {
                                            if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias)
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                            }
                                            else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId)
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                            }
                                            else
                                            {
                                                if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias)
                                                {
                                                    var nombre_equivalencia = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyEquivalence(pag.TecnologiaId, Int32.Parse(item.ValorAnterior));
                                                    techonologyType = GetTechnologyValues(item.CorrelativoCampo, nombre_equivalencia, nombre_equivalencia);
                                                }
                                                //else if(item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias)
                                                //    techonologyType = GetTechnologyValues(item.CorrelativoCampo, nombre_equivalencia, nombre_equivalencia);
                                                else
                                                {
                                                    techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias)
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, "True", item.ValorAnterior);
                                            }
                                            else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias)
                                            {
                                                var nombre_equivalencia = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyEquivalence(pag.TecnologiaId, Int32.Parse(item.ValorAnterior));
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, nombre_equivalencia, nombre_equivalencia);
                                            }
                                            else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId)
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                            }
                                            else
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada)
                                        {
                                            var application_description = ServiceManager<TechnologyConfigurationDAO>.Provider.GetApplicationTechnology(Int32.Parse(item.ValorAnterior));
                                            techonologyType = GetTechnologyValues(item.CorrelativoCampo, application_description, application_description);
                                        }
                                        else if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId)
                                        {
                                            techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                        }
                                        else
                                        {

                                            if (oTipoFlujoConfiguracion.ValorAnterior == "1")
                                            {
                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, null);
                                            }
                                            else
                                            {

                                                if (!string.IsNullOrEmpty(item.ValorAnterior) && !string.IsNullOrEmpty(item.ValorNuevo))
                                                {
                                                    if (Regex.IsMatch(item.ValorAnterior, @"^[0-9]+$") || Regex.IsMatch(item.ValorNuevo, @"^[0-9]+$"))
                                                    {

                                                        if (Int32.Parse(item.ValorAnterior) == 0)
                                                            item.ValorAnterior = string.Empty;

                                                        if (Int32.Parse(item.ValorNuevo) == 0)
                                                            item.ValorAnterior = string.Empty;
                                                    }
                                                }

                                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (oTipoFlujoConfiguracion.ValorAnterior == "1")
                                    {
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, null);
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(item.ValorAnterior, @"^[0-9]+$") || Regex.IsMatch(item.ValorNuevo, @"^[0-9]+$"))
                                        {
                                            if (Int32.Parse(item.ValorAnterior) == 0)
                                                item.ValorAnterior = string.Empty;

                                            if (Int32.Parse(item.ValorNuevo) == 0)
                                                item.ValorAnterior = string.Empty;
                                        }
                                        techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (oTipoFlujoConfiguracion.ValorAnterior == "1")
                            {
                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, null);
                            }
                            else
                            {
                                if (Regex.IsMatch(item.ValorAnterior, @"^[0-9]+$") || Regex.IsMatch(item.ValorNuevo, @"^[0-9]+$"))
                                {
                                    if(item.ValorAnterior != "")
                                    {
                                        if (Int32.Parse(item.ValorAnterior) == 0)
                                            item.ValorAnterior = string.Empty;
                                    }
                                    
                                    if (Int32.Parse(item.ValorNuevo) == 0)
                                        item.ValorAnterior = string.Empty;
                                }
                                techonologyType = GetTechnologyValues(item.CorrelativoCampo, item.ValorNuevo, item.ValorAnterior);
                            }
                        }
                    }

                    var listConfiguration = listRequestFlowDetail.Where(cn => cn.FlagConfiguration == 0).ToList();

                    if (string.IsNullOrEmpty(techonologyType.ValorAnterior) && string.IsNullOrEmpty(techonologyType.ValorNuevo))
                    {

                        if (item.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias)
                        {
                            solicitudFlujo = SetTechnology(item, (string.IsNullOrEmpty(techonologyType.ValorNuevo) ? string.Empty : techonologyType.ValorNuevo), (string.IsNullOrEmpty(techonologyType.ValorAnterior) ? string.Empty : techonologyType.ValorAnterior));
                        }
                        else
                        {
                            if (oTipoFlujoConfiguracion.ValorAnterior == "1")
                            {
                                solicitudFlujo = SetTechnology(item, item.ValorNuevo, null);
                            }
                            else
                            {
                                solicitudFlujo = SetTechnology(item, item.ValorNuevo.ToString(), item.ValorAnterior.ToString());
                            }
                        }
                    }
                    else
                    {
                        if (oTipoFlujoConfiguracion.ValorAnterior == "1")
                        {
                            solicitudFlujo = SetTechnology(item, techonologyType.ValorNuevo, null);
                        }
                        else
                        {
                            solicitudFlujo = SetTechnology(item, (string.IsNullOrEmpty(techonologyType.ValorNuevo) ? string.Empty : techonologyType.ValorNuevo),
                                             (string.IsNullOrEmpty(techonologyType.ValorAnterior) ? string.Empty : techonologyType.ValorAnterior));
                        }
                    }

                    listSolicitudFlujo.Add(solicitudFlujo);
                    position++;
                }
            var listRequest = new List<SolicitudFlujoDetalleDTO>();
            var result = new List<SolicitudFlujoDetalleDTO>();

            var motive_equivalences = listSolicitudFlujo.Where(u => u.ConfiguracionTecnologiaCamposId == (int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias).FirstOrDefault();

            if (motive_equivalences != null)
            {
                var itemRemove = listSolicitudFlujo.Where(x => string.IsNullOrEmpty(x.ValorNuevo)).ToList();
                listSolicitudFlujo = listSolicitudFlujo.Except(itemRemove).ToList();
            }

            var listInformation = listSolicitudFlujo.Select(o => new { ValorAnterior = o.ValorAnterior, ValorNuevo = o.ValorNuevo }).ToList();
            listInformation.ForEach(h =>
            {

                if (string.IsNullOrEmpty(h.ValorAnterior) && string.IsNullOrEmpty(h.ValorNuevo))
                {
                    var item_remove_information = listSolicitudFlujo.Where(x => string.IsNullOrEmpty(x.ValorNuevo) && string.IsNullOrEmpty(x.ValorAnterior)).ToList();
                    listSolicitudFlujo = listSolicitudFlujo.Except(item_remove_information).ToList();
                }

            });

            foreach (var request in listRequestFlowDetail)
            {
                var idSolicitud = listSolicitudFlujo.Select(sf => sf.SolicitudTecnologiaCamposId).ToList();

                if (idSolicitud.Contains(request.SolicitudTecnologiaCamposId))
                {
                    var solicitud = listSolicitudFlujo.Where(o => o.SolicitudTecnologiaCamposId == request.SolicitudTecnologiaCamposId).FirstOrDefault();


                    var technologyObj = SetTechnology(solicitud, solicitud.ValorNuevo.ToString().Trim(), solicitud.ValorAnterior.ToString().Trim());
                    result.Add(technologyObj);
                }
            }

            totalRows = result.Count();
            var resultado = result.ToList();

            return resultado;
        }

        private TipoFlujoConfiguracion GetValidateRegistrationInformation(string valor_anterior_request, string valor_nuevo_request, int? idTecnologiaCampos)
        {
            var oTipoFlujoConfiguracion = new TipoFlujoConfiguracion();

            switch (idTecnologiaCampos)
            {
                case (int)FlujoConfiguracionTecnologiaCampos.Dominio:
                    #region Dominio
                    var dominioid = Int32.Parse(valor_anterior_request);
                    var dataDominio = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewDomainValue(dominioid);

                    if (ReferenceEquals(null, dataDominio))
                    { 
                        if (valor_anterior_request == "" ||  Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = dataDominio;
                    }


                    break;
                #endregion
                case (int)FlujoConfiguracionTecnologiaCampos.Subdominio:
                    #region Subdominio
                    var subdominioId = Int32.Parse(valor_anterior_request);
                    var oDataSubDominio = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewSubDomainValue(subdominioId);

                    if (ReferenceEquals(null, oDataSubDominio))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oDataSubDominio;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoProducto):
                    #region TipoProducto

                    var tipoproductoId = Int32.Parse(valor_anterior_request);
                    var oDataTipoProducto = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(tipoproductoId);

                    if (ReferenceEquals(null, oDataTipoProducto))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oDataTipoProducto;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida):
                    #region TipoCicloVida

                    var tipociclovidaId = Int32.Parse(valor_anterior_request);
                    var oDataTipoCicloVida = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueTypeLifeCycle(tipociclovidaId);

                    if (ReferenceEquals(null, oDataTipoCicloVida))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oDataTipoCicloVida;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia):
                    #region TipoTecnologia 
                    var tipotecnologiaId = Int32.Parse(valor_anterior_request);
                    var oDataTipoTecnologia = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(tipotecnologiaId);

                    if (ReferenceEquals(null, oDataTipoTecnologia))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oDataTipoTecnologia;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.AutomatizacionImplementacionId):
                    #region TipoTecnologia 
                    var automatizacionImplementacionId = Int32.Parse(valor_anterior_request);
                    var oDataAutomatizacionImplemetnacion = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(automatizacionImplementacionId);

                    if (ReferenceEquals(null, oDataAutomatizacionImplemetnacion))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oDataAutomatizacionImplemetnacion;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.MotivoFechaIndefinida):
                    #region TipoTecnologia  
                    var oMotivoFechaIndefinida = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetailXId(valor_anterior_request, "MFI");

                    if (ReferenceEquals(null, oMotivoFechaIndefinida))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = oMotivoFechaIndefinida.Descripcion;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.Fuente):
                    #region Fuente 
                    if (!Regex.IsMatch(valor_anterior_request, @"^[0-9]+$"))
                    {
                        var oFuente = Utilitarios.EnumToList<Fuente>()
                                      .Where(o => Utilitarios.GetEnumDescription3(o) == valor_anterior_request) //CORRECCION ENUM
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                      })
                                      .FirstOrDefault();

                        if (ReferenceEquals(null, oFuente))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oFuente.Descripcion;
                        }
                    }
                    else
                    {
                        var fuenteId = Int32.Parse(valor_anterior_request);
                        var oFuentenum = Utilitarios.EnumToList<Fuente>()
                                      .Where(o => (int)o == fuenteId)
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                      })
                                      .FirstOrDefault();
                        if (ReferenceEquals(null, oFuentenum))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oFuentenum.Descripcion;
                        }
                    }

                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.FechaCalculo):
                    #region FechaCalculo 
                    if (!Regex.IsMatch(valor_anterior_request, @"^[0-9]+$"))
                    {
                        var oFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>()
                                      .Where(o => Utilitarios.GetEnumDescription3(o) == valor_anterior_request) //CORRECCION ENUM
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                      })
                                      .FirstOrDefault();

                        if (ReferenceEquals(null, oFechaCalculo))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oFechaCalculo.Descripcion;
                        }
                    }
                    else
                    {
                        var oFechaCalculoId = Int32.Parse(valor_anterior_request);
                        var oFechaCalculoNum = Utilitarios.EnumToList<FechaCalculoTecnologia>()
                                      .Where(o => (int)o == oFechaCalculoId)
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription2(x)
                                      })
                                      .FirstOrDefault();
                        if (ReferenceEquals(null, oFechaCalculoNum))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }

                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oFechaCalculoNum.Descripcion;
                        }
                    }
                    break;
                #endregion
                case (int)FlujoConfiguracionTecnologiaCampos.IndicarPlataforma:
                    #region Indicar la plataforma a la que aplica

                    var data = Utilitarios.EnumToList<EAplicaATecnologia>()
                                  .Where(o => Utilitarios.GetEnumDescription3(o) == valor_anterior_request.ToString()) //CORRECCION ENUM
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();

                    if (ReferenceEquals(null, data))
                    {
                        if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "";
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = "1";
                        }
                    }
                    else
                    {
                        oTipoFlujoConfiguracion.ValorAnterior = data.Descripcion;
                    }

                    #endregion
                    break;
                case (int)FlujoConfiguracionTecnologiaCampos.EstadoId:
                    #region EstadoId
                    if (!Regex.IsMatch(valor_anterior_request, @"^[0-9]+$"))
                    {
                        var oEstado = Utilitarios.EnumToList<ETecnologiaEstado>()
                                      .Where(o => Utilitarios.GetEnumDescription3(o) == valor_anterior_request) //CORRECCION ENUM
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                      })
                                      .FirstOrDefault();

                        if (ReferenceEquals(null, oEstado))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oEstado.Descripcion;
                        }
                    }
                    else
                    {
                        var estadoId = Int32.Parse(valor_anterior_request);
                        var oEstadoNum = Utilitarios.EnumToList<ETecnologiaEstado>()
                                      .Where(o => (int)o == estadoId)
                                      .Select(x => new MasterDetail
                                      {
                                          Id = (int)x,
                                          Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                      })
                                      .FirstOrDefault();
                        if (ReferenceEquals(null, oEstadoNum))
                        {
                            if (valor_anterior_request == "" || Int32.Parse(valor_anterior_request) == 0)
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "";
                            }
                            else
                            {
                                oTipoFlujoConfiguracion.ValorAnterior = "1";
                            }
                        }
                        else
                        {
                            oTipoFlujoConfiguracion.ValorAnterior = oEstadoNum.Descripcion;
                        }
                    }
                    #endregion
                    break;
            }

            return oTipoFlujoConfiguracion;
        }

        private SolicitudFlujoDetalleDTO SetTechnology(SolicitudFlujoDetalleDTO flujoDetalleDTO = null, string valorNuevo = null, string valorAnterior = null)
        {
            SolicitudFlujoDetalleDTO technology;
            if (ReferenceEquals(null, flujoDetalleDTO))
            {
                technology = new SolicitudFlujoDetalleDTO
                { 
                    ValorAnterior = (string.IsNullOrEmpty(valorAnterior) ? string.Empty : valorAnterior.ToString()),  
                };
            }
            else
            {
                technology = new SolicitudFlujoDetalleDTO
                {
                    NombreCampo = flujoDetalleDTO.NombreCampo.ToString(),
                    TablaProcedencia = flujoDetalleDTO.TablaProcedencia.ToString(),
                    RolAprueba = flujoDetalleDTO.RolAprueba.ToString(),
                    ValorAnterior = flujoDetalleDTO.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula ? (string.IsNullOrEmpty(valorAnterior) ? string.Empty : (valorAnterior.Length == 6 ? valorAnterior.ToString() : valorAnterior.Substring(1).ToString())) : (string.IsNullOrEmpty(valorAnterior) ? string.Empty : valorAnterior.ToString()),
                    ValorNuevo = flujoDetalleDTO.CorrelativoCampo == (int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula ? (string.IsNullOrEmpty(valorNuevo) ? string.Empty : (valorNuevo.Length == 6 ? valorNuevo.ToString() : valorNuevo.Substring(1).ToString())) : (string.IsNullOrEmpty(valorNuevo) ? string.Empty : valorNuevo.ToString()),
                    SolicitudTecnologiaCamposId = (int)flujoDetalleDTO.SolicitudTecnologiaCamposId,
                    ConfiguracionTecnologiaCamposId = flujoDetalleDTO.CorrelativoCampo,
                    EstadoCampo = flujoDetalleDTO.EstadoCampo,
                    CorrelativoCampo = flujoDetalleDTO.CorrelativoCampo
                };
            } 

            return technology;
        }

        private TipoFlujoConfiguracion GetTechnologyValues(int? idTechnology, string valorNuevo = "", string valorAnterior = "", int? idMotivoEquivalencia = null, int? idApplication = null)
        {
            var technologyType = new TipoFlujoConfiguracion();
            var idNewValue = 0;
            var idPreviousValue = 0;
            var newValue = string.Empty;
            var previoustValue = string.Empty;

            switch (idTechnology)
            {
                case ((int)FlujoConfiguracionTecnologiaCampos.Dominio):
                    #region Dominio
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        previoustValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewDomainValue(idPreviousValue);
                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        newValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewDomainValue(idNewValue);
                        technologyType.ValorNuevo = newValue;
                    }

                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.Subdominio):
                    #region SubDominio
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        previoustValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewSubDomainValue(idPreviousValue);
                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        newValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewSubDomainValue(idNewValue);
                        technologyType.ValorNuevo = newValue;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoProducto):
                    #region TipoProducto
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        previoustValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(idPreviousValue);
                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        newValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(idNewValue);
                        technologyType.ValorNuevo = newValue;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoCicloVida):
                    #region TipoCicloVida
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        if (valorAnterior != "-1")
                            previoustValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueTypeLifeCycle(idPreviousValue);
                        else
                            previoustValue = null;

                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        if (valorNuevo != "-1")
                            newValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueTypeLifeCycle(idNewValue);
                        else
                            newValue = null;

                        technologyType.ValorNuevo = newValue;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TipoTecnologia):
                    #region TipoTecnologia
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        previoustValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(idPreviousValue);
                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        newValue = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueType(idNewValue);
                        technologyType.ValorNuevo = newValue;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.FlagFechaFinSoporte):
                    #region FlagFechaFinSoporte
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        if (bool.Parse(valorAnterior) || !bool.Parse(valorAnterior))
                            previoustValue = (bool.Parse(valorAnterior) == true ? "Si" : "No");
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        if (bool.Parse(valorNuevo) || !bool.Parse(valorNuevo))
                            newValue = (bool.Parse(valorNuevo) == true ? "Si" : "No");
                    }

                    technologyType.ValorAnterior = previoustValue.ToString();
                    technologyType.ValorNuevo = newValue.ToString();
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.Fuente):
                    #region Fuente
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        if (valorAnterior != "-1")
                            idPreviousValue = Int32.Parse(valorAnterior);
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        if (valorNuevo != "-1")
                            idNewValue = Int32.Parse(valorNuevo);
                    }
                    //CORRECCION ENUM
                    technologyType.ValorAnterior = (valorAnterior == "-1" || string.IsNullOrEmpty(valorAnterior) ? string.Empty : Utilitarios.GetEnumDescription3((Fuente)idPreviousValue)).ToString();
                    technologyType.ValorNuevo = (valorNuevo == "-1" || string.IsNullOrEmpty(valorNuevo) ? string.Empty : Utilitarios.GetEnumDescription3((Fuente)idNewValue)).ToString();
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.FechaCalculo):
                    #region FechaCalculo
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        if (valorAnterior != "-1")
                            idPreviousValue = Int32.Parse(valorAnterior);
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        if (valorNuevo != "-1")
                            idNewValue = Int32.Parse(valorNuevo);
                    }
                    //CORRECCION ENUM
                    technologyType.ValorAnterior = (valorAnterior == "-1" || string.IsNullOrEmpty(valorAnterior) ? string.Empty : Utilitarios.GetEnumDescription3((FechaCalculoTecnologia)idPreviousValue)).ToString();
                    technologyType.ValorNuevo = (valorNuevo == "-1" || string.IsNullOrEmpty(valorNuevo) ? string.Empty : Utilitarios.GetEnumDescription3((FechaCalculoTecnologia)idNewValue)).ToString();
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TribuCOE):
                    #region TribuCOE
                    technologyType.ValorNuevo = string.Empty;
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.SquadEquipo):
                    #region SquadEquipo
                    technologyType.ValorNuevo = string.Empty;
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquad):
                    #region ResponsableSquad
                    technologyType.ValorNuevo = string.Empty;
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.ResponsableSquadMatricula):
                    #region ResponsableSquadMatricula
                    technologyType.ValorNuevo = string.Empty;
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.AplicacionAsociada):
                    #region AplicacionAsociada
                    var application = new Aplicacion();

                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        idPreviousValue = Int32.Parse(valorAnterior);
                        application = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueApplication(idPreviousValue);
                        previoustValue = application.CodigoAPT + " - " + application.Nombre;
                        technologyType.ValorAnterior = previoustValue;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        idNewValue = Int32.Parse(valorNuevo);
                        application = ServiceManager<TechnologyConfigurationDAO>.Provider.GetNewValueApplication(idNewValue);
                        newValue = application.CodigoAPT + " - " + application.Nombre;
                        technologyType.ValorNuevo = newValue;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.FlagTieneEquivalencias):
                    #region FlagTieneEquivalencias
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        if (bool.Parse(valorAnterior) || !bool.Parse(valorAnterior))
                            previoustValue = (bool.Parse(valorAnterior) == true ? "Si" : "No");
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        if (bool.Parse(valorNuevo) || !bool.Parse(valorNuevo))
                            newValue = (bool.Parse(valorNuevo) == true ? "Si" : "No");
                    }

                    if (idMotivoEquivalencia.Equals((int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias))
                        newValue = string.Empty;

                    technologyType.ValorAnterior = previoustValue.ToString();
                    technologyType.ValorNuevo = newValue.ToString();
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.MotivoNoEquivalencias):
                    #region MotivoNoEquivalencias
                    if (valorAnterior != "-1")
                        idPreviousValue = Int32.Parse(valorAnterior);

                    if (valorNuevo != "-1")
                        idNewValue = Int32.Parse(valorNuevo);

                    technologyType.ValorAnterior = (valorAnterior == "-1" || string.IsNullOrEmpty(valorAnterior) ? string.Empty : ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyReason(idPreviousValue));
                    technologyType.ValorNuevo = (valorNuevo == "-1" || string.IsNullOrEmpty(valorNuevo) ? string.Empty : ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyReason(idNewValue));
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.EstadoTecnologia):
                    #region EstadoTecnologia 
                    technologyType.ValorNuevo = (string.IsNullOrEmpty(valorNuevo) ? string.Empty : ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnologyRequest((int)idApplication));
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.EliminarEquivalencias):
                    #region EliminarEquivalencias
                    if (!string.IsNullOrEmpty(valorAnterior))
                        technologyType.ValorAnterior = valorAnterior;
                    else
                        technologyType.ValorAnterior = string.Empty;

                    if (!string.IsNullOrEmpty(valorNuevo))
                        technologyType.ValorNuevo = valorNuevo;
                    else
                        technologyType.ValorNuevo = string.Empty;

                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.AplicacionEliminada):
                    #region AplicacionEliminada
                    technologyType.ValorAnterior = valorAnterior;
                    technologyType.ValorNuevo = valorNuevo;
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.MotivoEliminacionTecnologia):
                    #region MotivoEliminacionTecnologia 
                    if (!string.IsNullOrEmpty(valorNuevo))
                    { 
                        var list_master_detail = ServiceManager<TechnologyConfigurationDAO>.Provider.GetMasterDetail("MDN");
                        idNewValue = Int32.Parse(valorNuevo);
                        newValue = list_master_detail.Where(u => u.Id == idNewValue).FirstOrDefault().Descripcion;
                        technologyType.ValorNuevo = newValue;
                    } 
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.DescripcionEliminacionTecnologia):
                    #region DescripcionEliminacionTecnologia 
                    if (!string.IsNullOrEmpty(valorNuevo))
                    {  
                        technologyType.ValorNuevo = valorNuevo;
                    }
                    break;
                #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.EstadoId):
                    #region EstadoId
                    if (!string.IsNullOrEmpty(valorAnterior))
                    {
                        if (valorAnterior != "-1")
                            idPreviousValue = Int32.Parse(valorAnterior);
                    }

                    if (!string.IsNullOrEmpty(valorNuevo))
                    {
                        if (valorNuevo != "-1")
                            idNewValue = Int32.Parse(valorNuevo);
                    }
                    //CORRECCION ENUM
                    technologyType.ValorAnterior = (valorAnterior == "-1" || string.IsNullOrEmpty(valorAnterior) ? string.Empty : Utilitarios.GetEnumDescription3((ETecnologiaEstado)idPreviousValue)).ToString();
                    technologyType.ValorNuevo = (valorNuevo == "-1" || string.IsNullOrEmpty(valorNuevo) ? string.Empty : Utilitarios.GetEnumDescription3((ETecnologiaEstado)idNewValue)).ToString();
                    break;
                    #endregion
                case ((int)FlujoConfiguracionTecnologiaCampos.TecReemplazoDepId):
                    #region TecReemplazoDeprecado
                    if (!string.IsNullOrEmpty(valorAnterior)) 
                    {
                        var tecReemplazo_data = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnology(Int32.Parse(valorAnterior));
                        var tecReemplazo_description = tecReemplazo_data.ClaveTecnologia;
                        technologyType.ValorAnterior = tecReemplazo_description;
                    }
                        
                    if (!string.IsNullOrEmpty(valorNuevo)) 
                    {
                        var tecReemplazo_data = ServiceManager<TechnologyConfigurationDAO>.Provider.GetTechnology(Int32.Parse(valorNuevo));
                        var tecReemplazo_description = tecReemplazo_data.ClaveTecnologia;
                        technologyType.ValorNuevo = tecReemplazo_description;
                    }
                    break;
                    #endregion
            }

            return technologyType;
        }

        public override List<TecnologiaEquivalenciaDTO> GetEquivalenciasPorTecnologia(int idTecnologia)
        {
            List<TecnologiaEquivalenciaDTO> registros = new List<TecnologiaEquivalenciaDTO>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        registros = (from u in ctx.Tecnologia
                                         join te in ctx.TecnologiaEquivalencia on u.TecnologiaId equals te.TecnologiaId
                                         where u.TecnologiaId == (idTecnologia == 0 ? u.TecnologiaId : idTecnologia)
                                         && u.Activo
                                         && te.FlagActivo
                                         select new TecnologiaEquivalenciaDTO
                                         {
                                             Id = te.TecnologiaEquivalenciaId,
                                             TecnologiaId = te.TecnologiaId,
                                             Nombre = te.Nombre,
                                             EstadoId = u.EstadoId,
                                         })
                                         .OrderBy(x => x.Nombre)
                                         .ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaEquivalenciaDTO> GetEquivalenciasPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaEquivalenciaDTO> GetEquivalenciasPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaInstanciaDTO> GetInstanciasPorTecnologia(int idTecnologia)
        {
            List<TecnologiaInstanciaDTO> registros = new List<TecnologiaInstanciaDTO>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var CURRENT_DAY = DateTime.Now;
                        var day = CURRENT_DAY.Day;
                        var month = CURRENT_DAY.Month;
                        var year = CURRENT_DAY.Year;

                        registros = (from et in ctx.EquipoTecnologia
                                     join e in ctx.Equipo on et.EquipoId equals e.EquipoId
                                     join t in ctx.Tecnologia on et.TecnologiaId equals t.TecnologiaId
                                     join s in ctx.TipoEquipo on e.TipoEquipoId equals s.TipoEquipoId
                                     join a in ctx.Ambiente on e.AmbienteId equals a.AmbienteId
                                     where t.TecnologiaId == (idTecnologia == 0 ? t.TecnologiaId : idTecnologia)
                                     && et.AnioRegistro == year
                                     && et.MesRegistro == month
                                     && et.DiaRegistro == day
                                     && (e.TipoEquipoId == 1 || e.TipoEquipoId == 4 || e.TipoEquipoId == 3)
                                     && t.Activo
                                     && !t.FlagEliminado
                                     && e.FlagActivo
                                     select new TecnologiaInstanciaDTO
                                     {
                                         TecnologiaId = t.TecnologiaId,
                                         ClaveTecnologia = t.ClaveTecnologia,
                                         TipoEquipoId = (int)e.TipoEquipoId,
                                         TipoEquipo = s.Nombre,
                                         EquipoId = e.EquipoId,
                                         Equipo = e.Nombre,
                                         AmbienteId = (int)e.AmbienteId,
                                         Ambiente = a.DetalleAmbiente
                                     })
                                     .OrderBy(x => x.TipoEquipo)
                                     .ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaInstanciaDTO> GetInstanciasPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaInstanciaDTO> GetInstanciasPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaRelacionDTO> GetRelacionesPorTecnologia(int idTecnologia)
        {
            List<TecnologiaRelacionDTO> registros = new List<TecnologiaRelacionDTO>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var CURRENT_DAY = DateTime.Now;
                        var day = CURRENT_DAY.Day;
                        var month = CURRENT_DAY.Month;
                        var year = CURRENT_DAY.Year;

                        var estadoRelacionIds = new int[]
                        {
                            (int)EEstadoRelacion.PendienteEliminacion,
                            (int)EEstadoRelacion.Aprobado,
                        };

                        registros = (from r in ctx.Relacion
                                     join rd in ctx.RelacionDetalle on r.RelacionId equals rd.RelacionId
                                     join a in ctx.Application on r.CodigoAPT equals a.applicationId
                                     join g in ctx.GestionadoPor on a.managed equals g.GestionadoPorId
                                     where rd.TecnologiaId == (idTecnologia == 0 ? rd.TecnologiaId : idTecnologia)
                                     && r.AnioRegistro == year
                                     && r.MesRegistro == month
                                     && r.DiaRegistro == day
                                     && estadoRelacionIds.Contains(r.EstadoId)
                                     && r.FlagActivo
                                     && rd.FlagActivo
                                     select new TecnologiaRelacionDTO
                                     {
                                         TecnologiaId = (int)rd.TecnologiaId,
                                         CodigoAPT = r.CodigoAPT,
                                         NombreAplicacion = a.applicationName,
                                         GestionadoPor = g.Nombre
                                     })
                                     .Distinct()
                                     .OrderBy(x => x.CodigoAPT)
                                     .ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaRelacionDTO> GetRelacionesPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaRelacionDTO> GetRelacionesPorTecnologia(int idTecnologia)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiaModificar(int? productoId, List<int> domIds, List<int> subdomIds, string nombre, string codigo, string tribuCoeStr, string squadStr, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, string perfil, out int totalRows)
        {
            try
            {
                //casoUso = "";
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var parametroMeses1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                        int cantidadMeses1 = int.Parse(parametroMeses1);

                        DateTime fechaActual = DateTime.Now;
                        DateTime fechaAgregada = fechaActual.AddMonths(cantidadMeses1);
                        List<TecnologiaG> datos = new List<TecnologiaG>();
                        bool flagAdministrador = false;
                        //=====
                        if (perfil.Contains("E195_Administrador"))
                        {
                            flagAdministrador = true;
                        }
                        //=====

                        var tecnologiaIds = new List<int>();

                        var tecExpertos = (from e in ctx.ProductoManagerRoles
                                                  where 
                                                  e.FlagActivo && e.ManagerMatricula.ToUpper().Contains(matricula.ToUpper())
                                                  select e.ProductoId
                                                ).ToList();

                        var tecEquivalenciaIds = (from e in ctx.TecnologiaEquivalencia
                                                  where e.FlagActivo && e.Nombre.ToUpper().Contains(nombre.ToUpper())
                                                  select e.TecnologiaId
                                                ).ToList();

                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId// into lj1
                                         join a in ctx.Producto on u.ProductoId equals a.ProductoId into lja
                                         from a in lja.DefaultIfEmpty()
                                         join b in ctx.Tipo on a.TipoProductoId equals b.TipoId into ljb
                                         from b in ljb.DefaultIfEmpty()
                                         join s in ctx.Subdominio on a.SubDominioId equals s.SubdominioId into ljs
                                         from s in ljs.DefaultIfEmpty()
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId into ljd
                                         from d in ljd.DefaultIfEmpty()
                                         join e in ctx.Motivo on u.MotivoId equals e.MotivoId into lje
                                         from e in lje.DefaultIfEmpty()
                                         
                                         let fechaCalculada = !u.FechaCalculoTec.HasValue ? null : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : null
                                         let estado = b.Nombre.ToLower().Contains("deprecado") ? (int)ETecnologiaEstado.Deprecado : ((u.FlagFechaFinSoporte ?? false) ? (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : ((fechaCalculada.Value < fechaActual) ? (int)ETecnologiaEstado.Obsoleto : (int)ETecnologiaEstado.Vigente))) : (int)ETecnologiaEstado.Vigente)
                                         where u.Activo == true
                                         &&
                                         (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre)
                                         || u.ClaveTecnologia.ToUpper().Contains(nombre.ToUpper())
                                         || tecEquivalenciaIds.Contains(u.TecnologiaId))

                                         && (productoId == null || u.ProductoId == productoId)
                                         && (domIds.Count == 0 || domIds.Contains(s.DominioId))
                                         && (subdomIds.Count == 0 || subdomIds.Contains(a.SubDominioId))

                                         && (estObsIds.Count == 0 || estObsIds.Contains(estado))
                                         && (tipoTecIds.Count == 0 || tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))
                                         && (a.TribuCoeDisplayName.ToUpper().Contains(tribuCoeStr) || string.IsNullOrEmpty(tribuCoeStr))
                                         && (a.SquadDisplayName.ToUpper().Contains(squadStr) || string.IsNullOrEmpty(squadStr))
                                         && ((u.CodigoProducto ?? (a != null ? (a.Codigo ?? "") : "")).ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))

                                         && (flagAdministrador == true ||
                                         ((flagAdministrador == false && a.OwnerMatricula.ToUpper().Contains(matricula.ToUpper())) ||
                                         (flagAdministrador == false && tecExpertos.Contains(a.ProductoId))))

                                         orderby u.Nombre
                                         select new TecnologiaG()
                                         {
                                             Id = u.TecnologiaId,
                                             TipoTecnologiaId = u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 4,
                                             ProductoId = u.ProductoId.HasValue ? u.ProductoId.Value : 0,
                                             Fabricante = u.Fabricante,
                                             Nombre = u.Nombre,
                                             Versiones = u.Versiones,
                                             Descripcion = u.Descripcion,
                                             MotivoId = u.MotivoId ?? 0,
                                             MotivoStr = e == null ? null : e.Nombre,
                                             AutomatizacionImplementadaId = u.AutomatizacionImplementadaId,
                                             RevisionSeguridadId = u.RevisionSeguridadId,
                                             RevisionSeguridadDescripcion = u.RevisionSeguridadDescripcion,
                                             FechaLanzamiento = u.FechaLanzamiento,
                                             Fuente = u.FuenteId,
                                             ComentariosFechaFin = u.ComentariosFechaFin,
                                             SustentoMotivo = u.SustentoMotivo,
                                             SustentoUrl = u.SustentoUrl,
                                             CasoUso = u.CasoUso,
                                             Aplica = u.Aplica,
                                             CompatibilidadSO = u.CompatibilidadSOId,
                                             CompatibilidadCloud = u.CompatibilidadCloudId,
                                             Requisitos = u.Requisitos,
                                             Existencia = u.Existencia,
                                             Riesgo = u.Riesgo,
                                             Facilidad = u.Facilidad,
                                             Vulnerabilidad = u.Vulnerabilidad,
                                             RoadmapOpcional = u.RoadmapOpcional,
                                             Referencias = u.Referencias,
                                             PlanTransConocimiento = u.PlanTransConocimiento,
                                             EsqMonitoreo = u.EsqMonitoreo,
                                             EsqPatchManagement = u.EsqPatchManagement,
                                             ConfArqSeg = u.ConfArqSegId,
                                             ConfArqTec = u.ConfArqTecId,
                                             EsqLicenciamiento = u.EsqLicenciamiento,
                                             EquipoAprovisionamiento = u.EquipoAprovisionamiento,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Tipo = t.Nombre,
                                             TipoProductoStr = b.Nombre,
                                             Estado = u.EstadoTecnologia,
                                             FechaAprobacion = u.FechaAprobacion,
                                             UsuarioAprobacion = u.UsuarioAprobacion,
                                             ClaveTecnologia = u.ClaveTecnologia,
                                             //TipoId = u.TipoId,
                                             EstadoId = u.EstadoId,
                                             FechaFinSoporte = u.FechaFinSoporte,
                                             FechaAcordada = u.FechaAcordada,
                                             FechaExtendida = u.FechaExtendida,
                                             FechaCalculoTec = u.FechaCalculoTec,
                                             FlagSiteEstandar = u.FlagSiteEstandar,
                                             FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                             FlagTieneEquivalencias = (from x in ctx.TecnologiaEquivalencia
                                                                       where x.FlagActivo && x.TecnologiaId == u.TecnologiaId
                                                                       select true).FirstOrDefault() == true,
                                             ProductoNombre = a == null ? "" : a.Fabricante + " " + a.Nombre,
                                             TribuCoeStr = a == null ? "" : a.TribuCoeDisplayName,
                                             SquadStr = a == null ? "" : a.SquadDisplayName,
                                             Dueno = u.DuenoId,
                                             CodigoProducto = u.CodigoProducto,
                                             GrupoSoporteRemedy = u.GrupoSoporteRemedy,
                                             EqAdmContacto = u.EqAdmContacto,
                                             UrlConfluenceId = u.UrlConfluenceId,
                                             UrlConfluence = u.UrlConfluence,
                                             TipoFechaInterna = u.TipoFechaInterna,
                                             CantidadMeses1 = cantidadMeses1
                                         }).OrderBy(sortName + " " + sortOrder).ToList();

                        
                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ProductoList> GetProductoModificar(int? productoId, List<int> domIds, List<int> subdomIds, string nombre, string codigo, string tribuCoeStr, string squadStr, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, string perfil, out int totalRows)
        {
            try
            {
                //casoUso = "";
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var parametroMeses1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES").Valor;
                        int cantidadMeses1 = int.Parse(parametroMeses1);

                        DateTime fechaActual = DateTime.Now;
                        DateTime fechaAgregada = fechaActual.AddMonths(cantidadMeses1);
                        List<TecnologiaG> datos = new List<TecnologiaG>();
                        bool flagAdministrador = false;
                        //=====
                        if (perfil.Contains("E195_Administrador") || perfil.Contains("E195_GestorTecnologia") || perfil.Contains("E195_GestorCVTCatalogoTecnologias"))
                        {
                            flagAdministrador = true;
                        }
                        //=====

                        var tecnologiaIds = new List<int>();

                        var tecExpertos = (from e in ctx.ProductoManagerRoles
                                           where
                                           e.FlagActivo && e.ManagerMatricula.ToUpper().Contains(matricula.ToUpper())
                                           select e.ProductoId
                                                ).ToList();

                        var tecEquivalenciaIds = (from e in ctx.TecnologiaEquivalencia
                                                  where e.FlagActivo && e.Nombre.ToUpper().Contains(nombre.ToUpper())
                                                  select e.TecnologiaId
                                                ).ToList();

                        var registros = (from u in ctx.Tecnologia
                                         join a in ctx.Producto on u.ProductoId equals a.ProductoId into lja
                                         from a in lja.DefaultIfEmpty()
                                         join b in ctx.Tipo on a.TipoProductoId equals b.TipoId into ljb
                                         from b in ljb.DefaultIfEmpty()
                                         join s in ctx.Subdominio on a.SubDominioId equals s.SubdominioId into ljs
                                         from s in ljs.DefaultIfEmpty()
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId into ljd
                                         from d in ljd.DefaultIfEmpty()

                                         let fechaCalculada = !u.FechaCalculoTec.HasValue ? null : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaExtendida ? u.FechaExtendida : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaInterna ? u.FechaAcordada : (FechaCalculoTecnologia)u.FechaCalculoTec.Value == FechaCalculoTecnologia.FechaFinSoporte ? u.FechaFinSoporte : null
                                         // let estado = b.Nombre.ToLower().Contains("deprecado") ? (int)ETecnologiaEstado.Deprecado : ((u.FlagFechaFinSoporte ?? false) ? (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : (!fechaCalculada.HasValue ? (int)ETecnologiaEstado.Obsoleto : ((fechaCalculada.Value < fechaActual) ? (int)ETecnologiaEstado.Obsoleto : (int)ETecnologiaEstado.Vigente))) : (int)ETecnologiaEstado.Vigente)
                                         where u.Activo == true
                                         &&
                                         (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre)
                                         || u.ClaveTecnologia.ToUpper().Contains(nombre.ToUpper())
                                         || tecEquivalenciaIds.Contains(u.TecnologiaId))

                                         && (productoId == null || u.ProductoId == productoId)
                                         && (domIds.Count == 0 || domIds.Contains(s.DominioId))
                                         && (subdomIds.Count == 0 || subdomIds.Contains(a.SubDominioId))

                                         && (estObsIds.Count == 0 || estObsIds.Contains(u.EstadoId.HasValue ? u.EstadoId.Value : 0)) //estObsIds.Contains(estado))
                                         && (tipoTecIds.Count == 0 || tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))
                                         && (a.TribuCoeDisplayName.ToUpper().Contains(tribuCoeStr) || string.IsNullOrEmpty(tribuCoeStr))
                                         && (a.SquadDisplayName.ToUpper().Contains(squadStr) || string.IsNullOrEmpty(squadStr))
                                         && ((u.CodigoProducto ?? (a != null ? (a.Codigo ?? "") : "")).ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))

                                         && (flagAdministrador == true ||
                                         ((flagAdministrador == false && a.OwnerMatricula.ToUpper().Contains(matricula.ToUpper())) ||
                                         (flagAdministrador == false && tecExpertos.Contains(a.ProductoId))))
                                         && (a.FlagActivo)
                                         select new ProductoList()
                                         {
                                             Id = u.ProductoId.HasValue ? u.ProductoId.Value : 0,
                                             ProductoId = u.ProductoId.HasValue ? u.ProductoId.Value : 0,
                                             Activo = u.Activo,
                                             Dominio = d.Nombre,
                                             Subdominio = s.Nombre,
                                             Estado = a.FlagActivo ? 1 : 0,
                                             ProductoNombre = a == null ? "" : a.Fabricante + " " + a.Nombre,
                                             TribuCoeStr = a == null ? "" : a.TribuCoeDisplayName,
                                             SquadStr = a == null ? "" : a.SquadDisplayName,
                                             CodigoProducto = a.Codigo,
                                         }).OrderBy(sortName + " " + sortOrder).Distinct().ToList();


                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaG> GetTecnologiaConsolidadaModificar(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string codigo, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, string perfil, string matricula, out int totalRows, string tribuCoeStr = null, string squadStr = null)
        {
            try
            {
                List<TecnologiaG> registros = new List<TecnologiaG>();
                totalRows = 0;
                int flagAdministrador = 0;
                if (perfil.Contains("E195_Administrador") || perfil.Contains("E195_GestorCVTCatalogoTecnologias")) {
                    flagAdministrador = 1;
                }

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_TECNOLOGIA_CONSOLIDADA_MODIFICAR_EXPORTAR]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@filtro", filtro == null ? DBNull.Value : (object)filtro);
                        //comando.Parameters.AddWithValue("@equipo", equipo == null ? DBNull.Value : (object)equipo);
                        comando.Parameters.AddWithValue("@productoId", productoId == null ? DBNull.Value : (object)productoId);
                        comando.Parameters.AddWithValue("@domIds", domIds == null ? DBNull.Value : domIds.Count == 0 ? DBNull.Value : (object)string.Join(",", domIds.ToArray()));
                        comando.Parameters.AddWithValue("@subdomIds", subdomIds == null ? DBNull.Value : subdomIds.Count == 0 ? DBNull.Value : (object)string.Join(",", subdomIds.ToArray()));
                        comando.Parameters.AddWithValue("@estObsIds", estObsIds == null ? DBNull.Value : estObsIds.Count == 0 ? DBNull.Value : (object)string.Join(",", estObsIds.ToArray()));
                        comando.Parameters.AddWithValue("@tipoTecIds", tipoTecIds == null ? DBNull.Value : tipoTecIds.Count == 0 ? DBNull.Value : (object)string.Join(",", tipoTecIds.ToArray()));
                        comando.Parameters.AddWithValue("@codigo ", codigo == null ? DBNull.Value : (object)codigo);
                        comando.Parameters.AddWithValue("@tribuCoeStr", tribuCoeStr == null ? DBNull.Value : (object)tribuCoeStr);
                        comando.Parameters.AddWithValue("@squadStr", squadStr == null ? DBNull.Value : (object)squadStr);
                        comando.Parameters.AddWithValue("@withApps", withApps);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);
                        comando.Parameters.AddWithValue("@flagAdministrador", flagAdministrador);
                        comando.Parameters.AddWithValue("@usuarioMatricula", matricula);

                        var dr = comando.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new TecnologiaG();
                                item.Id = dr.GetData<int>("Id");
                                item.TipoTecnologiaId = dr.GetData<int>("TipoTecnologiaId");
                                item.Fabricante = dr.GetData<string>("Fabricante");
                                item.Nombre = dr.GetData<string>("Nombre");
                                //item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Versiones = dr.GetData<string>("Versiones");
                                item.Descripcion = dr.GetData<string>("Descripcion");
                                item.MotivoId = dr.GetData<int>("MotivoId");
                                item.MotivoStr = dr.GetData<string>("MotivoStr");
                                item.AutomatizacionImplementadaId = dr.GetData<int>("AutomatizacionImplementadaId");
                                item.RevisionSeguridadId = dr.GetData<int>("RevisionSeguridadId");
                                item.RevisionSeguridadDescripcion = dr.GetData<string>("RevisionSeguridadDescripcion");
                                item.FechaLanzamiento = dr.GetData<DateTime?>("FechaLanzamiento");
                                item.Fuente = dr.GetData<int>("Fuente");
                                item.ComentariosFechaFin = dr.GetData<string>("ComentariosFechaFin");
                                item.SustentoMotivo = dr.GetData<string>("SustentoMotivo");
                                item.SustentoUrl = dr.GetData<string>("SustentoUrl");
                                item.CasoUso = dr.GetData<string>("CasoUso");
                                item.Aplica = dr.GetData<string>("Aplica");
                                item.CompatibilidadSO = dr.GetData<string>("CompatibilidadSO");
                                item.CompatibilidadCloud = dr.GetData<string>("CompatibilidadCloud");
                                item.Requisitos = dr.GetData<string>("Requisitos");
                                item.Existencia = dr.GetData<int>("Existencia");
                                item.Riesgo = dr.GetData<int>("Riesgo");
                                item.Facilidad = dr.GetData<int>("Facilidad");
                                item.Vulnerabilidad = dr.GetData<decimal>("Vulnerabilidad");
                                item.RoadmapOpcional = dr.GetData<string>("RoadmapOpcional");
                                item.Referencias = dr.GetData<string>("Referencias");
                                item.PlanTransConocimiento = dr.GetData<string>("PlanTransConocimiento");
                                item.EsqMonitoreo = dr.GetData<string>("EsqMonitoreo");
                                item.EsqPatchManagement = dr.GetData<string>("EsqPatchManagement");
                                item.ConfArqSeg = dr.GetData<string>("ConfArqSeg");
                                item.ConfArqTec = dr.GetData<string>("ConfArqTec");
                                item.EsqLicenciamiento = dr.GetData<string>("EsqLicenciamiento");
                                item.EquipoAprovisionamiento = dr.GetData<string>("EquipoAprovisionamiento");
                                item.Activo = dr.GetData<bool>("Activo");
                                item.Dominio = dr.GetData<string>("Dominio");
                                item.Subdominio = dr.GetData<string>("Subdominio");
                                item.Tipo = dr.GetData<string>("Tipo");
                                item.TipoProductoStr = dr.GetData<string>("TipoProductoStr");
                                item.Estado = dr.GetData<int>("Estado");
                                item.FechaAprobacion = dr.GetData<DateTime>("FechaAprobacion");
                                item.UsuarioAprobacion = dr.GetData<string>("UsuarioAprobacion");
                                item.ClaveTecnologia = dr.GetData<string>("ClaveTecnologia");
                                item.FechaFinSoporte = dr.GetData<DateTime?>("FechaFinSoporte");
                                item.FechaAcordada = dr.GetData<DateTime?>("FechaAcordada");
                                item.FechaExtendida = dr.GetData<DateTime?>("FechaExtendida");
                                item.FechaCalculoTec = dr.GetData<int>("FechaCalculoTec");
                                item.FlagSiteEstandar = dr.GetData<bool?>("FlagSiteEstandar");
                                item.FlagFechaFinSoporte = dr.GetData<bool?>("FlagFechaFinSoporte");
                                item.FlagTieneEquivalencias = dr.GetData<bool>("FlagTieneEquivalencias");
                                item.ProductoNombre = dr.GetData<string>("ProductoNombre");
                                item.TribuCoeStr = dr.GetData<string>("TribuCoeStr");
                                item.ResponsableTribuCoeStr = dr.GetData<string>("ResponsableTribuCoeStr");
                                item.SquadStr = dr.GetData<string>("SquadStr");
                                item.ResponsableSquadStr = dr.GetData<string>("ResponsableSquadStr");
                                item.Dueno = dr.GetData<string>("Dueno");
                                item.CodigoProducto = dr.GetData<string>("CodigoProducto");
                                item.GrupoSoporteRemedy = dr.GetData<string>("GrupoSoporteRemedy");
                                item.EqAdmContacto = dr.GetData<string>("EqAdmContacto");
                                item.UrlConfluenceId = dr.GetData<int?>("UrlConfluenceId");
                                item.UrlConfluence = dr.GetData<string>("UrlConfluence");
                                item.TipoFechaInterna = dr.GetData<string>("TipoFechaInterna");
                                item.EstadoId = dr.GetData<int>("EstadoId");
                                item.FechaDeprecada = dr.GetData<DateTime?>("FechaDeprecada");
                                item.TecReemplazoDepId = dr.GetData<int>("TecReemplazoDepId");
                                item.TecReemplazoDepNombre = dr.GetData<string>("TecReemplazoDepNombre");
                                //item.EstadoId = !(item.FlagFechaFinSoporte ?? false) ? (int)ETecnologiaEstado.Vigente : item.FechaCalculoTec == null ? (int)ETecnologiaEstado.Obsoleto : (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaExtendida && item.FechaExtendida.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaFinSoporte && item.FechaFinSoporte.Value > DateTime.Now) || (item.FechaCalculoTec == (int)FechaCalculoTecnologia.FechaInterna && item.FechaAcordada.Value > DateTime.Now) ? (int)ETecnologiaEstado.Vigente : (int)ETecnologiaEstado.Obsoleto;
                                item.UsuarioCreacion = dr.GetData<string>("UsuarioCreacion");
                                item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                item.UsuarioModificacion = dr.GetData<string>("UsuarioModificacion");
                                item.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");
                                item.CantRoles = dr.GetData<int>("CantRoles");
                                item.CantFunciones = dr.GetData<int>("CantFunciones");
                                item.ProductoId = dr.GetData<int>("ProductoId");
                                totalRows = dr.GetData<int>("TotalRows");
                                item.ListaExpertosStr = dr.GetData<string>("Expertos");
                                item.LineamientoBaseSeguridadId = dr.GetData<int>("LineamientoBaseSeguridadId");
                                item.LineamientoBaseSeguridad = dr.GetData<string>("LineamientoBaseSeguridad");
                                registros.Add(item);
                            }
                        }

                        //var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaG> GetTecSTD(int domId, int subdomId, string casoUso, string filtro, int estadoId, int famId, int fecId, string aplica, string codigo, string dueno, string equipo, int tipoTecId, int estObsId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllTipoEquipoyFiltro(string filtro)
        {
            try
            {

                int[] arrTipos = { 1, 2, 4, 6 };

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from te in ctx.TipoEquipo
                                       where te.FlagActivo && arrTipos.Contains(te.TipoEquipoId)
                                       select new CustomAutocomplete()
                                       {
                                           Id = te.TipoEquipoId.ToString(),
                                           Descripcion = te.Nombre
                                       }).OrderBy(z => z.Descripcion).ToList();


                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTipoEquipoyFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllTipoEquipoyFiltro(string filtro)"
                    , new object[] { null });
            }
        }

    }
}
#endregion